# C# 编译错误修复指南

## 问题诊断

根据错误日志，主要问题是：
- `error CS1010`: 常量中有换行符
- `error CS1056`: 意外的字符（通常是中文标点符号 `，`、`。`、`：` 等）
- 出现乱码字符 ``（U+FFFD），表明文件编码问题

## 根本原因

**中文标点在 C# 字符串字面量中本身是合法的**，真正的问题在于：

1. **全角标点被用在代码语法位置**（不在字符串里）
   - 例如：`Foo(a， b)` 中的 `，` 应该改为 `,`

2. **智能引号/全角引号导致字符串未闭合**
   - `"` 或 `"` 被当成了字符串定界符，编译器不识别
   - 例如：`new TextObject("你好")` 应该改为 `new TextObject("你好")`

3. **文件编码不一致**
   - 出现 `` 字符是编码问题的典型表现
   - 文件可能以 GBK/ANSI 保存，但以 UTF-8 读取

## 修复步骤

### 步骤 1: 检查问题字符

运行检查脚本：

```powershell
cd "D:\SteamLibrary\steamapps\common\Mount & Blade II Bannerlord\Modules\OriginSystemMod"
.\Check-ProblematicChars.ps1
```

这会列出所有包含问题字符的文件和位置。

### 步骤 2: 自动修复

运行修复脚本（先试运行）：

```powershell
# 试运行（不实际修改文件）
.\Fix-EncodingAndQuotes.ps1 -DryRun

# 实际修复
.\Fix-EncodingAndQuotes.ps1
```

修复脚本会：
- 将所有智能引号替换为普通引号
- 修复代码语法位置的全角标点
- 将所有 .cs 文件统一转换为 UTF-8 with BOM
- 创建/更新 .editorconfig 文件

### 步骤 3: 手动检查（如果需要）

如果自动修复后仍有问题，需要手动检查：

1. **检查引号问题**
   - 搜索所有 `"`、`"`、`＂`
   - 确保所有字符串定界符都是 `"`

2. **检查语法位置的全角标点**
   - 搜索参数列表中的 `，`（应该改为 `,`）
   - 搜索语句结束的 `；`（应该改为 `;`）

3. **检查文件编码**
   - 在 Visual Studio / VS Code 中打开文件
   - 查看右下角的编码显示
   - 如果显示 "UTF-8" 但没有 BOM，或显示其他编码，需要重新保存为 "UTF-8 with BOM"

### 步骤 4: 验证修复

```powershell
dotnet clean
dotnet build OriginSystemMod.csproj -c Release --no-incremental
```

检查是否还有 CS1010 或 CS1056 错误。

## 使用 Visual Studio / VS Code 手动修复

### Visual Studio

1. 打开问题文件
2. 文件 → 高级保存选项 → 编码：选择 "Unicode (UTF-8 with signature) - Codepage 65001"
3. 保存文件

### VS Code

1. 打开问题文件
2. 右下角点击编码（如 "UTF-8"）
3. 选择 "Save with Encoding"
4. 选择 "UTF-8 with BOM"

### 批量转换编码（PowerShell）

```powershell
$files = Get-ChildItem -Path "SubModule" -Filter "*.cs" -Recurse
$utf8WithBom = New-Object System.Text.UTF8Encoding $true

foreach ($file in $files) {
    $content = [System.IO.File]::ReadAllText($file.FullName)
    [System.IO.File]::WriteAllText($file.FullName, $content, $utf8WithBom)
    Write-Host "Converted: $($file.Name)"
}
```

## 注意事项

1. **不要误改字符串内容**
   - 字符串内部的标点（如 `"你好，世界。"`）是合法的，不需要修改
   - 只需要修改**代码语法位置**的标点

2. **优先修复引号问题**
   - 引号未闭合会导致后续所有字符都变成"意外字符"
   - 修复引号后，很多其他错误会自动消失

3. **统一编码最重要**
   - 即使修复了标点问题，如果编码不一致，将来还可能出问题
   - 建议所有 .cs 文件统一使用 UTF-8 with BOM

## 常见问题

**Q: 为什么我的字符串里有中文标点，但编译报错？**

A: 很可能是：
1. 引号被替换成了智能引号，导致字符串未闭合
2. 文件编码问题导致字符被误读

**Q: 修复后仍然报错怎么办？**

A: 
1. 检查是否所有文件都已转换为 UTF-8 with BOM
2. 检查是否还有遗漏的智能引号
3. 查看错误信息中的具体行号和列号，手动检查该位置

**Q: 批处理脚本的 `chcp 65001` 是问题吗？**

A: 不是。`chcp 65001` 只影响控制台显示编码，不影响 C# 编译器读取源码文件。














