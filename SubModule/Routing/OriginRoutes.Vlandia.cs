using System.Collections.Generic;

namespace OriginSystemMod
{
    /// <summary>
    /// 瓦兰迪亚预设出身路由表
    /// 将路由逻辑从代码中分离，改为数据驱动
    /// </summary>
    public static class VlandiaRoutes
    {
        /// <summary>
        /// 预设出身选择 → Node1 映射
        /// </summary>
        private static readonly Dictionary<string, string> PresetOriginToNode1 = new Dictionary<string, string>()
        {
            { "vlandia_expedition_knight", "expedition_knight_node1" },
            { "vlandia_bankrupt_banneret", "bankrupt_banneret_node1" },
            { "vlandia_tourney_champion", "tourney_champion_node1" },
            { "vlandia_black_path_captain", "black_path_captain_node1" },
            { "vlandia_crossbow_guild", "crossbow_guild_node1" },
            { "vlandia_march_warden", "march_warden_node1" },
            { "vlandia_order_squire", "order_squire_node1" },
            { "vlandia_sellsword_lieutenant", "sellsword_lieutenant_node1" },
            { "vlandia_tax_bailiff_enforcer", "tax_bailiff_enforcer_node1" },
            { "vlandia_free_militia_leader", "free_militia_leader_node1" },
            { "vlandia_degraded_rogue_knight", "degraded_rogue_knight_node1" },
        };

        /// <summary>
        /// 节点流转规则 (curMenuId, optionId) → nextMenuId
        /// 格式: "curMenuId:optionId" → "nextMenuId"
        /// </summary>
        private static readonly Dictionary<string, string> NodeTransitions = new Dictionary<string, string>()
        {
            // 远征的骑士
            { "expedition_knight_node1:expedition_fall_erased_defeat", "expedition_knight_node2" },
            { "expedition_knight_node1:expedition_fall_annexed", "expedition_knight_node2" },
            { "expedition_knight_node1:expedition_fall_debt", "expedition_knight_node2" },
            { "expedition_knight_node1:expedition_fall_political", "expedition_knight_node2" },
            { "expedition_knight_node2:expedition_oath_king", "expedition_knight_node3" },
            { "expedition_knight_node2:expedition_oath_justice", "expedition_knight_node3" },
            { "expedition_knight_node2:expedition_oath_glory", "expedition_knight_node3" },
            { "expedition_knight_node2:expedition_oath_cynical", "expedition_knight_node3" },
            { "expedition_knight_node3:expedition_location_west", "expedition_knight_node4" },
            { "expedition_knight_node3:expedition_location_east", "expedition_knight_node4" },
            { "expedition_knight_node3:expedition_location_south", "expedition_knight_node4" },
            { "expedition_knight_node3:expedition_location_north", "expedition_knight_node4" },
            // 破产的旗主继承人
            { "bankrupt_banneret_node1:bankrupt_reason_ransom", "bankrupt_banneret_node2" },
            { "bankrupt_banneret_node1:bankrupt_reason_trade_loss", "bankrupt_banneret_node2" },
            { "bankrupt_banneret_node1:bankrupt_reason_defeat", "bankrupt_banneret_node2" },
            { "bankrupt_banneret_node2:bankrupt_face_sell_armor", "bankrupt_banneret_node3" },
            { "bankrupt_banneret_node2:bankrupt_face_keep_armor", "bankrupt_banneret_node3" },
            { "bankrupt_banneret_node2:bankrupt_face_mortgage", "bankrupt_banneret_node3" },
            { "bankrupt_banneret_node3:bankrupt_troops_crossbow", "bankrupt_banneret_node4" },
            { "bankrupt_banneret_node3:bankrupt_troops_infantry", "bankrupt_banneret_node4" },
            { "bankrupt_banneret_node3:bankrupt_troops_mixed", "bankrupt_banneret_node4" },
            // 比武场的落魄冠军
            { "tourney_champion_node1:tourney_fall_injury", "tourney_champion_node2" },
            { "tourney_champion_node1:tourney_fall_age", "tourney_champion_node2" },
            { "tourney_champion_node1:tourney_fall_scandal", "tourney_champion_node2" },
            { "tourney_champion_node1:tourney_fall_betrayal", "tourney_champion_node2" },
            // 黑旗匪首
            { "black_path_captain_node1:black_path_type_robin", "black_path_captain_node2" },
            { "black_path_captain_node1:black_path_type_evil", "black_path_captain_node2" },
            { "black_path_captain_node1:black_path_type_revenge", "black_path_captain_node2" },
            { "black_path_captain_node1:black_path_type_hire", "black_path_captain_node2" },
            { "black_path_captain_node2:black_path_team_bandits", "black_path_captain_node3" },
            { "black_path_captain_node2:black_path_team_mercenaries", "black_path_captain_node3" },
            { "black_path_captain_node2:black_path_team_peasants", "black_path_captain_node3" },
            { "black_path_captain_node3:black_path_style_fear", "black_path_captain_node4" },
            { "black_path_captain_node3:black_path_style_stealth", "black_path_captain_node4" },
            { "black_path_captain_node3:black_path_style_brutal", "black_path_captain_node4" },
            // 堕落无赖骑士（现在是独立的preset）
            { "degraded_rogue_knight_node1:degraded_crime_tyranny", "degraded_rogue_knight_node2" },
            { "degraded_rogue_knight_node1:degraded_crime_indulgence", "degraded_rogue_knight_node2" },
            { "degraded_rogue_knight_node1:degraded_crime_filth", "degraded_rogue_knight_node2" },
            { "degraded_rogue_knight_node1:degraded_crime_conspiracy", "degraded_rogue_knight_node2" },
            { "degraded_rogue_knight_node2:degraded_first_case_fear", "degraded_rogue_knight_node3" },
            { "degraded_rogue_knight_node2:degraded_first_case_alliance", "degraded_rogue_knight_node3" },
            { "degraded_rogue_knight_node2:degraded_first_case_betrayal", "degraded_rogue_knight_node3" },
            { "degraded_rogue_knight_node2:degraded_first_case_harm", "degraded_rogue_knight_node3" },
            { "degraded_rogue_knight_node3:degraded_view_strength", "degraded_rogue_knight_node4" },
            { "degraded_rogue_knight_node3:degraded_view_pleasure", "degraded_rogue_knight_node4" },
            { "degraded_rogue_knight_node3:degraded_view_money", "degraded_rogue_knight_node4" },
            { "degraded_rogue_knight_node3:degraded_view_no_kill", "degraded_rogue_knight_node4" },
            // 其他节点的流转规则可以继续添加...
        };

        /// <summary>
        /// 获取预设出身的第一个节点
        /// </summary>
        public static string GetPresetOriginNode1(string originId)
        {
            return PresetOriginToNode1.TryGetValue(originId, out var node1) ? node1 : null;
        }

        /// <summary>
        /// 获取预设节点的下一个节点
        /// </summary>
        public static string GetPresetNodeTransition(string curMenuId, string optId)
        {
            var key = $"{curMenuId}:{optId}";
            return NodeTransitions.TryGetValue(key, out var next) ? next : null;
        }
    }
}
