# PowerShell Script to Check for Problematic Characters in C# Files
# Usage: .\Check-ProblematicChars.ps1

param(
    [string]$ProjectPath = "."
)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "检查 C# 文件中的问题字符" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$ErrorActionPreference = "Continue"

# Find all .cs files
$csFiles = Get-ChildItem -Path $ProjectPath -Filter "*.cs" -Recurse | Where-Object { $_.FullName -notmatch "\\bin\\|\\obj\\" }
Write-Host "搜索到 $($csFiles.Count) 个 .cs 文件" -ForegroundColor Green
Write-Host ""

# Patterns to check
$checks = @(
    @{ Name = "智能引号（左）"; Pattern = '"'; Unicode = "U+201C" },
    @{ Name = "智能引号（右）"; Pattern = '"'; Unicode = "U+201D" },
    @{ Name = "全角双引号"; Pattern = "＂"; Unicode = "U+FF02" },
    @{ Name = "全角逗号"; Pattern = "，"; Unicode = "U+FF0C" },
    @{ Name = "全角分号"; Pattern = "；"; Unicode = "U+FF1B" },
    @{ Name = "全角冒号"; Pattern = "："; Unicode = "U+FF1A" },
    @{ Name = "全角左括号"; Pattern = "（"; Unicode = "U+FF08" },
    @{ Name = "全角右括号"; Pattern = "）"; Unicode = "U+FF09" },
    @{ Name = "乱码字符"; Pattern = ""; Unicode = "U+FFFD" }
)

$results = @()

foreach ($file in $csFiles) {
    try {
        # Try UTF-8 first
        $content = $null
        $encoding = $null
        
        try {
            $content = [System.IO.File]::ReadAllText($file.FullName, [System.Text.Encoding]::UTF8)
            $encoding = "UTF-8"
        }
        catch {
            try {
                $content = [System.IO.File]::ReadAllText($file.FullName, [System.Text.Encoding]::Default)
                $encoding = "Default (ANSI)"
            }
            catch {
                Write-Host "警告: 无法读取 $($file.FullName)" -ForegroundColor Yellow
                continue
            }
        }
        
        if ($null -eq $content) { continue }
        
        $fileIssues = @()
        
        foreach ($check in $checks) {
            if ($content.Contains($check.Pattern)) {
                $count = ([regex]::Matches($content, [regex]::Escape($check.Pattern))).Count
                
                # Find line numbers
                $lines = $content -split "`r?`n"
                $lineNumbers = @()
                for ($i = 0; $i -lt $lines.Length; $i++) {
                    if ($lines[$i].Contains($check.Pattern)) {
                        $lineNumbers += ($i + 1)
                        if ($lineNumbers.Count -ge 10) {
                            $lineNumbers += "..."
                            break
                        }
                    }
                }
                
                $fileIssues += [PSCustomObject]@{
                    Character = $check.Name
                    Pattern = $check.Pattern
                    Unicode = $check.Unicode
                    Count = $count
                    Lines = $lineNumbers
                }
            }
        }
        
        if ($fileIssues.Count -gt 0) {
            $results += [PSCustomObject]@{
                File = $file.FullName.Replace($ProjectPath + "\", "")
                Encoding = $encoding
                Issues = $fileIssues
            }
        }
    }
    catch {
        Write-Host "错误: 处理文件 $($file.FullName) 时出错: $_" -ForegroundColor Red
    }
}

# Display results
if ($results.Count -eq 0) {
    Write-Host "✓ 未发现包含问题字符的文件" -ForegroundColor Green
}
else {
    Write-Host "发现 $($results.Count) 个文件包含问题字符:`n" -ForegroundColor Yellow
    
    foreach ($result in $results) {
        Write-Host "文件: $($result.File)" -ForegroundColor Cyan
        Write-Host "  编码: $($result.Encoding)" -ForegroundColor Gray
        
        foreach ($issue in $result.Issues) {
            $lineInfo = if ($issue.Lines.Count -le 5) {
                "行: $($issue.Lines -join ', ')"
            }
            else {
                "行: $($issue.Lines[0..4] -join ', ') ... (共 $($issue.Count) 处)"
            }
            
            Write-Host "  ❌ $($issue.Character) ($($issue.Unicode)): $($issue.Count) 个匹配, $lineInfo" -ForegroundColor Red
        }
        Write-Host ""
    }
    
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host "建议:" -ForegroundColor Yellow
    Write-Host "1. 运行 .\Fix-EncodingAndQuotes.ps1 自动修复" -ForegroundColor White
    Write-Host "2. 或者手动检查和修复上述文件" -ForegroundColor White
    Write-Host ""
}














