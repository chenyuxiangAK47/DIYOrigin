# 问题分析：OnSessionLaunched 过早调用导致状态为空

**日期：** 2024-12-19  
**问题：** 选择"战奴逃亡" → "逃向沙漠深处"后，出生位置仍在马凯布，而不是预期的古亚兹最南端村庄往南的沙漠深处

---

## 🔍 日志分析（最新测试）

### 关键日志时间线

```
[15:56:36.983] [ResetState] called
  at OriginSystemMod.OriginSystemSubModule.OnBeforeInitialModuleScreenSetAsRoot()
  → 游戏启动，进入主菜单，状态被清空（此时角色创建还没开始）

[15:57:03.034] [OnSessionLaunched] called
[15:57:03.034] [OnSessionLaunched] IsPresetOrigin=False  ← 状态已被清空！
[15:57:03.034] [OnSessionLaunched] SelectedPresetOriginId=null
[15:57:03.034] [OnSessionLaunched] PendingStartDirection=null
  → OnSessionLaunched 在角色创建流程开始之前就被调用了！
  → 此时用户还没有选择预设出身，所以状态是空的
  → ApplyPresetOrigin 没有被调用（因为条件不满足）

[15:57:16.216] Select: menu=preset_origin_selection option=khuzait_slave_escape
  → 用户选择预设出身，状态才被设置

[15:58:08.450] [SlaveEscape][Node5] 已保存期望出生位置: direction=desert
  → 用户选择方向，状态被设置

[15:58:25.431] [OnBeforeInitialModuleScreenSetAsRoot] SKIP ResetState: IsPresetOrigin=True PendingStartDirection=desert
  → guard 生效了，但已经太晚了（OnSessionLaunched 不会再被调用）
```

---

## 🐛 根本原因

### 问题：OnSessionLaunched 在角色创建完成之前就被触发了

**生命周期问题：**

1. **游戏启动阶段**
   - `OnBeforeInitialModuleScreenSetAsRoot` 被调用 → `ResetState` 清空状态
   - `OnGameStart` 被调用 → `CampaignBehavior` 被注册
   - **`OnSessionLaunched` 被调用** ← **问题在这里！**
     - 此时角色创建流程还没开始
     - 用户还没有选择预设出身
     - 状态是空的（`IsPresetOrigin=False`）
     - `ApplyPresetOrigin` 没有被调用

2. **角色创建阶段**
   - 用户选择预设出身 → 状态被设置（`IsPresetOrigin=True`）
   - 用户完成所有节点选择 → 状态完整（`PendingStartDirection=desert`）
   - **但 `OnSessionLaunched` 不会再被调用！**

3. **游戏开始阶段**
   - 角色创建完成，进入地图
   - 但预设出身效果没有被应用（因为 `OnSessionLaunched` 已经执行过了）

---

## ✅ 修复方案

### 方案1：在 OnCharacterCreationFinalized 中应用预设出身（推荐）

**文件：** `SubModule/OriginSystemPatches.cs`

**修改：** 在 `OnCharacterCreationFinalized` 的 Postfix 中调用 `ApplyPresetOrigin`

```csharp
[HarmonyPostfix]
public static void Postfix()
{
    try
    {
        OriginLog.Info("[SlaveEscape][Finalize] OnCharacterCreationFinalized Postfix called");
        
        // 检查是否是预设出身
        if (OriginSystemHelper.IsPresetOrigin && 
            !string.IsNullOrEmpty(OriginSystemHelper.SelectedPresetOriginId))
        {
            OriginLog.Info($"[Finalize] 开始应用预设出身: {OriginSystemHelper.SelectedPresetOriginId}");
            PresetOriginSystem.ApplyPresetOrigin(OriginSystemHelper.SelectedPresetOriginId);
            OriginLog.Info($"[Finalize] 已应用预设出身: {OriginSystemHelper.SelectedPresetOriginId}");
        }
        else if (!OriginSystemHelper.IsPresetOrigin)
        {
            OriginLog.Info("[Finalize] 开始应用非预设出身");
            NonPresetOriginSystem.ApplyNonPresetOrigin();
            OriginLog.Info("[Finalize] 已应用非预设出身");
        }
    }
    catch (Exception ex)
    {
        OriginLog.Error($"[Finalize] 失败: {ex.Message}");
        OriginLog.Error($"[Finalize] StackTrace: {ex.StackTrace}");
    }
}
```

**优点：**
- 在角色创建完成后立即应用，时机正确
- 不依赖 `OnSessionLaunched` 的调用时机
- 状态已经完整，可以正确应用

### 方案2：在 OnTick 中检查并应用（兜底方案）

**文件：** `SubModule/OriginSystemCampaignBehavior.cs`

**修改：** 在 `OnTick` 中检查状态，如果状态已经设置但还没应用，就应用它

```csharp
private void OnTick(float dt)
{
    // 检查是否需要应用预设出身（如果 OnSessionLaunched 时状态还没设置）
    if (!_hasAppliedPresetOrigin &&
        OriginSystemHelper.IsPresetOrigin &&
        !string.IsNullOrEmpty(OriginSystemHelper.SelectedPresetOriginId))
    {
        OriginLog.Info($"[OnTick] 检测到预设出身未应用，开始应用: {OriginSystemHelper.SelectedPresetOriginId}");
        PresetOriginSystem.ApplyPresetOrigin(OriginSystemHelper.SelectedPresetOriginId);
        _hasAppliedPresetOrigin = true;
    }
    
    // ... 现有的 teleport 逻辑 ...
}
```

**优点：**
- 作为兜底方案，确保预设出身一定会被应用
- 不改变现有逻辑，风险低

**缺点：**
- 可能延迟应用（需要等待 OnTick）
- 需要额外的标志位

---

## 📋 推荐修复方案

**推荐使用方案1**，因为：
1. 时机最准确：在角色创建完成后立即应用
2. 不依赖 `OnSessionLaunched` 的调用时机
3. 状态已经完整，可以正确应用

**同时保留方案2作为兜底**，确保即使 `OnCharacterCreationFinalized` 失败，也能在 `OnTick` 中应用。

---

## 📋 验证清单（测试时必须检查的日志）

### 必须出现的日志（按时间顺序）

1. **角色创建完成**
   ```
   [SlaveEscape][Finalize] OnCharacterCreationFinalized Postfix called
   ```

2. **应用预设出身（在 Finalize 中）**
   ```
   [Finalize] 开始应用预设出身: khuzait_slave_escape
   [ApplyPresetOrigin] originId=khuzait_slave_escape
   [Finalize] 已应用预设出身: khuzait_slave_escape
   ```

3. **ApplySlaveEscapeNode5 调用**
   ```
   [SlaveEscape][Apply] ApplySlaveEscapeNode5 被调用，nodes.Count=5
   [SlaveEscape][Apply] direction=desert
   ```

4. **位置设置**
   ```
   [SlaveEscape][Teleport] 找到 Quyaz 城市: Quyaz (town_A1)
   [SlaveEscape][Teleport] 找到 Quyaz 最南端村庄: ...
   [SlaveEscape][Teleport] 使用 setMethod.Invoke 设置位置: success=True
   ```

### 如果缺少某个日志，说明的问题

- **缺少 [SlaveEscape][Finalize]：** `OnCharacterCreationFinalized` 没有被调用（可能是 Patch 失败）
- **缺少 [Finalize] 开始应用预设出身：** 条件不满足（状态为空）
- **缺少 [ApplyPresetOrigin]：** `ApplyPresetOrigin` 没有被调用
- **缺少 [SlaveEscape][Apply]：** `ApplySlaveEscapeNode5` 没有被调用
- **缺少 [SlaveEscape][Teleport]：** `SetSlaveEscapeStartingLocation` 没有被调用或失败

---

## 🔄 修复历史

1. **第一次修复：** ResetState 加保护条件（方案1）
   - ✅ 已实施
   - ✅ 保护条件生效（第二次 ResetState 调用时）

2. **第二次修复：** OnBeforeInitialModuleScreenSetAsRoot 加 guard（方案2）
   - ✅ 已实施
   - ✅ 双重保护，确保状态不会被过早清空

3. **第三次修复（当前）：** OnSessionLaunched 过早调用问题
   - ✅ **已实施**：在 `OnCharacterCreationFinalized` 中应用预设出身
   - ✅ **已实施**：在 `OnTick` 中添加兜底检查

---

## 📝 总结

**问题根源：** `OnSessionLaunched` 在角色创建流程开始之前就被调用了，此时用户还没有选择预设出身，状态是空的。即使后来用户选择了预设出身，`OnSessionLaunched` 也不会再被调用，导致预设出身效果没有被应用。

**修复方案：** 在 `OnCharacterCreationFinalized` 中应用预设出身，而不是在 `OnSessionLaunched` 中。这样可以在角色创建完成后立即应用，时机正确，状态完整。

**预期结果：** 角色创建完成后，预设出身效果应该被正确应用，出生位置应该被正确设置。

