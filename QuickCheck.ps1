# Quick Check Script - 快速检查问题字符
# Usage: .\QuickCheck.ps1

$files = Get-ChildItem -Path "SubModule\Menus" -Filter "*.cs" -Recurse

Write-Host "检查智能引号和全角标点..." -ForegroundColor Yellow
Write-Host ""

$issues = @()

foreach ($file in $files) {
    try {
        $content = Get-Content $file.FullName -Raw -Encoding UTF8 -ErrorAction SilentlyContinue
        if (-not $content) {
            $content = Get-Content $file.FullName -Raw -Encoding Default
        }
        
        $hasIssue = $false
        $details = @()
        
        if ($content -match '"') { $hasIssue = $true; $details += "智能左引号" }
        if ($content -match '"') { $hasIssue = $true; $details += "智能右引号" }
        if ($content -match '＂') { $hasIssue = $true; $details += "全角引号" }
        if ($content -match '') { $hasIssue = $true; $details += "乱码字符" }
        
        if ($hasIssue) {
            $issues += [PSCustomObject]@{
                File = $file.Name
                Path = $file.FullName
                Details = $details -join ", "
            }
        }
    }
    catch {
        Write-Host "错误: $($file.Name) - $_" -ForegroundColor Red
    }
}

if ($issues.Count -gt 0) {
    Write-Host "发现 $($issues.Count) 个文件有问题:" -ForegroundColor Red
    $issues | Format-Table -AutoSize
}
else {
    Write-Host "✓ 未发现问题字符" -ForegroundColor Green
}














