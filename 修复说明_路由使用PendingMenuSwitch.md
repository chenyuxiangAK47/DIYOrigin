# 修复说明：路由使用 PendingMenuSwitch

## 问题发现

根据 ChatGPT 的建议，之前"能跑"的实现方式是：
1. **Node 的 OnSelect 只记录状态**（设置 `PendingMenuSwitch`），不切菜单 ✅
2. **统一路由 Patch 接管菜单跳转**（`TrySwitchToNextMenuPatch`）✅
3. **路由器中优先使用 `PendingMenuSwitch`** ❌ **缺失**

## 当前实现的问题

### ✅ 符合原则的部分

1. **菜单注入**：`RecreateParentMenuWithNewInputId` 存在 ✅
2. **OnSelect 只记录状态**：`SlaveDirectionDesertOnSelect` 等只设置 `PendingMenuSwitch`，不切菜单 ✅
3. **统一路由 Patch**：`TrySwitchToNextMenuPatch` 存在，直接设置 `CurrentMenu` ✅

### ❌ 不符合原则的部分

**`OriginMenuRouter.ResolveNextMenuId` 没有使用 `PendingMenuSwitch`！**

- OnSelect 中设置了 `PendingMenuSwitch = "preset_slave_escape"` 等
- 但 `ResolveNextMenuId` 完全忽略了它，只根据 `curMenuId` 和 `optId` 路由
- 这导致菜单跳转可能不生效

## 修复方案

在 `OriginMenuRouter.ResolveNextMenuId` 的开头添加：

```csharp
// ====== 0) 优先检查 PendingMenuSwitch（OnSelect 中设置的目标菜单）=====
// 这是"能跑"版本的核心：Node 的 OnSelect 只记录状态，路由由这里统一接管
if (!string.IsNullOrEmpty(OriginSystemHelper.PendingMenuSwitch))
{
    string targetMenuId = OriginSystemHelper.PendingMenuSwitch;
    OriginLog.Info($"[Route] 使用 PendingMenuSwitch: {targetMenuId} (cur={curMenuId} opt={optId})");
    // 清空 PendingMenuSwitch，避免影响后续路由
    OriginSystemHelper.PendingMenuSwitch = null;
    return targetMenuId;
}
```

## 修复后的行为

1. **OnSelect 设置 `PendingMenuSwitch`**：
   ```csharp
   OriginSystemHelper.PendingMenuSwitch = "preset_slave_escape";
   ```

2. **TrySwitchToNextMenuPatch 调用 `ResolveNextMenuId`**：
   ```csharp
   string nextMenuId = OriginMenuRouter.ResolveNextMenuId(cm.StringId, optId);
   ```

3. **`ResolveNextMenuId` 优先使用 `PendingMenuSwitch`**：
   ```csharp
   if (!string.IsNullOrEmpty(OriginSystemHelper.PendingMenuSwitch))
   {
       return OriginSystemHelper.PendingMenuSwitch; // 直接返回，不根据 curMenuId/optId 路由
   }
   ```

4. **清空 `PendingMenuSwitch`**，避免影响后续路由

## 预期日志

修复后，应该看到：

```
[OS] 用户选择战奴逃亡-Node5-留沙
[OS] [Route] 使用 PendingMenuSwitch: preset_slave_escape (cur=preset_origin_selection opt=preset_slave_escape_option)
[OS] Switch: cur=preset_origin_selection opt=preset_slave_escape_option resolved=preset_slave_escape
[OS] ForceSwitch: target=preset_slave_escape found=True setCurrent=True modifyCalled=True
```

## 验证清单

- [ ] 编译通过（0 errors, 0 warnings）
- [ ] 选择预设出身 → 选择"战奴逃亡" → 日志中出现 `[Route] 使用 PendingMenuSwitch: preset_slave_escape`
- [ ] 菜单正确跳转到 `preset_slave_escape`（Node1）
- [ ] 后续节点流转正常（Node1 → Node2 → ... → Node5）
- [ ] `PendingMenuSwitch` 在使用后被清空（不会影响后续路由）









































