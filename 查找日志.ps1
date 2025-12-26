# 查找 Bannerlord 日志文件（正确位置：ProgramData）

$logPath = "$env:ProgramData\Mount and Blade II Bannerlord\Logs"
if (-not (Test-Path $logPath)) {
    $logPath = "$env:ProgramData\Mount & Blade II Bannerlord\Logs"
}

if (Test-Path $logPath) {
    $latest = Get-ChildItem $logPath -Filter "*.log" | Sort-Object LastWriteTime -Descending | Select-Object -First 1
    if ($latest) {
        Write-Host "找到日志: $($latest.FullName)" -ForegroundColor Green
        Write-Host "大小: $([math]::Round($latest.Length/1KB, 2)) KB" -ForegroundColor Gray
        Write-Host "修改时间: $($latest.LastWriteTime)" -ForegroundColor Gray
        Write-Host ""
        
        Write-Host "提取 OriginSystem 相关日志..." -ForegroundColor Cyan
        $content = Get-Content $latest.FullName | Select-String -Pattern "OriginSystem|OS\]|Select:|Switch:|Route:|PendingMenuSwitch|ResetState" | Select-Object -Last 100
        if ($content) {
            $content | Out-File -FilePath "log_extract.txt" -Encoding UTF8
            Write-Host "已提取到: log_extract.txt" -ForegroundColor Green
            Write-Host ""
            Write-Host "最近 20 条相关日志:" -ForegroundColor Yellow
            $content | Select-Object -Last 20 | ForEach-Object { Write-Host $_.Line }
        } else {
            Write-Host "未找到 OriginSystem 相关日志" -ForegroundColor Red
        }
    } else {
        Write-Host "未找到日志文件" -ForegroundColor Red
    }
} else {
    Write-Host "日志目录不存在: $logPath" -ForegroundColor Red
    Write-Host "请检查游戏是否运行过" -ForegroundColor Yellow
}

























