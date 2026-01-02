# 修复说明：按 old-good 版本恢复 OnCharacterCreationFinalizedPatch

## 🔍 问题分析

从 `cursor_mod.md` 中看到，old-good 版本的实现是：

**关键区别：**
- old-good 版本：`TargetType()` 和 `TargetMethod()` 都是**简单直接**的实现，没有 try-catch
- current-bad 版本：添加了大量 try-catch，但 Harmony 仍然抛异常

**old-good 版本的实现：**
```csharp
static Type TargetType()
{
    var type = Type.GetType("SandBox.CharacterCreationContent.SandboxCharacterCreationContent, SandBox");
    if (type == null)
    {
        type = Type.GetType("TaleWorlds.CampaignSystem.CharacterCreationContent.CharacterCreationContent, TaleWorlds.CampaignSystem");
    }
    return type;
}

static MethodBase TargetMethod()
{
    var t = TargetType();
    if (t == null) return null;
    
    var method = AccessTools.Method(t, "OnCharacterCreationFinalized");
    if (method == null)
    {
        method = AccessTools.Method(t, "FinalizeCharacterCreation");
    }
    return method;
}
```

## 🔧 修复方案

按照 old-good 版本恢复简单实现，同时在 `OnSubModuleLoad` 中添加容错机制：

1. **恢复 old-good 版本的简单实现**（移除所有 try-catch）
2. **在 `OnSubModuleLoad` 中添加容错机制**：如果 `PatchAll` 失败，手动注册其他 Patch

## ✅ 修改内容

### 1. 恢复 `OnCharacterCreationFinalizedPatch` 为 old-good 版本

**文件：** `SubModule/OriginSystemPatches.cs`

**修改：** 移除所有 try-catch，恢复为简单实现

### 2. 在 `OnSubModuleLoad` 中添加容错机制

**文件：** `SubModule/OriginSystemSubModule.cs`

**修改：** 如果 `PatchAll` 失败，手动注册其他 Patch（除了 `OnCharacterCreationFinalizedPatch`）

## 📋 预期结果

修复后，即使 `OnCharacterCreationFinalizedPatch` 失败，其他 Patch 仍然能注册：

1. **如果 PatchAll 成功：**
   ```
   [OriginSystem] Harmony PatchAll 完成
   ```

2. **如果 PatchAll 部分失败：**
   ```
   [OriginSystem] Harmony PatchAll 部分失败: ...
   [OriginSystem] 尝试手动注册其他 Patch...
   [OriginSystem] 手动注册 Patch: OnNarrativeMenuOptionSelectedPatch
   [OriginSystem] 手动注册 Patch: TrySwitchToNextMenuPatch
   ...
   ```

3. **路由相关 Patch 应该能工作：**
   - `Select: menu=origin_type_selection option=preset_origin_option`
   - `Switch: cur=origin_type_selection opt=preset_origin_option resolved=...`

## 📝 修改统计

- **修改文件数：** 2
- **修改行数：** ~30 行
- **编译状态：** ✅ 0 errors 0 warnings








































