# 差异对比：old-good vs current-bad

## old-good 版本（能实现）

### 1. ApplySlaveEscapeNode5 中直接调用

**文件**: `SubModule/PresetOriginSystem.cs`

```csharp
private static void ApplySlaveEscapeNode5(Hero hero, Dictionary<string, string> nodes)
{
    if (!nodes.ContainsKey("khz_node_ex_slave_direction")) return;
    
    var direction = nodes["khz_node_ex_slave_direction"];
    var party = MobileParty.MainParty;
    
    switch (direction)
    {
        case "steppe":
            AddSkill(hero, "scouting", 3);
            AddSkill(hero, "riding", 2);
            break;
        case "desert":
            AddSkill(hero, "steward", 2);
            AddSkill(hero, "scouting", 3);
            // 设置出生位置：沙漠深处（阿塞莱的沙漠城市）
            SetSlaveEscapeStartingLocation(party, "desert");  // ← 直接调用
            break;
        case "empire":
            AddSkill(hero, "charm", 2);
            AddSkill(hero, "scouting", 2);
            // 设置出生位置：帝国最南端城市
            SetSlaveEscapeStartingLocation(party, "empire");  // ← 直接调用
            break;
    }
}

private static void SetSlaveEscapeStartingLocation(MobileParty party, string direction)
{
    // void 方法，只有2个参数
    // 直接设置位置
}
```

### 2. 没有 OnCharacterCreationFinalizedPatch

- 没有 Harmony Patch
- 没有 `PendingStartDirection` 机制
- 直接在 `ApplySlaveEscapeNode5` 中调用

### 3. OnTick 兜底逻辑（如果有）

```csharp
// 无论成功失败都清空 Pending
_hasSetSlaveEscapePosition = true;
OriginSystemHelper.PendingStartDirection = null;
OriginSystemHelper.PendingStartSettlementId = null;
```

---

## current-bad 版本（不生效）

### 1. ApplySlaveEscapeNode5 中不调用

**文件**: `SubModule/PresetOriginSystem.cs` 第1272-1313行

```csharp
private static void ApplySlaveEscapeNode5(Hero hero, Dictionary<string, string> nodes)
{
    // ...
    // 只应用技能，出生位置已在 node 的 OnSelect 中保存  ← 不调用
    switch (direction)
    {
        case "desert":
            AddSkill(hero, "steward", 2);
            AddSkill(hero, "scouting", 3);
            OriginLog.Info("[SlaveEscape] 选择沙漠深处，出生位置已在 node 中保存");
            // ← 没有调用 SetSlaveEscapeStartingLocation
            break;
    }
}
```

### 2. 添加了 OnCharacterCreationFinalizedPatch

**文件**: `SubModule/OriginSystemPatches.cs` 第324-417行

- 有 Harmony Patch
- 但可能没触发（TargetMethod 返回 null）

### 3. 添加了 PendingStartDirection 机制

**文件**: `SubModule/Menus/Preset/Nodes/SlaveEscapeNodes.cs` 第168-183行

- Node5 OnSelect 中保存到 `PendingStartDirection`
- 在 Finalize/OnTick 中读取并执行

### 4. SetSlaveEscapeStartingLocation 改为 bool

**文件**: `SubModule/PresetOriginSystem.cs` 第1319行

```csharp
public static bool SetSlaveEscapeStartingLocation(MobileParty party, string direction, string settlementId = null)
{
    // bool 方法，有3个参数
    // 返回 success
}
```

---

## 关键差异清单

### 差异1：ApplySlaveEscapeNode5 中是否直接调用 ⚠️ **最关键**

- **old-good**: ✅ 直接调用 `SetSlaveEscapeStartingLocation(party, "desert")`
- **current-bad**: ❌ 不调用，只保存到 Pending

**影响**: 如果 Patch 没触发，OnTick 兜底也可能失败，导致完全不执行

### 差异2：SetSlaveEscapeStartingLocation 方法签名

- **old-good**: `private static void SetSlaveEscapeStartingLocation(MobileParty party, string direction)`（2个参数）
- **current-bad**: `public static bool SetSlaveEscapeStartingLocation(MobileParty party, string direction, string settlementId = null)`（3个参数，返回 bool）

**影响**: 调用方式不同，需要适配

### 差异3：是否有 OnCharacterCreationFinalizedPatch

- **old-good**: ❌ 没有 Patch
- **current-bad**: ✅ 有 Patch（但可能没触发）

**影响**: 如果 Patch 没触发，完全依赖 OnTick 兜底

### 差异4：是否有 PendingStartDirection 机制

- **old-good**: ❌ 没有 Pending 机制
- **current-bad**: ✅ 有 Pending 机制

**影响**: 增加了数据传递环节，可能被 ResetState 清空

---

## 最小修复方案

### 修复1：在 ApplySlaveEscapeNode5 中直接调用（恢复 old-good 行为）

**文件**: `SubModule/PresetOriginSystem.cs` 第1302-1310行

**修改前**:
```csharp
case "desert":
    AddSkill(hero, "steward", 2);
    AddSkill(hero, "scouting", 3);
    OriginLog.Info("[SlaveEscape] 选择沙漠深处，出生位置已在 node 中保存");
    break;
```

**修改后**:
```csharp
case "desert":
    AddSkill(hero, "steward", 2);
    AddSkill(hero, "scouting", 3);
    // 设置出生位置：沙漠深处（阿塞莱的沙漠城市）
    if (MobileParty.MainParty != null && Campaign.Current != null)
    {
        SetSlaveEscapeStartingLocation(MobileParty.MainParty, "desert", null);
    }
    break;
case "empire":
    AddSkill(hero, "charm", 2);
    AddSkill(hero, "scouting", 2);
    // 设置出生位置：帝国最南端城市
    if (MobileParty.MainParty != null && Campaign.Current != null)
    {
        SetSlaveEscapeStartingLocation(MobileParty.MainParty, "empire", null);
    }
    break;
```

**原因**: 恢复 old-good 的直接调用方式，不依赖 Patch 和 Pending 机制

---

## 为什么这样改

1. **old-good 版本能实现** → 说明在 `ApplySlaveEscapeNode5` 中直接调用是可行的
2. **current-bad 版本不生效** → 说明 Patch 可能没触发，或 Pending 被清空
3. **最小改动** → 只在 `ApplySlaveEscapeNode5` 中添加直接调用，其他不变


























