using System;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace OriginSystemMod
{
    /// <summary>
    /// Minimal, compile-safe preset origin system.
    /// Keep public API stable; move detailed origin logic into separate files later.
    /// </summary>
    public static class PresetOriginSystem
    {
        public static void ApplyPresetOrigin(string originId)
        {
            try
            {
                OriginLog.Info($"[ApplyPresetOrigin] originId={originId ?? "null"}");

                if (string.IsNullOrWhiteSpace(originId))
                {
                    OriginLog.Warning("[ApplyPresetOrigin] originId is null/empty, skip.");
                    return;
                }

                var hero = Hero.MainHero;
                var clan = Clan.PlayerClan;
                var party = MobileParty.MainParty;

                if (hero == null || clan == null || party == null)
                {
                    OriginLog.Warning("[ApplyPresetOrigin] Hero/Clan/Party is null, cannot apply origin.");
                    return;
                }

                // 瓦兰迪亚出身
                if (originId == EKKeys.OriginId || originId == "vlandia_expedition_knight" || originId.Contains("expedition_knight"))
                {
                    OriginLog.Info($"[ApplyPresetOrigin] 调用远征骑士出身逻辑 (originId={originId})");
                    VlandiaOriginSystem.ApplyExpeditionKnightOrigin(hero, clan, party);
                    return;
                }

                // 其他出身...
                OriginLog.Info($"[ApplyPresetOrigin] Processing originId={originId} (logic to be migrated)");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyPresetOrigin] Exception: {ex}");
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
    }
}
