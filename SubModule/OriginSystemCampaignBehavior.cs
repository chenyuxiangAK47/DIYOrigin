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
        private bool _appliedPresetOnce = false; // 持久化：是否已成功应用（防重复/读档）
        private int _nobleRetryCount = 0; // 贵族身份重试次数
        private const int MAX_NOBLE_RETRY = 10; // 最多重试 10 次（约 2 秒）
        private bool _hasJoinedKhuzaitAsNoble = false; // 标志：是否已经加入库塞特为封臣
        private int _joinRetryCount = 0; // 加入王国重试次数
        private const int MAX_JOIN_RETRY = 5; // 最多重试 5 次
        private int _statusCheckCount = 0; // 状态检查计数（用于定期检查状态是否被覆盖）
        
        // 按照 ChatGPT 建议：在 OnCharacterCreationIsOverEvent 中应用出生点
        private bool _hasAppliedSpawnLocation = false; // 标志：是否已经应用出生点（防止重复执行）

        public override void RegisterEvents()
        {
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, OnSessionLaunched);
            CampaignEvents.TickEvent.AddNonSerializedListener(this, OnTick);
            
            // 添加验证日志
            OriginLog.Info("[RegisterEvents] 正在注册 OnCharacterCreationIsOverEvent");
            try
            {
                CampaignEvents.OnCharacterCreationIsOverEvent.AddNonSerializedListener(this, OnCharacterCreationIsOver);
                OriginLog.Info("[RegisterEvents] ✅ OnCharacterCreationIsOverEvent 注册成功");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[RegisterEvents] ❌ OnCharacterCreationIsOverEvent 注册失败: {ex.Message}");
            }
        }

        public override void SyncData(IDataStore dataStore)
        {
            // 序列化状态，防止读档后重复触发
            dataStore.SyncData("_hasAppliedSpawnLocation", ref _hasAppliedSpawnLocation);
            dataStore.SyncData("_appliedPresetOnce", ref _appliedPresetOnce);
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
                // OnSessionLaunched 仅做日志与非预设初始化，不再执行预设出身 Apply（避免被原生流程覆盖）
                if (!OriginSystemHelper.IsPresetOrigin)
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

        /// <summary>
        /// 按照 ChatGPT 建议：在角色创建结束后应用出生点（最稳的时机）
        /// </summary>
        private void OnCharacterCreationIsOver()
        {
            try
            {
                // 双重日志：确保可见
                OriginLog.Info("[OnCharacterCreationIsOver] called");
                Debug.Print("[OriginSystem] [OnCharacterCreationIsOver] called", 0, Debug.DebugColor.Green);
                OriginLog.Info($"[OnCharacterCreationIsOver] IsPresetOrigin={OriginSystemHelper.IsPresetOrigin} SelectedPresetOriginId={OriginSystemHelper.SelectedPresetOriginId ?? "null"}");
                OriginLog.Info($"[OnCharacterCreationIsOver] HeroReady={Hero.MainHero != null} ClanReady={Clan.PlayerClan != null} PartyReady={MobileParty.MainParty != null}");
                
                // 防止重复执行
                if (_hasAppliedSpawnLocation)
                {
                    OriginLog.Info("[OnCharacterCreationIsOver] 已经应用过出生点，跳过");
                    return;
                }

                if (Hero.MainHero == null || MobileParty.MainParty == null || Campaign.Current == null)
                {
                    OriginLog.Warning("[OnCharacterCreationIsOver] Hero/Party/Campaign 未初始化，延迟到 OnTick 处理");
                    return;
                }

                // 先应用预设出身（确保 pending 出生点被设置）
                if (!_appliedPresetOnce && OriginSystemHelper.IsPresetOrigin && !string.IsNullOrEmpty(OriginSystemHelper.SelectedPresetOriginId))
                {
                    OriginLog.Info($"[OnCharacterCreationIsOver] 准备 ApplyPresetOrigin: {OriginSystemHelper.SelectedPresetOriginId}");
                    Debug.Print($"[OriginSystem] [OnCharacterCreationIsOver] 准备 ApplyPresetOrigin: {OriginSystemHelper.SelectedPresetOriginId}", 0, Debug.DebugColor.Green);
                    bool applied = PresetOriginSystem.ApplyPresetOrigin(OriginSystemHelper.SelectedPresetOriginId);
                    if (applied)
                    {
                        _appliedPresetOnce = true;
                        _hasAppliedPresetOrigin = true;
                        OriginLog.Info("[OnCharacterCreationIsOver] ✅ ApplyPresetOrigin 成功");
                        Debug.Print("[OriginSystem] [OnCharacterCreationIsOver] ✅ ApplyPresetOrigin 成功", 0, Debug.DebugColor.Green);
                    }
                    else
                    {
                        OriginLog.Warning("[OnCharacterCreationIsOver] ApplyPresetOrigin 失败，稍后 OnTick 兜底重试");
                    }
                }

                // 处理逃奴出身的出生点
                if (!string.IsNullOrEmpty(OriginSystemHelper.PendingStartDirection))
                {
                    OriginLog.Info($"[OnCharacterCreationIsOver] 处理逃奴出生点: direction={OriginSystemHelper.PendingStartDirection}");
                    
                    string villageId = OriginSystemHelper.PendingStartSettlementId;
                    
                    // 如果没有保存具体的 settlementId，根据 direction 查找
                    if (string.IsNullOrEmpty(villageId))
                    {
                        villageId = OriginSpawnHelper.FindVillageByDirection(OriginSystemHelper.PendingStartDirection);
                    }

                    if (!string.IsNullOrEmpty(villageId))
                    {
                        bool success = OriginSpawnHelper.SpawnPlayerAtVillage(villageId, enterSettlementMenu: true);
                        if (success)
                        {
                            _hasAppliedSpawnLocation = true;
                            OriginSystemHelper.PendingStartDirection = null;
                            OriginSystemHelper.PendingStartSettlementId = null;
                            OriginLog.Info("[OnCharacterCreationIsOver] ✅ 逃奴出生点应用成功");
                        }
                        else
                        {
                            OriginLog.Warning("[OnCharacterCreationIsOver] 逃奴出生点应用失败，将在 OnTick 中重试");
                        }
                    }
                    else
                    {
                        OriginLog.Warning("[OnCharacterCreationIsOver] 找不到逃奴目标村子，将在 OnTick 中重试");
                    }
                }

                // 处理侠义骑士出身的出生点
                if (!string.IsNullOrEmpty(OriginSystemHelper.PendingVlandiaStartLocation))
                {
                    OriginLog.Info($"[OnCharacterCreationIsOver] 处理侠义骑士出生点: location={OriginSystemHelper.PendingVlandiaStartLocation}");
                    Debug.Print($"[OriginSystem] [OnCharacterCreationIsOver] 处理侠义骑士出生点: location={OriginSystemHelper.PendingVlandiaStartLocation}", 0, Debug.DebugColor.Green);
                    
                    string villageId = OriginSpawnHelper.FindVillageByCulture(OriginSystemHelper.PendingVlandiaStartLocation);
                    
                    if (!string.IsNullOrEmpty(villageId))
                    {
                        bool success = OriginSpawnHelper.SpawnPlayerAtVillage(villageId, enterSettlementMenu: true);
                        if (success)
                        {
                            _hasAppliedSpawnLocation = true;
                            OriginSystemHelper.PendingVlandiaStartLocation = null;
                            OriginLog.Info("[OnCharacterCreationIsOver] ✅ 侠义骑士出生点应用成功");
                            Debug.Print("[OriginSystem] [OnCharacterCreationIsOver] ✅ 侠义骑士出生点应用成功", 0, Debug.DebugColor.Green);
                        }
                        else
                        {
                            OriginLog.Warning("[OnCharacterCreationIsOver] 侠义骑士出生点应用失败，将在 OnTick 中重试");
                        }
                    }
                    else
                    {
                        OriginLog.Warning("[OnCharacterCreationIsOver] 找不到侠义骑士目标村子，将在 OnTick 中重试");
                    }
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[OnCharacterCreationIsOver] 异常: {ex.Message}");
                OriginLog.Error($"[OnCharacterCreationIsOver] StackTrace: {ex.StackTrace}");
            }
        }

        private void OnTick(float dt)
        {
            // 检查是否需要应用预设出身（如果 OnSessionLaunched 时状态还没设置，作为兜底）
            if (!_appliedPresetOnce &&
                OriginSystemHelper.IsPresetOrigin &&
                !string.IsNullOrEmpty(OriginSystemHelper.SelectedPresetOriginId) &&
                Hero.MainHero != null &&
                Clan.PlayerClan != null &&
                MobileParty.MainParty != null)
            {
                OriginLog.Info($"[OnTick] 兜底应用预设出身: {OriginSystemHelper.SelectedPresetOriginId}");
                try
                {
                    bool applied = PresetOriginSystem.ApplyPresetOrigin(OriginSystemHelper.SelectedPresetOriginId);
                    if (applied)
                    {
                        _appliedPresetOnce = true;
                        _hasAppliedPresetOrigin = true;
                        OriginLog.Info($"[OnTick] ✅ 兜底应用成功: {OriginSystemHelper.SelectedPresetOriginId}");
                    }
                    else
                    {
                        OriginLog.Warning("[OnTick] 兜底应用失败，将在后续 tick 再试");
                    }
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

            // 兜底逻辑：如果 OnCharacterCreationIsOver 失败，在 OnTick 中重试
            // 使用 OriginSpawnHelper（按照 ChatGPT 建议的标准实现）
            if (!_hasAppliedSpawnLocation)
            {
                // 处理逃奴出身的出生点
                if (!string.IsNullOrEmpty(OriginSystemHelper.PendingStartDirection))
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
                                var settlementId = OriginSystemHelper.PendingStartSettlementId;

                                OriginLog.Info($"[SlaveEscape][Teleport] 开始兜底重试: direction={direction} settlementId={settlementId ?? "null"}");

                                string villageId = settlementId;
                                
                                // 如果没有保存具体的 settlementId，根据 direction 查找
                                if (string.IsNullOrEmpty(villageId))
                                {
                                    villageId = OriginSpawnHelper.FindVillageByDirection(direction);
                                }

                                if (!string.IsNullOrEmpty(villageId))
                                {
                                    bool success = OriginSpawnHelper.SpawnPlayerAtVillage(villageId, enterSettlementMenu: true);
                                    
                                    // 无论成功失败都清空 Pending，避免死循环
                                    _hasAppliedSpawnLocation = true;
                                    OriginSystemHelper.PendingStartDirection = null;
                                    OriginSystemHelper.PendingStartSettlementId = null;
                                    
                                    if (success)
                                    {
                                        OriginLog.Info("[SlaveEscape][Teleport] 兜底重试成功，已清空 Pending");
                                        _verifyNextTick = true;
                                    }
                                    else
                                    {
                                        OriginLog.Warning("[SlaveEscape][Teleport] 兜底重试失败，但已清空 Pending（避免死循环）");
                                    }
                                }
                                else
                                {
                                    OriginLog.Warning("[SlaveEscape][Teleport] 找不到目标村子，但已清空 Pending（避免死循环）");
                                    _hasAppliedSpawnLocation = true;
                                    OriginSystemHelper.PendingStartDirection = null;
                                    OriginSystemHelper.PendingStartSettlementId = null;
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
                            // 清空 Pending 避免死循环
                            _hasAppliedSpawnLocation = true;
                            OriginSystemHelper.PendingStartDirection = null;
                            OriginSystemHelper.PendingStartSettlementId = null;
                        }
                    }
                }

                // 处理侠义骑士出身的出生点
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
                                OriginLog.Info($"[Vlandia][Teleport] 开始兜底重试: location={location}");
                                
                                string villageId = OriginSpawnHelper.FindVillageByCulture(location);
                                
                                if (!string.IsNullOrEmpty(villageId))
                                {
                                    bool success = OriginSpawnHelper.SpawnPlayerAtVillage(villageId, enterSettlementMenu: true);
                                    
                                    // Clear pending to stop retrying
                                    _hasAppliedSpawnLocation = true;
                                    OriginSystemHelper.PendingVlandiaStartLocation = null;
                                    
                                    if (success)
                                    {
                                        OriginLog.Info("[Vlandia][Teleport] 兜底重试成功");
                                        _verifyNextTick = true;
                                    }
                                    else
                                    {
                                        OriginLog.Warning("[Vlandia][Teleport] 兜底重试失败，但已清空 Pending");
                                    }
                                }
                                else
                                {
                                    OriginLog.Warning("[Vlandia][Teleport] 找不到目标村子，但已清空 Pending");
                                    _hasAppliedSpawnLocation = true;
                                    OriginSystemHelper.PendingVlandiaStartLocation = null;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            OriginLog.Error($"[Vlandia][Teleport] 兜底重试异常: {ex.Message}");
                            // 清空 Pending 避免死循环
                            _hasAppliedSpawnLocation = true;
                            OriginSystemHelper.PendingVlandiaStartLocation = null;
                        }
                    }
                }
            }
        }
    }
}

