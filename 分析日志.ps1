# OriginSystemMod 日志分析脚本
# 自动查找并分析 Bannerlord 日志，定位路由失败原因

Write-Host "=================================================================================" -ForegroundColor Cyan
Write-Host "OriginSystemMod 日志分析工具" -ForegroundColor Cyan
Write-Host "=================================================================================" -ForegroundColor Cyan
Write-Host ""

# 查找日志文件（正确位置：ProgramData）
$logPaths = @(
    "$env:ProgramData\Mount and Blade II Bannerlord\Logs",
    "$env:ProgramData\Mount & Blade II Bannerlord\Logs",
    "$env:USERPROFILE\Documents\Mount and Blade II Bannerlord\Logs",
    "$env:USERPROFILE\Documents\Mount & Blade II Bannerlord\Logs",
    "$env:LOCALAPPDATA\Mount and Blade II Bannerlord\Logs",
    "$env:LOCALAPPDATA\Mount & Blade II Bannerlord\Logs"
)

$logFile = $null
foreach ($path in $logPaths) {
    if (Test-Path $path) {
        $latest = Get-ChildItem $path -Filter "*.log" -ErrorAction SilentlyContinue | 
                  Sort-Object LastWriteTime -Descending | 
                  Select-Object -First 1
        if ($latest) {
            $logFile = $latest.FullName
            Write-Host "找到日志文件: $logFile" -ForegroundColor Green
            Write-Host "大小: $([math]::Round($latest.Length/1KB, 2)) KB" -ForegroundColor Gray
            Write-Host "修改时间: $($latest.LastWriteTime)" -ForegroundColor Gray
            Write-Host ""
            break
        }
    }
}

if (-not $logFile) {
    Write-Host "错误: 未找到日志文件" -ForegroundColor Red
    Write-Host "请检查以下路径:" -ForegroundColor Yellow
    foreach ($path in $logPaths) {
        Write-Host "  - $path" -ForegroundColor Gray
    }
    exit 1
}

# 分析步骤
Write-Host "=================================================================================" -ForegroundColor Cyan
Write-Host "步骤1: 检查 DLL 是否加载" -ForegroundColor Cyan
Write-Host "=================================================================================" -ForegroundColor Cyan

$dllLoaded = Select-String -Path $logFile -Pattern "\[OriginSystem\].*DLL 加载信息|\[OriginSystem\].*Harmony PatchAll 完成" -CaseSensitive:$false
if ($dllLoaded) {
    Write-Host "✓ DLL 已加载" -ForegroundColor Green
    $dllLoaded | Select-Object -First 3 | ForEach-Object { Write-Host "  $($_.Line)" -ForegroundColor Gray }
} else {
    Write-Host "✗ DLL 未加载！" -ForegroundColor Red
    Write-Host "  可能原因: SubModule.xml 配置错误或 DLL 路径不正确" -ForegroundColor Yellow
}
Write-Host ""

# 步骤2: 检查 Patch 注册
Write-Host "=================================================================================" -ForegroundColor Cyan
Write-Host "步骤2: 检查 Harmony Patch 是否注册" -ForegroundColor Cyan
Write-Host "=================================================================================" -ForegroundColor Cyan

$patchRegistered = Select-String -Path $logFile -Pattern "\[SlaveEscape\]\[Patch\].*TargetType 找到|\[SlaveEscape\]\[Patch\].*TargetMethod 找到" -CaseSensitive:$false
if ($patchRegistered) {
    Write-Host "✓ Patch 已注册" -ForegroundColor Green
    $patchRegistered | Select-Object -First 2 | ForEach-Object { Write-Host "  $($_.Line)" -ForegroundColor Gray }
} else {
    Write-Host "✗ Patch 未注册或未找到目标方法" -ForegroundColor Yellow
    $patchWarning = Select-String -Path $logFile -Pattern "\[SlaveEscape\]\[Patch\].*未找到" -CaseSensitive:$false
    if ($patchWarning) {
        Write-Host "  警告信息:" -ForegroundColor Yellow
        $patchWarning | ForEach-Object { Write-Host "    $($_.Line)" -ForegroundColor Gray }
    }
}
Write-Host ""

# 步骤3: 检查 Patch 是否触发
Write-Host "=================================================================================" -ForegroundColor Cyan
Write-Host "步骤3: 检查 Patch 是否触发（最关键）" -ForegroundColor Cyan
Write-Host "=================================================================================" -ForegroundColor Cyan

$selectLogs = Select-String -Path $logFile -Pattern "Select: menu=" -CaseSensitive:$false
if ($selectLogs) {
    Write-Host "✓ OnNarrativeMenuOptionSelected Patch 已触发" -ForegroundColor Green
    Write-Host "  最近的 Select 日志:" -ForegroundColor Gray
    $selectLogs | Select-Object -Last 5 | ForEach-Object { Write-Host "    $($_.Line)" -ForegroundColor Gray }
} else {
    Write-Host "✗ OnNarrativeMenuOptionSelected Patch 未触发！" -ForegroundColor Red
    Write-Host "  可能原因: Patch 方法签名不匹配或 Harmony 未正确应用" -ForegroundColor Yellow
}
Write-Host ""

$postfixLogs = Select-String -Path $logFile -Pattern "Postfix: 当前菜单 =|Postfix: PendingMenuSwitch=" -CaseSensitive:$false
if ($postfixLogs) {
    Write-Host "✓ Postfix 已执行" -ForegroundColor Green
    $postfixLogs | Select-Object -Last 3 | ForEach-Object { Write-Host "    $($_.Line)" -ForegroundColor Gray }
} else {
    Write-Host "✗ Postfix 未执行（可能有异常）" -ForegroundColor Yellow
}
Write-Host ""

# 步骤4: 检查 PendingMenuSwitch
Write-Host "=================================================================================" -ForegroundColor Cyan
Write-Host "步骤4: 检查 PendingMenuSwitch 是否被设置" -ForegroundColor Cyan
Write-Host "=================================================================================" -ForegroundColor Cyan

$pendingLogs = Select-String -Path $logFile -Pattern "PendingMenuSwitch=" -CaseSensitive:$false
if ($pendingLogs) {
    Write-Host "✓ PendingMenuSwitch 已设置" -ForegroundColor Green
    $pendingLogs | Select-Object -Last 5 | ForEach-Object { Write-Host "    $($_.Line)" -ForegroundColor Gray }
    
    # 检查是否有 null 或空值
    $nullPending = $pendingLogs | Where-Object { $_.Line -match "PendingMenuSwitch=(null|NULL|保留给)" }
    if ($nullPending) {
        Write-Host "⚠ 警告: 发现 PendingMenuSwitch 为空的情况" -ForegroundColor Yellow
    }
} else {
    Write-Host "✗ PendingMenuSwitch 未被设置！" -ForegroundColor Red
    Write-Host "  可能原因: OnSelect 回调未触发或未设置 PendingMenuSwitch" -ForegroundColor Yellow
}
Write-Host ""

# 步骤5: 检查路由
Write-Host "=================================================================================" -ForegroundColor Cyan
Write-Host "步骤5: 检查路由是否工作" -ForegroundColor Cyan
Write-Host "=================================================================================" -ForegroundColor Cyan

$switchLogs = Select-String -Path $logFile -Pattern "Switch: cur=" -CaseSensitive:$false
if ($switchLogs) {
    Write-Host "✓ TrySwitchToNextMenu 已触发" -ForegroundColor Green
    Write-Host "  最近的路由日志:" -ForegroundColor Gray
    $switchLogs | Select-Object -Last 5 | ForEach-Object { Write-Host "    $($_.Line)" -ForegroundColor Gray }
    
    # 检查 resolved 值
    $resolvedNull = $switchLogs | Where-Object { $_.Line -match "resolved=(null|NULL)" }
    if ($resolvedNull) {
        Write-Host "⚠ 警告: 发现 resolved=NULL 的情况（路由未生效）" -ForegroundColor Yellow
    }
} else {
    Write-Host "✗ TrySwitchToNextMenu 未触发！" -ForegroundColor Red
    Write-Host "  可能原因: 引擎流程变化或 Patch 未正确应用" -ForegroundColor Yellow
}
Write-Host ""

$routeLogs = Select-String -Path $logFile -Pattern "\[Route\] 使用 PendingMenuSwitch:" -CaseSensitive:$false
if ($routeLogs) {
    Write-Host "✓ 路由解析已执行" -ForegroundColor Green
    $routeLogs | Select-Object -Last 3 | ForEach-Object { Write-Host "    $($_.Line)" -ForegroundColor Gray }
} else {
    Write-Host "✗ 路由解析未执行（ResolveNextMenuId 未使用 PendingMenuSwitch）" -ForegroundColor Yellow
}
Write-Host ""

$forceSwitchLogs = Select-String -Path $logFile -Pattern "ForceSwitch: target=" -CaseSensitive:$false
if ($forceSwitchLogs) {
    Write-Host "✓ 强制切换已执行" -ForegroundColor Green
    $forceSwitchLogs | Select-Object -Last 3 | ForEach-Object { Write-Host "    $($_.Line)" -ForegroundColor Gray }
    
    # 检查 found=False
    $foundFalse = $forceSwitchLogs | Where-Object { $_.Line -match "found=False" }
    if ($foundFalse) {
        Write-Host "⚠ 警告: 发现 found=False（目标菜单未找到）" -ForegroundColor Yellow
    }
} else {
    Write-Host "✗ 强制切换未执行" -ForegroundColor Yellow
}
Write-Host ""

# 步骤6: 检查错误
Write-Host "=================================================================================" -ForegroundColor Cyan
Write-Host "步骤6: 检查错误和异常" -ForegroundColor Cyan
Write-Host "=================================================================================" -ForegroundColor Cyan

$errors = Select-String -Path $logFile -Pattern "\[OS\]\[ERR\]|\[OriginSystem\].*失败|Exception|Error" -CaseSensitive:$false
if ($errors) {
    Write-Host "⚠ 发现错误:" -ForegroundColor Red
    $errors | Select-Object -Last 10 | ForEach-Object { Write-Host "    $($_.Line)" -ForegroundColor Red }
} else {
    Write-Host "✓ 未发现明显错误" -ForegroundColor Green
}
Write-Host ""

# 总结
Write-Host "=================================================================================" -ForegroundColor Cyan
Write-Host "诊断总结" -ForegroundColor Cyan
Write-Host "=================================================================================" -ForegroundColor Cyan

$issues = @()
if (-not $dllLoaded) { $issues += "DLL 未加载" }
if (-not $selectLogs) { $issues += "Patch 未触发" }
if (-not $pendingLogs) { $issues += "PendingMenuSwitch 未设置" }
if (-not $switchLogs) { $issues += "路由未触发" }
if ($errors) { $issues += "发现错误" }

if ($issues.Count -eq 0) {
    Write-Host "✓ 所有检查项都正常，但路由仍然失败" -ForegroundColor Yellow
    Write-Host "  可能原因: 菜单 InputMenuId 不匹配或菜单未注册" -ForegroundColor Yellow
} else {
    Write-Host "发现以下问题:" -ForegroundColor Red
    foreach ($issue in $issues) {
        Write-Host "  - $issue" -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "详细日志已保存，请查看上述输出以定位问题" -ForegroundColor Cyan
Write-Host ""

