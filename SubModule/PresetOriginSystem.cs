using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace OriginSystemMod
{
    /// <summary>
    /// 预设出身系统
    /// 完整实现所有10个库塞特预设出身的应用逻辑
    /// 根据节点选择动态应用效果（兵力、技能、关系、金币等）
    /// </summary>
    public static class PresetOriginSystem
    {
        /// <summary>
        /// 应用预设出身效果
        /// </summary>
        public static void ApplyPresetOrigin(string originId)
        {
            // 自证日志：打印"我确实进来了"
            OriginLog.Info($"[ApplyPresetOrigin] originId={originId}");
            
            try
            {
                var hero = Hero.MainHero;
                if (hero == null)
                {
                    Debug.Print("[OriginSystem] MainHero 为空，无法应用预设出身", 0, Debug.DebugColor.Red);
                    return;
                }

                var clan = hero.Clan;
                if (clan == null)
                {
                    Debug.Print("[OriginSystem] PlayerClan 为空，无法应用预设出身", 0, Debug.DebugColor.Red);
                    return;
                }

                var party = MobileParty.MainParty;
                if (party == null)
                {
                    Debug.Print("[OriginSystem] MainParty 为空，无法应用预设出身", 0, Debug.DebugColor.Red);
                    return;
                }

                Debug.Print($"[OriginSystem] 开始应用预设出身: {originId}", 0, Debug.DebugColor.Green);

                // 根据出身ID应用基础效果和节点选择效果
                // 支持完整ID格式（khuzait_xxx）和简化格式（xxx）
                string normalizedOriginId = originId;
                if (originId.StartsWith("khuzait_"))
                {
                    normalizedOriginId = originId.Substring("khuzait_".Length);
                }
                
                switch (normalizedOriginId)
                {
                    case "rebel_chief":
                        ApplyRebelChiefOrigin(hero, clan, party);
                        break;
                    case "minor_noble":
                        ApplyMinorNobleOrigin(hero, clan, party);
                        break;
                    case "migrant_chief":
                        ApplyMigrantChiefOrigin(hero, clan, party);
                        break;
                    case "army_deserter":
                        ApplyArmyDeserterOrigin(hero, clan, party);
                        break;
                    case "trade_protector":
                        ApplyTradeProtectorOrigin(hero, clan, party);
                        break;
                    case "wandering_prince":
                        ApplyWanderingPrinceOrigin(hero, clan, party);
                        break;
                    case "khans_mercenary":
                        ApplyKhansMercenaryOrigin(hero, clan, party);
                        break;
                    case "slave_escape":
                        ApplySlaveEscapeOrigin(hero, clan, party);
                        break;
                    case "free_cossack":
                        ApplyFreeCossackOrigin(hero, clan, party);
                        break;
                    case "old_guard_avenger":
                        ApplyOldGuardAvengerOrigin(hero, clan, party);
                        break;
                    default:
                        Debug.Print($"[OriginSystem] 未知的预设出身 ID: {originId} (normalized: {normalizedOriginId})", 0, Debug.DebugColor.Yellow);
                        break;
                }

                Debug.Print($"[OriginSystem] 预设出身应用完成: {originId}", 0, Debug.DebugColor.Green);
            }
            catch (Exception ex)
            {
                Debug.Print($"[OriginSystem] 应用预设出身失败 ({originId}): {ex.Message}", 0, Debug.DebugColor.Red);
                Debug.Print($"[OriginSystem] StackTrace: {ex.StackTrace}", 0, Debug.DebugColor.Red);
            }
        }

        #region 1. 草原叛酋 (rebel_chief)

        private static void ApplyRebelChiefOrigin(Hero hero, Clan clan, MobileParty party)
        {
            SetClanTier(clan, 1);
            
            var khuzaitKingdom = FindKingdom("kingdom_khuzait");
            if (khuzaitKingdom != null)
            {
                ChangeRelationAction.ApplyPlayerRelation(khuzaitKingdom.Leader, -50);
            }

            var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
            
            // Node1: 为何被定为叛酋
            ApplyRebelChiefNode1(hero, selectedNodes);
            
            // Node2: 叛乱战帮组成（添加兵力）
            ApplyRebelChiefNode2(party, selectedNodes);
            
            // Node3: 叛酋象征
            ApplyRebelChiefNode3(hero, party, selectedNodes);
            
            // Node4: 第一步要做什么
            ApplyRebelChiefNode4(hero, selectedNodes);

            AddGold(hero, 3000); // 基础金币：低档
            
            // 设置出生位置：库塞特村子（避免城市会卡住）
            if (MobileParty.MainParty != null && Campaign.Current != null)
            {
                SetPresetOriginStartingLocation(MobileParty.MainParty, "khuzait");
            }
        }

        private static void ApplyRebelChiefNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_rebel_reason")) return;
            
            var reason = nodes["khz_node_rebel_reason"];
            var khuzaitKingdom = FindKingdom("kingdom_khuzait");
            
            switch (reason)
            {
                case "refuse_tribute":
                    // 拒绝纳贡：领导力+45，管理+30，名望+1，与库塞特可汗关系-70
                    AddSkill(hero, "leadership", 45);
                    AddSkill(hero, "steward", 30);
                    GainRenown(hero, 1);
                    if (khuzaitKingdom != null)
                    {
                        ChangeRelationAction.ApplyPlayerRelation(khuzaitKingdom.Leader, -70);
                    }
                    break;
                case "raid_caravan":
                    // 劫掠商队被点名：劫掠+45，侦察+30，仁慈-1，名望+1，金币+3000（低），与商人关系-70，与库塞特可汗关系-70
                    AddSkill(hero, "roguery", 45);
                    AddSkill(hero, "scouting", 30);
                    AddTrait(hero, "Mercy", -1);
                    GainRenown(hero, 1);
                    AddGold(hero, 3000);
                    if (khuzaitKingdom != null)
                    {
                        ChangeRelationAction.ApplyPlayerRelation(khuzaitKingdom.Leader, -70);
                    }
                    // 与商人关系-70（通过降低与所有商人的关系）
                    foreach (var hero2 in OriginSystemHelper.GetAllHeroesSafe().Where(h => h.IsMerchant))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(hero2, -70);
                    }
                    break;
                case "refuse_service":
                    // 拒绝服兵役/拒绝出征：领导力+30，战术+30，与库塞特可汗关系-70
                    AddSkill(hero, "leadership", 30);
                    AddSkill(hero, "tactics", 30);
                    if (khuzaitKingdom != null)
                    {
                        ChangeRelationAction.ApplyPlayerRelation(khuzaitKingdom.Leader, -70);
                    }
                    break;
                case "protect_tribe":
                    // 为保护族人顶撞汗廷：领导力+30，魅力+20，勇敢+1，名望+1，士气+5，与库塞特可汗关系-30
                    AddSkill(hero, "leadership", 30);
                    AddSkill(hero, "charm", 20);
                    AddTrait(hero, "Valor", 1);
                    GainRenown(hero, 1);
                    if (MobileParty.MainParty != null)
                    {
                        MobileParty.MainParty.RecentEventsMorale += 5;
                    }
                    if (khuzaitKingdom != null)
                    {
                        ChangeRelationAction.ApplyPlayerRelation(khuzaitKingdom.Leader, -30);
                    }
                    break;
            }
        }

        private static void ApplyRebelChiefNode2(MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_rebel_warband_makeup")) return;
            
            var warband = nodes["khz_node_rebel_warband_makeup"];
            switch (warband)
            {
                case "nomads":
                    // 以游牧战士为主：额外兵力：游牧战士×16，部落青年×6，食物+20（中）
                    AddTroops(party, "khuzait_nomad", 16);
                    AddTroops(party, "khuzait_tribal_youth", 6);
                    AddFood(party, 20);
                    break;
                case "light_cav":
                    // 以轻骑/骑射为主：额外兵力：骑射手×10，轻骑兵×6，马匹+2（中），食物+10（低）
                    AddTroops(party, "khuzait_horse_archer", 10);
                    AddTroops(party, "khuzait_light_cavalry", 6);
                    AddMounts(party, 2);
                    AddFood(party, 10);
                    break;
                case "mixed":
                    // 混编（更稳）：额外兵力：游牧战士×10，骑射手×6，轻骑兵×4，食物+20（中）
                    AddTroops(party, "khuzait_nomad", 10);
                    AddTroops(party, "khuzait_horse_archer", 6);
                    AddTroops(party, "khuzait_light_cavalry", 4);
                    AddFood(party, 20);
                    break;
            }
        }

        private static void ApplyRebelChiefNode3(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_rebel_symbol")) return;
            
            var symbol = nodes["khz_node_rebel_symbol"];
            switch (symbol)
            {
                case "banner":
                    // 部族的传世战旗（顶级宝贝）：名望+2，士气+6，领导力+45
                    GainRenown(hero, 2);
                    if (party != null) party.RecentEventsMorale += 6;
                    AddSkill(hero, "leadership", 45);
                    break;
                case "treasure":
                    // 部族积累的巨额财富（大）：金币+15000（大），马匹+3（大）
                    AddGold(hero, 15000);
                    AddMounts(party, 3);
                    break;
                case "seal":
                    // 部族传承的黄金印记（顶级宝贝）：名望+2，魅力+45
                    GainRenown(hero, 2);
                    AddSkill(hero, "charm", 45);
                    break;
                case "people":
                    // 忠诚的伙伴：鹰（the Hawk）：伙伴加入，额外兵力：游牧战士×6，士气+3
                    AddCompanion("spc_wanderer_khuzait_1");
                    AddTroops(party, "khuzait_nomad", 6);
                    if (party != null) party.RecentEventsMorale += 3;
                    break;
            }
        }

        private static void ApplyRebelChiefNode4(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_rebel_first_move")) return;
            
            var move = nodes["khz_node_rebel_first_move"];
            var khuzaitKingdom = FindKingdom("kingdom_khuzait");
            
            switch (move)
            {
                case "hide":
                    // 隐入草原，先活下来：食物+20（中），侦察+45
                    AddFood(MobileParty.MainParty, 20);
                    AddSkill(hero, "scouting", 45);
                    break;
                case "raid":
                    // 以战养战，先打一票：金币+3000（低），名望+1
                    AddGold(hero, 3000);
                    GainRenown(hero, 1);
                    break;
                case "ally":
                    // 找人结盟，换一口气：魅力+30，与精英氏族关系+20
                    AddSkill(hero, "charm", 30);
                    if (khuzaitKingdom != null)
                    {
                        foreach (var clan in khuzaitKingdom.Clans.Where(c => c.Tier >= 3 && c.Leader != null && c.Leader != khuzaitKingdom.Leader))
                        {
                            ChangeRelationAction.ApplyPlayerRelation(clan.Leader, 20);
                        }
                    }
                    break;
            }
        }

        #endregion

        #region 2. 汗廷旁支贵族 (minor_noble)

        private static void ApplyMinorNobleOrigin(Hero hero, Clan clan, MobileParty party)
        {
            OriginLog.Info($"[MinorNoble] Apply enter: clan.StringId={clan?.StringId ?? "null"}, clan.Name={clan?.Name?.ToString() ?? "null"}");
            OriginLog.Info($"[MinorNoble] clan==PlayerClan: {ReferenceEquals(clan, Clan.PlayerClan)}");
            OriginLog.Info($"[MinorNoble] PlayerClan.StringId={Clan.PlayerClan?.StringId ?? "null"}");
            
            SetClanTier(clan, 4); // 4级家族（贵族）
            
            // 关键：设置名望（Renown）- 名望是加入成为贵族的条件之一
            // 按照 ChatGPT 建议：Tier=4 需要更高的 Renown（至少 900+），避免被系统"纠正"
            // 设置为 1000+ 以确保不会被系统降级
            GainRenown(hero, 1000);
            OriginLog.Info($"[MinorNoble] 已设置名望: +1000 (当前名望: {hero.Clan?.Renown ?? 0})");
            
            // 按照 ChatGPT 建议：在角色创建阶段只设置标记，延迟到 OnSessionLaunched 执行真正的加入王国逻辑
            // 这样可以避免被后续的角色创建流程覆盖
            OriginSystemHelper.PendingMinorNobleJoinKhuzait = true;
            OriginLog.Info("[MinorNoble] 已设置标记 PendingMinorNobleJoinKhuzait = true，将在 OnSessionLaunched 中执行加入王国逻辑");

            var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
            
            // Node1: 为何被边缘化
            ApplyMinorNobleNode1(hero, selectedNodes);
            
            // Node2: 风格（技能+装备+金币）
            ApplyMinorNobleNode2(hero, selectedNodes);
            
            // Node3: 随骑（兵力）
            ApplyMinorNobleNode3(party, selectedNodes);
            
            // Node4: 政治立场（关系）
            ApplyMinorNobleNode4(hero, selectedNodes);

            AddGold(hero, 15000);
            
            // 设置出生位置：库塞特村子（避免城市会卡住）
            if (MobileParty.MainParty != null && Campaign.Current != null)
            {
                SetPresetOriginStartingLocation(MobileParty.MainParty, "khuzait");
            }
            
            // 关键：使用 SetClanNoble 方法设置贵族身份（更完善的方法，会尝试多种方式）
            SetClanNoble(clan, true);
            OriginLog.Info($"[MinorNoble] 已调用 SetClanNoble(clan, true)，当前 IsNoble={clan.IsNoble}");
            
            // 最后再次确保 IsNoble 设置成功（防止被覆盖）
            EnsurePlayerClanIsNoble();
        }
        
        /// <summary>
        /// 加入库塞特王国为封臣（vassal/贵族）
        /// 按照 ChatGPT 建议：使用编译期调用，不要用反射
        /// </summary>
        public static void JoinKhuzaitAsNoble()
        {
            if (Clan.PlayerClan == null)
            {
                OriginLog.Warning("[JoinKhuzaitAsNoble] Clan.PlayerClan 为 null");
                return;
            }

            // 记录版本信息（用于确认 API 版本）
            try
            {
                var assemblyVersion = typeof(ChangeKingdomAction).Assembly.GetName().Version;
                OriginLog.Info($"[JoinKhuzaitAsNoble] ChangeKingdomAction.Assembly.Version={assemblyVersion}");
            }
            catch (Exception ex)
            {
                OriginLog.Warning($"[JoinKhuzaitAsNoble] 获取版本信息失败: {ex.Message}");
            }

            var khuzaitKingdom = FindKingdom("kingdom_khuzait") ?? FindKingdom("khuzait");
            if (khuzaitKingdom == null)
            {
                OriginLog.Warning("[JoinKhuzaitAsNoble] 未找到库塞特王国");
                if (Campaign.Current?.Kingdoms != null)
                {
                    OriginLog.Info("[JoinKhuzaitAsNoble] 可用王国列表：");
                    foreach (var k in Campaign.Current.Kingdoms)
                    {
                        OriginLog.Info($"  - Kingdom: {k.Name} (StringId={k.StringId})");
                    }
                }
                return;
            }

            // 记录加入前的状态（关键字段：Clan + Hero）
            OriginLog.Info($"[JoinKhuzaitAsNoble] BEFORE:");
            OriginLog.Info($"[JoinKhuzaitAsNoble]   - clan.Kingdom?.StringId = {(Clan.PlayerClan.Kingdom?.StringId ?? "null")}");
            OriginLog.Info($"[JoinKhuzaitAsNoble]   - clan.IsNoble = {Clan.PlayerClan.IsNoble}");
            OriginLog.Info($"[JoinKhuzaitAsNoble]   - clan.IsMinorFaction = {Clan.PlayerClan.IsMinorFaction}");
            OriginLog.Info($"[JoinKhuzaitAsNoble]   - clan.IsClanTypeMercenary = {Clan.PlayerClan.IsClanTypeMercenary}");
            OriginLog.Info($"[JoinKhuzaitAsNoble]   - clan.IsUnderMercenaryService = {Clan.PlayerClan.IsUnderMercenaryService}");
            OriginLog.Info($"[JoinKhuzaitAsNoble]   - clan.Tier = {Clan.PlayerClan.Tier}");
            OriginLog.Info($"[JoinKhuzaitAsNoble]   - clan.Renown = {Clan.PlayerClan.Renown}");
            OriginLog.Info($"[JoinKhuzaitAsNoble]   - Hero.MainHero.IsLord = {(Hero.MainHero?.IsLord ?? false)}");
            OriginLog.Info($"[JoinKhuzaitAsNoble]   - Hero.MainHero.Occupation = {(Hero.MainHero != null ? Hero.MainHero.Occupation.ToString() : "null")}");
            OriginLog.Info($"[JoinKhuzaitAsNoble]   - target Kingdom = {khuzaitKingdom.StringId}");

            try
            {
                // 关键：在加入王国前先强制把主角设为 Lord
                // 这是判断"贵族/封臣"的真正判据，而不是 Clan.IsNoble
                if (Hero.MainHero != null && !Hero.MainHero.IsLord)
                {
                    OriginLog.Info("[JoinKhuzaitAsNoble] 主角不是 Lord，尝试设置 Occupation = Lord");
                    try
                    {
                        // 尝试直接调用 SetNewOccupation
                        try
                        {
                            Hero.MainHero.SetNewOccupation(Occupation.Lord);
                            OriginLog.Info($"[JoinKhuzaitAsNoble] 直接调用成功: IsLord={Hero.MainHero.IsLord}, Occupation={Hero.MainHero.Occupation}");
                        }
                        catch (MissingMethodException)
                        {
                            // 如果直接调用失败，尝试使用反射
                            OriginLog.Info("[JoinKhuzaitAsNoble] 直接调用失败，尝试使用反射");
                            var heroType = typeof(Hero);
                            var setOccupationMethod = heroType.GetMethod("SetNewOccupation", BindingFlags.Public | BindingFlags.Instance);
                            if (setOccupationMethod != null)
                            {
                                // 尝试获取 Occupation 枚举
                                var occupationType = typeof(Hero).Assembly.GetType("TaleWorlds.Core.Occupation") 
                                                    ?? typeof(Hero).Assembly.GetTypes().FirstOrDefault(t => t.Name == "Occupation" && t.IsEnum);
                                if (occupationType != null)
                                {
                                    var lordValue = Enum.Parse(occupationType, "Lord");
                                    setOccupationMethod.Invoke(Hero.MainHero, new object[] { lordValue });
                                    OriginLog.Info($"[JoinKhuzaitAsNoble] 反射调用成功: IsLord={Hero.MainHero.IsLord}, Occupation={Hero.MainHero.Occupation}");
                                }
                                else
                                {
                                    OriginLog.Warning("[JoinKhuzaitAsNoble] 未找到 Occupation 枚举类型");
                                }
                            }
                            else
                            {
                                OriginLog.Warning("[JoinKhuzaitAsNoble] 未找到 SetNewOccupation 方法，可能该方法不存在或已改名");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        OriginLog.Error($"[JoinKhuzaitAsNoble] 设置主角为 Lord 失败: {ex.Message}");
                        OriginLog.Error($"[JoinKhuzaitAsNoble] StackTrace: {ex.StackTrace}");
                        if (ex.InnerException != null)
                        {
                            OriginLog.Error($"[JoinKhuzaitAsNoble] InnerException: {ex.InnerException.Message}");
                        }
                    }
                }
                else if (Hero.MainHero != null)
                {
                    OriginLog.Info($"[JoinKhuzaitAsNoble] 主角已经是 Lord: IsLord={Hero.MainHero.IsLord}, Occupation={Hero.MainHero.Occupation}");
                }

                // 如果之前被当成雇佣兵进过某国，先解除雇佣兵（保险）
                if (Clan.PlayerClan.IsUnderMercenaryService)
                {
                    OriginLog.Info("[JoinKhuzaitAsNoble] 检测到雇佣兵状态，先退出雇佣兵服务");
                    try
                    {
                        // 使用编译期调用，不要用反射
                        ChangeKingdomAction.ApplyByLeaveKingdomAsMercenary(Clan.PlayerClan, false);
                        OriginLog.Info("[JoinKhuzaitAsNoble] 已退出雇佣兵服务");
                    }
                    catch (Exception ex)
                    {
                        OriginLog.Error($"[JoinKhuzaitAsNoble] 退出雇佣兵失败: {ex.Message}");
                        OriginLog.Error($"[JoinKhuzaitAsNoble] StackTrace: {ex.StackTrace}");
                    }
                }

                // 如果已经在库塞特王国中，检查状态
                if (Clan.PlayerClan.Kingdom == khuzaitKingdom)
                {
                    OriginLog.Info($"[JoinKhuzaitAsNoble] 已经在库塞特王国中，当前状态:");
                    OriginLog.Info($"[JoinKhuzaitAsNoble]   - clan.Kingdom?.StringId = {(Clan.PlayerClan.Kingdom?.StringId ?? "null")}");
                    OriginLog.Info($"[JoinKhuzaitAsNoble]   - clan.IsNoble = {Clan.PlayerClan.IsNoble}");
                    OriginLog.Info($"[JoinKhuzaitAsNoble]   - clan.IsUnderMercenaryService = {Clan.PlayerClan.IsUnderMercenaryService}");
                    OriginLog.Info($"[JoinKhuzaitAsNoble]   - Hero.MainHero.IsLord = {(Hero.MainHero?.IsLord ?? false)}");
                    OriginLog.Info($"[JoinKhuzaitAsNoble]   - Hero.MainHero.Occupation = {(Hero.MainHero != null ? Hero.MainHero.Occupation.ToString() : "null")}");
                    
                    // 判断是否是封臣（真正的判据：Kingdom != null && !IsUnderMercenaryService）
                    bool isVassalAlready = Clan.PlayerClan.Kingdom == khuzaitKingdom && !Clan.PlayerClan.IsUnderMercenaryService;
                    OriginLog.Info($"[JoinKhuzaitAsNoble]   判断封臣身份: isVassal = {isVassalAlready}");
                    
                    // 确保主角是 Lord
                    if (Hero.MainHero != null && !Hero.MainHero.IsLord)
                    {
                        OriginLog.Info("[JoinKhuzaitAsNoble] 已经在王国中但主角不是 Lord，尝试设置 Occupation = Lord");
                        try
                        {
                            try
                            {
                                Hero.MainHero.SetNewOccupation(Occupation.Lord);
                                OriginLog.Info($"[JoinKhuzaitAsNoble] 直接调用成功: IsLord={Hero.MainHero.IsLord}");
                            }
                            catch (MissingMethodException)
                            {
                                var heroType = typeof(Hero);
                                var setOccupationMethod = heroType.GetMethod("SetNewOccupation", BindingFlags.Public | BindingFlags.Instance);
                                if (setOccupationMethod != null)
                                {
                                    var occupationType = typeof(Hero).Assembly.GetType("TaleWorlds.Core.Occupation") 
                                                        ?? typeof(Hero).Assembly.GetTypes().FirstOrDefault(t => t.Name == "Occupation" && t.IsEnum);
                                    if (occupationType != null)
                                    {
                                        var lordValue = Enum.Parse(occupationType, "Lord");
                                        setOccupationMethod.Invoke(Hero.MainHero, new object[] { lordValue });
                                        OriginLog.Info($"[JoinKhuzaitAsNoble] 反射调用成功: IsLord={Hero.MainHero.IsLord}");
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            OriginLog.Error($"[JoinKhuzaitAsNoble] 设置主角为 Lord 失败: {ex.Message}");
                        }
                    }
                    
                    // 如果已经是封臣，确保 IsNoble = true（作为补充标记）
                    if (isVassalAlready && !Clan.PlayerClan.IsNoble)
                    {
                        OriginLog.Info("[JoinKhuzaitAsNoble] 已经是封臣但 IsNoble=false，设置 IsNoble = true");
                        Clan.PlayerClan.IsNoble = true;
                    }
                    return;
                }

                // 关键：使用编译期调用 ApplyByJoinToKingdom（4个参数，后两个可选）
                // 按照 ChatGPT 建议：不要用反射，直接编译期调用，让 C# 自动补默认参数
                OriginLog.Info("[JoinKhuzaitAsNoble] 调用 ChangeKingdomAction.ApplyByJoinToKingdom(Clan, Kingdom, CampaignTime.Now, false)");
                ChangeKingdomAction.ApplyByJoinToKingdom(Clan.PlayerClan, khuzaitKingdom, CampaignTime.Now, false);
                
                // 设置关系（加入王国后）
                ChangeRelationAction.ApplyPlayerRelation(khuzaitKingdom.Leader, 20);
                foreach (var lord in khuzaitKingdom.Clans.Where(c => c.Leader != null && c.Leader != khuzaitKingdom.Leader && c.Tier >= 3).Select(c => c.Leader))
                {
                    ChangeRelationAction.ApplyPlayerRelation(lord, 10);
                }
                
                // 记录加入后的状态（关键字段：Clan + Hero）
                OriginLog.Info($"[JoinKhuzaitAsNoble] AFTER:");
                OriginLog.Info($"[JoinKhuzaitAsNoble]   - clan.Kingdom?.StringId = {(Clan.PlayerClan.Kingdom?.StringId ?? "null")}");
                OriginLog.Info($"[JoinKhuzaitAsNoble]   - clan.IsNoble = {Clan.PlayerClan.IsNoble}");
                OriginLog.Info($"[JoinKhuzaitAsNoble]   - clan.IsMinorFaction = {Clan.PlayerClan.IsMinorFaction}");
                OriginLog.Info($"[JoinKhuzaitAsNoble]   - clan.IsClanTypeMercenary = {Clan.PlayerClan.IsClanTypeMercenary}");
                OriginLog.Info($"[JoinKhuzaitAsNoble]   - clan.IsUnderMercenaryService = {Clan.PlayerClan.IsUnderMercenaryService}");
                OriginLog.Info($"[JoinKhuzaitAsNoble]   - Hero.MainHero.IsLord = {(Hero.MainHero?.IsLord ?? false)}");
                OriginLog.Info($"[JoinKhuzaitAsNoble]   - Hero.MainHero.Occupation = {(Hero.MainHero != null ? Hero.MainHero.Occupation.ToString() : "null")}");

                // 加入王国后，兜底设置标志位（确保是封臣/贵族，不是雇佣兵）
                bool needFix = false;
                if (!Clan.PlayerClan.IsNoble)
                {
                    OriginLog.Info("[JoinKhuzaitAsNoble] 兜底设置: IsNoble = true");
                    Clan.PlayerClan.IsNoble = true;
                    needFix = true;
                }
                if (Clan.PlayerClan.IsUnderMercenaryService)
                {
                    OriginLog.Warning("[JoinKhuzaitAsNoble] 警告: 加入后 IsUnderMercenaryService 仍为 true");
                    needFix = true;
                }

                if (needFix)
                {
                    OriginLog.Info($"[JoinKhuzaitAsNoble] 兜底设置后最终状态:");
                    OriginLog.Info($"[JoinKhuzaitAsNoble]   - clan.Kingdom?.StringId = {(Clan.PlayerClan.Kingdom?.StringId ?? "null")}");
                    OriginLog.Info($"[JoinKhuzaitAsNoble]   - clan.IsNoble = {Clan.PlayerClan.IsNoble}");
                    OriginLog.Info($"[JoinKhuzaitAsNoble]   - clan.IsMinorFaction = {Clan.PlayerClan.IsMinorFaction} (只读，由系统管理)");
                    OriginLog.Info($"[JoinKhuzaitAsNoble]   - clan.IsClanTypeMercenary = {Clan.PlayerClan.IsClanTypeMercenary} (只读，由系统管理)");
                    OriginLog.Info($"[JoinKhuzaitAsNoble]   - clan.IsUnderMercenaryService = {Clan.PlayerClan.IsUnderMercenaryService}");
                }

                // 验证：确保成功加入为封臣（使用三个真正的判据）
                // 判据1：Clan.Kingdom == khuzaitKingdom
                bool condition1 = Clan.PlayerClan.Kingdom == khuzaitKingdom;
                // 判据2：!IsUnderMercenaryService（只读，系统判据）
                bool condition2 = !Clan.PlayerClan.IsUnderMercenaryService;
                // 判据3：khuzaitKingdom.Clans.Contains(Clan.PlayerClan)（是否被登记成这个王国的 clan 成员）
                bool condition3 = khuzaitKingdom.Clans?.Contains(Clan.PlayerClan) ?? false;
                
                bool isVassalFinal = condition1 && condition2 && condition3;
                bool isLord = Hero.MainHero?.IsLord ?? false;
                
                OriginLog.Info($"[JoinKhuzaitAsNoble] 最终验证（三个真正判据）:");
                OriginLog.Info($"[JoinKhuzaitAsNoble]   判据1: Clan.Kingdom == khuzaitKingdom = {condition1}");
                OriginLog.Info($"[JoinKhuzaitAsNoble]   判据2: !IsUnderMercenaryService = {condition2}");
                OriginLog.Info($"[JoinKhuzaitAsNoble]   判据3: kingdom.Clans.Contains(clan) = {condition3}");
                OriginLog.Info($"[JoinKhuzaitAsNoble]   isVassal (三个判据都满足) = {isVassalFinal}");
                OriginLog.Info($"[JoinKhuzaitAsNoble]   isLord (Hero.IsLord) = {isLord}");
                OriginLog.Info($"[JoinKhuzaitAsNoble]   clan.IsNoble = {Clan.PlayerClan.IsNoble} (补充标记，不是判据)");
                
                if (isVassalFinal && isLord)
                {
                    OriginLog.Info("[JoinKhuzaitAsNoble] ✅ 成功加入库塞特王国为封臣（vassal/贵族）");
                }
                else
                {
                    if (!condition1)
                    {
                        OriginLog.Warning("[JoinKhuzaitAsNoble] ⚠️ 判据1失败：Clan.Kingdom != khuzaitKingdom");
                    }
                    if (!condition2)
                    {
                        OriginLog.Warning("[JoinKhuzaitAsNoble] ⚠️ 判据2失败：IsUnderMercenaryService = true（仍在雇佣兵服务中）");
                    }
                    if (!condition3)
                    {
                        OriginLog.Warning("[JoinKhuzaitAsNoble] ⚠️ 判据3失败：未在 kingdom.Clans 列表中（未被登记为成员）");
                    }
                    if (!isLord)
                    {
                        OriginLog.Warning("[JoinKhuzaitAsNoble] ⚠️ 主角不是 Lord（Occupation 未正确设置）");
                    }
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[JoinKhuzaitAsNoble] 加入库塞特封臣失败: {ex.Message}");
                OriginLog.Error($"[JoinKhuzaitAsNoble] StackTrace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    OriginLog.Error($"[JoinKhuzaitAsNoble] InnerException: {ex.InnerException.Message}");
                }
            }
        }

        /// <summary>
        /// 在 OnTick 中定期检查状态（用于检测状态是否被覆盖）
        /// </summary>
        public static void CheckPlayerClanStatus()
        {
            if (Clan.PlayerClan == null) return;
            
            var khuzaitKingdom = FindKingdom("kingdom_khuzait");
            if (khuzaitKingdom == null) return;
            
            // 检查三个判据
            bool condition1 = Clan.PlayerClan.Kingdom == khuzaitKingdom;
            bool condition2 = !Clan.PlayerClan.IsUnderMercenaryService;
            bool condition3 = khuzaitKingdom.Clans?.Contains(Clan.PlayerClan) ?? false;
            bool isVassal = condition1 && condition2 && condition3;
            bool isLord = Hero.MainHero?.IsLord ?? false;
            
            OriginLog.Info($"[StatusCheck] 状态检查:");
            OriginLog.Info($"[StatusCheck]   判据1: Clan.Kingdom == khuzaitKingdom = {condition1}");
            OriginLog.Info($"[StatusCheck]   判据2: !IsUnderMercenaryService = {condition2}");
            OriginLog.Info($"[StatusCheck]   判据3: kingdom.Clans.Contains(clan) = {condition3}");
            OriginLog.Info($"[StatusCheck]   isVassal = {isVassal}, isLord = {isLord}, IsNoble = {Clan.PlayerClan.IsNoble}");
            
            // 如果状态异常，记录警告
            if (!isVassal || !isLord)
            {
                OriginLog.Warning($"[StatusCheck] ⚠️ 状态异常！isVassal={isVassal}, isLord={isLord}");
            }
        }

        /// <summary>
        /// 确保玩家家族是贵族（用于在稳定时机重新设置）
        /// 同时确保主角是 Lord（这是判断"贵族/封臣"的真正判据）
        /// </summary>
        public static void EnsurePlayerClanIsNoble()
        {
            if (Clan.PlayerClan == null)
            {
                OriginLog.Warning("[MinorNoble] EnsurePlayerClanIsNoble: Clan.PlayerClan 为 null");
                return;
            }
            
            // 关键：确保主角是 Lord
            if (Hero.MainHero != null && !Hero.MainHero.IsLord)
            {
                OriginLog.Info("[MinorNoble] EnsurePlayerClanIsNoble: 主角不是 Lord，尝试设置 Occupation = Lord");
                try
                {
                    try
                    {
                        Hero.MainHero.SetNewOccupation(Occupation.Lord);
                        OriginLog.Info($"[MinorNoble] EnsurePlayerClanIsNoble: 直接调用成功: IsLord={Hero.MainHero.IsLord}, Occupation={Hero.MainHero.Occupation}");
                    }
                    catch (MissingMethodException)
                    {
                        var heroType = typeof(Hero);
                        var setOccupationMethod = heroType.GetMethod("SetNewOccupation", BindingFlags.Public | BindingFlags.Instance);
                        if (setOccupationMethod != null)
                        {
                            var occupationType = typeof(Hero).Assembly.GetType("TaleWorlds.Core.Occupation") 
                                                ?? typeof(Hero).Assembly.GetTypes().FirstOrDefault(t => t.Name == "Occupation" && t.IsEnum);
                            if (occupationType != null)
                            {
                                var lordValue = Enum.Parse(occupationType, "Lord");
                                setOccupationMethod.Invoke(Hero.MainHero, new object[] { lordValue });
                                OriginLog.Info($"[MinorNoble] EnsurePlayerClanIsNoble: 反射调用成功: IsLord={Hero.MainHero.IsLord}, Occupation={Hero.MainHero.Occupation}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    OriginLog.Error($"[MinorNoble] EnsurePlayerClanIsNoble: 设置主角为 Lord 失败: {ex.Message}");
                    OriginLog.Error($"[MinorNoble] EnsurePlayerClanIsNoble: StackTrace: {ex.StackTrace}");
                }
            }
            
            bool beforeIsNoble = Clan.PlayerClan.IsNoble;
            if (!beforeIsNoble)
            {
                OriginLog.Info($"[MinorNoble] EnsurePlayerClanIsNoble: before={beforeIsNoble}, Tier={Clan.PlayerClan.Tier}");
                Clan.PlayerClan.IsNoble = true;
                bool afterIsNoble = Clan.PlayerClan.IsNoble;
                OriginLog.Info($"[MinorNoble] EnsurePlayerClanIsNoble: after={afterIsNoble} (设置成功: {afterIsNoble == true})");
            }
            else
            {
                OriginLog.Info($"[MinorNoble] EnsurePlayerClanIsNoble: 已经是贵族，无需设置");
            }
        }

        private static void ApplyMinorNobleNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_minor_noble_marginalized")) return;
            
            var reason = nodes["khz_node_minor_noble_marginalized"];
            var khuzaitKingdom = FindKingdom("kingdom_khuzait");
            
            switch (reason)
            {
                case "maternal":
                    // 母系旁支，名分尴尬：魅力+30，与库塞特可汗关系-30
                    AddSkill(hero, "charm", 30);
                    if (khuzaitKingdom != null)
                    {
                        ChangeRelationAction.ApplyPlayerRelation(khuzaitKingdom.Leader, -30);
                    }
                    break;
                case "wrong_side":
                    // 站错队，成了旧账：侦察+30，战术+30，与精英氏族关系-30
                    AddSkill(hero, "scouting", 30);
                    AddSkill(hero, "tactics", 30);
                    if (khuzaitKingdom != null)
                    {
                        foreach (var clan in khuzaitKingdom.Clans.Where(c => c.Tier >= 3 && c.Leader != null && c.Leader != khuzaitKingdom.Leader))
                        {
                            ChangeRelationAction.ApplyPlayerRelation(clan.Leader, -30);
                        }
                    }
                    break;
                case "unrewarded":
                    // 战功被吞，心怀不平：领导力+45，名望+1，与库塞特可汗关系-30
                    AddSkill(hero, "leadership", 45);
                    GainRenown(hero, 1);
                    if (khuzaitKingdom != null)
                    {
                        ChangeRelationAction.ApplyPlayerRelation(khuzaitKingdom.Leader, -30);
                    }
                    break;
                case "too_dangerous":
                    // 才名过盛，遭人忌惮：名望+1，魅力+20，与精英氏族关系-30
                    GainRenown(hero, 1);
                    AddSkill(hero, "charm", 20);
                    if (khuzaitKingdom != null)
                    {
                        foreach (var clan in khuzaitKingdom.Clans.Where(c => c.Tier >= 3 && c.Leader != null && c.Leader != khuzaitKingdom.Leader))
                        {
                            ChangeRelationAction.ApplyPlayerRelation(clan.Leader, -30);
                        }
                    }
                    break;
            }
        }

        private static void ApplyMinorNobleNode2(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_minor_noble_style")) return;
            
            var style = nodes["khz_node_minor_noble_style"];
            switch (style)
            {
                case "etiquette":
                    // 礼法与人情：魅力+45，管理+30，金币+8000（中）
                    AddSkill(hero, "charm", 45);
                    AddSkill(hero, "steward", 30);
                    AddGold(hero, 8000); // 基础金币：中档
                    break;
                case "military":
                    // 军功与威望：领导力+45，战术+30，金币+3000（低）
                    AddSkill(hero, "leadership", 45);
                    AddSkill(hero, "tactics", 30);
                    AddGold(hero, 3000);
                    break;
                case "trade":
                    // 账本与马队：贸易+45，管理+30，金币+15000（大），马匹+2（中）
                    AddSkill(hero, "trade", 45);
                    AddSkill(hero, "steward", 30);
                    AddGold(hero, 15000);
                    AddMounts(MobileParty.MainParty, 2);
                    break;
            }
        }

        private static void ApplyMinorNobleNode3(MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_minor_noble_retinue")) return;
            
            var retinue = nodes["khz_node_minor_noble_retinue"];
            switch (retinue)
            {
                case "noble":
                    // 少量贵族随骑（精）：额外兵力：贵族子弟×8，骑射手×10，食物+20（中），士气+4
                    AddTroops(party, "khuzait_noble_son", 8);
                    AddTroops(party, "khuzait_horse_archer", 10);
                    AddFood(party, 20);
                    if (party != null) party.RecentEventsMorale += 4;
                    break;
                case "light":
                    // 轻骑为主（快）：额外兵力：轻骑兵×12，骑射手×10，食物+20（中）
                    AddTroops(party, "khuzait_light_cavalry", 12);
                    AddTroops(party, "khuzait_horse_archer", 10);
                    AddFood(party, 20);
                    break;
                case "mixed":
                    // 混编（更稳）：额外兵力：骑射手×12，游牧战士×10，食物+30（大）
                    AddTroops(party, "khuzait_horse_archer", 12);
                    AddTroops(party, "khuzait_nomad", 10);
                    AddFood(party, 30);
                    break;
            }
        }

        private static void ApplyMinorNobleNode4(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_minor_noble_alignment")) return;
            
            var alignment = nodes["khz_node_minor_noble_alignment"];
            var khuzaitKingdom = FindKingdom("kingdom_khuzait");
            
            switch (alignment)
            {
                case "khan":
                    // 先靠近可汗：与库塞特可汗关系+20，与精英氏族关系+20
                    if (khuzaitKingdom != null)
                    {
                        ChangeRelationAction.ApplyPlayerRelation(khuzaitKingdom.Leader, 20);
                        foreach (var clan in khuzaitKingdom.Clans.Where(c => c.Tier >= 3 && c.Leader != null && c.Leader != khuzaitKingdom.Leader))
                        {
                            ChangeRelationAction.ApplyPlayerRelation(clan.Leader, 20);
                        }
                    }
                    break;
                case "clan":
                    // 先靠近大氏族：与精英氏族关系+20，与库塞特可汗关系-30
                    if (khuzaitKingdom != null)
                    {
                        ChangeRelationAction.ApplyPlayerRelation(khuzaitKingdom.Leader, -30);
                        foreach (var clan in khuzaitKingdom.Clans.Where(c => c.Tier >= 3 && c.Leader != null && c.Leader != khuzaitKingdom.Leader))
                        {
                            ChangeRelationAction.ApplyPlayerRelation(clan.Leader, 20);
                        }
                    }
                    break;
                case "neutral":
                    // 先中立观望：侦察+30，魅力+20
                    AddSkill(hero, "scouting", 30);
                    AddSkill(hero, "charm", 20);
                    break;
            }
        }

        #endregion

        #region 3. 西迁部族首领 (migrant_chief)

        private static void ApplyMigrantChiefOrigin(Hero hero, Clan clan, MobileParty party)
        {
            SetClanTier(clan, 2);

            var khuzaitKingdom = FindKingdom("kingdom_khuzait");
            if (khuzaitKingdom != null)
            {
                ChangeRelationAction.ApplyPlayerRelation(khuzaitKingdom.Leader, -10);
            }

            var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
            
            ApplyMigrantChiefNode1(hero, selectedNodes);
            ApplyMigrantChiefNode2(party, selectedNodes);
            ApplyMigrantChiefNode3(hero, party, selectedNodes);
            ApplyMigrantChiefNode4(hero, selectedNodes);

            AddGold(hero, 8000); // 基础金币：中档
            
            // 设置出生位置：库塞特村子（避免城市会卡住）
            if (MobileParty.MainParty != null && Campaign.Current != null)
            {
                SetPresetOriginStartingLocation(MobileParty.MainParty, "khuzait");
            }
        }

        private static void ApplyMigrantChiefNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_migrant_reason")) return;
            
            var reason = nodes["khz_node_migrant_reason"];
            var khuzaitKingdom = FindKingdom("kingdom_khuzait");
            
            switch (reason)
            {
                case "annexed":
                    // 被吞并，带人逃命：名望+1，侦察+30，管理+30，与库塞特可汗关系-30
                    GainRenown(hero, 1);
                    AddSkill(hero, "scouting", 30);
                    AddSkill(hero, "steward", 30);
                    if (khuzaitKingdom != null)
                    {
                        ChangeRelationAction.ApplyPlayerRelation(khuzaitKingdom.Leader, -30);
                    }
                    break;
                case "drought":
                    // 旱灾迫迁：食物-3，侦察+30，管理+30
                    AddFood(MobileParty.MainParty, -3);
                    AddSkill(hero, "scouting", 30);
                    AddSkill(hero, "steward", 30);
                    break;
                case "tax":
                    // 税役太重，举族出走：金币+3000（低），贸易+30，魅力+20，与精英氏族关系-30
                    AddGold(hero, 3000);
                    AddSkill(hero, "trade", 30);
                    AddSkill(hero, "charm", 20);
                    if (khuzaitKingdom != null)
                    {
                        foreach (var clan in khuzaitKingdom.Clans.Where(c => c.Tier >= 3 && c.Leader != null && c.Leader != khuzaitKingdom.Leader))
                        {
                            ChangeRelationAction.ApplyPlayerRelation(clan.Leader, -30);
                        }
                    }
                    break;
                case "blood_feud":
                    // 血仇不止，只能远走：战术+30，领导力+30
                    AddSkill(hero, "tactics", 30);
                    AddSkill(hero, "leadership", 30);
                    break;
            }
        }

        private static void ApplyMigrantChiefNode2(MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_migrant_people")) return;
            
            var people = nodes["khz_node_migrant_people"];
            switch (people)
            {
                case "warriors":
                    // 以青壮战士为主：额外兵力：游牧战士×14，骑射手×6，食物+20（中）
                    AddTroops(party, "khuzait_nomad", 14);
                    AddTroops(party, "khuzait_horse_archer", 6);
                    AddFood(party, 20);
                    break;
                case "families":
                    // 家眷居多（负担但稳）：额外兵力：游牧战士×10，部落青年×8，食物+30（大）
                    AddTroops(party, "khuzait_nomad", 10);
                    AddTroops(party, "khuzait_tribal_youth", 8);
                    AddFood(party, 30);
                    break;
                case "craftsmen":
                    // 带着匠人（更会经营）：额外兵力：游牧战士×10，部落青年×6，金币+8000（中），管理+30，贸易+20
                    AddTroops(party, "khuzait_nomad", 10);
                    AddTroops(party, "khuzait_tribal_youth", 6);
                    AddGold(Hero.MainHero, 8000);
                    AddSkill(Hero.MainHero, "steward", 30);
                    AddSkill(Hero.MainHero, "trade", 20);
                    break;
            }
        }

        private static void ApplyMigrantChiefNode3(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_migrant_assets")) return;
            
            var assets = nodes["khz_node_migrant_assets"];
            switch (assets)
            {
                case "livestock":
                    // 牲畜与皮货（中）：金币+8000（中），食物+20（中）
                    AddGold(hero, 8000); // 基础金币：中档
                    AddFood(party, 20);
                    break;
                case "horses":
                    // 更多的马（大）：马匹+3（大），金币+3000（低），骑术+30
                    AddMounts(party, 3);
                    AddGold(hero, 3000);
                    AddSkill(hero, "riding", 30);
                    break;
                case "relics":
                    // 部族传承的祖物（顶级宝贝）：名望+2，士气+6，魅力+45
                    GainRenown(hero, 2);
                    if (party != null) party.RecentEventsMorale += 6;
                    AddSkill(hero, "charm", 45);
                    break;
            }
        }

        private static void ApplyMigrantChiefNode4(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_migrant_goal")) return;
            
            var goal = nodes["khz_node_migrant_goal"];
            switch (goal)
            {
                case "settle":
                    // 找地定居：管理+30，魅力+20
                    AddSkill(hero, "steward", 30);
                    AddSkill(hero, "charm", 20);
                    break;
                case "mercenary":
                    // 卖力换生路：领导力+30，战术+30，金币+3000（低）
                    AddSkill(hero, "leadership", 30);
                    AddSkill(hero, "tactics", 30);
                    AddGold(hero, 3000);
                    break;
                case "revenge":
                    // 迟早复仇：战术+30，士气+3，名望+1
                    AddSkill(hero, "tactics", 30);
                    if (MobileParty.MainParty != null)
                    {
                        MobileParty.MainParty.RecentEventsMorale += 3;
                    }
                    GainRenown(hero, 1);
                    break;
            }
        }

        #endregion

        #region 4. 汗廷军叛逃者 (army_deserter)

        private static void ApplyArmyDeserterOrigin(Hero hero, Clan clan, MobileParty party)
        {
            SetClanTier(clan, 1);

            var khuzaitKingdom = FindKingdom("kingdom_khuzait");
            if (khuzaitKingdom != null)
            {
                ChangeRelationAction.ApplyPlayerRelation(khuzaitKingdom.Leader, -40);
            }

            var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
            
            ApplyArmyDeserterNode1(hero, selectedNodes);
            ApplyArmyDeserterNode2(party, selectedNodes);
            ApplyArmyDeserterNode3(hero, selectedNodes);
            ApplyArmyDeserterNode4(hero, selectedNodes);

            AddGold(hero, 3000); // 基础金币：低档
            
            // 设置出生位置：库塞特村子（避免城市会卡住）
            if (MobileParty.MainParty != null && Campaign.Current != null)
            {
                SetPresetOriginStartingLocation(MobileParty.MainParty, "khuzait");
            }
        }

        private static void ApplyArmyDeserterNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_deserter_reason")) return;
            
            var reason = nodes["khz_node_deserter_reason"];
            var khuzaitKingdom = FindKingdom("kingdom_khuzait");
            
            switch (reason)
            {
                case "scapegoat":
                    // 被当替罪羊：战术+30，侦察+20，与库塞特可汗关系-30
                    AddSkill(hero, "tactics", 30);
                    AddSkill(hero, "scouting", 20);
                    if (khuzaitKingdom != null)
                    {
                        ChangeRelationAction.ApplyPlayerRelation(khuzaitKingdom.Leader, -30);
                    }
                    break;
                case "unpaid":
                    // 军饷被克扣：金币+200，领导力+30，劫掠+20，与库塞特可汗关系-30
                    AddGold(hero, 3000); // 金币+3000（低）
                    AddSkill(hero, "leadership", 30);
                    AddSkill(hero, "roguery", 20);
                    if (khuzaitKingdom != null)
                    {
                        ChangeRelationAction.ApplyPlayerRelation(khuzaitKingdom.Leader, -30);
                    }
                    break;
                case "massacre":
                    // 不愿执行屠村命令（拒绝屠杀命令）：名望+1，领导力+30，魅力+20，仁慈+1，与库塞特可汗关系-30
                    GainRenown(hero, 1);
                    AddSkill(hero, "leadership", 30);
                    AddSkill(hero, "charm", 20);
                    AddTrait(hero, "Mercy", 1);
                    if (khuzaitKingdom != null)
                    {
                        ChangeRelationAction.ApplyPlayerRelation(khuzaitKingdom.Leader, -30);
                    }
                    break;
                case "murder":
                    // 军中斗殴闹出人命：金币+150，劫掠+30，体能+20，仁慈-1，与库塞特可汗关系-30
                    AddGold(hero, 3000); // 金币+3000（低）
                    AddSkill(hero, "roguery", 30);
                    AddSkill(hero, "athletics", 20);
                    AddTrait(hero, "Mercy", -1);
                    if (khuzaitKingdom != null)
                    {
                        ChangeRelationAction.ApplyPlayerRelation(khuzaitKingdom.Leader, -30);
                    }
                    break;
            }
        }

        private static void ApplyArmyDeserterNode2(MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_deserter_followers")) return;
            
            var followers = nodes["khz_node_deserter_followers"];
            switch (followers)
            {
                case "veteran_archer":
                    AddTroops(party, "khuzait_horse_archer", 12);
                    AddFood(party, 6);
                    break;
                case "javelin":
                    AddTroops(party, "khuzait_light_cavalry", 8);
                    AddTroops(party, "khuzait_nomad", 6);
                    AddFood(party, 6);
                    break;
                case "mixed":
                    AddTroops(party, "khuzait_horse_archer", 8);
                    AddTroops(party, "khuzait_nomad", 8);
                    AddFood(party, 7);
                    break;
            }
        }

        private static void ApplyArmyDeserterNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_deserter_supplies")) return;
            
            var supplies = nodes["khz_node_deserter_supplies"];
            switch (supplies)
            {
                case "gear":
                    // 带走一批甲械：金币+3000（低）
                    AddGold(hero, 3000);
                    break;
                case "pay":
                    // 带走一袋军饷：金币+8000（中）
                    AddGold(hero, 8000); // 基础金币：中档
                    break;
                case "docs":
                    // 带走军令/名册：魅力+20，战术+20
                    AddSkill(hero, "charm", 20);
                    AddSkill(hero, "tactics", 20);
                    break;
            }
        }

        private static void ApplyArmyDeserterNode4(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_deserter_path")) return;
            
            var path = nodes["khz_node_deserter_path"];
            var khuzaitKingdom = FindKingdom("kingdom_khuzait");
            
            switch (path)
            {
                case "redemption":
                    // 找机会洗白（赎罪）：魅力+30，荣誉+1，与库塞特可汗关系+20
                    AddSkill(hero, "charm", 30);
                    AddTrait(hero, "Honor", 1);
                    if (khuzaitKingdom != null)
                    {
                        ChangeRelationAction.ApplyPlayerRelation(khuzaitKingdom.Leader, 20);
                    }
                    break;
                case "warlord":
                    // 彻底当军阀（复仇）：劫掠+30，领导力+30，名望+1
                    AddSkill(hero, "roguery", 30);
                    AddSkill(hero, "leadership", 30);
                    GainRenown(hero, 1);
                    break;
                case "exile":
                    // 离开草原（自由生活）：侦察+20，魅力+20
                    AddSkill(hero, "scouting", 20);
                    AddSkill(hero, "charm", 20);
                    break;
            }
        }

        #endregion

        #region 5. 草原商路守护者 (trade_protector)

        private static void ApplyTradeProtectorOrigin(Hero hero, Clan clan, MobileParty party)
        {
            SetClanTier(clan, 2);

            var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
            
            ApplyTradeProtectorNode1(hero, selectedNodes);
            ApplyTradeProtectorNode2(hero, selectedNodes);
            ApplyTradeProtectorNode3(party, selectedNodes);
            ApplyTradeProtectorNode4(hero, selectedNodes);

            AddGold(hero, 8000); // 基础金币：中档
            
            // 设置出生位置：库塞特村子（避免城市会卡住）
            if (MobileParty.MainParty != null && Campaign.Current != null)
            {
                SetPresetOriginStartingLocation(MobileParty.MainParty, "khuzait");
            }
        }

        private static void ApplyTradeProtectorNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_trade_route")) return;
            
            var route = nodes["khz_node_trade_route"];
            switch (route)
            {
                case "empire":
                    // 通往帝国边境：贸易+30，侦察+20
                    AddSkill(hero, "trade", 30);
                    AddSkill(hero, "scouting", 20);
                    break;
                case "desert":
                    // 通往沙漠诸邦：贸易+30，侦察+20
                    AddSkill(hero, "trade", 30);
                    AddSkill(hero, "scouting", 20);
                    break;
                case "internal":
                    // 汗国内部走廊：管理+30，魅力+20
                    AddSkill(hero, "steward", 30);
                    AddSkill(hero, "charm", 20);
                    break;
            }
        }

        private static void ApplyTradeProtectorNode2(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_trade_model")) return;
            
            var model = nodes["khz_node_trade_model"];
            
            switch (model)
            {
                case "guard_fee":
                    // 正规护路费（中）：金币+8000（中），贸易+30，与商人关系+20
                    AddGold(hero, 8000); // 基础金币：中档
                    AddSkill(hero, "trade", 30);
                    // 与商人关系+20
                    foreach (var hero2 in OriginSystemHelper.GetAllHeroesSafe().Where(h => h.IsMerchant))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(hero2, 20);
                    }
                    break;
                case "smuggling":
                    // 夹带走私（大）：金币+15000（大），贸易+30，劫掠+30，与商人关系-30
                    AddGold(hero, 15000);
                    AddSkill(hero, "trade", 30);
                    AddSkill(hero, "roguery", 30);
                    AddTrait(hero, "Honor", -1);
                    // 与商人关系-30
                    foreach (var hero2 in OriginSystemHelper.GetAllHeroesSafe().Where(h => h.IsMerchant))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(hero2, -30);
                    }
                    break;
                case "monopoly":
                    // 垄断一段商路（大）：金币+15000（大），贸易+20，领导力+20，与商人关系-70
                    AddGold(hero, 15000);
                    AddSkill(hero, "trade", 20);
                    AddSkill(hero, "leadership", 20);
                    // 与商人关系-70
                    foreach (var hero2 in OriginSystemHelper.GetAllHeroesSafe().Where(h => h.IsMerchant))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(hero2, -70);
                    }
                    break;
            }
        }

        private static void ApplyTradeProtectorNode3(MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_trade_guard")) return;
            
            var guard = nodes["khz_node_trade_guard"];
            switch (guard)
            {
                case "light_cav":
                    AddTroops(party, "khuzait_light_cavalry", 10);
                    AddTroops(party, "khuzait_nomad", 6);
                    AddFood(party, 6);
                    break;
                case "horse_archer":
                    AddTroops(party, "khuzait_horse_archer", 12);
                    AddTroops(party, "khuzait_nomad", 4);
                    AddFood(party, 6);
                    break;
                case "mixed":
                    AddTroops(party, "khuzait_horse_archer", 8);
                    AddTroops(party, "khuzait_light_cavalry", 6);
                    AddTroops(party, "khuzait_nomad", 4);
                    AddFood(party, 7);
                    break;
            }
        }

        private static void ApplyTradeProtectorNode4(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_trade_attitude")) return;
            
            var attitude = nodes["khz_node_trade_attitude"];
            var khuzaitKingdom = FindKingdom("kingdom_khuzait");
            
            switch (attitude)
            {
                case "cooperate":
                    // 保护商队，但不参与政治：魅力+20
                    AddSkill(hero, "charm", 20);
                    break;
                case "neutral":
                    // 保护商队，但不参与政治：魅力+20
                    AddSkill(hero, "charm", 20);
                    break;
                case "resist":
                    // 建立自己的势力：领导力+30
                    AddSkill(hero, "leadership", 30);
                    break;
            }
        }

        #endregion

        #region 6. 迁徙王族 (wandering_prince)

        private static void ApplyWanderingPrinceOrigin(Hero hero, Clan clan, MobileParty party)
        {
            SetClanTier(clan, 3);
            SetClanNoble(clan, true);

            GainRenown(hero, 200);

            var khuzaitKingdom = FindKingdom("kingdom_khuzait");
            if (khuzaitKingdom != null)
            {
                ChangeRelationAction.ApplyPlayerRelation(khuzaitKingdom.Leader, -20);
            }

            var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
            
            ApplyWanderingPrinceNode1(hero, selectedNodes);
            ApplyWanderingPrinceNode2(party, selectedNodes);
            ApplyWanderingPrinceNode3(hero, selectedNodes);
            ApplyWanderingPrinceNode4(hero, party, selectedNodes);

            AddGold(hero, 15000); // 基础金币：大档
            
            // 设置出生位置：库塞特村子（避免城市会卡住）
            if (MobileParty.MainParty != null && Campaign.Current != null)
            {
                SetPresetOriginStartingLocation(MobileParty.MainParty, "khuzait");
            }
        }

        private static void ApplyWanderingPrinceNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_royal_exile_reason")) return;
            
            var exile = nodes["khz_node_royal_exile_reason"];
            var khuzaitKingdom = FindKingdom("kingdom_khuzait");
            
            switch (exile)
            {
                case "usurped":
                    // 被篡位者赶出故土：名望+1，魅力+30，领导力+30
                    GainRenown(hero, 1);
                    AddSkill(hero, "charm", 30);
                    AddSkill(hero, "leadership", 30);
                    break;
                case "clan_war":
                    // 内战败北，带人逃亡：名望+1，战术+30，领导力+30
                    GainRenown(hero, 1);
                    AddSkill(hero, "tactics", 30);
                    AddSkill(hero, "leadership", 30);
                    break;
                case "foresight":
                    // 预感风暴，先走一步：侦察+30，管理+30，魅力+20
                    AddSkill(hero, "scouting", 30);
                    AddSkill(hero, "steward", 30);
                    AddSkill(hero, "charm", 20);
                    break;
                case "treasure":
                    // 携宝逃亡，买路求生：金币+15000（大），与精英氏族关系-30
                    AddGold(hero, 15000);
                    if (khuzaitKingdom != null)
                    {
                        foreach (var clan in khuzaitKingdom.Clans.Where(c => c.Tier >= 3 && c.Leader != null && c.Leader != khuzaitKingdom.Leader))
                        {
                            ChangeRelationAction.ApplyPlayerRelation(clan.Leader, -30);
                        }
                    }
                    break;
            }
        }

        private static void ApplyWanderingPrinceNode2(MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_royal_followers")) return;
            
            var hero = Hero.MainHero;
            if (hero == null) return;
            
            var retinue = nodes["khz_node_royal_followers"];
            switch (retinue)
            {
                case "ochilan":
                    // 斡赤兰诸部（蒙古原型）：额外兵力：骑射手×14，游牧战士×8，食物+20（中）
                    AddTroops(party, "khuzait_horse_archer", 14);
                    AddTroops(party, "khuzait_nomad", 8);
                    AddFood(party, 20);
                    if (party != null) party.RecentEventsMorale += 3;
                    AddSkill(hero, "bow", 2);
                    AddSkill(hero, "tactics", 1);
                    AddSkill(hero, "leadership", 1);
                    break;
                case "dashi":
                    // 大石余庭（西辽原型）：额外兵力：骑射手×8，轻骑兵×6，游牧战士×6，食物+20（中）
                    AddTroops(party, "khuzait_horse_archer", 8);
                    AddTroops(party, "khuzait_light_cavalry", 6);
                    AddTroops(party, "khuzait_nomad", 6);
                    AddFood(party, 20);
                    if (party != null) party.RecentEventsMorale += 2;
                    AddSkill(hero, "steward", 2);
                    AddSkill(hero, "charm", 1);
                    break;
                case "kashan":
                    // 喀珊兀诸部（库曼原型）：额外兵力：轻骑兵×12，游牧战士×8，食物+20（中）
                    AddTroops(party, "khuzait_light_cavalry", 12);
                    AddTroops(party, "khuzait_nomad", 8);
                    AddFood(party, 20);
                    AddSkill(hero, "riding", 2);
                    AddSkill(hero, "trade", 1);
                    AddSkill(hero, "scouting", 1);
                    break;
                case "aigeli":
                    // 艾格里军帐（突厥原型）：额外兵力：骑射手×10，轻骑兵×8，食物+20（中）
                    AddTroops(party, "khuzait_horse_archer", 10);
                    AddTroops(party, "khuzait_light_cavalry", 8);
                    AddFood(party, 20);
                    if (party != null) party.RecentEventsMorale += 4;
                    AddSkill(hero, "tactics", 2);
                    AddSkill(hero, "leadership", 1);
                    break;
                case "tahun":
                    // 蹋浑残部（匈人原型）：额外兵力：轻骑兵×10，游牧战士×10，食物+10（低）
                    AddTroops(party, "khuzait_light_cavalry", 10);
                    AddTroops(party, "khuzait_nomad", 10);
                    AddFood(party, 10);
                    AddSkill(hero, "riding", 2);
                    AddSkill(hero, "scouting", 2);
                    AddSkill(hero, "roguery", 1);
                    break;
                // 兼容旧版本选项ID
                case "cuman":
                    AddTroops(party, "khuzait_light_cavalry", 15);
                    AddTroops(party, "khuzait_nomad", 10);
                    AddFood(party, 20);
                    if (party != null) party.RecentEventsMorale += 4;
                    break;
                case "mongol":
                    AddTroops(party, "khuzait_horse_archer", 12);
                    AddTroops(party, "khuzait_light_cavalry", 8);
                    AddFood(party, 20);
                    break;
                case "ottoman":
                    AddTroops(party, "khuzait_horse_archer", 8);
                    AddTroops(party, "khuzait_light_cavalry", 6);
                    AddTroops(party, "khuzait_nomad", 6);
                    AddFood(party, 20);
                    break;
                case "nobles":
                case "veterans":
                    // 忠诚的伙伴：无父者（the Fatherless）：伙伴加入，额外兵力：贵族子弟×6，骑射手×8，士气+3
                    AddCompanion("spc_wanderer_khuzait_2");
                    AddTroops(party, "khuzait_noble_son", 6);
                    AddTroops(party, "khuzait_horse_archer", 8);
                    if (party != null) party.RecentEventsMorale += 3;
                    break;
                case "mixed":
                    AddTroops(party, "khuzait_horse_archer", 8);
                    AddTroops(party, "khuzait_light_cavalry", 6);
                    AddTroops(party, "khuzait_nomad", 6);
                    AddFood(party, 8);
                    break;
            }
        }

        private static void ApplyWanderingPrinceNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_royal_strategy")) return;
            
            var strategy = nodes["khz_node_royal_strategy"];
            switch (strategy)
            {
                case "infiltrate":
                    // 夺回王位：魅力+30，领导力+30
                    AddSkill(hero, "charm", 30);
                    AddSkill(hero, "leadership", 30);
                    break;
                case "new_kingdom":
                    // 建立新的王国：战术+30，领导力+30
                    AddSkill(hero, "tactics", 30);
                    AddSkill(hero, "leadership", 30);
                    break;
                case "ally":
                    // 寻求庇护：魅力+20
                    AddSkill(hero, "charm", 20);
                    break;
            }
        }

        private static void ApplyWanderingPrinceNode4(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_royal_proof")) return;
            
            var proof = nodes["khz_node_royal_proof"];
            switch (proof)
            {
                case "seal":
                    // 家族传承的黄金战旗（顶级宝贝）：名望+2，士气+6
                    GainRenown(hero, 2);
                    if (party != null) party.RecentEventsMorale += 6;
                    break;
                case "banner":
                    // 家族传承的黄金战旗（顶级宝贝）：名望+2，士气+6
                    GainRenown(hero, 2);
                    if (party != null) party.RecentEventsMorale += 6;
                    break;
                case "wealth":
                    // 家族的巨额财宝（大）：金币+15000（大）
                    AddGold(hero, 15000);
                    break;
            }
        }

        #endregion

        #region 7. 可汗的雇佣战帮 (khans_mercenary)

        private static void ApplyKhansMercenaryOrigin(Hero hero, Clan clan, MobileParty party)
        {
            SetClanTier(clan, 1); // 1级家族（雇佣兵）
            SetClanMercenary(clan, true);

            var khuzaitKingdom = FindKingdom("kingdom_khuzait");
            if (khuzaitKingdom != null)
            {
                ChangeRelationAction.ApplyPlayerRelation(khuzaitKingdom.Leader, 30);
            }

            var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
            
            // 雇佣兵特点：劫掠熟练度和武器熟练度高
            AddSkill(hero, "roguery", 8); // 基础劫掠熟练度+8
            AddSkill(hero, "bow", 6); // 弓熟练度+6
            AddSkill(hero, "onehand", 4); // 单手武器+4
            AddSkill(hero, "throwing", 4); // 投掷武器+4
            
            ApplyKhansMercenaryNode1(hero, selectedNodes);
            ApplyKhansMercenaryNode2(party, selectedNodes);
            ApplyKhansMercenaryNode3(hero, selectedNodes);
            ApplyKhansMercenaryNode4(hero, selectedNodes);

            AddGold(hero, 8000); // 基础金币：中档
            
            // 设置出生位置：库塞特村子（避免城市会卡住）
            if (MobileParty.MainParty != null && Campaign.Current != null)
            {
                SetPresetOriginStartingLocation(MobileParty.MainParty, "khuzait");
            }
        }

        private static void ApplyKhansMercenaryNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_mercenary_reason")) return;
            
            var reason = nodes["khz_node_mercenary_reason"];
            var khuzaitKingdom = FindKingdom("kingdom_khuzait");
            
            switch (reason)
            {
                case "raider":
                    // 你能打，也敢抢：劫掠+30，领导力+20，与库塞特可汗关系+20
                    AddSkill(hero, "roguery", 30);
                    AddSkill(hero, "leadership", 20);
                    if (khuzaitKingdom != null)
                    {
                        ChangeRelationAction.ApplyPlayerRelation(khuzaitKingdom.Leader, 20);
                    }
                    break;
                case "rescue":
                    // 你救过汗廷的人：魅力+20，领导力+30，与库塞特可汗关系+20
                    AddSkill(hero, "charm", 20);
                    AddSkill(hero, "leadership", 30);
                    if (khuzaitKingdom != null)
                    {
                        ChangeRelationAction.ApplyPlayerRelation(khuzaitKingdom.Leader, 20);
                    }
                    break;
                case "patron":
                    // 你背后有大氏族作保：与精英氏族关系+20，与库塞特可汗关系+20
                    if (khuzaitKingdom != null)
                    {
                        ChangeRelationAction.ApplyPlayerRelation(khuzaitKingdom.Leader, 20);
                        foreach (var clan in khuzaitKingdom.Clans.Where(c => c.Tier >= 3 && c.Leader != null && c.Leader != khuzaitKingdom.Leader))
                        {
                            ChangeRelationAction.ApplyPlayerRelation(clan.Leader, 20);
                        }
                    }
                    break;
            }
        }

        private static void ApplyKhansMercenaryNode2(MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_mercenary_style")) return;
            
            var style = nodes["khz_node_mercenary_style"];
            switch (style)
            {
                case "horse_archer":
                    // 骑射机动队：额外兵力：骑射手×14，游牧战士×6，食物+20（中）
                    AddTroops(party, "khuzait_horse_archer", 14);
                    AddTroops(party, "khuzait_nomad", 6);
                    AddFood(party, 20);
                    break;
                case "javelin":
                    // 标枪突击队：额外兵力：轻骑兵×10，骑射手×6，食物+20（中）
                    AddTroops(party, "khuzait_light_cavalry", 10);
                    AddTroops(party, "khuzait_horse_archer", 6);
                    AddFood(party, 20);
                    break;
                case "mixed":
                    // 混编战帮：额外兵力：骑射手×8，轻骑兵×6，游牧战士×6，食物+30（大）
                    AddTroops(party, "khuzait_horse_archer", 8);
                    AddTroops(party, "khuzait_light_cavalry", 6);
                    AddTroops(party, "khuzait_nomad", 6);
                    AddFood(party, 30);
                    break;
            }
        }

        private static void ApplyKhansMercenaryNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_mercenary_advance")) return;
            
            var advance = nodes["khz_node_mercenary_advance"];
            switch (advance)
            {
                case "loot":
                    // 一笔战利品预支（中）：金币+8000（中）
                    AddGold(hero, 8000); // 基础金币：中档
                    break;
                case "gear":
                    // 一批军械马具（中）：马匹+2（中），金币+3000（低）
                    AddGold(hero, 3000);
                    AddMounts(MobileParty.MainParty, 2);
                    break;
                case "pass":
                    // 一纸通行与补给权（大）：食物+30（大），魅力+20
                    AddFood(MobileParty.MainParty, 30);
                    AddSkill(hero, "charm", 20);
                    break;
            }
        }

        private static void ApplyKhansMercenaryNode4(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_mercenary_bottomline")) return;
            
            var bottomline = nodes["khz_node_mercenary_bottomline"];
            var khuzaitKingdom = FindKingdom("kingdom_khuzait");
            
            switch (bottomline)
            {
                case "loyal":
                    // 拿钱办事，但不背刺：魅力+20，与库塞特可汗关系+20
                    AddSkill(hero, "charm", 20);
                    if (khuzaitKingdom != null)
                    {
                        ChangeRelationAction.ApplyPlayerRelation(khuzaitKingdom.Leader, 20);
                    }
                    break;
                case "profit":
                    // 利益第一：贸易+20，劫掠+20，金币+3000（低）
                    AddSkill(hero, "trade", 20);
                    AddSkill(hero, "roguery", 20);
                    AddGold(hero, 3000);
                    break;
                case "ambition":
                    // 迟早要自己当主子：领导力+30，名望+1
                    AddSkill(hero, "leadership", 30);
                    GainRenown(hero, 1);
                    break;
            }
        }

        #endregion

        #region 8. 逃出沙漠的战奴 (slave_escape)

        private static void ApplySlaveEscapeOrigin(Hero hero, Clan clan, MobileParty party)
        {
            SetClanTier(clan, 0);

            // 查找阿塞莱王国（支持多种ID格式）
            var aseraiKingdom = Campaign.Current?.Kingdoms?
                .FirstOrDefault(k => k.StringId == "aserai" || k.StringId == "kingdom_aserai" || k.Culture?.StringId == "aserai");
            
            if (aseraiKingdom != null)
            {
                // ========== 核心：设置国家敌对状态（宣战） ==========
                if (Clan.PlayerClan == null)
                {
                    OriginLog.Warning("[SlaveEscape][Apply] PlayerClan 为 null，无法宣战");
                }
                else
                {
                    IFaction playerFaction = (Clan.PlayerClan.Kingdom as IFaction) ?? (Clan.PlayerClan as IFaction);
                    
                    bool before = FactionManager.IsAtWarAgainstFaction(playerFaction, aseraiKingdom);
                    OriginLog.Info($"[SlaveEscape][War] before IsAtWar={before} playerFaction={playerFaction?.GetType().Name} aseraiId={aseraiKingdom.StringId} kingdomsCount={Campaign.Current?.Kingdoms?.Count ?? -1}");
                    
                    if (!before)
                    {
                        try
                        {
                            DeclareWarAction.ApplyByPlayerHostility(playerFaction, aseraiKingdom);
                            OriginLog.Info($"[SlaveEscape][War] 已调用 DeclareWarAction.ApplyByPlayerHostility");
                        }
                        catch (Exception ex)
                        {
                            OriginLog.Error($"[SlaveEscape][War] 宣战失败: {ex.Message}");
                            OriginLog.Error($"[SlaveEscape][War] StackTrace: {ex.StackTrace}");
                        }
                    }
                    
                    bool after = FactionManager.IsAtWarAgainstFaction(playerFaction, aseraiKingdom);
                    OriginLog.Info($"[SlaveEscape][War] after IsAtWar={after}");
                }
                
                // ========== 补充：设置个人关系（增强敌对氛围） ==========
                // 1. 设置与统治者的关系
                if (aseraiKingdom.Leader != null)
            {
                ChangeRelationAction.ApplyPlayerRelation(aseraiKingdom.Leader, -60);
                    OriginLog.Info($"[SlaveEscape][Apply] 已设置与阿塞莱统治者的敌对关系: {aseraiKingdom.Leader.Name} -60");
                }
                
                // 2. 设置与所有其他领主的关系（Tier >= 3 的主要家族）
                var aseraiLords = aseraiKingdom.Clans?
                    .Where(c => c.Leader != null && c.Leader != aseraiKingdom.Leader && c.Tier >= 3)
                    .Select(c => c.Leader)
                    .ToList();
                
                if (aseraiLords != null && aseraiLords.Any())
                {
                    foreach (var lord in aseraiLords)
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -40);
                    }
                    OriginLog.Info($"[SlaveEscape][Apply] 已设置与 {aseraiLords.Count} 个阿塞莱领主的敌对关系: -40");
                }
            }
            else
            {
                OriginLog.Warning("[SlaveEscape][Apply] 未找到阿塞莱王国");
            }

            var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
            
            ApplySlaveEscapeNode1(hero, selectedNodes);
            ApplySlaveEscapeNode2(hero, selectedNodes);
            ApplySlaveEscapeNode3(hero, selectedNodes);
            ApplySlaveEscapeNode4(hero, party, selectedNodes);
            ApplySlaveEscapeNode5(hero, selectedNodes);

            AddGold(hero, 3000);
        }

        private static void ApplySlaveEscapeNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_ex_slave_before")) return;
            
            var before = nodes["khz_node_ex_slave_before"];
            switch (before)
            {
                case "border_warrior":
                    // 边境战士：体能+30，单手武器+30，士气+2
                    AddSkill(hero, "athletics", 30);
                    AddSkill(hero, "onehand", 30);
                    if (MobileParty.MainParty != null)
                    {
                        MobileParty.MainParty.RecentEventsMorale += 2;
                    }
                    break;
                case "steppe_bandit":
                    // 草原响马：劫掠+45，侦察+30，金币+3000（低）
                    AddSkill(hero, "roguery", 45);
                    AddSkill(hero, "scouting", 30);
                    AddGold(hero, 3000);
                    break;
                case "horse_thief":
                    // 偷马贼：劫掠+30，骑术+30，马匹+2（中）
                    AddSkill(hero, "roguery", 30);
                    AddSkill(hero, "riding", 30);
                    AddMounts(MobileParty.MainParty, 2);
                    break;
                case "mercenary":
                    // 雇佣兵：领导力+30，战术+30，金币+3000（低）
                    AddSkill(hero, "leadership", 30);
                    AddSkill(hero, "tactics", 30);
                    AddGold(hero, 3000);
                    break;
            }
        }

        private static void ApplySlaveEscapeNode2(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_ex_slave_captured")) return;
            
            var captured = nodes["khz_node_ex_slave_captured"];
            var khuzaitKingdom = FindKingdom("kingdom_khuzait");
            
            switch (captured)
            {
                case "raid_village":
                    // 劫掠村子被反杀：体能+20，与商人关系-30
                    AddSkill(hero, "athletics", 20);
                    foreach (var hero2 in OriginSystemHelper.GetAllHeroesSafe().Where(h => h.IsMerchant))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(hero2, -30);
                    }
                    break;
                case "caravan_ambush":
                    // 伏击商队失手：劫掠+20，士气-2，金币+3000（低）
                    AddSkill(hero, "roguery", 20);
                    AddGold(hero, 3000);
                    if (MobileParty.MainParty != null)
                    {
                        MobileParty.MainParty.RecentEventsMorale -= 2;
                    }
                    break;
                case "scapegoat":
                    // 被人出卖当替罪羊：魅力+20，与精英氏族关系-30
                    AddSkill(hero, "charm", 20);
                    if (khuzaitKingdom != null)
                    {
                        foreach (var clan in khuzaitKingdom.Clans.Where(c => c.Tier >= 3 && c.Leader != null && c.Leader != khuzaitKingdom.Leader))
                        {
                            ChangeRelationAction.ApplyPlayerRelation(clan.Leader, -30);
                        }
                    }
                    break;
                case "lost":
                    // 迷路/受伤被捕奴队捡走：侦察+20
                    AddSkill(hero, "scouting", 20);
                    break;
            }
        }

        private static void ApplySlaveEscapeNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_ex_slave_escape")) return;
            
            var escape = nodes["khz_node_ex_slave_escape"];
            switch (escape)
            {
                case "revolt":
                    AddSkill(hero, "onehand", 3);
                    AddSkill(hero, "athletics", 3);
                    AddTrait(hero, "Valor", 1); // 暴动逃亡 → 勇敢+1
                    if (MobileParty.MainParty != null)
                    {
                        MobileParty.MainParty.RecentEventsMorale += 3;
                    }
                    break;
                case "stealth":
                    AddSkill(hero, "scouting", 5);
                    AddSkill(hero, "roguery", 2);
                    AddTrait(hero, "Calculating", 1); // 潜行逃亡 → 算计+1
                    break;
                case "organized":
                    AddSkill(hero, "leadership", 4);
                    AddSkill(hero, "charm", 2);
                    AddTrait(hero, "Calculating", 1); // 组织逃亡 → 算计+1
                    break;
            }
        }

        private static void ApplySlaveEscapeNode4(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_ex_slave_loot")) return;
            
            var loot = nodes["khz_node_ex_slave_loot"];
            switch (loot)
            {
                case "sword_head":
                    // 奴隶主的黄金战刀（顶级宝贝）：名望+2，士气+6
                    GainRenown(hero, 2);
                    if (party != null) party.RecentEventsMorale += 6;
                    break;
                case "money":
                    // 食物和金币（中）：金币+8000（中），食物+20（中）
                    AddGold(hero, 8000); // 基础金币：中档
                    AddFood(party, 20);
                    break;
                case "companions":
                    // 忠诚的伙伴：铁眼（Iron Eye）：伙伴加入，士气+2
                    AddCompanion("spc_wanderer_khuzait_3");
                    if (party != null) party.RecentEventsMorale += 2;
                    break;
                case "papers":
                    // 武器和装备：初始装备：基础武器
                    // 注意：游戏会自动给玩家基础装备，这里不需要额外操作
                    break;
            }
        }

        private static void ApplySlaveEscapeNode5(Hero hero, Dictionary<string, string> nodes)
        {
            OriginLog.Info($"[SlaveEscape][Apply] ApplySlaveEscapeNode5 被调用，nodes.Count={nodes?.Count ?? 0}");
            if (nodes == null)
            {
                OriginLog.Error("[SlaveEscape][Apply] nodes 为 null，无法设置出生位置");
                return;
            }
            
            OriginLog.Info($"[SlaveEscape][Apply] nodes.ContainsKey('khz_node_ex_slave_direction')={nodes.ContainsKey("khz_node_ex_slave_direction")}");
            if (nodes.ContainsKey("khz_node_ex_slave_direction"))
            {
                OriginLog.Info($"[SlaveEscape][Apply] nodes['khz_node_ex_slave_direction']={nodes["khz_node_ex_slave_direction"]}");
            }
            
            if (!nodes.ContainsKey("khz_node_ex_slave_direction"))
            {
                OriginLog.Warning("[SlaveEscape][Apply] nodes 中找不到 khz_node_ex_slave_direction，无法设置出生位置");
                return;
            }
            
            var direction = nodes["khz_node_ex_slave_direction"];
            
            // [SlaveEscape][Apply] 日志：记录调用时机和状态
            var campaignExists = Campaign.Current != null;
            var mainHeroExists = Hero.MainHero != null;
            var mainPartyExists = MobileParty.MainParty != null;
            Vec2 currentPos = Vec2.Invalid;
            if (mainPartyExists)
            {
                var posProp = typeof(MobileParty).GetProperty("Position2D", BindingFlags.Public | BindingFlags.Instance);
                if (posProp != null)
                {
                    currentPos = (Vec2)posProp.GetValue(MobileParty.MainParty);
                }
            }
            
            OriginLog.Info($"[SlaveEscape][Apply] direction={direction} campaign={campaignExists} mainHero={mainHeroExists} mainParty={mainPartyExists} pos=({currentPos.X:F2},{currentPos.Y:F2})");
            
            // 应用技能并设置出生位置（恢复 old-good 的直接调用方式）
            switch (direction)
            {
                case "steppe":
                    AddSkill(hero, "scouting", 3);
                    AddSkill(hero, "riding", 2);
                    // 设置出生位置：埃泽努尔村子（草原）
                    if (MobileParty.MainParty != null && Campaign.Current != null)
                    {
                        OriginLog.Info("[SlaveEscape][Apply] 在 ApplySlaveEscapeNode5 中直接调用 SetSlaveEscapeStartingLocation: steppe");
                        SetSlaveEscapeStartingLocation(MobileParty.MainParty, "steppe", null);
                    }
                    else
                    {
                        OriginLog.Warning("[SlaveEscape][Apply] MainParty 或 Campaign.Current 为空，无法直接设置位置，将依赖 OnTick 兜底");
                    }
                    break;
                case "desert":
                    AddSkill(hero, "steward", 2);
                    AddSkill(hero, "scouting", 3);
                    
                    // 设置与阿塞莱的敌对关系（因为从阿塞莱的奴隶主手中逃脱）
                    // 先打印调试信息
                    if (Campaign.Current?.Kingdoms != null)
                    {
                        OriginLog.Info($"[SlaveEscape][Apply] 可用王国列表 (Count={Campaign.Current.Kingdoms.Count}):");
                        foreach (var kingdom in Campaign.Current.Kingdoms)
                        {
                            OriginLog.Info($"  - {kingdom.Name} (StringId={kingdom.StringId}, Leader={kingdom.Leader?.Name?.ToString() ?? "null"})");
                        }
                    }
                    else
                    {
                        OriginLog.Warning("[SlaveEscape][Apply] Campaign.Current 或 Kingdoms 为 null");
                    }
                    
                    // 查找阿塞莱王国（支持多种ID格式）
                    var aseraiKingdom = Campaign.Current?.Kingdoms?
                        .FirstOrDefault(k => k.StringId == "aserai" || k.StringId == "kingdom_aserai" || k.Culture?.StringId == "aserai");
                    
                    if (aseraiKingdom != null)
                    {
                        OriginLog.Info($"[SlaveEscape][Apply] 找到阿塞莱王国: {aseraiKingdom.Name} (StringId={aseraiKingdom.StringId})");
                        
                        // ========== 核心：设置国家敌对状态（宣战） ==========
                        if (Clan.PlayerClan == null)
                        {
                            OriginLog.Warning("[SlaveEscape][Apply] PlayerClan 为 null，无法宣战");
                        }
                        else
                        {
                            IFaction playerFaction = (Clan.PlayerClan.Kingdom as IFaction) ?? (Clan.PlayerClan as IFaction);
                            
                            bool before = FactionManager.IsAtWarAgainstFaction(playerFaction, aseraiKingdom);
                            OriginLog.Info($"[SlaveEscape][War] before IsAtWar={before} playerFaction={playerFaction?.GetType().Name} aseraiId={aseraiKingdom.StringId} kingdomsCount={Campaign.Current?.Kingdoms?.Count ?? -1}");
                            
                            if (!before)
                            {
                                try
                                {
                                    DeclareWarAction.ApplyByPlayerHostility(playerFaction, aseraiKingdom);
                                    OriginLog.Info($"[SlaveEscape][War] 已调用 DeclareWarAction.ApplyByPlayerHostility");
                                }
                                catch (Exception ex)
                                {
                                    OriginLog.Error($"[SlaveEscape][War] 宣战失败: {ex.Message}");
                                    OriginLog.Error($"[SlaveEscape][War] StackTrace: {ex.StackTrace}");
                                }
                            }
                            
                            bool after = FactionManager.IsAtWarAgainstFaction(playerFaction, aseraiKingdom);
                            OriginLog.Info($"[SlaveEscape][War] after IsAtWar={after}");
                        }
                        
                        // ========== 补充：设置个人关系（增强敌对氛围） ==========
                        // 1. 设置与统治者的关系
                        if (aseraiKingdom.Leader != null)
                        {
                            ChangeRelationAction.ApplyPlayerRelation(aseraiKingdom.Leader, -60);
                            OriginLog.Info($"[SlaveEscape][Apply] 已设置与阿塞莱统治者的敌对关系: {aseraiKingdom.Leader.Name} -60");
                        }
                        
                        // 2. 设置与所有其他领主的关系（Tier >= 3 的主要家族）
                        var aseraiLords = aseraiKingdom.Clans?
                            .Where(c => c.Leader != null && c.Leader != aseraiKingdom.Leader && c.Tier >= 3)
                            .Select(c => c.Leader)
                            .ToList();
                        
                        if (aseraiLords != null && aseraiLords.Any())
                        {
                            foreach (var lord in aseraiLords)
                            {
                                ChangeRelationAction.ApplyPlayerRelation(lord, -40);
                            }
                            OriginLog.Info($"[SlaveEscape][Apply] 已设置与 {aseraiLords.Count} 个阿塞莱领主的敌对关系: -40");
                        }
                    }
                    else
                    {
                        // 仅在找不到时打印调试信息，帮助判断是 ID 错误还是时机问题
                        var cnt = Campaign.Current?.Kingdoms?.Count ?? -1;
                        OriginLog.Warning($"[SlaveEscape][Apply] 未找到阿塞莱王国, Kingdoms.Count={cnt}");
                        if (cnt > 0)
                        {
                            OriginLog.Info("[SlaveEscape][Apply] 可用王国列表:");
                            foreach (var kk in Campaign.Current.Kingdoms.Take(10))
                            {
                                OriginLog.Info($"  - kingdomId={kk.StringId} culture={kk.Culture?.StringId ?? "null"} leader={(kk.Leader != null ? kk.Leader.Name.ToString() : "null")}");
                            }
                        }
                    }
                    
                    // 设置出生位置：沙漠深处（阿塞莱的沙漠城市）
                    if (MobileParty.MainParty != null && Campaign.Current != null)
                    {
                        OriginLog.Info("[SlaveEscape][Apply] 在 ApplySlaveEscapeNode5 中直接调用 SetSlaveEscapeStartingLocation: desert");
                        SetSlaveEscapeStartingLocation(MobileParty.MainParty, "desert", null);
                    }
                    else
                    {
                        OriginLog.Warning("[SlaveEscape][Apply] MainParty 或 Campaign.Current 为空，无法直接设置位置，将依赖 OnTick 兜底");
                    }
                    break;
                case "empire":
                    AddSkill(hero, "charm", 2);
                    AddSkill(hero, "scouting", 2);
                    // 设置出生位置：帝国最南端城市
                    if (MobileParty.MainParty != null && Campaign.Current != null)
                    {
                        OriginLog.Info("[SlaveEscape][Apply] 在 ApplySlaveEscapeNode5 中直接调用 SetSlaveEscapeStartingLocation: empire");
                        SetSlaveEscapeStartingLocation(MobileParty.MainParty, "empire", null);
                    }
                    else
                    {
                        OriginLog.Warning("[SlaveEscape][Apply] MainParty 或 Campaign.Current 为空，无法直接设置位置，将依赖 OnTick 兜底");
                    }
                    break;
            }
        }

        /// <summary>
        /// 设置预设出身的出生位置（使用村子，避免城市会卡住）
        /// </summary>
        public static bool SetPresetOriginStartingLocation(MobileParty party, string cultureId)
        {
            if (party == null || Campaign.Current == null)
            {
                OriginLog.Warning($"[PresetOrigin][Teleport] party 或 Campaign.Current 为空，无法设置出生位置");
                return false;
            }

            try
            {
                Settlement targetSettlement = null;

                // 根据文化查找对应的村子
                if (cultureId == "khuzait")
                {
                    // 库塞特：优先使用埃泽努尔，如果找不到则使用任意库塞特村子
                    targetSettlement = Campaign.Current?.Settlements?.FirstOrDefault(s => 
                        s.StringId == "castle_village_K8_1"); // Erzenur（埃泽努尔）
                    
                    if (targetSettlement == null)
                    {
                        targetSettlement = Campaign.Current?.Settlements?
                            .Where(s => s.IsVillage && s.Culture?.StringId == "khuzait")
                            .FirstOrDefault();
                    }
                }
                else if (cultureId == "empire")
                {
                    // 帝国：优先使用泰格瑞索斯，如果找不到则使用任意帝国村子
                    targetSettlement = Campaign.Current?.Settlements?.FirstOrDefault(s => 
                        s.StringId == "village_ES1_1" || // 泰格瑞索斯
                        s.StringId == "castle_village_EN1_1"); // Varagos（瓦拉戈斯，有羊群）
                    
                    if (targetSettlement == null)
                    {
                        targetSettlement = Campaign.Current?.Settlements?
                            .Where(s => s.IsVillage && s.Culture?.StringId == "empire")
                            .FirstOrDefault();
                    }
                }
                else
                {
                    // 其他文化：使用任意对应文化的村子
                    targetSettlement = Campaign.Current?.Settlements?
                        .Where(s => s.IsVillage && s.Culture?.StringId == cultureId)
                        .FirstOrDefault();
                }

                if (targetSettlement == null)
                {
                    OriginLog.Warning($"[PresetOrigin][Teleport] 未找到 {cultureId} 文化的村子，使用默认位置");
                    return false;
                }

                // 使用 SetSlaveEscapeStartingLocation 的逻辑设置位置（但使用村子）
                var settlementPos = targetSettlement.Position;
                CampaignVec2 position = settlementPos;
                
                // 在村子附近随机偏移一点，避免直接重叠
                var random = new Random();
                var randomOffset = new TaleWorlds.Library.Vec2(
                    (float)(random.NextDouble() * 2.0 - 1.0) * 0.5f,
                    (float)(random.NextDouble() * 2.0 - 1.0) * 0.5f
                );
                position = settlementPos + randomOffset;

                OriginLog.Info($"[PresetOrigin][Teleport] 设置出生位置: {targetSettlement.Name} ({targetSettlement.StringId}) 坐标=({position.X:F2},{position.Y:F2})");

                // 尝试设置位置（复用 SetSlaveEscapeStartingLocation 的设置逻辑）
                return SetPartyPosition(party, position);
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[PresetOrigin][Teleport] 设置出生位置失败: {ex.Message}");
                OriginLog.Error($"[PresetOrigin][Teleport] StackTrace: {ex.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// 设置队伍位置的辅助方法（从 SetSlaveEscapeStartingLocation 提取）
        /// </summary>
        private static bool SetPartyPosition(MobileParty party, CampaignVec2 position)
        {
            try
            {
                // 尝试多种方法设置位置
                var posProp = typeof(MobileParty).GetProperty("Position2D", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (posProp != null && posProp.CanWrite)
                {
                    posProp.SetValue(party, position);
                    OriginLog.Info($"[PresetOrigin][Teleport] 通过 Position2D 设置位置成功");
                    return true;
                }

                // 尝试 Position 属性
                var posProp2 = typeof(MobileParty).GetProperty("Position", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (posProp2 != null && posProp2.CanWrite)
                {
                    posProp2.SetValue(party, position);
                    OriginLog.Info($"[PresetOrigin][Teleport] 通过 Position 设置位置成功");
                    return true;
                }

                // 尝试反射设置私有字段
                var posField = typeof(MobileParty).GetField("_position2D", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (posField != null)
                {
                    posField.SetValue(party, position);
                    OriginLog.Info($"[PresetOrigin][Teleport] 通过 _position2D 字段设置位置成功");
                    return true;
                }

                var posField2 = typeof(MobileParty).GetField("_position", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (posField2 != null)
                {
                    posField2.SetValue(party, position);
                    OriginLog.Info($"[PresetOrigin][Teleport] 通过 _position 字段设置位置成功");
                    return true;
                }

                OriginLog.Warning("[PresetOrigin][Teleport] 无法找到设置位置的方法/字段");
                return false;
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[PresetOrigin][Teleport] 设置位置时出错: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 设置逃奴的出生位置（在角色创建 finalize 时调用）
        /// 返回 true 表示成功，false 表示失败（需要重试）
        /// </summary>
        public static bool SetSlaveEscapeStartingLocation(MobileParty party, string direction, string settlementId = null)
        {
            // 打印输入参数（关键调试信息）
            OriginLog.Info($"[SlaveEscape][Teleport] ========== 开始设置出生位置 ==========");
            OriginLog.Info($"[SlaveEscape][Teleport] 输入参数: direction={direction ?? "null"} settlementId={settlementId ?? "null"}");
            
            if (party == null)
            {
                OriginLog.Error("[SlaveEscape][Teleport] MainParty 为空，无法设置出生位置");
                return false;
            }

            if (Campaign.Current == null)
            {
                OriginLog.Error("[SlaveEscape][Teleport] Campaign.Current 为空，无法查找 settlement");
                return false;
            }

            try
            {
                Settlement targetSettlement = null;

                if (direction == "desert")
                {
                    // 寻找 Quyaz（town_A1）最南端的附属村庄
                    // 先找到 Quyaz 城市
                    var quyazTown = Campaign.Current?.Settlements?.FirstOrDefault(s => s.StringId == "town_A1");
                    if (quyazTown == null)
                    {
                        // 如果找不到 town_A1，尝试通过名称查找
                        quyazTown = Campaign.Current?.Settlements?.FirstOrDefault(s => 
                            s.IsTown && s.Culture?.StringId == "aserai" && 
                            (s.Name.ToString().Contains("Quyaz") || s.Name.ToString().Contains("古亚兹")));
                    }
                    
                    if (quyazTown != null)
                    {
                        OriginLog.Info($"[SlaveEscape][Teleport] 找到 Quyaz 城市: {quyazTown.Name} ({quyazTown.StringId})");
                        
                        // 查找 Quyaz 的所有附属村庄（通过 StringId 匹配 village_A1_*）
                        var quyazVillages = Campaign.Current?.Settlements?
                            .Where(s => s.IsVillage && 
                                (s.StringId == "village_A1_1" || // Tasheba
                                 s.StringId == "village_A1_2" || // Baq
                                 s.StringId == "village_A1_4" || // Hiblet
                                 s.StringId == "castle_village_A1_1" || // Tubilis
                                 s.StringId == "castle_village_A1_2")) // Fanab
                        .ToList();
                    
                        if (quyazVillages != null && quyazVillages.Any())
                        {
                            OriginLog.Info($"[SlaveEscape][Teleport] 找到 {quyazVillages.Count} 个 Quyaz 附属村庄:");
                            foreach (var village in quyazVillages)
                            {
                                var pos = village.Position;
                                OriginLog.Info($"  - {village.Name} ({village.StringId}) Pos=({pos.X:F2},{pos.Y:F2})");
                            }
                            
                            // 选择最南端的村庄（Y值最大）
                            var southernmostVillage = quyazVillages.OrderByDescending(s => s.Position.Y).FirstOrDefault();
                            if (southernmostVillage != null)
                            {
                                targetSettlement = southernmostVillage;
                                OriginLog.Info($"[SlaveEscape][Teleport] 找到 Quyaz 最南端村庄: {southernmostVillage.Name} ({southernmostVillage.StringId})");
                            }
                        }
                    }
                    
                    // 如果找不到村庄，fallback 到 Quyaz 城市本身
                    if (targetSettlement == null && quyazTown != null)
                    {
                        targetSettlement = quyazTown;
                        OriginLog.Info($"[SlaveEscape][Teleport] 未找到村庄，使用 Quyaz 城市: {quyazTown.Name}");
                    }
                    
                    // 如果还是找不到，尝试通过ID查找其他阿塞莱城市
                    if (targetSettlement == null)
                    {
                        targetSettlement = Campaign.Current?.Settlements?.FirstOrDefault(s => 
                            s.StringId == "town_A1" || // Quyaz
                            s.StringId == "town_AN1" || // Quyaz (alternate ID)
                            s.StringId == "town_AN2" || // Sanala
                            s.StringId == "town_AN3");  // Husn Fulq
                        
                        if (targetSettlement != null)
                        {
                            OriginLog.Info($"[SlaveEscape][Teleport] 通过ID找到沙漠城市: {targetSettlement.Name}");
                        }
                    }
                }
                else if (direction == "empire")
                {
                    // 寻找泰格瑞索斯村子（village_ES1_1 或其他帝国村子）
                    // 优先使用泰格瑞索斯，如果找不到则使用其他帝国村子（避免城市会卡住）
                    targetSettlement = Campaign.Current?.Settlements?.FirstOrDefault(s => 
                        s.StringId == "village_ES1_1" || // 泰格瑞索斯（Tegriosos）
                        s.StringId == "village_ES1_2" ||
                        s.StringId == "village_ES1_3" ||
                        s.StringId == "village_ES1_4" ||
                        s.StringId == "castle_village_EN1_1"); // Varagos（瓦拉戈斯，有羊群的村子）
                    
                    if (targetSettlement == null)
                    {
                        // 如果找不到，尝试通过名称查找包含"泰格瑞索斯"的村子
                        targetSettlement = Campaign.Current?.Settlements?.FirstOrDefault(s => 
                            s.IsVillage && s.Culture?.StringId == "empire" &&
                            (s.Name.ToString().Contains("Tegriosos") || s.Name.ToString().Contains("泰格瑞索斯")));
                    }
                    
                    if (targetSettlement == null)
                    {
                        // 如果还是找不到，使用任意一个帝国村子（避免城市）
                        targetSettlement = Campaign.Current?.Settlements?
                            .Where(s => s.IsVillage && s.Culture?.StringId == "empire")
                            .OrderByDescending(s => s.Position.Y) // Y轴越大越靠南
                        .FirstOrDefault();
                    }

                    if (targetSettlement != null)
                    {
                        OriginLog.Info($"[SlaveEscape][Teleport] 找到帝国村子: {targetSettlement.Name} ({targetSettlement.StringId})");
                    }
                    else
                    {
                        OriginLog.Warning("[SlaveEscape][Teleport] 未找到帝国村子，将使用默认位置");
                    }
                }
                else if (direction == "steppe")
                {
                    // 寻找埃泽努尔村子（castle_village_K8_1）
                        targetSettlement = Campaign.Current?.Settlements?.FirstOrDefault(s => 
                        s.StringId == "castle_village_K8_1"); // Erzenur（埃泽努尔）
                        
                        if (targetSettlement != null)
                        {
                        OriginLog.Info($"[SlaveEscape][Teleport] 找到埃泽努尔村子: {targetSettlement.Name} ({targetSettlement.StringId})");
                    }
                    else
                    {
                        OriginLog.Warning("[SlaveEscape][Teleport] 未找到埃泽努尔村子 (castle_village_K8_1)，尝试通过名称查找");
                        // 如果找不到，尝试通过名称查找
                        targetSettlement = Campaign.Current?.Settlements?.FirstOrDefault(s => 
                            s.IsVillage && s.Culture?.StringId == "khuzait" &&
                            (s.Name.ToString().Contains("Erzenur") || s.Name.ToString().Contains("埃泽努尔")));
                        
                        if (targetSettlement != null)
                        {
                            OriginLog.Info($"[SlaveEscape][Teleport] 通过名称找到埃泽努尔村子: {targetSettlement.Name} ({targetSettlement.StringId})");
                        }
                    }
                }

                if (targetSettlement == null)
                {
                    OriginLog.Warning($"[SlaveEscape][Teleport] 未找到目标定居点 (direction: {direction})");
                    return false;
                }

                // 设置队伍位置到目标定居点附近
                // Settlement.Position 返回 CampaignVec2，必须使用 CampaignVec2（不能转换为 Vec2）
                var settlementPos = targetSettlement.Position;
                
                // Settlement 类型强制校验 + 详细坐标打印
                OriginLog.Info($"[SlaveEscape][Teleport] ========== 目标定居点信息 ==========");
                OriginLog.Info($"[SlaveEscape][Teleport] Settlement.StringId={targetSettlement.StringId}");
                OriginLog.Info($"[SlaveEscape][Teleport] Settlement.Name={targetSettlement.Name.ToString()}");
                OriginLog.Info($"[SlaveEscape][Teleport] Settlement.Culture={targetSettlement.Culture?.StringId ?? "null"}");
                OriginLog.Info($"[SlaveEscape][Teleport] Settlement.IsVillage={targetSettlement.IsVillage}");
                OriginLog.Info($"[SlaveEscape][Teleport] Settlement.Position type={settlementPos.GetType().FullName}");
                OriginLog.Info($"[SlaveEscape][Teleport] Settlement.Position=({settlementPos.X:F2},{settlementPos.Y:F2})");
                
                // 打印马凯布坐标用于对比（如果存在）
                var maKaibu = Campaign.Current?.Settlements?.FirstOrDefault(s => s.StringId == "town_KH1" || s.Name.ToString().Contains("马凯布") || s.Name.ToString().Contains("Makebu"));
                if (maKaibu != null)
                {
                    var maKaibuPos = maKaibu.Position;
                    OriginLog.Info($"[SlaveEscape][Teleport] [对比] 马凯布坐标: ({maKaibuPos.X:F2},{maKaibuPos.Y:F2})");
                }
                
                // 使用 CampaignVec2（不能使用 Vec2，因为 Position2D setter 需要 CampaignVec2）
                CampaignVec2 position = settlementPos;
                
                // 如果是"沙漠深处"方向，在村庄位置基础上再往南偏移（Y值增加）
                if (direction == "desert" && targetSettlement.IsVillage)
                {
                    // 往南偏移 2-3 个单位（Y值增加）
                var random = new Random();
                    float southOffset = 2.0f + (float)(random.NextDouble() * 1.0); // 2.0 到 3.0
                    // 使用 CampaignVec2 + Vec2 运算符
                    position = settlementPos + new TaleWorlds.Library.Vec2(0f, southOffset);
                    OriginLog.Info($"[SlaveEscape][Teleport] 沙漠深处：在村庄位置基础上往南偏移 {southOffset:F2} 单位");
                    OriginLog.Info($"[SlaveEscape][Teleport] 最终出生坐标: ({position.X:F2},{position.Y:F2})");
                }
                else
                {
                    // 其他方向：在定居点附近随机偏移一点，避免直接重叠
                    var random = new Random();
                    var randomOffset = new TaleWorlds.Library.Vec2(
                    (float)(random.NextDouble() * 2.0 - 1.0) * 0.5f,
                    (float)(random.NextDouble() * 2.0 - 1.0) * 0.5f
                );
                    // 使用 CampaignVec2 + Vec2 运算符
                    position = settlementPos + randomOffset;
                    OriginLog.Info($"[SlaveEscape][Teleport] 随机偏移: ({randomOffset.X:F2},{randomOffset.Y:F2})");
                    OriginLog.Info($"[SlaveEscape][Teleport] 最终出生坐标: ({position.X:F2},{position.Y:F2})");
                }

                // [SlaveEscape][Teleport] 日志：记录 teleport 前的状态
                Vec2 beforePos = Vec2.Invalid;
                var beforePosProp = typeof(MobileParty).GetProperty("Position2D", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (beforePosProp != null)
                {
                    beforePos = (Vec2)beforePosProp.GetValue(party);
                }
                OriginLog.Info($"[SlaveEscape][Teleport] when=OnCharacterCreationFinalized before=({beforePos.X:F2},{beforePos.Y:F2}) targetSettlement={targetSettlement.StringId} targetName={targetSettlement.Name}");

                // 尝试多种方法设置位置（在 finalize 时机，位置可能还没初始化）
                // 如果失败，会在 OnTick 中重试
                bool teleportSuccess = false;
                try
                {
                    // 方法1：尝试 Position2D 属性（Public）
                    var positionProperty = typeof(MobileParty).GetProperty("Position2D", BindingFlags.Public | BindingFlags.Instance);
                    if (positionProperty != null)
                    {
                        var setMethod = positionProperty.GetSetMethod(true);
                        OriginLog.Info($"[SlaveEscape][Teleport] Position2D setter exists={setMethod != null} isPublic={setMethod?.IsPublic ?? false}");
                        
                        if (setMethod != null)
                        {
                            // 打印参数类型和实际传入类型（自证日志）
                            var paramType = setMethod.GetParameters()[0].ParameterType;
                            OriginLog.Info($"[SlaveEscape][Teleport] Position2D setter paramType={paramType.FullName}");
                            OriginLog.Info($"[SlaveEscape][Teleport] argType={position.GetType().FullName}");
                            
                            setMethod.Invoke(party, new object[] { position });
                            teleportSuccess = true;
                            OriginLog.Info($"[SlaveEscape][Teleport] 使用 Position2D setMethod.Invoke 设置位置: success=True");
                        }
                    }
                    
                    // 方法2：尝试 Position 属性（不带 2D）
                    if (!teleportSuccess)
                    {
                        var positionProperty2 = typeof(MobileParty).GetProperty("Position", BindingFlags.Public | BindingFlags.Instance);
                        if (positionProperty2 != null)
                        {
                            var setMethod2 = positionProperty2.GetSetMethod(true);
                            OriginLog.Info($"[SlaveEscape][Teleport] Position setter exists={setMethod2 != null}");
                            
                            if (setMethod2 != null)
                            {
                                // 打印参数类型和实际传入类型（自证日志）
                                var paramType2 = setMethod2.GetParameters()[0].ParameterType;
                                OriginLog.Info($"[SlaveEscape][Teleport] Position setter paramType={paramType2.FullName}");
                                OriginLog.Info($"[SlaveEscape][Teleport] argType={position.GetType().FullName}");
                                
                                setMethod2.Invoke(party, new object[] { position });
                                teleportSuccess = true;
                                OriginLog.Info($"[SlaveEscape][Teleport] 使用 Position setMethod.Invoke 设置位置: success=True");
                            }
                        }
                    }
                    
                    // 方法3：尝试反射字段（非公共）
                    if (!teleportSuccess)
                    {
                        var positionField = typeof(MobileParty).GetField("_position2D", BindingFlags.NonPublic | BindingFlags.Instance);
                        if (positionField == null)
                        {
                            positionField = typeof(MobileParty).GetField("_position", BindingFlags.NonPublic | BindingFlags.Instance);
                        }
                        if (positionField == null)
                        {
                            positionField = typeof(MobileParty).GetField("Position2D", BindingFlags.NonPublic | BindingFlags.Instance);
                        }
                        
                        if (positionField != null)
                        {
                            positionField.SetValue(party, position);
                            teleportSuccess = true;
                            OriginLog.Info($"[SlaveEscape][Teleport] 使用反射字段设置位置: success=True fieldName={positionField.Name}");
                        }
                    }
                    
                    // 如果所有方法都失败，记录警告（会在 OnTick 中重试）
                    if (!teleportSuccess)
                    {
                        OriginLog.Warning($"[SlaveEscape][Teleport] 所有设置位置的方法都失败，将在 OnTick 中重试");
                    }
                }
                catch (Exception ex)
                {
                    OriginLog.Error($"[SlaveEscape][Teleport] 设置位置时异常: {ex.Message} success=False");
                    OriginLog.Error($"[SlaveEscape][Teleport] StackTrace: {ex.StackTrace}");
                }

                // 验证设置是否成功
                Vec2 afterPos = Vec2.Invalid;
                if (beforePosProp != null)
                {
                    afterPos = (Vec2)beforePosProp.GetValue(party);
                }
                OriginLog.Info($"[SlaveEscape][Teleport] ========== 位置设置结果 ==========");
                OriginLog.Info($"[SlaveEscape][Teleport] 设置前坐标: ({beforePos.X:F2},{beforePos.Y:F2})");
                OriginLog.Info($"[SlaveEscape][Teleport] 期望坐标: ({position.X:F2},{position.Y:F2})");
                OriginLog.Info($"[SlaveEscape][Teleport] 设置后坐标: ({afterPos.X:F2},{afterPos.Y:F2})");
                OriginLog.Info($"[SlaveEscape][Teleport] 设置成功: {teleportSuccess}");
                OriginLog.Info($"[SlaveEscape][Teleport] ========================================");

                // 尝试设置 HomeSettlement（如果方法存在）
                try
                {
                    var setHomeSettlementMethod = typeof(MobileParty).GetMethod("SetHomeSettlement", BindingFlags.Public | BindingFlags.Instance);
                    if (setHomeSettlementMethod != null)
                    {
                        setHomeSettlementMethod.Invoke(party, new object[] { targetSettlement });
                        OriginLog.Info($"[SlaveEscape][Teleport] 已设置 HomeSettlement: {targetSettlement.Name}");
                    }
                    else
                    {
                        OriginLog.Info($"[SlaveEscape][Teleport] SetHomeSettlement 方法不存在，跳过");
                    }
                }
                catch (Exception ex)
                {
                    OriginLog.Warning($"[SlaveEscape][Teleport] 设置 HomeSettlement 失败: {ex.Message}");
                }

                return teleportSuccess;
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[SlaveEscape][Teleport] 设置出生位置时出错: {ex.Message}");
                OriginLog.Error($"[SlaveEscape][Teleport] StackTrace: {ex.StackTrace}");
                return false;
            }
        }

        #endregion

        #region 9. 草原自由民 (free_cossack)

        private static void ApplyFreeCossackOrigin(Hero hero, Clan clan, MobileParty party)
        {
            SetClanTier(clan, 1);

            var khuzaitKingdom = FindKingdom("kingdom_khuzait");
            if (khuzaitKingdom != null)
            {
                ChangeRelationAction.ApplyPlayerRelation(khuzaitKingdom.Leader, 0);
            }

            var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
            
            ApplyFreeCossackNode1(hero, selectedNodes);
            ApplyFreeCossackNode2(hero, selectedNodes);
            ApplyFreeCossackNode3(party, selectedNodes);
            ApplyFreeCossackNode4(hero, selectedNodes);

            AddGold(hero, 3000); // 基础金币：低档
            
            // 设置出生位置：库塞特村子（避免城市会卡住）
            if (MobileParty.MainParty != null && Campaign.Current != null)
            {
                SetPresetOriginStartingLocation(MobileParty.MainParty, "khuzait");
            }
        }

        private static void ApplyFreeCossackNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_cossack_arrival")) return;
            
            var arrival = nodes["khz_node_cossack_arrival"];
            switch (arrival)
            {
                case "fugitive":
                    // 逃离压迫：侦察+20
                    AddSkill(hero, "scouting", 20);
                    break;
                case "profit":
                    // 追求自由：魅力+20
                    AddSkill(hero, "charm", 20);
                    break;
                case "exile":
                    // 家族传统：领导力+20
                    AddSkill(hero, "leadership", 20);
                    break;
            }
        }

        private static void ApplyFreeCossackNode2(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_cossack_survive")) return;
            
            var survive = nodes["khz_node_cossack_survive"];
            switch (survive)
            {
                case "mercenary":
                    // 以雇佣为生：领导力+20，战术+20
                    AddSkill(hero, "leadership", 20);
                    AddSkill(hero, "tactics", 20);
                    break;
                case "hunt":
                    // 以劫掠为生：劫掠+30，金币+3000（低）
                    AddSkill(hero, "roguery", 30);
                    AddGold(hero, 3000);
                    break;
                case "smuggle":
                    // 以贸易为生：贸易+30，金币+3000（低）
                    AddSkill(hero, "trade", 30);
                    AddGold(hero, 3000);
                    break;
            }
        }

        private static void ApplyFreeCossackNode3(MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_cossack_band")) return;
            
            var band = nodes["khz_node_cossack_band"];
            switch (band)
            {
                case "mixed":
                    // 混编战帮：额外兵力：骑射手×8，轻骑兵×6，游牧战士×6，食物+30（大）
                    AddTroops(party, "khuzait_horse_archer", 8);
                    AddTroops(party, "khuzait_light_cavalry", 6);
                    AddTroops(party, "khuzait_nomad", 6);
                    AddFood(party, 30);
                    break;
                case "outcast":
                    // 以骑射手为主：额外兵力：骑射手×12，游牧战士×6，食物+20（中）
                    AddTroops(party, "khuzait_horse_archer", 12);
                    AddTroops(party, "khuzait_nomad", 6);
                    AddFood(party, 20);
                    break;
                case "elite":
                    // 以轻骑兵为主：额外兵力：轻骑兵×10，骑射手×6，食物+20（中）
                    AddTroops(party, "khuzait_light_cavalry", 10);
                    AddTroops(party, "khuzait_horse_archer", 6);
                    AddFood(party, 20);
                    break;
            }
        }

        private static void ApplyFreeCossackNode4(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_cossack_khanate")) return;
            
            var khanate = nodes["khz_node_cossack_khanate"];
            var khuzaitKingdom = FindKingdom("kingdom_khuzait");
            
            switch (khanate)
            {
                case "legitimacy":
                    // 寻求庇护：魅力+20
                    AddSkill(hero, "charm", 20);
                    break;
                case "independent":
                    // 保持自由：侦察+20
                    AddSkill(hero, "scouting", 20);
                    break;
                case "oppose":
                    // 建立自己的势力：领导力+30
                    AddSkill(hero, "leadership", 30);
                    break;
            }
        }

        #endregion

        #region 10. 东方旧部复仇者 (old_guard_avenger)

        private static void ApplyOldGuardAvengerOrigin(Hero hero, Clan clan, MobileParty party)
        {
            SetClanTier(clan, 2);

            var khuzaitKingdom = FindKingdom("kingdom_khuzait");
            if (khuzaitKingdom != null)
            {
                ChangeRelationAction.ApplyPlayerRelation(khuzaitKingdom.Leader, -30);
            }

            var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
            
            ApplyOldGuardAvengerNode1(hero, selectedNodes);
            ApplyOldGuardAvengerNode2(hero, selectedNodes);
            ApplyOldGuardAvengerNode3(party, selectedNodes);
            ApplyOldGuardAvengerNode4(hero, selectedNodes);

            AddGold(hero, 8000); // 基础金币：中档
            
            // 设置出生位置：库塞特村子（避免城市会卡住）
            if (MobileParty.MainParty != null && Campaign.Current != null)
            {
                SetPresetOriginStartingLocation(MobileParty.MainParty, "khuzait");
            }
        }

        private static void ApplyOldGuardAvengerNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_avenger_old_master")) return;
            
            var master = nodes["khz_node_avenger_old_master"];
            var khuzaitKingdom = FindKingdom("kingdom_khuzait");
            
            switch (master)
            {
                case "royal":
                    // 失去荣誉：魅力+20
                    AddSkill(hero, "charm", 20);
                    break;
                case "clan":
                    // 被背叛：领导力+30，与库塞特可汗关系-30
                    AddSkill(hero, "leadership", 30);
                    if (khuzaitKingdom != null)
                    {
                        ChangeRelationAction.ApplyPlayerRelation(khuzaitKingdom.Leader, -30);
                    }
                    break;
                case "general":
                    // 家族被灭：战术+30，士气+3
                    AddSkill(hero, "tactics", 30);
                    if (MobileParty.MainParty != null)
                    {
                        MobileParty.MainParty.RecentEventsMorale += 3;
                    }
                    break;
            }
        }

        private static void ApplyOldGuardAvengerNode2(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_avenger_enemy")) return;
            
            var enemy = nodes["khz_node_avenger_enemy"];
            var khuzaitKingdom = FindKingdom("kingdom_khuzait");
            
            switch (enemy)
            {
                case "clan":
                    // 某个氏族：战术+30，与目标氏族关系-70
                    AddSkill(hero, "tactics", 30);
                    if (khuzaitKingdom != null)
                    {
                        // 随机选择一个精英氏族作为目标
                        var targetClan = khuzaitKingdom.Clans.Where(c => c.Tier >= 3 && c.Leader != null && c.Leader != khuzaitKingdom.Leader).FirstOrDefault();
                        if (targetClan != null)
                        {
                            ChangeRelationAction.ApplyPlayerRelation(targetClan.Leader, -70);
                        }
                    }
                    break;
                case "khanate":
                    // 整个系统：领导力+30，与库塞特可汗关系-70
                    AddSkill(hero, "leadership", 30);
                    if (khuzaitKingdom != null)
                    {
                        ChangeRelationAction.ApplyPlayerRelation(khuzaitKingdom.Leader, -70);
                    }
                    break;
                case "neighbor":
                    // 某个个人：劫掠+30，与目标个人关系-70
                    AddSkill(hero, "roguery", 30);
                    // 随机选择一个贵族作为目标
                    var targetHero = OriginSystemHelper.GetAllHeroesSafe().Where(h => h.IsLord && h.Clan != null && h.Clan.Tier >= 3).FirstOrDefault();
                    if (targetHero != null)
                    {
                        ChangeRelationAction.ApplyPlayerRelation(targetHero, -70);
                    }
                    break;
            }
        }

        private static void ApplyOldGuardAvengerNode3(MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_avenger_veterans")) return;
            
            var veterans = nodes["khz_node_avenger_veterans"];
            switch (veterans)
            {
                case "archer":
                    // 以骑射手为主：额外兵力：骑射手×12，游牧战士×6，食物+20（中）
                    AddTroops(party, "khuzait_horse_archer", 12);
                    AddTroops(party, "khuzait_nomad", 6);
                    AddFood(party, 20);
                    break;
                case "infantry":
                    // 以轻骑兵为主：额外兵力：轻骑兵×10，骑射手×6，食物+20（中）
                    AddTroops(party, "khuzait_light_cavalry", 10);
                    AddTroops(party, "khuzait_horse_archer", 6);
                    AddFood(party, 20);
                    break;
                case "mixed":
                    // 混编战帮：额外兵力：骑射手×8，轻骑兵×6，游牧战士×6，食物+30（大）
                    AddTroops(party, "khuzait_horse_archer", 8);
                    AddTroops(party, "khuzait_light_cavalry", 6);
                    AddTroops(party, "khuzait_nomad", 6);
                    AddFood(party, 30);
                    break;
            }
        }

        private static void ApplyOldGuardAvengerNode4(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("khz_node_avenger_opening")) return;
            
            var opening = nodes["khz_node_avenger_opening"];
            var khuzaitKingdom = FindKingdom("kingdom_khuzait");
            
            switch (opening)
            {
                case "stealth":
                    // 暗杀和突袭：劫掠+30，侦察+20
                    AddSkill(hero, "roguery", 30);
                    AddSkill(hero, "scouting", 20);
                    break;
                case "public":
                    // 正面战斗：战术+30，领导力+20
                    AddSkill(hero, "tactics", 30);
                    AddSkill(hero, "leadership", 20);
                    break;
                case "ally":
                    // 政治手段：魅力+30
                    AddSkill(hero, "charm", 30);
                    break;
            }
        }

        #endregion

        #region 通用辅助方法

        /// <summary>
        /// 添加兵力到队伍（额外加成，基础部队为10人）
        /// </summary>
        private static void AddTroops(MobileParty party, string troopId, int count)
        {
            if (party == null)
            {
                OriginLog.Warning($"[AddTroops] party is null, troopId={troopId}, count={count}");
                return;
            }
            
            try
            {
                OriginLog.Info($"[AddTroops] 开始添加兵力: troopId={troopId}, count={count}, party.Name={party.Name?.ToString() ?? "null"}");
                
                var troop = CharacterObject.All.FirstOrDefault(c => c.StringId == troopId);
                if (troop != null)
                {
                    var beforeCount = party.MemberRoster.Count;
                    party.AddElementToMemberRoster(troop, count);
                    var afterCount = party.MemberRoster.Count;
                    OriginLog.Info($"[AddTroops] 成功添加额外兵力: {troopId} x{count}, 队伍人数: {beforeCount} -> {afterCount}");
                }
                else
                {
                    OriginLog.Warning($"[AddTroops] 未找到兵力模板: troopId={troopId}, 尝试搜索所有可用兵力...");
                    // 尝试查找相似的兵力
                    var similarTroops = CharacterObject.All.Where(c => 
                        c != null && c.StringId != null && 
                        (c.StringId.Contains(troopId) || troopId.Contains(c.StringId))).Take(5).ToList();
                    if (similarTroops.Any())
                    {
                        OriginLog.Warning($"[AddTroops] 找到相似兵力: {string.Join(", ", similarTroops.Select(t => t.StringId))}");
                    }
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[AddTroops] 添加兵力失败: troopId={troopId}, count={count}, error={ex.Message}");
                OriginLog.Error($"[AddTroops] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 添加伙伴NPC到玩家队伍
        /// </summary>
        private static void AddCompanion(string companionId)
        {
            if (string.IsNullOrEmpty(companionId))
            {
                OriginLog.Warning("[AddCompanion] companionId is null or empty");
                return;
            }
            
            OriginLog.Info($"[AddCompanion] 开始添加伙伴: companionId={companionId}");
            
            try
            {
                // 查找伙伴模板
                var companionTemplate = CharacterObject.All.FirstOrDefault(c => 
                    c != null && c.StringId == companionId && c.IsTemplate);
                
                if (companionTemplate == null)
                {
                    OriginLog.Warning($"[AddCompanion] 未找到伙伴模板: companionId={companionId}");
                    // 尝试查找所有库塞特伙伴模板
                    var khuzaitTemplates = CharacterObject.All.Where(c => 
                        c != null && c.StringId != null && 
                        c.StringId.Contains("spc_wanderer_khuzait") && 
                        c.IsTemplate).Take(5).ToList();
                    if (khuzaitTemplates.Any())
                    {
                        OriginLog.Warning($"[AddCompanion] 找到的库塞特伙伴模板: {string.Join(", ", khuzaitTemplates.Select(t => t.StringId))}");
                    }
                    return;
                }

                OriginLog.Info($"[AddCompanion] 找到伙伴模板: {companionId}, Name={companionTemplate.Name?.ToString() ?? "null"}");

                // 使用HeroCreator创建伙伴实例
                var heroCreatorType = Type.GetType("TaleWorlds.CampaignSystem.HeroCreator, TaleWorlds.CampaignSystem");
                if (heroCreatorType == null)
                {
                    OriginLog.Error("[AddCompanion] 未找到HeroCreator类型");
                    return;
                }

                var createSpecialHeroMethod = heroCreatorType.GetMethod("CreateSpecialHero", 
                    BindingFlags.Public | BindingFlags.Static, 
                    null, 
                    new[] { typeof(CharacterObject) }, 
                    null);

                if (createSpecialHeroMethod == null)
                {
                    OriginLog.Error("[AddCompanion] 未找到CreateSpecialHero方法");
                    return;
                }

                OriginLog.Info($"[AddCompanion] 调用CreateSpecialHero创建伙伴实例...");
                var companion = createSpecialHeroMethod.Invoke(null, new object[] { companionTemplate }) as Hero;
                if (companion == null)
                {
                    OriginLog.Error($"[AddCompanion] 创建伙伴失败: companionId={companionId}, CreateSpecialHero返回null");
                    return;
                }

                OriginLog.Info($"[AddCompanion] 伙伴创建成功: companionId={companionId}, hero.Name={companion.Name?.ToString() ?? "null"}, hero.StringId={companion.StringId ?? "null"}");

                // 将伙伴添加到玩家队伍
                if (MobileParty.MainParty == null)
                {
                    OriginLog.Warning("[AddCompanion] MainParty is null, 无法添加伙伴");
                    return;
                }

                if (companion == null)
                {
                    OriginLog.Warning("[AddCompanion] companion is null, 无法添加伙伴");
                    return;
                }

                var beforeHeroCount = MobileParty.MainParty.MemberRoster.Count;
                OriginLog.Info($"[AddCompanion] 添加前队伍人数: {beforeHeroCount}");

                // 使用AddHeroToParty方法添加Hero到队伍
                var addHeroToPartyMethod = typeof(MobileParty).GetMethod("AddHeroToParty", 
                    BindingFlags.Public | BindingFlags.Instance, 
                    null, 
                    new[] { typeof(Hero), typeof(int) }, 
                    null);
                
                if (addHeroToPartyMethod != null)
                {
                    OriginLog.Info($"[AddCompanion] 使用AddHeroToParty方法添加伙伴...");
                    addHeroToPartyMethod.Invoke(MobileParty.MainParty, new object[] { companion, 0 });
                    var afterHeroCount = MobileParty.MainParty.MemberRoster.Count;
                    OriginLog.Info($"[AddCompanion] 成功添加伙伴: {companionId} ({companion.Name}), 队伍人数: {beforeHeroCount} -> {afterHeroCount}");
                }
                else
                {
                    OriginLog.Warning("[AddCompanion] AddHeroToParty方法未找到，使用备用方案...");
                    // 备用方案：直接添加到队伍成员列表
                    MobileParty.MainParty.AddElementToMemberRoster(companion.CharacterObject, 1);
                    var afterHeroCount = MobileParty.MainParty.MemberRoster.Count;
                    OriginLog.Info($"[AddCompanion] 使用备用方案添加伙伴: {companionId} ({companion.Name}), 队伍人数: {beforeHeroCount} -> {afterHeroCount}");
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[AddCompanion] 添加伙伴失败: companionId={companionId}, error={ex.Message}");
                OriginLog.Error($"[AddCompanion] StackTrace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    OriginLog.Error($"[AddCompanion] InnerException: {ex.InnerException.Message}");
                }
            }
        }

        /// <summary>
        /// 添加技能点
        /// </summary>
        private static void AddSkill(Hero hero, string skillName, int points)
        {
            if (hero == null || hero.HeroDeveloper == null) return;
            
            try
            {
                // 技能名称映射（配置文件中的名称 -> 游戏中的技能ID）
                var skillIdMap = new Dictionary<string, string>
                {
                    { "onehand", "OneHanded" },
                    { "twohand", "TwoHanded" },
                    { "polearm", "Polearm" },
                    { "bow", "Bow" },
                    { "crossbow", "Crossbow" },
                    { "throwing", "Throwing" },
                    { "riding", "Riding" },
                    { "athletics", "Athletics" },
                    { "crafting", "Crafting" },
                    { "tactics", "Tactics" },
                    { "scouting", "Scouting" },
                    { "roguery", "Roguery" },
                    { "leadership", "Leadership" },
                    { "charm", "Charm" },
                    { "trade", "Trade" },
                    { "steward", "Steward" },
                    { "medicine", "Medicine" },
                    { "engineering", "Engineering" }
                };

                var skillId = skillIdMap.ContainsKey(skillName.ToLower()) ? skillIdMap[skillName.ToLower()] : skillName;
                
                var skill = MBObjectManager.Instance.GetObject<SkillObject>(skillId);
                if (skill != null)
                {
                    var beforeLevel = hero.GetSkillValue(skill);
                    // 添加技能经验（经验值需要根据实际游戏调整）
                    hero.HeroDeveloper.AddSkillXp(skill, points * 100, true, true);
                    var afterLevel = hero.GetSkillValue(skill);
                    OriginLog.Info($"[AddSkill] 成功添加技能: hero={hero.Name?.ToString() ?? "null"}, skill={skillName} ({skillId}), points={points}, level: {beforeLevel} -> {afterLevel}");
                }
                else
                {
                    OriginLog.Warning($"[AddSkill] 未找到技能: skillName={skillName}, skillId={skillId}");
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[AddSkill] 添加技能失败: hero={hero?.Name?.ToString() ?? "null"}, skillName={skillName}, points={points}, error={ex.Message}");
                OriginLog.Error($"[AddSkill] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 增加声望
        /// </summary>
        private static void GainRenown(Hero hero, int amount)
        {
            if (hero == null)
            {
                OriginLog.Warning("[GainRenown] hero is null");
                return;
            }
            
            if (hero.Clan == null)
            {
                OriginLog.Warning("[GainRenown] hero.Clan is null");
                return;
            }
            
            var beforeRenown = hero.Clan.Renown;
            OriginLog.Info($"[GainRenown] Before: hero={hero.Name?.ToString() ?? "null"}, clan={hero.Clan.Name?.ToString() ?? "null"}, renown={beforeRenown}, amount={amount}");
            
            // 方法1: 尝试使用 GainRenownAction.Apply
            bool success = false;
            try
            {
                var gainRenownType = Type.GetType("TaleWorlds.CampaignSystem.Actions.GainRenownAction, TaleWorlds.CampaignSystem");
                if (gainRenownType != null)
                {
                    var applyMethod = gainRenownType.GetMethod("Apply", BindingFlags.Public | BindingFlags.Static);
                    if (applyMethod != null)
                    {
                        applyMethod.Invoke(null, new object[] { hero, amount });
                        OriginLog.Info($"[GainRenown] GainRenownAction.Apply called successfully");
                        success = true;
                    }
                    else
                    {
                        OriginLog.Warning("[GainRenown] GainRenownAction.Apply method not found");
                    }
                }
                else
                {
                    OriginLog.Warning("[GainRenown] GainRenownAction type not found");
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[GainRenown] GainRenownAction.Apply failed: {ex.Message}");
                OriginLog.Error($"[GainRenown] StackTrace: {ex.StackTrace}");
            }
            
            // 方法2: 如果 GainRenownAction 失败，尝试直接设置 Clan.Renown
            if (!success)
            {
                try
                {
                    // 尝试通过反射直接设置 Clan.Renown 属性
                    var renownProperty = typeof(Clan).GetProperty("Renown", BindingFlags.Public | BindingFlags.Instance);
                    if (renownProperty != null && renownProperty.CanWrite)
                    {
                        var currentRenown = (float)renownProperty.GetValue(hero.Clan);
                        renownProperty.SetValue(hero.Clan, currentRenown + amount);
                        OriginLog.Info($"[GainRenown] Directly set Clan.Renown: {currentRenown} + {amount} = {currentRenown + amount}");
                        success = true;
                    }
                    else
                    {
                        // 尝试通过反射设置私有字段 _renown
                        var renownField = typeof(Clan).GetField("_renown", BindingFlags.NonPublic | BindingFlags.Instance);
                        if (renownField != null)
                        {
                            var currentRenown = (float)renownField.GetValue(hero.Clan);
                            renownField.SetValue(hero.Clan, currentRenown + amount);
                            OriginLog.Info($"[GainRenown] Directly set Clan._renown field: {currentRenown} + {amount} = {currentRenown + amount}");
                            success = true;
                        }
                        else
                        {
                            OriginLog.Warning("[GainRenown] Clan.Renown property and _renown field not found or not writable");
                        }
                    }
                }
                catch (Exception ex)
                {
                    OriginLog.Error($"[GainRenown] Directly setting Clan.Renown failed: {ex.Message}");
                    OriginLog.Error($"[GainRenown] StackTrace: {ex.StackTrace}");
                }
            }
            
            // 验证结果
            var afterRenown = hero.Clan.Renown;
            OriginLog.Info($"[GainRenown] After: renown={afterRenown}, expected={beforeRenown + amount}, success={success}");
            
            if (Math.Abs(afterRenown - (beforeRenown + amount)) > 0.1f)
            {
                OriginLog.Warning($"[GainRenown] Renown mismatch! Expected {beforeRenown + amount}, but got {afterRenown}");
            }
        }

        /// <summary>
        /// 添加食物
        /// </summary>
        private static void AddFood(MobileParty party, int amount)
        {
            if (party == null)
            {
                OriginLog.Warning($"[AddFood] party is null, amount={amount}");
                return;
            }
            
            try
            {
                OriginLog.Info($"[AddFood] 开始添加食物: amount={amount}, party.Name={party.Name?.ToString() ?? "null"}");
                
                var foodItem = MBObjectManager.Instance.GetObject<ItemObject>("grain");
                if (foodItem != null)
                {
                    var beforeCount = party.ItemRoster.GetItemNumber(foodItem);
                    party.ItemRoster.AddToCounts(foodItem, amount);
                    var afterCount = party.ItemRoster.GetItemNumber(foodItem);
                    OriginLog.Info($"[AddFood] 成功添加食物: amount={amount}, 食物数量: {beforeCount} -> {afterCount}");
                }
                else
                {
                    OriginLog.Warning("[AddFood] 未找到食物物品: grain");
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[AddFood] 添加食物失败: amount={amount}, error={ex.Message}");
                OriginLog.Error($"[AddFood] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 添加马匹
        /// </summary>
        private static void AddMounts(MobileParty party, int count)
        {
            if (party == null)
            {
                OriginLog.Warning($"[AddMounts] party is null, count={count}");
                return;
            }
            
            try
            {
                OriginLog.Info($"[AddMounts] 开始添加马匹: count={count}, party.Name={party.Name?.ToString() ?? "null"}");
                
                var mountItem = MBObjectManager.Instance.GetObject<ItemObject>("sumpter_horse");
                if (mountItem != null)
                {
                    var beforeCount = party.ItemRoster.GetItemNumber(mountItem);
                    party.ItemRoster.AddToCounts(mountItem, count);
                    var afterCount = party.ItemRoster.GetItemNumber(mountItem);
                    OriginLog.Info($"[AddMounts] 成功添加马匹: count={count}, 马匹数量: {beforeCount} -> {afterCount}");
                }
                else
                {
                    OriginLog.Warning("[AddMounts] 未找到马匹物品: sumpter_horse");
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[AddMounts] 添加马匹失败: count={count}, error={ex.Message}");
                OriginLog.Error($"[AddMounts] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 添加金币（带日志）
        /// </summary>
        private static void AddGold(Hero hero, int amount)
        {
            if (hero == null)
            {
                OriginLog.Warning($"[AddGold] hero is null, amount={amount}");
                return;
            }
            
            try
            {
                var beforeGold = hero.Gold;
                OriginLog.Info($"[AddGold] 开始添加金币: hero={hero.Name?.ToString() ?? "null"}, amount={amount}, beforeGold={beforeGold}");
                
                GiveGoldAction.ApplyBetweenCharacters(null, hero, amount, false);
                
                var afterGold = hero.Gold;
                OriginLog.Info($"[AddGold] 成功添加金币: amount={amount}, gold: {beforeGold} -> {afterGold}");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[AddGold] 添加金币失败: hero={hero?.Name?.ToString() ?? "null"}, amount={amount}, error={ex.Message}");
                OriginLog.Error($"[AddGold] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 添加特质（带日志）
        /// </summary>
        private static void AddTrait(Hero hero, string traitName, int level)
        {
            if (hero == null)
            {
                OriginLog.Warning($"[AddTrait] hero is null, traitName={traitName}, level={level}");
                return;
            }
            
            try
            {
                OriginLog.Info($"[AddTrait] 开始添加特质: hero={hero.Name?.ToString() ?? "null"}, traitName={traitName}, level={level}");
                
                var traitType = Type.GetType("TaleWorlds.CampaignSystem.TraitObject, TaleWorlds.CampaignSystem");
                if (traitType == null)
                {
                    OriginLog.Warning("[AddTrait] TraitObject type not found");
                    return;
                }

                var getObjectMethod = typeof(MBObjectManager).GetMethod("GetObject", new[] { typeof(string) });
                if (getObjectMethod == null)
                {
                    OriginLog.Warning("[AddTrait] MBObjectManager.GetObject method not found");
                    return;
                }

                var genericMethod = getObjectMethod.MakeGenericMethod(traitType);
                var traitObj = genericMethod?.Invoke(MBObjectManager.Instance, new object[] { traitName });
                
                if (traitObj != null)
                {
                    // 获取当前特质值
                    var getTraitMethod = typeof(Hero).GetMethod("GetTrait", BindingFlags.Public | BindingFlags.Instance);
                    var beforeValue = 0;
                    if (getTraitMethod != null)
                    {
                        beforeValue = (int)getTraitMethod.Invoke(hero, new object[] { traitObj });
                    }
                    
                    var setTraitMethod = typeof(Hero).GetMethod("SetTraitLevel", BindingFlags.Public | BindingFlags.Instance);
                    if (setTraitMethod != null)
                    {
                        setTraitMethod.Invoke(hero, new object[] { traitObj, level });
                        var afterValue = getTraitMethod != null ? (int)getTraitMethod.Invoke(hero, new object[] { traitObj }) : level;
                        OriginLog.Info($"[AddTrait] 成功添加特质: traitName={traitName}, level={level}, value: {beforeValue} -> {afterValue}");
                    }
                    else
                    {
                        OriginLog.Warning("[AddTrait] Hero.SetTraitLevel method not found");
                    }
                }
                else
                {
                    OriginLog.Warning($"[AddTrait] 未找到特质: traitName={traitName}");
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[AddTrait] 添加特质失败: hero={hero?.Name?.ToString() ?? "null"}, traitName={traitName}, level={level}, error={ex.Message}");
                OriginLog.Error($"[AddTrait] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 修改关系（带日志）
        /// </summary>
        private static void ChangeRelation(Hero targetHero, int changeAmount)
        {
            if (targetHero == null)
            {
                OriginLog.Warning($"[ChangeRelation] targetHero is null, changeAmount={changeAmount}");
                return;
            }
            
            try
            {
                var beforeRelation = Hero.MainHero?.GetRelation(targetHero) ?? 0;
                OriginLog.Info($"[ChangeRelation] 开始修改关系: targetHero={targetHero.Name?.ToString() ?? "null"}, changeAmount={changeAmount}, beforeRelation={beforeRelation}");
                
                ChangeRelationAction.ApplyPlayerRelation(targetHero, changeAmount);
                
                var afterRelation = Hero.MainHero?.GetRelation(targetHero) ?? 0;
                OriginLog.Info($"[ChangeRelation] 成功修改关系: changeAmount={changeAmount}, relation: {beforeRelation} -> {afterRelation}");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ChangeRelation] 修改关系失败: targetHero={targetHero?.Name?.ToString() ?? "null"}, changeAmount={changeAmount}, error={ex.Message}");
                OriginLog.Error($"[ChangeRelation] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 设置家族等级
        /// </summary>
        private static void SetClanTier(Clan clan, int tier)
        {
            if (clan == null)
            {
                OriginLog.Warning($"[SetClanTier] clan is null, tier={tier}");
                return;
            }
            
            try
            {
                var beforeTier = clan.Tier;
                OriginLog.Info($"[SetClanTier] 开始设置家族等级: clan={clan.Name?.ToString() ?? "null"}, tier={tier}, beforeTier={beforeTier}");
                
                var tierProperty = typeof(Clan).GetProperty("Tier", BindingFlags.Public | BindingFlags.Instance);
                if (tierProperty != null && tierProperty.CanWrite)
                {
                    tierProperty.SetValue(clan, tier);
                    OriginLog.Info($"[SetClanTier] 通过Tier属性设置成功: tier={tier}");
                }
                else
                {
                    var changeClanTierType = Type.GetType("TaleWorlds.CampaignSystem.Actions.ChangeClanTierAction, TaleWorlds.CampaignSystem");
                    if (changeClanTierType != null)
                    {
                        var applyMethod = changeClanTierType.GetMethod("Apply", BindingFlags.Public | BindingFlags.Static);
                        if (applyMethod != null)
                        {
                            applyMethod.Invoke(null, new object[] { clan, tier });
                            OriginLog.Info($"[SetClanTier] 使用ChangeClanTierAction设置成功: tier={tier}");
                        }
                        else
                        {
                            OriginLog.Warning("[SetClanTier] ChangeClanTierAction.Apply method not found");
                        }
                    }
                    else
                    {
                        OriginLog.Warning("[SetClanTier] ChangeClanTierAction type not found");
                    }
                }
                
                var afterTier = clan.Tier;
                OriginLog.Info($"[SetClanTier] 设置完成: tier: {beforeTier} -> {afterTier}");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[SetClanTier] 设置家族等级失败: clan={clan?.Name?.ToString() ?? "null"}, tier={tier}, error={ex.Message}");
                OriginLog.Error($"[SetClanTier] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 设置家族为贵族家族
        /// </summary>
        private static void SetClanNoble(Clan clan, bool isNoble)
        {
            if (clan == null)
            {
                OriginLog.Warning("[SetClanNoble] clan 为空");
                return;
            }

            try
            {
                // 方法1: 尝试直接设置 IsNoble 属性
                var isNobleProperty = typeof(Clan).GetProperty("IsNoble", BindingFlags.Public | BindingFlags.Instance);
                if (isNobleProperty != null && isNobleProperty.CanWrite)
                {
                    isNobleProperty.SetValue(clan, isNoble);
                    OriginLog.Info($"[SetClanNoble] 通过 IsNoble 属性设置成功: {isNoble}");
                    Debug.Print($"[OriginSystem] 家族贵族状态已设置为: {isNoble}", 0, Debug.DebugColor.Green);
                    return;
                }
                else
                {
                    OriginLog.Warning($"[SetClanNoble] IsNoble 属性不可写 (CanWrite={isNobleProperty?.CanWrite ?? false})");
                }

                // 方法2: 尝试设置私有字段 _isNoble
                var isNobleField = typeof(Clan).GetField("_isNoble", BindingFlags.NonPublic | BindingFlags.Instance);
                if (isNobleField != null)
                {
                    isNobleField.SetValue(clan, isNoble);
                    OriginLog.Info($"[SetClanNoble] 通过 _isNoble 字段设置成功: {isNoble}");
                    Debug.Print($"[OriginSystem] 家族贵族状态已设置为: {isNoble}", 0, Debug.DebugColor.Green);
                    return;
                }
                else
                {
                    OriginLog.Warning("[SetClanNoble] 未找到 _isNoble 字段");
                }

                // 方法3: 尝试使用 ChangeClanNobleAction（如果存在）
                var changeClanNobleActionType = Type.GetType("TaleWorlds.CampaignSystem.Actions.ChangeClanNobleAction, TaleWorlds.CampaignSystem");
                if (changeClanNobleActionType != null)
                {
                    var applyMethod = changeClanNobleActionType.GetMethod("Apply", BindingFlags.Public | BindingFlags.Static);
                    if (applyMethod != null)
                    {
                        applyMethod.Invoke(null, new object[] { clan, isNoble });
                        OriginLog.Info($"[SetClanNoble] 通过 ChangeClanNobleAction 设置成功: {isNoble}");
                        Debug.Print($"[OriginSystem] 家族贵族状态已设置为: {isNoble}", 0, Debug.DebugColor.Green);
                        return;
                    }
                }

                // 方法4: 通过设置家族等级来间接设置（Tier >= 2 通常被视为贵族）
                // 注意：这个方法已经在 SetClanTier 中处理了，但我们可以确保等级足够高
                if (isNoble && clan.Tier < 2)
                {
                    OriginLog.Info($"[SetClanNoble] 尝试通过提升家族等级来设置贵族身份（当前等级: {clan.Tier}）");
                    SetClanTier(clan, Math.Max(2, clan.Tier));
                }

                OriginLog.Warning($"[SetClanNoble] 所有设置方法都失败，当前 IsNoble={clan.IsNoble}, Tier={clan.Tier}");
                Debug.Print($"[OriginSystem] 设置家族贵族状态失败: 无法找到可用的设置方法", 0, Debug.DebugColor.Yellow);
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[SetClanNoble] 设置失败: {ex.Message}");
                OriginLog.Error($"[SetClanNoble] StackTrace: {ex.StackTrace}");
                Debug.Print($"[OriginSystem] 设置家族贵族状态失败: {ex.Message}", 0, Debug.DebugColor.Red);
            }
        }

        /// <summary>
        /// 设置家族为雇佣兵家族
        /// </summary>
        private static void SetClanMercenary(Clan clan, bool isMercenary)
        {
            try
            {
                var isMinorFactionProperty = typeof(Clan).GetProperty("IsMinorFaction", BindingFlags.Public | BindingFlags.Instance);
                if (isMinorFactionProperty != null && isMinorFactionProperty.CanWrite)
                {
                    isMinorFactionProperty.SetValue(clan, isMercenary);
                }

                var isMercenaryProperty = typeof(Clan).GetProperty("IsMercenary", BindingFlags.Public | BindingFlags.Instance);
                if (isMercenaryProperty != null && isMercenaryProperty.CanWrite)
                {
                    isMercenaryProperty.SetValue(clan, isMercenary);
                }

                Debug.Print($"[OriginSystem] 家族雇佣兵状态已设置为: {isMercenary}", 0, Debug.DebugColor.Green);
            }
            catch (Exception ex)
            {
                Debug.Print($"[OriginSystem] 设置家族雇佣兵状态失败: {ex.Message}", 0, Debug.DebugColor.Red);
            }
        }

        /// <summary>
        /// 查找王国
        /// </summary>
        public static Kingdom FindKingdom(string kingdomId)
        {
            try
            {
                var kingdoms = Campaign.Current?.Kingdoms;
                if (kingdoms == null) return null;

                // 1) 精确匹配
                var k = kingdoms.FirstOrDefault(x => x.StringId == kingdomId);
                if (k != null) return k;

                // 2) 兼容 "kingdom_" 前缀：kingdom_aserai -> aserai
                if (!string.IsNullOrEmpty(kingdomId) && kingdomId.StartsWith("kingdom_"))
                {
                    var alt = kingdomId.Substring("kingdom_".Length);
                    k = kingdoms.FirstOrDefault(x => x.StringId == alt);
                    if (k != null) return k;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 添加特性（旧版本，保留兼容性）
        /// </summary>
        private static void AddTraitOld(Hero hero, string traitId, int value)
        {
            // 调用新的AddTrait方法（带日志）
            AddTrait(hero, traitId, value);
        }

        #endregion
    }
}
