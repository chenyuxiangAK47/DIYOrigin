# 快速修复命令（PowerShell）

## 方法1: 直接运行修复脚本（推荐）

在 **PowerShell** 中（不是 CMD），运行：

```powershell
# 切换到项目目录
cd "D:\SteamLibrary\steamapps\common\Mount & Blade II Bannerlord\Modules\OriginSystemMod"

# 运行一键修复脚本
.\Fix-Steps.ps1
```

## 方法2: 分步执行

```powershell
# 1. 切换到项目目录
cd "D:\SteamLibrary\steamapps\common\Mount & Blade II Bannerlord\Modules\OriginSystemMod"

# 2. 检查问题字符
.\Check-ProblematicChars.ps1

# 3. 修复编码和引号
.\Fix-EncodingAndQuotes.ps1

# 4. 清理项目
dotnet clean

# 5. 编译项目
dotnet build OriginSystemMod.csproj -c Release --no-incremental
```

## 如果脚本不存在，手动修复

```powershell
# 切换到项目目录
cd "D:\SteamLibrary\steamapps\common\Mount & Blade II Bannerlord\Modules\OriginSystemMod"

# 批量转换所有 .cs 文件为 UTF-8 with BOM
$files = Get-ChildItem -Path "SubModule" -Filter "*.cs" -Recurse
$utf8WithBom = New-Object System.Text.UTF8Encoding $true

foreach ($file in $files) {
    try {
        $content = [System.IO.File]::ReadAllText($file.FullName, [System.Text.Encoding]::UTF8)
        [System.IO.File]::WriteAllText($file.FullName, $content, $utf8WithBom)
        Write-Host "已转换: $($file.Name)"
    }
    catch {
        Write-Host "错误: $($file.Name) - $_"
    }
}

# 清理和编译
dotnet clean
dotnet build OriginSystemMod.csproj -c Release --no-incremental
```

## 重要提示

1. **PowerShell vs CMD**: 
   - PowerShell 注释用 `#`，不是 `REM`
   - PowerShell 切换目录用 `cd` 或 `Set-Location`，不需要 `/d` 参数
   - PowerShell 路径有空格需要用引号包裹

2. **如果遇到"无法执行脚本"错误**:
   ```powershell
   Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
   ```

3. **Visual Studio Developer PowerShell**:
   - 这是 PowerShell，不是 CMD
   - 可以直接运行 `.ps1` 脚本
   - 不需要 `powershell -File`，直接 `.\脚本名.ps1` 即可














