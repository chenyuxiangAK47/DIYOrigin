using System.Collections.Generic;

namespace OriginSystemMod
{
    /// <summary>
    /// 库塞特预设出身路由表
    /// 将路由逻辑从代码中分离，改为数据驱动
    /// </summary>
    public static class KhuzaitRoutes
    {
        /// <summary>
        /// 预设出身选择 → Node1 映射
        /// </summary>
        private static readonly Dictionary<string, string> PresetOriginToNode1 = new Dictionary<string, string>()
        {
            { "khuzait_rebel_chief", "rebel_chief_node1" },
            { "khuzait_minor_noble", "minor_noble_node1" },
            { "khuzait_migrant_chief", "migrant_chief_node1" },
            { "khuzait_army_deserter", "army_deserter_node1" },
            { "khuzait_trade_protector", "trade_protector_node1" },
            { "khuzait_wandering_prince", "wandering_prince_node1" },
            { "khuzait_khans_mercenary", "khans_mercenary_node1" },
            { "khuzait_slave_escape", "slave_escape_node1" },
            { "khuzait_free_cossack", "free_cossack_node1" },
            { "khuzait_old_guard_avenger", "old_guard_avenger_node1" },
        };

        /// <summary>
        /// 节点流转规则 (curMenuId, optionId) → nextMenuId
        /// 格式: "curMenuId:optionId" → "nextMenuId"
        /// </summary>
        private static readonly Dictionary<string, string> NodeTransitions = new Dictionary<string, string>()
        {
            // 草原叛酋
            { "rebel_chief_node1:rebel_reason_refuse_tribute", "rebel_chief_node2" },
            { "rebel_chief_node1:rebel_reason_raid_caravan", "rebel_chief_node2" },
            { "rebel_chief_node1:rebel_reason_refuse_service", "rebel_chief_node2" },
            { "rebel_chief_node1:rebel_reason_protect_tribe", "rebel_chief_node2" },
            { "rebel_chief_node2:rebel_warband_nomads", "rebel_chief_node3" },
            { "rebel_chief_node2:rebel_warband_light_cav", "rebel_chief_node3" },
            { "rebel_chief_node2:rebel_warband_mixed", "rebel_chief_node3" },
            { "rebel_chief_node3:rebel_symbol_banner", "rebel_chief_node4" },
            { "rebel_chief_node3:rebel_symbol_treasure", "rebel_chief_node4" },
            { "rebel_chief_node3:rebel_symbol_seal", "rebel_chief_node4" },
            { "rebel_chief_node3:rebel_symbol_people", "rebel_chief_node4" },
            // 迁徙王族
            { "wandering_prince_node1:royal_exile_usurped", "wandering_prince_subnode" },
            { "wandering_prince_node1:royal_exile_clan_war", "wandering_prince_subnode" },
            { "wandering_prince_node1:royal_exile_foresight", "wandering_prince_subnode" },
            { "wandering_prince_node1:royal_exile_treasure", "wandering_prince_subnode" },
            { "wandering_prince_subnode:wandering_prince_ochilan_tribes", "wandering_prince_node3" },
            { "wandering_prince_subnode:wandering_prince_dashi_yuting", "wandering_prince_node3" },
            { "wandering_prince_subnode:wandering_prince_kashan_wu_tribes", "wandering_prince_node3" },
            { "wandering_prince_subnode:wandering_prince_aigeli_military_tent", "wandering_prince_node3" },
            { "wandering_prince_subnode:wandering_prince_tahun_remnants", "wandering_prince_node3" },
            { "wandering_prince_node3:royal_strategy_infiltrate", "wandering_prince_node4" },
            { "wandering_prince_node3:royal_strategy_new_kingdom", "wandering_prince_node4" },
            { "wandering_prince_node3:royal_strategy_ally", "wandering_prince_node4" },
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

