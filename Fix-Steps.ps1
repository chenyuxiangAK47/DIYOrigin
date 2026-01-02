# PowerShell 脚本 - 修复编码和编译错误
# 使用方法：在 PowerShell 中运行 .\Fix-Steps.ps1

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "C# 文件编码修复和编译脚本" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# 切换到项目目录
$projectPath = "D:\SteamLibrary\steamapps\common\Mount & Blade II Bannerlord\Modules\OriginSystemMod"

if (-not (Test-Path $projectPath)) {
    Write-Host "错误: 找不到项目目录: $projectPath" -ForegroundColor Red
    exit 1
}

Set-Location $projectPath
Write-Host "当前目录: $(Get-Location)" -ForegroundColor Green
Write-Host ""

# 步骤1: 检查问题字符
Write-Host "[步骤 1/4] 检查问题字符..." -ForegroundColor Yellow
if (Test-Path ".\Check-ProblematicChars.ps1") {
    & ".\Check-ProblematicChars.ps1"
} else {
    Write-Host "警告: Check-ProblematicChars.ps1 不存在，跳过检查" -ForegroundColor Yellow
}
Write-Host ""

# 步骤2: 修复编码和引号
Write-Host "[步骤 2/4] 修复编码和引号..." -ForegroundColor Yellow
if (Test-Path ".\Fix-EncodingAndQuotes.ps1") {
    & ".\Fix-EncodingAndQuotes.ps1"
} else {
    Write-Host "警告: Fix-EncodingAndQuotes.ps1 不存在，跳过修复" -ForegroundColor Yellow
    Write-Host "请确保脚本文件在项目目录中" -ForegroundColor Yellow
}
Write-Host ""

# 步骤3: 清理项目
Write-Host "[步骤 3/4] 清理项目..." -ForegroundColor Yellow
dotnet clean
if ($LASTEXITCODE -ne 0) {
    Write-Host "警告: dotnet clean 失败" -ForegroundColor Yellow
}
Write-Host ""

# 步骤4: 编译项目
Write-Host "[步骤 4/4] 编译项目..." -ForegroundColor Yellow
dotnet build "OriginSystemMod.csproj" -c Release --no-incremental

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "========================================" -ForegroundColor Green
    Write-Host "编译成功！" -ForegroundColor Green
    Write-Host "========================================" -ForegroundColor Green
} else {
    Write-Host ""
    Write-Host "========================================" -ForegroundColor Red
    Write-Host "编译失败，请检查错误信息" -ForegroundColor Red
    Write-Host "========================================" -ForegroundColor Red
}

Write-Host ""
Write-Host "按任意键退出..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")














