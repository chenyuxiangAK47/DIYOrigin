using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;

namespace OriginSystemMod
{
    /// <summary>
    /// Harmony Hook 用于追踪所有与王国、家族、英雄状态相关的 Action 调用
    /// 用于调试和监控游戏状态变化
    /// </summary>
    [HarmonyPatch]
    internal static class KingdomActionHooks
    {
        /// <summary>
        /// 获取所有需要 Hook 的方法（版本无关，抓所有重载）
        /// </summary>
        static IEnumerable<MethodBase> TargetMethods()
        {
            // 抓所有 ApplyByJoinToKingdom 重载（避免版本差异导致 patch 不生效）
            var joinMethods = typeof(ChangeKingdomAction)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(m => m.Name == "ApplyByJoinToKingdom")
                .Cast<MethodBase>();
            
            // 抓所有 ApplyByJoinFactionAsMercenary 重载
            var mercenaryMethods = typeof(ChangeKingdomAction)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(m => m.Name == "ApplyByJoinFactionAsMercenary")
                .Cast<MethodBase>();
            
            // 抓所有 ApplyByLeaveKingdomAsMercenary 重载
            var leaveMethods = typeof(ChangeKingdomAction)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(m => m.Name == "ApplyByLeaveKingdomAsMercenary")
                .Cast<MethodBase>();
            
            var allMethods = joinMethods
                .Concat(mercenaryMethods)
                .Concat(leaveMethods);
            
            // 尝试添加 Kingdom.AddClan / RemoveClan（关键：归属变化）
            try
            {
                var kingdomType = typeof(Kingdom);
                var addClanMethod = kingdomType.GetMethod("AddClan", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                if (addClanMethod != null)
                    allMethods = allMethods.Append(addClanMethod);
                
                var removeClanMethod = kingdomType.GetMethod("RemoveClan", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                if (removeClanMethod != null)
                    allMethods = allMethods.Append(removeClanMethod);
            }
            catch { /* 忽略 */ }
            
            // 尝试添加 Clan.set_Kingdom（如果有 setter）
            try
            {
                var clanType = typeof(Clan);
                var kingdomProperty = clanType.GetProperty("Kingdom", BindingFlags.Public | BindingFlags.Instance);
                if (kingdomProperty != null && kingdomProperty.SetMethod != null)
                    allMethods = allMethods.Append(kingdomProperty.SetMethod);
            }
            catch { /* 忽略 */ }
            
            // 尝试添加任何名字包含 ChangeKingdom / OnClan...Kingdom 的方法
            try
            {
                var campaignEventType = typeof(CampaignEvents);
                var allEventMethods = campaignEventType.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic)
                    .Where(m => m.Name.Contains("ChangeKingdom") || m.Name.Contains("OnClan") && m.Name.Contains("Kingdom"))
                    .Cast<MethodBase>();
                // 注意：CampaignEvents 的方法通常不是直接调用的，但我们可以尝试
            }
            catch { /* 忽略 */ }
            
            // 尝试添加 ChangeClanTierAction（如果存在）
            try
            {
                var tierActionType = typeof(ChangeKingdomAction).Assembly.GetType("TaleWorlds.CampaignSystem.Actions.ChangeClanTierAction");
                if (tierActionType != null)
                {
                    var tierMethods = tierActionType
                        .GetMethods(BindingFlags.Public | BindingFlags.Static)
                        .Where(m => m.Name.Contains("Tier") || m.Name.Contains("Renown"))
                        .Cast<MethodBase>();
                    allMethods = allMethods.Concat(tierMethods);
                }
            }
            catch { /* 忽略，类型可能不存在 */ }
            
            // 添加 ChangeRelationAction（确定存在）
            var relationMethods = typeof(ChangeRelationAction)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(m => m.Name.Contains("Relation") || m.Name.Contains("Player"))
                .Cast<MethodBase>();
            allMethods = allMethods.Concat(relationMethods);
            
            // 添加 GiveGoldAction（确定存在）
            var goldMethods = typeof(GiveGoldAction)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(m => m.Name.Contains("Gold") || m.Name.Contains("Between"))
                .Cast<MethodBase>();
            allMethods = allMethods.Concat(goldMethods);
            
            return allMethods;
        }

        /// <summary>
        /// Prefix: 在方法调用前记录状态
        /// </summary>
        static void Prefix(MethodBase __originalMethod, object[] __args)
        {
            var clan = __args.FirstOrDefault(a => a is Clan) as Clan;
            var kingdom = __args.FirstOrDefault(a => a is Kingdom) as Kingdom;

            // 只追踪玩家家族相关的调用
            if (clan != Clan.PlayerClan && kingdom?.Clans?.Contains(Clan.PlayerClan) != true)
                return;

            OriginLog.Info($"[HOOK] {__originalMethod.Name} - clan={clan?.StringId ?? "null"}, kingdom={kingdom?.StringId ?? "null"}");
            
            // 使用统一的状态快照工具
            PlayerStateDumper.DumpPlayerState($"HOOK-PREFIX-{__originalMethod.Name}", clan ?? Clan.PlayerClan, kingdom);
            
            // 使用带行号的 StackTrace
            PlayerStateDumper.DumpStackTrace($"HOOK-PREFIX-{__originalMethod.Name}");
        }

        /// <summary>
        /// Postfix: 在方法调用后记录状态
        /// </summary>
        static void Postfix(MethodBase __originalMethod, object[] __args)
        {
            var clan = __args.FirstOrDefault(a => a is Clan) as Clan;
            var kingdom = __args.FirstOrDefault(a => a is Kingdom) as Kingdom;

            // 只追踪玩家家族相关的调用
            if (clan != Clan.PlayerClan && kingdom?.Clans?.Contains(Clan.PlayerClan) != true)
                return;

            // 使用统一的状态快照工具
            PlayerStateDumper.DumpPlayerState($"HOOK-POSTFIX-{__originalMethod.Name}", clan ?? Clan.PlayerClan, kingdom);
            
            // 如果是 JoinToKingdom，特别检查三个判据和 IsVassalLike
            if (__originalMethod.Name == "ApplyByJoinToKingdom" && (clan == Clan.PlayerClan || kingdom?.Clans?.Contains(Clan.PlayerClan) == true))
            {
                var playerClan = clan ?? Clan.PlayerClan;
                bool condition1 = playerClan.Kingdom == kingdom;
                bool condition2 = !playerClan.IsUnderMercenaryService;
                bool condition3 = kingdom?.Clans?.Contains(playerClan) ?? false;
                bool isVassalLike = PlayerStateDumper.CalculateIsVassalLike();
                
                OriginLog.Info($"[HOOK] ApplyByJoinToKingdom POSTFIX - Criteria check:");
                OriginLog.Info($"[HOOK]   Criteria1: Clan.Kingdom == kingdom = {condition1}");
                OriginLog.Info($"[HOOK]   Criteria2: !IsUnderMercenaryService = {condition2}");
                OriginLog.Info($"[HOOK]   Criteria3: kingdom.Clans.Contains(clan) = {condition3}");
                OriginLog.Info($"[HOOK]   IsVassalLike = {isVassalLike} (Kingdom!=null && !Mercenary && Occupation==Lord)");
                
                if (condition1 && condition2 && condition3 && isVassalLike)
                {
                    OriginLog.Info("[HOOK] ApplyByJoinToKingdom: SUCCESS - All criteria met, join landed");
                }
                else
                {
                    OriginLog.Warning("[HOOK] ApplyByJoinToKingdom: FAILED - Criteria not met or IsVassalLike=false");
                }
            }
        }

    }

    /// <summary>
    /// Hook 用于追踪 Clan.IsNoble 属性的设置
    /// 用于"抓凶手"：看是谁把 IsNoble 改回 false
    /// </summary>
    [HarmonyPatch(typeof(Clan), "set_IsNoble")]
    internal static class Hook_Clan_SetIsNoble
    {
        static void Prefix(Clan __instance, bool value)
        {
            // 只追踪玩家家族
            if (__instance != Clan.PlayerClan) return;

            // 在 Prefix 中，IsNoble 还是旧值，value 是新值
            var beforeValue = __instance.IsNoble;
            OriginLog.Warning($"[HOOK] set_IsNoble - PlayerClan IsNoble: {beforeValue} -> {value}");
            
            // 如果是从 true 变成 false，特别警告
            if (beforeValue && !value)
            {
                OriginLog.Warning("[HOOK] set_IsNoble - WARNING: IsNoble changed from true to false!");
            }
            
            // 使用带行号的 StackTrace
            PlayerStateDumper.DumpStackTrace("HOOK-set_IsNoble");
            
            // 使用统一的状态快照工具
            PlayerStateDumper.DumpPlayerState("HOOK-set_IsNoble", __instance, null);
        }
    }

    /// <summary>
    /// Hook 用于追踪 ApplyByJoinFactionAsMercenary 和 LeaveMercenary 相关调用
    /// 用于检测是否被错误地设置为雇佣兵或退出雇佣兵服务
    /// </summary>
    [HarmonyPatch]
    internal static class MercenaryActionHooks
    {
        static IEnumerable<MethodBase> TargetMethods()
        {
            var mercenaryMethods = typeof(ChangeKingdomAction)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(m => m.Name == "ApplyByJoinFactionAsMercenary" || 
                           m.Name == "ApplyByLeaveKingdomAsMercenary")
                .Cast<MethodBase>();
            
            return mercenaryMethods;
        }

        static void Prefix(MethodBase __originalMethod, object[] __args)
        {
            var clan = __args.FirstOrDefault(a => a is Clan) as Clan;
            if (clan != Clan.PlayerClan) return;

            OriginLog.Warning($"[HOOK] {__originalMethod.Name} - PlayerClan called!");
            
            // 使用带行号的 StackTrace
            PlayerStateDumper.DumpStackTrace($"HOOK-PREFIX-{__originalMethod.Name}");
            
            // 使用统一的状态快照工具
            PlayerStateDumper.DumpPlayerState($"HOOK-PREFIX-{__originalMethod.Name}", clan, null);
        }

        static void Postfix(MethodBase __originalMethod, object[] __args)
        {
            var clan = __args.FirstOrDefault(a => a is Clan) as Clan;
            if (clan != Clan.PlayerClan) return;

            // 使用统一的状态快照工具
            PlayerStateDumper.DumpPlayerState($"HOOK-POSTFIX-{__originalMethod.Name}", clan, null);
        }
    }

    /// <summary>
    /// Hook 用于追踪 Hero.SetNewOccupation 调用
    /// 用于记录 Occupation 的变化（这是判断"贵族/封臣"的关键字段）
    /// </summary>
    [HarmonyPatch]
    internal static class HeroOccupationHooks
    {
        static IEnumerable<MethodBase> TargetMethods()
        {
            var heroType = typeof(Hero);
            var methods = new List<MethodBase>();

            // 尝试找到 SetNewOccupation 方法（可能有多个重载）
            var setOccupationMethods = heroType
                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(m => m.Name == "SetNewOccupation" || m.Name == "set_Occupation")
                .Cast<MethodBase>();

            methods.AddRange(setOccupationMethods);

            return methods;
        }

        static void Prefix(MethodBase __originalMethod, object[] __args, Hero __instance)
        {
            // 只追踪玩家主角
            if (__instance != Hero.MainHero) return;

            var occupationParam = __args.FirstOrDefault();
            var occupationStr = occupationParam?.ToString() ?? "null";

            OriginLog.Info($"[HOOK] {__originalMethod.Name} - Hero.MainHero called!");
            OriginLog.Info($"[HOOK] {__originalMethod.Name} - Parameter: Occupation = {occupationStr}");
            
            // 使用统一的状态快照工具
            PlayerStateDumper.DumpPlayerState($"HOOK-PREFIX-{__originalMethod.Name}", Clan.PlayerClan, null);
            
            // 使用带行号的 StackTrace
            PlayerStateDumper.DumpStackTrace($"HOOK-PREFIX-{__originalMethod.Name}");
        }

        static void Postfix(MethodBase __originalMethod, object[] __args, Hero __instance)
        {
            // 只追踪玩家主角
            if (__instance != Hero.MainHero) return;

            // 使用统一的状态快照工具
            PlayerStateDumper.DumpPlayerState($"HOOK-POSTFIX-{__originalMethod.Name}", Clan.PlayerClan, null);
        }
    }

    /// <summary>
    /// Hook 用于追踪 Clan.StartMercenaryService 和 EndMercenaryService 调用
    /// 用于记录雇佣兵服务的开始和结束
    /// </summary>
    [HarmonyPatch]
    internal static class ClanMercenaryServiceHooks
    {
        static IEnumerable<MethodBase> TargetMethods()
        {
            var clanType = typeof(Clan);
            var methods = new List<MethodBase>();

            // 尝试找到 StartMercenaryService 和 EndMercenaryService 方法
            var startMethod = clanType.GetMethod("StartMercenaryService", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            if (startMethod != null)
                methods.Add(startMethod);

            var endMethod = clanType.GetMethod("EndMercenaryService", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            if (endMethod != null)
                methods.Add(endMethod);

            // 也尝试找其他可能的名称
            var allMethods = clanType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(m => m.Name.Contains("Mercenary") && (m.Name.Contains("Start") || m.Name.Contains("End") || m.Name.Contains("Begin") || m.Name.Contains("Stop")))
                .Cast<MethodBase>();
            
            methods.AddRange(allMethods);

            return methods.Distinct();
        }

        static void Prefix(MethodBase __originalMethod, object[] __args, Clan __instance)
        {
            // 只追踪玩家家族
            if (__instance != Clan.PlayerClan) return;

            OriginLog.Warning($"[HOOK] {__originalMethod.Name} - PlayerClan called!");
            
            // 使用带行号的 StackTrace
            PlayerStateDumper.DumpStackTrace($"HOOK-PREFIX-{__originalMethod.Name}");
            
            // 使用统一的状态快照工具
            PlayerStateDumper.DumpPlayerState($"HOOK-PREFIX-{__originalMethod.Name}", __instance, null);
        }

        static void Postfix(MethodBase __originalMethod, object[] __args, Clan __instance)
        {
            // 只追踪玩家家族
            if (__instance != Clan.PlayerClan) return;

            // 使用统一的状态快照工具
            PlayerStateDumper.DumpPlayerState($"HOOK-POSTFIX-{__originalMethod.Name}", __instance, null);
        }
    }
}
