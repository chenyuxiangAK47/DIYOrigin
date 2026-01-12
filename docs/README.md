# OriginSystemMod - 出身系统 Mod

## 功能概述

在沙盒模式下，为角色创建流程添加出身系统，包括：

1. **预设出身（Preset Origin）** - 带剧本、带势力的开局
   - 草原叛酋
   - 迁徙王族
   - 战奴逃亡
   - ...（其他预设出身）

2. **非预设出身（Non-Preset Origin）** - 自由拼装的开局
   - Step 0: 文化锚点
   - Step 1: 社会出身
   - Step 2: 技能来源
   - Step 3: 当前状态

## 项目结构

```
OriginSystemMod/
├── SubModule.xml                    # Mod 配置文件
├── OriginSystemMod.csproj           # 项目文件
├── README.md                        # 说明文档
└── SubModule/
    ├── OriginSystemSubModule.cs     # 主入口
    ├── OriginSystemHelper.cs        # 全局辅助类
    ├── OriginSystemPatches.cs       # Harmony Patches
    ├── OriginSystemCampaignBehavior.cs  # CampaignBehavior
    ├── PresetOriginSystem.cs        # 预设出身系统
    └── NonPresetOriginSystem.cs     # 非预设出身系统
```

## 实现状态

### MVP（待实现）
- [ ] 创建"出身类型选择"菜单（二选一）
- [ ] 实现预设出身分支（至少 3 个预设出身）
- [ ] 实现非预设出身分支（三步选择）
- [ ] 在 OnSessionLaunched 应用预设出身效果

## 编译

```powershell
cd "D:\SteamLibrary\steamapps\common\Mount & Blade II Bannerlord\Modules\OriginSystemMod"
msbuild OriginSystemMod.csproj /p:Configuration=Release /p:Platform=x64
```

## 参考文档

- `QuickStartMod/出身系统二选一实现步骤.md` - 实现步骤文档
- `QuickStartMod/出身系统设计文档.md` - 设计文档
- `cursor_.md` - 之前的设计讨论


























































