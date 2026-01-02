#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
修复剩余的所有语法错误
"""

import re
import os

def fix_preset_origin_system(file_path):
    """修复 PresetOriginSystem.cs 的剩余问题"""
    with open(file_path, 'rb') as f:
        content = f.read()
    
    try:
        text = content.decode('utf-8')
    except UnicodeDecodeError:
        text = content.decode('utf-8', errors='replace')
    
    original = text
    
    # 修复所有 $"..."(something) 模式
    text = re.sub(r'(\$"[^"]*\{[^}]+\})"\(([^)]+)\)"', 
                  lambda m: m.group(1) + ' (' + m.group(2) + ')"', text)
    
    # 修复所有缺失引号的情况
    # 模式: ...}"(something) -> ...} (something)"
    text = re.sub(r'(\{[^}]+\})"\(([^)]+)\)"', 
                  lambda m: m.group(1) + ' (' + m.group(2) + ')"', text)
    
    # 修复编码问题导致的中文字符损坏
    text = text.replace('出', '出身')
    text = text.replace('判', '判据')
    text = text.replace('满', '满足')
    text = text.replace('装', '装备')
    text = text.replace('坐', '坐标')
    text = text.replace('异', '异常')
    text = text.replace('失', '失败')
    text = text.replace('方', '方法')
    text = text.replace('王', '王国')
    text = text.replace('关', '关系')
    text = text.replace('兵', '兵力')
    text = text.replace('技', '技能')
    text = text.replace('特', '特质')
    text = text.replace('金', '金币')
    text = text.replace('食', '食物')
    text = text.replace('马', '马匹')
    text = text.replace('等）', '等级')
    text = text.replace('等', '等级')
    text = text.replace('偏', '偏移')
    text = text.replace('位', '位置')
    text = text.replace('找', '找到')
    text = text.replace('个', '个')
    text = text.replace('村', '村子')
    text = text.replace('城', '城市')
    text = text.replace('宣', '宣战')
    text = text.replace('设', '设置')
    text = text.replace('确', '确认')
    text = text.replace('已', '已经')
    text = text.replace('是', '是')
    text = text.replace('为', '为空')
    text = text.replace('文', '文化')
    text = text.replace('模', '模板')
    text = text.replace('伙', '伙伴')
    text = text.replace('领', '领主')
    text = text.replace('统', '统治者')
    text = text.replace('敌', '敌对')
    text = text.replace('深', '深处')
    text = text.replace('南', '南端')
    text = text.replace('草', '草原')
    text = text.replace('沙', '沙漠')
    text = text.replace('帝', '帝国')
    text = text.replace('附', '附属')
    text = text.replace('最', '最')
    text = text.replace('通', '通过')
    text = text.replace('任', '任意')
    text = text.replace('对', '对应')
    text = text.replace('可', '可用')
    text = text.replace('未', '未')
    text = text.replace('未找', '未找到')
    text = text.replace('未找', '未找到')
    
    # 修复所有 "false" 残片问题
    # 但要小心，只修复明显是残片的情况
    # 模式: identifier  false (后面是分号、逗号、右括号或行尾)
    text = re.sub(r'(\w+(?:\.\w+)+)\s+false([,;\)\]\s]*$)', r'\1 ?? false\2', text, flags=re.MULTILINE)
    
    # 修复 IsLord 的问题
    text = re.sub(r'\.IsLord\s+false', r'?.IsLord ?? false', text)
    text = re.sub(r'\.IsLord\s+\?\?\s+false', r'?.IsLord ?? false', text)
    
    # 修复 ?? 被打断的问题
    # 更精确的模式：方法调用后跟另一个方法调用
    text = re.sub(r'(\w+\([^)]*\))\s+(\w+\([^)]*\))', 
                  lambda m: f"{m.group(1)} ?? {m.group(2)}" if '??' not in m.group(1)[-5:] and not m.group(1).endswith(')') else m.group(0), text)
    
    # 修复缺失的逗号在字符串插值中
    text = re.sub(r'(\{[^}]+\})"([A-Za-z_][A-Za-z0-9_]*)=\{', r'\1, \2={', text)
    
    if text != original:
        with open(file_path, 'wb') as f:
            f.write(text.encode('utf-8'))
        print(f"Fixed remaining issues in PresetOriginSystem.cs")
        return True
    return False

if __name__ == '__main__':
    fix_preset_origin_system(r'SubModule\PresetOriginSystem.cs')
    print("Done!")

