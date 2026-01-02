#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
全面修复语法错误
"""

import re
import os

def fix_preset_origin_system(file_path):
    """修复 PresetOriginSystem.cs 的所有语法错误"""
    with open(file_path, 'rb') as f:
        content = f.read()
    
    try:
        text = content.decode('utf-8')
    except UnicodeDecodeError:
        text = content.decode('utf-8', errors='replace')
    
    original = text
    fixes = []
    
    # 修复1: 字符串插值后面又跟 "(... 的错误
    # 模式: $"..."(something...) -> $"... (something...)"
    text = re.sub(r'(\$"[^"]*\{[^}]+\})"\(([^)]+)\)"', 
                  lambda m: m.group(1) + ' (' + m.group(2) + ')"', text)
    fixes.append("Fixed string interpolation with parentheses")
    
    # 修复2: OriginLog.Info($"...", field={...}); 这种非法写法
    # 模式: OriginLog.Info($"...", field={...}); -> OriginLog.Info($"... field={...}");
    text = re.sub(r'OriginLog\.(Info|Warning|Error)\(\$"([^"]+)"\),\s*([A-Za-z_][A-Za-z0-9_]*)=\{([^}]+)\}\)', 
                  r'OriginLog.\1($"\2, \3={\4}")', text)
    
    # 修复3: 修复所有类似的模式 $"..."field={...}
    text = re.sub(r'(\$"[^"]*\{[^}]+\})"([A-Za-z_][A-Za-z0-9_]*)=\{', r'\1, \2={', text)
    
    # 修复4: 修复注释吞代码 - 检查每一行
    lines = text.split('\n')
    new_lines = []
    for i, line in enumerate(lines):
        stripped = line.strip()
        # 检查是否是注释行但后面有代码关键字
        if stripped.startswith('//') and len(stripped) > 2:
            after_comment = stripped[2:].strip()
            # 检查是否有代码关键字
            code_keywords = ['bool ', 'var ', 'if ', 'OriginLog', 'Debug.Print', 'return ', 'foreach ', 'for ', 'while ', 'using ', 'CampaignVec2 ', 'position =', 'var positionProperty', 'bool teleportSuccess', 'setMethod.IsPublic', 'isNobleProperty.CanWrite']
            for kw in code_keywords:
                if kw in after_comment:
                    # 找到关键字位置
                    kw_pos = after_comment.find(kw)
                    if kw_pos > 0:
                        # 分离注释和代码
                        comment_part = line[:line.find('//') + 2] + after_comment[:kw_pos].rstrip()
                        code_part = after_comment[kw_pos:]
                        indent = len(line) - len(line.lstrip())
                        new_lines.append(comment_part)
                        new_lines.append(' ' * indent + code_part)
                        fixes.append(f"Fixed comment eating code at line {i+1}: {kw}")
                        break
            else:
                new_lines.append(line)
        else:
            new_lines.append(line)
    text = '\n'.join(new_lines)
    
    # 修复5: 修复 "表达式后面莫名出现 false" 的问题
    # 但要小心，不要误修复正常的 false
    # 模式: identifier  false (在行尾或后面是分号/逗号/右括号)
    text = re.sub(r'(\w+(?:\.\w+)+)\s+false([,;\)\]])', r'\1 ?? false\2', text)
    text = re.sub(r'(\w+(?:\.\w+)+)\s+false\s*$', r'\1 ?? false', text, flags=re.MULTILINE)
    
    # 修复6: 修复 ?? 被打断的问题（更精确的模式）
    # 模式: method()  otherMethod() -> method() ?? otherMethod()
    text = re.sub(r'(\w+\([^)]*\))\s+(\w+\([^)]*\))', 
                  lambda m: f"{m.group(1)} ?? {m.group(2)}" if '??' not in m.group(1)[-5:] else m.group(0), text)
    
    # 修复7: 修复 IsLord ?? false 错误（IsLord不是nullable）
    text = re.sub(r'\.IsLord\s+\?\?\s+false', r'?.IsLord ?? false', text)
    text = re.sub(r'\.IsLord\s+false', r'?.IsLord ?? false', text)
    
    # 修复8: 修复缺失的引号 - 查找未闭合的字符串字面量
    # 这个需要更仔细的处理，先处理明显的模式
    
    if text != original:
        with open(file_path, 'wb') as f:
            f.write(text.encode('utf-8'))
        print(f"Fixed PresetOriginSystem.cs: {len(fixes)} fix categories")
        return True
    return False

def fix_add_parents_menu_patch(file_path):
    """修复 AddParentsMenuPatch.cs"""
    with open(file_path, 'rb') as f:
        content = f.read()
    
    try:
        text = content.decode('utf-8')
    except UnicodeDecodeError:
        text = content.decode('utf-8', errors='replace')
    
    original = text
    
    # 找到第一个namespace
    namespace_match = re.search(r'namespace\s+(\w+)', text)
    if namespace_match:
        namespace_start = namespace_match.start()
        # 收集文件开头的所有using语句
        header_using = []
        lines = text[:namespace_start].split('\n')
        for line in lines:
            if line.strip().startswith('using '):
                header_using.append(line.strip())
        
        # 移除namespace之后的所有using语句
        text_after = text[namespace_start:]
        text_after = re.sub(r'^using\s+[^;]+;\s*\n', '', text_after, flags=re.MULTILINE)
        
        # 重新组合
        header_text = '\n'.join(lines[:len([l for l in lines if l.strip().startswith('using ')])])
        text = header_text + '\n\n' + text_after
    
    if text != original:
        with open(file_path, 'wb') as f:
            f.write(text.encode('utf-8'))
        print(f"Fixed AddParentsMenuPatch.cs")
        return True
    return False

def fix_rebel_chief_nodes(file_path):
    """修复 RebelChiefNodes.cs"""
    with open(file_path, 'rb') as f:
        content = f.read()
    
    try:
        text = content.decode('utf-8')
    except UnicodeDecodeError:
        text = content.decode('utf-8', errors='replace')
    
    original = text
    
    # 找到最后一个完整的class定义
    # 查找所有class定义
    class_pattern = r'^\s*(public\s+)?class\s+\w+'
    lines = text.split('\n')
    last_class_line = -1
    for i, line in enumerate(lines):
        if re.match(class_pattern, line.strip()):
            last_class_line = i
    
    if last_class_line >= 0:
        # 从最后一个class开始，找到匹配的闭合括号
        brace_count = 0
        found_open = False
        end_line = len(lines)
        
        for i in range(last_class_line, len(lines)):
            line = lines[i]
            for char in line:
                if char == '{':
                    brace_count += 1
                    found_open = True
                elif char == '}':
                    brace_count -= 1
                    if found_open and brace_count == 0:
                        end_line = i + 1
                        break
            if end_line < len(lines):
                break
        
        # 截断到正确位置
        if end_line < len(lines):
            text = '\n'.join(lines[:end_line]) + '\n'
    
    if text != original:
        with open(file_path, 'wb') as f:
            f.write(text.encode('utf-8'))
        print(f"Fixed RebelChiefNodes.cs")
        return True
    return False

if __name__ == '__main__':
    base_path = r'SubModule'
    
    fix_preset_origin_system(os.path.join(base_path, 'PresetOriginSystem.cs'))
    fix_add_parents_menu_patch(os.path.join(base_path, 'Patches', 'AddParentsMenuPatch.cs'))
    fix_rebel_chief_nodes(os.path.join(base_path, 'Menus', 'Preset', 'Nodes', 'RebelChiefNodes.cs'))
    print("All fixes applied!")


