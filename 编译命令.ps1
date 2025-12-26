# OriginSystemMod 编译脚本
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "OriginSystemMod 编译脚本" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# 切换到脚本所在目录
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $scriptPath

Write-Host "当前目录: $scriptPath" -ForegroundColor Yellow
Write-Host "正在编译项目..." -ForegroundColor Yellow
Write-Host ""

# 执行编译
dotnet build "OriginSystemMod.csproj" -c Release --no-incremental

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "========================================" -ForegroundColor Green
    Write-Host "编译成功！" -ForegroundColor Green
    Write-Host "========================================" -ForegroundColor Green
    Write-Host ""
    $dllPath = Join-Path $scriptPath "bin\Win64_Shipping_Client\OriginSystemMod.dll"
    if (Test-Path $dllPath) {
        $dllInfo = Get-Item $dllPath
        Write-Host "DLL 位置: $dllPath" -ForegroundColor Green
        Write-Host "修改时间: $($dllInfo.LastWriteTime)" -ForegroundColor Green
        Write-Host "文件大小: $([math]::Round($dllInfo.Length / 1KB, 2)) KB" -ForegroundColor Green
    }
    Write-Host ""
} else {
    Write-Host ""
    Write-Host "========================================" -ForegroundColor Red
    Write-Host "编译失败！请检查错误信息" -ForegroundColor Red
    Write-Host "========================================" -ForegroundColor Red
    Write-Host ""
}








