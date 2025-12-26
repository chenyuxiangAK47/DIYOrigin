# 代码拆分模板

这个目录包含了拆分后的代码结构模板，可以作为拆分的参考。

## 文件说明

### 核心结构
- `OriginSystemPatches.partial.cs` - 主文件，使用partial class
- `Util/OriginLog.cs` - 统一日志工具
- `Util/ReflectionUtil.cs` - 反射工具类

### 路由系统
- `Routing/OriginMenuRouter.cs` - 核心路由逻辑
- `Routing/OriginRoutes.Khuzait.cs` - 预设出身路由表
- `Routing/OriginRoutes.NonPreset.cs` - 非预设路由表

## 使用方法

1. **阶段1**: 先创建 `Util/` 目录和文件，移动日志和反射工具
2. **阶段2**: 创建 `Routing/` 目录，移动路由逻辑
3. **阶段3**: 逐步拆分菜单工厂到 `Menus/` 目录
4. **阶段4**: 最后处理文本集中管理

## 注意事项

- 所有新文件都要在 `namespace OriginSystemMod` 下
- 使用 `partial class` 避免修改调用点
- 保持功能不变，只做"搬家"
- 每个阶段完成后都要编译测试

































