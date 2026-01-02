// Util/ReflectionUtil.cs
// 反射工具类，用于访问和修改游戏内部对象

using System;
using System.Linq;
using System.Reflection;
using TaleWorlds.CampaignSystem.CharacterCreationContent;

namespace OriginSystemMod
{
    /// <summary>
    /// 反射工具类，提供安全的反射操作
    /// </summary>
    public static class ReflectionUtil
    {
        /// <summary>
        /// 通过反射设置 CharacterCreationManager.CurrentMenu
        /// 不硬写字段名，枚举找 NarrativeMenu 类型的字段
        /// </summary>
        public static bool TrySetCurrentMenu(CharacterCreationManager mgr, NarrativeMenu target)
        {
            var flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            var fields = typeof(CharacterCreationManager).GetFields(flags);

            // 先尝试找字段
            foreach (var f in fields)
            {
                if (typeof(NarrativeMenu).IsAssignableFrom(f.FieldType))
                {
                    OriginLog.Info($"CurrentMenu backing field candidate = {f.Name} (类型: {f.FieldType.Name})");
                    try
                    {
                        f.SetValue(mgr, target);
                        OriginLog.Info($"成功通过字段 {f.Name} 设置 CurrentMenu");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        OriginLog.Error($"通过字段 {f.Name} 设置失败: {ex.Message}");
                    }
                }
            }

            // 如果字段方式失败，尝试通过属性的 setter
            var prop = typeof(CharacterCreationManager).GetProperty("CurrentMenu", flags);
            if (prop != null && prop.CanWrite)
            {
                try
                {
                    prop.SetValue(mgr, target);
                    OriginLog.Info("成功通过属性 setter 设置 CurrentMenu");
                    return true;
                }
                catch (Exception ex)
                {
                    OriginLog.Error($"通过属性 setter 设置失败: {ex.Message}");
                }
            }
            else
            {
                OriginLog.Error("CurrentMenu 属性没有 setter 或不可写");
            }

            OriginLog.Error("所有方式都失败：没找到 NarrativeMenu 类型的字段，且属性不可写");
            return false;
        }

        /// <summary>
        /// 调用 ModifyMenuCharacters（如果存在）
        /// </summary>
        public static void InvokeModifyMenuCharacters(CharacterCreationManager mgr)
        {
            var flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            
            // 先列出所有同名方法
            var methods = typeof(CharacterCreationManager).GetMethods(flags)
                .Where(m => m.Name == "ModifyMenuCharacters")
                .ToArray();
            
            if (methods.Length == 0)
            {
                OriginLog.Info("ModifyMenuCharacters 方法不存在，跳过调用");
                return;
            }

            OriginLog.Info($"找到 {methods.Length} 个 ModifyMenuCharacters 方法:");
            foreach (var m in methods)
            {
                var paramTypes = m.GetParameters().Select(p => p.ParameterType.Name).ToArray();
                OriginLog.Info($"  - {m.Name}({string.Join(", ", paramTypes)})");
            }

            // 尝试调用无参数版本
            var method = methods.FirstOrDefault(m => m.GetParameters().Length == 0);
            if (method != null)
            {
                try
                {
                    method.Invoke(mgr, null);
                    OriginLog.Info("成功调用 ModifyMenuCharacters()");
                }
                catch (Exception ex)
                {
                    OriginLog.Error($"调用 ModifyMenuCharacters() 失败: {ex.Message}");
                }
            }
            else
            {
                OriginLog.Info("ModifyMenuCharacters 没有无参数版本，跳过调用");
            }
        }
    }
}
















































