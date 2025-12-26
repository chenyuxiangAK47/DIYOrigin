# 给 Cursor 的改进要求（强硬版）

## 第一部分：直接发给 Cursor 的话（强硬但不脏）

你这次改得太离谱了。`OriginSystemPatches.cs` 现在一堆 **CS1010/CS1003/CS1026**，典型就是 **字符串/引号被你弄坏 + 编码被你写烂**。

**别再凭感觉瞎改。** 你现在立刻做这几件事：

1. **先把文件恢复到能编译的原版/上一版（git revert / backup）**，不要在一坨语法报错上继续补丁式修修补补。
2. **逐段对照原版（Bannerlord Decompiled）代码结构**来写：菜单创建、Input/OutputMenuId、OnSelect/OnConsequence、TrySwitchToNextMenu 的调用时机，都按引擎原本流程走。
3. **所有 TextObject 字符串必须保证引号闭合**，不要把中文注释/字符串写到一半断掉。
4. **编码统一 UTF-8（无 BOM 也行），禁止写出乱码**。出现"库塞和/阿塞和/修和OutputMenuId"这种破字，说明你根本没检查文件内容。
5. 每改一小段就跑一次编译，**不要一次性生成几千行然后交一坨不能编译的垃圾**。

请你认真点：**先让代码能编译，再谈逻辑。** 不要再给我制造 40+ 个 CS1010。

---

## 第二部分：硬核可执行的改进要求清单（按 checklist 工作）

### A. 工作流程（必须遵守）

- ✅ **先恢复文件到最近一次能编译的 commit**（或把坏文件丢掉重新从原版拷贝再改）
- ✅ 每次改动不超过一个功能点：
  - 先修编码/字符串 → 编译通过
  - 再修菜单链路 → 编译通过
  - 再修 Harmony Patch 时序 → 编译通过
- ✅ **每次输出前自检：搜索 `new TextObject("` 是否都成对闭合**；搜索 `#region` 是否都有 `#endregion`

### B. 编码与字符串硬规则

- ✅ `OriginSystemPatches.cs` 统一保存为 **UTF-8**
- ✅ 所有中文只能出现在：
  - `TextObject` 字符串里（且引号完整）
  - 注释里（不允许注释断到字符串中间）
- ✅ TextObject 多行文本必须用：
  - `@"..."`（逐字字符串）**或**
  - `"...\n..."`
  - **绝对不允许裸换行进 `"..."` 里（这就是 CS1010 的根源）**

### C. 最重要：不要瞎改引擎状态机

- ✅ **禁止**用反射去 set `CurrentMenu`（你之前已经证明会失败/不稳定）
- ✅ 菜单切换要通过 **唯一 InputMenuId / OutputMenuId 链路**让引擎自动走
- ✅ `OnSelect` 里只做"设置标志/写数据"，不要再直接调用 `TrySwitchToNextMenu()` 抢时序

### D. 最后交付物要求（让他对结果负责）

1. 提交一个 PR/patch：**只修编码+字符串+结构，让项目零报错编译通过**
2. 第二个 PR：修菜单链（每条预设出身 Node1 的 InputMenuId 必须唯一）
3. 第三个 PR：修 Patch 时序（Postfix 或 next-tick），并提供日志证明每次能切到正确菜单

---

## 第三部分：更狠一句（但不脏）

你现在的输出不是"有 bug"，而是"**连编译都过不了**"。请先把代码写到最基本的质量线：**能编译、能运行、再谈功能。**

---

## 第四部分：针对 CS1010 的快速修复法

### 问题诊断

CS1010 错误（常量中有换行符）的典型模式：

```csharp
// ❌ 错误示例
new TextObject("你在码头做搬运。
体力和耐力技能，疲惫状态。")  // 裸换行导致 CS1010

// ✅ 正确示例1：使用转义
new TextObject("你在码头做搬运。\n体力和耐力技能，疲惫状态。")

// ✅ 正确示例2：使用逐字字符串
new TextObject(@"你在码头做搬运。
体力和耐力技能，疲惫状态。")
```

### 批量修复步骤

1. **定位所有 TextObject 字符串**
   ```regex
   new TextObject\(".*?"
   ```

2. **检查字符串是否闭合**
   - 搜索 `new TextObject("` 的数量
   - 搜索对应的 `")` 数量
   - 数量必须相等

3. **检查是否有裸换行**
   - 搜索模式：`new TextObject("[^"]*\n[^"]*")`
   - 如果找到，说明字符串中有未转义的换行符

4. **修复编码问题**
   - 搜索所有 `和` 字符，根据上下文判断：
     - `库塞和` → `库塞特`
     - `阿塞和` → `阿塞莱`
     - `修和` → `修复` 或 `修改`
     - `新和` → `新的`
     - `菜和` → `菜单`
     - `使和` → `使用`
     - `遍历和` → `遍历`
     - `崩溃和` → `崩溃`
     - `切和` → `切换`
     - `CurrentMenu 和OutputMenuId` → `CurrentMenu 的OutputMenuId`

### 快速验证脚本

在修复后，运行以下检查：

```powershell
# 检查 TextObject 是否成对
$content = Get-Content "SubModule\OriginSystemPatches.cs" -Raw
$textObjectOpen = ([regex]::Matches($content, 'new TextObject\("')).Count
$textObjectClose = ([regex]::Matches($content, '"\s*\)')).Count
Write-Host "TextObject 开括号: $textObjectOpen"
Write-Host "TextObject 闭括号: $textObjectClose"
if ($textObjectOpen -ne $textObjectClose) {
    Write-Host "❌ TextObject 括号不匹配！" -ForegroundColor Red
} else {
    Write-Host "✅ TextObject 括号匹配" -ForegroundColor Green
}

# 检查是否有裸换行在字符串中
$nakedNewlines = [regex]::Matches($content, 'new TextObject\("[^"]*\r?\n[^"]*"\)')
if ($nakedNewlines.Count -gt 0) {
    Write-Host "❌ 发现 $($nakedNewlines.Count) 个包含裸换行的 TextObject！" -ForegroundColor Red
} else {
    Write-Host "✅ 没有发现裸换行" -ForegroundColor Green
}

# 检查编码问题
$encodingIssues = [regex]::Matches($content, '[库阿修新菜使遍崩切]和')
if ($encodingIssues.Count -gt 0) {
    Write-Host "❌ 发现 $($encodingIssues.Count) 个可能的编码问题！" -ForegroundColor Red
} else {
    Write-Host "✅ 没有发现明显的编码问题" -ForegroundColor Green
}
```

### 修复优先级

1. **最高优先级**：修复所有字符串引号闭合问题（CS1010/CS1003）
2. **高优先级**：修复编码损坏的中文字符
3. **中优先级**：修复文件结构问题（缺失的 `}`、`#endregion` 等）
4. **低优先级**：优化代码逻辑

---

## 第五部分：具体需要修复的位置（基于 debugLog.md）

### 关键错误位置

根据编译日志，以下行号附近需要重点检查：

- **458行**：第一个 CS1010 错误
- **500行**：CS1010 + CS1003
- **542行**：CS1010 + CS1003
- **584行**：CS1010 + CS1003
- **1046行**：CS1010 + CS1026 + CS1002（多个相关错误）
- **1185-1233行**：连续多个错误
- **1827-2157行**：大量 TextObject 相关错误
- **2765行**：文件末尾结构错误（CS1026 + CS1002 + CS1513 + CS1038）

### 建议修复顺序

1. 先修复文件末尾的结构问题（2765行）
2. 然后从前往后逐个修复 TextObject 字符串问题
3. 最后修复编码损坏的中文字符

---

## 总结

**核心要求**：
1. 先让代码能编译（零错误）
2. 再谈功能逻辑
3. 每次改动都要验证编译通过
4. 不要一次性改太多，分步骤进行

**禁止事项**：
1. 禁止在不能编译的代码上继续修补
2. 禁止用反射直接修改 `CurrentMenu`
3. 禁止在字符串中使用裸换行
4. 禁止输出编码损坏的中文字符

**交付标准**：
- 编译零错误
- 所有 TextObject 字符串正确闭合
- 所有中文字符正确显示
- 文件结构完整（所有括号、region 都匹配）










































