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
    /// Minimal, compile-safe preset origin system.
    /// Keep public API stable; move detailed origin logic into separate files later.
    /// </summary>
    public static class PresetOriginSystem
    {
        /// <summary>
        /// 应用预设出身。返回是否成功执行（命中分支且未因空对象早退）。
        /// </summary>
        public static bool ApplyPresetOrigin(string originId)
        {
            try
            {
                OriginLog.Info($"[ApplyPresetOrigin] originId={originId ?? "null"}");
                Debug.Print($"[OriginSystem] [ApplyPresetOrigin] originId={originId ?? "null"}", 0, Debug.DebugColor.Green);

                if (string.IsNullOrWhiteSpace(originId))
                {
                    OriginLog.Warning("[ApplyPresetOrigin] originId is null/empty, skip.");
                    return false;
                }

                var hero = Hero.MainHero;
                var clan = Clan.PlayerClan;
                var party = MobileParty.MainParty;

                if (hero == null || clan == null || party == null)
                {
                    OriginLog.Warning("[ApplyPresetOrigin] Hero/Clan/Party is null, cannot apply origin.");
                    return false;
                }

                // 瓦兰迪亚出身
                if (originId == "vlandia_expedition_knight" || originId.Contains("expedition_knight"))
                {
                    OriginLog.Info($"[ApplyPresetOrigin] 调用远征骑士出身逻辑 (originId={originId})");
                    VlandiaOriginSystem.ApplyExpeditionKnightOrigin(hero, clan, party);
                    return true;
                }

                // 其他出身...
                OriginLog.Info($"[ApplyPresetOrigin] Processing originId={originId} (logic to be migrated)");
                return true; // 已处理（占位）
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyPresetOrigin] Exception: {ex}");
                return false;
            }
        }

        public static void JoinKhuzaitAsNoble()
        {
            try
            {
                if (Campaign.Current == null || Hero.MainHero == null || Clan.PlayerClan == null)
                {
                    OriginLog.Warning("[JoinKhuzaitAsNoble] Campaign/MainHero/PlayerClan is null, abort.");
                    return;
                }

                var khuzait = FindKingdom("khuzait");
                OriginLog.Info($"[JoinKhuzaitAsNoble] khuzait kingdom={(khuzait != null ? khuzait.StringId : "null")}");

                // Ensure occupation is Lord (some versions allow direct call; if not, swallow safely)
                try
                {
                    if (!Hero.MainHero.IsLord)
                        Hero.MainHero.SetNewOccupation(Occupation.Lord);
                }
                catch (Exception exOcc)
                {
                    OriginLog.Warning($"[JoinKhuzaitAsNoble] SetNewOccupation failed (safe to ignore): {exOcc.Message}");
                }

                // Join kingdom if not already in one
                if (khuzait != null && Clan.PlayerClan.Kingdom == null)
                {
                    ChangeKingdomAction.ApplyByJoinToKingdom(Clan.PlayerClan, khuzait, CampaignTime.Now, false);
                    OriginLog.Info("[JoinKhuzaitAsNoble] Player clan joined khuzait kingdom.");
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[JoinKhuzaitAsNoble] Exception: {ex}");
            }
        }

        public static void CheckPlayerClanStatus()
        {
            try
            {
                var clan = Clan.PlayerClan;
                var hero = Hero.MainHero;

                OriginLog.Info(
                    $"[CheckPlayerClanStatus] clan={(clan != null ? clan.StringId : "null")}, " +
                    $"kingdom={(clan?.Kingdom != null ? clan.Kingdom.StringId : "null")}, " +
                    $"isLord={(hero != null && hero.IsLord)}"
                );
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[CheckPlayerClanStatus] Exception: {ex}");
            }
        }

        public static void EnsurePlayerClanIsNoble()
        {
            try
            {
                // Keep this as a safe no-op for now. You can add tier/renown adjustments later.
                if (Clan.PlayerClan == null)
                {
                    OriginLog.Warning("[EnsurePlayerClanIsNoble] PlayerClan is null.");
                    return;
                }

                OriginLog.Info("[EnsurePlayerClanIsNoble] No-op (compile-safe).");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[EnsurePlayerClanIsNoble] Exception: {ex}");
            }
        }

        public static bool SetPresetOriginStartingLocation(MobileParty party, string cultureId, string settlementId = null)
        {
            try
            {
                if (party == null || Campaign.Current == null)
                {
                    OriginLog.Warning("[PresetOrigin][Teleport] party/Campaign is null.");
                    return false;
                }

                Settlement target = null;

                // 优先使用指定的 settlementId（参考奴隶逃亡的实现）
                if (!string.IsNullOrWhiteSpace(settlementId))
                {
                    target = Campaign.Current.Settlements.FirstOrDefault(s => s != null && s.StringId == settlementId);
                    if (target != null)
                    {
                        OriginLog.Info($"[PresetOrigin][Teleport] 通过 settlementId 找到目标: {target.StringId} ({target.Name})");
                    }
                }

                // 如果 settlementId 未指定或未找到，按文化查找
                if (target == null)
                {
                    // 特殊处理：诺德文化（sturgia）- 尝试查找拉维凯尔（在《战帆》DLC中）
                    if (cultureId == "sturgia" || cultureId == "nord")
                    {
                        // 首先尝试通过名称查找拉维凯尔（支持DLC中的新定居点）
                        // 拉维凯尔（Ravikail）在《战帆》DLC中
                        target = Campaign.Current.Settlements.FirstOrDefault(s => 
                            s != null && s.IsVillage && 
                            (s.Name.ToString().Contains("拉维凯尔") || 
                             s.Name.ToString().Contains("Ravikail") ||
                             s.StringId.Contains("ravikail") ||
                             s.StringId.Contains("Ravikail")));
                        
                        if (target != null)
                        {
                            OriginLog.Info($"[PresetOrigin][Teleport] 通过名称找到拉维凯尔: {target.StringId} ({target.Name})");
                        }
                        
                        if (target == null)
                        {
                            // 如果找不到拉维凯尔，优先查找Sturgia文化的村子
                            target = Campaign.Current.Settlements
                                .Where(s => s != null && s.IsVillage && s.Culture != null && s.Culture.StringId == "sturgia")
                                .FirstOrDefault();
                        }
                        
                        if (target == null)
                        {
                            // 如果找不到，尝试通过StringId查找已知的诺德村子
                            target = Campaign.Current.Settlements.FirstOrDefault(s => 
                                s != null && s.IsVillage && 
                                (s.StringId.Contains("sturgia") || s.StringId.Contains("nord")));
                        }
                    }
                    else
                    {
                        // 其他文化正常查找
                        target = Campaign.Current.Settlements
                            .Where(s => s != null && s.IsVillage && s.Culture != null && s.Culture.StringId == cultureId)
                            .FirstOrDefault();
                    }
                }

                // fallback: any village
                if (target == null)
                    target = Campaign.Current.Settlements.FirstOrDefault(s => s != null && s.IsVillage);

                if (target == null)
                {
                    OriginLog.Warning($"[PresetOrigin][Teleport] No settlement found for cultureId={cultureId ?? "null"}, settlementId={settlementId ?? "null"}.");
                    return false;
                }

                // small random offset to avoid overlap
                var rnd = new Random();
                var offset = new Vec2(
                    (float)(rnd.NextDouble() * 2.0 - 1.0) * 0.5f,
                    (float)(rnd.NextDouble() * 2.0 - 1.0) * 0.5f
                );

                // Use Settlement.Position (CampaignVec2) and convert to Vec2
                var settlementPos = target.Position;
                var pos = new Vec2(settlementPos.X, settlementPos.Y) + offset;
                OriginLog.Info($"[PresetOrigin][Teleport] target={target.StringId} ({target.Name}), pos=({pos.X:F2},{pos.Y:F2})");

                return SetPartyPositionSafe(party, pos);
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[PresetOrigin][Teleport] Exception: {ex}");
                return false;
            }
        }

        public static bool SetSlaveEscapeStartingLocation(MobileParty party, string direction, string settlementId = null)
        {
            try
            {
                if (party == null || Campaign.Current == null)
                {
                    OriginLog.Warning("[SlaveEscape][Teleport] party/Campaign is null.");
                    return false;
                }

                Settlement target = null;

                if (!string.IsNullOrWhiteSpace(settlementId))
                {
                    target = Campaign.Current.Settlements.FirstOrDefault(s => s != null && s.StringId == settlementId);
                }

                if (target == null)
                {
                    // Very safe fallback logic: choose a village by "direction" or just first village.
                    string culture = null;
                    if (direction == "desert")
                        culture = "aserai";
                    else if (direction == "steppe")
                        culture = "khuzait";
                    else if (direction == "empire")
                        culture = "empire";

                    if (!string.IsNullOrWhiteSpace(culture))
                    {
                        target = Campaign.Current.Settlements
                            .Where(s => s != null && s.IsVillage && s.Culture != null && s.Culture.StringId == culture)
                            .FirstOrDefault();
                    }

                    if (target == null)
                        target = Campaign.Current.Settlements.FirstOrDefault(s => s != null && s.IsVillage);
                }

                if (target == null)
                {
                    OriginLog.Warning("[SlaveEscape][Teleport] No valid settlement found.");
                    return false;
                }

                var rnd = new Random();
                var offset = new Vec2(
                    (float)(rnd.NextDouble() * 2.0 - 1.0) * 0.6f,
                    (float)(rnd.NextDouble() * 2.0 - 1.0) * 0.6f
                );

                // Use Settlement.Position (CampaignVec2) and convert to Vec2
                var settlementPos = target.Position;
                var pos = new Vec2(settlementPos.X, settlementPos.Y) + offset;
                OriginLog.Info($"[SlaveEscape][Teleport] direction={direction ?? "null"}, target={target.StringId}, pos=({pos.X:F2},{pos.Y:F2})");

                return SetPartyPositionSafe(party, pos);
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[SlaveEscape][Teleport] Exception: {ex}");
                return false;
            }
        }

        public static Kingdom FindKingdom(string kingdomId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(kingdomId))
                    return null;

                // Prefer Campaign.Current if available
                var k1 = Campaign.Current?.Kingdoms?.FirstOrDefault(k => k != null && k.StringId == kingdomId);
                if (k1 != null) return k1;

                // Fallback to global list if available in your version
                return Kingdom.All?.FirstOrDefault(k => k != null && k.StringId == kingdomId);
            }
            catch (Exception ex)
            {
                OriginLog.Warning($"[FindKingdom] Exception: {ex.Message}");
                return null;
            }
        }

        private static bool SetPartyPositionSafe(MobileParty party, Vec2 position)
        {
            try
            {
                // Use reflection to avoid access issues across Bannerlord versions.
                var prop = typeof(MobileParty).GetProperty("Position2D",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(party, position);
                    return true;
                }

                // Try private fields
                var field = typeof(MobileParty).GetField("_position2D",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                if (field != null)
                {
                    field.SetValue(party, position);
                    return true;
                }

                OriginLog.Warning("[Teleport] Cannot set position (no writable prop/field found).");
                return false;
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[Teleport] Set position failed: {ex.Message}");
                return false;
            }
        }

        #region 辅助方法

        /// <summary>
        /// 设置家族等级
        /// </summary>
        public static void SetClanTier(Clan clan, int tier)
        {
            if (clan == null)
            {
                OriginLog.Warning($"[SetClanTier] clan is null, tier={tier}");
                return;
            }
            
            try
            {
                var beforeTier = clan.Tier;
                OriginLog.Info($"[SetClanTier] 开始设置家族等级 clan={clan.Name?.ToString() ?? "null"}, tier={tier}, beforeTier={beforeTier}");
                
                var tierProperty = typeof(Clan).GetProperty("Tier", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (tierProperty != null && tierProperty.CanWrite)
                {
                    tierProperty.SetValue(clan, tier);
                    OriginLog.Info($"[SetClanTier] 通过Tier属性设置成功 tier={tier}");
                }
                else
                {
                    var changeClanTierType = Type.GetType("TaleWorlds.CampaignSystem.Actions.ChangeClanTierAction, TaleWorlds.CampaignSystem");
                    if (changeClanTierType != null)
                    {
                        var applyMethod = changeClanTierType.GetMethod("Apply", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                        if (applyMethod != null)
                        {
                            applyMethod.Invoke(null, new object[] { clan, tier });
                            OriginLog.Info($"[SetClanTier] 使用ChangeClanTierAction设置成功 tier={tier}");
                        }
                    }
                }
                
                var afterTier = clan.Tier;
                OriginLog.Info($"[SetClanTier] 设置完成: tier: {beforeTier} -> {afterTier}");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[SetClanTier] 设置家族等级失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 增加声望
        /// </summary>
        public static void GainRenown(Hero hero, int amount)
        {
            if (hero == null || hero.Clan == null)
            {
                OriginLog.Warning("[GainRenown] hero or hero.Clan is null");
                return;
            }
            
            try
            {
                var beforeRenown = hero.Clan.Renown;
                OriginLog.Info($"[GainRenown] Before: hero={hero.Name?.ToString() ?? "null"}, clan={hero.Clan.Name?.ToString() ?? "null"}, renown={beforeRenown}, amount={amount}");
                
                bool success = false;
                var gainRenownType = Type.GetType("TaleWorlds.CampaignSystem.Actions.GainRenownAction, TaleWorlds.CampaignSystem");
                if (gainRenownType != null)
                {
                    var applyMethod = gainRenownType.GetMethod("Apply", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                    if (applyMethod != null)
                    {
                        applyMethod.Invoke(null, new object[] { hero, amount });
                        OriginLog.Info($"[GainRenown] GainRenownAction.Apply called successfully");
                        success = true;
                    }
                }
                
                if (!success)
                {
                    var renownProperty = typeof(Clan).GetProperty("Renown", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    if (renownProperty != null && renownProperty.CanWrite)
                    {
                        var currentRenown = (float)renownProperty.GetValue(hero.Clan);
                        renownProperty.SetValue(hero.Clan, currentRenown + amount);
                        OriginLog.Info($"[GainRenown] Directly set Clan.Renown: {currentRenown} + {amount} = {currentRenown + amount}");
                        success = true;
                    }
                    else
                    {
                        var renownField = typeof(Clan).GetField("_renown", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                        if (renownField != null)
                        {
                            var currentRenown = (float)renownField.GetValue(hero.Clan);
                            renownField.SetValue(hero.Clan, currentRenown + amount);
                            OriginLog.Info($"[GainRenown] Directly set Clan._renown field: {currentRenown} + {amount} = {currentRenown + amount}");
                            success = true;
                        }
                    }
                }
                
                var afterRenown = hero.Clan.Renown;
                OriginLog.Info($"[GainRenown] After: renown={afterRenown}, expected={beforeRenown + amount}, success={success}");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[GainRenown] Exception: {ex.Message}");
            }
        }

        /// <summary>
        /// 添加金币
        /// </summary>
        public static void AddGold(Hero hero, int amount)
        {
            if (hero == null)
            {
                OriginLog.Warning($"[AddGold] hero is null, amount={amount}");
                return;
            }
            
            try
            {
                var beforeGold = hero.Gold;
                OriginLog.Info($"[AddGold] 开始添加金币 hero={hero.Name?.ToString() ?? "null"}, amount={amount}, beforeGold={beforeGold}");
                
                GiveGoldAction.ApplyBetweenCharacters(null, hero, amount, false);
                
                var afterGold = hero.Gold;
                OriginLog.Info($"[AddGold] 成功添加金币: amount={amount}, gold: {beforeGold} -> {afterGold}");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[AddGold] 添加金币失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 添加技能
        /// </summary>
        public static void AddSkill(Hero hero, string skillName, int points)
        {
            if (hero == null || hero.HeroDeveloper == null) return;
            
            try
            {
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
                    hero.HeroDeveloper.AddSkillXp(skill, points * 100, true, true);
                    var afterLevel = hero.GetSkillValue(skill);
                    OriginLog.Info($"[AddSkill] 成功添加技能 hero={hero.Name?.ToString() ?? "null"}, skill={skillName} ({skillId}), points={points}, level: {beforeLevel} -> {afterLevel}");
                }
                else
                {
                    OriginLog.Warning($"[AddSkill] 未找到技能 skillName={skillName}, skillId={skillId}");
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[AddSkill] 添加技能失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 设置犯罪度
        /// </summary>
        public static void SetCrimeRating(Kingdom kingdom, int rating)
        {
            try
            {
                if (kingdom == null || Campaign.Current == null)
                {
                    OriginLog.Warning($"[SetCrimeRating] 王国或Campaign为空，无法设置犯罪度");
                    return;
                }

                var campaignType = typeof(Campaign);
                var setCrimeRatingMethod = campaignType.GetMethod("SetCrimeRating", 
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
                
                if (setCrimeRatingMethod != null)
                {
                    var parameters = setCrimeRatingMethod.GetParameters();
                    if (parameters.Length == 2)
                    {
                        if (parameters[0].ParameterType == typeof(int))
                        {
                            setCrimeRatingMethod.Invoke(null, new object[] { rating, kingdom });
                        }
                        else
                        {
                            setCrimeRatingMethod.Invoke(null, new object[] { kingdom, rating });
                        }
                        OriginLog.Info($"[SetCrimeRating] 成功设置 {kingdom.Name} 犯罪度为 {rating}");
                        return;
                    }
                }

                OriginLog.Warning($"[SetCrimeRating] 无法找到设置犯罪度的方法");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[SetCrimeRating] 设置犯罪度失败: {ex.Message}");
            }
        }

        #endregion
    }
}
