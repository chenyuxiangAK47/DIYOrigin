# Bannerlord 模组开发 Hook 方法库

> 本文档记录 Bannerlord 模组开发中常用的 Hook 方法和使用场景
> 每次有新的发现或改进都会更新此文档

## 目录

1. [王国和家族相关](#王国和家族相关)
2. [英雄和角色相关](#英雄和角色相关)
3. [物品和资源相关](#物品和资源相关)
4. [关系和外交相关](#关系和外交相关)
5. [队伍和单位相关](#队伍和单位相关)
6. [Hook 实现模式](#hook-实现模式)

---

## 王国和家族相关

### ChangeKingdomAction

#### 1. ApplyByJoinToKingdom
**用途**：加入王国成为封臣（vassal）  
**参数**：`Clan clan, Kingdom kingdom, CampaignTime shouldStayInKingdomUntil, bool showNotification`  
**Hook 场景**：
- 监控玩家加入王国的时机和状态
- 检查加入后是否正确成为封臣
- 追踪是否有其他行为覆盖了加入状态

**关键状态字段**：
- `clan.Kingdom` - 是否在目标王国中
- `clan.IsUnderMercenaryService` - 是否在雇佣兵服务中（只读）
- `kingdom.Clans.Contains(clan)` - 是否在王国 Clans 列表中

**示例代码**：
```csharp
// 在 Hook 中检查三个真正判据
bool condition1 = clan.Kingdom == kingdom;
bool condition2 = !clan.IsUnderMercenaryService;
bool condition3 = kingdom.Clans?.Contains(clan) ?? false;
bool isVassal = condition1 && condition2 && condition3;
```

#### 2. ApplyByJoinFactionAsMercenary
**用途**：加入王国成为雇佣兵  
**参数**：`Clan clan, Kingdom kingdom, int initialServiceDays, bool showNotification`  
**Hook 场景**：
- 监控是否被错误地设置为雇佣兵
- 检查雇佣兵服务的初始天数

#### 3. ApplyByLeaveKingdomAsMercenary
**用途**：离开王国（退出雇佣兵服务）  
**参数**：`Clan clan, bool showNotification`  
**Hook 场景**：
- 监控是否被意外退出雇佣兵服务
- 检查退出后的状态变化

### ChangeClanTierAction

#### 1. Apply
**用途**：改变家族等级  
**参数**：`Clan clan, int newTier`  
**Hook 场景**：
- 监控家族等级变化
- 检查等级是否被系统重置
- 追踪等级变化的原因（谁调用的）

**关键状态字段**：
- `clan.Tier` - 当前等级
- `clan.Renown` - 当前名望（等级依赖名望）

**注意事项**：
- Tier=4 通常需要 Renown >= 900
- Tier 和 Renown 不一致可能被系统纠正

---

## 英雄和角色相关

### Hero 状态设置

#### 1. SetNewOccupation
**用途**：设置英雄的职业（Occupation）  
**参数**：`Occupation occupation`  
**Hook 场景**：
- 监控 Occupation 变化
- 检查是否成功设置为 Lord
- 追踪是否有其他行为覆盖了 Occupation

**关键字段**：
- `Hero.IsLord` - 是否是领主（只读，由 Occupation 决定）
- `Hero.Occupation` - 当前职业

**注意事项**：
- 某些版本可能没有此方法，需要使用反射
- Occupation 是枚举类型，需要确认命名空间

#### 2. ChangeHeroGold
**用途**：改变英雄的金币  
**参数**：`int amount`  
**Hook 场景**：
- 监控金币变化
- 检查金币是否正确添加
- 追踪金币变化的来源

---

## 物品和资源相关

### GiveGoldAction

#### 1. ApplyBetweenCharacters
**用途**：在角色之间转移金币  
**参数**：`Hero giver, Hero receiver, int amount, bool showNotification`  
**Hook 场景**：
- 监控金币转移
- 检查转移是否成功
- 追踪金币来源和去向

**替代方法**：
- `Hero.ChangeHeroGold(int amount)` - 直接改变英雄金币（可能不存在，需要反射）

---

## 关系和外交相关

### ChangeRelationAction

#### 1. ApplyPlayerRelation
**用途**：改变玩家与目标的关系  
**参数**：`Hero targetHero, int relationChange`  
**Hook 场景**：
- 监控关系变化
- 检查关系是否正确设置
- 追踪关系变化的来源

**关键字段**：
- `Hero.GetRelation(Hero otherHero)` - 获取两个英雄之间的关系值

---

## 队伍和单位相关

### MobileParty

#### 1. AddElementToMemberRoster
**用途**：向队伍添加单位  
**参数**：`CharacterObject character, int count`  
**Hook 场景**：
- 监控单位添加
- 检查单位是否正确添加
- 追踪单位来源

#### 2. AddFood
**用途**：添加食物  
**参数**：`int amount`  
**Hook 场景**：
- 监控食物变化
- 检查食物是否正确添加

#### 3. AddMounts
**用途**：添加坐骑  
**参数**：`int count`  
**Hook 场景**：
- 监控坐骑变化
- 检查坐骑是否正确添加

---

## Hook 实现模式

### 模式 1：版本无关的多重载 Hook

**适用场景**：方法有多个重载，不同版本参数不同

```csharp
[HarmonyPatch]
internal static class MyHooks
{
    static IEnumerable<MethodBase> TargetMethods()
    {
        return typeof(TargetClass)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.Name == "TargetMethodName")
            .Cast<MethodBase>();
    }

    static void Prefix(MethodBase __originalMethod, object[] __args)
    {
        // 从 __args 中提取参数
        var param1 = __args.FirstOrDefault(a => a is ParamType1) as ParamType1;
        
        OriginLog.Info($"[HOOK {__originalMethod.Name}] 调用前");
        OriginLog.Info($"[HOOK] StackTrace:\n{Environment.StackTrace}");
    }

    static void Postfix(MethodBase __originalMethod, object[] __args)
    {
        OriginLog.Info($"[HOOK {__originalMethod.Name}] 调用后");
    }
}
```

### 模式 2：单一方法 Hook

**适用场景**：方法签名确定，只有一个版本

```csharp
[HarmonyPatch(typeof(TargetClass), "TargetMethodName")]
internal static class MyHooks
{
    static void Prefix(ParamType1 param1, ParamType2 param2)
    {
        OriginLog.Info($"[HOOK] 调用前: param1={param1}, param2={param2}");
    }

    static void Postfix(ParamType1 param1, ParamType2 param2)
    {
        OriginLog.Info($"[HOOK] 调用后");
    }
}
```

### 模式 3：状态转储辅助方法

**适用场景**：需要记录复杂对象的状态

```csharp
private static void DumpCoreState(string tag, Clan clan, Kingdom kingdom)
{
    if (clan == null) return;
    
    OriginLog.Info($"[{tag}] Clan.Kingdom={clan.Kingdom?.StringId ?? "null"}");
    OriginLog.Info($"[{tag}] IsNoble={clan.IsNoble}, Tier={clan.Tier}, Renown={clan.Renown}");
    OriginLog.Info($"[{tag}] IsUnderMercenaryService={clan.IsUnderMercenaryService}");
    
    if (kingdom != null)
    {
        bool inClans = kingdom.Clans?.Contains(clan) ?? false;
        OriginLog.Info($"[{tag}] InKingdom.Clans={inClans}");
    }
}
```

---

## 更新日志

### 2024-12-19
- 初始版本
- 添加了王国和家族相关的 Hook 方法
- 添加了 Hook 实现模式
- 扩展了 KingdomActionHooks 来追踪更多 Action（ChangeRelationAction, GiveGoldAction, ChangeClanTierAction）

### 待补充
- 更多 Action 类的 Hook 方法
- 更多 Hook 实现模式
- 常见问题和解决方案
- 队伍和单位相关的 Hook
- 物品和装备相关的 Hook

---

## 使用建议

1. **先 Hook，再修改**：在修改代码前，先用 Hook 观察正常流程
2. **记录堆栈**：使用 `Environment.StackTrace` 追踪调用来源
3. **记录状态**：在 Prefix 和 Postfix 中都记录关键状态
4. **版本兼容**：使用 `TargetMethods()` 模式支持多版本
5. **性能考虑**：Hook 会频繁调用，避免在 Hook 中做耗时操作

---

## 相关文档

- [问题分析_贵族开局失败_给ChatGPT.md](./问题分析/问题分析_贵族开局失败_给ChatGPT.md)
- [KingdomActionHooks.cs](../SubModule/Patches/KingdomActionHooks.cs)

