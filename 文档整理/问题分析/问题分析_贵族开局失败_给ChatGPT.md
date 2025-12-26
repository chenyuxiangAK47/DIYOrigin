# 问题分析：贵族开局失败（minor_noble 出身）

## 问题描述

用户测试"汗廷旁支贵族"（minor_noble）出身时发现：
- ✅ 家族等级正确设置为 4 级（Tier = 4）
- ✅ 名望正确设置为 300（Renown = 300）
- ✅ 兵力正确添加
- ✅ 成功调用了 `ChangeKingdomAction.ApplyByJoinToKingdom` 加入库塞特王国
- ❌ **但是玩家仍然不是贵族身份**（IsNoble = false）
- ❌ **玩家可能被识别为雇佣兵而不是封臣**

## 当前实现代码

### 代码位置
`OriginSystemMod/SubModule/PresetOriginSystem.cs` 的 `ApplyMinorNobleOrigin` 方法（第306-552行）

### 完整代码逻辑

```csharp
private static void ApplyMinorNobleOrigin(Hero hero, Clan clan, MobileParty party)
{
    OriginLog.Info($"[MinorNoble] Apply enter: clan.StringId={clan?.StringId ?? "null"}, clan.Name={clan?.Name?.ToString() ?? "null"}");
    OriginLog.Info($"[MinorNoble] clan==PlayerClan: {ReferenceEquals(clan, Clan.PlayerClan)}");
    OriginLog.Info($"[MinorNoble] PlayerClan.StringId={Clan.PlayerClan?.StringId ?? "null"}");
    
    SetClanTier(clan, 4); // 4级家族（贵族）
    
    // 关键：设置名望（Renown）- 名望是加入成为贵族的条件之一
    GainRenown(hero, 300);
    OriginLog.Info($"[MinorNoble] 已设置名望: +300 (当前名望: {hero.Clan?.Renown ?? 0})");
    
    // 关键：使用 ApplyByJoinToKingdom 加入成为封臣（vassal），而不是 ApplyByJoinFactionAsMercenary（雇佣兵）
    var khuzaitKingdom = FindKingdom("kingdom_khuzait") ?? FindKingdom("khuzait");
    if (khuzaitKingdom == null)
    {
        OriginLog.Warning("[MinorNoble] 未找到库塞特王国");
    }
    else
    {
        // 如果之前被当成雇佣兵进过某国，先解除雇佣兵（保险）
        if (Clan.PlayerClan != null && Clan.PlayerClan.IsUnderMercenaryService)
        {
            OriginLog.Info("[MinorNoble] 检测到雇佣兵状态，先退出雇佣兵服务");
            try
            {
                var leaveMethod = typeof(ChangeKingdomAction).GetMethod("ApplyByLeaveKingdomAsMercenary", BindingFlags.Public | BindingFlags.Static);
                if (leaveMethod != null)
                {
                    var parameters = leaveMethod.GetParameters();
                    if (parameters.Length == 1)
                    {
                        leaveMethod.Invoke(null, new object[] { Clan.PlayerClan });
                    }
                    else if (parameters.Length == 2)
                    {
                        leaveMethod.Invoke(null, new object[] { Clan.PlayerClan, false });
                    }
                    OriginLog.Info("[MinorNoble] 已退出雇佣兵服务");
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[MinorNoble] 退出雇佣兵失败: {ex.Message}");
            }
        }
        
        // 使用 ApplyByJoinToKingdom 加入成为封臣（vassal/贵族）
        if (Clan.PlayerClan != null && Clan.PlayerClan.Kingdom != khuzaitKingdom)
        {
            // 记录加入前的状态（5个关键字段）
            OriginLog.Info($"[MinorNoble] JoinToKingdom BEFORE:");
            OriginLog.Info($"[MinorNoble]   - clan.Kingdom?.StringId = {(Clan.PlayerClan.Kingdom?.StringId ?? "null")}");
            OriginLog.Info($"[MinorNoble]   - clan.IsNoble = {Clan.PlayerClan.IsNoble}");
            OriginLog.Info($"[MinorNoble]   - clan.IsMinorFaction = {Clan.PlayerClan.IsMinorFaction}");
            OriginLog.Info($"[MinorNoble]   - clan.IsClanTypeMercenary = {Clan.PlayerClan.IsClanTypeMercenary}");
            OriginLog.Info($"[MinorNoble]   - clan.IsUnderMercenaryService = {Clan.PlayerClan.IsUnderMercenaryService}");
            OriginLog.Info($"[MinorNoble]   - target Kingdom = {khuzaitKingdom.StringId}");
            
            try
            {
                // 关键：使用 ApplyByJoinToKingdom 加入为封臣（vassal），不是 ApplyByJoinFactionAsMercenary（雇佣兵）
                var joinMethod = typeof(ChangeKingdomAction).GetMethod("ApplyByJoinToKingdom", BindingFlags.Public | BindingFlags.Static);
                if (joinMethod != null)
                {
                    var parameters = joinMethod.GetParameters();
                    OriginLog.Info($"[MinorNoble] ApplyByJoinToKingdom 方法找到，参数数量: {parameters.Length}");
                    
                    if (parameters.Length == 2)
                    {
                        joinMethod.Invoke(null, new object[] { Clan.PlayerClan, khuzaitKingdom });
                        OriginLog.Info("[MinorNoble] 调用 ApplyByJoinToKingdom(Clan, Kingdom)");
                    }
                    else if (parameters.Length == 3 && parameters[2].ParameterType == typeof(bool))
                    {
                        joinMethod.Invoke(null, new object[] { Clan.PlayerClan, khuzaitKingdom, false });
                        OriginLog.Info("[MinorNoble] 调用 ApplyByJoinToKingdom(Clan, Kingdom, bool=false)");
                    }
                    else if (parameters.Length == 3 && parameters[2].ParameterType == typeof(CampaignTime))
                    {
                        joinMethod.Invoke(null, new object[] { Clan.PlayerClan, khuzaitKingdom, CampaignTime.Now });
                        OriginLog.Info("[MinorNoble] 调用 ApplyByJoinToKingdom(Clan, Kingdom, CampaignTime)");
                    }
                    else
                    {
                        OriginLog.Error($"[MinorNoble] 未找到 ApplyByJoinToKingdom 的合适重载");
                    }
                }
                else
                {
                    OriginLog.Error("[MinorNoble] 未找到 ApplyByJoinToKingdom 方法");
                }
                
                // 记录加入后的状态（5个关键字段）
                OriginLog.Info($"[MinorNoble] JoinToKingdom AFTER:");
                OriginLog.Info($"[MinorNoble]   - clan.Kingdom?.StringId = {(Clan.PlayerClan.Kingdom?.StringId ?? "null")}");
                OriginLog.Info($"[MinorNoble]   - clan.IsNoble = {Clan.PlayerClan.IsNoble}");
                OriginLog.Info($"[MinorNoble]   - clan.IsMinorFaction = {Clan.PlayerClan.IsMinorFaction}");
                OriginLog.Info($"[MinorNoble]   - clan.IsClanTypeMercenary = {Clan.PlayerClan.IsClanTypeMercenary}");
                OriginLog.Info($"[MinorNoble]   - clan.IsUnderMercenaryService = {Clan.PlayerClan.IsUnderMercenaryService}");
                
                // 加入王国后，兜底设置标志位（确保是封臣/贵族，不是雇佣兵）
                bool needFix = false;
                if (!Clan.PlayerClan.IsNoble)
                {
                    OriginLog.Info("[MinorNoble] 兜底设置: IsNoble = true");
                    Clan.PlayerClan.IsNoble = true;
                    needFix = true;
                }
                if (Clan.PlayerClan.IsUnderMercenaryService)
                {
                    OriginLog.Warning("[MinorNoble] 警告: 加入后 IsUnderMercenaryService 仍为 true");
                    // 尝试通过 EndMercenaryService 修复
                    try
                    {
                        var endMercenaryMethod = typeof(Clan).GetMethod("EndMercenaryService", BindingFlags.Public | BindingFlags.Instance);
                        if (endMercenaryMethod != null)
                        {
                            var endParams = endMercenaryMethod.GetParameters();
                            if (endParams.Length == 0)
                            {
                                endMercenaryMethod.Invoke(Clan.PlayerClan, null);
                            }
                            else if (endParams.Length == 1 && endParams[0].ParameterType == typeof(bool))
                            {
                                endMercenaryMethod.Invoke(Clan.PlayerClan, new object[] { false });
                            }
                            OriginLog.Info("[MinorNoble] EndMercenaryService 调用成功");
                        }
                    }
                    catch (Exception ex2)
                    {
                        OriginLog.Error($"[MinorNoble] EndMercenaryService 调用失败: {ex2.Message}");
                    }
                    needFix = true;
                }
                
                if (needFix)
                {
                    OriginLog.Info($"[MinorNoble] 兜底设置后最终状态:");
                    OriginLog.Info($"[MinorNoble]   - clan.Kingdom?.StringId = {(Clan.PlayerClan.Kingdom?.StringId ?? "null")}");
                    OriginLog.Info($"[MinorNoble]   - clan.IsNoble = {Clan.PlayerClan.IsNoble}");
                    OriginLog.Info($"[MinorNoble]   - clan.IsMinorFaction = {Clan.PlayerClan.IsMinorFaction} (只读，由系统管理)");
                    OriginLog.Info($"[MinorNoble]   - clan.IsClanTypeMercenary = {Clan.PlayerClan.IsClanTypeMercenary} (只读，由系统管理)");
                    OriginLog.Info($"[MinorNoble]   - clan.IsUnderMercenaryService = {Clan.PlayerClan.IsUnderMercenaryService}");
                }
                
                // 验证：确保成功加入为封臣
                if (Clan.PlayerClan.Kingdom == khuzaitKingdom && Clan.PlayerClan.IsNoble && !Clan.PlayerClan.IsUnderMercenaryService)
                {
                    OriginLog.Info("[MinorNoble] ✅ 成功加入库塞特王国为封臣（vassal/贵族）");
                }
                else
                {
                    OriginLog.Warning("[MinorNoble] ⚠️ 加入状态异常，请检查上述日志");
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[MinorNoble] 加入库塞特封臣失败: {ex.Message}");
                OriginLog.Error($"[MinorNoble] StackTrace: {ex.StackTrace}");
            }
        }
        else if (Clan.PlayerClan?.Kingdom == khuzaitKingdom)
        {
            OriginLog.Info($"[MinorNoble] 已经在库塞特王国中，当前状态:");
            OriginLog.Info($"[MinorNoble]   - clan.Kingdom?.StringId = {(Clan.PlayerClan.Kingdom?.StringId ?? "null")}");
            OriginLog.Info($"[MinorNoble]   - clan.IsNoble = {Clan.PlayerClan.IsNoble}");
            OriginLog.Info($"[MinorNoble]   - clan.IsMinorFaction = {Clan.PlayerClan.IsMinorFaction}");
            OriginLog.Info($"[MinorNoble]   - clan.IsClanTypeMercenary = {Clan.PlayerClan.IsClanTypeMercenary}");
            OriginLog.Info($"[MinorNoble]   - clan.IsUnderMercenaryService = {Clan.PlayerClan.IsUnderMercenaryService}");
        }
        
        ChangeRelationAction.ApplyPlayerRelation(khuzaitKingdom.Leader, 20);
    }
    
    // ... 其他节点处理 ...
}
```

## 关键问题点

### 1. 调用时机问题
- **问题**：`ApplyMinorNobleOrigin` 可能在角色创建完成之前被调用
- **影响**：此时 `Clan.PlayerClan` 可能还没有完全初始化，或者会被后续的角色创建流程覆盖

### 2. ApplyByJoinToKingdom 的条件检查
- **问题**：`ApplyByJoinToKingdom` 方法内部可能有条件检查，如果不符合条件，可能会：
  - 静默失败（不抛出异常，但不执行操作）
  - 自动转换为雇佣兵加入（如果不符合封臣条件）
- **可能的原因**：
  - 名望不足（虽然设置了300，但可能需要在特定时机设置）
  - 家族等级不足（虽然设置了4级，但可能被重置）
  - 玩家角色创建流程会重置这些状态

### 3. 只读属性问题
- **问题**：`IsMinorFaction` 和 `IsClanTypeMercenary` 是只读属性，由系统自动管理
- **影响**：即使设置了 `IsNoble = true`，如果系统认为应该是雇佣兵，这些属性可能仍然显示为雇佣兵状态

### 4. 兜底设置可能被覆盖
- **问题**：在 `ApplyByJoinToKingdom` 之后立即设置 `IsNoble = true`，但可能被后续的角色创建流程覆盖
- **影响**：需要在更晚的时机（如 `OnSessionLaunched`）重新设置

## 日志查找位置

### 游戏日志文件位置
- **Windows**: `%USERPROFILE%\Documents\Mount and Blade II Bannerlord\Logs\rgl_log.txt`
- **或者**: `%LOCALAPPDATA%\Mount and Blade II Bannerlord\Logs\rgl_log.txt`

### 关键日志标记
在日志中搜索以下标记：
- `[MinorNoble]` - 所有 minor_noble 相关的日志
- `[OS]` - OriginLog 系统的日志
- `JoinToKingdom BEFORE` - 加入王国前的状态
- `JoinToKingdom AFTER` - 加入王国后的状态
- `兜底设置` - 兜底设置的状态

## 需要 ChatGPT 帮助的问题

1. **ApplyByJoinToKingdom 的正确调用时机**
   - 应该在角色创建的哪个阶段调用？
   - 是否需要在 `OnCharacterCreationFinalized` 或 `OnSessionLaunched` 中调用？

2. **ApplyByJoinToKingdom 的条件检查**
   - 这个方法内部有哪些条件检查？
   - 如果不符合封臣条件，是否会静默失败或自动转换为雇佣兵？
   - 需要满足哪些条件才能成功加入为封臣？

3. **IsNoble 属性的设置时机**
   - 在角色创建流程中，什么时候设置 `IsNoble = true` 最合适？
   - 是否需要在多个时机都设置（创建时、加入王国时、会话启动时）？

4. **原版如何实现玩家开局为贵族**
   - 原版游戏中，玩家如何开局就成为某个王国的封臣/贵族？
   - 是否有相关的示例代码或配置？

5. **替代方案**
   - 如果 `ApplyByJoinToKingdom` 在角色创建时无法正常工作，是否有其他方法？
   - 是否可以通过修改 XML 配置文件来实现？

## 当前日志输出示例（期望）

如果代码正常工作，日志应该显示：

```
[OS] [MinorNoble] Apply enter: clan.StringId=player_faction, clan.Name=Playerland
[OS] [MinorNoble] clan==PlayerClan: True
[OS] [MinorNoble] PlayerClan.StringId=player_faction
[OS] [SetClanTier] 开始设置家族等级: clan=Playerland, tier=4, beforeTier=0
[OS] [SetClanTier] 设置完成: tier: 0 -> 4
[OS] [GainRenown] Before: hero=Player, clan=Playerland, renown=0, amount=300
[OS] [GainRenown] After: renown=300, expected=300, success=True
[OS] [MinorNoble] 已设置名望: +300 (当前名望: 300)
[OS] [MinorNoble] JoinToKingdom BEFORE:
[OS] [MinorNoble]   - clan.Kingdom?.StringId = null
[OS] [MinorNoble]   - clan.IsNoble = False
[OS] [MinorNoble]   - clan.IsMinorFaction = True
[OS] [MinorNoble]   - clan.IsClanTypeMercenary = False
[OS] [MinorNoble]   - clan.IsUnderMercenaryService = False
[OS] [MinorNoble]   - target Kingdom = kingdom_khuzait
[OS] [MinorNoble] ApplyByJoinToKingdom 方法找到，参数数量: 2
[OS] [MinorNoble] 调用 ApplyByJoinToKingdom(Clan, Kingdom)
[OS] [MinorNoble] JoinToKingdom AFTER:
[OS] [MinorNoble]   - clan.Kingdom?.StringId = kingdom_khuzait
[OS] [MinorNoble]   - clan.IsNoble = True  <-- 这里应该是 True
[OS] [MinorNoble]   - clan.IsMinorFaction = False  <-- 这里应该是 False
[OS] [MinorNoble]   - clan.IsClanTypeMercenary = False
[OS] [MinorNoble]   - clan.IsUnderMercenaryService = False
[OS] [MinorNoble] ✅ 成功加入库塞特王国为封臣（vassal/贵族）
```

## 修复方案（已实施）

按照 ChatGPT 的建议，已进行以下修复：

### 1. 移除反射调用，使用编译期调用

**问题**：`ApplyByJoinToKingdom` 实际上是 4 个参数（后两个可选），反射调用不会自动补默认值，导致可能根本没调用成功。

**修复**：
```csharp
// 旧代码（反射调用，可能失败）
var joinMethod = typeof(ChangeKingdomAction).GetMethod("ApplyByJoinToKingdom", ...);
joinMethod.Invoke(null, new object[] { Clan.PlayerClan, khuzaitKingdom });

// 新代码（编译期调用，C# 自动补默认参数）
ChangeKingdomAction.ApplyByJoinToKingdom(Clan.PlayerClan, khuzaitKingdom, CampaignTime.Now, false);
```

### 2. 延迟到 OnSessionLaunched 执行

**问题**：在角色创建阶段调用可能太早，后续流程会覆盖状态。

**修复**：
- 在 `ApplyMinorNobleOrigin` 中只设置标记：`OriginSystemHelper.PendingMinorNobleJoinKhuzait = true`
- 在 `OnSessionLaunched` 中执行真正的加入王国逻辑：`PresetOriginSystem.JoinKhuzaitAsNoble()`
- 在 `OnTick` 中添加重试逻辑（最多 5 次）

### 3. 添加完整的状态检查

**修复**：在 `JoinKhuzaitAsNoble` 方法中同时检查 5 个关键字段：
- `clan.Kingdom?.StringId`
- `clan.IsNoble`
- `clan.IsMinorFaction`（只读）
- `clan.IsClanTypeMercenary`（只读）
- `clan.IsUnderMercenaryService`（只读）

### 4. 添加详细日志

所有关键操作都有详细日志记录，包括：
- 加入前后的状态对比
- 错误和异常信息
- 重试次数和结果

## 请提供实际日志

请将游戏日志文件（`rgl_log.txt`）中所有包含 `[JoinKhuzaitAsNoble]` 或 `[MinorNoble]` 或 `[OS]` 的行复制给我，特别是：
- `JoinKhuzaitAsNoble BEFORE` 和 `JoinKhuzaitAsNoble AFTER` 之间的所有日志
- `OnSessionLaunched` 中的相关日志
- `OnTick` 中的重试日志
- 任何错误或警告信息
- 最终的状态值

这样我可以更准确地分析问题所在。

## 最新更新（2024）

### 已实施的修复
1. ✅ 移除反射调用，使用编译期调用 `ChangeKingdomAction.ApplyByJoinToKingdom(Clan, Kingdom, CampaignTime.Now, false)`
2. ✅ 延迟到 `OnSessionLaunched` 执行加入王国逻辑
3. ✅ 添加 `OnTick` 重试机制（最多 5 次）
4. ✅ 添加 `EnsurePlayerClanIsNoble` 方法持续检查并设置 `IsNoble`
5. ✅ 添加详细日志记录

### 当前问题
用户反馈：**依旧失败，出身后不是贵族**

### 需要检查的关键点
1. **OnSessionLaunched 是否被调用？**
   - 查看日志中是否有 `[OnSessionLaunched] called`
   - 如果没有，说明事件没有被触发

2. **JoinKhuzaitAsNoble 是否被调用？**
   - 查看日志中是否有 `[JoinKhuzaitAsNoble] BEFORE`
   - 如果没有，说明标记没有被设置或检查失败

3. **ApplyByJoinToKingdom 是否成功？**
   - 查看日志中 `AFTER` 的状态，特别是 `IsNoble` 的值
   - 如果 `IsNoble` 仍然是 `false`，说明 `ApplyByJoinToKingdom` 可能失败了

4. **IsNoble 是否被覆盖？**
   - 查看 `OnTick` 中的重试日志
   - 检查 `EnsurePlayerClanIsNoble` 是否被调用

5. **是否有错误或异常？**
   - 查看日志中是否有 `Error` 或 `Warning` 信息

### 日志文件位置
- **Windows**: `%USERPROFILE%\Documents\Mount and Blade II Bannerlord\Logs\rgl_log.txt`
- **或者**: `%LOCALAPPDATA%\Mount and Blade II Bannerlord\Logs\rgl_log.txt`
- **游戏目录**: `D:\SteamLibrary\steamapps\common\Mount & Blade II Bannerlord\Logs\rgl_log.txt`

### 下一步
请提供最新的日志文件内容，特别是包含以下关键词的部分：
- `[JoinKhuzaitAsNoble]`
- `[OnSessionLaunched]`
- `[MinorNoble]`
- `[OS]`（OriginLog 系统的日志标记）

---

## 最新修复（2024-12-19）

### 关键发现
根据用户反馈，问题的根本原因可能是：
1. **`Clan.IsNoble` 不是判断"封臣/贵族"的正确判据**
   - 真正的判据应该是：
     - `Hero.MainHero.IsLord` 或 `Hero.MainHero.Occupation == Occupation.Lord`
     - `clan.Kingdom != null && !clan.IsUnderMercenaryService`

2. **需要在加入王国前先设置主角为 Lord**
   - 使用 `Hero.MainHero.SetNewOccupation(Occupation.Lord)`

### 已实施的修复

#### 1. 在 `JoinKhuzaitAsNoble` 中添加设置主角为 Lord
```csharp
// 关键：在加入王国前先强制把主角设为 Lord
if (Hero.MainHero != null && !Hero.MainHero.IsLord)
{
    Hero.MainHero.SetNewOccupation(Occupation.Lord);
}
```

#### 2. 在日志中添加 `IsLord` 和 `Occupation` 字段
```csharp
OriginLog.Info($"[JoinKhuzaitAsNoble]   - Hero.MainHero.IsLord = {(Hero.MainHero?.IsLord ?? false)}");
OriginLog.Info($"[JoinKhuzaitAsNoble]   - Hero.MainHero.Occupation = {(Hero.MainHero?.Occupation?.ToString() ?? "null")}");
```

#### 3. 修改判据，使用 `isVassal` 而不是只依赖 `IsNoble`
```csharp
// 真正的判据：Kingdom == khuzaitKingdom && !IsUnderMercenaryService
bool isVassal = Clan.PlayerClan.Kingdom == khuzaitKingdom && !Clan.PlayerClan.IsUnderMercenaryService;
bool isLord = Hero.MainHero?.IsLord ?? false;

if (isVassal && isLord)
{
    OriginLog.Info("[JoinKhuzaitAsNoble] ✅ 成功加入库塞特王国为封臣（vassal/贵族）");
}
```

#### 4. 在 `EnsurePlayerClanIsNoble` 中也添加设置 `IsLord` 的逻辑
```csharp
// 确保主角是 Lord
if (Hero.MainHero != null && !Hero.MainHero.IsLord)
{
    Hero.MainHero.SetNewOccupation(Occupation.Lord);
}
```

#### 5. 在 `OnTick` 的重试逻辑中也添加设置 `IsLord` 的逻辑
```csharp
// 确保主角是 Lord
if (Hero.MainHero != null && !Hero.MainHero.IsLord)
{
    Hero.MainHero.SetNewOccupation(Occupation.Lord);
}
```

### 新的日志输出（期望）

如果修复成功，日志应该显示：

```
[OS] [JoinKhuzaitAsNoble] BEFORE:
[OS] [JoinKhuzaitAsNoble]   - Hero.MainHero.IsLord = False
[OS] [JoinKhuzaitAsNoble]   - Hero.MainHero.Occupation = Wanderer
[OS] [JoinKhuzaitAsNoble] 主角不是 Lord，设置 Occupation = Lord
[OS] [JoinKhuzaitAsNoble] 已设置主角为 Lord: IsLord=True, Occupation=Lord
[OS] [JoinKhuzaitAsNoble] 调用 ChangeKingdomAction.ApplyByJoinToKingdom(...)
[OS] [JoinKhuzaitAsNoble] AFTER:
[OS] [JoinKhuzaitAsNoble]   - clan.Kingdom?.StringId = kingdom_khuzait
[OS] [JoinKhuzaitAsNoble]   - clan.IsUnderMercenaryService = False
[OS] [JoinKhuzaitAsNoble]   - Hero.MainHero.IsLord = True  <-- 关键
[OS] [JoinKhuzaitAsNoble]   - Hero.MainHero.Occupation = Lord  <-- 关键
[OS] [JoinKhuzaitAsNoble] 最终验证:
[OS] [JoinKhuzaitAsNoble]   - isVassal (Kingdom==khuzait && !MercenaryService) = True
[OS] [JoinKhuzaitAsNoble]   - isLord (Hero.IsLord) = True
[OS] [JoinKhuzaitAsNoble] ✅ 成功加入库塞特王国为封臣（vassal/贵族）
```

### 关键改进点

1. **不再依赖 `Clan.IsNoble` 作为唯一判据**
   - 使用 `isVassal = Kingdom == khuzaitKingdom && !IsUnderMercenaryService`
   - 使用 `isLord = Hero.MainHero.IsLord`

2. **在加入王国前先设置主角为 Lord**
   - 这是判断"贵族/封臣"的真正判据

3. **在多个时机都设置 `IsLord`**
   - `JoinKhuzaitAsNoble` 中
   - `EnsurePlayerClanIsNoble` 中
   - `OnTick` 的重试逻辑中

---

## 最新修复（2024-12-19 第二次更新）

### 问题分析
用户反馈修复后仍然失败。可能的原因：
1. **`SetNewOccupation` 方法可能不存在或签名不同**
   - 直接调用 `Hero.MainHero.SetNewOccupation(Occupation.Lord)` 可能失败
   - 某些 Bannerlord 版本可能没有这个方法，或者方法签名不同

2. **`Occupation` 类型可能不在预期的命名空间**
   - 虽然代码中有 `using TaleWorlds.Core;`，但 `Occupation` 可能在其他命名空间

### 已实施的修复（第二次）

#### 1. 添加反射调用的备用方案
在所有调用 `SetNewOccupation` 的地方，添加了反射调用的备用方案：

```csharp
try
{
    Hero.MainHero.SetNewOccupation(Occupation.Lord);
    OriginLog.Info("直接调用成功");
}
catch (MissingMethodException)
{
    // 如果直接调用失败，尝试使用反射
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
            OriginLog.Info("反射调用成功");
        }
    }
}
```

#### 2. 添加必要的 using 语句
在 `OriginSystemCampaignBehavior.cs` 中添加：
```csharp
using System.Linq;
using System.Reflection;
using TaleWorlds.Core;
```

#### 3. 增强错误日志
所有 `SetNewOccupation` 调用都添加了详细的错误日志，包括：
- 直接调用是否成功
- 反射调用是否成功
- 如果都失败，记录详细的错误信息

### 修改的文件
1. `PresetOriginSystem.cs` - 在 `JoinKhuzaitAsNoble` 和 `EnsurePlayerClanIsNoble` 中添加反射备用方案
2. `OriginSystemCampaignBehavior.cs` - 在 `OnTick` 重试逻辑中添加反射备用方案，并添加必要的 using 语句

### 下一步
如果仍然失败，请检查日志中的：
1. 是否有 `[JoinKhuzaitAsNoble] 设置主角为 Lord 失败` 的错误
2. 是否有 `MissingMethodException` 或 `MethodNotFoundException`
3. `IsLord` 和 `Occupation` 的值是什么

如果 `SetNewOccupation` 方法确实不存在，可能需要：
- 使用其他方式设置 Occupation（例如通过 CharacterObject）
- 或者接受 `IsLord` 可能无法直接设置，只依赖 `isVassal` 判据

---

**请提供最新的日志文件，特别是包含 `[JoinKhuzaitAsNoble]` 和 `[OnSessionLaunched]` 的部分，以便进一步分析问题。**

---

## 最新修复（2024-12-19 第三次更新）

### 用户反馈
用户反馈修复后仍然失败，要求查看日志并修改代码。

### 已实施的修复（第三次）

#### 1. 在所有 `SetNewOccupation` 调用处添加反射备用方案
修改了以下三个位置：
- `JoinKhuzaitAsNoble` 方法中的两处调用
- `EnsurePlayerClanIsNoble` 方法中的调用
- `OriginSystemCampaignBehavior.OnTick` 中的调用

#### 2. 反射调用的实现细节
```csharp
try
{
    Hero.MainHero.SetNewOccupation(Occupation.Lord);
    OriginLog.Info("直接调用成功");
}
catch (MissingMethodException)
{
    // 使用反射查找方法
    var heroType = typeof(Hero);
    var setOccupationMethod = heroType.GetMethod("SetNewOccupation", 
        BindingFlags.Public | BindingFlags.Instance);
    if (setOccupationMethod != null)
    {
        // 查找 Occupation 枚举类型
        var occupationType = typeof(Hero).Assembly.GetType("TaleWorlds.Core.Occupation") 
                            ?? typeof(Hero).Assembly.GetTypes()
                                .FirstOrDefault(t => t.Name == "Occupation" && t.IsEnum);
        if (occupationType != null)
        {
            var lordValue = Enum.Parse(occupationType, "Lord");
            setOccupationMethod.Invoke(Hero.MainHero, new object[] { lordValue });
            OriginLog.Info("反射调用成功");
        }
    }
}
```

#### 3. 添加必要的命名空间
在 `OriginSystemCampaignBehavior.cs` 中添加：
```csharp
using System.Linq;
using System.Reflection;
using TaleWorlds.Core;
```

### 修改的文件列表
1. ✅ `PresetOriginSystem.cs`
   - `JoinKhuzaitAsNoble` 方法：添加反射备用方案（2处）
   - `EnsurePlayerClanIsNoble` 方法：添加反射备用方案

2. ✅ `OriginSystemCampaignBehavior.cs`
   - `OnTick` 方法：添加反射备用方案
   - 添加必要的 using 语句

### 日志输出期望
如果修复成功，日志应该显示：
```
[OS] [JoinKhuzaitAsNoble] 主角不是 Lord，尝试设置 Occupation = Lord
[OS] [JoinKhuzaitAsNoble] 直接调用成功: IsLord=True, Occupation=Lord
```
或者（如果直接调用失败）：
```
[OS] [JoinKhuzaitAsNoble] 直接调用失败，尝试使用反射
[OS] [JoinKhuzaitAsNoble] 反射调用成功: IsLord=True, Occupation=Lord
```

### 如果仍然失败
请检查日志中的：
1. 是否有 `MissingMethodException` 或 `MethodNotFoundException`
2. 是否有 `SetNewOccupation 方法不存在` 的警告
3. `IsLord` 和 `Occupation` 的最终值是什么

如果 `SetNewOccupation` 方法确实不存在，可能需要：
- 检查 Bannerlord 版本，确认 API 是否可用
- 使用其他方式设置 Occupation（例如通过 CharacterObject 的模板）
- 或者只依赖 `isVassal` 判据，不强制设置 `IsLord`

---

## 问题分析（2024-12-19 第四次更新）

### 用户反馈
用户反馈修复后仍然失败，要求查看日志分析原因，特别是反射到了什么。

### 代码分析

#### 当前反射实现的逻辑
```csharp
try
{
    Hero.MainHero.SetNewOccupation(Occupation.Lord);
    OriginLog.Info("直接调用成功");
}
catch (MissingMethodException)
{
    // 反射查找 SetNewOccupation 方法
    var heroType = typeof(Hero);
    var setOccupationMethod = heroType.GetMethod("SetNewOccupation", 
        BindingFlags.Public | BindingFlags.Instance);
    // ...
}
```

#### 潜在问题

**问题 1：只捕获 `MissingMethodException`**
- 如果方法不存在，编译期就会失败，不会抛出 `MissingMethodException`
- 如果方法存在但调用失败（参数错误、权限问题等），可能抛出其他异常
- **建议**：应该捕获所有 `Exception`，而不仅仅是 `MissingMethodException`

**问题 2：反射查找可能失败**
- `GetMethod` 可能返回 `null`（方法不存在或签名不匹配）
- `GetType("TaleWorlds.Core.Occupation")` 可能返回 `null`（类型不在该命名空间）
- `FirstOrDefault` 可能返回 `null`（找不到 Occupation 枚举）
- **建议**：添加详细的日志记录每一步的查找结果

**问题 3：反射调用可能失败**
- `Enum.Parse` 可能失败（枚举值不存在）
- `Invoke` 可能失败（参数类型不匹配、权限问题等）
- **建议**：添加 try-catch 包裹反射调用，记录详细错误

### 需要检查的日志内容

请检查日志中是否有以下信息：

1. **直接调用是否成功**
   - 查找：`[JoinKhuzaitAsNoble] 直接调用成功`
   - 如果没有这条日志，说明直接调用失败了

2. **是否进入反射分支**
   - 查找：`[JoinKhuzaitAsNoble] 直接调用失败，尝试使用反射`
   - 如果没有这条日志，说明抛出的不是 `MissingMethodException`

3. **反射查找方法的结果**
   - 查找：`[JoinKhuzaitAsNoble] 未找到 SetNewOccupation 方法`
   - 如果有这条日志，说明方法不存在或签名不匹配

4. **反射查找类型的结果**
   - 查找：`[JoinKhuzaitAsNoble] 未找到 Occupation 枚举类型`
   - 如果有这条日志，说明类型查找失败

5. **反射调用的结果**
   - 查找：`[JoinKhuzaitAsNoble] 反射调用成功`
   - 如果没有这条日志，说明反射调用失败了

6. **最终的错误信息**
   - 查找：`[JoinKhuzaitAsNoble] 设置主角为 Lord 失败`
   - 查看具体的异常消息和堆栈跟踪

### 下一步改进建议

1. **改进异常捕获**
   - 将 `catch (MissingMethodException)` 改为 `catch (Exception ex)`
   - 记录异常类型和消息，判断是否需要尝试反射

2. **增强反射日志**
   - 记录 `GetMethod` 的返回值（null 或 MethodInfo）
   - 记录 `GetType` 的返回值（null 或 Type）
   - 记录 `Enum.Parse` 的结果
   - 记录 `Invoke` 的返回值

3. **添加反射方法查找的备选方案**
   - 尝试不同的方法名（如 `SetOccupation`、`ChangeOccupation`）
   - 尝试不同的参数类型（如 `string` 而不是 `Occupation` 枚举）
   - 尝试查找私有方法或静态方法

---

## 代码改进方案（待实施）

### 改进后的反射实现（建议）

```csharp
if (Hero.MainHero != null && !Hero.MainHero.IsLord)
{
    OriginLog.Info("[JoinKhuzaitAsNoble] 主角不是 Lord，尝试设置 Occupation = Lord");
    bool setSuccess = false;
    
    // 方法 1：尝试直接调用
    try
    {
        Hero.MainHero.SetNewOccupation(Occupation.Lord);
        OriginLog.Info($"[JoinKhuzaitAsNoble] 直接调用成功: IsLord={Hero.MainHero.IsLord}, Occupation={Hero.MainHero.Occupation}");
        setSuccess = true;
    }
    catch (Exception ex)
    {
        OriginLog.Info($"[JoinKhuzaitAsNoble] 直接调用失败: {ex.GetType().Name} - {ex.Message}");
        
        // 方法 2：尝试反射调用
        try
        {
            var heroType = typeof(Hero);
            OriginLog.Info($"[JoinKhuzaitAsNoble] 开始反射查找: Hero类型={heroType.FullName}");
            
            // 查找 SetNewOccupation 方法
            var setOccupationMethod = heroType.GetMethod("SetNewOccupation", 
                BindingFlags.Public | BindingFlags.Instance);
            
            if (setOccupationMethod == null)
            {
                OriginLog.Warning("[JoinKhuzaitAsNoble] 反射: GetMethod('SetNewOccupation') 返回 null");
                
                // 尝试查找所有公共实例方法
                var allMethods = heroType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Where(m => m.Name.Contains("Occupation") || m.Name.Contains("Lord"))
                    .ToList();
                OriginLog.Info($"[JoinKhuzaitAsNoble] 反射: 找到包含 'Occupation' 或 'Lord' 的方法: {string.Join(", ", allMethods.Select(m => m.Name))}");
            }
            else
            {
                OriginLog.Info($"[JoinKhuzaitAsNoble] 反射: 找到方法 SetNewOccupation, 参数类型={string.Join(", ", setOccupationMethod.GetParameters().Select(p => p.ParameterType.Name))}");
                
                // 查找 Occupation 枚举类型
                var occupationType = typeof(Hero).Assembly.GetType("TaleWorlds.Core.Occupation");
                if (occupationType == null)
                {
                    OriginLog.Info("[JoinKhuzaitAsNoble] 反射: GetType('TaleWorlds.Core.Occupation') 返回 null，尝试查找所有枚举类型");
                    var allEnums = typeof(Hero).Assembly.GetTypes()
                        .Where(t => t.IsEnum && t.Name.Contains("Occupation"))
                        .ToList();
                    OriginLog.Info($"[JoinKhuzaitAsNoble] 反射: 找到包含 'Occupation' 的枚举类型: {string.Join(", ", allEnums.Select(t => t.FullName))}");
                    occupationType = allEnums.FirstOrDefault();
                }
                
                if (occupationType != null)
                {
                    OriginLog.Info($"[JoinKhuzaitAsNoble] 反射: 找到 Occupation 类型={occupationType.FullName}");
                    var lordValue = Enum.Parse(occupationType, "Lord");
                    OriginLog.Info($"[JoinKhuzaitAsNoble] 反射: Enum.Parse('Lord') 成功, 值={lordValue}");
                    
                    setOccupationMethod.Invoke(Hero.MainHero, new object[] { lordValue });
                    OriginLog.Info($"[JoinKhuzaitAsNoble] 反射调用成功: IsLord={Hero.MainHero.IsLord}, Occupation={Hero.MainHero.Occupation}");
                    setSuccess = true;
                }
                else
                {
                    OriginLog.Warning("[JoinKhuzaitAsNoble] 反射: 未找到 Occupation 枚举类型");
                }
            }
        }
        catch (Exception reflectEx)
        {
            OriginLog.Error($"[JoinKhuzaitAsNoble] 反射调用失败: {reflectEx.GetType().Name} - {reflectEx.Message}");
            OriginLog.Error($"[JoinKhuzaitAsNoble] 反射调用 StackTrace: {reflectEx.StackTrace}");
        }
    }
    
    if (!setSuccess)
    {
        OriginLog.Warning("[JoinKhuzaitAsNoble] ⚠️ 所有方法都失败，无法设置主角为 Lord");
        OriginLog.Warning("[JoinKhuzaitAsNoble] 当前状态: IsLord=" + (Hero.MainHero?.IsLord ?? false) + ", Occupation=" + (Hero.MainHero?.Occupation?.ToString() ?? "null"));
    }
}
```

### 关键改进点

1. **捕获所有异常**：不再只捕获 `MissingMethodException`，而是捕获所有 `Exception`
2. **详细日志记录**：
   - 记录异常类型和消息
   - 记录反射查找的每一步结果
   - 记录找到的所有相关方法和类型
3. **备选查找方案**：如果标准查找失败，尝试查找所有包含关键词的方法和类型

---

## 给 ChatGPT 的问题模板

### 问题描述
我在 Bannerlord 模组中尝试设置主角的 Occupation 为 Lord，但失败了。代码使用了直接调用和反射两种方式，但都失败了。

### 当前实现
[粘贴上面的代码]

### 日志输出
[请粘贴日志中关于 `JoinKhuzaitAsNoble`、`SetNewOccupation`、`Occupation`、`IsLord` 的所有相关行]

### 问题
1. `SetNewOccupation` 方法在 Bannerlord API 中是否存在？
2. 如果存在，正确的调用方式是什么？
3. 如果不存在，有什么替代方案可以设置主角为 Lord？
4. 反射查找失败的可能原因是什么？

### Bannerlord 版本
[请填写你的 Bannerlord 版本号]

---

## 最新修复（2024-12-19 第五次更新 - 基于 ChatGPT 深度分析）

### ChatGPT 的核心发现

1. **封臣身份是 Clan-level 的状态，不是 Hero-level**
   - `ChangeKingdomAction.ApplyByJoinToKingdom` 的入参是 `Clan`
   - "封臣身份"本质是 Clan-level 的状态（是不是在某 Kingdom 下、是不是雇佣兵服务中）
   - `Hero.IsLord` 和 `Occupation` 影响 UI/对话表现，但不是封臣身份的判据

2. **不要只盯着 `IsNoble`**
   - `Clan.IsNoble` 是可写的，但 `IsMinorFaction`、`IsClanTypeMercenary`、`IsUnderMercenaryService` 是只读的（系统说了算）
   - 即使设置了 `IsNoble=true`，系统可能在其他地方又改回去

3. **真正的三个判据**
   - 判据1：`Clan.Kingdom == khuzaitKingdom`
   - 判据2：`!Clan.IsUnderMercenaryService`（只读，系统判据）
   - 判据3：`khuzaitKingdom.Clans.Contains(Clan.PlayerClan)`（是否被登记成这个王国的 clan 成员）

### 已实施的修复

#### 1. 创建 Harmony Hook 追踪 ChangeKingdomAction 调用
创建了新文件 `KingdomActionHooks.cs`，用于追踪：
- `ApplyByJoinToKingdom`（所有重载）
- `ApplyByJoinFactionAsMercenary`（防止被"改回雇佣兵"）
- `ApplyByLeaveKingdomAsMercenary`（看有没有被动触发离开）

Hook 会记录：
- 调用前后的 Clan 和 Kingdom 状态
- 调用堆栈（看是谁调用的）
- 三个真正判据的值

#### 2. 添加版本信息记录
在 `JoinKhuzaitAsNoble` 开始时记录：
```csharp
var assemblyVersion = typeof(ChangeKingdomAction).Assembly.GetName().Version;
OriginLog.Info($"[JoinKhuzaitAsNoble] ChangeKingdomAction.Assembly.Version={assemblyVersion}");
```
用于确认运行时使用的是哪个版本的 API。

#### 3. 改进验证逻辑（使用三个真正判据）
替换了原来的验证逻辑，现在使用三个真正的判据：
```csharp
// 判据1：Clan.Kingdom == khuzaitKingdom
bool condition1 = Clan.PlayerClan.Kingdom == khuzaitKingdom;
// 判据2：!IsUnderMercenaryService（只读，系统判据）
bool condition2 = !Clan.PlayerClan.IsUnderMercenaryService;
// 判据3：khuzaitKingdom.Clans.Contains(Clan.PlayerClan)
bool condition3 = khuzaitKingdom.Clans?.Contains(Clan.PlayerClan) ?? false;

bool isVassal = condition1 && condition2 && condition3;
```

### 修改的文件

1. ✅ **新建：`KingdomActionHooks.cs`**
   - Harmony Hook 用于追踪所有 `ChangeKingdomAction` 相关调用
   - 记录调用前后的状态和堆栈

2. ✅ **`PresetOriginSystem.cs`**
   - 添加版本信息记录
   - 改进验证逻辑，使用三个真正判据

### 下一步

运行游戏后，查看日志中的：
1. `[HOOK ApplyByJoinToKingdom]` 的 prefix/postfix 日志
2. 三个判据的值（condition1, condition2, condition3）
3. 调用堆栈（看是谁在调用，是否有其他行为在覆盖状态）

如果三个判据都满足，说明封臣身份已正确设置。
如果某个判据失败，日志会明确指出是哪个判据失败。

---

## 最新修复（2024-12-19 第六次更新 - Hook 万物）

### 用户反馈
用户已成功加入库塞特，要求扩展 Hook 来追踪更多内容，为后续开发做准备。

### 已实施的修复

#### 1. 扩展 KingdomActionHooks
在原有基础上，添加了对以下 Action 的 Hook：
- `ChangeRelationAction` - 关系变化
- `GiveGoldAction` - 金币变化
- `ChangeClanTierAction` - 家族等级变化（如果存在）

#### 2. 创建 Hook 方法库文档
创建了新文档 `Hook方法库_Bannerlord模组开发指南.md`，记录：
- 各种 Action 类的 Hook 方法和使用场景
- Hook 实现模式（版本无关的多重载 Hook、单一方法 Hook、状态转储辅助方法）
- 关键状态字段说明
- 使用建议和注意事项

#### 3. 改进 Hook 代码安全性
- 使用 try-catch 处理可能不存在的类型（如 `ChangeClanTierAction`）
- 使用反射安全地获取类型，避免编译错误

### 修改的文件

1. ✅ **`KingdomActionHooks.cs`**
   - 扩展了 `TargetMethods()` 来追踪更多 Action
   - 改进了类型查找的安全性

2. ✅ **新建：`Hook方法库_Bannerlord模组开发指南.md`**
   - 记录了各种 Hook 方法和使用场景
   - 记录了 Hook 实现模式
   - 将作为后续开发的参考文档

### 下一步

1. 查看日志中的 Hook 输出，确认：
   - `[HOOK ApplyByJoinToKingdom]` 的调用堆栈
   - 三个判据的值
   - 是否有其他 Action 覆盖了状态

2. 根据日志结果，继续优化代码

3. 在 `Hook方法库_Bannerlord模组开发指南.md` 中记录新的发现和改进

---

## 最新修复（2024-12-19 第七次更新 - 修复出场不是贵族问题）

### 问题发现
用户反馈"出场不是贵族"，检查代码后发现：

**问题原因**：
1. `ApplyMinorNobleOrigin` 方法中只调用了 `EnsurePlayerClanIsNoble()`
2. `EnsurePlayerClanIsNoble()` 只是简单地设置 `Clan.PlayerClan.IsNoble = true`
3. 没有调用更完善的 `SetClanNoble()` 方法，该方法会尝试多种方式设置贵族身份

**根本原因分析**：
根据 ChatGPT 的分析，`IsNoble` 虽然可写，但可能被系统覆盖。真正判断"贵族/封臣"的是：
- 三个真正判据（Kingdom、!IsUnderMercenaryService、在 Clans 列表中）
- `Hero.IsLord`（Occupation = Lord）

所以即使设置了 `IsNoble = true`，如果：
1. 没有真正加入王国（三个判据不满足）
2. 或者 `IsLord` 没有设置成功
3. 或者 `IsNoble` 被后续流程覆盖

玩家仍然不会被认为是"贵族"。

### 已实施的修复

#### 1. 在 `ApplyMinorNobleOrigin` 中添加 `SetClanNoble` 调用
```csharp
// 关键：使用 SetClanNoble 方法设置贵族身份（更完善的方法，会尝试多种方式）
SetClanNoble(clan, true);
OriginLog.Info($"[MinorNoble] 已调用 SetClanNoble(clan, true)，当前 IsNoble={clan.IsNoble}");

// 最后再次确保 IsNoble 设置成功（防止被覆盖）
EnsurePlayerClanIsNoble();
```

`SetClanNoble` 方法会尝试：
1. 直接设置 `IsNoble` 属性
2. 设置私有字段 `_isNoble`
3. 使用 `ChangeClanNobleAction`（如果存在）
4. 通过提升家族等级来间接设置

#### 2. 确保 `JoinKhuzaitAsNoble` 被正确调用
- 在 `OnSessionLaunched` 中调用
- 在 `OnTick` 中重试（最多5次）
- 使用三个真正判据验证是否成功

#### 3. 确保 `IsLord` 被设置
- 在 `JoinKhuzaitAsNoble` 中设置 `Hero.MainHero.SetNewOccupation(Occupation.Lord)`
- 在 `EnsurePlayerClanIsNoble` 中也设置
- 在 `OnTick` 重试逻辑中也设置

### 修改的文件

1. ✅ **`PresetOriginSystem.cs`**
   - 在 `ApplyMinorNobleOrigin` 中添加了 `SetClanNoble(clan, true)` 调用

### 验证步骤

运行游戏后，检查日志中的：

1. **`[MinorNoble] 已调用 SetClanNoble(clan, true)`**
   - 确认 `SetClanNoble` 被调用
   - 查看 `当前 IsNoble=` 的值

2. **`[JoinKhuzaitAsNoble]` 的日志**
   - 查看三个判据的值
   - 查看 `IsLord` 的值
   - 查看最终验证结果

3. **`[HOOK ApplyByJoinToKingdom]` 的日志**
   - 查看调用堆栈（看是谁调用的）
   - 查看调用前后的状态变化

### 如果仍然失败

请检查日志中的：
1. `SetClanNoble` 是否成功（查看日志中的"设置成功"消息）
2. `JoinKhuzaitAsNoble` 是否被调用
3. 三个判据是否都满足
4. `IsLord` 是否设置成功
5. 是否有其他行为覆盖了状态（查看 Hook 日志中的调用堆栈）

---

## 问题分析（2024-12-19 第八次更新 - 仍然失败）

### 用户反馈
用户反馈修复后仍然失败，出场仍然不是贵族。

### 可能的问题原因（基于代码分析）

#### 1. 时机问题：`SetClanNoble` 在角色创建阶段被覆盖
**问题**：
- `ApplyMinorNobleOrigin` 在 `OnCharacterCreationFinalized` 时调用
- 此时角色创建流程可能还未完全结束
- 后续的系统初始化可能覆盖了 `IsNoble` 设置

**证据**：
- `SetClanNoble` 可能成功设置了 `IsNoble = true`
- 但后续流程（如 `OnSessionLaunched` 之前的初始化）可能重置了它

#### 2. `JoinKhuzaitAsNoble` 可能没有被调用
**问题**：
- `PendingMinorNobleJoinKhuzait` 标记可能没有正确设置
- 或者 `OnSessionLaunched` 时条件不满足，没有调用 `JoinKhuzaitAsNoble`

**检查点**：
- 日志中是否有 `[OnSessionLaunched] 检测到 PendingMinorNobleJoinKhuzait = true`
- 日志中是否有 `[JoinKhuzaitAsNoble]` 的调用记录

#### 3. `JoinKhuzaitAsNoble` 被调用了，但三个判据不满足
**问题**：
- `ApplyByJoinToKingdom` 可能没有真正执行成功
- 或者执行后被其他行为覆盖

**检查点**：
- 日志中三个判据的值（condition1, condition2, condition3）
- Hook 日志中是否有 `ApplyByJoinToKingdom` 的调用
- Hook 日志中调用前后的状态变化

#### 4. `IsLord` 设置失败
**问题**：
- `SetNewOccupation(Occupation.Lord)` 可能不存在或失败
- 反射调用也可能失败

**检查点**：
- 日志中是否有 `[JoinKhuzaitAsNoble] 设置主角为 Lord 失败`
- 日志中 `IsLord` 和 `Occupation` 的最终值

#### 5. 系统在后续流程中覆盖了状态
**问题**：
- 某个 `CampaignBehavior` 可能在 `OnSessionLaunched` 之后重置了状态
- 或者某个系统初始化流程覆盖了 `IsNoble`

**检查点**：
- Hook 日志中的调用堆栈，看是谁在调用 `ApplyByJoinToKingdom` 或其他相关方法
- 是否有多次调用，最后一次调用覆盖了之前的状态

### 需要检查的日志内容

请从日志中提取以下信息：

#### 1. SetClanNoble 相关
```
[MinorNoble] 已调用 SetClanNoble(clan, true)
[SetClanNoble] 通过 ... 设置成功
[MinorNoble] EnsurePlayerClanIsNoble: after=...
```

#### 2. JoinKhuzaitAsNoble 相关
```
[OnSessionLaunched] 检测到 PendingMinorNobleJoinKhuzait = true
[JoinKhuzaitAsNoble] BEFORE:
[JoinKhuzaitAsNoble] 调用 ChangeKingdomAction.ApplyByJoinToKingdom
[JoinKhuzaitAsNoble] AFTER:
[JoinKhuzaitAsNoble] 最终验证（三个真正判据）:
[JoinKhuzaitAsNoble]   判据1: ... = ...
[JoinKhuzaitAsNoble]   判据2: ... = ...
[JoinKhuzaitAsNoble]   判据3: ... = ...
[JoinKhuzaitAsNoble]   isVassal = ...
[JoinKhuzaitAsNoble]   isLord = ...
```

#### 3. Hook 相关
```
[HOOK ApplyByJoinToKingdom] method=...
[HOOK ApplyByJoinToKingdom] clan=..., kingdom=...
[HOOK-PREFIX-ApplyByJoinToKingdom] ...
[HOOK-POSTFIX-ApplyByJoinToKingdom] ...
[HOOK ApplyByJoinToKingdom] stack:
```

#### 4. 错误相关
```
[Error] ...
[Exception] ...
[Warning] ...
```

### 给 ChatGPT 的问题模板

**问题描述**：
我在 Bannerlord 模组中实现了 `minor_noble` 出身，目标是让玩家开局就成为库塞特王国的封臣（贵族）。但玩家出场时仍然不是贵族。

**当前实现**：
1. 在 `ApplyMinorNobleOrigin` 中：
   - 调用 `SetClanTier(clan, 4)` 设置家族等级为4
   - 调用 `GainRenown(hero, 300)` 设置名望为300
   - 调用 `SetClanNoble(clan, true)` 设置贵族身份
   - 调用 `EnsurePlayerClanIsNoble()` 再次确保
   - 设置 `PendingMinorNobleJoinKhuzait = true` 标记

2. 在 `OnSessionLaunched` 中：
   - 检查 `PendingMinorNobleJoinKhuzait` 标记
   - 调用 `JoinKhuzaitAsNoble()` 加入库塞特王国

3. `JoinKhuzaitAsNoble` 方法：
   - 设置 `Hero.MainHero.SetNewOccupation(Occupation.Lord)`
   - 调用 `ChangeKingdomAction.ApplyByJoinToKingdom(Clan.PlayerClan, khuzaitKingdom, CampaignTime.Now, false)`
   - 使用三个判据验证：`Clan.Kingdom == khuzaitKingdom && !IsUnderMercenaryService && kingdom.Clans.Contains(clan)`

**日志输出**：
[请粘贴日志中关于 `MinorNoble`、`JoinKhuzaitAsNoble`、`SetClanNoble`、`HOOK` 的所有相关行]

**问题**：
1. 为什么设置了 `IsNoble = true` 和加入了王国，玩家仍然不是贵族？
2. 三个判据都满足的情况下，为什么 UI 仍然显示不是贵族？
3. 是否有其他系统行为在覆盖状态？
4. 应该在什么时机设置这些状态？
5. 是否需要其他操作才能让玩家真正成为贵族？

**Bannerlord 版本**：
[请填写版本号]

---

## 最新修复（2024-12-19 第九次更新 - 按照 ChatGPT 建议添加"抓凶手"Hook）

### ChatGPT 的核心建议

1. **一眼判定问题类型**：看 Hook 的 POSTFIX 三个判据是否当场成立
   - 情况 A：当场就不成立（Join 根本没落地）
   - 情况 B：当场成立，但过一会儿又变回去（被覆盖）
   - 情况 C：三条一直成立，但 UI 仍说"不是贵族"（判据用错）

2. **最狠的一招**：Hook `Clan.set_IsNoble`，看是谁把 IsNoble 改回 false

3. **Tier=4 + Renown=300 可能被系统纠正**：提升到 1000+ 避免被降级

### 已实施的修复

#### 1. 添加 Hook_Clan_SetIsNoble（抓凶手）
```csharp
[HarmonyPatch(typeof(Clan), "set_IsNoble")]
internal static class Hook_Clan_SetIsNoble
{
    static void Prefix(Clan __instance, bool value)
    {
        if (__instance != Clan.PlayerClan) return;
        OriginLog.Warning($"[HOOK set_IsNoble] PlayerClan IsNoble: {beforeValue} -> {value}");
        OriginLog.Warning($"[HOOK set_IsNoble] 调用堆栈:\n{Environment.StackTrace}");
    }
}
```

这个 Hook 会记录：
- 谁在设置 `IsNoble`
- 设置的值（true 还是 false）
- 调用堆栈（看是谁调用的）

#### 2. 添加 MercenaryActionHooks（追踪雇佣兵相关调用）
追踪：
- `ApplyByJoinFactionAsMercenary` - 是否被错误地设置为雇佣兵
- `ApplyByLeaveKingdomAsMercenary` - 是否被错误地退出雇佣兵服务

#### 3. 改进 JoinToKingdom Postfix 检查
在 `Postfix` 中特别检查三个判据：
```csharp
if (__originalMethod.Name == "ApplyByJoinToKingdom" && clan == Clan.PlayerClan && kingdom != null)
{
    bool condition1 = clan.Kingdom == kingdom;
    bool condition2 = !clan.IsUnderMercenaryService;
    bool condition3 = kingdom.Clans?.Contains(clan) ?? false;
    
    if (condition1 && condition2 && condition3)
    {
        OriginLog.Info("[HOOK ApplyByJoinToKingdom] ✅ POSTFIX 三个判据都满足，Join 成功落地");
    }
    else
    {
        OriginLog.Warning("[HOOK ApplyByJoinToKingdom] ⚠️ POSTFIX 三个判据不满足，Join 未成功落地");
    }
}
```

#### 4. 调整 Renown 值（从 300 提升到 1000）
```csharp
// 按照 ChatGPT 建议：Tier=4 需要更高的 Renown（至少 900+），避免被系统"纠正"
GainRenown(hero, 1000);
```

#### 5. 添加定期状态检查
在 `OnTick` 中定期调用 `CheckPlayerClanStatus()`，检查状态是否被覆盖。

### 修改的文件

1. ✅ **`KingdomActionHooks.cs`**
   - 添加了 `Hook_Clan_SetIsNoble` Hook
   - 添加了 `MercenaryActionHooks` Hook
   - 改进了 `Postfix` 中的三个判据检查

2. ✅ **`PresetOriginSystem.cs`**
   - 调整 Renown 从 300 到 1000
   - 添加了 `CheckPlayerClanStatus()` 方法

3. ✅ **`OriginSystemCampaignBehavior.cs`**
   - 添加了定期状态检查逻辑

### 判定表（按照 ChatGPT 建议）

运行游戏后，查看日志中的：

#### 1. 一眼判定：Join 真的成功了吗？

查看 `[HOOK ApplyByJoinToKingdom] POSTFIX` 的日志：

**情况 A：当场就不成立（Join 根本没落地）**
- `Clan.PlayerClan.Kingdom != khuzait` 或
- `khuzaitKingdom.Clans.Contains(PlayerClan) == false` 或
- `IsUnderMercenaryService == true`
- ➡️ 这属于"Join 不落地"

**情况 B：当场成立，但过一会儿又变回去**
- POSTFIX 那一刻三条都成立
- 但后面某个 Tick 的日志里又变成：
  - `Kingdom` 变回 null 或换国
  - `IsUnderMercenaryService` 变 true
  - `kingdom.Clans` 不包含你了
- ➡️ 这就是"被覆盖"，也是最常见的

**情况 C：三条一直成立，但 UI/对话仍说"不是贵族"**
- 三条判据一直满足
- 但 UI 仍然显示不是贵族
- ➡️ 这是"判据用错（你其实已经是封臣，但 UI 不认）"

#### 2. 抓凶手：是谁把 IsNoble 改回 false？

查看 `[HOOK set_IsNoble]` 的日志：
- 如果有 `IsNoble: true -> false`，说明被改回去了
- 查看调用堆栈，看是谁调用的

#### 3. 检查雇佣兵相关调用

查看 `[HOOK ApplyByJoinFactionAsMercenary]` 或 `[HOOK ApplyByLeaveKingdomAsMercenary]` 的日志：
- 如果出现，说明被错误地设置为雇佣兵或退出雇佣兵服务

### 需要提取的日志信息

请从日志中提取以下四段信息（ChatGPT 说这四段就够定性）：

1. **`[HOOK ApplyByJoinToKingdom] POSTFIX` 那段**
   - 三个判据的值
   - 是否成功落地

2. **之后任意 1~2 秒内（或几 Tick 后）的状态检查**
   - `[StatusCheck]` 的日志
   - 或 `[JoinKhuzaitAsNoble]` 的最终验证日志

3. **`[HOOK set_IsNoble]` 的调用记录（如果触发）**
   - 是否有 `IsNoble: true -> false`
   - 调用堆栈

4. **任何 `ApplyByJoinFactionAsMercenary` / `LeaveMercenary` 的 hook（如果有）**
   - 是否被错误地设置为雇佣兵

### 下一步

1. 运行游戏，选择 minor_noble 出身
2. 从日志中提取上述四段信息
3. 使用判定表判断是情况 A、B 还是 C
4. 根据判断结果继续修复

---

## 日志分析工具（2024-12-19 新增）

### 创建了日志分析指南
创建了新文档 `日志分析指南_贵族开局问题.md`，包含：
- 如何快速定位日志文件
- 需要提取的四段信息及其搜索关键词
- 判定表（情况 A/B/C）的详细说明
- 分析步骤和常见问题

### Hook 代码检查

已确认所有 Hook 代码正确：
1. ✅ `KingdomActionHooks` - 追踪 `ApplyByJoinToKingdom` 等
2. ✅ `Hook_Clan_SetIsNoble` - 追踪 `IsNoble` 的设置（抓凶手）
3. ✅ `MercenaryActionHooks` - 追踪雇佣兵相关调用

### 关键改进

在 `Hook_Clan_SetIsNoble` 中添加了特别警告：
- 如果检测到 `IsNoble: true -> false`，会输出 `⚠️⚠️⚠️ 检测到 IsNoble 被从 true 改为 false！`
- 这样更容易在日志中找到问题

### 下一步操作

1. **运行游戏**，选择 minor_noble 出身
2. **查找日志文件**（参考日志分析指南）
3. **提取四段信息**（使用指南中的搜索关键词）
4. **使用判定表判断**是情况 A、B 还是 C
5. **根据判断结果**继续修复或向 ChatGPT 提问

### 下一步

1. 从日志中提取上述信息
2. 使用上面的问题模板向 ChatGPT 提问
3. 根据 ChatGPT 的建议继续修复

