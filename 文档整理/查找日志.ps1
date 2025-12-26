# 查找 Bannerlord 日志文件并提取关键信息

$logFile = "$env:USERPROFILE\Documents\Mount and Blade II Bannerlord\Logs\rgl_log.txt"

if (Test-Path $logFile) {
    Write-Host "找到日志文件: $logFile`n"
    $content = Get-Content $logFile -Tail 5000 -Encoding UTF8
    
    Write-Host "=== 1. [HOOK ApplyByJoinToKingdom] POSTFIX ==="
    $content | Select-String -Pattern "HOOK.*ApplyByJoinToKingdom.*POSTFIX|判据1|判据2|判据3|Join 成功落地|Join 未成功落地" -Context 0,3 | Select-Object -Last 15
    
    Write-Host "`n=== 2. 状态检查 (StatusCheck / JoinKhuzaitAsNoble 最终验证) ==="
    $content | Select-String -Pattern "StatusCheck|JoinKhuzaitAsNoble.*最终验证|isVassal|isLord|判据1|判据2|判据3" -Context 0,2 | Select-Object -Last 15
    
    Write-Host "`n=== 3. [HOOK set_IsNoble] ==="
    $content | Select-String -Pattern "HOOK set_IsNoble|PlayerClan IsNoble" -Context 1,3 | Select-Object -Last 10
    
    Write-Host "`n=== 4. 雇佣兵相关 (Mercenary) ==="
    $content | Select-String -Pattern "ApplyByJoinFactionAsMercenary|ApplyByLeaveKingdomAsMercenary|Mercenary.*HOOK" -Context 1,2 | Select-Object -Last 10
} else {
    Write-Host "未找到日志文件: $logFile"
    Write-Host "`n尝试搜索其他位置..."
    
    $searchPaths = @(
        "$env:USERPROFILE\Documents\Mount and Blade II Bannerlord\Logs",
        "$env:LOCALAPPDATA\Mount and Blade II Bannerlord",
        "$env:APPDATA\Mount and Blade II Bannerlord"
    )
    
    foreach ($path in $searchPaths) {
        if (Test-Path $path) {
            Write-Host "`n检查路径: $path"
            $files = Get-ChildItem -Path $path -Recurse -Filter "*.txt" -ErrorAction SilentlyContinue | 
                Where-Object { $_.LastWriteTime -gt (Get-Date).AddHours(-6) } | 
                Sort-Object LastWriteTime -Descending | 
                Select-Object -First 3
            
            foreach ($file in $files) {
                Write-Host "  找到: $($file.FullName) (修改时间: $($file.LastWriteTime))"
                $sample = Get-Content $file.FullName -Tail 100 -Encoding UTF8 -ErrorAction SilentlyContinue
                if ($sample -match "HOOK|OriginSystem|OS\]|判据") {
                    Write-Host "  ✓ 包含相关日志！"
                    Write-Host "`n开始提取信息..."
                    $content = Get-Content $file.FullName -Tail 5000 -Encoding UTF8
                    
                    Write-Host "`n=== 1. [HOOK ApplyByJoinToKingdom] POSTFIX ==="
                    $content | Select-String -Pattern "HOOK.*ApplyByJoinToKingdom.*POSTFIX|判据1|判据2|判据3" -Context 0,3 | Select-Object -Last 15
                    
                    Write-Host "`n=== 2. 状态检查 ==="
                    $content | Select-String -Pattern "StatusCheck|JoinKhuzaitAsNoble.*最终验证|isVassal|isLord" -Context 0,2 | Select-Object -Last 15
                    
                    Write-Host "`n=== 3. [HOOK set_IsNoble] ==="
                    $content | Select-String -Pattern "HOOK set_IsNoble|PlayerClan IsNoble" -Context 1,3 | Select-Object -Last 10
                    
                    Write-Host "`n=== 4. 雇佣兵相关 ==="
                    $content | Select-String -Pattern "ApplyByJoinFactionAsMercenary|ApplyByLeaveKingdomAsMercenary|Mercenary" -Context 1,2 | Select-Object -Last 10
                    break
                }
            }
        }
    }
}

Write-Host "`n`n按任意键退出..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")

