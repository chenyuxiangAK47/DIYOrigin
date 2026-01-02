# PowerShell Script to Fix C# File Encoding and Quote Issues
# Usage: .\Fix-EncodingAndQuotes.ps1

param(
    [string]$ProjectPath = ".",
    [switch]$DryRun = $false
)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "C# 文件编码和引号修复脚本" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$ErrorActionPreference = "Continue"
$filesFound = @()
$filesFixed = @()

# Step 1: Find all .cs files
Write-Host "[1/5] 搜索所有 .cs 文件..." -ForegroundColor Yellow
$csFiles = Get-ChildItem -Path $ProjectPath -Filter "*.cs" -Recurse | Where-Object { $_.FullName -notmatch "\\bin\\|\\obj\\" }
Write-Host "找到 $($csFiles.Count) 个 .cs 文件" -ForegroundColor Green
Write-Host ""

# Step 2: Check for problematic characters
Write-Host "[2/5] 检查问题字符..." -ForegroundColor Yellow

# Patterns to check
$patterns = @{
    "智能引号（中文左引号）" = "[""]"
    "智能引号（中文右引号）" = "[""]"
    "全角双引号" = "[＂]"
    "全角逗号" = "，"
    "全角分号" = "；"
    "全角冒号" = "："
    "全角左括号" = "（"
    "全角右括号" = "）"
    "乱码字符" = ""
}

foreach ($file in $csFiles) {
    try {
        $content = Get-Content -Path $file.FullName -Raw -Encoding UTF8 -ErrorAction SilentlyContinue
        if (-not $content) {
            # Try UTF8 with BOM
            $content = Get-Content -Path $file.FullName -Raw -Encoding UTF8 -ErrorAction SilentlyContinue
        }
        
        $issues = @()
        
        foreach ($patternName in $patterns.Keys) {
            $pattern = $patterns[$patternName]
            if ($content -match $pattern) {
                $matches = [regex]::Matches($content, $pattern)
                $lineNumbers = @()
                
                # Find line numbers
                $lines = $content -split "`r?`n"
                for ($i = 0; $i -lt $lines.Length; $i++) {
                    if ($lines[$i] -match $pattern) {
                        $lineNumbers += ($i + 1)
                    }
                }
                
                if ($lineNumbers.Count -gt 0) {
                    $issues += "  - $patternName`: 行 $($lineNumbers -join ', ') ($($matches.Count) 个匹配)"
                }
            }
        }
        
        if ($issues.Count -gt 0) {
            $filesFound += [PSCustomObject]@{
                File = $file.FullName
                Issues = $issues
            }
        }
    }
    catch {
        Write-Host "错误: 无法读取文件 $($file.FullName): $_" -ForegroundColor Red
    }
}

if ($filesFound.Count -eq 0) {
    Write-Host "未发现包含问题字符的文件" -ForegroundColor Green
}
else {
    Write-Host "发现 $($filesFound.Count) 个文件包含问题字符:" -ForegroundColor Yellow
    foreach ($fileInfo in $filesFound) {
        Write-Host "`n文件: $($fileInfo.File)" -ForegroundColor Cyan
        foreach ($issue in $fileInfo.Issues) {
            Write-Host $issue -ForegroundColor Red
        }
    }
}
Write-Host ""

# Step 3: Fix issues
if ($filesFound.Count -gt 0) {
    Write-Host "[3/5] 修复问题..." -ForegroundColor Yellow
    
    foreach ($fileInfo in $filesFound) {
        $filePath = $fileInfo.File
        
        try {
            # Read file with UTF-8
            $content = [System.IO.File]::ReadAllText($filePath, [System.Text.Encoding]::UTF8)
            $originalContent = $content
            $fixed = $false
            
            # Fix smart quotes and full-width quotes (ONLY if used as string delimiters or in code syntax positions)
            # This is tricky - we need to be careful not to replace quotes inside strings
            # For now, we'll replace all smart quotes with regular quotes (safer)
            if ($content -match '[""＂]') {
                $content = $content -replace '"', '"'  # Left smart quote
                $content = $content -replace '"', '"'  # Right smart quote
                $content = $content -replace '＂', '"'  # Full-width quote
                $fixed = $true
            }
            
            # Fix full-width punctuation in CODE SYNTAX positions (not in strings)
            # This requires careful parsing - for now, we'll do a conservative replacement
            # that only affects common syntax patterns
            
            # Fix full-width comma in parameter lists (common pattern: Foo(a， b))
            $content = $content -replace '([a-zA-Z0-9_\)\]\}])(，)(\s*[a-zA-Z0-9_\(\[\{])', '$1,$3'
            # Fix full-width semicolon at end of statements
            $content = $content -replace '(；)(\s*$)', ';$2'
            $content = $content -replace '(；)(\s*\/\/)', ';$2'
            # Fix full-width colon in code (but be careful with string interpolation)
            # This is more complex, so we'll be conservative
            
            if ($content -ne $originalContent) {
                $fixed = $true
            }
            
            if ($fixed -and -not $DryRun) {
                # Save with UTF-8 with BOM
                $utf8WithBom = New-Object System.Text.UTF8Encoding $true
                [System.IO.File]::WriteAllText($filePath, $content, $utf8WithBom)
                $filesFixed += $filePath
                Write-Host "已修复: $filePath" -ForegroundColor Green
            }
            elseif ($fixed -and $DryRun) {
                Write-Host "（试运行）需要修复: $filePath" -ForegroundColor Yellow
            }
        }
        catch {
            Write-Host "错误: 无法修复文件 $filePath : $_" -ForegroundColor Red
        }
    }
}
else {
    Write-Host "[3/5] 跳过修复（未发现需要修复的文件）" -ForegroundColor Gray
}
Write-Host ""

# Step 4: Convert all files to UTF-8 with BOM
Write-Host "[4/5] 统一所有 .cs 文件为 UTF-8 with BOM..." -ForegroundColor Yellow

$convertedCount = 0
foreach ($file in $csFiles) {
    try {
        $content = [System.IO.File]::ReadAllText($file.FullName, [System.Text.Encoding]::UTF8)
        $utf8WithBom = New-Object System.Text.UTF8Encoding $true
        [System.IO.File]::WriteAllText($file.FullName, $content, $utf8WithBom)
        $convertedCount++
    }
    catch {
        Write-Host "警告: 无法转换 $($file.FullName): $_" -ForegroundColor Yellow
    }
}

Write-Host "已转换 $convertedCount 个文件为 UTF-8 with BOM" -ForegroundColor Green
Write-Host ""

# Step 5: Create/Update .editorconfig
Write-Host "[5/5] 创建/更新 .editorconfig..." -ForegroundColor Yellow

$editorConfigPath = Join-Path $ProjectPath ".editorconfig"
$editorConfigContent = @"
root = true

[*]
charset = utf-8-bom
end_of_line = crlf
insert_final_newline = true

[*.cs]
charset = utf-8-bom
indent_style = space
indent_size = 4
"@

if (-not (Test-Path $editorConfigPath)) {
    Set-Content -Path $editorConfigPath -Value $editorConfigContent -Encoding UTF8
    Write-Host "已创建 .editorconfig" -ForegroundColor Green
}
else {
    Write-Host ".editorconfig 已存在，跳过创建" -ForegroundColor Gray
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "修复完成！" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "修复的文件数: $($filesFixed.Count)" -ForegroundColor Green
Write-Host "转换编码的文件数: $convertedCount" -ForegroundColor Green
Write-Host ""
Write-Host "下一步:" -ForegroundColor Yellow
Write-Host "1. 运行 'dotnet clean'" -ForegroundColor White
Write-Host "2. 运行 'dotnet build' 验证编译" -ForegroundColor White
Write-Host ""














