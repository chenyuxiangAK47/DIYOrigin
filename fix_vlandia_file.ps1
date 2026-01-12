# 修复 VlandiaOriginSystem.cs 文件 - 移除重复的命名空间块

$filePath = "SubModule\VlandiaOriginSystem.cs"
$content = Get-Content $filePath -Raw -Encoding UTF8

# 找到第一个命名空间块的结束位置（第一个 } 后跟空行和 using 语句）
# 第一个块从第10行开始（namespace OriginSystemMod），到第574行结束（}）

# 使用正则表达式提取第一个完整的命名空间块
$pattern = '(?s)(using System;.*?namespace OriginSystemMod.*?}\s*)(?=\s*using System\.Collections)'

if ($content -match $pattern) {
    $firstBlock = $matches[1]
    
    # 写入修复后的文件
    $firstBlock.TrimEnd() | Out-File $filePath -Encoding UTF8 -NoNewline
    
    Write-Host "文件已修复：移除了所有重复的命名空间块"
    Write-Host "原始文件大小：$($content.Length) 字符"
    Write-Host "修复后文件大小：$($firstBlock.Length) 字符"
} else {
    Write-Host "无法匹配模式，尝试另一种方法..."
    
    # 方法2：找到第一个 } 后跟空行和 using 的位置
    $lines = Get-Content $filePath -Encoding UTF8
    $firstBlockEnd = -1
    
    for ($i = 0; $i -lt $lines.Count; $i++) {
        if ($lines[$i] -match '^\s*\}\s*$' -and $i + 1 -lt $lines.Count) {
            # 检查下一行是否是空行，再下一行是否是 using
            if ($i + 2 -lt $lines.Count -and 
                $lines[$i + 1] -match '^\s*$' -and 
                $lines[$i + 2] -match '^using ') {
                $firstBlockEnd = $i
                break
            }
        }
    }
    
    if ($firstBlockEnd -ge 0) {
        $firstBlock = $lines[0..$firstBlockEnd] -join "`r`n"
        $firstBlock | Out-File $filePath -Encoding UTF8 -NoNewline
        Write-Host "文件已修复：保留前 $($firstBlockEnd + 1) 行"
    } else {
        Write-Host "无法找到第一个块的结束位置"
    }
}


