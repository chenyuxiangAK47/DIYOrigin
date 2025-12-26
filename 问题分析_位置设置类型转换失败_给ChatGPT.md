# 问题分析：位置设置类型转换失败 - 给 ChatGPT

**日期：** 2024-12-19  
**问题：** 选择"战奴逃亡" → "逃向沙漠深处"后，出生位置仍在马凯布（库塞特的城市），而不是预期的古亚兹最南端村庄往南的沙漠深处

---

## 🔍 最新日志分析（rgl_log_43548.txt）

### 关键错误日志

```
[16:20:53.144] [SlaveEscape][Teleport] Settlement.Position type=TaleWorlds.CampaignSystem.CampaignVec2
[16:20:53.144] [SlaveEscape][Teleport] Position setter exists=True
[16:20:53.145] [OS][ERR] [SlaveEscape][Teleport] 设置位置时异常: Object of type 'TaleWorlds.Library.Vec2' cannot be converted to type 'TaleWorlds.CampaignSystem.CampaignVec2'. success=False
[16:20:53.145] [OS][ERR] [SlaveEscape][Teleport] StackTrace:    at System.RuntimeType.TryChangeType(Object value, Binder binder, CultureInfo culture, Boolean needsSpecialCast)
   at System.Reflection.MethodBase.Invoke(Object obj, Object[] parameters)
   at OriginSystemMod.PresetOriginSystem.SetSlaveEscapeStartingLocation(MobileParty party, String direction, String settlementId)
```

### 问题分析

1. ✅ **目标位置计算正确**
   - 找到了正确的定居点（古亚兹或帝国城市）
   - `Settlement.Position` 返回 `CampaignVec2` 类型

2. ✅ **属性存在**
   - `Position2D` 属性存在
   - Setter 存在（`Position setter exists=True`）

3. ❌ **类型转换失败**
   - 我们传入的是 `Vec2` 类型
   - 但 `Position2D` 属性需要 `CampaignVec2` 类型
   - 无法自动转换：`Object of type 'TaleWorlds.Library.Vec2' cannot be converted to type 'TaleWorlds.CampaignSystem.CampaignVec2'`

---

## 🐛 根本原因

### 问题：类型不匹配

**代码逻辑：**
```csharp
// 从 Settlement.Position 获取位置（返回 CampaignVec2）
var settlementPos = targetSettlement.Position; // CampaignVec2

// 转换为 Vec2（错误！）
var position = new Vec2(settlementPos.X, settlementPos.Y); // Vec2

// 尝试设置 Position2D（需要 CampaignVec2）
setMethod.Invoke(party, new object[] { position }); // ❌ 类型不匹配
```

**问题：**
- `Settlement.Position` 返回 `CampaignVec2` 类型
- 我们将其转换为 `Vec2` 类型
- 但 `MobileParty.Position2D` 属性需要 `CampaignVec2` 类型
- `Vec2` 无法自动转换为 `CampaignVec2`

---

## ✅ 修复方案

### 方案1：直接使用 CampaignVec2（推荐）

**修改：** 不要转换为 `Vec2`，直接使用 `CampaignVec2`

```csharp
// 从 Settlement.Position 获取位置（返回 CampaignVec2）
var settlementPos = targetSettlement.Position; // CampaignVec2

// 创建新的 CampaignVec2（如果需要偏移）
var position = new CampaignVec2(settlementPos.X, settlementPos.Y);

// 如果是"沙漠深处"方向，在村庄位置基础上再往南偏移
if (direction == "desert" && targetSettlement.IsVillage)
{
    position = new CampaignVec2(settlementPos.X, settlementPos.Y + southOffset);
}

// 直接设置 Position2D（使用 CampaignVec2）
setMethod.Invoke(party, new object[] { position }); // ✅ 类型匹配
```

**优点：**
- 类型匹配，不会转换失败
- 直接使用游戏原生的类型

### 方案2：使用 CampaignVec2 的构造函数或转换方法

**修改：** 检查 `CampaignVec2` 是否有从 `Vec2` 转换的构造函数或方法

```csharp
// 如果 CampaignVec2 有从 Vec2 转换的构造函数
var position = new CampaignVec2(vec2Position.X, vec2Position.Y);

// 或者使用隐式转换（如果存在）
CampaignVec2 position = vec2Position;
```

### 方案3：使用反射创建 CampaignVec2

**修改：** 使用反射创建 `CampaignVec2` 实例

```csharp
// 使用反射创建 CampaignVec2
var campaignVec2Type = typeof(CampaignVec2);
var campaignVec2Constructor = campaignVec2Type.GetConstructor(new Type[] { typeof(float), typeof(float) });
var position = campaignVec2Constructor.Invoke(new object[] { settlementPos.X, settlementPos.Y + southOffset });

// 设置位置
setMethod.Invoke(party, new object[] { position });
```

---

## 📋 需要确认的问题

1. **CampaignVec2 的构造函数**
   - `CampaignVec2` 是否有 `(float x, float y)` 构造函数？
   - 或者是否有其他构造函数？

2. **类型转换**
   - `Vec2` 是否可以隐式转换为 `CampaignVec2`？
   - 或者是否有显式转换方法？

3. **Position2D 属性的类型**
   - `MobileParty.Position2D` 的确切类型是什么？
   - 是 `CampaignVec2` 还是其他类型？

4. **最佳实践**
   - 在 Bannerlord 中设置 `MobileParty` 位置的最佳方法是什么？
   - 是否有官方 API 或推荐的方法？

---

## 📝 当前代码位置

**文件：** `SubModule/PresetOriginSystem.cs`  
**方法：** `SetSlaveEscapeStartingLocation`  
**行号：** 约 1515-1560

**相关代码：**
```csharp
// 从 Settlement.Position 获取位置
var settlementPos = targetSettlement.Position; // CampaignVec2

// 转换为 Vec2（问题在这里）
var position = new Vec2(settlementPos.X, settlementPos.Y); // Vec2

// 尝试设置位置（类型不匹配）
setMethod.Invoke(party, new object[] { position }); // ❌
```

---

## 🔄 修复历史

1. **第一次修复：** ResetState 加保护条件
2. **第二次修复：** OnBeforeInitialModuleScreenSetAsRoot 加 guard
3. **第三次修复：** OnSessionLaunched 过早调用问题
4. **第四次修复：** Position2D 属性不存在问题（尝试多种方法）
5. **第五次修复（当前）：** 类型转换失败问题

---

## 📝 总结

**问题根源：** `MobileParty.Position2D` 属性需要 `CampaignVec2` 类型，但我们传入的是 `Vec2` 类型，导致类型转换失败。

**修复方向：** 直接使用 `CampaignVec2` 类型，不要转换为 `Vec2`。

**修复状态：** ✅ **已修复** - 按照 ChatGPT 建议，使用 `CampaignVec2 + Vec2` 运算符进行偏移操作，并添加了参数类型自证日志。

**修复内容：**
1. 将 `position` 变量从 `Vec2` 改为 `CampaignVec2`
2. 使用 `CampaignVec2 + Vec2` 运算符进行偏移（`settlementPos + new Vec2(0f, southOffset)`）
3. 在 invoke 前添加参数类型日志（`setter paramType` 和 `argType`）
4. 确保所有 setter 调用都传入 `CampaignVec2` 类型


