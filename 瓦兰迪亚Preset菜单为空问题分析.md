# 瓦兰迪亚Preset选择菜单为空问题分析

## 问题描述

在Mount & Blade II: Bannerlord游戏中，当玩家选择瓦兰迪亚文化后，进入"Choose Preset Origin"菜单时，菜单是空的，没有任何选项显示。

## 代码实现

### 1. 菜单创建代码 (`PresetOriginSelectionMenu.cs`)

```csharp
private static NarrativeMenu CreatePresetOriginSelectionMenu(CharacterCreationManager manager)
{
    var characters = new List<NarrativeMenuCharacter>();
    var menu = new NarrativeMenu(
        "preset_origin_selection",
        "origin_type_selection",
        "narrative_parent_menu",
        new TextObject("Choose Preset Origin"),
        new TextObject("Choose a preset origin background"),
        characters,
        GetPresetOriginMenuCharacterArgs
    );

    // 获取当前选择的文化
    CultureObject selectedCulture = null;
    try
    {
        var contentProp = manager.GetType().GetProperty("CharacterCreationContent",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
        if (contentProp != null)
        {
            var content = contentProp.GetValue(manager, null);
            if (content != null)
            {
                var cultureProp = content.GetType().GetProperty("SelectedCulture",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                if (cultureProp != null)
                {
                    selectedCulture = cultureProp.GetValue(content, null) as CultureObject;
                }
            }
        }
    }
    catch (Exception ex)
    {
        OriginLog.Error($"Failed to get selected culture: {ex.Message}");
    }

    string cultureId = selectedCulture?.StringId?.ToLower() ?? "";
    bool isVlandia = cultureId == "vlandia" || cultureId.Contains("vlandia");
    bool isKhuzait = cultureId == "khuzait" || cultureId.Contains("khuzait");
    // ... 其他文化检测

    OriginLog.Info($"Preset origin selection: cultureId={cultureId}, isVlandia={isVlandia}, isKhuzait={isKhuzait}");

    // 瓦兰迪亚预设出身选项
    if (isVlandia)
    {
        menu.AddNarrativeMenuOption(new NarrativeMenuOption(
            "vlandia_expedition_knight",
            new TextObject("远征的骑士"),
            new TextObject("远征归来的骑士，拥有丰富的战斗经验和声望"),
            GetExpeditionKnightArgs,
            ExpeditionKnightOnCondition,
            ExpeditionKnightOnSelect,
            null
        ));
        // ... 其他9个瓦兰迪亚选项
    }

    // 库塞特预设出身选项
    if (isKhuzait)
    {
        // ... 库塞特选项
    }

    return menu;
}
```

### 2. OnCondition方法

所有瓦兰迪亚选项的OnCondition方法都返回true：

```csharp
private static bool ExpeditionKnightOnCondition(CharacterCreationManager characterCreationManager) { return true; }
private static bool BankruptBanneretOnCondition(CharacterCreationManager characterCreationManager) { return true; }
// ... 其他都是 return true;
```

### 3. 菜单注册 (`AddParentsMenuPatch.cs`)

菜单在`AddParentsMenu`的Postfix中被注册：

```csharp
[HarmonyPatch(typeof(CharacterCreationCampaignBehavior), "AddParentsMenu")]
public static class AddParentsMenuPatch
{
    static void Postfix(CharacterCreationManager characterCreationManager)
    {
        var presetOriginMenu = CreatePresetOriginSelectionMenu(characterCreationManager);
        characterCreationManager.AddNewMenu(presetOriginMenu);
        OriginLog.Info("已添加预设出身选择菜单");
        // ... 其他菜单注册
    }
}
```

## 可能的问题

### 问题1: 文化检测失败

**假设**: `selectedCulture`可能是null，或者`StringId`的值不是"vlandia"

**验证方法**: 检查日志输出`Preset origin selection: cultureId=...`

**可能原因**:
- 菜单创建时机过早，文化还未选择
- `CharacterCreationContent.SelectedCulture`属性访问方式不正确
- 文化ID的实际值不是"vlandia"（可能是"Vlandia"或其他格式）

### 问题2: 菜单选项的OnCondition被过滤

**假设**: 虽然OnCondition返回true，但游戏引擎可能有其他过滤机制

**验证方法**: 检查是否有其他条件影响选项显示

### 问题3: 菜单创建时机问题

**假设**: `CreatePresetOriginSelectionMenu`在文化选择之前被调用，此时`SelectedCulture`还是null

**验证方法**: 检查菜单创建和注册的时机

### 问题4: 菜单选项未正确添加

**假设**: `menu.AddNarrativeMenuOption`调用失败但没有抛出异常

**验证方法**: 在添加选项前后添加日志，确认选项是否成功添加

## 调试建议

1. **添加详细日志**:
   ```csharp
   OriginLog.Info($"Creating preset menu, selectedCulture={selectedCulture?.StringId ?? "NULL"}");
   OriginLog.Info($"isVlandia={isVlandia}, will add {isVlandia ? "10" : "0"} Vlandia options");
   ```

2. **检查文化ID的实际值**:
   ```csharp
   if (selectedCulture != null)
   {
       OriginLog.Info($"Culture StringId: '{selectedCulture.StringId}', Lower: '{selectedCulture.StringId?.ToLower()}'");
   }
   ```

3. **验证选项是否添加**:
   ```csharp
   if (isVlandia)
   {
       int beforeCount = menu.Options?.Count ?? 0;
       menu.AddNarrativeMenuOption(...);
       int afterCount = menu.Options?.Count ?? 0;
       OriginLog.Info($"Added option, count: {beforeCount} -> {afterCount}");
   }
   ```

4. **检查菜单注册时机**:
   确认`AddParentsMenu`是在文化选择之后调用的

## 需要ChatGPT帮助的问题

1. **如何正确获取CharacterCreationManager中当前选择的文化？**
   - 是否有其他属性或方法可以获取？
   - `CharacterCreationContent.SelectedCulture`是否是正确的方式？

2. **NarrativeMenu的选项显示机制是什么？**
   - 除了OnCondition，是否还有其他过滤机制？
   - 选项是否需要在特定时机添加？

3. **菜单创建时机问题**
   - `CreatePresetOriginSelectionMenu`应该在什么时候调用？
   - 是否应该在文化选择之后动态创建菜单，而不是在`AddParentsMenu`时创建？

4. **替代方案**
   - 是否应该使用`ModifyMenuCharacters`或其他回调来动态添加选项？
   - 是否应该使用`PendingMenuSwitch`机制来延迟菜单创建？

## 相关代码文件

- `SubModule/Menus/Preset/PresetOriginSelectionMenu.cs` - 菜单创建代码
- `SubModule/Patches/AddParentsMenuPatch.cs` - 菜单注册
- `SubModule/Routing/OriginMenuRouter.cs` - 路由逻辑
- `SubModule/Routing/OriginRoutes.Vlandia.cs` - 瓦兰迪亚路由表

## 当前状态

- ✅ 所有瓦兰迪亚节点菜单已创建并注册
- ✅ 路由表已配置
- ✅ OnCondition方法都返回true
- ❌ 菜单选项不显示（菜单为空）

## 日志输出位置

游戏日志通常在：
- `C:\ProgramData\Mount and Blade II Bannerlord\crashes\[timestamp]\rgl_log_*.txt`
- 或游戏安装目录的日志文件

请检查日志中是否有`Preset origin selection: cultureId=...`的输出，以及相关的错误信息。



## 问题描述

在Mount & Blade II: Bannerlord游戏中，当玩家选择瓦兰迪亚文化后，进入"Choose Preset Origin"菜单时，菜单是空的，没有任何选项显示。

## 代码实现

### 1. 菜单创建代码 (`PresetOriginSelectionMenu.cs`)

```csharp
private static NarrativeMenu CreatePresetOriginSelectionMenu(CharacterCreationManager manager)
{
    var characters = new List<NarrativeMenuCharacter>();
    var menu = new NarrativeMenu(
        "preset_origin_selection",
        "origin_type_selection",
        "narrative_parent_menu",
        new TextObject("Choose Preset Origin"),
        new TextObject("Choose a preset origin background"),
        characters,
        GetPresetOriginMenuCharacterArgs
    );

    // 获取当前选择的文化
    CultureObject selectedCulture = null;
    try
    {
        var contentProp = manager.GetType().GetProperty("CharacterCreationContent",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
        if (contentProp != null)
        {
            var content = contentProp.GetValue(manager, null);
            if (content != null)
            {
                var cultureProp = content.GetType().GetProperty("SelectedCulture",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                if (cultureProp != null)
                {
                    selectedCulture = cultureProp.GetValue(content, null) as CultureObject;
                }
            }
        }
    }
    catch (Exception ex)
    {
        OriginLog.Error($"Failed to get selected culture: {ex.Message}");
    }

    string cultureId = selectedCulture?.StringId?.ToLower() ?? "";
    bool isVlandia = cultureId == "vlandia" || cultureId.Contains("vlandia");
    bool isKhuzait = cultureId == "khuzait" || cultureId.Contains("khuzait");
    // ... 其他文化检测

    OriginLog.Info($"Preset origin selection: cultureId={cultureId}, isVlandia={isVlandia}, isKhuzait={isKhuzait}");

    // 瓦兰迪亚预设出身选项
    if (isVlandia)
    {
        menu.AddNarrativeMenuOption(new NarrativeMenuOption(
            "vlandia_expedition_knight",
            new TextObject("远征的骑士"),
            new TextObject("远征归来的骑士，拥有丰富的战斗经验和声望"),
            GetExpeditionKnightArgs,
            ExpeditionKnightOnCondition,
            ExpeditionKnightOnSelect,
            null
        ));
        // ... 其他9个瓦兰迪亚选项
    }

    // 库塞特预设出身选项
    if (isKhuzait)
    {
        // ... 库塞特选项
    }

    return menu;
}
```

### 2. OnCondition方法

所有瓦兰迪亚选项的OnCondition方法都返回true：

```csharp
private static bool ExpeditionKnightOnCondition(CharacterCreationManager characterCreationManager) { return true; }
private static bool BankruptBanneretOnCondition(CharacterCreationManager characterCreationManager) { return true; }
// ... 其他都是 return true;
```

### 3. 菜单注册 (`AddParentsMenuPatch.cs`)

菜单在`AddParentsMenu`的Postfix中被注册：

```csharp
[HarmonyPatch(typeof(CharacterCreationCampaignBehavior), "AddParentsMenu")]
public static class AddParentsMenuPatch
{
    static void Postfix(CharacterCreationManager characterCreationManager)
    {
        var presetOriginMenu = CreatePresetOriginSelectionMenu(characterCreationManager);
        characterCreationManager.AddNewMenu(presetOriginMenu);
        OriginLog.Info("已添加预设出身选择菜单");
        // ... 其他菜单注册
    }
}
```

## 可能的问题

### 问题1: 文化检测失败

**假设**: `selectedCulture`可能是null，或者`StringId`的值不是"vlandia"

**验证方法**: 检查日志输出`Preset origin selection: cultureId=...`

**可能原因**:
- 菜单创建时机过早，文化还未选择
- `CharacterCreationContent.SelectedCulture`属性访问方式不正确
- 文化ID的实际值不是"vlandia"（可能是"Vlandia"或其他格式）

### 问题2: 菜单选项的OnCondition被过滤

**假设**: 虽然OnCondition返回true，但游戏引擎可能有其他过滤机制

**验证方法**: 检查是否有其他条件影响选项显示

### 问题3: 菜单创建时机问题

**假设**: `CreatePresetOriginSelectionMenu`在文化选择之前被调用，此时`SelectedCulture`还是null

**验证方法**: 检查菜单创建和注册的时机

### 问题4: 菜单选项未正确添加

**假设**: `menu.AddNarrativeMenuOption`调用失败但没有抛出异常

**验证方法**: 在添加选项前后添加日志，确认选项是否成功添加

## 调试建议

1. **添加详细日志**:
   ```csharp
   OriginLog.Info($"Creating preset menu, selectedCulture={selectedCulture?.StringId ?? "NULL"}");
   OriginLog.Info($"isVlandia={isVlandia}, will add {isVlandia ? "10" : "0"} Vlandia options");
   ```

2. **检查文化ID的实际值**:
   ```csharp
   if (selectedCulture != null)
   {
       OriginLog.Info($"Culture StringId: '{selectedCulture.StringId}', Lower: '{selectedCulture.StringId?.ToLower()}'");
   }
   ```

3. **验证选项是否添加**:
   ```csharp
   if (isVlandia)
   {
       int beforeCount = menu.Options?.Count ?? 0;
       menu.AddNarrativeMenuOption(...);
       int afterCount = menu.Options?.Count ?? 0;
       OriginLog.Info($"Added option, count: {beforeCount} -> {afterCount}");
   }
   ```

4. **检查菜单注册时机**:
   确认`AddParentsMenu`是在文化选择之后调用的

## 需要ChatGPT帮助的问题

1. **如何正确获取CharacterCreationManager中当前选择的文化？**
   - 是否有其他属性或方法可以获取？
   - `CharacterCreationContent.SelectedCulture`是否是正确的方式？

2. **NarrativeMenu的选项显示机制是什么？**
   - 除了OnCondition，是否还有其他过滤机制？
   - 选项是否需要在特定时机添加？

3. **菜单创建时机问题**
   - `CreatePresetOriginSelectionMenu`应该在什么时候调用？
   - 是否应该在文化选择之后动态创建菜单，而不是在`AddParentsMenu`时创建？

4. **替代方案**
   - 是否应该使用`ModifyMenuCharacters`或其他回调来动态添加选项？
   - 是否应该使用`PendingMenuSwitch`机制来延迟菜单创建？

## 相关代码文件

- `SubModule/Menus/Preset/PresetOriginSelectionMenu.cs` - 菜单创建代码
- `SubModule/Patches/AddParentsMenuPatch.cs` - 菜单注册
- `SubModule/Routing/OriginMenuRouter.cs` - 路由逻辑
- `SubModule/Routing/OriginRoutes.Vlandia.cs` - 瓦兰迪亚路由表

## 当前状态

- ✅ 所有瓦兰迪亚节点菜单已创建并注册
- ✅ 路由表已配置
- ✅ OnCondition方法都返回true
- ❌ 菜单选项不显示（菜单为空）

## 日志输出位置

游戏日志通常在：
- `C:\ProgramData\Mount and Blade II Bannerlord\crashes\[timestamp]\rgl_log_*.txt`
- 或游戏安装目录的日志文件

请检查日志中是否有`Preset origin selection: cultureId=...`的输出，以及相关的错误信息。



## 问题描述

在Mount & Blade II: Bannerlord游戏中，当玩家选择瓦兰迪亚文化后，进入"Choose Preset Origin"菜单时，菜单是空的，没有任何选项显示。

## 代码实现

### 1. 菜单创建代码 (`PresetOriginSelectionMenu.cs`)

```csharp
private static NarrativeMenu CreatePresetOriginSelectionMenu(CharacterCreationManager manager)
{
    var characters = new List<NarrativeMenuCharacter>();
    var menu = new NarrativeMenu(
        "preset_origin_selection",
        "origin_type_selection",
        "narrative_parent_menu",
        new TextObject("Choose Preset Origin"),
        new TextObject("Choose a preset origin background"),
        characters,
        GetPresetOriginMenuCharacterArgs
    );

    // 获取当前选择的文化
    CultureObject selectedCulture = null;
    try
    {
        var contentProp = manager.GetType().GetProperty("CharacterCreationContent",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
        if (contentProp != null)
        {
            var content = contentProp.GetValue(manager, null);
            if (content != null)
            {
                var cultureProp = content.GetType().GetProperty("SelectedCulture",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                if (cultureProp != null)
                {
                    selectedCulture = cultureProp.GetValue(content, null) as CultureObject;
                }
            }
        }
    }
    catch (Exception ex)
    {
        OriginLog.Error($"Failed to get selected culture: {ex.Message}");
    }

    string cultureId = selectedCulture?.StringId?.ToLower() ?? "";
    bool isVlandia = cultureId == "vlandia" || cultureId.Contains("vlandia");
    bool isKhuzait = cultureId == "khuzait" || cultureId.Contains("khuzait");
    // ... 其他文化检测

    OriginLog.Info($"Preset origin selection: cultureId={cultureId}, isVlandia={isVlandia}, isKhuzait={isKhuzait}");

    // 瓦兰迪亚预设出身选项
    if (isVlandia)
    {
        menu.AddNarrativeMenuOption(new NarrativeMenuOption(
            "vlandia_expedition_knight",
            new TextObject("远征的骑士"),
            new TextObject("远征归来的骑士，拥有丰富的战斗经验和声望"),
            GetExpeditionKnightArgs,
            ExpeditionKnightOnCondition,
            ExpeditionKnightOnSelect,
            null
        ));
        // ... 其他9个瓦兰迪亚选项
    }

    // 库塞特预设出身选项
    if (isKhuzait)
    {
        // ... 库塞特选项
    }

    return menu;
}
```

### 2. OnCondition方法

所有瓦兰迪亚选项的OnCondition方法都返回true：

```csharp
private static bool ExpeditionKnightOnCondition(CharacterCreationManager characterCreationManager) { return true; }
private static bool BankruptBanneretOnCondition(CharacterCreationManager characterCreationManager) { return true; }
// ... 其他都是 return true;
```

### 3. 菜单注册 (`AddParentsMenuPatch.cs`)

菜单在`AddParentsMenu`的Postfix中被注册：

```csharp
[HarmonyPatch(typeof(CharacterCreationCampaignBehavior), "AddParentsMenu")]
public static class AddParentsMenuPatch
{
    static void Postfix(CharacterCreationManager characterCreationManager)
    {
        var presetOriginMenu = CreatePresetOriginSelectionMenu(characterCreationManager);
        characterCreationManager.AddNewMenu(presetOriginMenu);
        OriginLog.Info("已添加预设出身选择菜单");
        // ... 其他菜单注册
    }
}
```

## 可能的问题

### 问题1: 文化检测失败

**假设**: `selectedCulture`可能是null，或者`StringId`的值不是"vlandia"

**验证方法**: 检查日志输出`Preset origin selection: cultureId=...`

**可能原因**:
- 菜单创建时机过早，文化还未选择
- `CharacterCreationContent.SelectedCulture`属性访问方式不正确
- 文化ID的实际值不是"vlandia"（可能是"Vlandia"或其他格式）

### 问题2: 菜单选项的OnCondition被过滤

**假设**: 虽然OnCondition返回true，但游戏引擎可能有其他过滤机制

**验证方法**: 检查是否有其他条件影响选项显示

### 问题3: 菜单创建时机问题

**假设**: `CreatePresetOriginSelectionMenu`在文化选择之前被调用，此时`SelectedCulture`还是null

**验证方法**: 检查菜单创建和注册的时机

### 问题4: 菜单选项未正确添加

**假设**: `menu.AddNarrativeMenuOption`调用失败但没有抛出异常

**验证方法**: 在添加选项前后添加日志，确认选项是否成功添加

## 调试建议

1. **添加详细日志**:
   ```csharp
   OriginLog.Info($"Creating preset menu, selectedCulture={selectedCulture?.StringId ?? "NULL"}");
   OriginLog.Info($"isVlandia={isVlandia}, will add {isVlandia ? "10" : "0"} Vlandia options");
   ```

2. **检查文化ID的实际值**:
   ```csharp
   if (selectedCulture != null)
   {
       OriginLog.Info($"Culture StringId: '{selectedCulture.StringId}', Lower: '{selectedCulture.StringId?.ToLower()}'");
   }
   ```

3. **验证选项是否添加**:
   ```csharp
   if (isVlandia)
   {
       int beforeCount = menu.Options?.Count ?? 0;
       menu.AddNarrativeMenuOption(...);
       int afterCount = menu.Options?.Count ?? 0;
       OriginLog.Info($"Added option, count: {beforeCount} -> {afterCount}");
   }
   ```

4. **检查菜单注册时机**:
   确认`AddParentsMenu`是在文化选择之后调用的

## 需要ChatGPT帮助的问题

1. **如何正确获取CharacterCreationManager中当前选择的文化？**
   - 是否有其他属性或方法可以获取？
   - `CharacterCreationContent.SelectedCulture`是否是正确的方式？

2. **NarrativeMenu的选项显示机制是什么？**
   - 除了OnCondition，是否还有其他过滤机制？
   - 选项是否需要在特定时机添加？

3. **菜单创建时机问题**
   - `CreatePresetOriginSelectionMenu`应该在什么时候调用？
   - 是否应该在文化选择之后动态创建菜单，而不是在`AddParentsMenu`时创建？

4. **替代方案**
   - 是否应该使用`ModifyMenuCharacters`或其他回调来动态添加选项？
   - 是否应该使用`PendingMenuSwitch`机制来延迟菜单创建？

## 相关代码文件

- `SubModule/Menus/Preset/PresetOriginSelectionMenu.cs` - 菜单创建代码
- `SubModule/Patches/AddParentsMenuPatch.cs` - 菜单注册
- `SubModule/Routing/OriginMenuRouter.cs` - 路由逻辑
- `SubModule/Routing/OriginRoutes.Vlandia.cs` - 瓦兰迪亚路由表

## 当前状态

- ✅ 所有瓦兰迪亚节点菜单已创建并注册
- ✅ 路由表已配置
- ✅ OnCondition方法都返回true
- ❌ 菜单选项不显示（菜单为空）

## 日志输出位置

游戏日志通常在：
- `C:\ProgramData\Mount and Blade II Bannerlord\crashes\[timestamp]\rgl_log_*.txt`
- 或游戏安装目录的日志文件

请检查日志中是否有`Preset origin selection: cultureId=...`的输出，以及相关的错误信息。




## 问题描述

在Mount & Blade II: Bannerlord游戏中，当玩家选择瓦兰迪亚文化后，进入"Choose Preset Origin"菜单时，菜单是空的，没有任何选项显示。

## 代码实现

### 1. 菜单创建代码 (`PresetOriginSelectionMenu.cs`)

```csharp
private static NarrativeMenu CreatePresetOriginSelectionMenu(CharacterCreationManager manager)
{
    var characters = new List<NarrativeMenuCharacter>();
    var menu = new NarrativeMenu(
        "preset_origin_selection",
        "origin_type_selection",
        "narrative_parent_menu",
        new TextObject("Choose Preset Origin"),
        new TextObject("Choose a preset origin background"),
        characters,
        GetPresetOriginMenuCharacterArgs
    );

    // 获取当前选择的文化
    CultureObject selectedCulture = null;
    try
    {
        var contentProp = manager.GetType().GetProperty("CharacterCreationContent",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
        if (contentProp != null)
        {
            var content = contentProp.GetValue(manager, null);
            if (content != null)
            {
                var cultureProp = content.GetType().GetProperty("SelectedCulture",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                if (cultureProp != null)
                {
                    selectedCulture = cultureProp.GetValue(content, null) as CultureObject;
                }
            }
        }
    }
    catch (Exception ex)
    {
        OriginLog.Error($"Failed to get selected culture: {ex.Message}");
    }

    string cultureId = selectedCulture?.StringId?.ToLower() ?? "";
    bool isVlandia = cultureId == "vlandia" || cultureId.Contains("vlandia");
    bool isKhuzait = cultureId == "khuzait" || cultureId.Contains("khuzait");
    // ... 其他文化检测

    OriginLog.Info($"Preset origin selection: cultureId={cultureId}, isVlandia={isVlandia}, isKhuzait={isKhuzait}");

    // 瓦兰迪亚预设出身选项
    if (isVlandia)
    {
        menu.AddNarrativeMenuOption(new NarrativeMenuOption(
            "vlandia_expedition_knight",
            new TextObject("远征的骑士"),
            new TextObject("远征归来的骑士，拥有丰富的战斗经验和声望"),
            GetExpeditionKnightArgs,
            ExpeditionKnightOnCondition,
            ExpeditionKnightOnSelect,
            null
        ));
        // ... 其他9个瓦兰迪亚选项
    }

    // 库塞特预设出身选项
    if (isKhuzait)
    {
        // ... 库塞特选项
    }

    return menu;
}
```

### 2. OnCondition方法

所有瓦兰迪亚选项的OnCondition方法都返回true：

```csharp
private static bool ExpeditionKnightOnCondition(CharacterCreationManager characterCreationManager) { return true; }
private static bool BankruptBanneretOnCondition(CharacterCreationManager characterCreationManager) { return true; }
// ... 其他都是 return true;
```

### 3. 菜单注册 (`AddParentsMenuPatch.cs`)

菜单在`AddParentsMenu`的Postfix中被注册：

```csharp
[HarmonyPatch(typeof(CharacterCreationCampaignBehavior), "AddParentsMenu")]
public static class AddParentsMenuPatch
{
    static void Postfix(CharacterCreationManager characterCreationManager)
    {
        var presetOriginMenu = CreatePresetOriginSelectionMenu(characterCreationManager);
        characterCreationManager.AddNewMenu(presetOriginMenu);
        OriginLog.Info("已添加预设出身选择菜单");
        // ... 其他菜单注册
    }
}
```

## 可能的问题

### 问题1: 文化检测失败

**假设**: `selectedCulture`可能是null，或者`StringId`的值不是"vlandia"

**验证方法**: 检查日志输出`Preset origin selection: cultureId=...`

**可能原因**:
- 菜单创建时机过早，文化还未选择
- `CharacterCreationContent.SelectedCulture`属性访问方式不正确
- 文化ID的实际值不是"vlandia"（可能是"Vlandia"或其他格式）

### 问题2: 菜单选项的OnCondition被过滤

**假设**: 虽然OnCondition返回true，但游戏引擎可能有其他过滤机制

**验证方法**: 检查是否有其他条件影响选项显示

### 问题3: 菜单创建时机问题

**假设**: `CreatePresetOriginSelectionMenu`在文化选择之前被调用，此时`SelectedCulture`还是null

**验证方法**: 检查菜单创建和注册的时机

### 问题4: 菜单选项未正确添加

**假设**: `menu.AddNarrativeMenuOption`调用失败但没有抛出异常

**验证方法**: 在添加选项前后添加日志，确认选项是否成功添加

## 调试建议

1. **添加详细日志**:
   ```csharp
   OriginLog.Info($"Creating preset menu, selectedCulture={selectedCulture?.StringId ?? "NULL"}");
   OriginLog.Info($"isVlandia={isVlandia}, will add {isVlandia ? "10" : "0"} Vlandia options");
   ```

2. **检查文化ID的实际值**:
   ```csharp
   if (selectedCulture != null)
   {
       OriginLog.Info($"Culture StringId: '{selectedCulture.StringId}', Lower: '{selectedCulture.StringId?.ToLower()}'");
   }
   ```

3. **验证选项是否添加**:
   ```csharp
   if (isVlandia)
   {
       int beforeCount = menu.Options?.Count ?? 0;
       menu.AddNarrativeMenuOption(...);
       int afterCount = menu.Options?.Count ?? 0;
       OriginLog.Info($"Added option, count: {beforeCount} -> {afterCount}");
   }
   ```

4. **检查菜单注册时机**:
   确认`AddParentsMenu`是在文化选择之后调用的

## 需要ChatGPT帮助的问题

1. **如何正确获取CharacterCreationManager中当前选择的文化？**
   - 是否有其他属性或方法可以获取？
   - `CharacterCreationContent.SelectedCulture`是否是正确的方式？

2. **NarrativeMenu的选项显示机制是什么？**
   - 除了OnCondition，是否还有其他过滤机制？
   - 选项是否需要在特定时机添加？

3. **菜单创建时机问题**
   - `CreatePresetOriginSelectionMenu`应该在什么时候调用？
   - 是否应该在文化选择之后动态创建菜单，而不是在`AddParentsMenu`时创建？

4. **替代方案**
   - 是否应该使用`ModifyMenuCharacters`或其他回调来动态添加选项？
   - 是否应该使用`PendingMenuSwitch`机制来延迟菜单创建？

## 相关代码文件

- `SubModule/Menus/Preset/PresetOriginSelectionMenu.cs` - 菜单创建代码
- `SubModule/Patches/AddParentsMenuPatch.cs` - 菜单注册
- `SubModule/Routing/OriginMenuRouter.cs` - 路由逻辑
- `SubModule/Routing/OriginRoutes.Vlandia.cs` - 瓦兰迪亚路由表

## 当前状态

- ✅ 所有瓦兰迪亚节点菜单已创建并注册
- ✅ 路由表已配置
- ✅ OnCondition方法都返回true
- ❌ 菜单选项不显示（菜单为空）

## 日志输出位置

游戏日志通常在：
- `C:\ProgramData\Mount and Blade II Bannerlord\crashes\[timestamp]\rgl_log_*.txt`
- 或游戏安装目录的日志文件

请检查日志中是否有`Preset origin selection: cultureId=...`的输出，以及相关的错误信息。



## 问题描述

在Mount & Blade II: Bannerlord游戏中，当玩家选择瓦兰迪亚文化后，进入"Choose Preset Origin"菜单时，菜单是空的，没有任何选项显示。

## 代码实现

### 1. 菜单创建代码 (`PresetOriginSelectionMenu.cs`)

```csharp
private static NarrativeMenu CreatePresetOriginSelectionMenu(CharacterCreationManager manager)
{
    var characters = new List<NarrativeMenuCharacter>();
    var menu = new NarrativeMenu(
        "preset_origin_selection",
        "origin_type_selection",
        "narrative_parent_menu",
        new TextObject("Choose Preset Origin"),
        new TextObject("Choose a preset origin background"),
        characters,
        GetPresetOriginMenuCharacterArgs
    );

    // 获取当前选择的文化
    CultureObject selectedCulture = null;
    try
    {
        var contentProp = manager.GetType().GetProperty("CharacterCreationContent",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
        if (contentProp != null)
        {
            var content = contentProp.GetValue(manager, null);
            if (content != null)
            {
                var cultureProp = content.GetType().GetProperty("SelectedCulture",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                if (cultureProp != null)
                {
                    selectedCulture = cultureProp.GetValue(content, null) as CultureObject;
                }
            }
        }
    }
    catch (Exception ex)
    {
        OriginLog.Error($"Failed to get selected culture: {ex.Message}");
    }

    string cultureId = selectedCulture?.StringId?.ToLower() ?? "";
    bool isVlandia = cultureId == "vlandia" || cultureId.Contains("vlandia");
    bool isKhuzait = cultureId == "khuzait" || cultureId.Contains("khuzait");
    // ... 其他文化检测

    OriginLog.Info($"Preset origin selection: cultureId={cultureId}, isVlandia={isVlandia}, isKhuzait={isKhuzait}");

    // 瓦兰迪亚预设出身选项
    if (isVlandia)
    {
        menu.AddNarrativeMenuOption(new NarrativeMenuOption(
            "vlandia_expedition_knight",
            new TextObject("远征的骑士"),
            new TextObject("远征归来的骑士，拥有丰富的战斗经验和声望"),
            GetExpeditionKnightArgs,
            ExpeditionKnightOnCondition,
            ExpeditionKnightOnSelect,
            null
        ));
        // ... 其他9个瓦兰迪亚选项
    }

    // 库塞特预设出身选项
    if (isKhuzait)
    {
        // ... 库塞特选项
    }

    return menu;
}
```

### 2. OnCondition方法

所有瓦兰迪亚选项的OnCondition方法都返回true：

```csharp
private static bool ExpeditionKnightOnCondition(CharacterCreationManager characterCreationManager) { return true; }
private static bool BankruptBanneretOnCondition(CharacterCreationManager characterCreationManager) { return true; }
// ... 其他都是 return true;
```

### 3. 菜单注册 (`AddParentsMenuPatch.cs`)

菜单在`AddParentsMenu`的Postfix中被注册：

```csharp
[HarmonyPatch(typeof(CharacterCreationCampaignBehavior), "AddParentsMenu")]
public static class AddParentsMenuPatch
{
    static void Postfix(CharacterCreationManager characterCreationManager)
    {
        var presetOriginMenu = CreatePresetOriginSelectionMenu(characterCreationManager);
        characterCreationManager.AddNewMenu(presetOriginMenu);
        OriginLog.Info("已添加预设出身选择菜单");
        // ... 其他菜单注册
    }
}
```

## 可能的问题

### 问题1: 文化检测失败

**假设**: `selectedCulture`可能是null，或者`StringId`的值不是"vlandia"

**验证方法**: 检查日志输出`Preset origin selection: cultureId=...`

**可能原因**:
- 菜单创建时机过早，文化还未选择
- `CharacterCreationContent.SelectedCulture`属性访问方式不正确
- 文化ID的实际值不是"vlandia"（可能是"Vlandia"或其他格式）

### 问题2: 菜单选项的OnCondition被过滤

**假设**: 虽然OnCondition返回true，但游戏引擎可能有其他过滤机制

**验证方法**: 检查是否有其他条件影响选项显示

### 问题3: 菜单创建时机问题

**假设**: `CreatePresetOriginSelectionMenu`在文化选择之前被调用，此时`SelectedCulture`还是null

**验证方法**: 检查菜单创建和注册的时机

### 问题4: 菜单选项未正确添加

**假设**: `menu.AddNarrativeMenuOption`调用失败但没有抛出异常

**验证方法**: 在添加选项前后添加日志，确认选项是否成功添加

## 调试建议

1. **添加详细日志**:
   ```csharp
   OriginLog.Info($"Creating preset menu, selectedCulture={selectedCulture?.StringId ?? "NULL"}");
   OriginLog.Info($"isVlandia={isVlandia}, will add {isVlandia ? "10" : "0"} Vlandia options");
   ```

2. **检查文化ID的实际值**:
   ```csharp
   if (selectedCulture != null)
   {
       OriginLog.Info($"Culture StringId: '{selectedCulture.StringId}', Lower: '{selectedCulture.StringId?.ToLower()}'");
   }
   ```

3. **验证选项是否添加**:
   ```csharp
   if (isVlandia)
   {
       int beforeCount = menu.Options?.Count ?? 0;
       menu.AddNarrativeMenuOption(...);
       int afterCount = menu.Options?.Count ?? 0;
       OriginLog.Info($"Added option, count: {beforeCount} -> {afterCount}");
   }
   ```

4. **检查菜单注册时机**:
   确认`AddParentsMenu`是在文化选择之后调用的

## 需要ChatGPT帮助的问题

1. **如何正确获取CharacterCreationManager中当前选择的文化？**
   - 是否有其他属性或方法可以获取？
   - `CharacterCreationContent.SelectedCulture`是否是正确的方式？

2. **NarrativeMenu的选项显示机制是什么？**
   - 除了OnCondition，是否还有其他过滤机制？
   - 选项是否需要在特定时机添加？

3. **菜单创建时机问题**
   - `CreatePresetOriginSelectionMenu`应该在什么时候调用？
   - 是否应该在文化选择之后动态创建菜单，而不是在`AddParentsMenu`时创建？

4. **替代方案**
   - 是否应该使用`ModifyMenuCharacters`或其他回调来动态添加选项？
   - 是否应该使用`PendingMenuSwitch`机制来延迟菜单创建？

## 相关代码文件

- `SubModule/Menus/Preset/PresetOriginSelectionMenu.cs` - 菜单创建代码
- `SubModule/Patches/AddParentsMenuPatch.cs` - 菜单注册
- `SubModule/Routing/OriginMenuRouter.cs` - 路由逻辑
- `SubModule/Routing/OriginRoutes.Vlandia.cs` - 瓦兰迪亚路由表

## 当前状态

- ✅ 所有瓦兰迪亚节点菜单已创建并注册
- ✅ 路由表已配置
- ✅ OnCondition方法都返回true
- ❌ 菜单选项不显示（菜单为空）

## 日志输出位置

游戏日志通常在：
- `C:\ProgramData\Mount and Blade II Bannerlord\crashes\[timestamp]\rgl_log_*.txt`
- 或游戏安装目录的日志文件

请检查日志中是否有`Preset origin selection: cultureId=...`的输出，以及相关的错误信息。



## 问题描述

在Mount & Blade II: Bannerlord游戏中，当玩家选择瓦兰迪亚文化后，进入"Choose Preset Origin"菜单时，菜单是空的，没有任何选项显示。

## 代码实现

### 1. 菜单创建代码 (`PresetOriginSelectionMenu.cs`)

```csharp
private static NarrativeMenu CreatePresetOriginSelectionMenu(CharacterCreationManager manager)
{
    var characters = new List<NarrativeMenuCharacter>();
    var menu = new NarrativeMenu(
        "preset_origin_selection",
        "origin_type_selection",
        "narrative_parent_menu",
        new TextObject("Choose Preset Origin"),
        new TextObject("Choose a preset origin background"),
        characters,
        GetPresetOriginMenuCharacterArgs
    );

    // 获取当前选择的文化
    CultureObject selectedCulture = null;
    try
    {
        var contentProp = manager.GetType().GetProperty("CharacterCreationContent",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
        if (contentProp != null)
        {
            var content = contentProp.GetValue(manager, null);
            if (content != null)
            {
                var cultureProp = content.GetType().GetProperty("SelectedCulture",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                if (cultureProp != null)
                {
                    selectedCulture = cultureProp.GetValue(content, null) as CultureObject;
                }
            }
        }
    }
    catch (Exception ex)
    {
        OriginLog.Error($"Failed to get selected culture: {ex.Message}");
    }

    string cultureId = selectedCulture?.StringId?.ToLower() ?? "";
    bool isVlandia = cultureId == "vlandia" || cultureId.Contains("vlandia");
    bool isKhuzait = cultureId == "khuzait" || cultureId.Contains("khuzait");
    // ... 其他文化检测

    OriginLog.Info($"Preset origin selection: cultureId={cultureId}, isVlandia={isVlandia}, isKhuzait={isKhuzait}");

    // 瓦兰迪亚预设出身选项
    if (isVlandia)
    {
        menu.AddNarrativeMenuOption(new NarrativeMenuOption(
            "vlandia_expedition_knight",
            new TextObject("远征的骑士"),
            new TextObject("远征归来的骑士，拥有丰富的战斗经验和声望"),
            GetExpeditionKnightArgs,
            ExpeditionKnightOnCondition,
            ExpeditionKnightOnSelect,
            null
        ));
        // ... 其他9个瓦兰迪亚选项
    }

    // 库塞特预设出身选项
    if (isKhuzait)
    {
        // ... 库塞特选项
    }

    return menu;
}
```

### 2. OnCondition方法

所有瓦兰迪亚选项的OnCondition方法都返回true：

```csharp
private static bool ExpeditionKnightOnCondition(CharacterCreationManager characterCreationManager) { return true; }
private static bool BankruptBanneretOnCondition(CharacterCreationManager characterCreationManager) { return true; }
// ... 其他都是 return true;
```

### 3. 菜单注册 (`AddParentsMenuPatch.cs`)

菜单在`AddParentsMenu`的Postfix中被注册：

```csharp
[HarmonyPatch(typeof(CharacterCreationCampaignBehavior), "AddParentsMenu")]
public static class AddParentsMenuPatch
{
    static void Postfix(CharacterCreationManager characterCreationManager)
    {
        var presetOriginMenu = CreatePresetOriginSelectionMenu(characterCreationManager);
        characterCreationManager.AddNewMenu(presetOriginMenu);
        OriginLog.Info("已添加预设出身选择菜单");
        // ... 其他菜单注册
    }
}
```

## 可能的问题

### 问题1: 文化检测失败

**假设**: `selectedCulture`可能是null，或者`StringId`的值不是"vlandia"

**验证方法**: 检查日志输出`Preset origin selection: cultureId=...`

**可能原因**:
- 菜单创建时机过早，文化还未选择
- `CharacterCreationContent.SelectedCulture`属性访问方式不正确
- 文化ID的实际值不是"vlandia"（可能是"Vlandia"或其他格式）

### 问题2: 菜单选项的OnCondition被过滤

**假设**: 虽然OnCondition返回true，但游戏引擎可能有其他过滤机制

**验证方法**: 检查是否有其他条件影响选项显示

### 问题3: 菜单创建时机问题

**假设**: `CreatePresetOriginSelectionMenu`在文化选择之前被调用，此时`SelectedCulture`还是null

**验证方法**: 检查菜单创建和注册的时机

### 问题4: 菜单选项未正确添加

**假设**: `menu.AddNarrativeMenuOption`调用失败但没有抛出异常

**验证方法**: 在添加选项前后添加日志，确认选项是否成功添加

## 调试建议

1. **添加详细日志**:
   ```csharp
   OriginLog.Info($"Creating preset menu, selectedCulture={selectedCulture?.StringId ?? "NULL"}");
   OriginLog.Info($"isVlandia={isVlandia}, will add {isVlandia ? "10" : "0"} Vlandia options");
   ```

2. **检查文化ID的实际值**:
   ```csharp
   if (selectedCulture != null)
   {
       OriginLog.Info($"Culture StringId: '{selectedCulture.StringId}', Lower: '{selectedCulture.StringId?.ToLower()}'");
   }
   ```

3. **验证选项是否添加**:
   ```csharp
   if (isVlandia)
   {
       int beforeCount = menu.Options?.Count ?? 0;
       menu.AddNarrativeMenuOption(...);
       int afterCount = menu.Options?.Count ?? 0;
       OriginLog.Info($"Added option, count: {beforeCount} -> {afterCount}");
   }
   ```

4. **检查菜单注册时机**:
   确认`AddParentsMenu`是在文化选择之后调用的

## 需要ChatGPT帮助的问题

1. **如何正确获取CharacterCreationManager中当前选择的文化？**
   - 是否有其他属性或方法可以获取？
   - `CharacterCreationContent.SelectedCulture`是否是正确的方式？

2. **NarrativeMenu的选项显示机制是什么？**
   - 除了OnCondition，是否还有其他过滤机制？
   - 选项是否需要在特定时机添加？

3. **菜单创建时机问题**
   - `CreatePresetOriginSelectionMenu`应该在什么时候调用？
   - 是否应该在文化选择之后动态创建菜单，而不是在`AddParentsMenu`时创建？

4. **替代方案**
   - 是否应该使用`ModifyMenuCharacters`或其他回调来动态添加选项？
   - 是否应该使用`PendingMenuSwitch`机制来延迟菜单创建？

## 相关代码文件

- `SubModule/Menus/Preset/PresetOriginSelectionMenu.cs` - 菜单创建代码
- `SubModule/Patches/AddParentsMenuPatch.cs` - 菜单注册
- `SubModule/Routing/OriginMenuRouter.cs` - 路由逻辑
- `SubModule/Routing/OriginRoutes.Vlandia.cs` - 瓦兰迪亚路由表

## 当前状态

- ✅ 所有瓦兰迪亚节点菜单已创建并注册
- ✅ 路由表已配置
- ✅ OnCondition方法都返回true
- ❌ 菜单选项不显示（菜单为空）

## 日志输出位置

游戏日志通常在：
- `C:\ProgramData\Mount and Blade II Bannerlord\crashes\[timestamp]\rgl_log_*.txt`
- 或游戏安装目录的日志文件

请检查日志中是否有`Preset origin selection: cultureId=...`的输出，以及相关的错误信息。




## 问题描述

在Mount & Blade II: Bannerlord游戏中，当玩家选择瓦兰迪亚文化后，进入"Choose Preset Origin"菜单时，菜单是空的，没有任何选项显示。

## 代码实现

### 1. 菜单创建代码 (`PresetOriginSelectionMenu.cs`)

```csharp
private static NarrativeMenu CreatePresetOriginSelectionMenu(CharacterCreationManager manager)
{
    var characters = new List<NarrativeMenuCharacter>();
    var menu = new NarrativeMenu(
        "preset_origin_selection",
        "origin_type_selection",
        "narrative_parent_menu",
        new TextObject("Choose Preset Origin"),
        new TextObject("Choose a preset origin background"),
        characters,
        GetPresetOriginMenuCharacterArgs
    );

    // 获取当前选择的文化
    CultureObject selectedCulture = null;
    try
    {
        var contentProp = manager.GetType().GetProperty("CharacterCreationContent",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
        if (contentProp != null)
        {
            var content = contentProp.GetValue(manager, null);
            if (content != null)
            {
                var cultureProp = content.GetType().GetProperty("SelectedCulture",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                if (cultureProp != null)
                {
                    selectedCulture = cultureProp.GetValue(content, null) as CultureObject;
                }
            }
        }
    }
    catch (Exception ex)
    {
        OriginLog.Error($"Failed to get selected culture: {ex.Message}");
    }

    string cultureId = selectedCulture?.StringId?.ToLower() ?? "";
    bool isVlandia = cultureId == "vlandia" || cultureId.Contains("vlandia");
    bool isKhuzait = cultureId == "khuzait" || cultureId.Contains("khuzait");
    // ... 其他文化检测

    OriginLog.Info($"Preset origin selection: cultureId={cultureId}, isVlandia={isVlandia}, isKhuzait={isKhuzait}");

    // 瓦兰迪亚预设出身选项
    if (isVlandia)
    {
        menu.AddNarrativeMenuOption(new NarrativeMenuOption(
            "vlandia_expedition_knight",
            new TextObject("远征的骑士"),
            new TextObject("远征归来的骑士，拥有丰富的战斗经验和声望"),
            GetExpeditionKnightArgs,
            ExpeditionKnightOnCondition,
            ExpeditionKnightOnSelect,
            null
        ));
        // ... 其他9个瓦兰迪亚选项
    }

    // 库塞特预设出身选项
    if (isKhuzait)
    {
        // ... 库塞特选项
    }

    return menu;
}
```

### 2. OnCondition方法

所有瓦兰迪亚选项的OnCondition方法都返回true：

```csharp
private static bool ExpeditionKnightOnCondition(CharacterCreationManager characterCreationManager) { return true; }
private static bool BankruptBanneretOnCondition(CharacterCreationManager characterCreationManager) { return true; }
// ... 其他都是 return true;
```

### 3. 菜单注册 (`AddParentsMenuPatch.cs`)

菜单在`AddParentsMenu`的Postfix中被注册：

```csharp
[HarmonyPatch(typeof(CharacterCreationCampaignBehavior), "AddParentsMenu")]
public static class AddParentsMenuPatch
{
    static void Postfix(CharacterCreationManager characterCreationManager)
    {
        var presetOriginMenu = CreatePresetOriginSelectionMenu(characterCreationManager);
        characterCreationManager.AddNewMenu(presetOriginMenu);
        OriginLog.Info("已添加预设出身选择菜单");
        // ... 其他菜单注册
    }
}
```

## 可能的问题

### 问题1: 文化检测失败

**假设**: `selectedCulture`可能是null，或者`StringId`的值不是"vlandia"

**验证方法**: 检查日志输出`Preset origin selection: cultureId=...`

**可能原因**:
- 菜单创建时机过早，文化还未选择
- `CharacterCreationContent.SelectedCulture`属性访问方式不正确
- 文化ID的实际值不是"vlandia"（可能是"Vlandia"或其他格式）

### 问题2: 菜单选项的OnCondition被过滤

**假设**: 虽然OnCondition返回true，但游戏引擎可能有其他过滤机制

**验证方法**: 检查是否有其他条件影响选项显示

### 问题3: 菜单创建时机问题

**假设**: `CreatePresetOriginSelectionMenu`在文化选择之前被调用，此时`SelectedCulture`还是null

**验证方法**: 检查菜单创建和注册的时机

### 问题4: 菜单选项未正确添加

**假设**: `menu.AddNarrativeMenuOption`调用失败但没有抛出异常

**验证方法**: 在添加选项前后添加日志，确认选项是否成功添加

## 调试建议

1. **添加详细日志**:
   ```csharp
   OriginLog.Info($"Creating preset menu, selectedCulture={selectedCulture?.StringId ?? "NULL"}");
   OriginLog.Info($"isVlandia={isVlandia}, will add {isVlandia ? "10" : "0"} Vlandia options");
   ```

2. **检查文化ID的实际值**:
   ```csharp
   if (selectedCulture != null)
   {
       OriginLog.Info($"Culture StringId: '{selectedCulture.StringId}', Lower: '{selectedCulture.StringId?.ToLower()}'");
   }
   ```

3. **验证选项是否添加**:
   ```csharp
   if (isVlandia)
   {
       int beforeCount = menu.Options?.Count ?? 0;
       menu.AddNarrativeMenuOption(...);
       int afterCount = menu.Options?.Count ?? 0;
       OriginLog.Info($"Added option, count: {beforeCount} -> {afterCount}");
   }
   ```

4. **检查菜单注册时机**:
   确认`AddParentsMenu`是在文化选择之后调用的

## 需要ChatGPT帮助的问题

1. **如何正确获取CharacterCreationManager中当前选择的文化？**
   - 是否有其他属性或方法可以获取？
   - `CharacterCreationContent.SelectedCulture`是否是正确的方式？

2. **NarrativeMenu的选项显示机制是什么？**
   - 除了OnCondition，是否还有其他过滤机制？
   - 选项是否需要在特定时机添加？

3. **菜单创建时机问题**
   - `CreatePresetOriginSelectionMenu`应该在什么时候调用？
   - 是否应该在文化选择之后动态创建菜单，而不是在`AddParentsMenu`时创建？

4. **替代方案**
   - 是否应该使用`ModifyMenuCharacters`或其他回调来动态添加选项？
   - 是否应该使用`PendingMenuSwitch`机制来延迟菜单创建？

## 相关代码文件

- `SubModule/Menus/Preset/PresetOriginSelectionMenu.cs` - 菜单创建代码
- `SubModule/Patches/AddParentsMenuPatch.cs` - 菜单注册
- `SubModule/Routing/OriginMenuRouter.cs` - 路由逻辑
- `SubModule/Routing/OriginRoutes.Vlandia.cs` - 瓦兰迪亚路由表

## 当前状态

- ✅ 所有瓦兰迪亚节点菜单已创建并注册
- ✅ 路由表已配置
- ✅ OnCondition方法都返回true
- ❌ 菜单选项不显示（菜单为空）

## 日志输出位置

游戏日志通常在：
- `C:\ProgramData\Mount and Blade II Bannerlord\crashes\[timestamp]\rgl_log_*.txt`
- 或游戏安装目录的日志文件

请检查日志中是否有`Preset origin selection: cultureId=...`的输出，以及相关的错误信息。



## 问题描述

在Mount & Blade II: Bannerlord游戏中，当玩家选择瓦兰迪亚文化后，进入"Choose Preset Origin"菜单时，菜单是空的，没有任何选项显示。

## 代码实现

### 1. 菜单创建代码 (`PresetOriginSelectionMenu.cs`)

```csharp
private static NarrativeMenu CreatePresetOriginSelectionMenu(CharacterCreationManager manager)
{
    var characters = new List<NarrativeMenuCharacter>();
    var menu = new NarrativeMenu(
        "preset_origin_selection",
        "origin_type_selection",
        "narrative_parent_menu",
        new TextObject("Choose Preset Origin"),
        new TextObject("Choose a preset origin background"),
        characters,
        GetPresetOriginMenuCharacterArgs
    );

    // 获取当前选择的文化
    CultureObject selectedCulture = null;
    try
    {
        var contentProp = manager.GetType().GetProperty("CharacterCreationContent",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
        if (contentProp != null)
        {
            var content = contentProp.GetValue(manager, null);
            if (content != null)
            {
                var cultureProp = content.GetType().GetProperty("SelectedCulture",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                if (cultureProp != null)
                {
                    selectedCulture = cultureProp.GetValue(content, null) as CultureObject;
                }
            }
        }
    }
    catch (Exception ex)
    {
        OriginLog.Error($"Failed to get selected culture: {ex.Message}");
    }

    string cultureId = selectedCulture?.StringId?.ToLower() ?? "";
    bool isVlandia = cultureId == "vlandia" || cultureId.Contains("vlandia");
    bool isKhuzait = cultureId == "khuzait" || cultureId.Contains("khuzait");
    // ... 其他文化检测

    OriginLog.Info($"Preset origin selection: cultureId={cultureId}, isVlandia={isVlandia}, isKhuzait={isKhuzait}");

    // 瓦兰迪亚预设出身选项
    if (isVlandia)
    {
        menu.AddNarrativeMenuOption(new NarrativeMenuOption(
            "vlandia_expedition_knight",
            new TextObject("远征的骑士"),
            new TextObject("远征归来的骑士，拥有丰富的战斗经验和声望"),
            GetExpeditionKnightArgs,
            ExpeditionKnightOnCondition,
            ExpeditionKnightOnSelect,
            null
        ));
        // ... 其他9个瓦兰迪亚选项
    }

    // 库塞特预设出身选项
    if (isKhuzait)
    {
        // ... 库塞特选项
    }

    return menu;
}
```

### 2. OnCondition方法

所有瓦兰迪亚选项的OnCondition方法都返回true：

```csharp
private static bool ExpeditionKnightOnCondition(CharacterCreationManager characterCreationManager) { return true; }
private static bool BankruptBanneretOnCondition(CharacterCreationManager characterCreationManager) { return true; }
// ... 其他都是 return true;
```

### 3. 菜单注册 (`AddParentsMenuPatch.cs`)

菜单在`AddParentsMenu`的Postfix中被注册：

```csharp
[HarmonyPatch(typeof(CharacterCreationCampaignBehavior), "AddParentsMenu")]
public static class AddParentsMenuPatch
{
    static void Postfix(CharacterCreationManager characterCreationManager)
    {
        var presetOriginMenu = CreatePresetOriginSelectionMenu(characterCreationManager);
        characterCreationManager.AddNewMenu(presetOriginMenu);
        OriginLog.Info("已添加预设出身选择菜单");
        // ... 其他菜单注册
    }
}
```

## 可能的问题

### 问题1: 文化检测失败

**假设**: `selectedCulture`可能是null，或者`StringId`的值不是"vlandia"

**验证方法**: 检查日志输出`Preset origin selection: cultureId=...`

**可能原因**:
- 菜单创建时机过早，文化还未选择
- `CharacterCreationContent.SelectedCulture`属性访问方式不正确
- 文化ID的实际值不是"vlandia"（可能是"Vlandia"或其他格式）

### 问题2: 菜单选项的OnCondition被过滤

**假设**: 虽然OnCondition返回true，但游戏引擎可能有其他过滤机制

**验证方法**: 检查是否有其他条件影响选项显示

### 问题3: 菜单创建时机问题

**假设**: `CreatePresetOriginSelectionMenu`在文化选择之前被调用，此时`SelectedCulture`还是null

**验证方法**: 检查菜单创建和注册的时机

### 问题4: 菜单选项未正确添加

**假设**: `menu.AddNarrativeMenuOption`调用失败但没有抛出异常

**验证方法**: 在添加选项前后添加日志，确认选项是否成功添加

## 调试建议

1. **添加详细日志**:
   ```csharp
   OriginLog.Info($"Creating preset menu, selectedCulture={selectedCulture?.StringId ?? "NULL"}");
   OriginLog.Info($"isVlandia={isVlandia}, will add {isVlandia ? "10" : "0"} Vlandia options");
   ```

2. **检查文化ID的实际值**:
   ```csharp
   if (selectedCulture != null)
   {
       OriginLog.Info($"Culture StringId: '{selectedCulture.StringId}', Lower: '{selectedCulture.StringId?.ToLower()}'");
   }
   ```

3. **验证选项是否添加**:
   ```csharp
   if (isVlandia)
   {
       int beforeCount = menu.Options?.Count ?? 0;
       menu.AddNarrativeMenuOption(...);
       int afterCount = menu.Options?.Count ?? 0;
       OriginLog.Info($"Added option, count: {beforeCount} -> {afterCount}");
   }
   ```

4. **检查菜单注册时机**:
   确认`AddParentsMenu`是在文化选择之后调用的

## 需要ChatGPT帮助的问题

1. **如何正确获取CharacterCreationManager中当前选择的文化？**
   - 是否有其他属性或方法可以获取？
   - `CharacterCreationContent.SelectedCulture`是否是正确的方式？

2. **NarrativeMenu的选项显示机制是什么？**
   - 除了OnCondition，是否还有其他过滤机制？
   - 选项是否需要在特定时机添加？

3. **菜单创建时机问题**
   - `CreatePresetOriginSelectionMenu`应该在什么时候调用？
   - 是否应该在文化选择之后动态创建菜单，而不是在`AddParentsMenu`时创建？

4. **替代方案**
   - 是否应该使用`ModifyMenuCharacters`或其他回调来动态添加选项？
   - 是否应该使用`PendingMenuSwitch`机制来延迟菜单创建？

## 相关代码文件

- `SubModule/Menus/Preset/PresetOriginSelectionMenu.cs` - 菜单创建代码
- `SubModule/Patches/AddParentsMenuPatch.cs` - 菜单注册
- `SubModule/Routing/OriginMenuRouter.cs` - 路由逻辑
- `SubModule/Routing/OriginRoutes.Vlandia.cs` - 瓦兰迪亚路由表

## 当前状态

- ✅ 所有瓦兰迪亚节点菜单已创建并注册
- ✅ 路由表已配置
- ✅ OnCondition方法都返回true
- ❌ 菜单选项不显示（菜单为空）

## 日志输出位置

游戏日志通常在：
- `C:\ProgramData\Mount and Blade II Bannerlord\crashes\[timestamp]\rgl_log_*.txt`
- 或游戏安装目录的日志文件

请检查日志中是否有`Preset origin selection: cultureId=...`的输出，以及相关的错误信息。



## 问题描述

在Mount & Blade II: Bannerlord游戏中，当玩家选择瓦兰迪亚文化后，进入"Choose Preset Origin"菜单时，菜单是空的，没有任何选项显示。

## 代码实现

### 1. 菜单创建代码 (`PresetOriginSelectionMenu.cs`)

```csharp
private static NarrativeMenu CreatePresetOriginSelectionMenu(CharacterCreationManager manager)
{
    var characters = new List<NarrativeMenuCharacter>();
    var menu = new NarrativeMenu(
        "preset_origin_selection",
        "origin_type_selection",
        "narrative_parent_menu",
        new TextObject("Choose Preset Origin"),
        new TextObject("Choose a preset origin background"),
        characters,
        GetPresetOriginMenuCharacterArgs
    );

    // 获取当前选择的文化
    CultureObject selectedCulture = null;
    try
    {
        var contentProp = manager.GetType().GetProperty("CharacterCreationContent",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
        if (contentProp != null)
        {
            var content = contentProp.GetValue(manager, null);
            if (content != null)
            {
                var cultureProp = content.GetType().GetProperty("SelectedCulture",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                if (cultureProp != null)
                {
                    selectedCulture = cultureProp.GetValue(content, null) as CultureObject;
                }
            }
        }
    }
    catch (Exception ex)
    {
        OriginLog.Error($"Failed to get selected culture: {ex.Message}");
    }

    string cultureId = selectedCulture?.StringId?.ToLower() ?? "";
    bool isVlandia = cultureId == "vlandia" || cultureId.Contains("vlandia");
    bool isKhuzait = cultureId == "khuzait" || cultureId.Contains("khuzait");
    // ... 其他文化检测

    OriginLog.Info($"Preset origin selection: cultureId={cultureId}, isVlandia={isVlandia}, isKhuzait={isKhuzait}");

    // 瓦兰迪亚预设出身选项
    if (isVlandia)
    {
        menu.AddNarrativeMenuOption(new NarrativeMenuOption(
            "vlandia_expedition_knight",
            new TextObject("远征的骑士"),
            new TextObject("远征归来的骑士，拥有丰富的战斗经验和声望"),
            GetExpeditionKnightArgs,
            ExpeditionKnightOnCondition,
            ExpeditionKnightOnSelect,
            null
        ));
        // ... 其他9个瓦兰迪亚选项
    }

    // 库塞特预设出身选项
    if (isKhuzait)
    {
        // ... 库塞特选项
    }

    return menu;
}
```

### 2. OnCondition方法

所有瓦兰迪亚选项的OnCondition方法都返回true：

```csharp
private static bool ExpeditionKnightOnCondition(CharacterCreationManager characterCreationManager) { return true; }
private static bool BankruptBanneretOnCondition(CharacterCreationManager characterCreationManager) { return true; }
// ... 其他都是 return true;
```

### 3. 菜单注册 (`AddParentsMenuPatch.cs`)

菜单在`AddParentsMenu`的Postfix中被注册：

```csharp
[HarmonyPatch(typeof(CharacterCreationCampaignBehavior), "AddParentsMenu")]
public static class AddParentsMenuPatch
{
    static void Postfix(CharacterCreationManager characterCreationManager)
    {
        var presetOriginMenu = CreatePresetOriginSelectionMenu(characterCreationManager);
        characterCreationManager.AddNewMenu(presetOriginMenu);
        OriginLog.Info("已添加预设出身选择菜单");
        // ... 其他菜单注册
    }
}
```

## 可能的问题

### 问题1: 文化检测失败

**假设**: `selectedCulture`可能是null，或者`StringId`的值不是"vlandia"

**验证方法**: 检查日志输出`Preset origin selection: cultureId=...`

**可能原因**:
- 菜单创建时机过早，文化还未选择
- `CharacterCreationContent.SelectedCulture`属性访问方式不正确
- 文化ID的实际值不是"vlandia"（可能是"Vlandia"或其他格式）

### 问题2: 菜单选项的OnCondition被过滤

**假设**: 虽然OnCondition返回true，但游戏引擎可能有其他过滤机制

**验证方法**: 检查是否有其他条件影响选项显示

### 问题3: 菜单创建时机问题

**假设**: `CreatePresetOriginSelectionMenu`在文化选择之前被调用，此时`SelectedCulture`还是null

**验证方法**: 检查菜单创建和注册的时机

### 问题4: 菜单选项未正确添加

**假设**: `menu.AddNarrativeMenuOption`调用失败但没有抛出异常

**验证方法**: 在添加选项前后添加日志，确认选项是否成功添加

## 调试建议

1. **添加详细日志**:
   ```csharp
   OriginLog.Info($"Creating preset menu, selectedCulture={selectedCulture?.StringId ?? "NULL"}");
   OriginLog.Info($"isVlandia={isVlandia}, will add {isVlandia ? "10" : "0"} Vlandia options");
   ```

2. **检查文化ID的实际值**:
   ```csharp
   if (selectedCulture != null)
   {
       OriginLog.Info($"Culture StringId: '{selectedCulture.StringId}', Lower: '{selectedCulture.StringId?.ToLower()}'");
   }
   ```

3. **验证选项是否添加**:
   ```csharp
   if (isVlandia)
   {
       int beforeCount = menu.Options?.Count ?? 0;
       menu.AddNarrativeMenuOption(...);
       int afterCount = menu.Options?.Count ?? 0;
       OriginLog.Info($"Added option, count: {beforeCount} -> {afterCount}");
   }
   ```

4. **检查菜单注册时机**:
   确认`AddParentsMenu`是在文化选择之后调用的

## 需要ChatGPT帮助的问题

1. **如何正确获取CharacterCreationManager中当前选择的文化？**
   - 是否有其他属性或方法可以获取？
   - `CharacterCreationContent.SelectedCulture`是否是正确的方式？

2. **NarrativeMenu的选项显示机制是什么？**
   - 除了OnCondition，是否还有其他过滤机制？
   - 选项是否需要在特定时机添加？

3. **菜单创建时机问题**
   - `CreatePresetOriginSelectionMenu`应该在什么时候调用？
   - 是否应该在文化选择之后动态创建菜单，而不是在`AddParentsMenu`时创建？

4. **替代方案**
   - 是否应该使用`ModifyMenuCharacters`或其他回调来动态添加选项？
   - 是否应该使用`PendingMenuSwitch`机制来延迟菜单创建？

## 相关代码文件

- `SubModule/Menus/Preset/PresetOriginSelectionMenu.cs` - 菜单创建代码
- `SubModule/Patches/AddParentsMenuPatch.cs` - 菜单注册
- `SubModule/Routing/OriginMenuRouter.cs` - 路由逻辑
- `SubModule/Routing/OriginRoutes.Vlandia.cs` - 瓦兰迪亚路由表

## 当前状态

- ✅ 所有瓦兰迪亚节点菜单已创建并注册
- ✅ 路由表已配置
- ✅ OnCondition方法都返回true
- ❌ 菜单选项不显示（菜单为空）

## 日志输出位置

游戏日志通常在：
- `C:\ProgramData\Mount and Blade II Bannerlord\crashes\[timestamp]\rgl_log_*.txt`
- 或游戏安装目录的日志文件

请检查日志中是否有`Preset origin selection: cultureId=...`的输出，以及相关的错误信息。







