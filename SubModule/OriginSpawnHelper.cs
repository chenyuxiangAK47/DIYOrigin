using System;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace OriginSystemMod
{
    /// <summary>
    /// 出身系统出生点传送辅助类
    /// 按照 ChatGPT 建议的标准模板实现
    /// </summary>
    public static class OriginSpawnHelper
    {
        /// <summary>
        /// 传送玩家到指定村子
        /// </summary>
        /// <param name="villageId">村子的 StringId（如 "village_steppe_1"）</param>
        /// <param name="enterSettlementMenu">是否进入村子菜单（false = 出生在村子旁边的大地图上，可见可走路；true = 直接进入村子界面，大地图上不可见）</param>
        /// <returns>是否成功</returns>
        public static bool SpawnPlayerAtVillage(string villageId, bool enterSettlementMenu = false)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(villageId))
                {
                    OriginLog.Warning("[OriginSpawnHelper] villageId is null/empty");
                    return false;
                }

                if (Campaign.Current == null)
                {
                    OriginLog.Warning("[OriginSpawnHelper] Campaign.Current is null");
                    return false;
                }

                // 1) 找到目标村子 Settlement
                Settlement target = Settlement.Find(villageId) ?? Settlement.All.FirstOrDefault(s => s != null && s.StringId == villageId);
                if (target == null)
                {
                    OriginLog.Warning($"[OriginSpawnHelper] 找不到村子: {villageId}");
                    return false;
                }

                var party = MobileParty.MainParty;
                if (party == null)
                {
                    OriginLog.Warning("[OriginSpawnHelper] MobileParty.MainParty is null");
                    return false;
                }

                OriginLog.Info($"[OriginSpawnHelper] 找到目标村子: {target.StringId} ({target.Name})");

                // 2) 设置队伍位置到村子门口（根据反编译结果：使用 Position 属性，CampaignVec2 类型）
                Vec2 targetPos;
                try
                {
                    // 优先使用 GatePosition，转换为 Vec2
                    var gatePos = target.GatePosition;
                    if (gatePos != CampaignVec2.Invalid)
                    {
                        targetPos = new Vec2(gatePos.X, gatePos.Y);
                    }
                    else
                    {
                        // 回退到 Position
                        var settlementPos = target.Position;
                        targetPos = new Vec2(settlementPos.X, settlementPos.Y);
                    }
                    OriginLog.Info($"[OriginSpawnHelper] 目标位置: ({targetPos.X:F2}, {targetPos.Y:F2})");
                }
                catch (Exception ex)
                {
                    OriginLog.Error($"[OriginSpawnHelper] 获取目标位置失败: {ex.Message}");
                    return false;
                }

                // 添加小随机偏移，避免多个队伍在同一位置重叠
                var rnd = new Random();
                var offset = new Vec2(
                    (float)(rnd.NextDouble() * 2.0 - 1.0) * 0.5f,
                    (float)(rnd.NextDouble() * 2.0 - 1.0) * 0.5f
                );
                targetPos += offset;

                // 根据反编译结果：MobileParty 没有 Position2D 属性，但有 Position 属性（CampaignVec2 类型，可读写）
                // 使用 Position 属性设置位置
                bool positionSetSuccess = false;
                var partyType = party.GetType(); // 使用运行时类型
                
                try
                {
                    // 方法1：使用 Position 属性（CampaignVec2 类型，可读写）
                    var flags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.FlattenHierarchy;
                    var positionProp = partyType.GetProperty("Position", flags);
                    
                    if (positionProp != null && positionProp.CanWrite)
                    {
                        var propType = positionProp.PropertyType;
                        OriginLog.Info($"[OriginSpawnHelper] 找到 Position 属性，类型: {propType.FullName}, 可写: {positionProp.CanWrite}");
                        
                            if (propType == typeof(CampaignVec2))
                            {
                                // 创建 CampaignVec2：构造函数是 CampaignVec2(Vec2 pos, bool isOnLand)
                                var campaignPosCtor = typeof(CampaignVec2).GetConstructor(new Type[] { typeof(Vec2), typeof(bool) });
                                if (campaignPosCtor != null)
                                {
                                    var campaignPos = campaignPosCtor.Invoke(new object[] { targetPos, true }); // isOnLand = true
                                    var beforePos = (CampaignVec2)positionProp.GetValue(party);
                                    positionProp.SetValue(party, campaignPos);
                                    party.IsVisible = true; // 确保队伍可见
                                    var afterPos = (CampaignVec2)positionProp.GetValue(party);
                                    var afterVec2 = new Vec2(afterPos.X, afterPos.Y);
                                    var distance = (afterVec2 - targetPos).Length;
                                    
                                    OriginLog.Info($"[OriginSpawnHelper] ✅ 已通过 Position 属性设置位置: before=({beforePos.X:F2},{beforePos.Y:F2}) after=({afterPos.X:F2},{afterPos.Y:F2}) target=({targetPos.X:F2},{targetPos.Y:F2}) distance={distance:F2}");
                                    positionSetSuccess = true;
                                    
                                    if (distance > 1.0f)
                                    {
                                        OriginLog.Warning($"[OriginSpawnHelper] 位置设置后距离较大 ({distance:F2})，可能被重置");
                                    }
                                }
                                else
                                {
                                    OriginLog.Error("[OriginSpawnHelper] CampaignVec2(Vec2, bool) 构造函数不存在");
                                }
                            }
                        else
                        {
                            OriginLog.Warning($"[OriginSpawnHelper] Position 属性类型不匹配: 期望 CampaignVec2，实际 {propType.FullName}");
                        }
                    }
                    
                    // 方法2：如果 Position 属性不存在，尝试 _position 字段（CampaignVec2类型）
                    if (!positionSetSuccess)
                    {
                        var positionField = partyType.GetField("_position", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                        if (positionField != null && positionField.FieldType == typeof(CampaignVec2))
                        {
                            OriginLog.Info("[OriginSpawnHelper] Position 属性不存在，尝试使用 _position 字段 (CampaignVec2)");
                            var campaignPosCtor = typeof(CampaignVec2).GetConstructor(new Type[] { typeof(Vec2), typeof(bool) });
                            if (campaignPosCtor != null)
                            {
                                var campaignPos = campaignPosCtor.Invoke(new object[] { targetPos, true }); // isOnLand = true
                                var beforePos = (CampaignVec2)positionField.GetValue(party);
                                positionField.SetValue(party, campaignPos);
                                party.IsVisible = true;
                                var afterPos = (CampaignVec2)positionField.GetValue(party);
                                var afterVec2 = new Vec2(afterPos.X, afterPos.Y);
                                var distance = (afterVec2 - targetPos).Length;
                                
                                OriginLog.Info($"[OriginSpawnHelper] ✅ 已通过 _position 字段设置位置: before=({beforePos.X:F2},{beforePos.Y:F2}) after=({afterPos.X:F2},{afterPos.Y:F2}) target=({targetPos.X:F2},{targetPos.Y:F2}) distance={distance:F2}");
                                positionSetSuccess = true;
                            }
                        }
                    }
                    
                    if (!positionSetSuccess)
                    {
                        OriginLog.Error("[OriginSpawnHelper] Position 属性和 _position 字段都无法设置位置");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    OriginLog.Error($"[OriginSpawnHelper] 设置位置失败: {ex.Message}");
                    OriginLog.Error($"[OriginSpawnHelper] StackTrace: {ex.StackTrace}");
                    return false;
                }

                // 3) 根据 enterSettlementMenu 参数决定是否进入定居点菜单
                if (enterSettlementMenu)
                {
                    try
                    {
                        // 进入前如果在别的 settlement，先离开（避免状态冲突）
                        if (party.CurrentSettlement != null)
                        {
                            LeaveSettlementAction.ApplyForParty(party);
                            OriginLog.Info($"[OriginSpawnHelper] 已离开当前定居点: {party.CurrentSettlement.StringId}");
                        }
                        
                        // 进入目标定居点（会导致队伍在大地图上不可见，这是正常行为）
                        EnterSettlementAction.ApplyForParty(party, target);
                        OriginLog.Info($"[OriginSpawnHelper] ✅ 已进入村子菜单: {target.StringId} (队伍在大地图上不可见是正常的)");
                    }
                    catch (Exception ex)
                    {
                        OriginLog.Warning($"[OriginSpawnHelper] 进入村子菜单失败: {ex.Message}");
                        // 不返回 false，因为位置已经设置成功了
                    }
                }
                else
                {
                    // 不进入菜单，确保队伍在大地图上可见
                    try
                    {
                        if (party.CurrentSettlement != null)
                        {
                            LeaveSettlementAction.ApplyForParty(party);
                            OriginLog.Info($"[OriginSpawnHelper] 已离开当前定居点，确保在大地图上可见");
                        }
                        OriginLog.Info($"[OriginSpawnHelper] ✅ 位置已设置，玩家在大地图上可见（未进入菜单）");
                    }
                    catch (Exception ex)
                    {
                        OriginLog.Warning($"[OriginSpawnHelper] 离开定居点失败: {ex.Message}");
                        // 不返回 false，因为位置已经设置成功了
                    }
                }

                OriginLog.Info($"[OriginSpawnHelper] ✅ 成功传送玩家到村子: {target.StringId}");
                return true;
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[OriginSpawnHelper] 异常: {ex.Message}");
                OriginLog.Error($"[OriginSpawnHelper] StackTrace: {ex.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// 根据方向（direction）查找对应的村子
        /// 用于逃奴出身：根据 "desert"、"empire" 等方向查找合适的村子
        /// </summary>
        /// <param name="direction">方向（"desert"、"empire"、"steppe"）</param>
        /// <returns>村子的 StringId，如果找不到则返回 null</returns>
        public static string FindVillageByDirection(string direction)
        {
            try
            {
                if (Campaign.Current == null)
                {
                    OriginLog.Warning("[OriginSpawnHelper] Campaign.Current is null");
                    return null;
                }

                string culture = null;
                if (direction == "desert")
                    culture = "aserai";
                else if (direction == "steppe")
                    culture = "khuzait";
                else if (direction == "empire")
                    culture = "empire";

                if (string.IsNullOrWhiteSpace(culture))
                {
                    OriginLog.Warning($"[OriginSpawnHelper] 未知方向: {direction}");
                    return null;
                }

                // 查找该文化的第一个村子
                var village = Campaign.Current.Settlements
                    .FirstOrDefault(s => s != null && s.IsVillage && s.Culture != null && s.Culture.StringId == culture);

                if (village != null)
                {
                    OriginLog.Info($"[OriginSpawnHelper] 根据方向 {direction} 找到村子: {village.StringId} ({village.Name})");
                    return village.StringId;
                }

                OriginLog.Warning($"[OriginSpawnHelper] 根据方向 {direction} 找不到村子");
                return null;
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[OriginSpawnHelper] FindVillageByDirection 异常: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 根据文化ID查找对应的村子
        /// 用于侠义骑士出身：根据 "nord"、"aserai" 等文化查找村子
        /// </summary>
        /// <param name="cultureId">文化ID（"nord"、"aserai"、"battania"、"vlandia"）</param>
        /// <returns>村子的 StringId，如果找不到则返回 null</returns>
        public static string FindVillageByCulture(string cultureId)
        {
            try
            {
                if (Campaign.Current == null)
                {
                    OriginLog.Warning("[OriginSpawnHelper] Campaign.Current is null");
                    return null;
                }

                // 特殊处理：诺德文化（sturgia）- 尝试查找拉维凯尔
                if (cultureId == "sturgia" || cultureId == "nord")
                {
                    var ravikail = Campaign.Current.Settlements.FirstOrDefault(s =>
                        s != null && s.IsVillage &&
                        (s.Name.ToString().Contains("拉维凯尔") ||
                         s.Name.ToString().Contains("Ravikail") ||
                         s.StringId.Contains("ravikail") ||
                         s.StringId.Contains("Ravikail")));

                    if (ravikail != null)
                    {
                        OriginLog.Info($"[OriginSpawnHelper] 找到拉维凯尔: {ravikail.StringId}");
                        return ravikail.StringId;
                    }

                    // 回退到 Sturgia 文化的第一个村子
                    cultureId = "sturgia";
                }

                // 查找该文化的第一个村子
                var village = Campaign.Current.Settlements
                    .FirstOrDefault(s => s != null && s.IsVillage && s.Culture != null && s.Culture.StringId == cultureId);

                if (village != null)
                {
                    OriginLog.Info($"[OriginSpawnHelper] 根据文化 {cultureId} 找到村子: {village.StringId} ({village.Name})");
                    return village.StringId;
                }

                OriginLog.Warning($"[OriginSpawnHelper] 根据文化 {cultureId} 找不到村子");
                return null;
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[OriginSpawnHelper] FindVillageByCulture 异常: {ex.Message}");
                return null;
            }
        }
    }
}
