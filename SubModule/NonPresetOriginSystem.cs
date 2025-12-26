using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace OriginSystemMod
{
    /// <summary>
    /// 非预设出身系统
    /// 实现非预设出身的应用逻辑（不绑定派系、不通缉、不强剧情身份）
    /// </summary>
    public static class NonPresetOriginSystem
    {
        /// <summary>
        /// 应用非预设出身效果
        /// </summary>
        public static void ApplyNonPresetOrigin()
        {
            try
            {
                var hero = Hero.MainHero;
                if (hero == null)
                {
                    Debug.Print("[OriginSystem] MainHero 为空，无法应用非预设出身", 0, Debug.DebugColor.Red);
                    return;
                }

                var clan = hero.Clan;
                if (clan == null)
                {
                    Debug.Print("[OriginSystem] PlayerClan 为空，无法应用非预设出身", 0, Debug.DebugColor.Red);
                    return;
                }

                var party = MobileParty.MainParty;
                if (party == null)
                {
                    Debug.Print("[OriginSystem] MainParty 为空，无法应用非预设出身", 0, Debug.DebugColor.Red);
                    return;
                }

                Debug.Print("[OriginSystem] 开始应用非预设出身", 0, Debug.DebugColor.Green);

                var selectedNodes = OriginSystemHelper.SelectedNonPresetOriginNodes ?? new Dictionary<string, string>();

                // 1. 社会出身（必需）
                ApplySocialOrigin(hero, party, selectedNodes);

                // 2. 技能背景（必需）
                ApplySkillBackground(hero, party, selectedNodes);

                // 3. 当前状态（必需）
                ApplyStartingCondition(hero, party, selectedNodes);

                // 4. 可选节点
                ApplyLocationType(hero, party, selectedNodes);
                ApplyValuableItem(hero, party, selectedNodes);
                ApplyContactType(hero, selectedNodes);

                // 5. 私有节点
                ApplyPrivateNodes(hero, party, selectedNodes);

                Debug.Print("[OriginSystem] 非预设出身应用完成", 0, Debug.DebugColor.Green);
            }
            catch (Exception ex)
            {
                Debug.Print($"[OriginSystem] 应用非预设出身失败: {ex.Message}", 0, Debug.DebugColor.Red);
                Debug.Print($"[OriginSystem] StackTrace: {ex.StackTrace}", 0, Debug.DebugColor.Red);
            }
        }

        #region 公共节点

        /// <summary>
        /// 应用社会出身效果
        /// </summary>
        private static void ApplySocialOrigin(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("np_node_social_origin")) return;

            var origin = nodes["np_node_social_origin"];
            switch (origin)
            {
                case "social_origin_rural_peasant":
                    GiveGoldAction.ApplyBetweenCharacters(null, hero, 60, false);
                    AddFood(party, 8);
                    AddSkill(hero, "athletics", 2);
                    AddSkill(hero, "steward", 1);
                    break;
                case "social_origin_nomad_camp":
                    GiveGoldAction.ApplyBetweenCharacters(null, hero, 40, false);
                    AddFood(party, 6);
                    AddSkill(hero, "riding", 2);
                    AddSkill(hero, "scouting", 2);
                    break;
                case "social_origin_urban_poor":
                    GiveGoldAction.ApplyBetweenCharacters(null, hero, 20, false);
                    AddFood(party, 2);
                    AddSkill(hero, "roguery", 2);
                    AddSkill(hero, "charm", 1);
                    break;
                case "social_origin_urban_artisan":
                    GiveGoldAction.ApplyBetweenCharacters(null, hero, 120, false);
                    AddFood(party, 4);
                    AddSkill(hero, "steward", 2);
                    AddSkill(hero, "trade", 1);
                    break;
                case "social_origin_caravan_follower":
                    GiveGoldAction.ApplyBetweenCharacters(null, hero, 150, false);
                    AddFood(party, 6);
                    AddSkill(hero, "trade", 2);
                    AddSkill(hero, "scouting", 1);
                    break;
                case "social_origin_minor_clan":
                    GiveGoldAction.ApplyBetweenCharacters(null, hero, 140, false);
                    AddFood(party, 4);
                    AddSkill(hero, "charm", 2);
                    AddSkill(hero, "leadership", 1);
                    break;
                case "social_origin_wanderer":
                    GiveGoldAction.ApplyBetweenCharacters(null, hero, 10, false);
                    AddFood(party, 1);
                    AddSkill(hero, "scouting", 2);
                    AddSkill(hero, "athletics", 2);
                    break;
            }
        }

        /// <summary>
        /// 应用技能背景效果
        /// </summary>
        private static void ApplySkillBackground(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("np_node_skill_background")) return;

            var background = nodes["np_node_skill_background"];
            switch (background)
            {
                case "bg_herd_hunt":
                    AddSkill(hero, "bow", 3);
                    AddSkill(hero, "scouting", 2);
                    AddSkill(hero, "athletics", 1);
                    break;
                case "bg_village_defense":
                    AddSkill(hero, "onehand", 2);
                    AddSkill(hero, "athletics", 2);
                    AddSkill(hero, "leadership", 1);
                    break;
                case "bg_caravan_guard":
                    AddSkill(hero, "riding", 2);
                    AddSkill(hero, "tactics", 2);
                    AddSkill(hero, "onehand", 1);
                    break;
                case "bg_merc_helper":
                    AddSkill(hero, "athletics", 2);
                    AddSkill(hero, "onehand", 1);
                    AddSkill(hero, "tactics", 1);
                    break;
                case "bg_border_patrol":
                    AddSkill(hero, "scouting", 3);
                    AddSkill(hero, "polearm", 1);
                    AddSkill(hero, "bow", 1);
                    break;
                case "bg_city_apprentice":
                    AddSkill(hero, "steward", 2);
                    AddSkill(hero, "trade", 1);
                    AddSkill(hero, "charm", 1);
                    break;
                case "bg_poach_smuggle":
                    AddSkill(hero, "roguery", 3);
                    AddSkill(hero, "scouting", 2);
                    AddSkill(hero, "bow", 1);
                    break;
                case "bg_conscripted":
                    AddSkill(hero, "onehand", 1);
                    AddSkill(hero, "bow", 1);
                    AddSkill(hero, "polearm", 1);
                    AddSkill(hero, "athletics", 1);
                    break;
            }
        }

        /// <summary>
        /// 应用当前状态效果
        /// </summary>
        private static void ApplyStartingCondition(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("np_node_starting_condition")) return;

            var condition = nodes["np_node_starting_condition"];
            switch (condition)
            {
                case "sc_alone":
                    GiveGoldAction.ApplyBetweenCharacters(null, hero, 120, false);
                    AddFood(party, 2);
                    break;
                case "sc_with_friends":
                    GiveGoldAction.ApplyBetweenCharacters(null, hero, 60, false);
                    AddFood(party, 4);
                    AddTroops(party, "commoner", 2);
                    if (party != null) party.RecentEventsMorale += 2;
                    break;
                case "sc_with_commoners":
                    GiveGoldAction.ApplyBetweenCharacters(null, hero, 20, false);
                    AddFood(party, 8);
                    AddTroops(party, "commoner", 4);
                    break;
                case "sc_cash_start":
                    GiveGoldAction.ApplyBetweenCharacters(null, hero, 260, false);
                    break;
                case "sc_in_debt":
                    GiveGoldAction.ApplyBetweenCharacters(null, hero, 320, false);
                    AddFood(party, 4);
                    break;
                case "sc_injured_or_tired":
                    GiveGoldAction.ApplyBetweenCharacters(null, hero, 150, false);
                    AddFood(party, 4);
                    break;
            }
        }

        #endregion

        #region 可选公共节点

        /// <summary>
        /// 应用起始位置类型效果
        /// </summary>
        private static void ApplyLocationType(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("np_node_start_location_type")) return;

            var location = nodes["np_node_start_location_type"];
            switch (location)
            {
                case "loc_rural_edge":
                    AddFood(party, 4);
                    GiveGoldAction.ApplyBetweenCharacters(hero, null, 20, false);
                    AddSkill(hero, "steward", 1);
                    break;
                case "loc_town_outskirts":
                    GiveGoldAction.ApplyBetweenCharacters(null, hero, 40, false);
                    AddFood(party, -2);
                    AddSkill(hero, "trade", 1);
                    break;
                case "loc_trade_road":
                    GiveGoldAction.ApplyBetweenCharacters(null, hero, 20, false);
                    AddSkill(hero, "scouting", 1);
                    break;
                case "loc_border_wild":
                    AddSkill(hero, "scouting", 2);
                    AddSkill(hero, "athletics", 1);
                    break;
            }
        }

        /// <summary>
        /// 应用贵重物品效果
        /// </summary>
        private static void ApplyValuableItem(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("np_node_valuable_item")) return;

            var item = nodes["np_node_valuable_item"];
            switch (item)
            {
                case "val_good_knife":
                    AddSkill(hero, "onehand", 1);
                    break;
                case "val_warm_coat":
                    if (party != null) party.RecentEventsMorale += 1;
                    AddSkill(hero, "athletics", 1);
                    break;
                case "val_pack_mule":
                    AddMounts(party, 1);
                    GiveGoldAction.ApplyBetweenCharacters(null, hero, 20, false);
                    break;
                case "val_small_savings":
                    GiveGoldAction.ApplyBetweenCharacters(null, hero, 120, false);
                    break;
            }
        }

        /// <summary>
        /// 应用联系人类型效果
        /// </summary>
        private static void ApplyContactType(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("np_node_contact_type")) return;

            var contact = nodes["np_node_contact_type"];
            switch (contact)
            {
                case "ct_caravan_contact":
                    AddSkill(hero, "trade", 1);
                    break;
                case "ct_workshop_contact":
                    AddSkill(hero, "steward", 1);
                    AddSkill(hero, "charm", 1);
                    break;
                case "ct_officer_contact":
                    AddSkill(hero, "leadership", 1);
                    AddSkill(hero, "tactics", 1);
                    break;
                case "ct_none":
                    GiveGoldAction.ApplyBetweenCharacters(null, hero, 40, false);
                    break;
            }
        }

        #endregion

        #region 私有节点

        /// <summary>
        /// 应用私有节点效果
        /// </summary>
        private static void ApplyPrivateNodes(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            // 检查是否选择了游牧营地
            if (nodes.ContainsKey("np_node_social_origin") && 
                nodes["np_node_social_origin"] == "social_origin_nomad_camp" &&
                nodes.ContainsKey("np_node_nomad_role"))
            {
                ApplyNomadRole(hero, party, nodes);
            }

            // 检查是否选择了城镇手工业者
            if (nodes.ContainsKey("np_node_social_origin") && 
                nodes["np_node_social_origin"] == "social_origin_urban_artisan" &&
                nodes.ContainsKey("np_node_artisan_trade"))
            {
                ApplyArtisanTrade(hero, party, nodes);
            }

            // 检查是否选择了商队随行者
            if (nodes.ContainsKey("np_node_social_origin") && 
                nodes["np_node_social_origin"] == "social_origin_caravan_follower" &&
                nodes.ContainsKey("np_node_caravan_duty"))
            {
                ApplyCaravanDuty(hero, party, nodes);
            }

            // 检查是否选择了小氏族旁支
            if (nodes.ContainsKey("np_node_social_origin") && 
                nodes["np_node_social_origin"] == "social_origin_minor_clan" &&
                nodes.ContainsKey("np_node_minor_clan_position"))
            {
                ApplyMinorClanPosition(hero, nodes);
            }

            // 检查是否选择了流浪难民
            if (nodes.ContainsKey("np_node_social_origin") && 
                nodes["np_node_social_origin"] == "social_origin_wanderer" &&
                nodes.ContainsKey("np_node_refugee_burden"))
            {
                ApplyRefugeeBurden(hero, party, nodes);
            }
        }

        private static void ApplyNomadRole(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            var role = nodes["np_node_nomad_role"];
            switch (role)
            {
                case "nomad_role_scout":
                    AddSkill(hero, "scouting", 2);
                    break;
                case "nomad_role_vet":
                    AddSkill(hero, "steward", 2);
                    AddSkill(hero, "trade", 1);
                    break;
                case "nomad_role_horse_handler":
                    AddSkill(hero, "riding", 2);
                    AddSkill(hero, "scouting", 1);
                    break;
                case "nomad_role_guard":
                    AddSkill(hero, "polearm", 1);
                    AddSkill(hero, "onehand", 1);
                    AddSkill(hero, "athletics", 1);
                    break;
            }
        }

        private static void ApplyArtisanTrade(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            var trade = nodes["np_node_artisan_trade"];
            switch (trade)
            {
                case "artisan_trade_smith":
                    AddSkill(hero, "steward", 1);
                    AddSkill(hero, "athletics", 1);
                    break;
                case "artisan_trade_leather":
                    AddSkill(hero, "steward", 2);
                    AddSkill(hero, "trade", 1);
                    break;
                case "artisan_trade_carpenter":
                    AddSkill(hero, "steward", 2);
                    AddSkill(hero, "athletics", 1);
                    break;
                case "artisan_trade_clerk":
                    AddSkill(hero, "trade", 1);
                    AddSkill(hero, "charm", 2);
                    break;
            }
        }

        private static void ApplyCaravanDuty(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            var duty = nodes["np_node_caravan_duty"];
            switch (duty)
            {
                case "caravan_duty_guide":
                    AddSkill(hero, "scouting", 2);
                    AddSkill(hero, "trade", 1);
                    break;
                case "caravan_duty_guard":
                    AddSkill(hero, "tactics", 1);
                    AddSkill(hero, "onehand", 1);
                    AddSkill(hero, "athletics", 1);
                    break;
                case "caravan_duty_muleteer":
                    AddSkill(hero, "steward", 2);
                    AddSkill(hero, "trade", 1);
                    AddFood(party, 2);
                    break;
                case "caravan_duty_broker":
                    AddSkill(hero, "charm", 2);
                    AddSkill(hero, "trade", 1);
                    break;
            }
        }

        private static void ApplyMinorClanPosition(Hero hero, Dictionary<string, string> nodes)
        {
            var position = nodes["np_node_minor_clan_position"];
            switch (position)
            {
                case "clan_pos_hostage":
                    AddSkill(hero, "charm", 2);
                    AddSkill(hero, "steward", 1);
                    break;
                case "clan_pos_client":
                    AddSkill(hero, "leadership", 1);
                    AddSkill(hero, "charm", 1);
                    break;
                case "clan_pos_bastard":
                    AddSkill(hero, "athletics", 1);
                    AddSkill(hero, "roguery", 1);
                    AddSkill(hero, "charm", 1);
                    break;
                case "clan_pos_ignored":
                    AddSkill(hero, "scouting", 1);
                    AddSkill(hero, "trade", 1);
                    GiveGoldAction.ApplyBetweenCharacters(null, hero, 40, false);
                    break;
            }
        }

        private static void ApplyRefugeeBurden(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            var burden = nodes["np_node_refugee_burden"];
            switch (burden)
            {
                case "ref_burden_sick":
                    AddFood(party, 2);
                    AddSkill(hero, "steward", 1);
                    break;
                case "ref_burden_child":
                    AddFood(party, 4);
                    if (party != null) party.RecentEventsMorale += 1;
                    break;
                case "ref_burden_owed":
                    GiveGoldAction.ApplyBetweenCharacters(null, hero, 120, false);
                    break;
                case "ref_burden_nothing":
                    AddSkill(hero, "athletics", 1);
                    AddSkill(hero, "scouting", 1);
                    break;
            }
        }

        #endregion

        #region 通用辅助方法

        private static void AddSkill(Hero hero, string skillName, int points)
        {
            if (hero == null || hero.HeroDeveloper == null) return;
            
            try
            {
                var skillIdMap = new Dictionary<string, string>
                {
                    { "onehand", "OneHanded" },
                    { "twohand", "TwoHanded" },
                    { "polearm", "Polearm" },
                    { "bow", "Bow" },
                    { "crossbow", "Crossbow" },
                    { "throwing", "Throwing" },
                    { "riding", "Riding" },
                    { "athletics", "Athletics" },
                    { "crafting", "Crafting" },
                    { "tactics", "Tactics" },
                    { "scouting", "Scouting" },
                    { "roguery", "Roguery" },
                    { "leadership", "Leadership" },
                    { "charm", "Charm" },
                    { "trade", "Trade" },
                    { "steward", "Steward" },
                    { "medicine", "Medicine" },
                    { "engineering", "Engineering" }
                };

                var skillId = skillIdMap.ContainsKey(skillName.ToLower()) ? skillIdMap[skillName.ToLower()] : skillName;
                var skill = MBObjectManager.Instance.GetObject<SkillObject>(skillId);
                if (skill != null)
                {
                    hero.HeroDeveloper.AddSkillXp(skill, points * 100, true, true);
                    Debug.Print($"[OriginSystem] 添加技能: {skillName} ({skillId}) +{points}", 0, Debug.DebugColor.Green);
                }
            }
            catch (Exception ex)
            {
                Debug.Print($"[OriginSystem] 添加技能失败 ({skillName}): {ex.Message}", 0, Debug.DebugColor.Red);
            }
        }

        private static void AddTroops(MobileParty party, string troopId, int count)
        {
            if (party == null) return;
            
            try
            {
                var troop = CharacterObject.All.FirstOrDefault(c => c.StringId == troopId || c.StringId.Contains("commoner") || c.StringId.Contains("villager"));
                if (troop != null)
                {
                    party.AddElementToMemberRoster(troop, count);
                    Debug.Print($"[OriginSystem] 添加兵力: {troopId} x{count}", 0, Debug.DebugColor.Green);
                }
            }
            catch (Exception ex)
            {
                Debug.Print($"[OriginSystem] 添加兵力失败 ({troopId}): {ex.Message}", 0, Debug.DebugColor.Red);
            }
        }

        private static void AddFood(MobileParty party, int amount)
        {
            if (party == null) return;
            
            try
            {
                var foodItem = MBObjectManager.Instance.GetObject<ItemObject>("grain");
                if (foodItem != null)
                {
                    party.ItemRoster.AddToCounts(foodItem, amount);
                    Debug.Print($"[OriginSystem] 添加食物: +{amount}", 0, Debug.DebugColor.Green);
                }
            }
            catch (Exception ex)
            {
                Debug.Print($"[OriginSystem] 添加食物失败: {ex.Message}", 0, Debug.DebugColor.Yellow);
            }
        }

        private static void AddMounts(MobileParty party, int count)
        {
            if (party == null) return;
            
            try
            {
                var mountItem = MBObjectManager.Instance.GetObject<ItemObject>("sumpter_horse");
                if (mountItem != null)
                {
                    party.ItemRoster.AddToCounts(mountItem, count);
                    Debug.Print($"[OriginSystem] 添加马匹: +{count}", 0, Debug.DebugColor.Green);
                }
            }
            catch (Exception ex)
            {
                Debug.Print($"[OriginSystem] 添加马匹失败: {ex.Message}", 0, Debug.DebugColor.Yellow);
            }
        }

        #endregion
    }
}
