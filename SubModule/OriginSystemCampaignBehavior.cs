using System;
using System.Linq;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace OriginSystemMod
{
    /// <summary>
    /// CampaignBehavior 用于在战役开始时应用预设出身效果
    /// 按照 ChatGPT 教诲：在 OnSessionLaunched 事件中应用预设出身效果
    /// </summary>
    public class OriginSystemCampaignBehavior : CampaignBehaviorBase
    {
        private bool _hasSetSlaveEscapePosition = false;
        private float _teleportDelay = 0f;
        private bool _verifyNextTick = false;
        private const float TELEPORT_DELAY_SECONDS = 1.0f; // 延迟1秒，确保地图已加载
        private int _verifyTickCount = 0;
        private const int MAX_VERIFY_TICKS = 50; // 验证 50 次 tick（约 2 秒）
        private bool _hasAppliedPresetOrigin = false; // 标志：是否已经应用预设出身
        private int _nobleRetryCount = 0; // 贵族身份重试次数
        private const int MAX_NOBLE_RETRY = 10; // 最多重试 10 次（约 2 秒）
        private bool _hasJoinedKhuzaitAsNoble = false; // 标志：是否已经加入库塞特为封臣
        private int _joinRetryCount = 0; // 加入王国重试次数
        private const int MAX_JOIN_RETRY = 5; // 最多重试 5 次
        private int _statusCheckCount = 0; // 状态检查计数（用于定期检查状态是否被覆盖）

        public override void RegisterEvents()
        {
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, OnSessionLaunched);
            CampaignEvents.TickEvent.AddNonSerializedListener(this, OnTick);
        }

        public override void SyncData(IDataStore dataStore)
        {
            // 不需要序列化数据
        }

        private void OnSessionLaunched(CampaignGameStarter starter)
        {
            // 必须用 OriginLog，不准 Debug.Print（自证日志）
            OriginLog.Info($"[OnSessionLaunched] called");
            OriginLog.Info($"[OnSessionLaunched] IsPresetOrigin={OriginSystemHelper.IsPresetOrigin}");
            OriginLog.Info($"[OnSessionLaunched] SelectedPresetOriginId={OriginSystemHelper.SelectedPresetOriginId ?? "null"}");
            OriginLog.Info($"[OnSessionLaunched] SelectedPresetOriginNodes.Count={OriginSystemHelper.SelectedPresetOriginNodes?.Count ?? 0}");
            OriginLog.Info($"[OnSessionLaunched] PendingStartDirection={OriginSystemHelper.PendingStartDirection ?? "null"}");
            
            try
            {
                if (OriginSystemHelper.IsPresetOrigin && 
                    !string.IsNullOrEmpty(OriginSystemHelper.SelectedPresetOriginId))
                {
                    OriginLog.Info($"[OnSessionLaunched] 开始应用预设出身: {OriginSystemHelper.SelectedPresetOriginId}");
                    OriginLog.Info($"[OnSessionLaunched] PendingMinorNobleJoinKhuzait={OriginSystemHelper.PendingMinorNobleJoinKhuzait}");
                    
                    // 如果是 minor_noble，先保存标记（因为 ApplyPresetOrigin 会再次设置它）
                    bool wasPendingJoin = OriginSystemHelper.PendingMinorNobleJoinKhuzait;
                    
                    // 应用预设出身效果（但不设置位置，位置延迟执行）
                    PresetOriginSystem.ApplyPresetOrigin(OriginSystemHelper.SelectedPresetOriginId);
                    OriginLog.Info($"[OnSessionLaunched] 已应用预设出身: {OriginSystemHelper.SelectedPresetOriginId}");
                    
                    // 如果是 minor_noble，在 OnSessionLaunched 中执行加入库塞特王国的逻辑
                    // 使用保存的标记或重新检查（因为 ApplyPresetOrigin 会重新设置它）
                    if (wasPendingJoin || OriginSystemHelper.PendingMinorNobleJoinKhuzait)
                    {
                        OriginLog.Info($"[OnSessionLaunched] 检测到 PendingMinorNobleJoinKhuzait = true (wasPendingJoin={wasPendingJoin}, current={OriginSystemHelper.PendingMinorNobleJoinKhuzait})，开始执行加入库塞特王国逻辑");
                        PresetOriginSystem.JoinKhuzaitAsNoble();
                    }
                    else
                    {
                        OriginLog.Warning($"[OnSessionLaunched] PendingMinorNobleJoinKhuzait 为 false，跳过加入库塞特王国逻辑");
                    }
                }
                else if (!OriginSystemHelper.IsPresetOrigin)
                {
                    OriginLog.Info("[OnSessionLaunched] 开始应用非预设出身");
                    // 应用非预设出身效果
                    NonPresetOriginSystem.ApplyNonPresetOrigin();
                    OriginLog.Info("[OnSessionLaunched] 已应用非预设出身");
                }
                else
                {
                    OriginLog.Warning($"[OnSessionLaunched] 条件不满足: IsPresetOrigin={OriginSystemHelper.IsPresetOrigin} SelectedPresetOriginId={OriginSystemHelper.SelectedPresetOriginId ?? "null"}");
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[OnSessionLaunched] 失败: {ex.Message}");
                OriginLog.Error($"[OnSessionLaunched] StackTrace: {ex.StackTrace}");
            }
        }

        private void OnTick(float dt)
        {
            // 检查是否需要应用预设出身（如果 OnSessionLaunched 时状态还没设置，作为兜底）
            if (!_hasAppliedPresetOrigin &&
                OriginSystemHelper.IsPresetOrigin &&
                !string.IsNullOrEmpty(OriginSystemHelper.SelectedPresetOriginId))
            {
                OriginLog.Info($"[OnTick] 检测到预设出身未应用，开始应用: {OriginSystemHelper.SelectedPresetOriginId}");
                try
                {
                    PresetOriginSystem.ApplyPresetOrigin(OriginSystemHelper.SelectedPresetOriginId);
                    _hasAppliedPresetOrigin = true;
                    OriginLog.Info($"[OnTick] 已应用预设出身: {OriginSystemHelper.SelectedPresetOriginId}");
                }
                catch (Exception ex)
                {
                    OriginLog.Error($"[OnTick] 应用预设出身失败: {ex.Message}");
                    OriginLog.Error($"[OnTick] StackTrace: {ex.StackTrace}");
                }
            }

            // 如果 minor_noble 需要加入库塞特，在 OnTick 中重试（最多 5 次）
            if (OriginSystemHelper.PendingMinorNobleJoinKhuzait && 
                !_hasJoinedKhuzaitAsNoble &&
                _joinRetryCount < MAX_JOIN_RETRY &&
                Clan.PlayerClan != null)
            {
                _joinRetryCount++;
                OriginLog.Info($"[MinorNoble] OnTick retry #{_joinRetryCount}: 重试加入库塞特王国");
                PresetOriginSystem.JoinKhuzaitAsNoble();
                
                // 检查是否成功（使用真正的判据：isVassal + isLord）
                var khuzaitKingdom = PresetOriginSystem.FindKingdom("kingdom_khuzait");
                bool isVassal = khuzaitKingdom != null && 
                               Clan.PlayerClan.Kingdom == khuzaitKingdom && 
                               !Clan.PlayerClan.IsUnderMercenaryService;
                bool isLord = Hero.MainHero?.IsLord ?? false;
                
                if (isVassal && isLord)
                {
                    _hasJoinedKhuzaitAsNoble = true;
                    OriginSystemHelper.PendingMinorNobleJoinKhuzait = false;
                    OriginLog.Info($"[MinorNoble] OnTick retry #{_joinRetryCount}: ✅ 成功加入库塞特为封臣（isVassal={isVassal}, isLord={isLord}），停止重试");
                }
                else
                {
                    OriginLog.Info($"[MinorNoble] OnTick retry #{_joinRetryCount}: 状态检查 - isVassal={isVassal}, isLord={isLord}");
                }
            }
            
            // 兜底：如果 minor_noble 的 IsNoble 被覆盖，在 OnTick 中重新设置（最多 2 秒）
            // 同时确保主角是 Lord（这是判断"贵族/封臣"的真正判据）
            if (OriginSystemHelper.IsPresetOrigin && 
                OriginSystemHelper.SelectedPresetOriginId?.Contains("minor_noble") == true &&
                _nobleRetryCount < MAX_NOBLE_RETRY &&
                Clan.PlayerClan != null)
            {
                _nobleRetryCount++;
                OriginLog.Info($"[MinorNoble] OnTick reapply #{_nobleRetryCount}: 检查并重新设置 IsNoble 和 IsLord");
                
                // 确保主角是 Lord
                if (Hero.MainHero != null && !Hero.MainHero.IsLord)
                {
                    OriginLog.Info($"[MinorNoble] OnTick reapply #{_nobleRetryCount}: 主角不是 Lord，尝试设置 Occupation = Lord");
                    try
                    {
                        try
                        {
                            Hero.MainHero.SetNewOccupation(Occupation.Lord);
                            OriginLog.Info($"[MinorNoble] OnTick reapply #{_nobleRetryCount}: 直接调用成功: IsLord={Hero.MainHero.IsLord}");
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
                                    OriginLog.Info($"[MinorNoble] OnTick reapply #{_nobleRetryCount}: 反射调用成功: IsLord={Hero.MainHero.IsLord}");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        OriginLog.Error($"[MinorNoble] OnTick reapply #{_nobleRetryCount}: 设置主角为 Lord 失败: {ex.Message}");
                    }
                }
                
                // 重新设置 IsNoble（作为补充标记）
                if (!Clan.PlayerClan.IsNoble)
                {
                    OriginLog.Info($"[MinorNoble] OnTick reapply #{_nobleRetryCount}: 检测到 IsNoble=false，重新设置");
                    Clan.PlayerClan.IsNoble = true;
                }
                
                // 检查最终状态（使用真正的判据：isVassal + isLord）
                var khuzaitKingdom = PresetOriginSystem.FindKingdom("kingdom_khuzait");
                bool isVassal = khuzaitKingdom != null && Clan.PlayerClan.Kingdom == khuzaitKingdom && !Clan.PlayerClan.IsUnderMercenaryService;
                bool isLord = Hero.MainHero?.IsLord ?? false;
                bool afterIsNoble = Clan.PlayerClan.IsNoble;
                
                OriginLog.Info($"[MinorNoble] OnTick reapply #{_nobleRetryCount}: 最终状态检查:");
                OriginLog.Info($"[MinorNoble]   - isVassal = {isVassal}");
                OriginLog.Info($"[MinorNoble]   - isLord = {isLord}");
                OriginLog.Info($"[MinorNoble]   - clan.IsNoble = {afterIsNoble} (补充标记)");
                
                if (isVassal && isLord && afterIsNoble)
                {
                    OriginLog.Info($"[MinorNoble] OnTick reapply #{_nobleRetryCount} stop: ✅ 封臣身份、Lord 状态和 IsNoble 都已正确设置，停止重试");
                    _nobleRetryCount = MAX_NOBLE_RETRY; // 停止重试
                }
                else
                {
                    if (!isVassal)
                    {
                        OriginLog.Warning($"[MinorNoble] OnTick reapply #{_nobleRetryCount}: ⚠️ 不是封臣（Kingdom 或 IsUnderMercenaryService 有问题）");
                    }
                    if (!isLord)
                    {
                        OriginLog.Warning($"[MinorNoble] OnTick reapply #{_nobleRetryCount}: ⚠️ 主角不是 Lord");
                    }
                    if (!afterIsNoble)
                    {
                        OriginLog.Warning($"[MinorNoble] OnTick reapply #{_nobleRetryCount}: ⚠️ IsNoble 仍为 false");
                    }
                }
            }
            
            // 定期检查 minor_noble 的状态（用于检测是否被覆盖）
            if (OriginSystemHelper.IsPresetOrigin && 
                OriginSystemHelper.SelectedPresetOriginId == "minor_noble" &&
                _statusCheckCount < 10) // 前 10 次 tick 都检查
            {
                _statusCheckCount++;
                if (_statusCheckCount % 2 == 0) // 每 2 个 tick 检查一次
                {
                    PresetOriginSystem.CheckPlayerClanStatus();
                }
            }
            
            // 连续验证位置（如果设置了逃奴出生位置）
            if (_verifyNextTick)
            {
                _verifyTickCount++;
                Vec2 currentPos = Vec2.Invalid;
                if (MobileParty.MainParty != null)
                {
                    var posProp = typeof(MobileParty).GetProperty("Position2D", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    if (posProp != null)
                    {
                        currentPos = (Vec2)posProp.GetValue(MobileParty.MainParty);
                    }
                }
                
                OriginLog.Info($"[SlaveEscape][VerifyTick] tick={_verifyTickCount} pos=({currentPos.X:F2},{currentPos.Y:F2})");
                
                if (_verifyTickCount >= MAX_VERIFY_TICKS)
                {
                    _verifyNextTick = false;
                    _verifyTickCount = 0;
                    OriginLog.Info("[SlaveEscape][VerifyTick] 连续验证完成，位置未回跳");
                }
                return;
            }

            // 兜底逻辑：如果 OnCharacterCreationFinalized 失败，在 OnTick 中重试
            // 注意：PendingStartSettlementId 可能为 null（在 node 中只保存了 direction），需要在 SetSlaveEscapeStartingLocation 中查找
            if (!_hasSetSlaveEscapePosition && 
                !string.IsNullOrEmpty(OriginSystemHelper.PendingStartDirection))
            {
                _teleportDelay += dt;
                
                // 等待延迟时间，确保地图和队伍已完全初始化
                if (_teleportDelay >= TELEPORT_DELAY_SECONDS)
                {
                    try
                    {
                        if (Hero.MainHero != null && MobileParty.MainParty != null && Campaign.Current != null)
                        {
                            var direction = OriginSystemHelper.PendingStartDirection;
                            var settlementId = OriginSystemHelper.PendingStartSettlementId; // 可能为 null

                            OriginLog.Info($"[SlaveEscape][Teleport] 开始兜底重试: direction={direction} settlementId={settlementId ?? "null"}");

                            // 执行 teleport（settlementId 可能为 null，会在方法内查找）
                            bool success = PresetOriginSystem.SetSlaveEscapeStartingLocation(
                                MobileParty.MainParty, 
                                direction, 
                                settlementId
                            );

                            // 无论成功失败都清空 Pending，避免死循环（old-good 版本的行为）
                            // 如果失败，至少已经尝试过了，不会无限重试
                            _hasSetSlaveEscapePosition = true;
                            OriginSystemHelper.PendingStartDirection = null;
                            OriginSystemHelper.PendingStartSettlementId = null;
                            
                            if (success)
                            {
                                OriginLog.Info("[SlaveEscape][Teleport] 兜底重试成功，已清空 Pending");
                                // 标记下一帧验证位置
                                _verifyNextTick = true;
                            }
                            else
                            {
                                OriginLog.Warning("[SlaveEscape][Teleport] 兜底重试失败，但已清空 Pending（避免死循环）");
                            }
                        }
                        else
                        {
                            OriginLog.Info($"[SlaveEscape][Teleport] 等待条件满足: mainHero={Hero.MainHero != null} mainParty={MobileParty.MainParty != null} campaign={Campaign.Current != null}");
                        }
                    }
                    catch (Exception ex)
                    {
                        OriginLog.Error($"[SlaveEscape][Teleport] 兜底重试异常: {ex.Message}");
                        OriginLog.Error($"[SlaveEscape][Teleport] StackTrace: {ex.StackTrace}");
                    }
                }
                }

            // Fix for Vlandia Movement: Check PendingVlandiaStartLocation
            if (!string.IsNullOrEmpty(OriginSystemHelper.PendingVlandiaStartLocation))
            {
                 _teleportDelay += dt;
                 if (_teleportDelay >= TELEPORT_DELAY_SECONDS)
                 {
                    try
                    {
                        if (Hero.MainHero != null && MobileParty.MainParty != null && Campaign.Current != null)
                        {
                            var location = OriginSystemHelper.PendingVlandiaStartLocation;
                            OriginLog.Info($"[Vlandia][Teleport] Executing pending move to: {location}");
                            
                            // Use the generic SetPresetOriginStartingLocation which handles cultureIds like "nord", "vlandia"
                            bool success = PresetOriginSystem.SetPresetOriginStartingLocation(MobileParty.MainParty, location, null);
                            
                            // Clear pending to stop retrying
                            OriginSystemHelper.PendingVlandiaStartLocation = null;
                            
                            if (success) {
                                OriginLog.Info("[Vlandia][Teleport] Success.");
                                _verifyNextTick = true; // reusing verification logic
                            } else {
                                OriginLog.Warning("[Vlandia][Teleport] Failed.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        OriginLog.Error($"[Vlandia][Teleport] Exception: {ex.Message}");
                    }
                 }
            }
        }
    }
}

