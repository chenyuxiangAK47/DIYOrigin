# OriginSystemMod 崩溃分析：TrySwitchToNextMenu 字典键缺失问题

## 问题描述

在角色创建界面选择"预设出身"选项后，游戏崩溃。错误信息显示 `TrySwitchToNextMenu()` 方法在访问 `SelectedOptions` 字典时找不到键。

## 日志信息

### 关键日志片段

```
[19:19:08.655] [OriginSystem] 用户选择了'预设出身' 

[19:19:09.455] Exception occurred inside invoke: OnNextStage
Target type: TaleWorlds.CampaignSystem.ViewModelCollection.CharacterCreation.CharacterCreationNarrativeStageVM
Argument count: 0
Inner message: The given key was not present in the dictionary.
```

### 错误特征

- **错误位置**：`TrySwitchToNextMenu()` 方法内部
- **错误类型**：`KeyNotFoundException`（"The given key was not present in the dictionary"）
- **触发时机**：用户选择"预设出身"后，调用 `TrySwitchToNextMenu()` 时

## 代码分析

### TrySwitchToNextMenu 的实现（反编译代码）

```csharp
public bool TrySwitchToNextMenu()
{
    string stringId = CurrentMenu.StringId;
    SelectedOptions[CurrentMenu].OnConsequence(this);  // ⚠️ 第 232 行：这里崩溃
    foreach (NarrativeMenu narrativeMenu in NarrativeMenus)
    {
        if (narrativeMenu.InputMenuId.Equals(stringId))
        {
            CurrentMenu = narrativeMenu;
            ModifyMenuCharacters();
            return true;
        }
    }
    return false;
}
```

### 问题根源

**`TrySwitchToNextMenu()` 在第 232 行访问 `SelectedOptions[CurrentMenu]` 时，字典中没有 `CurrentMenu` 这个键。**

这说明：
1. `OnNarrativeMenuOptionSelected(option)` 应该会将选择添加到 `SelectedOptions` 字典
2. 但在调用 `TrySwitchToNextMenu()` 时，字典中却没有对应的键

### 当前实现（有问题的版本）

```csharp
private static void PresetOriginOnSelect(CharacterCreationManager characterCreationManager)
{
    // Re-entrancy guard
    if (_isProcessingPresetOrigin)
    {
        ThrottledLog("[OriginSystem] 警告：PresetOriginOnSelect 正在处理中，跳过递归调用", Debug.DebugColor.Yellow);
        return;
    }
    
    try
    {
        _isProcessingPresetOrigin = true;
        ThrottledLog("[OriginSystem] 用户选择了'预设出身'", Debug.DebugColor.Green);
        OriginSystemHelper.IsPresetOrigin = true;
        
        // 优先使用引擎的状态机：TrySwitchToNextMenu()
        if (characterCreationManager.CurrentMenu == null)
        {
            characterCreationManager.StartNarrativeStage();
        }
        
        // 尝试切换到下一个菜单
        if (characterCreationManager.TrySwitchToNextMenu())  // ⚠️ 这里崩溃
        {
            ThrottledLog("[OriginSystem] 已通过 TrySwitchToNextMenu() 切换到预设出身选择菜单", Debug.DebugColor.Green);
        }
        else
        {
            // 兜底：反射手动切换
        }
    }
    catch (Exception ex)
    {
        Debug.Print("[OriginSystem] PresetOriginOnSelect 失败: " + ex.Message, 0, Debug.DebugColor.Red);
    }
    finally
    {
        _isProcessingPresetOrigin = false;
    }
}
```

### 问题分析

**关键问题：`OnSelect` 回调是在 `OnNarrativeMenuOptionSelected` 内部被调用的，但调用顺序是：**

1. UI 层调用 `OnNarrativeMenuOptionSelected(presetOption)`
2. `OnNarrativeMenuOptionSelected` 内部：
   ```csharp
   SelectedOptions[CurrentMenu] = option;  // 先写入字典
   option.OnSelect(this);  // 然后调用 OnSelect 回调
   ```
3. `OnSelect` 回调（即 `PresetOriginOnSelect`）中调用 `TrySwitchToNextMenu()`
4. `TrySwitchToNextMenu()` 访问 `SelectedOptions[CurrentMenu]`

**但是，这里有一个潜在问题：**

- `OnNarrativeMenuOptionSelected` 写入的是 `SelectedOptions[CurrentMenu]`，其中 `CurrentMenu` 是**调用时的当前菜单**
- 如果在 `OnSelect` 回调中，`CurrentMenu` 已经被改变了（或者有其他原因导致不一致），那么 `TrySwitchToNextMenu()` 访问 `SelectedOptions[CurrentMenu]` 时就会找不到键

### 可能的原因

1. **`CurrentMenu` 在 `OnSelect` 回调执行过程中被改变了**
   - 虽然不太可能，但如果其他代码修改了 `CurrentMenu`，就会导致不一致

2. **`OnNarrativeMenuOptionSelected` 没有被正确调用**
   - 如果 UI 层没有调用 `OnNarrativeMenuOptionSelected`，而是直接调用了 `OnSelect`，那么 `SelectedOptions` 字典就不会被填充

3. **菜单对象引用不一致**
   - 如果 `CurrentMenu` 对象在 `OnNarrativeMenuOptionSelected` 和 `TrySwitchToNextMenu()` 之间被替换了（虽然引用相同，但对象不同），也会导致找不到键

4. **`OnSelect` 回调在 `OnNarrativeMenuOptionSelected` 之前被调用**
   - 如果调用顺序不对，`SelectedOptions` 字典还没有被填充

## 解决方案

### 方案 1：在 OnSelect 中先检查 SelectedOptions（推荐）

在调用 `TrySwitchToNextMenu()` 之前，先检查 `SelectedOptions` 字典中是否有 `CurrentMenu` 的键：

```csharp
private static void PresetOriginOnSelect(CharacterCreationManager characterCreationManager)
{
    if (_isProcessingPresetOrigin)
    {
        ThrottledLog("[OriginSystem] 警告：PresetOriginOnSelect 正在处理中，跳过递归调用", Debug.DebugColor.Yellow);
        return;
    }
    
    try
    {
        _isProcessingPresetOrigin = true;
        ThrottledLog("[OriginSystem] 用户选择了'预设出身'", Debug.DebugColor.Green);
        OriginSystemHelper.IsPresetOrigin = true;
        
        var currentMenu = characterCreationManager.CurrentMenu;
        if (currentMenu == null)
        {
            characterCreationManager.StartNarrativeStage();
            currentMenu = characterCreationManager.CurrentMenu;
        }
        
        // ⚠️ 关键：先检查 SelectedOptions 字典中是否有当前菜单的键
        if (!characterCreationManager.SelectedOptions.ContainsKey(currentMenu))
        {
            ThrottledLog("[OriginSystem] 警告：SelectedOptions 字典中没有当前菜单的键，尝试手动添加", Debug.DebugColor.Yellow);
            
            // 找到"预设出身"选项
            var presetOption = currentMenu.CharacterCreationMenuOptions.FirstOrDefault(
                opt => opt.StringId == "preset_origin_option");
            if (presetOption != null)
            {
                // 手动添加到字典
                characterCreationManager.SelectedOptions[currentMenu] = presetOption;
                ThrottledLog("[OriginSystem] 已手动添加选择到 SelectedOptions 字典", Debug.DebugColor.Green);
            }
            else
            {
                ThrottledLog("[OriginSystem] 错误：找不到'预设出身'选项", Debug.DebugColor.Red);
                return;
            }
        }
        
        // 现在可以安全地调用 TrySwitchToNextMenu()
        if (characterCreationManager.TrySwitchToNextMenu())
        {
            ThrottledLog("[OriginSystem] 已通过 TrySwitchToNextMenu() 切换到预设出身选择菜单", Debug.DebugColor.Green);
        }
        else
        {
            // 兜底：反射手动切换
            ThrottledLog("[OriginSystem] TrySwitchToNextMenu() 失败，使用反射手动切换", Debug.DebugColor.Yellow);
            // ... 反射代码
        }
    }
    catch (Exception ex)
    {
        Debug.Print("[OriginSystem] PresetOriginOnSelect 失败: " + ex.Message, 0, Debug.DebugColor.Red);
        Debug.Print("[OriginSystem] StackTrace: " + ex.StackTrace, 0, Debug.DebugColor.Red);
    }
    finally
    {
        _isProcessingPresetOrigin = false;
    }
}
```

### 方案 2：不使用 TrySwitchToNextMenu，直接反射切换菜单

如果 `TrySwitchToNextMenu()` 总是失败，可以考虑直接使用反射切换菜单，跳过 `TrySwitchToNextMenu()`：

```csharp
private static void PresetOriginOnSelect(CharacterCreationManager characterCreationManager)
{
    if (_isProcessingPresetOrigin)
    {
        return;
    }
    
    try
    {
        _isProcessingPresetOrigin = true;
        OriginSystemHelper.IsPresetOrigin = true;
        
        // 直接使用反射切换菜单，不调用 TrySwitchToNextMenu()
        var presetMenu = characterCreationManager.GetNarrativeMenuWithId("preset_origin_selection");
        if (presetMenu != null)
        {
            var currentMenuField = typeof(CharacterCreationManager).GetField("CurrentMenu",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (currentMenuField != null)
            {
                currentMenuField.SetValue(characterCreationManager, presetMenu);
                var modifyMethod = typeof(CharacterCreationManager).GetMethod("ModifyMenuCharacters",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                modifyMethod?.Invoke(characterCreationManager, null);
                ThrottledLog("[OriginSystem] 已通过反射切换到预设出身选择菜单", Debug.DebugColor.Green);
            }
        }
    }
    catch (Exception ex)
    {
        Debug.Print("[OriginSystem] PresetOriginOnSelect 失败: " + ex.Message, 0, Debug.DebugColor.Red);
    }
    finally
    {
        _isProcessingPresetOrigin = false;
    }
}
```

### 方案 3：延迟调用 TrySwitchToNextMenu

如果问题是时序问题，可以考虑延迟调用 `TrySwitchToNextMenu()`，或者使用 Harmony Patch 在 `OnNarrativeMenuOptionSelected` 的 Postfix 中调用：

```csharp
[HarmonyPatch(typeof(CharacterCreationManager), "OnNarrativeMenuOptionSelected")]
public static class OnNarrativeMenuOptionSelectedPatch
{
    static void Postfix(CharacterCreationManager __instance, NarrativeMenuOption option)
    {
        // 在 OnNarrativeMenuOptionSelected 完成后，检查是否需要切换菜单
        // 这里可以安全地访问 SelectedOptions 字典
    }
}
```

## 需要 ChatGPT 确认的问题

1. **`OnSelect` 回调的执行时机**：
   - `OnSelect` 是在 `OnNarrativeMenuOptionSelected` 内部被调用的吗？
   - 在 `OnSelect` 回调执行时，`SelectedOptions` 字典是否已经被填充？

2. **`CurrentMenu` 的一致性**：
   - 在 `OnSelect` 回调中，`CurrentMenu` 是否与 `OnNarrativeMenuOptionSelected` 调用时相同？
   - 是否存在其他代码会修改 `CurrentMenu`？

3. **`TrySwitchToNextMenu()` 的使用场景**：
   - `TrySwitchToNextMenu()` 是否应该在 `OnSelect` 回调中调用？
   - 还是应该在 `OnSelect` 回调之外调用？

4. **菜单切换的最佳实践**：
   - 在 `OnSelect` 回调中，应该使用 `TrySwitchToNextMenu()` 还是直接反射切换菜单？
   - 如果使用 `TrySwitchToNextMenu()`，如何确保 `SelectedOptions` 字典中有正确的键？

5. **`OnConsequence` 回调的作用**：
   - `TrySwitchToNextMenu()` 中调用 `SelectedOptions[CurrentMenu].OnConsequence(this)` 的作用是什么？
   - 如果 `OnConsequence` 为 `null`，会发生什么？

## 相关文件

- `OriginSystemMod/SubModule/OriginSystemPatches.cs` - 包含 `PresetOriginOnSelect` 方法
- `D:\Bannerlord_Decompiled\TaleWorlds.CampaignSystem\TaleWorlds.CampaignSystem.CharacterCreationContent\CharacterCreationManager.cs` - 反编译的游戏源码

## 下一步

1. 实现方案 1：在 `OnSelect` 中先检查 `SelectedOptions` 字典
2. 如果方案 1 不行，尝试方案 2：直接使用反射切换菜单
3. 如果还是不行，考虑方案 3：使用 Harmony Patch 延迟调用


























































