// Routing/OriginRoutes.NonPreset.cs
// 非预设出身的路由表

using System.Collections.Generic;

namespace OriginSystemMod
{
    /// <summary>
    /// 非预设出身路由表
    /// </summary>
    public static class NonPresetRoutes
    {
        /// <summary>
        /// 社会出身 → 风味节点菜单ID映射
        /// </summary>
        private static readonly Dictionary<string, string> SocialOriginToFlavorNode = new()
        {
            { "nomad_camp", "non_preset_nomad_role" },
            { "urban_artisan", "non_preset_artisan_trade" },
            { "caravan_follower", "non_preset_caravan_duty" },
            { "minor_clan", "non_preset_minor_clan_position" },
            { "wanderer", "non_preset_refugee_burden" },
            { "rural_peasant", "non_preset_peasant_role" },
            { "urban_poor", "non_preset_urban_poor_role" },
        };

        /// <summary>
        /// 风味节点菜单ID列表（用于判断当前菜单是否是风味节点）
        /// </summary>
        private static readonly HashSet<string> FlavorNodeMenuIds = new()
        {
            "non_preset_nomad_role",
            "non_preset_artisan_trade",
            "non_preset_caravan_duty",
            "non_preset_minor_clan_position",
            "non_preset_refugee_burden",
            "non_preset_peasant_role",
            "non_preset_urban_poor_role",
        };

        /// <summary>
        /// 根据选项ID获取风味节点菜单ID
        /// 选项ID格式: social_origin_{socialOriginId}
        /// </summary>
        public static string GetFlavorNode(string optionId)
        {
            if (string.IsNullOrEmpty(optionId) || !optionId.StartsWith("social_origin_"))
            {
                return null;
            }

            string socialOriginId = optionId.Substring("social_origin_".Length);
            
            // 检查是否需要风味节点
            if (SocialOriginToFlavorNode.TryGetValue(socialOriginId, out var flavorMenuId))
            {
                return flavorMenuId;
            }
            
            // 不需要风味节点，直接进入技能来源菜单
            return "non_preset_skill_background";
        }

        /// <summary>
        /// 判断是否是风味节点菜单
        /// </summary>
        public static bool IsFlavorNode(string menuId)
        {
            return FlavorNodeMenuIds.Contains(menuId);
        }
    }
}

































