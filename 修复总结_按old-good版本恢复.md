# 修复总结：按 old-good 版本恢复

## 修复的文件列表

### 1. `SubModule/Routing/OriginMenuRouter.cs`
- **修改**：在 `ResolveNextMenuId` 开头添加优先检查 `PendingMenuSwitch` 的逻辑
- **原因**：old-good 版本的核心原则：Node 的 OnSelect 只记录状态，路由由统一路由器接管

### 2. `SubModule/OriginSystemPatches.cs`
- **修改**：移除 `OnNarrativeMenuOptionSelected` Postfix 中清空 `PendingMenuSwitch` 的逻辑
- **原因**：`PendingMenuSwitch` 必须保留到 `TrySwitchToNextMenu` 调用时，由 `ResolveNextMenuId` 使用并清空

### 3. `SubModule/PresetOriginSystem.cs`
- **修改**：在 `ApplySlaveEscapeNode5` 中恢复直接调用 `SetSlaveEscapeStartingLocation`（old-good 方式）
- **原因**：old-good 版本在 `ApplySlaveEscapeNode5` 中直接调用，不依赖 Patch

### 4. `SubModule/OriginSystemHelper.cs`
- **已有**：`ResetState` 中已有日志，会警告如果清空了 `PendingStartDirection`
- **状态**：✅ 已符合要求

### 5. `SubModule/PresetOriginSystem.cs` (SetSlaveEscapeStartingLocation)
- **已有**：使用 `GetSetMethod(true)` 并 `Invoke` 设置 Position2D
- **状态**：✅ 已符合要求

## 为什么这样改（对应 old-good vs current-bad）

### 修复点1：路由使用 PendingMenuSwitch（最关键）
- **old-good**：`ResolveNextMenuId` 优先使用 `PendingMenuSwitch`
- **current-bad**：`ResolveNextMenuId` 忽略了 `PendingMenuSwitch`，只根据 `curMenuId` 和 `optId` 路由
- **修复**：在 `ResolveNextMenuId` 开头添加优先检查 `PendingMenuSwitch` 的逻辑

### 修复点2：不提前清空 PendingMenuSwitch
- **old-good**：`PendingMenuSwitch` 保留到 `TrySwitchToNextMenu` 调用时
- **current-bad**：`OnNarrativeMenuOptionSelected` Postfix 提前清空了 `PendingMenuSwitch`
- **修复**：移除 Postfix 中清空 `PendingMenuSwitch` 的逻辑

### 修复点3：出生位置直接调用（保证执行）
- **old-good**：在 `ApplySlaveEscapeNode5` 中直接调用 `SetSlaveEscapeStartingLocation(party, "desert")`
- **current-bad**：只保存到 Pending，依赖 Patch/OnTick 兜底
- **修复**：恢复在 `ApplySlaveEscapeNode5` 中直接调用

## 关键日志样例（必须出现的序列）

### 路由链路（预设出身选择）
```
[OS] Select: menu=origin_type_selection option=preset_origin_option hasSelectedKey=False
[OS] [OriginSystem] 用户选择了预设出身 (仅标记,不切菜单)
[OS] Postfix: PendingMenuSwitch=preset_origin_selection (保留给 TrySwitchToNextMenu 使用)
[OS] Switch: cur=origin_type_selection opt=preset_origin_option resolved=preset_origin_selection
[OS] [Route] 使用 PendingMenuSwitch: preset_origin_selection (cur=origin_type_selection opt=preset_origin_option)
[OS] ForceSwitch: target=preset_origin_selection found=True setCurrent=True modifyCalled=True
```

### 逃奴出生位置设置（Node5）
```
[OS] 用户选择战奴逃亡-Node5-留沙
[OS] [SlaveEscape][Node5] 已保存期望出生位置: direction=desert
[OS] [SlaveEscape][Apply] direction=desert campaign=True mainHero=True mainParty=True pos=(...)
[OS] [SlaveEscape][Apply] 在 ApplySlaveEscapeNode5 中直接调用 SetSlaveEscapeStartingLocation: desert
[OS] [SlaveEscape][Teleport] when=延迟执行 before=(...) after=(...) success=True/False
```

### ResetState 警告（如果清空了 PendingStartDirection）
```
[OS][WARN] [ResetState] 清空了 PendingStartDirection=desert PendingStartSettlementId=null
```

### Finalize Patch（如果触发）
```
[OS] [SlaveEscape][Finalize] OnCharacterCreationFinalized Postfix called
[OS] [SlaveEscape][Finalize] 开始设置出生位置: direction=desert
[OS] [SlaveEscape][Teleport] when=OnCharacterCreationFinalized before=(...) after=(...) success=True/False
```

## 编译结果

```
编译命令: msbuild OriginSystemMod.csproj /p:Configuration=Release /p:Platform=x64 /nologo /v:minimal
编译结果: ✅ 0 errors, 0 warnings
输出文件: OriginSystemMod.dll
```

## 验证清单

- [x] 编译通过（0 errors, 0 warnings）
- [x] `ResolveNextMenuId` 优先使用 `PendingMenuSwitch`
- [x] `OnNarrativeMenuOptionSelected` Postfix 不清空 `PendingMenuSwitch`
- [x] `ApplySlaveEscapeNode5` 中直接调用 `SetSlaveEscapeStartingLocation`
- [x] `SetSlaveEscapeStartingLocation` 使用 `GetSetMethod(true)` 设置 Position2D
- [x] `ResetState` 有日志警告如果清空了 `PendingStartDirection`
- [ ] 运行时验证：选择预设出身 → 选择"战奴逃亡" → 选择"沙漠深处" → 查看日志序列
- [ ] 运行时验证：菜单正确跳转到 `preset_slave_escape`（Node1）
- [ ] 运行时验证：出生位置正确设置到阿塞莱沙漠城市









































