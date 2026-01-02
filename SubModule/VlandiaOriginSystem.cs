using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace OriginSystemMod
{
    /// <summary>
    /// 瓦兰迪亚预设出身应用逻辑
    /// </summary>
    public static class VlandiaOriginSystem
    {
        /// <summary>
        /// 应用远征的骑士出身
        /// </summary>
        public static void ApplyExpeditionKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 开始应用远征的骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 3);
                PresetOriginSystem.GainRenown(hero, 30);
                PresetOriginSystem.AddGold(hero, 3500);

                // 设置初始瓦兰迪亚风格装备
                SetVlandiaInitialEquipment(hero, "expedition_knight");

                // 设置出生位置（根据Node2的誓言选择决定）
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                if (selectedNodes.ContainsKey("vla_node_expedition_oath"))
                {
                    string oath = selectedNodes["vla_node_expedition_oath"];
                    string startLocation = GetExpeditionKnightStartLocation(oath);
                    if (!string.IsNullOrEmpty(startLocation))
                    {
                        PresetOriginSystem.SetPresetOriginStartingLocation(party, startLocation);
                    }
                }
                else
                {
                    PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");
                }

                // 应用节点效果
                ApplyExpeditionKnightNode1(hero, selectedNodes);
                ApplyExpeditionKnightNode2(hero, clan, party, selectedNodes);
                ApplyExpeditionKnightNode3(hero, selectedNodes);
                ApplyExpeditionKnightNode4(hero, party, selectedNodes);

                // 创建哥哥NPC（固定存在）
                CreateExpeditionKnightBrother(hero, clan);

                OriginLog.Info("[ExpeditionKnight] 远征的骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[ExpeditionKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 根据誓言选择获取出生位置
        /// </summary>
        private static string GetExpeditionKnightStartLocation(string oath)
        {
            switch (oath)
            {
                case "kill_sea_raiders": // 杀1000海寇
                    return "nord"; // 诺德村子
                case "conquer_gyaz": // 远征古亚兹
                    return "aserai"; // 阿塞莱村子（靠近古亚兹）
                case "kill_battanian_lord": // 斩巴丹尼亚氏族
                    return "battania"; // 巴丹尼亚村子（肖农附近）
                case "recover_banner": // 寻回失旗
                    return "vlandia"; // 默认瓦兰迪亚
                default:
                    return "vlandia";
            }
        }

        /// <summary>
        /// 应用远征骑士Node1效果
        /// </summary>
        private static void ApplyExpeditionKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_fall")) return;

            string fall = nodes["vla_node_expedition_fall"];
            switch (fall)
            {
                case "erased_defeat":
                    PresetOriginSystem.AddSkill(hero, "tactics", 30);
                    PresetOriginSystem.AddSkill(hero, "leadership", 20);
                    ApplyRandomNobleRelation(hero, -30);
                    break;
                case "annexed":
                    PresetOriginSystem.AddSkill(hero, "charm", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "debt":
                    PresetOriginSystem.AddSkill(hero, "trade", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 2500);
                    break;
                case "political":
                    PresetOriginSystem.AddSkill(hero, "scouting", 20);
                    PresetOriginSystem.AddSkill(hero, "tactics", 20);
                    ApplyRoyalFactionRelation(hero, -30);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node2效果（誓言任务）
        /// </summary>
        private static void ApplyExpeditionKnightNode2(Hero hero, Clan clan, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_oath")) return;

            string oath = nodes["vla_node_expedition_oath"];
            
            // 创建誓言任务
            CreateExpeditionKnightOathQuest(hero, oath);
        }

        /// <summary>
        /// 应用远征骑士Node3效果
        /// </summary>
        private static void ApplyExpeditionKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_chivalry")) return;

            string chivalry = nodes["vla_node_expedition_chivalry"];
            switch (chivalry)
            {
                case "mercy": // 仁慈之誓
                    PresetOriginSystem.AddGold(hero, -2000);
                    break;
                case "valor": // 勇武之誓
                    // 开局多2-4精锐随骑（在Node4中处理）
                    break;
                case "prudence": // 谨慎之誓
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "cynical": // 犬儒之誓
                    PresetOriginSystem.AddGold(hero, 3000);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node4效果
        /// </summary>
        private static void ApplyExpeditionKnightNode4(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_division")) return;

            string division = nodes["vla_node_expedition_division"];
            switch (division)
            {
                case "brother_commands":
                    // 哥哥掌军，你主外交：哥哥偏战术/统御，你偏魅力/管理；随从偏步弩
                    break;
                case "you_command":
                    // 你掌军，哥哥做见证人：你拿骑兵核心，哥哥提供稳定后勤
                    break;
                case "joint_decision":
                    // 共同决策：队伍更均衡
                    break;
            }
        }

        /// <summary>
        /// 创建远征骑士的哥哥NPC
        /// </summary>
        private static void CreateExpeditionKnightBrother(Hero hero, Clan clan)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 需要创建哥哥NPC（待实现）");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建哥哥NPC失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 创建远征骑士的誓言任务
        /// </summary>
        private static void CreateExpeditionKnightOathQuest(Hero hero, string oath)
        {
            try
            {
                OriginLog.Info($"[ExpeditionKnight] 创建誓言任务: {oath}");
                // 任务系统需要单独实现，这里先记录
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建任务失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用堕落无赖骑士出身
        /// </summary>
        public static void ApplyDegradedRogueKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[DegradedRogueKnight] 开始应用堕落无赖骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 2);
                PresetOriginSystem.GainRenown(hero, 10);
                PresetOriginSystem.AddGold(hero, 4000);

                // 设置初始瓦兰迪亚风格装备（败落骑士风格）
                SetVlandiaInitialEquipment(hero, "degraded_rogue_knight");

                // 设置敌人关系
                ApplyDegradedRogueKnightEnemyRelations(hero);

                // 设置犯罪度（罪犯标记）
                ApplyDegradedRogueKnightCrimeStatus(hero);

                // 设置出生位置（瓦兰迪亚附近，但作为罪犯）
                PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");

                // 应用节点效果
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                ApplyDegradedRogueKnightNode1(hero, selectedNodes);
                ApplyDegradedRogueKnightNode2(hero, selectedNodes);
                ApplyDegradedRogueKnightNode3(hero, selectedNodes);
                ApplyDegradedRogueKnightNode4(hero, selectedNodes);

                OriginLog.Info("[DegradedRogueKnight] 堕落无赖骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[DegradedRogueKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的敌人关系
        /// </summary>
        private static void ApplyDegradedRogueKnightEnemyRelations(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                var empireKingdom = FindKingdom("kingdom_empire");
                var battaniaKingdom = FindKingdom("kingdom_battania");

                // 与瓦兰迪亚敌对
                if (vlandiaKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, vlandiaKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, vlandiaKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与瓦兰迪亚宣战");
                    }

                    foreach (var lord in vlandiaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与帝国敌对
                if (empireKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, empireKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, empireKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与帝国宣战");
                    }

                    foreach (var lord in empireKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与巴丹尼亚贵族关系-80（但不一定开战）
                if (battaniaKingdom != null)
                {
                    foreach (var lord in battaniaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                OriginLog.Info("[DegradedRogueKnight] 敌人关系设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置敌人关系失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的罪犯状态
        /// </summary>
        private static void ApplyDegradedRogueKnightCrimeStatus(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(vlandiaKingdom, 100);
                }

                var empireKingdom = FindKingdom("kingdom_empire");
                if (empireKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(empireKingdom, 100);
                }

                OriginLog.Info("[DegradedRogueKnight] 罪犯状态设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置罪犯状态失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士Node1效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_crime")) return;

            string crime = nodes["vla_node_degraded_crime"];
            switch (crime)
            {
                case "tyranny": // 暴虐之罪
                    ApplyVillageRelations(hero, -20);
                    break;
                case "indulgence": // 沉溺之罪
                    PresetOriginSystem.AddGold(hero, 3500);
                    break;
                case "filth": // 污秽之罪
                    break;
                case "conspiracy": // 阴谋之罪
                    break;
            }
        }

        /// <summary>
        /// 应用无赖骑士Node2效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode2(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_first_case")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node3效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_view")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node4效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode4(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_goal")) return;
        }

        /// <summary>
        /// 设置瓦兰迪亚初始装备
        /// </summary>
        private static void SetVlandiaInitialEquipment(Hero hero, string originType)
        {
            try
            {
                if (hero == null || hero.BattleEquipment == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] Hero或BattleEquipment为空");
                    return;
                }

                var allItems = MBObjectManager.Instance?.GetObjectTypeList<ItemObject>();
                if (allItems == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] 无法获取物品列表");
                    return;
                }

                if (originType == "expedition_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else if (originType == "degraded_rogue_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, false);
                }

                OriginLog.Info($"[SetVlandiaInitialEquipment] {originType} 装备设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[SetVlandiaInitialEquipment] 设置装备失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据档位设置瓦兰迪亚装备
        /// </summary>
        private static void SetVlandiaEquipmentByTier(Hero hero, IEnumerable<ItemObject> allItems, int tier, bool damaged)
        {
            var vlandiaItems = allItems.Where(item =>
                (item.Culture != null && item.Culture.StringId == "vlandia") ||
                item.StringId.Contains("vlandia") ||
                item.StringId.Contains("vlandian"))
                .ToList();

            // 身体护甲
            if (hero.BattleEquipment[EquipmentIndex.Body].IsEmpty)
            {
                var bodyArmor = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.BodyArmor &&
                    (tier == 2 ? item.StringId.Contains("t2") : tier == 3 ? item.StringId.Contains("t3") : item.StringId.Contains("t4")));
                if (bodyArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Body] = new EquipmentElement(bodyArmor);
                }
            }

            // 腿部护甲
            if (hero.BattleEquipment[EquipmentIndex.Leg].IsEmpty)
            {
                var legArmor = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.LegArmor);
                if (legArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Leg] = new EquipmentElement(legArmor);
                }
            }

            // 武器：长剑+盾
            if (hero.BattleEquipment[EquipmentIndex.Weapon0].IsEmpty)
            {
                var sword = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.OneHandedWeapon &&
                    (item.StringId.Contains("sword") || item.StringId.Contains("blade")));
                if (sword != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon0] = new EquipmentElement(sword);
                }
            }

            // 盾牌
            if (hero.BattleEquipment[EquipmentIndex.Weapon1].IsEmpty)
            {
                var shield = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.Shield);
                if (shield != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon1] = new EquipmentElement(shield);
                }
            }

            // 骑枪
            if (hero.BattleEquipment[EquipmentIndex.Weapon2].IsEmpty)
            {
                var lance = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.Polearm &&
                    (item.StringId.Contains("lance") || item.StringId.Contains("spear")));
                if (lance != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon2] = new EquipmentElement(lance);
                }
            }
        }

        /// <summary>
        /// 应用随机贵族关系
        /// </summary>
        private static void ApplyRandomNobleRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    var lords = vlandiaKingdom.Lords.Where(h => h.IsLord && h != hero).ToList();
                    if (lords.Count > 0)
                    {
                        var randomLord = lords[new Random().Next(lords.Count)];
                        ChangeRelationAction.ApplyPlayerRelation(randomLord, relationChange);
                        OriginLog.Info($"[ApplyRandomNobleRelation] 与 {randomLord.Name} 关系 {relationChange}");
                    }
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRandomNobleRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用王室派系关系
        /// </summary>
        private static void ApplyRoyalFactionRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null && vlandiaKingdom.Leader != null)
                {
                    ChangeRelationAction.ApplyPlayerRelation(vlandiaKingdom.Leader, relationChange);
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRoyalFactionRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用村庄关系
        /// </summary>
        private static void ApplyVillageRelations(Hero hero, int relationChange)
        {
            try
            {
                OriginLog.Info($"[ApplyVillageRelations] 村庄关系变化: {relationChange}");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyVillageRelations] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 查找王国
        /// </summary>
        private static Kingdom FindKingdom(string kingdomId)
        {
            try
            {
                return Campaign.Current?.Kingdoms?.FirstOrDefault(k =>
                    k.StringId == kingdomId ||
                    k.StringId == $"kingdom_{kingdomId}" ||
                    (k.Culture != null && k.Culture.StringId == kingdomId));
            }
            catch
            {
                return null;
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace OriginSystemMod
{
    /// <summary>
    /// 瓦兰迪亚预设出身应用逻辑
    /// </summary>
    public static class VlandiaOriginSystem
    {
        /// <summary>
        /// 应用远征的骑士出身
        /// </summary>
        public static void ApplyExpeditionKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 开始应用远征的骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 3);
                PresetOriginSystem.GainRenown(hero, 30);
                PresetOriginSystem.AddGold(hero, 3500);

                // 设置初始瓦兰迪亚风格装备
                SetVlandiaInitialEquipment(hero, "expedition_knight");

                // 设置出生位置（根据Node2的誓言选择决定）
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                if (selectedNodes.ContainsKey("vla_node_expedition_oath"))
                {
                    string oath = selectedNodes["vla_node_expedition_oath"];
                    string startLocation = GetExpeditionKnightStartLocation(oath);
                    if (!string.IsNullOrEmpty(startLocation))
                    {
                        PresetOriginSystem.SetPresetOriginStartingLocation(party, startLocation);
                    }
                }
                else
                {
                    PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");
                }

                // 应用节点效果
                ApplyExpeditionKnightNode1(hero, selectedNodes);
                ApplyExpeditionKnightNode2(hero, clan, party, selectedNodes);
                ApplyExpeditionKnightNode3(hero, selectedNodes);
                ApplyExpeditionKnightNode4(hero, party, selectedNodes);

                // 创建哥哥NPC（固定存在）
                CreateExpeditionKnightBrother(hero, clan);

                OriginLog.Info("[ExpeditionKnight] 远征的骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[ExpeditionKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 根据誓言选择获取出生位置
        /// </summary>
        private static string GetExpeditionKnightStartLocation(string oath)
        {
            switch (oath)
            {
                case "kill_sea_raiders": // 杀1000海寇
                    return "nord"; // 诺德村子
                case "conquer_gyaz": // 远征古亚兹
                    return "aserai"; // 阿塞莱村子（靠近古亚兹）
                case "kill_battanian_lord": // 斩巴丹尼亚氏族
                    return "battania"; // 巴丹尼亚村子（肖农附近）
                case "recover_banner": // 寻回失旗
                    return "vlandia"; // 默认瓦兰迪亚
                default:
                    return "vlandia";
            }
        }

        /// <summary>
        /// 应用远征骑士Node1效果
        /// </summary>
        private static void ApplyExpeditionKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_fall")) return;

            string fall = nodes["vla_node_expedition_fall"];
            switch (fall)
            {
                case "erased_defeat":
                    PresetOriginSystem.AddSkill(hero, "tactics", 30);
                    PresetOriginSystem.AddSkill(hero, "leadership", 20);
                    ApplyRandomNobleRelation(hero, -30);
                    break;
                case "annexed":
                    PresetOriginSystem.AddSkill(hero, "charm", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "debt":
                    PresetOriginSystem.AddSkill(hero, "trade", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 2500);
                    break;
                case "political":
                    PresetOriginSystem.AddSkill(hero, "scouting", 20);
                    PresetOriginSystem.AddSkill(hero, "tactics", 20);
                    ApplyRoyalFactionRelation(hero, -30);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node2效果（誓言任务）
        /// </summary>
        private static void ApplyExpeditionKnightNode2(Hero hero, Clan clan, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_oath")) return;

            string oath = nodes["vla_node_expedition_oath"];
            
            // 创建誓言任务
            CreateExpeditionKnightOathQuest(hero, oath);
        }

        /// <summary>
        /// 应用远征骑士Node3效果
        /// </summary>
        private static void ApplyExpeditionKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_chivalry")) return;

            string chivalry = nodes["vla_node_expedition_chivalry"];
            switch (chivalry)
            {
                case "mercy": // 仁慈之誓
                    PresetOriginSystem.AddGold(hero, -2000);
                    break;
                case "valor": // 勇武之誓
                    // 开局多2-4精锐随骑（在Node4中处理）
                    break;
                case "prudence": // 谨慎之誓
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "cynical": // 犬儒之誓
                    PresetOriginSystem.AddGold(hero, 3000);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node4效果
        /// </summary>
        private static void ApplyExpeditionKnightNode4(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_division")) return;

            string division = nodes["vla_node_expedition_division"];
            switch (division)
            {
                case "brother_commands":
                    // 哥哥掌军，你主外交：哥哥偏战术/统御，你偏魅力/管理；随从偏步弩
                    break;
                case "you_command":
                    // 你掌军，哥哥做见证人：你拿骑兵核心，哥哥提供稳定后勤
                    break;
                case "joint_decision":
                    // 共同决策：队伍更均衡
                    break;
            }
        }

        /// <summary>
        /// 创建远征骑士的哥哥NPC
        /// </summary>
        private static void CreateExpeditionKnightBrother(Hero hero, Clan clan)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 需要创建哥哥NPC（待实现）");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建哥哥NPC失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 创建远征骑士的誓言任务
        /// </summary>
        private static void CreateExpeditionKnightOathQuest(Hero hero, string oath)
        {
            try
            {
                OriginLog.Info($"[ExpeditionKnight] 创建誓言任务: {oath}");
                // 任务系统需要单独实现，这里先记录
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建任务失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用堕落无赖骑士出身
        /// </summary>
        public static void ApplyDegradedRogueKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[DegradedRogueKnight] 开始应用堕落无赖骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 2);
                PresetOriginSystem.GainRenown(hero, 10);
                PresetOriginSystem.AddGold(hero, 4000);

                // 设置初始瓦兰迪亚风格装备（败落骑士风格）
                SetVlandiaInitialEquipment(hero, "degraded_rogue_knight");

                // 设置敌人关系
                ApplyDegradedRogueKnightEnemyRelations(hero);

                // 设置犯罪度（罪犯标记）
                ApplyDegradedRogueKnightCrimeStatus(hero);

                // 设置出生位置（瓦兰迪亚附近，但作为罪犯）
                PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");

                // 应用节点效果
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                ApplyDegradedRogueKnightNode1(hero, selectedNodes);
                ApplyDegradedRogueKnightNode2(hero, selectedNodes);
                ApplyDegradedRogueKnightNode3(hero, selectedNodes);
                ApplyDegradedRogueKnightNode4(hero, selectedNodes);

                OriginLog.Info("[DegradedRogueKnight] 堕落无赖骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[DegradedRogueKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的敌人关系
        /// </summary>
        private static void ApplyDegradedRogueKnightEnemyRelations(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                var empireKingdom = FindKingdom("kingdom_empire");
                var battaniaKingdom = FindKingdom("kingdom_battania");

                // 与瓦兰迪亚敌对
                if (vlandiaKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, vlandiaKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, vlandiaKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与瓦兰迪亚宣战");
                    }

                    foreach (var lord in vlandiaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与帝国敌对
                if (empireKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, empireKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, empireKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与帝国宣战");
                    }

                    foreach (var lord in empireKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与巴丹尼亚贵族关系-80（但不一定开战）
                if (battaniaKingdom != null)
                {
                    foreach (var lord in battaniaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                OriginLog.Info("[DegradedRogueKnight] 敌人关系设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置敌人关系失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的罪犯状态
        /// </summary>
        private static void ApplyDegradedRogueKnightCrimeStatus(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(vlandiaKingdom, 100);
                }

                var empireKingdom = FindKingdom("kingdom_empire");
                if (empireKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(empireKingdom, 100);
                }

                OriginLog.Info("[DegradedRogueKnight] 罪犯状态设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置罪犯状态失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士Node1效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_crime")) return;

            string crime = nodes["vla_node_degraded_crime"];
            switch (crime)
            {
                case "tyranny": // 暴虐之罪
                    ApplyVillageRelations(hero, -20);
                    break;
                case "indulgence": // 沉溺之罪
                    PresetOriginSystem.AddGold(hero, 3500);
                    break;
                case "filth": // 污秽之罪
                    break;
                case "conspiracy": // 阴谋之罪
                    break;
            }
        }

        /// <summary>
        /// 应用无赖骑士Node2效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode2(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_first_case")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node3效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_view")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node4效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode4(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_goal")) return;
        }

        /// <summary>
        /// 设置瓦兰迪亚初始装备
        /// </summary>
        private static void SetVlandiaInitialEquipment(Hero hero, string originType)
        {
            try
            {
                if (hero == null || hero.BattleEquipment == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] Hero或BattleEquipment为空");
                    return;
                }

                var allItems = MBObjectManager.Instance?.GetObjectTypeList<ItemObject>();
                if (allItems == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] 无法获取物品列表");
                    return;
                }

                if (originType == "expedition_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else if (originType == "degraded_rogue_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, false);
                }

                OriginLog.Info($"[SetVlandiaInitialEquipment] {originType} 装备设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[SetVlandiaInitialEquipment] 设置装备失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据档位设置瓦兰迪亚装备
        /// </summary>
        private static void SetVlandiaEquipmentByTier(Hero hero, IEnumerable<ItemObject> allItems, int tier, bool damaged)
        {
            var vlandiaItems = allItems.Where(item =>
                (item.Culture != null && item.Culture.StringId == "vlandia") ||
                item.StringId.Contains("vlandia") ||
                item.StringId.Contains("vlandian"))
                .ToList();

            // 身体护甲
            if (hero.BattleEquipment[EquipmentIndex.Body].IsEmpty)
            {
                var bodyArmor = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.BodyArmor &&
                    (tier == 2 ? item.StringId.Contains("t2") : tier == 3 ? item.StringId.Contains("t3") : item.StringId.Contains("t4")));
                if (bodyArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Body] = new EquipmentElement(bodyArmor);
                }
            }

            // 腿部护甲
            if (hero.BattleEquipment[EquipmentIndex.Leg].IsEmpty)
            {
                var legArmor = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.LegArmor);
                if (legArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Leg] = new EquipmentElement(legArmor);
                }
            }

            // 武器：长剑+盾
            if (hero.BattleEquipment[EquipmentIndex.Weapon0].IsEmpty)
            {
                var sword = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.OneHandedWeapon &&
                    (item.StringId.Contains("sword") || item.StringId.Contains("blade")));
                if (sword != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon0] = new EquipmentElement(sword);
                }
            }

            // 盾牌
            if (hero.BattleEquipment[EquipmentIndex.Weapon1].IsEmpty)
            {
                var shield = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.Shield);
                if (shield != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon1] = new EquipmentElement(shield);
                }
            }

            // 骑枪
            if (hero.BattleEquipment[EquipmentIndex.Weapon2].IsEmpty)
            {
                var lance = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.Polearm &&
                    (item.StringId.Contains("lance") || item.StringId.Contains("spear")));
                if (lance != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon2] = new EquipmentElement(lance);
                }
            }
        }

        /// <summary>
        /// 应用随机贵族关系
        /// </summary>
        private static void ApplyRandomNobleRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    var lords = vlandiaKingdom.Lords.Where(h => h.IsLord && h != hero).ToList();
                    if (lords.Count > 0)
                    {
                        var randomLord = lords[new Random().Next(lords.Count)];
                        ChangeRelationAction.ApplyPlayerRelation(randomLord, relationChange);
                        OriginLog.Info($"[ApplyRandomNobleRelation] 与 {randomLord.Name} 关系 {relationChange}");
                    }
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRandomNobleRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用王室派系关系
        /// </summary>
        private static void ApplyRoyalFactionRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null && vlandiaKingdom.Leader != null)
                {
                    ChangeRelationAction.ApplyPlayerRelation(vlandiaKingdom.Leader, relationChange);
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRoyalFactionRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用村庄关系
        /// </summary>
        private static void ApplyVillageRelations(Hero hero, int relationChange)
        {
            try
            {
                OriginLog.Info($"[ApplyVillageRelations] 村庄关系变化: {relationChange}");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyVillageRelations] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 查找王国
        /// </summary>
        private static Kingdom FindKingdom(string kingdomId)
        {
            try
            {
                return Campaign.Current?.Kingdoms?.FirstOrDefault(k =>
                    k.StringId == kingdomId ||
                    k.StringId == $"kingdom_{kingdomId}" ||
                    (k.Culture != null && k.Culture.StringId == kingdomId));
            }
            catch
            {
                return null;
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace OriginSystemMod
{
    /// <summary>
    /// 瓦兰迪亚预设出身应用逻辑
    /// </summary>
    public static class VlandiaOriginSystem
    {
        /// <summary>
        /// 应用远征的骑士出身
        /// </summary>
        public static void ApplyExpeditionKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 开始应用远征的骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 3);
                PresetOriginSystem.GainRenown(hero, 30);
                PresetOriginSystem.AddGold(hero, 3500);

                // 设置初始瓦兰迪亚风格装备
                SetVlandiaInitialEquipment(hero, "expedition_knight");

                // 设置出生位置（根据Node2的誓言选择决定）
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                if (selectedNodes.ContainsKey("vla_node_expedition_oath"))
                {
                    string oath = selectedNodes["vla_node_expedition_oath"];
                    string startLocation = GetExpeditionKnightStartLocation(oath);
                    if (!string.IsNullOrEmpty(startLocation))
                    {
                        PresetOriginSystem.SetPresetOriginStartingLocation(party, startLocation);
                    }
                }
                else
                {
                    PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");
                }

                // 应用节点效果
                ApplyExpeditionKnightNode1(hero, selectedNodes);
                ApplyExpeditionKnightNode2(hero, clan, party, selectedNodes);
                ApplyExpeditionKnightNode3(hero, selectedNodes);
                ApplyExpeditionKnightNode4(hero, party, selectedNodes);

                // 创建哥哥NPC（固定存在）
                CreateExpeditionKnightBrother(hero, clan);

                OriginLog.Info("[ExpeditionKnight] 远征的骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[ExpeditionKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 根据誓言选择获取出生位置
        /// </summary>
        private static string GetExpeditionKnightStartLocation(string oath)
        {
            switch (oath)
            {
                case "kill_sea_raiders": // 杀1000海寇
                    return "nord"; // 诺德村子
                case "conquer_gyaz": // 远征古亚兹
                    return "aserai"; // 阿塞莱村子（靠近古亚兹）
                case "kill_battanian_lord": // 斩巴丹尼亚氏族
                    return "battania"; // 巴丹尼亚村子（肖农附近）
                case "recover_banner": // 寻回失旗
                    return "vlandia"; // 默认瓦兰迪亚
                default:
                    return "vlandia";
            }
        }

        /// <summary>
        /// 应用远征骑士Node1效果
        /// </summary>
        private static void ApplyExpeditionKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_fall")) return;

            string fall = nodes["vla_node_expedition_fall"];
            switch (fall)
            {
                case "erased_defeat":
                    PresetOriginSystem.AddSkill(hero, "tactics", 30);
                    PresetOriginSystem.AddSkill(hero, "leadership", 20);
                    ApplyRandomNobleRelation(hero, -30);
                    break;
                case "annexed":
                    PresetOriginSystem.AddSkill(hero, "charm", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "debt":
                    PresetOriginSystem.AddSkill(hero, "trade", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 2500);
                    break;
                case "political":
                    PresetOriginSystem.AddSkill(hero, "scouting", 20);
                    PresetOriginSystem.AddSkill(hero, "tactics", 20);
                    ApplyRoyalFactionRelation(hero, -30);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node2效果（誓言任务）
        /// </summary>
        private static void ApplyExpeditionKnightNode2(Hero hero, Clan clan, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_oath")) return;

            string oath = nodes["vla_node_expedition_oath"];
            
            // 创建誓言任务
            CreateExpeditionKnightOathQuest(hero, oath);
        }

        /// <summary>
        /// 应用远征骑士Node3效果
        /// </summary>
        private static void ApplyExpeditionKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_chivalry")) return;

            string chivalry = nodes["vla_node_expedition_chivalry"];
            switch (chivalry)
            {
                case "mercy": // 仁慈之誓
                    PresetOriginSystem.AddGold(hero, -2000);
                    break;
                case "valor": // 勇武之誓
                    // 开局多2-4精锐随骑（在Node4中处理）
                    break;
                case "prudence": // 谨慎之誓
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "cynical": // 犬儒之誓
                    PresetOriginSystem.AddGold(hero, 3000);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node4效果
        /// </summary>
        private static void ApplyExpeditionKnightNode4(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_division")) return;

            string division = nodes["vla_node_expedition_division"];
            switch (division)
            {
                case "brother_commands":
                    // 哥哥掌军，你主外交：哥哥偏战术/统御，你偏魅力/管理；随从偏步弩
                    break;
                case "you_command":
                    // 你掌军，哥哥做见证人：你拿骑兵核心，哥哥提供稳定后勤
                    break;
                case "joint_decision":
                    // 共同决策：队伍更均衡
                    break;
            }
        }

        /// <summary>
        /// 创建远征骑士的哥哥NPC
        /// </summary>
        private static void CreateExpeditionKnightBrother(Hero hero, Clan clan)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 需要创建哥哥NPC（待实现）");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建哥哥NPC失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 创建远征骑士的誓言任务
        /// </summary>
        private static void CreateExpeditionKnightOathQuest(Hero hero, string oath)
        {
            try
            {
                OriginLog.Info($"[ExpeditionKnight] 创建誓言任务: {oath}");
                // 任务系统需要单独实现，这里先记录
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建任务失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用堕落无赖骑士出身
        /// </summary>
        public static void ApplyDegradedRogueKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[DegradedRogueKnight] 开始应用堕落无赖骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 2);
                PresetOriginSystem.GainRenown(hero, 10);
                PresetOriginSystem.AddGold(hero, 4000);

                // 设置初始瓦兰迪亚风格装备（败落骑士风格）
                SetVlandiaInitialEquipment(hero, "degraded_rogue_knight");

                // 设置敌人关系
                ApplyDegradedRogueKnightEnemyRelations(hero);

                // 设置犯罪度（罪犯标记）
                ApplyDegradedRogueKnightCrimeStatus(hero);

                // 设置出生位置（瓦兰迪亚附近，但作为罪犯）
                PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");

                // 应用节点效果
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                ApplyDegradedRogueKnightNode1(hero, selectedNodes);
                ApplyDegradedRogueKnightNode2(hero, selectedNodes);
                ApplyDegradedRogueKnightNode3(hero, selectedNodes);
                ApplyDegradedRogueKnightNode4(hero, selectedNodes);

                OriginLog.Info("[DegradedRogueKnight] 堕落无赖骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[DegradedRogueKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的敌人关系
        /// </summary>
        private static void ApplyDegradedRogueKnightEnemyRelations(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                var empireKingdom = FindKingdom("kingdom_empire");
                var battaniaKingdom = FindKingdom("kingdom_battania");

                // 与瓦兰迪亚敌对
                if (vlandiaKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, vlandiaKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, vlandiaKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与瓦兰迪亚宣战");
                    }

                    foreach (var lord in vlandiaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与帝国敌对
                if (empireKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, empireKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, empireKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与帝国宣战");
                    }

                    foreach (var lord in empireKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与巴丹尼亚贵族关系-80（但不一定开战）
                if (battaniaKingdom != null)
                {
                    foreach (var lord in battaniaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                OriginLog.Info("[DegradedRogueKnight] 敌人关系设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置敌人关系失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的罪犯状态
        /// </summary>
        private static void ApplyDegradedRogueKnightCrimeStatus(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(vlandiaKingdom, 100);
                }

                var empireKingdom = FindKingdom("kingdom_empire");
                if (empireKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(empireKingdom, 100);
                }

                OriginLog.Info("[DegradedRogueKnight] 罪犯状态设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置罪犯状态失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士Node1效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_crime")) return;

            string crime = nodes["vla_node_degraded_crime"];
            switch (crime)
            {
                case "tyranny": // 暴虐之罪
                    ApplyVillageRelations(hero, -20);
                    break;
                case "indulgence": // 沉溺之罪
                    PresetOriginSystem.AddGold(hero, 3500);
                    break;
                case "filth": // 污秽之罪
                    break;
                case "conspiracy": // 阴谋之罪
                    break;
            }
        }

        /// <summary>
        /// 应用无赖骑士Node2效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode2(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_first_case")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node3效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_view")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node4效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode4(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_goal")) return;
        }

        /// <summary>
        /// 设置瓦兰迪亚初始装备
        /// </summary>
        private static void SetVlandiaInitialEquipment(Hero hero, string originType)
        {
            try
            {
                if (hero == null || hero.BattleEquipment == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] Hero或BattleEquipment为空");
                    return;
                }

                var allItems = MBObjectManager.Instance?.GetObjectTypeList<ItemObject>();
                if (allItems == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] 无法获取物品列表");
                    return;
                }

                if (originType == "expedition_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else if (originType == "degraded_rogue_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, false);
                }

                OriginLog.Info($"[SetVlandiaInitialEquipment] {originType} 装备设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[SetVlandiaInitialEquipment] 设置装备失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据档位设置瓦兰迪亚装备
        /// </summary>
        private static void SetVlandiaEquipmentByTier(Hero hero, IEnumerable<ItemObject> allItems, int tier, bool damaged)
        {
            var vlandiaItems = allItems.Where(item =>
                (item.Culture != null && item.Culture.StringId == "vlandia") ||
                item.StringId.Contains("vlandia") ||
                item.StringId.Contains("vlandian"))
                .ToList();

            // 身体护甲
            if (hero.BattleEquipment[EquipmentIndex.Body].IsEmpty)
            {
                var bodyArmor = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.BodyArmor &&
                    (tier == 2 ? item.StringId.Contains("t2") : tier == 3 ? item.StringId.Contains("t3") : item.StringId.Contains("t4")));
                if (bodyArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Body] = new EquipmentElement(bodyArmor);
                }
            }

            // 腿部护甲
            if (hero.BattleEquipment[EquipmentIndex.Leg].IsEmpty)
            {
                var legArmor = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.LegArmor);
                if (legArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Leg] = new EquipmentElement(legArmor);
                }
            }

            // 武器：长剑+盾
            if (hero.BattleEquipment[EquipmentIndex.Weapon0].IsEmpty)
            {
                var sword = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.OneHandedWeapon &&
                    (item.StringId.Contains("sword") || item.StringId.Contains("blade")));
                if (sword != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon0] = new EquipmentElement(sword);
                }
            }

            // 盾牌
            if (hero.BattleEquipment[EquipmentIndex.Weapon1].IsEmpty)
            {
                var shield = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.Shield);
                if (shield != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon1] = new EquipmentElement(shield);
                }
            }

            // 骑枪
            if (hero.BattleEquipment[EquipmentIndex.Weapon2].IsEmpty)
            {
                var lance = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.Polearm &&
                    (item.StringId.Contains("lance") || item.StringId.Contains("spear")));
                if (lance != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon2] = new EquipmentElement(lance);
                }
            }
        }

        /// <summary>
        /// 应用随机贵族关系
        /// </summary>
        private static void ApplyRandomNobleRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    var lords = vlandiaKingdom.Lords.Where(h => h.IsLord && h != hero).ToList();
                    if (lords.Count > 0)
                    {
                        var randomLord = lords[new Random().Next(lords.Count)];
                        ChangeRelationAction.ApplyPlayerRelation(randomLord, relationChange);
                        OriginLog.Info($"[ApplyRandomNobleRelation] 与 {randomLord.Name} 关系 {relationChange}");
                    }
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRandomNobleRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用王室派系关系
        /// </summary>
        private static void ApplyRoyalFactionRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null && vlandiaKingdom.Leader != null)
                {
                    ChangeRelationAction.ApplyPlayerRelation(vlandiaKingdom.Leader, relationChange);
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRoyalFactionRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用村庄关系
        /// </summary>
        private static void ApplyVillageRelations(Hero hero, int relationChange)
        {
            try
            {
                OriginLog.Info($"[ApplyVillageRelations] 村庄关系变化: {relationChange}");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyVillageRelations] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 查找王国
        /// </summary>
        private static Kingdom FindKingdom(string kingdomId)
        {
            try
            {
                return Campaign.Current?.Kingdoms?.FirstOrDefault(k =>
                    k.StringId == kingdomId ||
                    k.StringId == $"kingdom_{kingdomId}" ||
                    (k.Culture != null && k.Culture.StringId == kingdomId));
            }
            catch
            {
                return null;
            }
        }
    }
}


using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace OriginSystemMod
{
    /// <summary>
    /// 瓦兰迪亚预设出身应用逻辑
    /// </summary>
    public static class VlandiaOriginSystem
    {
        /// <summary>
        /// 应用远征的骑士出身
        /// </summary>
        public static void ApplyExpeditionKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 开始应用远征的骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 3);
                PresetOriginSystem.GainRenown(hero, 30);
                PresetOriginSystem.AddGold(hero, 3500);

                // 设置初始瓦兰迪亚风格装备
                SetVlandiaInitialEquipment(hero, "expedition_knight");

                // 设置出生位置（根据Node2的誓言选择决定）
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                if (selectedNodes.ContainsKey("vla_node_expedition_oath"))
                {
                    string oath = selectedNodes["vla_node_expedition_oath"];
                    string startLocation = GetExpeditionKnightStartLocation(oath);
                    if (!string.IsNullOrEmpty(startLocation))
                    {
                        PresetOriginSystem.SetPresetOriginStartingLocation(party, startLocation);
                    }
                }
                else
                {
                    PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");
                }

                // 应用节点效果
                ApplyExpeditionKnightNode1(hero, selectedNodes);
                ApplyExpeditionKnightNode2(hero, clan, party, selectedNodes);
                ApplyExpeditionKnightNode3(hero, selectedNodes);
                ApplyExpeditionKnightNode4(hero, party, selectedNodes);

                // 创建哥哥NPC（固定存在）
                CreateExpeditionKnightBrother(hero, clan);

                OriginLog.Info("[ExpeditionKnight] 远征的骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[ExpeditionKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 根据誓言选择获取出生位置
        /// </summary>
        private static string GetExpeditionKnightStartLocation(string oath)
        {
            switch (oath)
            {
                case "kill_sea_raiders": // 杀1000海寇
                    return "nord"; // 诺德村子
                case "conquer_gyaz": // 远征古亚兹
                    return "aserai"; // 阿塞莱村子（靠近古亚兹）
                case "kill_battanian_lord": // 斩巴丹尼亚氏族
                    return "battania"; // 巴丹尼亚村子（肖农附近）
                case "recover_banner": // 寻回失旗
                    return "vlandia"; // 默认瓦兰迪亚
                default:
                    return "vlandia";
            }
        }

        /// <summary>
        /// 应用远征骑士Node1效果
        /// </summary>
        private static void ApplyExpeditionKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_fall")) return;

            string fall = nodes["vla_node_expedition_fall"];
            switch (fall)
            {
                case "erased_defeat":
                    PresetOriginSystem.AddSkill(hero, "tactics", 30);
                    PresetOriginSystem.AddSkill(hero, "leadership", 20);
                    ApplyRandomNobleRelation(hero, -30);
                    break;
                case "annexed":
                    PresetOriginSystem.AddSkill(hero, "charm", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "debt":
                    PresetOriginSystem.AddSkill(hero, "trade", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 2500);
                    break;
                case "political":
                    PresetOriginSystem.AddSkill(hero, "scouting", 20);
                    PresetOriginSystem.AddSkill(hero, "tactics", 20);
                    ApplyRoyalFactionRelation(hero, -30);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node2效果（誓言任务）
        /// </summary>
        private static void ApplyExpeditionKnightNode2(Hero hero, Clan clan, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_oath")) return;

            string oath = nodes["vla_node_expedition_oath"];
            
            // 创建誓言任务
            CreateExpeditionKnightOathQuest(hero, oath);
        }

        /// <summary>
        /// 应用远征骑士Node3效果
        /// </summary>
        private static void ApplyExpeditionKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_chivalry")) return;

            string chivalry = nodes["vla_node_expedition_chivalry"];
            switch (chivalry)
            {
                case "mercy": // 仁慈之誓
                    PresetOriginSystem.AddGold(hero, -2000);
                    break;
                case "valor": // 勇武之誓
                    // 开局多2-4精锐随骑（在Node4中处理）
                    break;
                case "prudence": // 谨慎之誓
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "cynical": // 犬儒之誓
                    PresetOriginSystem.AddGold(hero, 3000);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node4效果
        /// </summary>
        private static void ApplyExpeditionKnightNode4(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_division")) return;

            string division = nodes["vla_node_expedition_division"];
            switch (division)
            {
                case "brother_commands":
                    // 哥哥掌军，你主外交：哥哥偏战术/统御，你偏魅力/管理；随从偏步弩
                    break;
                case "you_command":
                    // 你掌军，哥哥做见证人：你拿骑兵核心，哥哥提供稳定后勤
                    break;
                case "joint_decision":
                    // 共同决策：队伍更均衡
                    break;
            }
        }

        /// <summary>
        /// 创建远征骑士的哥哥NPC
        /// </summary>
        private static void CreateExpeditionKnightBrother(Hero hero, Clan clan)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 需要创建哥哥NPC（待实现）");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建哥哥NPC失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 创建远征骑士的誓言任务
        /// </summary>
        private static void CreateExpeditionKnightOathQuest(Hero hero, string oath)
        {
            try
            {
                OriginLog.Info($"[ExpeditionKnight] 创建誓言任务: {oath}");
                // 任务系统需要单独实现，这里先记录
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建任务失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用堕落无赖骑士出身
        /// </summary>
        public static void ApplyDegradedRogueKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[DegradedRogueKnight] 开始应用堕落无赖骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 2);
                PresetOriginSystem.GainRenown(hero, 10);
                PresetOriginSystem.AddGold(hero, 4000);

                // 设置初始瓦兰迪亚风格装备（败落骑士风格）
                SetVlandiaInitialEquipment(hero, "degraded_rogue_knight");

                // 设置敌人关系
                ApplyDegradedRogueKnightEnemyRelations(hero);

                // 设置犯罪度（罪犯标记）
                ApplyDegradedRogueKnightCrimeStatus(hero);

                // 设置出生位置（瓦兰迪亚附近，但作为罪犯）
                PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");

                // 应用节点效果
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                ApplyDegradedRogueKnightNode1(hero, selectedNodes);
                ApplyDegradedRogueKnightNode2(hero, selectedNodes);
                ApplyDegradedRogueKnightNode3(hero, selectedNodes);
                ApplyDegradedRogueKnightNode4(hero, selectedNodes);

                OriginLog.Info("[DegradedRogueKnight] 堕落无赖骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[DegradedRogueKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的敌人关系
        /// </summary>
        private static void ApplyDegradedRogueKnightEnemyRelations(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                var empireKingdom = FindKingdom("kingdom_empire");
                var battaniaKingdom = FindKingdom("kingdom_battania");

                // 与瓦兰迪亚敌对
                if (vlandiaKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, vlandiaKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, vlandiaKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与瓦兰迪亚宣战");
                    }

                    foreach (var lord in vlandiaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与帝国敌对
                if (empireKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, empireKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, empireKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与帝国宣战");
                    }

                    foreach (var lord in empireKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与巴丹尼亚贵族关系-80（但不一定开战）
                if (battaniaKingdom != null)
                {
                    foreach (var lord in battaniaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                OriginLog.Info("[DegradedRogueKnight] 敌人关系设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置敌人关系失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的罪犯状态
        /// </summary>
        private static void ApplyDegradedRogueKnightCrimeStatus(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(vlandiaKingdom, 100);
                }

                var empireKingdom = FindKingdom("kingdom_empire");
                if (empireKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(empireKingdom, 100);
                }

                OriginLog.Info("[DegradedRogueKnight] 罪犯状态设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置罪犯状态失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士Node1效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_crime")) return;

            string crime = nodes["vla_node_degraded_crime"];
            switch (crime)
            {
                case "tyranny": // 暴虐之罪
                    ApplyVillageRelations(hero, -20);
                    break;
                case "indulgence": // 沉溺之罪
                    PresetOriginSystem.AddGold(hero, 3500);
                    break;
                case "filth": // 污秽之罪
                    break;
                case "conspiracy": // 阴谋之罪
                    break;
            }
        }

        /// <summary>
        /// 应用无赖骑士Node2效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode2(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_first_case")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node3效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_view")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node4效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode4(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_goal")) return;
        }

        /// <summary>
        /// 设置瓦兰迪亚初始装备
        /// </summary>
        private static void SetVlandiaInitialEquipment(Hero hero, string originType)
        {
            try
            {
                if (hero == null || hero.BattleEquipment == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] Hero或BattleEquipment为空");
                    return;
                }

                var allItems = MBObjectManager.Instance?.GetObjectTypeList<ItemObject>();
                if (allItems == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] 无法获取物品列表");
                    return;
                }

                if (originType == "expedition_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else if (originType == "degraded_rogue_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, false);
                }

                OriginLog.Info($"[SetVlandiaInitialEquipment] {originType} 装备设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[SetVlandiaInitialEquipment] 设置装备失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据档位设置瓦兰迪亚装备
        /// </summary>
        private static void SetVlandiaEquipmentByTier(Hero hero, IEnumerable<ItemObject> allItems, int tier, bool damaged)
        {
            var vlandiaItems = allItems.Where(item =>
                (item.Culture != null && item.Culture.StringId == "vlandia") ||
                item.StringId.Contains("vlandia") ||
                item.StringId.Contains("vlandian"))
                .ToList();

            // 身体护甲
            if (hero.BattleEquipment[EquipmentIndex.Body].IsEmpty)
            {
                var bodyArmor = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.BodyArmor &&
                    (tier == 2 ? item.StringId.Contains("t2") : tier == 3 ? item.StringId.Contains("t3") : item.StringId.Contains("t4")));
                if (bodyArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Body] = new EquipmentElement(bodyArmor);
                }
            }

            // 腿部护甲
            if (hero.BattleEquipment[EquipmentIndex.Leg].IsEmpty)
            {
                var legArmor = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.LegArmor);
                if (legArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Leg] = new EquipmentElement(legArmor);
                }
            }

            // 武器：长剑+盾
            if (hero.BattleEquipment[EquipmentIndex.Weapon0].IsEmpty)
            {
                var sword = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.OneHandedWeapon &&
                    (item.StringId.Contains("sword") || item.StringId.Contains("blade")));
                if (sword != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon0] = new EquipmentElement(sword);
                }
            }

            // 盾牌
            if (hero.BattleEquipment[EquipmentIndex.Weapon1].IsEmpty)
            {
                var shield = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.Shield);
                if (shield != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon1] = new EquipmentElement(shield);
                }
            }

            // 骑枪
            if (hero.BattleEquipment[EquipmentIndex.Weapon2].IsEmpty)
            {
                var lance = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.Polearm &&
                    (item.StringId.Contains("lance") || item.StringId.Contains("spear")));
                if (lance != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon2] = new EquipmentElement(lance);
                }
            }
        }

        /// <summary>
        /// 应用随机贵族关系
        /// </summary>
        private static void ApplyRandomNobleRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    var lords = vlandiaKingdom.Lords.Where(h => h.IsLord && h != hero).ToList();
                    if (lords.Count > 0)
                    {
                        var randomLord = lords[new Random().Next(lords.Count)];
                        ChangeRelationAction.ApplyPlayerRelation(randomLord, relationChange);
                        OriginLog.Info($"[ApplyRandomNobleRelation] 与 {randomLord.Name} 关系 {relationChange}");
                    }
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRandomNobleRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用王室派系关系
        /// </summary>
        private static void ApplyRoyalFactionRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null && vlandiaKingdom.Leader != null)
                {
                    ChangeRelationAction.ApplyPlayerRelation(vlandiaKingdom.Leader, relationChange);
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRoyalFactionRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用村庄关系
        /// </summary>
        private static void ApplyVillageRelations(Hero hero, int relationChange)
        {
            try
            {
                OriginLog.Info($"[ApplyVillageRelations] 村庄关系变化: {relationChange}");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyVillageRelations] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 查找王国
        /// </summary>
        private static Kingdom FindKingdom(string kingdomId)
        {
            try
            {
                return Campaign.Current?.Kingdoms?.FirstOrDefault(k =>
                    k.StringId == kingdomId ||
                    k.StringId == $"kingdom_{kingdomId}" ||
                    (k.Culture != null && k.Culture.StringId == kingdomId));
            }
            catch
            {
                return null;
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace OriginSystemMod
{
    /// <summary>
    /// 瓦兰迪亚预设出身应用逻辑
    /// </summary>
    public static class VlandiaOriginSystem
    {
        /// <summary>
        /// 应用远征的骑士出身
        /// </summary>
        public static void ApplyExpeditionKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 开始应用远征的骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 3);
                PresetOriginSystem.GainRenown(hero, 30);
                PresetOriginSystem.AddGold(hero, 3500);

                // 设置初始瓦兰迪亚风格装备
                SetVlandiaInitialEquipment(hero, "expedition_knight");

                // 设置出生位置（根据Node2的誓言选择决定）
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                if (selectedNodes.ContainsKey("vla_node_expedition_oath"))
                {
                    string oath = selectedNodes["vla_node_expedition_oath"];
                    string startLocation = GetExpeditionKnightStartLocation(oath);
                    if (!string.IsNullOrEmpty(startLocation))
                    {
                        PresetOriginSystem.SetPresetOriginStartingLocation(party, startLocation);
                    }
                }
                else
                {
                    PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");
                }

                // 应用节点效果
                ApplyExpeditionKnightNode1(hero, selectedNodes);
                ApplyExpeditionKnightNode2(hero, clan, party, selectedNodes);
                ApplyExpeditionKnightNode3(hero, selectedNodes);
                ApplyExpeditionKnightNode4(hero, party, selectedNodes);

                // 创建哥哥NPC（固定存在）
                CreateExpeditionKnightBrother(hero, clan);

                OriginLog.Info("[ExpeditionKnight] 远征的骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[ExpeditionKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 根据誓言选择获取出生位置
        /// </summary>
        private static string GetExpeditionKnightStartLocation(string oath)
        {
            switch (oath)
            {
                case "kill_sea_raiders": // 杀1000海寇
                    return "nord"; // 诺德村子
                case "conquer_gyaz": // 远征古亚兹
                    return "aserai"; // 阿塞莱村子（靠近古亚兹）
                case "kill_battanian_lord": // 斩巴丹尼亚氏族
                    return "battania"; // 巴丹尼亚村子（肖农附近）
                case "recover_banner": // 寻回失旗
                    return "vlandia"; // 默认瓦兰迪亚
                default:
                    return "vlandia";
            }
        }

        /// <summary>
        /// 应用远征骑士Node1效果
        /// </summary>
        private static void ApplyExpeditionKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_fall")) return;

            string fall = nodes["vla_node_expedition_fall"];
            switch (fall)
            {
                case "erased_defeat":
                    PresetOriginSystem.AddSkill(hero, "tactics", 30);
                    PresetOriginSystem.AddSkill(hero, "leadership", 20);
                    ApplyRandomNobleRelation(hero, -30);
                    break;
                case "annexed":
                    PresetOriginSystem.AddSkill(hero, "charm", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "debt":
                    PresetOriginSystem.AddSkill(hero, "trade", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 2500);
                    break;
                case "political":
                    PresetOriginSystem.AddSkill(hero, "scouting", 20);
                    PresetOriginSystem.AddSkill(hero, "tactics", 20);
                    ApplyRoyalFactionRelation(hero, -30);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node2效果（誓言任务）
        /// </summary>
        private static void ApplyExpeditionKnightNode2(Hero hero, Clan clan, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_oath")) return;

            string oath = nodes["vla_node_expedition_oath"];
            
            // 创建誓言任务
            CreateExpeditionKnightOathQuest(hero, oath);
        }

        /// <summary>
        /// 应用远征骑士Node3效果
        /// </summary>
        private static void ApplyExpeditionKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_chivalry")) return;

            string chivalry = nodes["vla_node_expedition_chivalry"];
            switch (chivalry)
            {
                case "mercy": // 仁慈之誓
                    PresetOriginSystem.AddGold(hero, -2000);
                    break;
                case "valor": // 勇武之誓
                    // 开局多2-4精锐随骑（在Node4中处理）
                    break;
                case "prudence": // 谨慎之誓
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "cynical": // 犬儒之誓
                    PresetOriginSystem.AddGold(hero, 3000);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node4效果
        /// </summary>
        private static void ApplyExpeditionKnightNode4(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_division")) return;

            string division = nodes["vla_node_expedition_division"];
            switch (division)
            {
                case "brother_commands":
                    // 哥哥掌军，你主外交：哥哥偏战术/统御，你偏魅力/管理；随从偏步弩
                    break;
                case "you_command":
                    // 你掌军，哥哥做见证人：你拿骑兵核心，哥哥提供稳定后勤
                    break;
                case "joint_decision":
                    // 共同决策：队伍更均衡
                    break;
            }
        }

        /// <summary>
        /// 创建远征骑士的哥哥NPC
        /// </summary>
        private static void CreateExpeditionKnightBrother(Hero hero, Clan clan)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 需要创建哥哥NPC（待实现）");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建哥哥NPC失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 创建远征骑士的誓言任务
        /// </summary>
        private static void CreateExpeditionKnightOathQuest(Hero hero, string oath)
        {
            try
            {
                OriginLog.Info($"[ExpeditionKnight] 创建誓言任务: {oath}");
                // 任务系统需要单独实现，这里先记录
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建任务失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用堕落无赖骑士出身
        /// </summary>
        public static void ApplyDegradedRogueKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[DegradedRogueKnight] 开始应用堕落无赖骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 2);
                PresetOriginSystem.GainRenown(hero, 10);
                PresetOriginSystem.AddGold(hero, 4000);

                // 设置初始瓦兰迪亚风格装备（败落骑士风格）
                SetVlandiaInitialEquipment(hero, "degraded_rogue_knight");

                // 设置敌人关系
                ApplyDegradedRogueKnightEnemyRelations(hero);

                // 设置犯罪度（罪犯标记）
                ApplyDegradedRogueKnightCrimeStatus(hero);

                // 设置出生位置（瓦兰迪亚附近，但作为罪犯）
                PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");

                // 应用节点效果
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                ApplyDegradedRogueKnightNode1(hero, selectedNodes);
                ApplyDegradedRogueKnightNode2(hero, selectedNodes);
                ApplyDegradedRogueKnightNode3(hero, selectedNodes);
                ApplyDegradedRogueKnightNode4(hero, selectedNodes);

                OriginLog.Info("[DegradedRogueKnight] 堕落无赖骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[DegradedRogueKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的敌人关系
        /// </summary>
        private static void ApplyDegradedRogueKnightEnemyRelations(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                var empireKingdom = FindKingdom("kingdom_empire");
                var battaniaKingdom = FindKingdom("kingdom_battania");

                // 与瓦兰迪亚敌对
                if (vlandiaKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, vlandiaKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, vlandiaKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与瓦兰迪亚宣战");
                    }

                    foreach (var lord in vlandiaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与帝国敌对
                if (empireKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, empireKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, empireKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与帝国宣战");
                    }

                    foreach (var lord in empireKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与巴丹尼亚贵族关系-80（但不一定开战）
                if (battaniaKingdom != null)
                {
                    foreach (var lord in battaniaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                OriginLog.Info("[DegradedRogueKnight] 敌人关系设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置敌人关系失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的罪犯状态
        /// </summary>
        private static void ApplyDegradedRogueKnightCrimeStatus(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(vlandiaKingdom, 100);
                }

                var empireKingdom = FindKingdom("kingdom_empire");
                if (empireKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(empireKingdom, 100);
                }

                OriginLog.Info("[DegradedRogueKnight] 罪犯状态设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置罪犯状态失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士Node1效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_crime")) return;

            string crime = nodes["vla_node_degraded_crime"];
            switch (crime)
            {
                case "tyranny": // 暴虐之罪
                    ApplyVillageRelations(hero, -20);
                    break;
                case "indulgence": // 沉溺之罪
                    PresetOriginSystem.AddGold(hero, 3500);
                    break;
                case "filth": // 污秽之罪
                    break;
                case "conspiracy": // 阴谋之罪
                    break;
            }
        }

        /// <summary>
        /// 应用无赖骑士Node2效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode2(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_first_case")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node3效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_view")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node4效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode4(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_goal")) return;
        }

        /// <summary>
        /// 设置瓦兰迪亚初始装备
        /// </summary>
        private static void SetVlandiaInitialEquipment(Hero hero, string originType)
        {
            try
            {
                if (hero == null || hero.BattleEquipment == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] Hero或BattleEquipment为空");
                    return;
                }

                var allItems = MBObjectManager.Instance?.GetObjectTypeList<ItemObject>();
                if (allItems == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] 无法获取物品列表");
                    return;
                }

                if (originType == "expedition_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else if (originType == "degraded_rogue_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, false);
                }

                OriginLog.Info($"[SetVlandiaInitialEquipment] {originType} 装备设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[SetVlandiaInitialEquipment] 设置装备失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据档位设置瓦兰迪亚装备
        /// </summary>
        private static void SetVlandiaEquipmentByTier(Hero hero, IEnumerable<ItemObject> allItems, int tier, bool damaged)
        {
            var vlandiaItems = allItems.Where(item =>
                (item.Culture != null && item.Culture.StringId == "vlandia") ||
                item.StringId.Contains("vlandia") ||
                item.StringId.Contains("vlandian"))
                .ToList();

            // 身体护甲
            if (hero.BattleEquipment[EquipmentIndex.Body].IsEmpty)
            {
                var bodyArmor = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.BodyArmor &&
                    (tier == 2 ? item.StringId.Contains("t2") : tier == 3 ? item.StringId.Contains("t3") : item.StringId.Contains("t4")));
                if (bodyArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Body] = new EquipmentElement(bodyArmor);
                }
            }

            // 腿部护甲
            if (hero.BattleEquipment[EquipmentIndex.Leg].IsEmpty)
            {
                var legArmor = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.LegArmor);
                if (legArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Leg] = new EquipmentElement(legArmor);
                }
            }

            // 武器：长剑+盾
            if (hero.BattleEquipment[EquipmentIndex.Weapon0].IsEmpty)
            {
                var sword = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.OneHandedWeapon &&
                    (item.StringId.Contains("sword") || item.StringId.Contains("blade")));
                if (sword != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon0] = new EquipmentElement(sword);
                }
            }

            // 盾牌
            if (hero.BattleEquipment[EquipmentIndex.Weapon1].IsEmpty)
            {
                var shield = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.Shield);
                if (shield != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon1] = new EquipmentElement(shield);
                }
            }

            // 骑枪
            if (hero.BattleEquipment[EquipmentIndex.Weapon2].IsEmpty)
            {
                var lance = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.Polearm &&
                    (item.StringId.Contains("lance") || item.StringId.Contains("spear")));
                if (lance != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon2] = new EquipmentElement(lance);
                }
            }
        }

        /// <summary>
        /// 应用随机贵族关系
        /// </summary>
        private static void ApplyRandomNobleRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    var lords = vlandiaKingdom.Lords.Where(h => h.IsLord && h != hero).ToList();
                    if (lords.Count > 0)
                    {
                        var randomLord = lords[new Random().Next(lords.Count)];
                        ChangeRelationAction.ApplyPlayerRelation(randomLord, relationChange);
                        OriginLog.Info($"[ApplyRandomNobleRelation] 与 {randomLord.Name} 关系 {relationChange}");
                    }
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRandomNobleRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用王室派系关系
        /// </summary>
        private static void ApplyRoyalFactionRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null && vlandiaKingdom.Leader != null)
                {
                    ChangeRelationAction.ApplyPlayerRelation(vlandiaKingdom.Leader, relationChange);
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRoyalFactionRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用村庄关系
        /// </summary>
        private static void ApplyVillageRelations(Hero hero, int relationChange)
        {
            try
            {
                OriginLog.Info($"[ApplyVillageRelations] 村庄关系变化: {relationChange}");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyVillageRelations] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 查找王国
        /// </summary>
        private static Kingdom FindKingdom(string kingdomId)
        {
            try
            {
                return Campaign.Current?.Kingdoms?.FirstOrDefault(k =>
                    k.StringId == kingdomId ||
                    k.StringId == $"kingdom_{kingdomId}" ||
                    (k.Culture != null && k.Culture.StringId == kingdomId));
            }
            catch
            {
                return null;
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace OriginSystemMod
{
    /// <summary>
    /// 瓦兰迪亚预设出身应用逻辑
    /// </summary>
    public static class VlandiaOriginSystem
    {
        /// <summary>
        /// 应用远征的骑士出身
        /// </summary>
        public static void ApplyExpeditionKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 开始应用远征的骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 3);
                PresetOriginSystem.GainRenown(hero, 30);
                PresetOriginSystem.AddGold(hero, 3500);

                // 设置初始瓦兰迪亚风格装备
                SetVlandiaInitialEquipment(hero, "expedition_knight");

                // 设置出生位置（根据Node2的誓言选择决定）
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                if (selectedNodes.ContainsKey("vla_node_expedition_oath"))
                {
                    string oath = selectedNodes["vla_node_expedition_oath"];
                    string startLocation = GetExpeditionKnightStartLocation(oath);
                    if (!string.IsNullOrEmpty(startLocation))
                    {
                        PresetOriginSystem.SetPresetOriginStartingLocation(party, startLocation);
                    }
                }
                else
                {
                    PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");
                }

                // 应用节点效果
                ApplyExpeditionKnightNode1(hero, selectedNodes);
                ApplyExpeditionKnightNode2(hero, clan, party, selectedNodes);
                ApplyExpeditionKnightNode3(hero, selectedNodes);
                ApplyExpeditionKnightNode4(hero, party, selectedNodes);

                // 创建哥哥NPC（固定存在）
                CreateExpeditionKnightBrother(hero, clan);

                OriginLog.Info("[ExpeditionKnight] 远征的骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[ExpeditionKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 根据誓言选择获取出生位置
        /// </summary>
        private static string GetExpeditionKnightStartLocation(string oath)
        {
            switch (oath)
            {
                case "kill_sea_raiders": // 杀1000海寇
                    return "nord"; // 诺德村子
                case "conquer_gyaz": // 远征古亚兹
                    return "aserai"; // 阿塞莱村子（靠近古亚兹）
                case "kill_battanian_lord": // 斩巴丹尼亚氏族
                    return "battania"; // 巴丹尼亚村子（肖农附近）
                case "recover_banner": // 寻回失旗
                    return "vlandia"; // 默认瓦兰迪亚
                default:
                    return "vlandia";
            }
        }

        /// <summary>
        /// 应用远征骑士Node1效果
        /// </summary>
        private static void ApplyExpeditionKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_fall")) return;

            string fall = nodes["vla_node_expedition_fall"];
            switch (fall)
            {
                case "erased_defeat":
                    PresetOriginSystem.AddSkill(hero, "tactics", 30);
                    PresetOriginSystem.AddSkill(hero, "leadership", 20);
                    ApplyRandomNobleRelation(hero, -30);
                    break;
                case "annexed":
                    PresetOriginSystem.AddSkill(hero, "charm", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "debt":
                    PresetOriginSystem.AddSkill(hero, "trade", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 2500);
                    break;
                case "political":
                    PresetOriginSystem.AddSkill(hero, "scouting", 20);
                    PresetOriginSystem.AddSkill(hero, "tactics", 20);
                    ApplyRoyalFactionRelation(hero, -30);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node2效果（誓言任务）
        /// </summary>
        private static void ApplyExpeditionKnightNode2(Hero hero, Clan clan, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_oath")) return;

            string oath = nodes["vla_node_expedition_oath"];
            
            // 创建誓言任务
            CreateExpeditionKnightOathQuest(hero, oath);
        }

        /// <summary>
        /// 应用远征骑士Node3效果
        /// </summary>
        private static void ApplyExpeditionKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_chivalry")) return;

            string chivalry = nodes["vla_node_expedition_chivalry"];
            switch (chivalry)
            {
                case "mercy": // 仁慈之誓
                    PresetOriginSystem.AddGold(hero, -2000);
                    break;
                case "valor": // 勇武之誓
                    // 开局多2-4精锐随骑（在Node4中处理）
                    break;
                case "prudence": // 谨慎之誓
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "cynical": // 犬儒之誓
                    PresetOriginSystem.AddGold(hero, 3000);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node4效果
        /// </summary>
        private static void ApplyExpeditionKnightNode4(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_division")) return;

            string division = nodes["vla_node_expedition_division"];
            switch (division)
            {
                case "brother_commands":
                    // 哥哥掌军，你主外交：哥哥偏战术/统御，你偏魅力/管理；随从偏步弩
                    break;
                case "you_command":
                    // 你掌军，哥哥做见证人：你拿骑兵核心，哥哥提供稳定后勤
                    break;
                case "joint_decision":
                    // 共同决策：队伍更均衡
                    break;
            }
        }

        /// <summary>
        /// 创建远征骑士的哥哥NPC
        /// </summary>
        private static void CreateExpeditionKnightBrother(Hero hero, Clan clan)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 需要创建哥哥NPC（待实现）");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建哥哥NPC失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 创建远征骑士的誓言任务
        /// </summary>
        private static void CreateExpeditionKnightOathQuest(Hero hero, string oath)
        {
            try
            {
                OriginLog.Info($"[ExpeditionKnight] 创建誓言任务: {oath}");
                // 任务系统需要单独实现，这里先记录
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建任务失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用堕落无赖骑士出身
        /// </summary>
        public static void ApplyDegradedRogueKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[DegradedRogueKnight] 开始应用堕落无赖骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 2);
                PresetOriginSystem.GainRenown(hero, 10);
                PresetOriginSystem.AddGold(hero, 4000);

                // 设置初始瓦兰迪亚风格装备（败落骑士风格）
                SetVlandiaInitialEquipment(hero, "degraded_rogue_knight");

                // 设置敌人关系
                ApplyDegradedRogueKnightEnemyRelations(hero);

                // 设置犯罪度（罪犯标记）
                ApplyDegradedRogueKnightCrimeStatus(hero);

                // 设置出生位置（瓦兰迪亚附近，但作为罪犯）
                PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");

                // 应用节点效果
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                ApplyDegradedRogueKnightNode1(hero, selectedNodes);
                ApplyDegradedRogueKnightNode2(hero, selectedNodes);
                ApplyDegradedRogueKnightNode3(hero, selectedNodes);
                ApplyDegradedRogueKnightNode4(hero, selectedNodes);

                OriginLog.Info("[DegradedRogueKnight] 堕落无赖骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[DegradedRogueKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的敌人关系
        /// </summary>
        private static void ApplyDegradedRogueKnightEnemyRelations(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                var empireKingdom = FindKingdom("kingdom_empire");
                var battaniaKingdom = FindKingdom("kingdom_battania");

                // 与瓦兰迪亚敌对
                if (vlandiaKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, vlandiaKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, vlandiaKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与瓦兰迪亚宣战");
                    }

                    foreach (var lord in vlandiaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与帝国敌对
                if (empireKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, empireKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, empireKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与帝国宣战");
                    }

                    foreach (var lord in empireKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与巴丹尼亚贵族关系-80（但不一定开战）
                if (battaniaKingdom != null)
                {
                    foreach (var lord in battaniaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                OriginLog.Info("[DegradedRogueKnight] 敌人关系设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置敌人关系失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的罪犯状态
        /// </summary>
        private static void ApplyDegradedRogueKnightCrimeStatus(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(vlandiaKingdom, 100);
                }

                var empireKingdom = FindKingdom("kingdom_empire");
                if (empireKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(empireKingdom, 100);
                }

                OriginLog.Info("[DegradedRogueKnight] 罪犯状态设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置罪犯状态失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士Node1效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_crime")) return;

            string crime = nodes["vla_node_degraded_crime"];
            switch (crime)
            {
                case "tyranny": // 暴虐之罪
                    ApplyVillageRelations(hero, -20);
                    break;
                case "indulgence": // 沉溺之罪
                    PresetOriginSystem.AddGold(hero, 3500);
                    break;
                case "filth": // 污秽之罪
                    break;
                case "conspiracy": // 阴谋之罪
                    break;
            }
        }

        /// <summary>
        /// 应用无赖骑士Node2效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode2(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_first_case")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node3效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_view")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node4效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode4(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_goal")) return;
        }

        /// <summary>
        /// 设置瓦兰迪亚初始装备
        /// </summary>
        private static void SetVlandiaInitialEquipment(Hero hero, string originType)
        {
            try
            {
                if (hero == null || hero.BattleEquipment == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] Hero或BattleEquipment为空");
                    return;
                }

                var allItems = MBObjectManager.Instance?.GetObjectTypeList<ItemObject>();
                if (allItems == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] 无法获取物品列表");
                    return;
                }

                if (originType == "expedition_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else if (originType == "degraded_rogue_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, false);
                }

                OriginLog.Info($"[SetVlandiaInitialEquipment] {originType} 装备设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[SetVlandiaInitialEquipment] 设置装备失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据档位设置瓦兰迪亚装备
        /// </summary>
        private static void SetVlandiaEquipmentByTier(Hero hero, IEnumerable<ItemObject> allItems, int tier, bool damaged)
        {
            var vlandiaItems = allItems.Where(item =>
                (item.Culture != null && item.Culture.StringId == "vlandia") ||
                item.StringId.Contains("vlandia") ||
                item.StringId.Contains("vlandian"))
                .ToList();

            // 身体护甲
            if (hero.BattleEquipment[EquipmentIndex.Body].IsEmpty)
            {
                var bodyArmor = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.BodyArmor &&
                    (tier == 2 ? item.StringId.Contains("t2") : tier == 3 ? item.StringId.Contains("t3") : item.StringId.Contains("t4")));
                if (bodyArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Body] = new EquipmentElement(bodyArmor);
                }
            }

            // 腿部护甲
            if (hero.BattleEquipment[EquipmentIndex.Leg].IsEmpty)
            {
                var legArmor = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.LegArmor);
                if (legArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Leg] = new EquipmentElement(legArmor);
                }
            }

            // 武器：长剑+盾
            if (hero.BattleEquipment[EquipmentIndex.Weapon0].IsEmpty)
            {
                var sword = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.OneHandedWeapon &&
                    (item.StringId.Contains("sword") || item.StringId.Contains("blade")));
                if (sword != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon0] = new EquipmentElement(sword);
                }
            }

            // 盾牌
            if (hero.BattleEquipment[EquipmentIndex.Weapon1].IsEmpty)
            {
                var shield = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.Shield);
                if (shield != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon1] = new EquipmentElement(shield);
                }
            }

            // 骑枪
            if (hero.BattleEquipment[EquipmentIndex.Weapon2].IsEmpty)
            {
                var lance = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.Polearm &&
                    (item.StringId.Contains("lance") || item.StringId.Contains("spear")));
                if (lance != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon2] = new EquipmentElement(lance);
                }
            }
        }

        /// <summary>
        /// 应用随机贵族关系
        /// </summary>
        private static void ApplyRandomNobleRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    var lords = vlandiaKingdom.Lords.Where(h => h.IsLord && h != hero).ToList();
                    if (lords.Count > 0)
                    {
                        var randomLord = lords[new Random().Next(lords.Count)];
                        ChangeRelationAction.ApplyPlayerRelation(randomLord, relationChange);
                        OriginLog.Info($"[ApplyRandomNobleRelation] 与 {randomLord.Name} 关系 {relationChange}");
                    }
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRandomNobleRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用王室派系关系
        /// </summary>
        private static void ApplyRoyalFactionRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null && vlandiaKingdom.Leader != null)
                {
                    ChangeRelationAction.ApplyPlayerRelation(vlandiaKingdom.Leader, relationChange);
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRoyalFactionRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用村庄关系
        /// </summary>
        private static void ApplyVillageRelations(Hero hero, int relationChange)
        {
            try
            {
                OriginLog.Info($"[ApplyVillageRelations] 村庄关系变化: {relationChange}");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyVillageRelations] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 查找王国
        /// </summary>
        private static Kingdom FindKingdom(string kingdomId)
        {
            try
            {
                return Campaign.Current?.Kingdoms?.FirstOrDefault(k =>
                    k.StringId == kingdomId ||
                    k.StringId == $"kingdom_{kingdomId}" ||
                    (k.Culture != null && k.Culture.StringId == kingdomId));
            }
            catch
            {
                return null;
            }
        }
    }
}


using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace OriginSystemMod
{
    /// <summary>
    /// 瓦兰迪亚预设出身应用逻辑
    /// </summary>
    public static class VlandiaOriginSystem
    {
        /// <summary>
        /// 应用远征的骑士出身
        /// </summary>
        public static void ApplyExpeditionKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 开始应用远征的骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 3);
                PresetOriginSystem.GainRenown(hero, 30);
                PresetOriginSystem.AddGold(hero, 3500);

                // 设置初始瓦兰迪亚风格装备
                SetVlandiaInitialEquipment(hero, "expedition_knight");

                // 设置出生位置（根据Node2的誓言选择决定）
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                if (selectedNodes.ContainsKey("vla_node_expedition_oath"))
                {
                    string oath = selectedNodes["vla_node_expedition_oath"];
                    string startLocation = GetExpeditionKnightStartLocation(oath);
                    if (!string.IsNullOrEmpty(startLocation))
                    {
                        PresetOriginSystem.SetPresetOriginStartingLocation(party, startLocation);
                    }
                }
                else
                {
                    PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");
                }

                // 应用节点效果
                ApplyExpeditionKnightNode1(hero, selectedNodes);
                ApplyExpeditionKnightNode2(hero, clan, party, selectedNodes);
                ApplyExpeditionKnightNode3(hero, selectedNodes);
                ApplyExpeditionKnightNode4(hero, party, selectedNodes);

                // 创建哥哥NPC（固定存在）
                CreateExpeditionKnightBrother(hero, clan);

                OriginLog.Info("[ExpeditionKnight] 远征的骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[ExpeditionKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 根据誓言选择获取出生位置
        /// </summary>
        private static string GetExpeditionKnightStartLocation(string oath)
        {
            switch (oath)
            {
                case "kill_sea_raiders": // 杀1000海寇
                    return "nord"; // 诺德村子
                case "conquer_gyaz": // 远征古亚兹
                    return "aserai"; // 阿塞莱村子（靠近古亚兹）
                case "kill_battanian_lord": // 斩巴丹尼亚氏族
                    return "battania"; // 巴丹尼亚村子（肖农附近）
                case "recover_banner": // 寻回失旗
                    return "vlandia"; // 默认瓦兰迪亚
                default:
                    return "vlandia";
            }
        }

        /// <summary>
        /// 应用远征骑士Node1效果
        /// </summary>
        private static void ApplyExpeditionKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_fall")) return;

            string fall = nodes["vla_node_expedition_fall"];
            switch (fall)
            {
                case "erased_defeat":
                    PresetOriginSystem.AddSkill(hero, "tactics", 30);
                    PresetOriginSystem.AddSkill(hero, "leadership", 20);
                    ApplyRandomNobleRelation(hero, -30);
                    break;
                case "annexed":
                    PresetOriginSystem.AddSkill(hero, "charm", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "debt":
                    PresetOriginSystem.AddSkill(hero, "trade", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 2500);
                    break;
                case "political":
                    PresetOriginSystem.AddSkill(hero, "scouting", 20);
                    PresetOriginSystem.AddSkill(hero, "tactics", 20);
                    ApplyRoyalFactionRelation(hero, -30);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node2效果（誓言任务）
        /// </summary>
        private static void ApplyExpeditionKnightNode2(Hero hero, Clan clan, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_oath")) return;

            string oath = nodes["vla_node_expedition_oath"];
            
            // 创建誓言任务
            CreateExpeditionKnightOathQuest(hero, oath);
        }

        /// <summary>
        /// 应用远征骑士Node3效果
        /// </summary>
        private static void ApplyExpeditionKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_chivalry")) return;

            string chivalry = nodes["vla_node_expedition_chivalry"];
            switch (chivalry)
            {
                case "mercy": // 仁慈之誓
                    PresetOriginSystem.AddGold(hero, -2000);
                    break;
                case "valor": // 勇武之誓
                    // 开局多2-4精锐随骑（在Node4中处理）
                    break;
                case "prudence": // 谨慎之誓
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "cynical": // 犬儒之誓
                    PresetOriginSystem.AddGold(hero, 3000);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node4效果
        /// </summary>
        private static void ApplyExpeditionKnightNode4(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_division")) return;

            string division = nodes["vla_node_expedition_division"];
            switch (division)
            {
                case "brother_commands":
                    // 哥哥掌军，你主外交：哥哥偏战术/统御，你偏魅力/管理；随从偏步弩
                    break;
                case "you_command":
                    // 你掌军，哥哥做见证人：你拿骑兵核心，哥哥提供稳定后勤
                    break;
                case "joint_decision":
                    // 共同决策：队伍更均衡
                    break;
            }
        }

        /// <summary>
        /// 创建远征骑士的哥哥NPC
        /// </summary>
        private static void CreateExpeditionKnightBrother(Hero hero, Clan clan)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 需要创建哥哥NPC（待实现）");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建哥哥NPC失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 创建远征骑士的誓言任务
        /// </summary>
        private static void CreateExpeditionKnightOathQuest(Hero hero, string oath)
        {
            try
            {
                OriginLog.Info($"[ExpeditionKnight] 创建誓言任务: {oath}");
                // 任务系统需要单独实现，这里先记录
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建任务失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用堕落无赖骑士出身
        /// </summary>
        public static void ApplyDegradedRogueKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[DegradedRogueKnight] 开始应用堕落无赖骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 2);
                PresetOriginSystem.GainRenown(hero, 10);
                PresetOriginSystem.AddGold(hero, 4000);

                // 设置初始瓦兰迪亚风格装备（败落骑士风格）
                SetVlandiaInitialEquipment(hero, "degraded_rogue_knight");

                // 设置敌人关系
                ApplyDegradedRogueKnightEnemyRelations(hero);

                // 设置犯罪度（罪犯标记）
                ApplyDegradedRogueKnightCrimeStatus(hero);

                // 设置出生位置（瓦兰迪亚附近，但作为罪犯）
                PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");

                // 应用节点效果
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                ApplyDegradedRogueKnightNode1(hero, selectedNodes);
                ApplyDegradedRogueKnightNode2(hero, selectedNodes);
                ApplyDegradedRogueKnightNode3(hero, selectedNodes);
                ApplyDegradedRogueKnightNode4(hero, selectedNodes);

                OriginLog.Info("[DegradedRogueKnight] 堕落无赖骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[DegradedRogueKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的敌人关系
        /// </summary>
        private static void ApplyDegradedRogueKnightEnemyRelations(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                var empireKingdom = FindKingdom("kingdom_empire");
                var battaniaKingdom = FindKingdom("kingdom_battania");

                // 与瓦兰迪亚敌对
                if (vlandiaKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, vlandiaKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, vlandiaKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与瓦兰迪亚宣战");
                    }

                    foreach (var lord in vlandiaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与帝国敌对
                if (empireKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, empireKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, empireKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与帝国宣战");
                    }

                    foreach (var lord in empireKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与巴丹尼亚贵族关系-80（但不一定开战）
                if (battaniaKingdom != null)
                {
                    foreach (var lord in battaniaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                OriginLog.Info("[DegradedRogueKnight] 敌人关系设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置敌人关系失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的罪犯状态
        /// </summary>
        private static void ApplyDegradedRogueKnightCrimeStatus(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(vlandiaKingdom, 100);
                }

                var empireKingdom = FindKingdom("kingdom_empire");
                if (empireKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(empireKingdom, 100);
                }

                OriginLog.Info("[DegradedRogueKnight] 罪犯状态设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置罪犯状态失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士Node1效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_crime")) return;

            string crime = nodes["vla_node_degraded_crime"];
            switch (crime)
            {
                case "tyranny": // 暴虐之罪
                    ApplyVillageRelations(hero, -20);
                    break;
                case "indulgence": // 沉溺之罪
                    PresetOriginSystem.AddGold(hero, 3500);
                    break;
                case "filth": // 污秽之罪
                    break;
                case "conspiracy": // 阴谋之罪
                    break;
            }
        }

        /// <summary>
        /// 应用无赖骑士Node2效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode2(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_first_case")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node3效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_view")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node4效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode4(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_goal")) return;
        }

        /// <summary>
        /// 设置瓦兰迪亚初始装备
        /// </summary>
        private static void SetVlandiaInitialEquipment(Hero hero, string originType)
        {
            try
            {
                if (hero == null || hero.BattleEquipment == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] Hero或BattleEquipment为空");
                    return;
                }

                var allItems = MBObjectManager.Instance?.GetObjectTypeList<ItemObject>();
                if (allItems == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] 无法获取物品列表");
                    return;
                }

                if (originType == "expedition_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else if (originType == "degraded_rogue_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, false);
                }

                OriginLog.Info($"[SetVlandiaInitialEquipment] {originType} 装备设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[SetVlandiaInitialEquipment] 设置装备失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据档位设置瓦兰迪亚装备
        /// </summary>
        private static void SetVlandiaEquipmentByTier(Hero hero, IEnumerable<ItemObject> allItems, int tier, bool damaged)
        {
            var vlandiaItems = allItems.Where(item =>
                (item.Culture != null && item.Culture.StringId == "vlandia") ||
                item.StringId.Contains("vlandia") ||
                item.StringId.Contains("vlandian"))
                .ToList();

            // 身体护甲
            if (hero.BattleEquipment[EquipmentIndex.Body].IsEmpty)
            {
                var bodyArmor = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.BodyArmor &&
                    (tier == 2 ? item.StringId.Contains("t2") : tier == 3 ? item.StringId.Contains("t3") : item.StringId.Contains("t4")));
                if (bodyArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Body] = new EquipmentElement(bodyArmor);
                }
            }

            // 腿部护甲
            if (hero.BattleEquipment[EquipmentIndex.Leg].IsEmpty)
            {
                var legArmor = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.LegArmor);
                if (legArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Leg] = new EquipmentElement(legArmor);
                }
            }

            // 武器：长剑+盾
            if (hero.BattleEquipment[EquipmentIndex.Weapon0].IsEmpty)
            {
                var sword = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.OneHandedWeapon &&
                    (item.StringId.Contains("sword") || item.StringId.Contains("blade")));
                if (sword != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon0] = new EquipmentElement(sword);
                }
            }

            // 盾牌
            if (hero.BattleEquipment[EquipmentIndex.Weapon1].IsEmpty)
            {
                var shield = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.Shield);
                if (shield != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon1] = new EquipmentElement(shield);
                }
            }

            // 骑枪
            if (hero.BattleEquipment[EquipmentIndex.Weapon2].IsEmpty)
            {
                var lance = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.Polearm &&
                    (item.StringId.Contains("lance") || item.StringId.Contains("spear")));
                if (lance != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon2] = new EquipmentElement(lance);
                }
            }
        }

        /// <summary>
        /// 应用随机贵族关系
        /// </summary>
        private static void ApplyRandomNobleRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    var lords = vlandiaKingdom.Lords.Where(h => h.IsLord && h != hero).ToList();
                    if (lords.Count > 0)
                    {
                        var randomLord = lords[new Random().Next(lords.Count)];
                        ChangeRelationAction.ApplyPlayerRelation(randomLord, relationChange);
                        OriginLog.Info($"[ApplyRandomNobleRelation] 与 {randomLord.Name} 关系 {relationChange}");
                    }
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRandomNobleRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用王室派系关系
        /// </summary>
        private static void ApplyRoyalFactionRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null && vlandiaKingdom.Leader != null)
                {
                    ChangeRelationAction.ApplyPlayerRelation(vlandiaKingdom.Leader, relationChange);
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRoyalFactionRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用村庄关系
        /// </summary>
        private static void ApplyVillageRelations(Hero hero, int relationChange)
        {
            try
            {
                OriginLog.Info($"[ApplyVillageRelations] 村庄关系变化: {relationChange}");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyVillageRelations] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 查找王国
        /// </summary>
        private static Kingdom FindKingdom(string kingdomId)
        {
            try
            {
                return Campaign.Current?.Kingdoms?.FirstOrDefault(k =>
                    k.StringId == kingdomId ||
                    k.StringId == $"kingdom_{kingdomId}" ||
                    (k.Culture != null && k.Culture.StringId == kingdomId));
            }
            catch
            {
                return null;
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace OriginSystemMod
{
    /// <summary>
    /// 瓦兰迪亚预设出身应用逻辑
    /// </summary>
    public static class VlandiaOriginSystem
    {
        /// <summary>
        /// 应用远征的骑士出身
        /// </summary>
        public static void ApplyExpeditionKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 开始应用远征的骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 3);
                PresetOriginSystem.GainRenown(hero, 30);
                PresetOriginSystem.AddGold(hero, 3500);

                // 设置初始瓦兰迪亚风格装备
                SetVlandiaInitialEquipment(hero, "expedition_knight");

                // 设置出生位置（根据Node2的誓言选择决定）
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                if (selectedNodes.ContainsKey("vla_node_expedition_oath"))
                {
                    string oath = selectedNodes["vla_node_expedition_oath"];
                    string startLocation = GetExpeditionKnightStartLocation(oath);
                    if (!string.IsNullOrEmpty(startLocation))
                    {
                        PresetOriginSystem.SetPresetOriginStartingLocation(party, startLocation);
                    }
                }
                else
                {
                    PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");
                }

                // 应用节点效果
                ApplyExpeditionKnightNode1(hero, selectedNodes);
                ApplyExpeditionKnightNode2(hero, clan, party, selectedNodes);
                ApplyExpeditionKnightNode3(hero, selectedNodes);
                ApplyExpeditionKnightNode4(hero, party, selectedNodes);

                // 创建哥哥NPC（固定存在）
                CreateExpeditionKnightBrother(hero, clan);

                OriginLog.Info("[ExpeditionKnight] 远征的骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[ExpeditionKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 根据誓言选择获取出生位置
        /// </summary>
        private static string GetExpeditionKnightStartLocation(string oath)
        {
            switch (oath)
            {
                case "kill_sea_raiders": // 杀1000海寇
                    return "nord"; // 诺德村子
                case "conquer_gyaz": // 远征古亚兹
                    return "aserai"; // 阿塞莱村子（靠近古亚兹）
                case "kill_battanian_lord": // 斩巴丹尼亚氏族
                    return "battania"; // 巴丹尼亚村子（肖农附近）
                case "recover_banner": // 寻回失旗
                    return "vlandia"; // 默认瓦兰迪亚
                default:
                    return "vlandia";
            }
        }

        /// <summary>
        /// 应用远征骑士Node1效果
        /// </summary>
        private static void ApplyExpeditionKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_fall")) return;

            string fall = nodes["vla_node_expedition_fall"];
            switch (fall)
            {
                case "erased_defeat":
                    PresetOriginSystem.AddSkill(hero, "tactics", 30);
                    PresetOriginSystem.AddSkill(hero, "leadership", 20);
                    ApplyRandomNobleRelation(hero, -30);
                    break;
                case "annexed":
                    PresetOriginSystem.AddSkill(hero, "charm", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "debt":
                    PresetOriginSystem.AddSkill(hero, "trade", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 2500);
                    break;
                case "political":
                    PresetOriginSystem.AddSkill(hero, "scouting", 20);
                    PresetOriginSystem.AddSkill(hero, "tactics", 20);
                    ApplyRoyalFactionRelation(hero, -30);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node2效果（誓言任务）
        /// </summary>
        private static void ApplyExpeditionKnightNode2(Hero hero, Clan clan, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_oath")) return;

            string oath = nodes["vla_node_expedition_oath"];
            
            // 创建誓言任务
            CreateExpeditionKnightOathQuest(hero, oath);
        }

        /// <summary>
        /// 应用远征骑士Node3效果
        /// </summary>
        private static void ApplyExpeditionKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_chivalry")) return;

            string chivalry = nodes["vla_node_expedition_chivalry"];
            switch (chivalry)
            {
                case "mercy": // 仁慈之誓
                    PresetOriginSystem.AddGold(hero, -2000);
                    break;
                case "valor": // 勇武之誓
                    // 开局多2-4精锐随骑（在Node4中处理）
                    break;
                case "prudence": // 谨慎之誓
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "cynical": // 犬儒之誓
                    PresetOriginSystem.AddGold(hero, 3000);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node4效果
        /// </summary>
        private static void ApplyExpeditionKnightNode4(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_division")) return;

            string division = nodes["vla_node_expedition_division"];
            switch (division)
            {
                case "brother_commands":
                    // 哥哥掌军，你主外交：哥哥偏战术/统御，你偏魅力/管理；随从偏步弩
                    break;
                case "you_command":
                    // 你掌军，哥哥做见证人：你拿骑兵核心，哥哥提供稳定后勤
                    break;
                case "joint_decision":
                    // 共同决策：队伍更均衡
                    break;
            }
        }

        /// <summary>
        /// 创建远征骑士的哥哥NPC
        /// </summary>
        private static void CreateExpeditionKnightBrother(Hero hero, Clan clan)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 需要创建哥哥NPC（待实现）");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建哥哥NPC失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 创建远征骑士的誓言任务
        /// </summary>
        private static void CreateExpeditionKnightOathQuest(Hero hero, string oath)
        {
            try
            {
                OriginLog.Info($"[ExpeditionKnight] 创建誓言任务: {oath}");
                // 任务系统需要单独实现，这里先记录
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建任务失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用堕落无赖骑士出身
        /// </summary>
        public static void ApplyDegradedRogueKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[DegradedRogueKnight] 开始应用堕落无赖骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 2);
                PresetOriginSystem.GainRenown(hero, 10);
                PresetOriginSystem.AddGold(hero, 4000);

                // 设置初始瓦兰迪亚风格装备（败落骑士风格）
                SetVlandiaInitialEquipment(hero, "degraded_rogue_knight");

                // 设置敌人关系
                ApplyDegradedRogueKnightEnemyRelations(hero);

                // 设置犯罪度（罪犯标记）
                ApplyDegradedRogueKnightCrimeStatus(hero);

                // 设置出生位置（瓦兰迪亚附近，但作为罪犯）
                PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");

                // 应用节点效果
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                ApplyDegradedRogueKnightNode1(hero, selectedNodes);
                ApplyDegradedRogueKnightNode2(hero, selectedNodes);
                ApplyDegradedRogueKnightNode3(hero, selectedNodes);
                ApplyDegradedRogueKnightNode4(hero, selectedNodes);

                OriginLog.Info("[DegradedRogueKnight] 堕落无赖骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[DegradedRogueKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的敌人关系
        /// </summary>
        private static void ApplyDegradedRogueKnightEnemyRelations(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                var empireKingdom = FindKingdom("kingdom_empire");
                var battaniaKingdom = FindKingdom("kingdom_battania");

                // 与瓦兰迪亚敌对
                if (vlandiaKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, vlandiaKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, vlandiaKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与瓦兰迪亚宣战");
                    }

                    foreach (var lord in vlandiaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与帝国敌对
                if (empireKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, empireKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, empireKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与帝国宣战");
                    }

                    foreach (var lord in empireKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与巴丹尼亚贵族关系-80（但不一定开战）
                if (battaniaKingdom != null)
                {
                    foreach (var lord in battaniaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                OriginLog.Info("[DegradedRogueKnight] 敌人关系设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置敌人关系失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的罪犯状态
        /// </summary>
        private static void ApplyDegradedRogueKnightCrimeStatus(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(vlandiaKingdom, 100);
                }

                var empireKingdom = FindKingdom("kingdom_empire");
                if (empireKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(empireKingdom, 100);
                }

                OriginLog.Info("[DegradedRogueKnight] 罪犯状态设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置罪犯状态失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士Node1效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_crime")) return;

            string crime = nodes["vla_node_degraded_crime"];
            switch (crime)
            {
                case "tyranny": // 暴虐之罪
                    ApplyVillageRelations(hero, -20);
                    break;
                case "indulgence": // 沉溺之罪
                    PresetOriginSystem.AddGold(hero, 3500);
                    break;
                case "filth": // 污秽之罪
                    break;
                case "conspiracy": // 阴谋之罪
                    break;
            }
        }

        /// <summary>
        /// 应用无赖骑士Node2效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode2(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_first_case")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node3效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_view")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node4效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode4(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_goal")) return;
        }

        /// <summary>
        /// 设置瓦兰迪亚初始装备
        /// </summary>
        private static void SetVlandiaInitialEquipment(Hero hero, string originType)
        {
            try
            {
                if (hero == null || hero.BattleEquipment == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] Hero或BattleEquipment为空");
                    return;
                }

                var allItems = MBObjectManager.Instance?.GetObjectTypeList<ItemObject>();
                if (allItems == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] 无法获取物品列表");
                    return;
                }

                if (originType == "expedition_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else if (originType == "degraded_rogue_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, false);
                }

                OriginLog.Info($"[SetVlandiaInitialEquipment] {originType} 装备设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[SetVlandiaInitialEquipment] 设置装备失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据档位设置瓦兰迪亚装备
        /// </summary>
        private static void SetVlandiaEquipmentByTier(Hero hero, IEnumerable<ItemObject> allItems, int tier, bool damaged)
        {
            var vlandiaItems = allItems.Where(item =>
                (item.Culture != null && item.Culture.StringId == "vlandia") ||
                item.StringId.Contains("vlandia") ||
                item.StringId.Contains("vlandian"))
                .ToList();

            // 身体护甲
            if (hero.BattleEquipment[EquipmentIndex.Body].IsEmpty)
            {
                var bodyArmor = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.BodyArmor &&
                    (tier == 2 ? item.StringId.Contains("t2") : tier == 3 ? item.StringId.Contains("t3") : item.StringId.Contains("t4")));
                if (bodyArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Body] = new EquipmentElement(bodyArmor);
                }
            }

            // 腿部护甲
            if (hero.BattleEquipment[EquipmentIndex.Leg].IsEmpty)
            {
                var legArmor = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.LegArmor);
                if (legArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Leg] = new EquipmentElement(legArmor);
                }
            }

            // 武器：长剑+盾
            if (hero.BattleEquipment[EquipmentIndex.Weapon0].IsEmpty)
            {
                var sword = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.OneHandedWeapon &&
                    (item.StringId.Contains("sword") || item.StringId.Contains("blade")));
                if (sword != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon0] = new EquipmentElement(sword);
                }
            }

            // 盾牌
            if (hero.BattleEquipment[EquipmentIndex.Weapon1].IsEmpty)
            {
                var shield = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.Shield);
                if (shield != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon1] = new EquipmentElement(shield);
                }
            }

            // 骑枪
            if (hero.BattleEquipment[EquipmentIndex.Weapon2].IsEmpty)
            {
                var lance = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.Polearm &&
                    (item.StringId.Contains("lance") || item.StringId.Contains("spear")));
                if (lance != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon2] = new EquipmentElement(lance);
                }
            }
        }

        /// <summary>
        /// 应用随机贵族关系
        /// </summary>
        private static void ApplyRandomNobleRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    var lords = vlandiaKingdom.Lords.Where(h => h.IsLord && h != hero).ToList();
                    if (lords.Count > 0)
                    {
                        var randomLord = lords[new Random().Next(lords.Count)];
                        ChangeRelationAction.ApplyPlayerRelation(randomLord, relationChange);
                        OriginLog.Info($"[ApplyRandomNobleRelation] 与 {randomLord.Name} 关系 {relationChange}");
                    }
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRandomNobleRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用王室派系关系
        /// </summary>
        private static void ApplyRoyalFactionRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null && vlandiaKingdom.Leader != null)
                {
                    ChangeRelationAction.ApplyPlayerRelation(vlandiaKingdom.Leader, relationChange);
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRoyalFactionRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用村庄关系
        /// </summary>
        private static void ApplyVillageRelations(Hero hero, int relationChange)
        {
            try
            {
                OriginLog.Info($"[ApplyVillageRelations] 村庄关系变化: {relationChange}");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyVillageRelations] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 查找王国
        /// </summary>
        private static Kingdom FindKingdom(string kingdomId)
        {
            try
            {
                return Campaign.Current?.Kingdoms?.FirstOrDefault(k =>
                    k.StringId == kingdomId ||
                    k.StringId == $"kingdom_{kingdomId}" ||
                    (k.Culture != null && k.Culture.StringId == kingdomId));
            }
            catch
            {
                return null;
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace OriginSystemMod
{
    /// <summary>
    /// 瓦兰迪亚预设出身应用逻辑
    /// </summary>
    public static class VlandiaOriginSystem
    {
        /// <summary>
        /// 应用远征的骑士出身
        /// </summary>
        public static void ApplyExpeditionKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 开始应用远征的骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 3);
                PresetOriginSystem.GainRenown(hero, 30);
                PresetOriginSystem.AddGold(hero, 3500);

                // 设置初始瓦兰迪亚风格装备
                SetVlandiaInitialEquipment(hero, "expedition_knight");

                // 设置出生位置（根据Node2的誓言选择决定）
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                if (selectedNodes.ContainsKey("vla_node_expedition_oath"))
                {
                    string oath = selectedNodes["vla_node_expedition_oath"];
                    string startLocation = GetExpeditionKnightStartLocation(oath);
                    if (!string.IsNullOrEmpty(startLocation))
                    {
                        PresetOriginSystem.SetPresetOriginStartingLocation(party, startLocation);
                    }
                }
                else
                {
                    PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");
                }

                // 应用节点效果
                ApplyExpeditionKnightNode1(hero, selectedNodes);
                ApplyExpeditionKnightNode2(hero, clan, party, selectedNodes);
                ApplyExpeditionKnightNode3(hero, selectedNodes);
                ApplyExpeditionKnightNode4(hero, party, selectedNodes);

                // 创建哥哥NPC（固定存在）
                CreateExpeditionKnightBrother(hero, clan);

                OriginLog.Info("[ExpeditionKnight] 远征的骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[ExpeditionKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 根据誓言选择获取出生位置
        /// </summary>
        private static string GetExpeditionKnightStartLocation(string oath)
        {
            switch (oath)
            {
                case "kill_sea_raiders": // 杀1000海寇
                    return "nord"; // 诺德村子
                case "conquer_gyaz": // 远征古亚兹
                    return "aserai"; // 阿塞莱村子（靠近古亚兹）
                case "kill_battanian_lord": // 斩巴丹尼亚氏族
                    return "battania"; // 巴丹尼亚村子（肖农附近）
                case "recover_banner": // 寻回失旗
                    return "vlandia"; // 默认瓦兰迪亚
                default:
                    return "vlandia";
            }
        }

        /// <summary>
        /// 应用远征骑士Node1效果
        /// </summary>
        private static void ApplyExpeditionKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_fall")) return;

            string fall = nodes["vla_node_expedition_fall"];
            switch (fall)
            {
                case "erased_defeat":
                    PresetOriginSystem.AddSkill(hero, "tactics", 30);
                    PresetOriginSystem.AddSkill(hero, "leadership", 20);
                    ApplyRandomNobleRelation(hero, -30);
                    break;
                case "annexed":
                    PresetOriginSystem.AddSkill(hero, "charm", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "debt":
                    PresetOriginSystem.AddSkill(hero, "trade", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 2500);
                    break;
                case "political":
                    PresetOriginSystem.AddSkill(hero, "scouting", 20);
                    PresetOriginSystem.AddSkill(hero, "tactics", 20);
                    ApplyRoyalFactionRelation(hero, -30);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node2效果（誓言任务）
        /// </summary>
        private static void ApplyExpeditionKnightNode2(Hero hero, Clan clan, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_oath")) return;

            string oath = nodes["vla_node_expedition_oath"];
            
            // 创建誓言任务
            CreateExpeditionKnightOathQuest(hero, oath);
        }

        /// <summary>
        /// 应用远征骑士Node3效果
        /// </summary>
        private static void ApplyExpeditionKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_chivalry")) return;

            string chivalry = nodes["vla_node_expedition_chivalry"];
            switch (chivalry)
            {
                case "mercy": // 仁慈之誓
                    PresetOriginSystem.AddGold(hero, -2000);
                    break;
                case "valor": // 勇武之誓
                    // 开局多2-4精锐随骑（在Node4中处理）
                    break;
                case "prudence": // 谨慎之誓
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "cynical": // 犬儒之誓
                    PresetOriginSystem.AddGold(hero, 3000);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node4效果
        /// </summary>
        private static void ApplyExpeditionKnightNode4(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_division")) return;

            string division = nodes["vla_node_expedition_division"];
            switch (division)
            {
                case "brother_commands":
                    // 哥哥掌军，你主外交：哥哥偏战术/统御，你偏魅力/管理；随从偏步弩
                    break;
                case "you_command":
                    // 你掌军，哥哥做见证人：你拿骑兵核心，哥哥提供稳定后勤
                    break;
                case "joint_decision":
                    // 共同决策：队伍更均衡
                    break;
            }
        }

        /// <summary>
        /// 创建远征骑士的哥哥NPC
        /// </summary>
        private static void CreateExpeditionKnightBrother(Hero hero, Clan clan)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 需要创建哥哥NPC（待实现）");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建哥哥NPC失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 创建远征骑士的誓言任务
        /// </summary>
        private static void CreateExpeditionKnightOathQuest(Hero hero, string oath)
        {
            try
            {
                OriginLog.Info($"[ExpeditionKnight] 创建誓言任务: {oath}");
                // 任务系统需要单独实现，这里先记录
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建任务失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用堕落无赖骑士出身
        /// </summary>
        public static void ApplyDegradedRogueKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[DegradedRogueKnight] 开始应用堕落无赖骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 2);
                PresetOriginSystem.GainRenown(hero, 10);
                PresetOriginSystem.AddGold(hero, 4000);

                // 设置初始瓦兰迪亚风格装备（败落骑士风格）
                SetVlandiaInitialEquipment(hero, "degraded_rogue_knight");

                // 设置敌人关系
                ApplyDegradedRogueKnightEnemyRelations(hero);

                // 设置犯罪度（罪犯标记）
                ApplyDegradedRogueKnightCrimeStatus(hero);

                // 设置出生位置（瓦兰迪亚附近，但作为罪犯）
                PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");

                // 应用节点效果
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                ApplyDegradedRogueKnightNode1(hero, selectedNodes);
                ApplyDegradedRogueKnightNode2(hero, selectedNodes);
                ApplyDegradedRogueKnightNode3(hero, selectedNodes);
                ApplyDegradedRogueKnightNode4(hero, selectedNodes);

                OriginLog.Info("[DegradedRogueKnight] 堕落无赖骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[DegradedRogueKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的敌人关系
        /// </summary>
        private static void ApplyDegradedRogueKnightEnemyRelations(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                var empireKingdom = FindKingdom("kingdom_empire");
                var battaniaKingdom = FindKingdom("kingdom_battania");

                // 与瓦兰迪亚敌对
                if (vlandiaKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, vlandiaKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, vlandiaKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与瓦兰迪亚宣战");
                    }

                    foreach (var lord in vlandiaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与帝国敌对
                if (empireKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, empireKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, empireKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与帝国宣战");
                    }

                    foreach (var lord in empireKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与巴丹尼亚贵族关系-80（但不一定开战）
                if (battaniaKingdom != null)
                {
                    foreach (var lord in battaniaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                OriginLog.Info("[DegradedRogueKnight] 敌人关系设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置敌人关系失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的罪犯状态
        /// </summary>
        private static void ApplyDegradedRogueKnightCrimeStatus(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(vlandiaKingdom, 100);
                }

                var empireKingdom = FindKingdom("kingdom_empire");
                if (empireKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(empireKingdom, 100);
                }

                OriginLog.Info("[DegradedRogueKnight] 罪犯状态设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置罪犯状态失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士Node1效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_crime")) return;

            string crime = nodes["vla_node_degraded_crime"];
            switch (crime)
            {
                case "tyranny": // 暴虐之罪
                    ApplyVillageRelations(hero, -20);
                    break;
                case "indulgence": // 沉溺之罪
                    PresetOriginSystem.AddGold(hero, 3500);
                    break;
                case "filth": // 污秽之罪
                    break;
                case "conspiracy": // 阴谋之罪
                    break;
            }
        }

        /// <summary>
        /// 应用无赖骑士Node2效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode2(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_first_case")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node3效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_view")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node4效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode4(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_goal")) return;
        }

        /// <summary>
        /// 设置瓦兰迪亚初始装备
        /// </summary>
        private static void SetVlandiaInitialEquipment(Hero hero, string originType)
        {
            try
            {
                if (hero == null || hero.BattleEquipment == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] Hero或BattleEquipment为空");
                    return;
                }

                var allItems = MBObjectManager.Instance?.GetObjectTypeList<ItemObject>();
                if (allItems == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] 无法获取物品列表");
                    return;
                }

                if (originType == "expedition_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else if (originType == "degraded_rogue_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, false);
                }

                OriginLog.Info($"[SetVlandiaInitialEquipment] {originType} 装备设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[SetVlandiaInitialEquipment] 设置装备失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据档位设置瓦兰迪亚装备
        /// </summary>
        private static void SetVlandiaEquipmentByTier(Hero hero, IEnumerable<ItemObject> allItems, int tier, bool damaged)
        {
            var vlandiaItems = allItems.Where(item =>
                (item.Culture != null && item.Culture.StringId == "vlandia") ||
                item.StringId.Contains("vlandia") ||
                item.StringId.Contains("vlandian"))
                .ToList();

            // 身体护甲
            if (hero.BattleEquipment[EquipmentIndex.Body].IsEmpty)
            {
                var bodyArmor = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.BodyArmor &&
                    (tier == 2 ? item.StringId.Contains("t2") : tier == 3 ? item.StringId.Contains("t3") : item.StringId.Contains("t4")));
                if (bodyArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Body] = new EquipmentElement(bodyArmor);
                }
            }

            // 腿部护甲
            if (hero.BattleEquipment[EquipmentIndex.Leg].IsEmpty)
            {
                var legArmor = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.LegArmor);
                if (legArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Leg] = new EquipmentElement(legArmor);
                }
            }

            // 武器：长剑+盾
            if (hero.BattleEquipment[EquipmentIndex.Weapon0].IsEmpty)
            {
                var sword = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.OneHandedWeapon &&
                    (item.StringId.Contains("sword") || item.StringId.Contains("blade")));
                if (sword != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon0] = new EquipmentElement(sword);
                }
            }

            // 盾牌
            if (hero.BattleEquipment[EquipmentIndex.Weapon1].IsEmpty)
            {
                var shield = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.Shield);
                if (shield != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon1] = new EquipmentElement(shield);
                }
            }

            // 骑枪
            if (hero.BattleEquipment[EquipmentIndex.Weapon2].IsEmpty)
            {
                var lance = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.Polearm &&
                    (item.StringId.Contains("lance") || item.StringId.Contains("spear")));
                if (lance != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon2] = new EquipmentElement(lance);
                }
            }
        }

        /// <summary>
        /// 应用随机贵族关系
        /// </summary>
        private static void ApplyRandomNobleRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    var lords = vlandiaKingdom.Lords.Where(h => h.IsLord && h != hero).ToList();
                    if (lords.Count > 0)
                    {
                        var randomLord = lords[new Random().Next(lords.Count)];
                        ChangeRelationAction.ApplyPlayerRelation(randomLord, relationChange);
                        OriginLog.Info($"[ApplyRandomNobleRelation] 与 {randomLord.Name} 关系 {relationChange}");
                    }
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRandomNobleRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用王室派系关系
        /// </summary>
        private static void ApplyRoyalFactionRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null && vlandiaKingdom.Leader != null)
                {
                    ChangeRelationAction.ApplyPlayerRelation(vlandiaKingdom.Leader, relationChange);
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRoyalFactionRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用村庄关系
        /// </summary>
        private static void ApplyVillageRelations(Hero hero, int relationChange)
        {
            try
            {
                OriginLog.Info($"[ApplyVillageRelations] 村庄关系变化: {relationChange}");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyVillageRelations] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 查找王国
        /// </summary>
        private static Kingdom FindKingdom(string kingdomId)
        {
            try
            {
                return Campaign.Current?.Kingdoms?.FirstOrDefault(k =>
                    k.StringId == kingdomId ||
                    k.StringId == $"kingdom_{kingdomId}" ||
                    (k.Culture != null && k.Culture.StringId == kingdomId));
            }
            catch
            {
                return null;
            }
        }
    }
}



using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace OriginSystemMod
{
    /// <summary>
    /// 瓦兰迪亚预设出身应用逻辑
    /// </summary>
    public static class VlandiaOriginSystem
    {
        /// <summary>
        /// 应用远征的骑士出身
        /// </summary>
        public static void ApplyExpeditionKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 开始应用远征的骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 3);
                PresetOriginSystem.GainRenown(hero, 30);
                PresetOriginSystem.AddGold(hero, 3500);

                // 设置初始瓦兰迪亚风格装备
                SetVlandiaInitialEquipment(hero, "expedition_knight");

                // 设置出生位置（根据Node2的誓言选择决定）
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                if (selectedNodes.ContainsKey("vla_node_expedition_oath"))
                {
                    string oath = selectedNodes["vla_node_expedition_oath"];
                    string startLocation = GetExpeditionKnightStartLocation(oath);
                    if (!string.IsNullOrEmpty(startLocation))
                    {
                        PresetOriginSystem.SetPresetOriginStartingLocation(party, startLocation);
                    }
                }
                else
                {
                    PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");
                }

                // 应用节点效果
                ApplyExpeditionKnightNode1(hero, selectedNodes);
                ApplyExpeditionKnightNode2(hero, clan, party, selectedNodes);
                ApplyExpeditionKnightNode3(hero, selectedNodes);
                ApplyExpeditionKnightNode4(hero, party, selectedNodes);

                // 创建哥哥NPC（固定存在）
                CreateExpeditionKnightBrother(hero, clan);

                OriginLog.Info("[ExpeditionKnight] 远征的骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[ExpeditionKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 根据誓言选择获取出生位置
        /// </summary>
        private static string GetExpeditionKnightStartLocation(string oath)
        {
            switch (oath)
            {
                case "kill_sea_raiders": // 杀1000海寇
                    return "nord"; // 诺德村子
                case "conquer_gyaz": // 远征古亚兹
                    return "aserai"; // 阿塞莱村子（靠近古亚兹）
                case "kill_battanian_lord": // 斩巴丹尼亚氏族
                    return "battania"; // 巴丹尼亚村子（肖农附近）
                case "recover_banner": // 寻回失旗
                    return "vlandia"; // 默认瓦兰迪亚
                default:
                    return "vlandia";
            }
        }

        /// <summary>
        /// 应用远征骑士Node1效果
        /// </summary>
        private static void ApplyExpeditionKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_fall")) return;

            string fall = nodes["vla_node_expedition_fall"];
            switch (fall)
            {
                case "erased_defeat":
                    PresetOriginSystem.AddSkill(hero, "tactics", 30);
                    PresetOriginSystem.AddSkill(hero, "leadership", 20);
                    ApplyRandomNobleRelation(hero, -30);
                    break;
                case "annexed":
                    PresetOriginSystem.AddSkill(hero, "charm", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "debt":
                    PresetOriginSystem.AddSkill(hero, "trade", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 2500);
                    break;
                case "political":
                    PresetOriginSystem.AddSkill(hero, "scouting", 20);
                    PresetOriginSystem.AddSkill(hero, "tactics", 20);
                    ApplyRoyalFactionRelation(hero, -30);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node2效果（誓言任务）
        /// </summary>
        private static void ApplyExpeditionKnightNode2(Hero hero, Clan clan, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_oath")) return;

            string oath = nodes["vla_node_expedition_oath"];
            
            // 创建誓言任务
            CreateExpeditionKnightOathQuest(hero, oath);
        }

        /// <summary>
        /// 应用远征骑士Node3效果
        /// </summary>
        private static void ApplyExpeditionKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_chivalry")) return;

            string chivalry = nodes["vla_node_expedition_chivalry"];
            switch (chivalry)
            {
                case "mercy": // 仁慈之誓
                    PresetOriginSystem.AddGold(hero, -2000);
                    break;
                case "valor": // 勇武之誓
                    // 开局多2-4精锐随骑（在Node4中处理）
                    break;
                case "prudence": // 谨慎之誓
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "cynical": // 犬儒之誓
                    PresetOriginSystem.AddGold(hero, 3000);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node4效果
        /// </summary>
        private static void ApplyExpeditionKnightNode4(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_division")) return;

            string division = nodes["vla_node_expedition_division"];
            switch (division)
            {
                case "brother_commands":
                    // 哥哥掌军，你主外交：哥哥偏战术/统御，你偏魅力/管理；随从偏步弩
                    break;
                case "you_command":
                    // 你掌军，哥哥做见证人：你拿骑兵核心，哥哥提供稳定后勤
                    break;
                case "joint_decision":
                    // 共同决策：队伍更均衡
                    break;
            }
        }

        /// <summary>
        /// 创建远征骑士的哥哥NPC
        /// </summary>
        private static void CreateExpeditionKnightBrother(Hero hero, Clan clan)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 需要创建哥哥NPC（待实现）");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建哥哥NPC失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 创建远征骑士的誓言任务
        /// </summary>
        private static void CreateExpeditionKnightOathQuest(Hero hero, string oath)
        {
            try
            {
                OriginLog.Info($"[ExpeditionKnight] 创建誓言任务: {oath}");
                // 任务系统需要单独实现，这里先记录
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建任务失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用堕落无赖骑士出身
        /// </summary>
        public static void ApplyDegradedRogueKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[DegradedRogueKnight] 开始应用堕落无赖骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 2);
                PresetOriginSystem.GainRenown(hero, 10);
                PresetOriginSystem.AddGold(hero, 4000);

                // 设置初始瓦兰迪亚风格装备（败落骑士风格）
                SetVlandiaInitialEquipment(hero, "degraded_rogue_knight");

                // 设置敌人关系
                ApplyDegradedRogueKnightEnemyRelations(hero);

                // 设置犯罪度（罪犯标记）
                ApplyDegradedRogueKnightCrimeStatus(hero);

                // 设置出生位置（瓦兰迪亚附近，但作为罪犯）
                PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");

                // 应用节点效果
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                ApplyDegradedRogueKnightNode1(hero, selectedNodes);
                ApplyDegradedRogueKnightNode2(hero, selectedNodes);
                ApplyDegradedRogueKnightNode3(hero, selectedNodes);
                ApplyDegradedRogueKnightNode4(hero, selectedNodes);

                OriginLog.Info("[DegradedRogueKnight] 堕落无赖骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[DegradedRogueKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的敌人关系
        /// </summary>
        private static void ApplyDegradedRogueKnightEnemyRelations(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                var empireKingdom = FindKingdom("kingdom_empire");
                var battaniaKingdom = FindKingdom("kingdom_battania");

                // 与瓦兰迪亚敌对
                if (vlandiaKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, vlandiaKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, vlandiaKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与瓦兰迪亚宣战");
                    }

                    foreach (var lord in vlandiaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与帝国敌对
                if (empireKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, empireKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, empireKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与帝国宣战");
                    }

                    foreach (var lord in empireKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与巴丹尼亚贵族关系-80（但不一定开战）
                if (battaniaKingdom != null)
                {
                    foreach (var lord in battaniaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                OriginLog.Info("[DegradedRogueKnight] 敌人关系设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置敌人关系失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的罪犯状态
        /// </summary>
        private static void ApplyDegradedRogueKnightCrimeStatus(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(vlandiaKingdom, 100);
                }

                var empireKingdom = FindKingdom("kingdom_empire");
                if (empireKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(empireKingdom, 100);
                }

                OriginLog.Info("[DegradedRogueKnight] 罪犯状态设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置罪犯状态失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士Node1效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_crime")) return;

            string crime = nodes["vla_node_degraded_crime"];
            switch (crime)
            {
                case "tyranny": // 暴虐之罪
                    ApplyVillageRelations(hero, -20);
                    break;
                case "indulgence": // 沉溺之罪
                    PresetOriginSystem.AddGold(hero, 3500);
                    break;
                case "filth": // 污秽之罪
                    break;
                case "conspiracy": // 阴谋之罪
                    break;
            }
        }

        /// <summary>
        /// 应用无赖骑士Node2效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode2(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_first_case")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node3效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_view")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node4效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode4(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_goal")) return;
        }

        /// <summary>
        /// 设置瓦兰迪亚初始装备
        /// </summary>
        private static void SetVlandiaInitialEquipment(Hero hero, string originType)
        {
            try
            {
                if (hero == null || hero.BattleEquipment == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] Hero或BattleEquipment为空");
                    return;
                }

                var allItems = MBObjectManager.Instance?.GetObjectTypeList<ItemObject>();
                if (allItems == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] 无法获取物品列表");
                    return;
                }

                if (originType == "expedition_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else if (originType == "degraded_rogue_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, false);
                }

                OriginLog.Info($"[SetVlandiaInitialEquipment] {originType} 装备设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[SetVlandiaInitialEquipment] 设置装备失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据档位设置瓦兰迪亚装备
        /// </summary>
        private static void SetVlandiaEquipmentByTier(Hero hero, IEnumerable<ItemObject> allItems, int tier, bool damaged)
        {
            var vlandiaItems = allItems.Where(item =>
                (item.Culture != null && item.Culture.StringId == "vlandia") ||
                item.StringId.Contains("vlandia") ||
                item.StringId.Contains("vlandian"))
                .ToList();

            // 身体护甲
            if (hero.BattleEquipment[EquipmentIndex.Body].IsEmpty)
            {
                var bodyArmor = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.BodyArmor &&
                    (tier == 2 ? item.StringId.Contains("t2") : tier == 3 ? item.StringId.Contains("t3") : item.StringId.Contains("t4")));
                if (bodyArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Body] = new EquipmentElement(bodyArmor);
                }
            }

            // 腿部护甲
            if (hero.BattleEquipment[EquipmentIndex.Leg].IsEmpty)
            {
                var legArmor = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.LegArmor);
                if (legArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Leg] = new EquipmentElement(legArmor);
                }
            }

            // 武器：长剑+盾
            if (hero.BattleEquipment[EquipmentIndex.Weapon0].IsEmpty)
            {
                var sword = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.OneHandedWeapon &&
                    (item.StringId.Contains("sword") || item.StringId.Contains("blade")));
                if (sword != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon0] = new EquipmentElement(sword);
                }
            }

            // 盾牌
            if (hero.BattleEquipment[EquipmentIndex.Weapon1].IsEmpty)
            {
                var shield = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.Shield);
                if (shield != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon1] = new EquipmentElement(shield);
                }
            }

            // 骑枪
            if (hero.BattleEquipment[EquipmentIndex.Weapon2].IsEmpty)
            {
                var lance = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.Polearm &&
                    (item.StringId.Contains("lance") || item.StringId.Contains("spear")));
                if (lance != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon2] = new EquipmentElement(lance);
                }
            }
        }

        /// <summary>
        /// 应用随机贵族关系
        /// </summary>
        private static void ApplyRandomNobleRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    var lords = vlandiaKingdom.Lords.Where(h => h.IsLord && h != hero).ToList();
                    if (lords.Count > 0)
                    {
                        var randomLord = lords[new Random().Next(lords.Count)];
                        ChangeRelationAction.ApplyPlayerRelation(randomLord, relationChange);
                        OriginLog.Info($"[ApplyRandomNobleRelation] 与 {randomLord.Name} 关系 {relationChange}");
                    }
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRandomNobleRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用王室派系关系
        /// </summary>
        private static void ApplyRoyalFactionRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null && vlandiaKingdom.Leader != null)
                {
                    ChangeRelationAction.ApplyPlayerRelation(vlandiaKingdom.Leader, relationChange);
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRoyalFactionRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用村庄关系
        /// </summary>
        private static void ApplyVillageRelations(Hero hero, int relationChange)
        {
            try
            {
                OriginLog.Info($"[ApplyVillageRelations] 村庄关系变化: {relationChange}");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyVillageRelations] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 查找王国
        /// </summary>
        private static Kingdom FindKingdom(string kingdomId)
        {
            try
            {
                return Campaign.Current?.Kingdoms?.FirstOrDefault(k =>
                    k.StringId == kingdomId ||
                    k.StringId == $"kingdom_{kingdomId}" ||
                    (k.Culture != null && k.Culture.StringId == kingdomId));
            }
            catch
            {
                return null;
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace OriginSystemMod
{
    /// <summary>
    /// 瓦兰迪亚预设出身应用逻辑
    /// </summary>
    public static class VlandiaOriginSystem
    {
        /// <summary>
        /// 应用远征的骑士出身
        /// </summary>
        public static void ApplyExpeditionKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 开始应用远征的骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 3);
                PresetOriginSystem.GainRenown(hero, 30);
                PresetOriginSystem.AddGold(hero, 3500);

                // 设置初始瓦兰迪亚风格装备
                SetVlandiaInitialEquipment(hero, "expedition_knight");

                // 设置出生位置（根据Node2的誓言选择决定）
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                if (selectedNodes.ContainsKey("vla_node_expedition_oath"))
                {
                    string oath = selectedNodes["vla_node_expedition_oath"];
                    string startLocation = GetExpeditionKnightStartLocation(oath);
                    if (!string.IsNullOrEmpty(startLocation))
                    {
                        PresetOriginSystem.SetPresetOriginStartingLocation(party, startLocation);
                    }
                }
                else
                {
                    PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");
                }

                // 应用节点效果
                ApplyExpeditionKnightNode1(hero, selectedNodes);
                ApplyExpeditionKnightNode2(hero, clan, party, selectedNodes);
                ApplyExpeditionKnightNode3(hero, selectedNodes);
                ApplyExpeditionKnightNode4(hero, party, selectedNodes);

                // 创建哥哥NPC（固定存在）
                CreateExpeditionKnightBrother(hero, clan);

                OriginLog.Info("[ExpeditionKnight] 远征的骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[ExpeditionKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 根据誓言选择获取出生位置
        /// </summary>
        private static string GetExpeditionKnightStartLocation(string oath)
        {
            switch (oath)
            {
                case "kill_sea_raiders": // 杀1000海寇
                    return "nord"; // 诺德村子
                case "conquer_gyaz": // 远征古亚兹
                    return "aserai"; // 阿塞莱村子（靠近古亚兹）
                case "kill_battanian_lord": // 斩巴丹尼亚氏族
                    return "battania"; // 巴丹尼亚村子（肖农附近）
                case "recover_banner": // 寻回失旗
                    return "vlandia"; // 默认瓦兰迪亚
                default:
                    return "vlandia";
            }
        }

        /// <summary>
        /// 应用远征骑士Node1效果
        /// </summary>
        private static void ApplyExpeditionKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_fall")) return;

            string fall = nodes["vla_node_expedition_fall"];
            switch (fall)
            {
                case "erased_defeat":
                    PresetOriginSystem.AddSkill(hero, "tactics", 30);
                    PresetOriginSystem.AddSkill(hero, "leadership", 20);
                    ApplyRandomNobleRelation(hero, -30);
                    break;
                case "annexed":
                    PresetOriginSystem.AddSkill(hero, "charm", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "debt":
                    PresetOriginSystem.AddSkill(hero, "trade", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 2500);
                    break;
                case "political":
                    PresetOriginSystem.AddSkill(hero, "scouting", 20);
                    PresetOriginSystem.AddSkill(hero, "tactics", 20);
                    ApplyRoyalFactionRelation(hero, -30);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node2效果（誓言任务）
        /// </summary>
        private static void ApplyExpeditionKnightNode2(Hero hero, Clan clan, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_oath")) return;

            string oath = nodes["vla_node_expedition_oath"];
            
            // 创建誓言任务
            CreateExpeditionKnightOathQuest(hero, oath);
        }

        /// <summary>
        /// 应用远征骑士Node3效果
        /// </summary>
        private static void ApplyExpeditionKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_chivalry")) return;

            string chivalry = nodes["vla_node_expedition_chivalry"];
            switch (chivalry)
            {
                case "mercy": // 仁慈之誓
                    PresetOriginSystem.AddGold(hero, -2000);
                    break;
                case "valor": // 勇武之誓
                    // 开局多2-4精锐随骑（在Node4中处理）
                    break;
                case "prudence": // 谨慎之誓
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "cynical": // 犬儒之誓
                    PresetOriginSystem.AddGold(hero, 3000);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node4效果
        /// </summary>
        private static void ApplyExpeditionKnightNode4(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_division")) return;

            string division = nodes["vla_node_expedition_division"];
            switch (division)
            {
                case "brother_commands":
                    // 哥哥掌军，你主外交：哥哥偏战术/统御，你偏魅力/管理；随从偏步弩
                    break;
                case "you_command":
                    // 你掌军，哥哥做见证人：你拿骑兵核心，哥哥提供稳定后勤
                    break;
                case "joint_decision":
                    // 共同决策：队伍更均衡
                    break;
            }
        }

        /// <summary>
        /// 创建远征骑士的哥哥NPC
        /// </summary>
        private static void CreateExpeditionKnightBrother(Hero hero, Clan clan)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 需要创建哥哥NPC（待实现）");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建哥哥NPC失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 创建远征骑士的誓言任务
        /// </summary>
        private static void CreateExpeditionKnightOathQuest(Hero hero, string oath)
        {
            try
            {
                OriginLog.Info($"[ExpeditionKnight] 创建誓言任务: {oath}");
                // 任务系统需要单独实现，这里先记录
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建任务失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用堕落无赖骑士出身
        /// </summary>
        public static void ApplyDegradedRogueKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[DegradedRogueKnight] 开始应用堕落无赖骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 2);
                PresetOriginSystem.GainRenown(hero, 10);
                PresetOriginSystem.AddGold(hero, 4000);

                // 设置初始瓦兰迪亚风格装备（败落骑士风格）
                SetVlandiaInitialEquipment(hero, "degraded_rogue_knight");

                // 设置敌人关系
                ApplyDegradedRogueKnightEnemyRelations(hero);

                // 设置犯罪度（罪犯标记）
                ApplyDegradedRogueKnightCrimeStatus(hero);

                // 设置出生位置（瓦兰迪亚附近，但作为罪犯）
                PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");

                // 应用节点效果
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                ApplyDegradedRogueKnightNode1(hero, selectedNodes);
                ApplyDegradedRogueKnightNode2(hero, selectedNodes);
                ApplyDegradedRogueKnightNode3(hero, selectedNodes);
                ApplyDegradedRogueKnightNode4(hero, selectedNodes);

                OriginLog.Info("[DegradedRogueKnight] 堕落无赖骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[DegradedRogueKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的敌人关系
        /// </summary>
        private static void ApplyDegradedRogueKnightEnemyRelations(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                var empireKingdom = FindKingdom("kingdom_empire");
                var battaniaKingdom = FindKingdom("kingdom_battania");

                // 与瓦兰迪亚敌对
                if (vlandiaKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, vlandiaKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, vlandiaKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与瓦兰迪亚宣战");
                    }

                    foreach (var lord in vlandiaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与帝国敌对
                if (empireKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, empireKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, empireKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与帝国宣战");
                    }

                    foreach (var lord in empireKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与巴丹尼亚贵族关系-80（但不一定开战）
                if (battaniaKingdom != null)
                {
                    foreach (var lord in battaniaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                OriginLog.Info("[DegradedRogueKnight] 敌人关系设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置敌人关系失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的罪犯状态
        /// </summary>
        private static void ApplyDegradedRogueKnightCrimeStatus(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(vlandiaKingdom, 100);
                }

                var empireKingdom = FindKingdom("kingdom_empire");
                if (empireKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(empireKingdom, 100);
                }

                OriginLog.Info("[DegradedRogueKnight] 罪犯状态设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置罪犯状态失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士Node1效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_crime")) return;

            string crime = nodes["vla_node_degraded_crime"];
            switch (crime)
            {
                case "tyranny": // 暴虐之罪
                    ApplyVillageRelations(hero, -20);
                    break;
                case "indulgence": // 沉溺之罪
                    PresetOriginSystem.AddGold(hero, 3500);
                    break;
                case "filth": // 污秽之罪
                    break;
                case "conspiracy": // 阴谋之罪
                    break;
            }
        }

        /// <summary>
        /// 应用无赖骑士Node2效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode2(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_first_case")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node3效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_view")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node4效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode4(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_goal")) return;
        }

        /// <summary>
        /// 设置瓦兰迪亚初始装备
        /// </summary>
        private static void SetVlandiaInitialEquipment(Hero hero, string originType)
        {
            try
            {
                if (hero == null || hero.BattleEquipment == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] Hero或BattleEquipment为空");
                    return;
                }

                var allItems = MBObjectManager.Instance?.GetObjectTypeList<ItemObject>();
                if (allItems == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] 无法获取物品列表");
                    return;
                }

                if (originType == "expedition_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else if (originType == "degraded_rogue_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, false);
                }

                OriginLog.Info($"[SetVlandiaInitialEquipment] {originType} 装备设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[SetVlandiaInitialEquipment] 设置装备失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据档位设置瓦兰迪亚装备
        /// </summary>
        private static void SetVlandiaEquipmentByTier(Hero hero, IEnumerable<ItemObject> allItems, int tier, bool damaged)
        {
            var vlandiaItems = allItems.Where(item =>
                (item.Culture != null && item.Culture.StringId == "vlandia") ||
                item.StringId.Contains("vlandia") ||
                item.StringId.Contains("vlandian"))
                .ToList();

            // 身体护甲
            if (hero.BattleEquipment[EquipmentIndex.Body].IsEmpty)
            {
                var bodyArmor = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.BodyArmor &&
                    (tier == 2 ? item.StringId.Contains("t2") : tier == 3 ? item.StringId.Contains("t3") : item.StringId.Contains("t4")));
                if (bodyArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Body] = new EquipmentElement(bodyArmor);
                }
            }

            // 腿部护甲
            if (hero.BattleEquipment[EquipmentIndex.Leg].IsEmpty)
            {
                var legArmor = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.LegArmor);
                if (legArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Leg] = new EquipmentElement(legArmor);
                }
            }

            // 武器：长剑+盾
            if (hero.BattleEquipment[EquipmentIndex.Weapon0].IsEmpty)
            {
                var sword = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.OneHandedWeapon &&
                    (item.StringId.Contains("sword") || item.StringId.Contains("blade")));
                if (sword != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon0] = new EquipmentElement(sword);
                }
            }

            // 盾牌
            if (hero.BattleEquipment[EquipmentIndex.Weapon1].IsEmpty)
            {
                var shield = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.Shield);
                if (shield != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon1] = new EquipmentElement(shield);
                }
            }

            // 骑枪
            if (hero.BattleEquipment[EquipmentIndex.Weapon2].IsEmpty)
            {
                var lance = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.Polearm &&
                    (item.StringId.Contains("lance") || item.StringId.Contains("spear")));
                if (lance != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon2] = new EquipmentElement(lance);
                }
            }
        }

        /// <summary>
        /// 应用随机贵族关系
        /// </summary>
        private static void ApplyRandomNobleRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    var lords = vlandiaKingdom.Lords.Where(h => h.IsLord && h != hero).ToList();
                    if (lords.Count > 0)
                    {
                        var randomLord = lords[new Random().Next(lords.Count)];
                        ChangeRelationAction.ApplyPlayerRelation(randomLord, relationChange);
                        OriginLog.Info($"[ApplyRandomNobleRelation] 与 {randomLord.Name} 关系 {relationChange}");
                    }
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRandomNobleRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用王室派系关系
        /// </summary>
        private static void ApplyRoyalFactionRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null && vlandiaKingdom.Leader != null)
                {
                    ChangeRelationAction.ApplyPlayerRelation(vlandiaKingdom.Leader, relationChange);
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRoyalFactionRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用村庄关系
        /// </summary>
        private static void ApplyVillageRelations(Hero hero, int relationChange)
        {
            try
            {
                OriginLog.Info($"[ApplyVillageRelations] 村庄关系变化: {relationChange}");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyVillageRelations] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 查找王国
        /// </summary>
        private static Kingdom FindKingdom(string kingdomId)
        {
            try
            {
                return Campaign.Current?.Kingdoms?.FirstOrDefault(k =>
                    k.StringId == kingdomId ||
                    k.StringId == $"kingdom_{kingdomId}" ||
                    (k.Culture != null && k.Culture.StringId == kingdomId));
            }
            catch
            {
                return null;
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace OriginSystemMod
{
    /// <summary>
    /// 瓦兰迪亚预设出身应用逻辑
    /// </summary>
    public static class VlandiaOriginSystem
    {
        /// <summary>
        /// 应用远征的骑士出身
        /// </summary>
        public static void ApplyExpeditionKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 开始应用远征的骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 3);
                PresetOriginSystem.GainRenown(hero, 30);
                PresetOriginSystem.AddGold(hero, 3500);

                // 设置初始瓦兰迪亚风格装备
                SetVlandiaInitialEquipment(hero, "expedition_knight");

                // 设置出生位置（根据Node2的誓言选择决定）
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                if (selectedNodes.ContainsKey("vla_node_expedition_oath"))
                {
                    string oath = selectedNodes["vla_node_expedition_oath"];
                    string startLocation = GetExpeditionKnightStartLocation(oath);
                    if (!string.IsNullOrEmpty(startLocation))
                    {
                        PresetOriginSystem.SetPresetOriginStartingLocation(party, startLocation);
                    }
                }
                else
                {
                    PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");
                }

                // 应用节点效果
                ApplyExpeditionKnightNode1(hero, selectedNodes);
                ApplyExpeditionKnightNode2(hero, clan, party, selectedNodes);
                ApplyExpeditionKnightNode3(hero, selectedNodes);
                ApplyExpeditionKnightNode4(hero, party, selectedNodes);

                // 创建哥哥NPC（固定存在）
                CreateExpeditionKnightBrother(hero, clan);

                OriginLog.Info("[ExpeditionKnight] 远征的骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[ExpeditionKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 根据誓言选择获取出生位置
        /// </summary>
        private static string GetExpeditionKnightStartLocation(string oath)
        {
            switch (oath)
            {
                case "kill_sea_raiders": // 杀1000海寇
                    return "nord"; // 诺德村子
                case "conquer_gyaz": // 远征古亚兹
                    return "aserai"; // 阿塞莱村子（靠近古亚兹）
                case "kill_battanian_lord": // 斩巴丹尼亚氏族
                    return "battania"; // 巴丹尼亚村子（肖农附近）
                case "recover_banner": // 寻回失旗
                    return "vlandia"; // 默认瓦兰迪亚
                default:
                    return "vlandia";
            }
        }

        /// <summary>
        /// 应用远征骑士Node1效果
        /// </summary>
        private static void ApplyExpeditionKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_fall")) return;

            string fall = nodes["vla_node_expedition_fall"];
            switch (fall)
            {
                case "erased_defeat":
                    PresetOriginSystem.AddSkill(hero, "tactics", 30);
                    PresetOriginSystem.AddSkill(hero, "leadership", 20);
                    ApplyRandomNobleRelation(hero, -30);
                    break;
                case "annexed":
                    PresetOriginSystem.AddSkill(hero, "charm", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "debt":
                    PresetOriginSystem.AddSkill(hero, "trade", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 2500);
                    break;
                case "political":
                    PresetOriginSystem.AddSkill(hero, "scouting", 20);
                    PresetOriginSystem.AddSkill(hero, "tactics", 20);
                    ApplyRoyalFactionRelation(hero, -30);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node2效果（誓言任务）
        /// </summary>
        private static void ApplyExpeditionKnightNode2(Hero hero, Clan clan, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_oath")) return;

            string oath = nodes["vla_node_expedition_oath"];
            
            // 创建誓言任务
            CreateExpeditionKnightOathQuest(hero, oath);
        }

        /// <summary>
        /// 应用远征骑士Node3效果
        /// </summary>
        private static void ApplyExpeditionKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_chivalry")) return;

            string chivalry = nodes["vla_node_expedition_chivalry"];
            switch (chivalry)
            {
                case "mercy": // 仁慈之誓
                    PresetOriginSystem.AddGold(hero, -2000);
                    break;
                case "valor": // 勇武之誓
                    // 开局多2-4精锐随骑（在Node4中处理）
                    break;
                case "prudence": // 谨慎之誓
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "cynical": // 犬儒之誓
                    PresetOriginSystem.AddGold(hero, 3000);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node4效果
        /// </summary>
        private static void ApplyExpeditionKnightNode4(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_division")) return;

            string division = nodes["vla_node_expedition_division"];
            switch (division)
            {
                case "brother_commands":
                    // 哥哥掌军，你主外交：哥哥偏战术/统御，你偏魅力/管理；随从偏步弩
                    break;
                case "you_command":
                    // 你掌军，哥哥做见证人：你拿骑兵核心，哥哥提供稳定后勤
                    break;
                case "joint_decision":
                    // 共同决策：队伍更均衡
                    break;
            }
        }

        /// <summary>
        /// 创建远征骑士的哥哥NPC
        /// </summary>
        private static void CreateExpeditionKnightBrother(Hero hero, Clan clan)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 需要创建哥哥NPC（待实现）");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建哥哥NPC失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 创建远征骑士的誓言任务
        /// </summary>
        private static void CreateExpeditionKnightOathQuest(Hero hero, string oath)
        {
            try
            {
                OriginLog.Info($"[ExpeditionKnight] 创建誓言任务: {oath}");
                // 任务系统需要单独实现，这里先记录
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建任务失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用堕落无赖骑士出身
        /// </summary>
        public static void ApplyDegradedRogueKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[DegradedRogueKnight] 开始应用堕落无赖骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 2);
                PresetOriginSystem.GainRenown(hero, 10);
                PresetOriginSystem.AddGold(hero, 4000);

                // 设置初始瓦兰迪亚风格装备（败落骑士风格）
                SetVlandiaInitialEquipment(hero, "degraded_rogue_knight");

                // 设置敌人关系
                ApplyDegradedRogueKnightEnemyRelations(hero);

                // 设置犯罪度（罪犯标记）
                ApplyDegradedRogueKnightCrimeStatus(hero);

                // 设置出生位置（瓦兰迪亚附近，但作为罪犯）
                PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");

                // 应用节点效果
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                ApplyDegradedRogueKnightNode1(hero, selectedNodes);
                ApplyDegradedRogueKnightNode2(hero, selectedNodes);
                ApplyDegradedRogueKnightNode3(hero, selectedNodes);
                ApplyDegradedRogueKnightNode4(hero, selectedNodes);

                OriginLog.Info("[DegradedRogueKnight] 堕落无赖骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[DegradedRogueKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的敌人关系
        /// </summary>
        private static void ApplyDegradedRogueKnightEnemyRelations(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                var empireKingdom = FindKingdom("kingdom_empire");
                var battaniaKingdom = FindKingdom("kingdom_battania");

                // 与瓦兰迪亚敌对
                if (vlandiaKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, vlandiaKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, vlandiaKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与瓦兰迪亚宣战");
                    }

                    foreach (var lord in vlandiaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与帝国敌对
                if (empireKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, empireKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, empireKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与帝国宣战");
                    }

                    foreach (var lord in empireKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与巴丹尼亚贵族关系-80（但不一定开战）
                if (battaniaKingdom != null)
                {
                    foreach (var lord in battaniaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                OriginLog.Info("[DegradedRogueKnight] 敌人关系设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置敌人关系失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的罪犯状态
        /// </summary>
        private static void ApplyDegradedRogueKnightCrimeStatus(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(vlandiaKingdom, 100);
                }

                var empireKingdom = FindKingdom("kingdom_empire");
                if (empireKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(empireKingdom, 100);
                }

                OriginLog.Info("[DegradedRogueKnight] 罪犯状态设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置罪犯状态失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士Node1效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_crime")) return;

            string crime = nodes["vla_node_degraded_crime"];
            switch (crime)
            {
                case "tyranny": // 暴虐之罪
                    ApplyVillageRelations(hero, -20);
                    break;
                case "indulgence": // 沉溺之罪
                    PresetOriginSystem.AddGold(hero, 3500);
                    break;
                case "filth": // 污秽之罪
                    break;
                case "conspiracy": // 阴谋之罪
                    break;
            }
        }

        /// <summary>
        /// 应用无赖骑士Node2效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode2(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_first_case")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node3效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_view")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node4效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode4(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_goal")) return;
        }

        /// <summary>
        /// 设置瓦兰迪亚初始装备
        /// </summary>
        private static void SetVlandiaInitialEquipment(Hero hero, string originType)
        {
            try
            {
                if (hero == null || hero.BattleEquipment == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] Hero或BattleEquipment为空");
                    return;
                }

                var allItems = MBObjectManager.Instance?.GetObjectTypeList<ItemObject>();
                if (allItems == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] 无法获取物品列表");
                    return;
                }

                if (originType == "expedition_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else if (originType == "degraded_rogue_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, false);
                }

                OriginLog.Info($"[SetVlandiaInitialEquipment] {originType} 装备设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[SetVlandiaInitialEquipment] 设置装备失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据档位设置瓦兰迪亚装备
        /// </summary>
        private static void SetVlandiaEquipmentByTier(Hero hero, IEnumerable<ItemObject> allItems, int tier, bool damaged)
        {
            var vlandiaItems = allItems.Where(item =>
                (item.Culture != null && item.Culture.StringId == "vlandia") ||
                item.StringId.Contains("vlandia") ||
                item.StringId.Contains("vlandian"))
                .ToList();

            // 身体护甲
            if (hero.BattleEquipment[EquipmentIndex.Body].IsEmpty)
            {
                var bodyArmor = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.BodyArmor &&
                    (tier == 2 ? item.StringId.Contains("t2") : tier == 3 ? item.StringId.Contains("t3") : item.StringId.Contains("t4")));
                if (bodyArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Body] = new EquipmentElement(bodyArmor);
                }
            }

            // 腿部护甲
            if (hero.BattleEquipment[EquipmentIndex.Leg].IsEmpty)
            {
                var legArmor = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.LegArmor);
                if (legArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Leg] = new EquipmentElement(legArmor);
                }
            }

            // 武器：长剑+盾
            if (hero.BattleEquipment[EquipmentIndex.Weapon0].IsEmpty)
            {
                var sword = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.OneHandedWeapon &&
                    (item.StringId.Contains("sword") || item.StringId.Contains("blade")));
                if (sword != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon0] = new EquipmentElement(sword);
                }
            }

            // 盾牌
            if (hero.BattleEquipment[EquipmentIndex.Weapon1].IsEmpty)
            {
                var shield = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.Shield);
                if (shield != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon1] = new EquipmentElement(shield);
                }
            }

            // 骑枪
            if (hero.BattleEquipment[EquipmentIndex.Weapon2].IsEmpty)
            {
                var lance = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.Polearm &&
                    (item.StringId.Contains("lance") || item.StringId.Contains("spear")));
                if (lance != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon2] = new EquipmentElement(lance);
                }
            }
        }

        /// <summary>
        /// 应用随机贵族关系
        /// </summary>
        private static void ApplyRandomNobleRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    var lords = vlandiaKingdom.Lords.Where(h => h.IsLord && h != hero).ToList();
                    if (lords.Count > 0)
                    {
                        var randomLord = lords[new Random().Next(lords.Count)];
                        ChangeRelationAction.ApplyPlayerRelation(randomLord, relationChange);
                        OriginLog.Info($"[ApplyRandomNobleRelation] 与 {randomLord.Name} 关系 {relationChange}");
                    }
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRandomNobleRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用王室派系关系
        /// </summary>
        private static void ApplyRoyalFactionRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null && vlandiaKingdom.Leader != null)
                {
                    ChangeRelationAction.ApplyPlayerRelation(vlandiaKingdom.Leader, relationChange);
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRoyalFactionRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用村庄关系
        /// </summary>
        private static void ApplyVillageRelations(Hero hero, int relationChange)
        {
            try
            {
                OriginLog.Info($"[ApplyVillageRelations] 村庄关系变化: {relationChange}");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyVillageRelations] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 查找王国
        /// </summary>
        private static Kingdom FindKingdom(string kingdomId)
        {
            try
            {
                return Campaign.Current?.Kingdoms?.FirstOrDefault(k =>
                    k.StringId == kingdomId ||
                    k.StringId == $"kingdom_{kingdomId}" ||
                    (k.Culture != null && k.Culture.StringId == kingdomId));
            }
            catch
            {
                return null;
            }
        }
    }
}


using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace OriginSystemMod
{
    /// <summary>
    /// 瓦兰迪亚预设出身应用逻辑
    /// </summary>
    public static class VlandiaOriginSystem
    {
        /// <summary>
        /// 应用远征的骑士出身
        /// </summary>
        public static void ApplyExpeditionKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 开始应用远征的骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 3);
                PresetOriginSystem.GainRenown(hero, 30);
                PresetOriginSystem.AddGold(hero, 3500);

                // 设置初始瓦兰迪亚风格装备
                SetVlandiaInitialEquipment(hero, "expedition_knight");

                // 设置出生位置（根据Node2的誓言选择决定）
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                if (selectedNodes.ContainsKey("vla_node_expedition_oath"))
                {
                    string oath = selectedNodes["vla_node_expedition_oath"];
                    string startLocation = GetExpeditionKnightStartLocation(oath);
                    if (!string.IsNullOrEmpty(startLocation))
                    {
                        PresetOriginSystem.SetPresetOriginStartingLocation(party, startLocation);
                    }
                }
                else
                {
                    PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");
                }

                // 应用节点效果
                ApplyExpeditionKnightNode1(hero, selectedNodes);
                ApplyExpeditionKnightNode2(hero, clan, party, selectedNodes);
                ApplyExpeditionKnightNode3(hero, selectedNodes);
                ApplyExpeditionKnightNode4(hero, party, selectedNodes);

                // 创建哥哥NPC（固定存在）
                CreateExpeditionKnightBrother(hero, clan);

                OriginLog.Info("[ExpeditionKnight] 远征的骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[ExpeditionKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 根据誓言选择获取出生位置
        /// </summary>
        private static string GetExpeditionKnightStartLocation(string oath)
        {
            switch (oath)
            {
                case "kill_sea_raiders": // 杀1000海寇
                    return "nord"; // 诺德村子
                case "conquer_gyaz": // 远征古亚兹
                    return "aserai"; // 阿塞莱村子（靠近古亚兹）
                case "kill_battanian_lord": // 斩巴丹尼亚氏族
                    return "battania"; // 巴丹尼亚村子（肖农附近）
                case "recover_banner": // 寻回失旗
                    return "vlandia"; // 默认瓦兰迪亚
                default:
                    return "vlandia";
            }
        }

        /// <summary>
        /// 应用远征骑士Node1效果
        /// </summary>
        private static void ApplyExpeditionKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_fall")) return;

            string fall = nodes["vla_node_expedition_fall"];
            switch (fall)
            {
                case "erased_defeat":
                    PresetOriginSystem.AddSkill(hero, "tactics", 30);
                    PresetOriginSystem.AddSkill(hero, "leadership", 20);
                    ApplyRandomNobleRelation(hero, -30);
                    break;
                case "annexed":
                    PresetOriginSystem.AddSkill(hero, "charm", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "debt":
                    PresetOriginSystem.AddSkill(hero, "trade", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 2500);
                    break;
                case "political":
                    PresetOriginSystem.AddSkill(hero, "scouting", 20);
                    PresetOriginSystem.AddSkill(hero, "tactics", 20);
                    ApplyRoyalFactionRelation(hero, -30);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node2效果（誓言任务）
        /// </summary>
        private static void ApplyExpeditionKnightNode2(Hero hero, Clan clan, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_oath")) return;

            string oath = nodes["vla_node_expedition_oath"];
            
            // 创建誓言任务
            CreateExpeditionKnightOathQuest(hero, oath);
        }

        /// <summary>
        /// 应用远征骑士Node3效果
        /// </summary>
        private static void ApplyExpeditionKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_chivalry")) return;

            string chivalry = nodes["vla_node_expedition_chivalry"];
            switch (chivalry)
            {
                case "mercy": // 仁慈之誓
                    PresetOriginSystem.AddGold(hero, -2000);
                    break;
                case "valor": // 勇武之誓
                    // 开局多2-4精锐随骑（在Node4中处理）
                    break;
                case "prudence": // 谨慎之誓
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "cynical": // 犬儒之誓
                    PresetOriginSystem.AddGold(hero, 3000);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node4效果
        /// </summary>
        private static void ApplyExpeditionKnightNode4(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_division")) return;

            string division = nodes["vla_node_expedition_division"];
            switch (division)
            {
                case "brother_commands":
                    // 哥哥掌军，你主外交：哥哥偏战术/统御，你偏魅力/管理；随从偏步弩
                    break;
                case "you_command":
                    // 你掌军，哥哥做见证人：你拿骑兵核心，哥哥提供稳定后勤
                    break;
                case "joint_decision":
                    // 共同决策：队伍更均衡
                    break;
            }
        }

        /// <summary>
        /// 创建远征骑士的哥哥NPC
        /// </summary>
        private static void CreateExpeditionKnightBrother(Hero hero, Clan clan)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 需要创建哥哥NPC（待实现）");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建哥哥NPC失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 创建远征骑士的誓言任务
        /// </summary>
        private static void CreateExpeditionKnightOathQuest(Hero hero, string oath)
        {
            try
            {
                OriginLog.Info($"[ExpeditionKnight] 创建誓言任务: {oath}");
                // 任务系统需要单独实现，这里先记录
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建任务失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用堕落无赖骑士出身
        /// </summary>
        public static void ApplyDegradedRogueKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[DegradedRogueKnight] 开始应用堕落无赖骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 2);
                PresetOriginSystem.GainRenown(hero, 10);
                PresetOriginSystem.AddGold(hero, 4000);

                // 设置初始瓦兰迪亚风格装备（败落骑士风格）
                SetVlandiaInitialEquipment(hero, "degraded_rogue_knight");

                // 设置敌人关系
                ApplyDegradedRogueKnightEnemyRelations(hero);

                // 设置犯罪度（罪犯标记）
                ApplyDegradedRogueKnightCrimeStatus(hero);

                // 设置出生位置（瓦兰迪亚附近，但作为罪犯）
                PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");

                // 应用节点效果
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                ApplyDegradedRogueKnightNode1(hero, selectedNodes);
                ApplyDegradedRogueKnightNode2(hero, selectedNodes);
                ApplyDegradedRogueKnightNode3(hero, selectedNodes);
                ApplyDegradedRogueKnightNode4(hero, selectedNodes);

                OriginLog.Info("[DegradedRogueKnight] 堕落无赖骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[DegradedRogueKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的敌人关系
        /// </summary>
        private static void ApplyDegradedRogueKnightEnemyRelations(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                var empireKingdom = FindKingdom("kingdom_empire");
                var battaniaKingdom = FindKingdom("kingdom_battania");

                // 与瓦兰迪亚敌对
                if (vlandiaKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, vlandiaKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, vlandiaKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与瓦兰迪亚宣战");
                    }

                    foreach (var lord in vlandiaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与帝国敌对
                if (empireKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, empireKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, empireKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与帝国宣战");
                    }

                    foreach (var lord in empireKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与巴丹尼亚贵族关系-80（但不一定开战）
                if (battaniaKingdom != null)
                {
                    foreach (var lord in battaniaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                OriginLog.Info("[DegradedRogueKnight] 敌人关系设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置敌人关系失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的罪犯状态
        /// </summary>
        private static void ApplyDegradedRogueKnightCrimeStatus(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(vlandiaKingdom, 100);
                }

                var empireKingdom = FindKingdom("kingdom_empire");
                if (empireKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(empireKingdom, 100);
                }

                OriginLog.Info("[DegradedRogueKnight] 罪犯状态设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置罪犯状态失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士Node1效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_crime")) return;

            string crime = nodes["vla_node_degraded_crime"];
            switch (crime)
            {
                case "tyranny": // 暴虐之罪
                    ApplyVillageRelations(hero, -20);
                    break;
                case "indulgence": // 沉溺之罪
                    PresetOriginSystem.AddGold(hero, 3500);
                    break;
                case "filth": // 污秽之罪
                    break;
                case "conspiracy": // 阴谋之罪
                    break;
            }
        }

        /// <summary>
        /// 应用无赖骑士Node2效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode2(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_first_case")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node3效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_view")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node4效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode4(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_goal")) return;
        }

        /// <summary>
        /// 设置瓦兰迪亚初始装备
        /// </summary>
        private static void SetVlandiaInitialEquipment(Hero hero, string originType)
        {
            try
            {
                if (hero == null || hero.BattleEquipment == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] Hero或BattleEquipment为空");
                    return;
                }

                var allItems = MBObjectManager.Instance?.GetObjectTypeList<ItemObject>();
                if (allItems == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] 无法获取物品列表");
                    return;
                }

                if (originType == "expedition_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else if (originType == "degraded_rogue_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, false);
                }

                OriginLog.Info($"[SetVlandiaInitialEquipment] {originType} 装备设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[SetVlandiaInitialEquipment] 设置装备失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据档位设置瓦兰迪亚装备
        /// </summary>
        private static void SetVlandiaEquipmentByTier(Hero hero, IEnumerable<ItemObject> allItems, int tier, bool damaged)
        {
            var vlandiaItems = allItems.Where(item =>
                (item.Culture != null && item.Culture.StringId == "vlandia") ||
                item.StringId.Contains("vlandia") ||
                item.StringId.Contains("vlandian"))
                .ToList();

            // 身体护甲
            if (hero.BattleEquipment[EquipmentIndex.Body].IsEmpty)
            {
                var bodyArmor = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.BodyArmor &&
                    (tier == 2 ? item.StringId.Contains("t2") : tier == 3 ? item.StringId.Contains("t3") : item.StringId.Contains("t4")));
                if (bodyArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Body] = new EquipmentElement(bodyArmor);
                }
            }

            // 腿部护甲
            if (hero.BattleEquipment[EquipmentIndex.Leg].IsEmpty)
            {
                var legArmor = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.LegArmor);
                if (legArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Leg] = new EquipmentElement(legArmor);
                }
            }

            // 武器：长剑+盾
            if (hero.BattleEquipment[EquipmentIndex.Weapon0].IsEmpty)
            {
                var sword = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.OneHandedWeapon &&
                    (item.StringId.Contains("sword") || item.StringId.Contains("blade")));
                if (sword != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon0] = new EquipmentElement(sword);
                }
            }

            // 盾牌
            if (hero.BattleEquipment[EquipmentIndex.Weapon1].IsEmpty)
            {
                var shield = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.Shield);
                if (shield != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon1] = new EquipmentElement(shield);
                }
            }

            // 骑枪
            if (hero.BattleEquipment[EquipmentIndex.Weapon2].IsEmpty)
            {
                var lance = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.Polearm &&
                    (item.StringId.Contains("lance") || item.StringId.Contains("spear")));
                if (lance != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon2] = new EquipmentElement(lance);
                }
            }
        }

        /// <summary>
        /// 应用随机贵族关系
        /// </summary>
        private static void ApplyRandomNobleRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    var lords = vlandiaKingdom.Lords.Where(h => h.IsLord && h != hero).ToList();
                    if (lords.Count > 0)
                    {
                        var randomLord = lords[new Random().Next(lords.Count)];
                        ChangeRelationAction.ApplyPlayerRelation(randomLord, relationChange);
                        OriginLog.Info($"[ApplyRandomNobleRelation] 与 {randomLord.Name} 关系 {relationChange}");
                    }
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRandomNobleRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用王室派系关系
        /// </summary>
        private static void ApplyRoyalFactionRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null && vlandiaKingdom.Leader != null)
                {
                    ChangeRelationAction.ApplyPlayerRelation(vlandiaKingdom.Leader, relationChange);
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRoyalFactionRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用村庄关系
        /// </summary>
        private static void ApplyVillageRelations(Hero hero, int relationChange)
        {
            try
            {
                OriginLog.Info($"[ApplyVillageRelations] 村庄关系变化: {relationChange}");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyVillageRelations] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 查找王国
        /// </summary>
        private static Kingdom FindKingdom(string kingdomId)
        {
            try
            {
                return Campaign.Current?.Kingdoms?.FirstOrDefault(k =>
                    k.StringId == kingdomId ||
                    k.StringId == $"kingdom_{kingdomId}" ||
                    (k.Culture != null && k.Culture.StringId == kingdomId));
            }
            catch
            {
                return null;
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace OriginSystemMod
{
    /// <summary>
    /// 瓦兰迪亚预设出身应用逻辑
    /// </summary>
    public static class VlandiaOriginSystem
    {
        /// <summary>
        /// 应用远征的骑士出身
        /// </summary>
        public static void ApplyExpeditionKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 开始应用远征的骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 3);
                PresetOriginSystem.GainRenown(hero, 30);
                PresetOriginSystem.AddGold(hero, 3500);

                // 设置初始瓦兰迪亚风格装备
                SetVlandiaInitialEquipment(hero, "expedition_knight");

                // 设置出生位置（根据Node2的誓言选择决定）
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                if (selectedNodes.ContainsKey("vla_node_expedition_oath"))
                {
                    string oath = selectedNodes["vla_node_expedition_oath"];
                    string startLocation = GetExpeditionKnightStartLocation(oath);
                    if (!string.IsNullOrEmpty(startLocation))
                    {
                        PresetOriginSystem.SetPresetOriginStartingLocation(party, startLocation);
                    }
                }
                else
                {
                    PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");
                }

                // 应用节点效果
                ApplyExpeditionKnightNode1(hero, selectedNodes);
                ApplyExpeditionKnightNode2(hero, clan, party, selectedNodes);
                ApplyExpeditionKnightNode3(hero, selectedNodes);
                ApplyExpeditionKnightNode4(hero, party, selectedNodes);

                // 创建哥哥NPC（固定存在）
                CreateExpeditionKnightBrother(hero, clan);

                OriginLog.Info("[ExpeditionKnight] 远征的骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[ExpeditionKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 根据誓言选择获取出生位置
        /// </summary>
        private static string GetExpeditionKnightStartLocation(string oath)
        {
            switch (oath)
            {
                case "kill_sea_raiders": // 杀1000海寇
                    return "nord"; // 诺德村子
                case "conquer_gyaz": // 远征古亚兹
                    return "aserai"; // 阿塞莱村子（靠近古亚兹）
                case "kill_battanian_lord": // 斩巴丹尼亚氏族
                    return "battania"; // 巴丹尼亚村子（肖农附近）
                case "recover_banner": // 寻回失旗
                    return "vlandia"; // 默认瓦兰迪亚
                default:
                    return "vlandia";
            }
        }

        /// <summary>
        /// 应用远征骑士Node1效果
        /// </summary>
        private static void ApplyExpeditionKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_fall")) return;

            string fall = nodes["vla_node_expedition_fall"];
            switch (fall)
            {
                case "erased_defeat":
                    PresetOriginSystem.AddSkill(hero, "tactics", 30);
                    PresetOriginSystem.AddSkill(hero, "leadership", 20);
                    ApplyRandomNobleRelation(hero, -30);
                    break;
                case "annexed":
                    PresetOriginSystem.AddSkill(hero, "charm", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "debt":
                    PresetOriginSystem.AddSkill(hero, "trade", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 2500);
                    break;
                case "political":
                    PresetOriginSystem.AddSkill(hero, "scouting", 20);
                    PresetOriginSystem.AddSkill(hero, "tactics", 20);
                    ApplyRoyalFactionRelation(hero, -30);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node2效果（誓言任务）
        /// </summary>
        private static void ApplyExpeditionKnightNode2(Hero hero, Clan clan, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_oath")) return;

            string oath = nodes["vla_node_expedition_oath"];
            
            // 创建誓言任务
            CreateExpeditionKnightOathQuest(hero, oath);
        }

        /// <summary>
        /// 应用远征骑士Node3效果
        /// </summary>
        private static void ApplyExpeditionKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_chivalry")) return;

            string chivalry = nodes["vla_node_expedition_chivalry"];
            switch (chivalry)
            {
                case "mercy": // 仁慈之誓
                    PresetOriginSystem.AddGold(hero, -2000);
                    break;
                case "valor": // 勇武之誓
                    // 开局多2-4精锐随骑（在Node4中处理）
                    break;
                case "prudence": // 谨慎之誓
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "cynical": // 犬儒之誓
                    PresetOriginSystem.AddGold(hero, 3000);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node4效果
        /// </summary>
        private static void ApplyExpeditionKnightNode4(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_division")) return;

            string division = nodes["vla_node_expedition_division"];
            switch (division)
            {
                case "brother_commands":
                    // 哥哥掌军，你主外交：哥哥偏战术/统御，你偏魅力/管理；随从偏步弩
                    break;
                case "you_command":
                    // 你掌军，哥哥做见证人：你拿骑兵核心，哥哥提供稳定后勤
                    break;
                case "joint_decision":
                    // 共同决策：队伍更均衡
                    break;
            }
        }

        /// <summary>
        /// 创建远征骑士的哥哥NPC
        /// </summary>
        private static void CreateExpeditionKnightBrother(Hero hero, Clan clan)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 需要创建哥哥NPC（待实现）");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建哥哥NPC失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 创建远征骑士的誓言任务
        /// </summary>
        private static void CreateExpeditionKnightOathQuest(Hero hero, string oath)
        {
            try
            {
                OriginLog.Info($"[ExpeditionKnight] 创建誓言任务: {oath}");
                // 任务系统需要单独实现，这里先记录
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建任务失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用堕落无赖骑士出身
        /// </summary>
        public static void ApplyDegradedRogueKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[DegradedRogueKnight] 开始应用堕落无赖骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 2);
                PresetOriginSystem.GainRenown(hero, 10);
                PresetOriginSystem.AddGold(hero, 4000);

                // 设置初始瓦兰迪亚风格装备（败落骑士风格）
                SetVlandiaInitialEquipment(hero, "degraded_rogue_knight");

                // 设置敌人关系
                ApplyDegradedRogueKnightEnemyRelations(hero);

                // 设置犯罪度（罪犯标记）
                ApplyDegradedRogueKnightCrimeStatus(hero);

                // 设置出生位置（瓦兰迪亚附近，但作为罪犯）
                PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");

                // 应用节点效果
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                ApplyDegradedRogueKnightNode1(hero, selectedNodes);
                ApplyDegradedRogueKnightNode2(hero, selectedNodes);
                ApplyDegradedRogueKnightNode3(hero, selectedNodes);
                ApplyDegradedRogueKnightNode4(hero, selectedNodes);

                OriginLog.Info("[DegradedRogueKnight] 堕落无赖骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[DegradedRogueKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的敌人关系
        /// </summary>
        private static void ApplyDegradedRogueKnightEnemyRelations(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                var empireKingdom = FindKingdom("kingdom_empire");
                var battaniaKingdom = FindKingdom("kingdom_battania");

                // 与瓦兰迪亚敌对
                if (vlandiaKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, vlandiaKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, vlandiaKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与瓦兰迪亚宣战");
                    }

                    foreach (var lord in vlandiaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与帝国敌对
                if (empireKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, empireKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, empireKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与帝国宣战");
                    }

                    foreach (var lord in empireKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与巴丹尼亚贵族关系-80（但不一定开战）
                if (battaniaKingdom != null)
                {
                    foreach (var lord in battaniaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                OriginLog.Info("[DegradedRogueKnight] 敌人关系设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置敌人关系失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的罪犯状态
        /// </summary>
        private static void ApplyDegradedRogueKnightCrimeStatus(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(vlandiaKingdom, 100);
                }

                var empireKingdom = FindKingdom("kingdom_empire");
                if (empireKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(empireKingdom, 100);
                }

                OriginLog.Info("[DegradedRogueKnight] 罪犯状态设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置罪犯状态失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士Node1效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_crime")) return;

            string crime = nodes["vla_node_degraded_crime"];
            switch (crime)
            {
                case "tyranny": // 暴虐之罪
                    ApplyVillageRelations(hero, -20);
                    break;
                case "indulgence": // 沉溺之罪
                    PresetOriginSystem.AddGold(hero, 3500);
                    break;
                case "filth": // 污秽之罪
                    break;
                case "conspiracy": // 阴谋之罪
                    break;
            }
        }

        /// <summary>
        /// 应用无赖骑士Node2效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode2(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_first_case")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node3效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_view")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node4效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode4(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_goal")) return;
        }

        /// <summary>
        /// 设置瓦兰迪亚初始装备
        /// </summary>
        private static void SetVlandiaInitialEquipment(Hero hero, string originType)
        {
            try
            {
                if (hero == null || hero.BattleEquipment == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] Hero或BattleEquipment为空");
                    return;
                }

                var allItems = MBObjectManager.Instance?.GetObjectTypeList<ItemObject>();
                if (allItems == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] 无法获取物品列表");
                    return;
                }

                if (originType == "expedition_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else if (originType == "degraded_rogue_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, false);
                }

                OriginLog.Info($"[SetVlandiaInitialEquipment] {originType} 装备设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[SetVlandiaInitialEquipment] 设置装备失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据档位设置瓦兰迪亚装备
        /// </summary>
        private static void SetVlandiaEquipmentByTier(Hero hero, IEnumerable<ItemObject> allItems, int tier, bool damaged)
        {
            var vlandiaItems = allItems.Where(item =>
                (item.Culture != null && item.Culture.StringId == "vlandia") ||
                item.StringId.Contains("vlandia") ||
                item.StringId.Contains("vlandian"))
                .ToList();

            // 身体护甲
            if (hero.BattleEquipment[EquipmentIndex.Body].IsEmpty)
            {
                var bodyArmor = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.BodyArmor &&
                    (tier == 2 ? item.StringId.Contains("t2") : tier == 3 ? item.StringId.Contains("t3") : item.StringId.Contains("t4")));
                if (bodyArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Body] = new EquipmentElement(bodyArmor);
                }
            }

            // 腿部护甲
            if (hero.BattleEquipment[EquipmentIndex.Leg].IsEmpty)
            {
                var legArmor = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.LegArmor);
                if (legArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Leg] = new EquipmentElement(legArmor);
                }
            }

            // 武器：长剑+盾
            if (hero.BattleEquipment[EquipmentIndex.Weapon0].IsEmpty)
            {
                var sword = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.OneHandedWeapon &&
                    (item.StringId.Contains("sword") || item.StringId.Contains("blade")));
                if (sword != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon0] = new EquipmentElement(sword);
                }
            }

            // 盾牌
            if (hero.BattleEquipment[EquipmentIndex.Weapon1].IsEmpty)
            {
                var shield = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.Shield);
                if (shield != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon1] = new EquipmentElement(shield);
                }
            }

            // 骑枪
            if (hero.BattleEquipment[EquipmentIndex.Weapon2].IsEmpty)
            {
                var lance = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.Polearm &&
                    (item.StringId.Contains("lance") || item.StringId.Contains("spear")));
                if (lance != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon2] = new EquipmentElement(lance);
                }
            }
        }

        /// <summary>
        /// 应用随机贵族关系
        /// </summary>
        private static void ApplyRandomNobleRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    var lords = vlandiaKingdom.Lords.Where(h => h.IsLord && h != hero).ToList();
                    if (lords.Count > 0)
                    {
                        var randomLord = lords[new Random().Next(lords.Count)];
                        ChangeRelationAction.ApplyPlayerRelation(randomLord, relationChange);
                        OriginLog.Info($"[ApplyRandomNobleRelation] 与 {randomLord.Name} 关系 {relationChange}");
                    }
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRandomNobleRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用王室派系关系
        /// </summary>
        private static void ApplyRoyalFactionRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null && vlandiaKingdom.Leader != null)
                {
                    ChangeRelationAction.ApplyPlayerRelation(vlandiaKingdom.Leader, relationChange);
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRoyalFactionRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用村庄关系
        /// </summary>
        private static void ApplyVillageRelations(Hero hero, int relationChange)
        {
            try
            {
                OriginLog.Info($"[ApplyVillageRelations] 村庄关系变化: {relationChange}");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyVillageRelations] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 查找王国
        /// </summary>
        private static Kingdom FindKingdom(string kingdomId)
        {
            try
            {
                return Campaign.Current?.Kingdoms?.FirstOrDefault(k =>
                    k.StringId == kingdomId ||
                    k.StringId == $"kingdom_{kingdomId}" ||
                    (k.Culture != null && k.Culture.StringId == kingdomId));
            }
            catch
            {
                return null;
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace OriginSystemMod
{
    /// <summary>
    /// 瓦兰迪亚预设出身应用逻辑
    /// </summary>
    public static class VlandiaOriginSystem
    {
        /// <summary>
        /// 应用远征的骑士出身
        /// </summary>
        public static void ApplyExpeditionKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 开始应用远征的骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 3);
                PresetOriginSystem.GainRenown(hero, 30);
                PresetOriginSystem.AddGold(hero, 3500);

                // 设置初始瓦兰迪亚风格装备
                SetVlandiaInitialEquipment(hero, "expedition_knight");

                // 设置出生位置（根据Node2的誓言选择决定）
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                if (selectedNodes.ContainsKey("vla_node_expedition_oath"))
                {
                    string oath = selectedNodes["vla_node_expedition_oath"];
                    string startLocation = GetExpeditionKnightStartLocation(oath);
                    if (!string.IsNullOrEmpty(startLocation))
                    {
                        PresetOriginSystem.SetPresetOriginStartingLocation(party, startLocation);
                    }
                }
                else
                {
                    PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");
                }

                // 应用节点效果
                ApplyExpeditionKnightNode1(hero, selectedNodes);
                ApplyExpeditionKnightNode2(hero, clan, party, selectedNodes);
                ApplyExpeditionKnightNode3(hero, selectedNodes);
                ApplyExpeditionKnightNode4(hero, party, selectedNodes);

                // 创建哥哥NPC（固定存在）
                CreateExpeditionKnightBrother(hero, clan);

                OriginLog.Info("[ExpeditionKnight] 远征的骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[ExpeditionKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 根据誓言选择获取出生位置
        /// </summary>
        private static string GetExpeditionKnightStartLocation(string oath)
        {
            switch (oath)
            {
                case "kill_sea_raiders": // 杀1000海寇
                    return "nord"; // 诺德村子
                case "conquer_gyaz": // 远征古亚兹
                    return "aserai"; // 阿塞莱村子（靠近古亚兹）
                case "kill_battanian_lord": // 斩巴丹尼亚氏族
                    return "battania"; // 巴丹尼亚村子（肖农附近）
                case "recover_banner": // 寻回失旗
                    return "vlandia"; // 默认瓦兰迪亚
                default:
                    return "vlandia";
            }
        }

        /// <summary>
        /// 应用远征骑士Node1效果
        /// </summary>
        private static void ApplyExpeditionKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_fall")) return;

            string fall = nodes["vla_node_expedition_fall"];
            switch (fall)
            {
                case "erased_defeat":
                    PresetOriginSystem.AddSkill(hero, "tactics", 30);
                    PresetOriginSystem.AddSkill(hero, "leadership", 20);
                    ApplyRandomNobleRelation(hero, -30);
                    break;
                case "annexed":
                    PresetOriginSystem.AddSkill(hero, "charm", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "debt":
                    PresetOriginSystem.AddSkill(hero, "trade", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 2500);
                    break;
                case "political":
                    PresetOriginSystem.AddSkill(hero, "scouting", 20);
                    PresetOriginSystem.AddSkill(hero, "tactics", 20);
                    ApplyRoyalFactionRelation(hero, -30);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node2效果（誓言任务）
        /// </summary>
        private static void ApplyExpeditionKnightNode2(Hero hero, Clan clan, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_oath")) return;

            string oath = nodes["vla_node_expedition_oath"];
            
            // 创建誓言任务
            CreateExpeditionKnightOathQuest(hero, oath);
        }

        /// <summary>
        /// 应用远征骑士Node3效果
        /// </summary>
        private static void ApplyExpeditionKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_chivalry")) return;

            string chivalry = nodes["vla_node_expedition_chivalry"];
            switch (chivalry)
            {
                case "mercy": // 仁慈之誓
                    PresetOriginSystem.AddGold(hero, -2000);
                    break;
                case "valor": // 勇武之誓
                    // 开局多2-4精锐随骑（在Node4中处理）
                    break;
                case "prudence": // 谨慎之誓
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "cynical": // 犬儒之誓
                    PresetOriginSystem.AddGold(hero, 3000);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node4效果
        /// </summary>
        private static void ApplyExpeditionKnightNode4(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_division")) return;

            string division = nodes["vla_node_expedition_division"];
            switch (division)
            {
                case "brother_commands":
                    // 哥哥掌军，你主外交：哥哥偏战术/统御，你偏魅力/管理；随从偏步弩
                    break;
                case "you_command":
                    // 你掌军，哥哥做见证人：你拿骑兵核心，哥哥提供稳定后勤
                    break;
                case "joint_decision":
                    // 共同决策：队伍更均衡
                    break;
            }
        }

        /// <summary>
        /// 创建远征骑士的哥哥NPC
        /// </summary>
        private static void CreateExpeditionKnightBrother(Hero hero, Clan clan)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 需要创建哥哥NPC（待实现）");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建哥哥NPC失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 创建远征骑士的誓言任务
        /// </summary>
        private static void CreateExpeditionKnightOathQuest(Hero hero, string oath)
        {
            try
            {
                OriginLog.Info($"[ExpeditionKnight] 创建誓言任务: {oath}");
                // 任务系统需要单独实现，这里先记录
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建任务失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用堕落无赖骑士出身
        /// </summary>
        public static void ApplyDegradedRogueKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[DegradedRogueKnight] 开始应用堕落无赖骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 2);
                PresetOriginSystem.GainRenown(hero, 10);
                PresetOriginSystem.AddGold(hero, 4000);

                // 设置初始瓦兰迪亚风格装备（败落骑士风格）
                SetVlandiaInitialEquipment(hero, "degraded_rogue_knight");

                // 设置敌人关系
                ApplyDegradedRogueKnightEnemyRelations(hero);

                // 设置犯罪度（罪犯标记）
                ApplyDegradedRogueKnightCrimeStatus(hero);

                // 设置出生位置（瓦兰迪亚附近，但作为罪犯）
                PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");

                // 应用节点效果
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                ApplyDegradedRogueKnightNode1(hero, selectedNodes);
                ApplyDegradedRogueKnightNode2(hero, selectedNodes);
                ApplyDegradedRogueKnightNode3(hero, selectedNodes);
                ApplyDegradedRogueKnightNode4(hero, selectedNodes);

                OriginLog.Info("[DegradedRogueKnight] 堕落无赖骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[DegradedRogueKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的敌人关系
        /// </summary>
        private static void ApplyDegradedRogueKnightEnemyRelations(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                var empireKingdom = FindKingdom("kingdom_empire");
                var battaniaKingdom = FindKingdom("kingdom_battania");

                // 与瓦兰迪亚敌对
                if (vlandiaKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, vlandiaKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, vlandiaKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与瓦兰迪亚宣战");
                    }

                    foreach (var lord in vlandiaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与帝国敌对
                if (empireKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, empireKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, empireKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与帝国宣战");
                    }

                    foreach (var lord in empireKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与巴丹尼亚贵族关系-80（但不一定开战）
                if (battaniaKingdom != null)
                {
                    foreach (var lord in battaniaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                OriginLog.Info("[DegradedRogueKnight] 敌人关系设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置敌人关系失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的罪犯状态
        /// </summary>
        private static void ApplyDegradedRogueKnightCrimeStatus(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(vlandiaKingdom, 100);
                }

                var empireKingdom = FindKingdom("kingdom_empire");
                if (empireKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(empireKingdom, 100);
                }

                OriginLog.Info("[DegradedRogueKnight] 罪犯状态设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置罪犯状态失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士Node1效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_crime")) return;

            string crime = nodes["vla_node_degraded_crime"];
            switch (crime)
            {
                case "tyranny": // 暴虐之罪
                    ApplyVillageRelations(hero, -20);
                    break;
                case "indulgence": // 沉溺之罪
                    PresetOriginSystem.AddGold(hero, 3500);
                    break;
                case "filth": // 污秽之罪
                    break;
                case "conspiracy": // 阴谋之罪
                    break;
            }
        }

        /// <summary>
        /// 应用无赖骑士Node2效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode2(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_first_case")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node3效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_view")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node4效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode4(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_goal")) return;
        }

        /// <summary>
        /// 设置瓦兰迪亚初始装备
        /// </summary>
        private static void SetVlandiaInitialEquipment(Hero hero, string originType)
        {
            try
            {
                if (hero == null || hero.BattleEquipment == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] Hero或BattleEquipment为空");
                    return;
                }

                var allItems = MBObjectManager.Instance?.GetObjectTypeList<ItemObject>();
                if (allItems == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] 无法获取物品列表");
                    return;
                }

                if (originType == "expedition_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else if (originType == "degraded_rogue_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, false);
                }

                OriginLog.Info($"[SetVlandiaInitialEquipment] {originType} 装备设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[SetVlandiaInitialEquipment] 设置装备失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据档位设置瓦兰迪亚装备
        /// </summary>
        private static void SetVlandiaEquipmentByTier(Hero hero, IEnumerable<ItemObject> allItems, int tier, bool damaged)
        {
            var vlandiaItems = allItems.Where(item =>
                (item.Culture != null && item.Culture.StringId == "vlandia") ||
                item.StringId.Contains("vlandia") ||
                item.StringId.Contains("vlandian"))
                .ToList();

            // 身体护甲
            if (hero.BattleEquipment[EquipmentIndex.Body].IsEmpty)
            {
                var bodyArmor = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.BodyArmor &&
                    (tier == 2 ? item.StringId.Contains("t2") : tier == 3 ? item.StringId.Contains("t3") : item.StringId.Contains("t4")));
                if (bodyArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Body] = new EquipmentElement(bodyArmor);
                }
            }

            // 腿部护甲
            if (hero.BattleEquipment[EquipmentIndex.Leg].IsEmpty)
            {
                var legArmor = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.LegArmor);
                if (legArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Leg] = new EquipmentElement(legArmor);
                }
            }

            // 武器：长剑+盾
            if (hero.BattleEquipment[EquipmentIndex.Weapon0].IsEmpty)
            {
                var sword = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.OneHandedWeapon &&
                    (item.StringId.Contains("sword") || item.StringId.Contains("blade")));
                if (sword != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon0] = new EquipmentElement(sword);
                }
            }

            // 盾牌
            if (hero.BattleEquipment[EquipmentIndex.Weapon1].IsEmpty)
            {
                var shield = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.Shield);
                if (shield != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon1] = new EquipmentElement(shield);
                }
            }

            // 骑枪
            if (hero.BattleEquipment[EquipmentIndex.Weapon2].IsEmpty)
            {
                var lance = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.Polearm &&
                    (item.StringId.Contains("lance") || item.StringId.Contains("spear")));
                if (lance != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon2] = new EquipmentElement(lance);
                }
            }
        }

        /// <summary>
        /// 应用随机贵族关系
        /// </summary>
        private static void ApplyRandomNobleRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    var lords = vlandiaKingdom.Lords.Where(h => h.IsLord && h != hero).ToList();
                    if (lords.Count > 0)
                    {
                        var randomLord = lords[new Random().Next(lords.Count)];
                        ChangeRelationAction.ApplyPlayerRelation(randomLord, relationChange);
                        OriginLog.Info($"[ApplyRandomNobleRelation] 与 {randomLord.Name} 关系 {relationChange}");
                    }
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRandomNobleRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用王室派系关系
        /// </summary>
        private static void ApplyRoyalFactionRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null && vlandiaKingdom.Leader != null)
                {
                    ChangeRelationAction.ApplyPlayerRelation(vlandiaKingdom.Leader, relationChange);
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRoyalFactionRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用村庄关系
        /// </summary>
        private static void ApplyVillageRelations(Hero hero, int relationChange)
        {
            try
            {
                OriginLog.Info($"[ApplyVillageRelations] 村庄关系变化: {relationChange}");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyVillageRelations] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 查找王国
        /// </summary>
        private static Kingdom FindKingdom(string kingdomId)
        {
            try
            {
                return Campaign.Current?.Kingdoms?.FirstOrDefault(k =>
                    k.StringId == kingdomId ||
                    k.StringId == $"kingdom_{kingdomId}" ||
                    (k.Culture != null && k.Culture.StringId == kingdomId));
            }
            catch
            {
                return null;
            }
        }
    }
}


using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace OriginSystemMod
{
    /// <summary>
    /// 瓦兰迪亚预设出身应用逻辑
    /// </summary>
    public static class VlandiaOriginSystem
    {
        /// <summary>
        /// 应用远征的骑士出身
        /// </summary>
        public static void ApplyExpeditionKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 开始应用远征的骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 3);
                PresetOriginSystem.GainRenown(hero, 30);
                PresetOriginSystem.AddGold(hero, 3500);

                // 设置初始瓦兰迪亚风格装备
                SetVlandiaInitialEquipment(hero, "expedition_knight");

                // 设置出生位置（根据Node2的誓言选择决定）
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                if (selectedNodes.ContainsKey("vla_node_expedition_oath"))
                {
                    string oath = selectedNodes["vla_node_expedition_oath"];
                    string startLocation = GetExpeditionKnightStartLocation(oath);
                    if (!string.IsNullOrEmpty(startLocation))
                    {
                        PresetOriginSystem.SetPresetOriginStartingLocation(party, startLocation);
                    }
                }
                else
                {
                    PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");
                }

                // 应用节点效果
                ApplyExpeditionKnightNode1(hero, selectedNodes);
                ApplyExpeditionKnightNode2(hero, clan, party, selectedNodes);
                ApplyExpeditionKnightNode3(hero, selectedNodes);
                ApplyExpeditionKnightNode4(hero, party, selectedNodes);

                // 创建哥哥NPC（固定存在）
                CreateExpeditionKnightBrother(hero, clan);

                OriginLog.Info("[ExpeditionKnight] 远征的骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[ExpeditionKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 根据誓言选择获取出生位置
        /// </summary>
        private static string GetExpeditionKnightStartLocation(string oath)
        {
            switch (oath)
            {
                case "kill_sea_raiders": // 杀1000海寇
                    return "nord"; // 诺德村子
                case "conquer_gyaz": // 远征古亚兹
                    return "aserai"; // 阿塞莱村子（靠近古亚兹）
                case "kill_battanian_lord": // 斩巴丹尼亚氏族
                    return "battania"; // 巴丹尼亚村子（肖农附近）
                case "recover_banner": // 寻回失旗
                    return "vlandia"; // 默认瓦兰迪亚
                default:
                    return "vlandia";
            }
        }

        /// <summary>
        /// 应用远征骑士Node1效果
        /// </summary>
        private static void ApplyExpeditionKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_fall")) return;

            string fall = nodes["vla_node_expedition_fall"];
            switch (fall)
            {
                case "erased_defeat":
                    PresetOriginSystem.AddSkill(hero, "tactics", 30);
                    PresetOriginSystem.AddSkill(hero, "leadership", 20);
                    ApplyRandomNobleRelation(hero, -30);
                    break;
                case "annexed":
                    PresetOriginSystem.AddSkill(hero, "charm", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "debt":
                    PresetOriginSystem.AddSkill(hero, "trade", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 2500);
                    break;
                case "political":
                    PresetOriginSystem.AddSkill(hero, "scouting", 20);
                    PresetOriginSystem.AddSkill(hero, "tactics", 20);
                    ApplyRoyalFactionRelation(hero, -30);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node2效果（誓言任务）
        /// </summary>
        private static void ApplyExpeditionKnightNode2(Hero hero, Clan clan, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_oath")) return;

            string oath = nodes["vla_node_expedition_oath"];
            
            // 创建誓言任务
            CreateExpeditionKnightOathQuest(hero, oath);
        }

        /// <summary>
        /// 应用远征骑士Node3效果
        /// </summary>
        private static void ApplyExpeditionKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_chivalry")) return;

            string chivalry = nodes["vla_node_expedition_chivalry"];
            switch (chivalry)
            {
                case "mercy": // 仁慈之誓
                    PresetOriginSystem.AddGold(hero, -2000);
                    break;
                case "valor": // 勇武之誓
                    // 开局多2-4精锐随骑（在Node4中处理）
                    break;
                case "prudence": // 谨慎之誓
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "cynical": // 犬儒之誓
                    PresetOriginSystem.AddGold(hero, 3000);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node4效果
        /// </summary>
        private static void ApplyExpeditionKnightNode4(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_division")) return;

            string division = nodes["vla_node_expedition_division"];
            switch (division)
            {
                case "brother_commands":
                    // 哥哥掌军，你主外交：哥哥偏战术/统御，你偏魅力/管理；随从偏步弩
                    break;
                case "you_command":
                    // 你掌军，哥哥做见证人：你拿骑兵核心，哥哥提供稳定后勤
                    break;
                case "joint_decision":
                    // 共同决策：队伍更均衡
                    break;
            }
        }

        /// <summary>
        /// 创建远征骑士的哥哥NPC
        /// </summary>
        private static void CreateExpeditionKnightBrother(Hero hero, Clan clan)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 需要创建哥哥NPC（待实现）");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建哥哥NPC失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 创建远征骑士的誓言任务
        /// </summary>
        private static void CreateExpeditionKnightOathQuest(Hero hero, string oath)
        {
            try
            {
                OriginLog.Info($"[ExpeditionKnight] 创建誓言任务: {oath}");
                // 任务系统需要单独实现，这里先记录
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建任务失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用堕落无赖骑士出身
        /// </summary>
        public static void ApplyDegradedRogueKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[DegradedRogueKnight] 开始应用堕落无赖骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 2);
                PresetOriginSystem.GainRenown(hero, 10);
                PresetOriginSystem.AddGold(hero, 4000);

                // 设置初始瓦兰迪亚风格装备（败落骑士风格）
                SetVlandiaInitialEquipment(hero, "degraded_rogue_knight");

                // 设置敌人关系
                ApplyDegradedRogueKnightEnemyRelations(hero);

                // 设置犯罪度（罪犯标记）
                ApplyDegradedRogueKnightCrimeStatus(hero);

                // 设置出生位置（瓦兰迪亚附近，但作为罪犯）
                PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");

                // 应用节点效果
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                ApplyDegradedRogueKnightNode1(hero, selectedNodes);
                ApplyDegradedRogueKnightNode2(hero, selectedNodes);
                ApplyDegradedRogueKnightNode3(hero, selectedNodes);
                ApplyDegradedRogueKnightNode4(hero, selectedNodes);

                OriginLog.Info("[DegradedRogueKnight] 堕落无赖骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[DegradedRogueKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的敌人关系
        /// </summary>
        private static void ApplyDegradedRogueKnightEnemyRelations(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                var empireKingdom = FindKingdom("kingdom_empire");
                var battaniaKingdom = FindKingdom("kingdom_battania");

                // 与瓦兰迪亚敌对
                if (vlandiaKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, vlandiaKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, vlandiaKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与瓦兰迪亚宣战");
                    }

                    foreach (var lord in vlandiaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与帝国敌对
                if (empireKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, empireKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, empireKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与帝国宣战");
                    }

                    foreach (var lord in empireKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与巴丹尼亚贵族关系-80（但不一定开战）
                if (battaniaKingdom != null)
                {
                    foreach (var lord in battaniaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                OriginLog.Info("[DegradedRogueKnight] 敌人关系设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置敌人关系失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的罪犯状态
        /// </summary>
        private static void ApplyDegradedRogueKnightCrimeStatus(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(vlandiaKingdom, 100);
                }

                var empireKingdom = FindKingdom("kingdom_empire");
                if (empireKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(empireKingdom, 100);
                }

                OriginLog.Info("[DegradedRogueKnight] 罪犯状态设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置罪犯状态失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士Node1效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_crime")) return;

            string crime = nodes["vla_node_degraded_crime"];
            switch (crime)
            {
                case "tyranny": // 暴虐之罪
                    ApplyVillageRelations(hero, -20);
                    break;
                case "indulgence": // 沉溺之罪
                    PresetOriginSystem.AddGold(hero, 3500);
                    break;
                case "filth": // 污秽之罪
                    break;
                case "conspiracy": // 阴谋之罪
                    break;
            }
        }

        /// <summary>
        /// 应用无赖骑士Node2效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode2(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_first_case")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node3效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_view")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node4效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode4(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_goal")) return;
        }

        /// <summary>
        /// 设置瓦兰迪亚初始装备
        /// </summary>
        private static void SetVlandiaInitialEquipment(Hero hero, string originType)
        {
            try
            {
                if (hero == null || hero.BattleEquipment == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] Hero或BattleEquipment为空");
                    return;
                }

                var allItems = MBObjectManager.Instance?.GetObjectTypeList<ItemObject>();
                if (allItems == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] 无法获取物品列表");
                    return;
                }

                if (originType == "expedition_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else if (originType == "degraded_rogue_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, false);
                }

                OriginLog.Info($"[SetVlandiaInitialEquipment] {originType} 装备设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[SetVlandiaInitialEquipment] 设置装备失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据档位设置瓦兰迪亚装备
        /// </summary>
        private static void SetVlandiaEquipmentByTier(Hero hero, IEnumerable<ItemObject> allItems, int tier, bool damaged)
        {
            var vlandiaItems = allItems.Where(item =>
                (item.Culture != null && item.Culture.StringId == "vlandia") ||
                item.StringId.Contains("vlandia") ||
                item.StringId.Contains("vlandian"))
                .ToList();

            // 身体护甲
            if (hero.BattleEquipment[EquipmentIndex.Body].IsEmpty)
            {
                var bodyArmor = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.BodyArmor &&
                    (tier == 2 ? item.StringId.Contains("t2") : tier == 3 ? item.StringId.Contains("t3") : item.StringId.Contains("t4")));
                if (bodyArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Body] = new EquipmentElement(bodyArmor);
                }
            }

            // 腿部护甲
            if (hero.BattleEquipment[EquipmentIndex.Leg].IsEmpty)
            {
                var legArmor = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.LegArmor);
                if (legArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Leg] = new EquipmentElement(legArmor);
                }
            }

            // 武器：长剑+盾
            if (hero.BattleEquipment[EquipmentIndex.Weapon0].IsEmpty)
            {
                var sword = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.OneHandedWeapon &&
                    (item.StringId.Contains("sword") || item.StringId.Contains("blade")));
                if (sword != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon0] = new EquipmentElement(sword);
                }
            }

            // 盾牌
            if (hero.BattleEquipment[EquipmentIndex.Weapon1].IsEmpty)
            {
                var shield = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.Shield);
                if (shield != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon1] = new EquipmentElement(shield);
                }
            }

            // 骑枪
            if (hero.BattleEquipment[EquipmentIndex.Weapon2].IsEmpty)
            {
                var lance = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.Polearm &&
                    (item.StringId.Contains("lance") || item.StringId.Contains("spear")));
                if (lance != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon2] = new EquipmentElement(lance);
                }
            }
        }

        /// <summary>
        /// 应用随机贵族关系
        /// </summary>
        private static void ApplyRandomNobleRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    var lords = vlandiaKingdom.Lords.Where(h => h.IsLord && h != hero).ToList();
                    if (lords.Count > 0)
                    {
                        var randomLord = lords[new Random().Next(lords.Count)];
                        ChangeRelationAction.ApplyPlayerRelation(randomLord, relationChange);
                        OriginLog.Info($"[ApplyRandomNobleRelation] 与 {randomLord.Name} 关系 {relationChange}");
                    }
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRandomNobleRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用王室派系关系
        /// </summary>
        private static void ApplyRoyalFactionRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null && vlandiaKingdom.Leader != null)
                {
                    ChangeRelationAction.ApplyPlayerRelation(vlandiaKingdom.Leader, relationChange);
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRoyalFactionRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用村庄关系
        /// </summary>
        private static void ApplyVillageRelations(Hero hero, int relationChange)
        {
            try
            {
                OriginLog.Info($"[ApplyVillageRelations] 村庄关系变化: {relationChange}");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyVillageRelations] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 查找王国
        /// </summary>
        private static Kingdom FindKingdom(string kingdomId)
        {
            try
            {
                return Campaign.Current?.Kingdoms?.FirstOrDefault(k =>
                    k.StringId == kingdomId ||
                    k.StringId == $"kingdom_{kingdomId}" ||
                    (k.Culture != null && k.Culture.StringId == kingdomId));
            }
            catch
            {
                return null;
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace OriginSystemMod
{
    /// <summary>
    /// 瓦兰迪亚预设出身应用逻辑
    /// </summary>
    public static class VlandiaOriginSystem
    {
        /// <summary>
        /// 应用远征的骑士出身
        /// </summary>
        public static void ApplyExpeditionKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 开始应用远征的骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 3);
                PresetOriginSystem.GainRenown(hero, 30);
                PresetOriginSystem.AddGold(hero, 3500);

                // 设置初始瓦兰迪亚风格装备
                SetVlandiaInitialEquipment(hero, "expedition_knight");

                // 设置出生位置（根据Node2的誓言选择决定）
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                if (selectedNodes.ContainsKey("vla_node_expedition_oath"))
                {
                    string oath = selectedNodes["vla_node_expedition_oath"];
                    string startLocation = GetExpeditionKnightStartLocation(oath);
                    if (!string.IsNullOrEmpty(startLocation))
                    {
                        PresetOriginSystem.SetPresetOriginStartingLocation(party, startLocation);
                    }
                }
                else
                {
                    PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");
                }

                // 应用节点效果
                ApplyExpeditionKnightNode1(hero, selectedNodes);
                ApplyExpeditionKnightNode2(hero, clan, party, selectedNodes);
                ApplyExpeditionKnightNode3(hero, selectedNodes);
                ApplyExpeditionKnightNode4(hero, party, selectedNodes);

                // 创建哥哥NPC（固定存在）
                CreateExpeditionKnightBrother(hero, clan);

                OriginLog.Info("[ExpeditionKnight] 远征的骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[ExpeditionKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 根据誓言选择获取出生位置
        /// </summary>
        private static string GetExpeditionKnightStartLocation(string oath)
        {
            switch (oath)
            {
                case "kill_sea_raiders": // 杀1000海寇
                    return "nord"; // 诺德村子
                case "conquer_gyaz": // 远征古亚兹
                    return "aserai"; // 阿塞莱村子（靠近古亚兹）
                case "kill_battanian_lord": // 斩巴丹尼亚氏族
                    return "battania"; // 巴丹尼亚村子（肖农附近）
                case "recover_banner": // 寻回失旗
                    return "vlandia"; // 默认瓦兰迪亚
                default:
                    return "vlandia";
            }
        }

        /// <summary>
        /// 应用远征骑士Node1效果
        /// </summary>
        private static void ApplyExpeditionKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_fall")) return;

            string fall = nodes["vla_node_expedition_fall"];
            switch (fall)
            {
                case "erased_defeat":
                    PresetOriginSystem.AddSkill(hero, "tactics", 30);
                    PresetOriginSystem.AddSkill(hero, "leadership", 20);
                    ApplyRandomNobleRelation(hero, -30);
                    break;
                case "annexed":
                    PresetOriginSystem.AddSkill(hero, "charm", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "debt":
                    PresetOriginSystem.AddSkill(hero, "trade", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 2500);
                    break;
                case "political":
                    PresetOriginSystem.AddSkill(hero, "scouting", 20);
                    PresetOriginSystem.AddSkill(hero, "tactics", 20);
                    ApplyRoyalFactionRelation(hero, -30);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node2效果（誓言任务）
        /// </summary>
        private static void ApplyExpeditionKnightNode2(Hero hero, Clan clan, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_oath")) return;

            string oath = nodes["vla_node_expedition_oath"];
            
            // 创建誓言任务
            CreateExpeditionKnightOathQuest(hero, oath);
        }

        /// <summary>
        /// 应用远征骑士Node3效果
        /// </summary>
        private static void ApplyExpeditionKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_chivalry")) return;

            string chivalry = nodes["vla_node_expedition_chivalry"];
            switch (chivalry)
            {
                case "mercy": // 仁慈之誓
                    PresetOriginSystem.AddGold(hero, -2000);
                    break;
                case "valor": // 勇武之誓
                    // 开局多2-4精锐随骑（在Node4中处理）
                    break;
                case "prudence": // 谨慎之誓
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "cynical": // 犬儒之誓
                    PresetOriginSystem.AddGold(hero, 3000);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node4效果
        /// </summary>
        private static void ApplyExpeditionKnightNode4(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_division")) return;

            string division = nodes["vla_node_expedition_division"];
            switch (division)
            {
                case "brother_commands":
                    // 哥哥掌军，你主外交：哥哥偏战术/统御，你偏魅力/管理；随从偏步弩
                    break;
                case "you_command":
                    // 你掌军，哥哥做见证人：你拿骑兵核心，哥哥提供稳定后勤
                    break;
                case "joint_decision":
                    // 共同决策：队伍更均衡
                    break;
            }
        }

        /// <summary>
        /// 创建远征骑士的哥哥NPC
        /// </summary>
        private static void CreateExpeditionKnightBrother(Hero hero, Clan clan)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 需要创建哥哥NPC（待实现）");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建哥哥NPC失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 创建远征骑士的誓言任务
        /// </summary>
        private static void CreateExpeditionKnightOathQuest(Hero hero, string oath)
        {
            try
            {
                OriginLog.Info($"[ExpeditionKnight] 创建誓言任务: {oath}");
                // 任务系统需要单独实现，这里先记录
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建任务失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用堕落无赖骑士出身
        /// </summary>
        public static void ApplyDegradedRogueKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[DegradedRogueKnight] 开始应用堕落无赖骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 2);
                PresetOriginSystem.GainRenown(hero, 10);
                PresetOriginSystem.AddGold(hero, 4000);

                // 设置初始瓦兰迪亚风格装备（败落骑士风格）
                SetVlandiaInitialEquipment(hero, "degraded_rogue_knight");

                // 设置敌人关系
                ApplyDegradedRogueKnightEnemyRelations(hero);

                // 设置犯罪度（罪犯标记）
                ApplyDegradedRogueKnightCrimeStatus(hero);

                // 设置出生位置（瓦兰迪亚附近，但作为罪犯）
                PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");

                // 应用节点效果
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                ApplyDegradedRogueKnightNode1(hero, selectedNodes);
                ApplyDegradedRogueKnightNode2(hero, selectedNodes);
                ApplyDegradedRogueKnightNode3(hero, selectedNodes);
                ApplyDegradedRogueKnightNode4(hero, selectedNodes);

                OriginLog.Info("[DegradedRogueKnight] 堕落无赖骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[DegradedRogueKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的敌人关系
        /// </summary>
        private static void ApplyDegradedRogueKnightEnemyRelations(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                var empireKingdom = FindKingdom("kingdom_empire");
                var battaniaKingdom = FindKingdom("kingdom_battania");

                // 与瓦兰迪亚敌对
                if (vlandiaKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, vlandiaKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, vlandiaKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与瓦兰迪亚宣战");
                    }

                    foreach (var lord in vlandiaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与帝国敌对
                if (empireKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, empireKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, empireKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与帝国宣战");
                    }

                    foreach (var lord in empireKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与巴丹尼亚贵族关系-80（但不一定开战）
                if (battaniaKingdom != null)
                {
                    foreach (var lord in battaniaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                OriginLog.Info("[DegradedRogueKnight] 敌人关系设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置敌人关系失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的罪犯状态
        /// </summary>
        private static void ApplyDegradedRogueKnightCrimeStatus(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(vlandiaKingdom, 100);
                }

                var empireKingdom = FindKingdom("kingdom_empire");
                if (empireKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(empireKingdom, 100);
                }

                OriginLog.Info("[DegradedRogueKnight] 罪犯状态设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置罪犯状态失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士Node1效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_crime")) return;

            string crime = nodes["vla_node_degraded_crime"];
            switch (crime)
            {
                case "tyranny": // 暴虐之罪
                    ApplyVillageRelations(hero, -20);
                    break;
                case "indulgence": // 沉溺之罪
                    PresetOriginSystem.AddGold(hero, 3500);
                    break;
                case "filth": // 污秽之罪
                    break;
                case "conspiracy": // 阴谋之罪
                    break;
            }
        }

        /// <summary>
        /// 应用无赖骑士Node2效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode2(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_first_case")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node3效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_view")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node4效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode4(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_goal")) return;
        }

        /// <summary>
        /// 设置瓦兰迪亚初始装备
        /// </summary>
        private static void SetVlandiaInitialEquipment(Hero hero, string originType)
        {
            try
            {
                if (hero == null || hero.BattleEquipment == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] Hero或BattleEquipment为空");
                    return;
                }

                var allItems = MBObjectManager.Instance?.GetObjectTypeList<ItemObject>();
                if (allItems == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] 无法获取物品列表");
                    return;
                }

                if (originType == "expedition_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else if (originType == "degraded_rogue_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, false);
                }

                OriginLog.Info($"[SetVlandiaInitialEquipment] {originType} 装备设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[SetVlandiaInitialEquipment] 设置装备失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据档位设置瓦兰迪亚装备
        /// </summary>
        private static void SetVlandiaEquipmentByTier(Hero hero, IEnumerable<ItemObject> allItems, int tier, bool damaged)
        {
            var vlandiaItems = allItems.Where(item =>
                (item.Culture != null && item.Culture.StringId == "vlandia") ||
                item.StringId.Contains("vlandia") ||
                item.StringId.Contains("vlandian"))
                .ToList();

            // 身体护甲
            if (hero.BattleEquipment[EquipmentIndex.Body].IsEmpty)
            {
                var bodyArmor = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.BodyArmor &&
                    (tier == 2 ? item.StringId.Contains("t2") : tier == 3 ? item.StringId.Contains("t3") : item.StringId.Contains("t4")));
                if (bodyArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Body] = new EquipmentElement(bodyArmor);
                }
            }

            // 腿部护甲
            if (hero.BattleEquipment[EquipmentIndex.Leg].IsEmpty)
            {
                var legArmor = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.LegArmor);
                if (legArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Leg] = new EquipmentElement(legArmor);
                }
            }

            // 武器：长剑+盾
            if (hero.BattleEquipment[EquipmentIndex.Weapon0].IsEmpty)
            {
                var sword = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.OneHandedWeapon &&
                    (item.StringId.Contains("sword") || item.StringId.Contains("blade")));
                if (sword != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon0] = new EquipmentElement(sword);
                }
            }

            // 盾牌
            if (hero.BattleEquipment[EquipmentIndex.Weapon1].IsEmpty)
            {
                var shield = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.Shield);
                if (shield != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon1] = new EquipmentElement(shield);
                }
            }

            // 骑枪
            if (hero.BattleEquipment[EquipmentIndex.Weapon2].IsEmpty)
            {
                var lance = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.Polearm &&
                    (item.StringId.Contains("lance") || item.StringId.Contains("spear")));
                if (lance != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon2] = new EquipmentElement(lance);
                }
            }
        }

        /// <summary>
        /// 应用随机贵族关系
        /// </summary>
        private static void ApplyRandomNobleRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    var lords = vlandiaKingdom.Lords.Where(h => h.IsLord && h != hero).ToList();
                    if (lords.Count > 0)
                    {
                        var randomLord = lords[new Random().Next(lords.Count)];
                        ChangeRelationAction.ApplyPlayerRelation(randomLord, relationChange);
                        OriginLog.Info($"[ApplyRandomNobleRelation] 与 {randomLord.Name} 关系 {relationChange}");
                    }
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRandomNobleRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用王室派系关系
        /// </summary>
        private static void ApplyRoyalFactionRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null && vlandiaKingdom.Leader != null)
                {
                    ChangeRelationAction.ApplyPlayerRelation(vlandiaKingdom.Leader, relationChange);
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRoyalFactionRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用村庄关系
        /// </summary>
        private static void ApplyVillageRelations(Hero hero, int relationChange)
        {
            try
            {
                OriginLog.Info($"[ApplyVillageRelations] 村庄关系变化: {relationChange}");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyVillageRelations] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 查找王国
        /// </summary>
        private static Kingdom FindKingdom(string kingdomId)
        {
            try
            {
                return Campaign.Current?.Kingdoms?.FirstOrDefault(k =>
                    k.StringId == kingdomId ||
                    k.StringId == $"kingdom_{kingdomId}" ||
                    (k.Culture != null && k.Culture.StringId == kingdomId));
            }
            catch
            {
                return null;
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace OriginSystemMod
{
    /// <summary>
    /// 瓦兰迪亚预设出身应用逻辑
    /// </summary>
    public static class VlandiaOriginSystem
    {
        /// <summary>
        /// 应用远征的骑士出身
        /// </summary>
        public static void ApplyExpeditionKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 开始应用远征的骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 3);
                PresetOriginSystem.GainRenown(hero, 30);
                PresetOriginSystem.AddGold(hero, 3500);

                // 设置初始瓦兰迪亚风格装备
                SetVlandiaInitialEquipment(hero, "expedition_knight");

                // 设置出生位置（根据Node2的誓言选择决定）
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                if (selectedNodes.ContainsKey("vla_node_expedition_oath"))
                {
                    string oath = selectedNodes["vla_node_expedition_oath"];
                    string startLocation = GetExpeditionKnightStartLocation(oath);
                    if (!string.IsNullOrEmpty(startLocation))
                    {
                        PresetOriginSystem.SetPresetOriginStartingLocation(party, startLocation);
                    }
                }
                else
                {
                    PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");
                }

                // 应用节点效果
                ApplyExpeditionKnightNode1(hero, selectedNodes);
                ApplyExpeditionKnightNode2(hero, clan, party, selectedNodes);
                ApplyExpeditionKnightNode3(hero, selectedNodes);
                ApplyExpeditionKnightNode4(hero, party, selectedNodes);

                // 创建哥哥NPC（固定存在）
                CreateExpeditionKnightBrother(hero, clan);

                OriginLog.Info("[ExpeditionKnight] 远征的骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[ExpeditionKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 根据誓言选择获取出生位置
        /// </summary>
        private static string GetExpeditionKnightStartLocation(string oath)
        {
            switch (oath)
            {
                case "kill_sea_raiders": // 杀1000海寇
                    return "nord"; // 诺德村子
                case "conquer_gyaz": // 远征古亚兹
                    return "aserai"; // 阿塞莱村子（靠近古亚兹）
                case "kill_battanian_lord": // 斩巴丹尼亚氏族
                    return "battania"; // 巴丹尼亚村子（肖农附近）
                case "recover_banner": // 寻回失旗
                    return "vlandia"; // 默认瓦兰迪亚
                default:
                    return "vlandia";
            }
        }

        /// <summary>
        /// 应用远征骑士Node1效果
        /// </summary>
        private static void ApplyExpeditionKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_fall")) return;

            string fall = nodes["vla_node_expedition_fall"];
            switch (fall)
            {
                case "erased_defeat":
                    PresetOriginSystem.AddSkill(hero, "tactics", 30);
                    PresetOriginSystem.AddSkill(hero, "leadership", 20);
                    ApplyRandomNobleRelation(hero, -30);
                    break;
                case "annexed":
                    PresetOriginSystem.AddSkill(hero, "charm", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "debt":
                    PresetOriginSystem.AddSkill(hero, "trade", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 2500);
                    break;
                case "political":
                    PresetOriginSystem.AddSkill(hero, "scouting", 20);
                    PresetOriginSystem.AddSkill(hero, "tactics", 20);
                    ApplyRoyalFactionRelation(hero, -30);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node2效果（誓言任务）
        /// </summary>
        private static void ApplyExpeditionKnightNode2(Hero hero, Clan clan, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_oath")) return;

            string oath = nodes["vla_node_expedition_oath"];
            
            // 创建誓言任务
            CreateExpeditionKnightOathQuest(hero, oath);
        }

        /// <summary>
        /// 应用远征骑士Node3效果
        /// </summary>
        private static void ApplyExpeditionKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_chivalry")) return;

            string chivalry = nodes["vla_node_expedition_chivalry"];
            switch (chivalry)
            {
                case "mercy": // 仁慈之誓
                    PresetOriginSystem.AddGold(hero, -2000);
                    break;
                case "valor": // 勇武之誓
                    // 开局多2-4精锐随骑（在Node4中处理）
                    break;
                case "prudence": // 谨慎之誓
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "cynical": // 犬儒之誓
                    PresetOriginSystem.AddGold(hero, 3000);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node4效果
        /// </summary>
        private static void ApplyExpeditionKnightNode4(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_division")) return;

            string division = nodes["vla_node_expedition_division"];
            switch (division)
            {
                case "brother_commands":
                    // 哥哥掌军，你主外交：哥哥偏战术/统御，你偏魅力/管理；随从偏步弩
                    break;
                case "you_command":
                    // 你掌军，哥哥做见证人：你拿骑兵核心，哥哥提供稳定后勤
                    break;
                case "joint_decision":
                    // 共同决策：队伍更均衡
                    break;
            }
        }

        /// <summary>
        /// 创建远征骑士的哥哥NPC
        /// </summary>
        private static void CreateExpeditionKnightBrother(Hero hero, Clan clan)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 需要创建哥哥NPC（待实现）");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建哥哥NPC失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 创建远征骑士的誓言任务
        /// </summary>
        private static void CreateExpeditionKnightOathQuest(Hero hero, string oath)
        {
            try
            {
                OriginLog.Info($"[ExpeditionKnight] 创建誓言任务: {oath}");
                // 任务系统需要单独实现，这里先记录
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建任务失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用堕落无赖骑士出身
        /// </summary>
        public static void ApplyDegradedRogueKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[DegradedRogueKnight] 开始应用堕落无赖骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 2);
                PresetOriginSystem.GainRenown(hero, 10);
                PresetOriginSystem.AddGold(hero, 4000);

                // 设置初始瓦兰迪亚风格装备（败落骑士风格）
                SetVlandiaInitialEquipment(hero, "degraded_rogue_knight");

                // 设置敌人关系
                ApplyDegradedRogueKnightEnemyRelations(hero);

                // 设置犯罪度（罪犯标记）
                ApplyDegradedRogueKnightCrimeStatus(hero);

                // 设置出生位置（瓦兰迪亚附近，但作为罪犯）
                PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");

                // 应用节点效果
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                ApplyDegradedRogueKnightNode1(hero, selectedNodes);
                ApplyDegradedRogueKnightNode2(hero, selectedNodes);
                ApplyDegradedRogueKnightNode3(hero, selectedNodes);
                ApplyDegradedRogueKnightNode4(hero, selectedNodes);

                OriginLog.Info("[DegradedRogueKnight] 堕落无赖骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[DegradedRogueKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的敌人关系
        /// </summary>
        private static void ApplyDegradedRogueKnightEnemyRelations(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                var empireKingdom = FindKingdom("kingdom_empire");
                var battaniaKingdom = FindKingdom("kingdom_battania");

                // 与瓦兰迪亚敌对
                if (vlandiaKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, vlandiaKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, vlandiaKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与瓦兰迪亚宣战");
                    }

                    foreach (var lord in vlandiaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与帝国敌对
                if (empireKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, empireKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, empireKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与帝国宣战");
                    }

                    foreach (var lord in empireKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与巴丹尼亚贵族关系-80（但不一定开战）
                if (battaniaKingdom != null)
                {
                    foreach (var lord in battaniaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                OriginLog.Info("[DegradedRogueKnight] 敌人关系设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置敌人关系失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的罪犯状态
        /// </summary>
        private static void ApplyDegradedRogueKnightCrimeStatus(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(vlandiaKingdom, 100);
                }

                var empireKingdom = FindKingdom("kingdom_empire");
                if (empireKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(empireKingdom, 100);
                }

                OriginLog.Info("[DegradedRogueKnight] 罪犯状态设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置罪犯状态失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士Node1效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_crime")) return;

            string crime = nodes["vla_node_degraded_crime"];
            switch (crime)
            {
                case "tyranny": // 暴虐之罪
                    ApplyVillageRelations(hero, -20);
                    break;
                case "indulgence": // 沉溺之罪
                    PresetOriginSystem.AddGold(hero, 3500);
                    break;
                case "filth": // 污秽之罪
                    break;
                case "conspiracy": // 阴谋之罪
                    break;
            }
        }

        /// <summary>
        /// 应用无赖骑士Node2效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode2(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_first_case")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node3效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_view")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node4效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode4(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_goal")) return;
        }

        /// <summary>
        /// 设置瓦兰迪亚初始装备
        /// </summary>
        private static void SetVlandiaInitialEquipment(Hero hero, string originType)
        {
            try
            {
                if (hero == null || hero.BattleEquipment == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] Hero或BattleEquipment为空");
                    return;
                }

                var allItems = MBObjectManager.Instance?.GetObjectTypeList<ItemObject>();
                if (allItems == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] 无法获取物品列表");
                    return;
                }

                if (originType == "expedition_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else if (originType == "degraded_rogue_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, false);
                }

                OriginLog.Info($"[SetVlandiaInitialEquipment] {originType} 装备设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[SetVlandiaInitialEquipment] 设置装备失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据档位设置瓦兰迪亚装备
        /// </summary>
        private static void SetVlandiaEquipmentByTier(Hero hero, IEnumerable<ItemObject> allItems, int tier, bool damaged)
        {
            var vlandiaItems = allItems.Where(item =>
                (item.Culture != null && item.Culture.StringId == "vlandia") ||
                item.StringId.Contains("vlandia") ||
                item.StringId.Contains("vlandian"))
                .ToList();

            // 身体护甲
            if (hero.BattleEquipment[EquipmentIndex.Body].IsEmpty)
            {
                var bodyArmor = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.BodyArmor &&
                    (tier == 2 ? item.StringId.Contains("t2") : tier == 3 ? item.StringId.Contains("t3") : item.StringId.Contains("t4")));
                if (bodyArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Body] = new EquipmentElement(bodyArmor);
                }
            }

            // 腿部护甲
            if (hero.BattleEquipment[EquipmentIndex.Leg].IsEmpty)
            {
                var legArmor = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.LegArmor);
                if (legArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Leg] = new EquipmentElement(legArmor);
                }
            }

            // 武器：长剑+盾
            if (hero.BattleEquipment[EquipmentIndex.Weapon0].IsEmpty)
            {
                var sword = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.OneHandedWeapon &&
                    (item.StringId.Contains("sword") || item.StringId.Contains("blade")));
                if (sword != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon0] = new EquipmentElement(sword);
                }
            }

            // 盾牌
            if (hero.BattleEquipment[EquipmentIndex.Weapon1].IsEmpty)
            {
                var shield = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.Shield);
                if (shield != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon1] = new EquipmentElement(shield);
                }
            }

            // 骑枪
            if (hero.BattleEquipment[EquipmentIndex.Weapon2].IsEmpty)
            {
                var lance = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.Polearm &&
                    (item.StringId.Contains("lance") || item.StringId.Contains("spear")));
                if (lance != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon2] = new EquipmentElement(lance);
                }
            }
        }

        /// <summary>
        /// 应用随机贵族关系
        /// </summary>
        private static void ApplyRandomNobleRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    var lords = vlandiaKingdom.Lords.Where(h => h.IsLord && h != hero).ToList();
                    if (lords.Count > 0)
                    {
                        var randomLord = lords[new Random().Next(lords.Count)];
                        ChangeRelationAction.ApplyPlayerRelation(randomLord, relationChange);
                        OriginLog.Info($"[ApplyRandomNobleRelation] 与 {randomLord.Name} 关系 {relationChange}");
                    }
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRandomNobleRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用王室派系关系
        /// </summary>
        private static void ApplyRoyalFactionRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null && vlandiaKingdom.Leader != null)
                {
                    ChangeRelationAction.ApplyPlayerRelation(vlandiaKingdom.Leader, relationChange);
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRoyalFactionRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用村庄关系
        /// </summary>
        private static void ApplyVillageRelations(Hero hero, int relationChange)
        {
            try
            {
                OriginLog.Info($"[ApplyVillageRelations] 村庄关系变化: {relationChange}");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyVillageRelations] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 查找王国
        /// </summary>
        private static Kingdom FindKingdom(string kingdomId)
        {
            try
            {
                return Campaign.Current?.Kingdoms?.FirstOrDefault(k =>
                    k.StringId == kingdomId ||
                    k.StringId == $"kingdom_{kingdomId}" ||
                    (k.Culture != null && k.Culture.StringId == kingdomId));
            }
            catch
            {
                return null;
            }
        }
    }
}



using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace OriginSystemMod
{
    /// <summary>
    /// 瓦兰迪亚预设出身应用逻辑
    /// </summary>
    public static class VlandiaOriginSystem
    {
        /// <summary>
        /// 应用远征的骑士出身
        /// </summary>
        public static void ApplyExpeditionKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 开始应用远征的骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 3);
                PresetOriginSystem.GainRenown(hero, 30);
                PresetOriginSystem.AddGold(hero, 3500);

                // 设置初始瓦兰迪亚风格装备
                SetVlandiaInitialEquipment(hero, "expedition_knight");

                // 设置出生位置（根据Node2的誓言选择决定）
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                if (selectedNodes.ContainsKey("vla_node_expedition_oath"))
                {
                    string oath = selectedNodes["vla_node_expedition_oath"];
                    string startLocation = GetExpeditionKnightStartLocation(oath);
                    if (!string.IsNullOrEmpty(startLocation))
                    {
                        PresetOriginSystem.SetPresetOriginStartingLocation(party, startLocation);
                    }
                }
                else
                {
                    PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");
                }

                // 应用节点效果
                ApplyExpeditionKnightNode1(hero, selectedNodes);
                ApplyExpeditionKnightNode2(hero, clan, party, selectedNodes);
                ApplyExpeditionKnightNode3(hero, selectedNodes);
                ApplyExpeditionKnightNode4(hero, party, selectedNodes);

                // 创建哥哥NPC（固定存在）
                CreateExpeditionKnightBrother(hero, clan);

                OriginLog.Info("[ExpeditionKnight] 远征的骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[ExpeditionKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 根据誓言选择获取出生位置
        /// </summary>
        private static string GetExpeditionKnightStartLocation(string oath)
        {
            switch (oath)
            {
                case "kill_sea_raiders": // 杀1000海寇
                    return "nord"; // 诺德村子
                case "conquer_gyaz": // 远征古亚兹
                    return "aserai"; // 阿塞莱村子（靠近古亚兹）
                case "kill_battanian_lord": // 斩巴丹尼亚氏族
                    return "battania"; // 巴丹尼亚村子（肖农附近）
                case "recover_banner": // 寻回失旗
                    return "vlandia"; // 默认瓦兰迪亚
                default:
                    return "vlandia";
            }
        }

        /// <summary>
        /// 应用远征骑士Node1效果
        /// </summary>
        private static void ApplyExpeditionKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_fall")) return;

            string fall = nodes["vla_node_expedition_fall"];
            switch (fall)
            {
                case "erased_defeat":
                    PresetOriginSystem.AddSkill(hero, "tactics", 30);
                    PresetOriginSystem.AddSkill(hero, "leadership", 20);
                    ApplyRandomNobleRelation(hero, -30);
                    break;
                case "annexed":
                    PresetOriginSystem.AddSkill(hero, "charm", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "debt":
                    PresetOriginSystem.AddSkill(hero, "trade", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 2500);
                    break;
                case "political":
                    PresetOriginSystem.AddSkill(hero, "scouting", 20);
                    PresetOriginSystem.AddSkill(hero, "tactics", 20);
                    ApplyRoyalFactionRelation(hero, -30);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node2效果（誓言任务）
        /// </summary>
        private static void ApplyExpeditionKnightNode2(Hero hero, Clan clan, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_oath")) return;

            string oath = nodes["vla_node_expedition_oath"];
            
            // 创建誓言任务
            CreateExpeditionKnightOathQuest(hero, oath);
        }

        /// <summary>
        /// 应用远征骑士Node3效果
        /// </summary>
        private static void ApplyExpeditionKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_chivalry")) return;

            string chivalry = nodes["vla_node_expedition_chivalry"];
            switch (chivalry)
            {
                case "mercy": // 仁慈之誓
                    PresetOriginSystem.AddGold(hero, -2000);
                    break;
                case "valor": // 勇武之誓
                    // 开局多2-4精锐随骑（在Node4中处理）
                    break;
                case "prudence": // 谨慎之誓
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "cynical": // 犬儒之誓
                    PresetOriginSystem.AddGold(hero, 3000);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node4效果
        /// </summary>
        private static void ApplyExpeditionKnightNode4(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_division")) return;

            string division = nodes["vla_node_expedition_division"];
            switch (division)
            {
                case "brother_commands":
                    // 哥哥掌军，你主外交：哥哥偏战术/统御，你偏魅力/管理；随从偏步弩
                    break;
                case "you_command":
                    // 你掌军，哥哥做见证人：你拿骑兵核心，哥哥提供稳定后勤
                    break;
                case "joint_decision":
                    // 共同决策：队伍更均衡
                    break;
            }
        }

        /// <summary>
        /// 创建远征骑士的哥哥NPC
        /// </summary>
        private static void CreateExpeditionKnightBrother(Hero hero, Clan clan)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 需要创建哥哥NPC（待实现）");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建哥哥NPC失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 创建远征骑士的誓言任务
        /// </summary>
        private static void CreateExpeditionKnightOathQuest(Hero hero, string oath)
        {
            try
            {
                OriginLog.Info($"[ExpeditionKnight] 创建誓言任务: {oath}");
                // 任务系统需要单独实现，这里先记录
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建任务失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用堕落无赖骑士出身
        /// </summary>
        public static void ApplyDegradedRogueKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[DegradedRogueKnight] 开始应用堕落无赖骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 2);
                PresetOriginSystem.GainRenown(hero, 10);
                PresetOriginSystem.AddGold(hero, 4000);

                // 设置初始瓦兰迪亚风格装备（败落骑士风格）
                SetVlandiaInitialEquipment(hero, "degraded_rogue_knight");

                // 设置敌人关系
                ApplyDegradedRogueKnightEnemyRelations(hero);

                // 设置犯罪度（罪犯标记）
                ApplyDegradedRogueKnightCrimeStatus(hero);

                // 设置出生位置（瓦兰迪亚附近，但作为罪犯）
                PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");

                // 应用节点效果
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                ApplyDegradedRogueKnightNode1(hero, selectedNodes);
                ApplyDegradedRogueKnightNode2(hero, selectedNodes);
                ApplyDegradedRogueKnightNode3(hero, selectedNodes);
                ApplyDegradedRogueKnightNode4(hero, selectedNodes);

                OriginLog.Info("[DegradedRogueKnight] 堕落无赖骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[DegradedRogueKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的敌人关系
        /// </summary>
        private static void ApplyDegradedRogueKnightEnemyRelations(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                var empireKingdom = FindKingdom("kingdom_empire");
                var battaniaKingdom = FindKingdom("kingdom_battania");

                // 与瓦兰迪亚敌对
                if (vlandiaKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, vlandiaKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, vlandiaKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与瓦兰迪亚宣战");
                    }

                    foreach (var lord in vlandiaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与帝国敌对
                if (empireKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, empireKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, empireKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与帝国宣战");
                    }

                    foreach (var lord in empireKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与巴丹尼亚贵族关系-80（但不一定开战）
                if (battaniaKingdom != null)
                {
                    foreach (var lord in battaniaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                OriginLog.Info("[DegradedRogueKnight] 敌人关系设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置敌人关系失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的罪犯状态
        /// </summary>
        private static void ApplyDegradedRogueKnightCrimeStatus(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(vlandiaKingdom, 100);
                }

                var empireKingdom = FindKingdom("kingdom_empire");
                if (empireKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(empireKingdom, 100);
                }

                OriginLog.Info("[DegradedRogueKnight] 罪犯状态设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置罪犯状态失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士Node1效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_crime")) return;

            string crime = nodes["vla_node_degraded_crime"];
            switch (crime)
            {
                case "tyranny": // 暴虐之罪
                    ApplyVillageRelations(hero, -20);
                    break;
                case "indulgence": // 沉溺之罪
                    PresetOriginSystem.AddGold(hero, 3500);
                    break;
                case "filth": // 污秽之罪
                    break;
                case "conspiracy": // 阴谋之罪
                    break;
            }
        }

        /// <summary>
        /// 应用无赖骑士Node2效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode2(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_first_case")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node3效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_view")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node4效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode4(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_goal")) return;
        }

        /// <summary>
        /// 设置瓦兰迪亚初始装备
        /// </summary>
        private static void SetVlandiaInitialEquipment(Hero hero, string originType)
        {
            try
            {
                if (hero == null || hero.BattleEquipment == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] Hero或BattleEquipment为空");
                    return;
                }

                var allItems = MBObjectManager.Instance?.GetObjectTypeList<ItemObject>();
                if (allItems == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] 无法获取物品列表");
                    return;
                }

                if (originType == "expedition_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else if (originType == "degraded_rogue_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, false);
                }

                OriginLog.Info($"[SetVlandiaInitialEquipment] {originType} 装备设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[SetVlandiaInitialEquipment] 设置装备失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据档位设置瓦兰迪亚装备
        /// </summary>
        private static void SetVlandiaEquipmentByTier(Hero hero, IEnumerable<ItemObject> allItems, int tier, bool damaged)
        {
            var vlandiaItems = allItems.Where(item =>
                (item.Culture != null && item.Culture.StringId == "vlandia") ||
                item.StringId.Contains("vlandia") ||
                item.StringId.Contains("vlandian"))
                .ToList();

            // 身体护甲
            if (hero.BattleEquipment[EquipmentIndex.Body].IsEmpty)
            {
                var bodyArmor = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.BodyArmor &&
                    (tier == 2 ? item.StringId.Contains("t2") : tier == 3 ? item.StringId.Contains("t3") : item.StringId.Contains("t4")));
                if (bodyArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Body] = new EquipmentElement(bodyArmor);
                }
            }

            // 腿部护甲
            if (hero.BattleEquipment[EquipmentIndex.Leg].IsEmpty)
            {
                var legArmor = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.LegArmor);
                if (legArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Leg] = new EquipmentElement(legArmor);
                }
            }

            // 武器：长剑+盾
            if (hero.BattleEquipment[EquipmentIndex.Weapon0].IsEmpty)
            {
                var sword = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.OneHandedWeapon &&
                    (item.StringId.Contains("sword") || item.StringId.Contains("blade")));
                if (sword != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon0] = new EquipmentElement(sword);
                }
            }

            // 盾牌
            if (hero.BattleEquipment[EquipmentIndex.Weapon1].IsEmpty)
            {
                var shield = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.Shield);
                if (shield != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon1] = new EquipmentElement(shield);
                }
            }

            // 骑枪
            if (hero.BattleEquipment[EquipmentIndex.Weapon2].IsEmpty)
            {
                var lance = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.Polearm &&
                    (item.StringId.Contains("lance") || item.StringId.Contains("spear")));
                if (lance != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon2] = new EquipmentElement(lance);
                }
            }
        }

        /// <summary>
        /// 应用随机贵族关系
        /// </summary>
        private static void ApplyRandomNobleRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    var lords = vlandiaKingdom.Lords.Where(h => h.IsLord && h != hero).ToList();
                    if (lords.Count > 0)
                    {
                        var randomLord = lords[new Random().Next(lords.Count)];
                        ChangeRelationAction.ApplyPlayerRelation(randomLord, relationChange);
                        OriginLog.Info($"[ApplyRandomNobleRelation] 与 {randomLord.Name} 关系 {relationChange}");
                    }
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRandomNobleRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用王室派系关系
        /// </summary>
        private static void ApplyRoyalFactionRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null && vlandiaKingdom.Leader != null)
                {
                    ChangeRelationAction.ApplyPlayerRelation(vlandiaKingdom.Leader, relationChange);
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRoyalFactionRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用村庄关系
        /// </summary>
        private static void ApplyVillageRelations(Hero hero, int relationChange)
        {
            try
            {
                OriginLog.Info($"[ApplyVillageRelations] 村庄关系变化: {relationChange}");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyVillageRelations] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 查找王国
        /// </summary>
        private static Kingdom FindKingdom(string kingdomId)
        {
            try
            {
                return Campaign.Current?.Kingdoms?.FirstOrDefault(k =>
                    k.StringId == kingdomId ||
                    k.StringId == $"kingdom_{kingdomId}" ||
                    (k.Culture != null && k.Culture.StringId == kingdomId));
            }
            catch
            {
                return null;
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace OriginSystemMod
{
    /// <summary>
    /// 瓦兰迪亚预设出身应用逻辑
    /// </summary>
    public static class VlandiaOriginSystem
    {
        /// <summary>
        /// 应用远征的骑士出身
        /// </summary>
        public static void ApplyExpeditionKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 开始应用远征的骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 3);
                PresetOriginSystem.GainRenown(hero, 30);
                PresetOriginSystem.AddGold(hero, 3500);

                // 设置初始瓦兰迪亚风格装备
                SetVlandiaInitialEquipment(hero, "expedition_knight");

                // 设置出生位置（根据Node2的誓言选择决定）
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                if (selectedNodes.ContainsKey("vla_node_expedition_oath"))
                {
                    string oath = selectedNodes["vla_node_expedition_oath"];
                    string startLocation = GetExpeditionKnightStartLocation(oath);
                    if (!string.IsNullOrEmpty(startLocation))
                    {
                        PresetOriginSystem.SetPresetOriginStartingLocation(party, startLocation);
                    }
                }
                else
                {
                    PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");
                }

                // 应用节点效果
                ApplyExpeditionKnightNode1(hero, selectedNodes);
                ApplyExpeditionKnightNode2(hero, clan, party, selectedNodes);
                ApplyExpeditionKnightNode3(hero, selectedNodes);
                ApplyExpeditionKnightNode4(hero, party, selectedNodes);

                // 创建哥哥NPC（固定存在）
                CreateExpeditionKnightBrother(hero, clan);

                OriginLog.Info("[ExpeditionKnight] 远征的骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[ExpeditionKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 根据誓言选择获取出生位置
        /// </summary>
        private static string GetExpeditionKnightStartLocation(string oath)
        {
            switch (oath)
            {
                case "kill_sea_raiders": // 杀1000海寇
                    return "nord"; // 诺德村子
                case "conquer_gyaz": // 远征古亚兹
                    return "aserai"; // 阿塞莱村子（靠近古亚兹）
                case "kill_battanian_lord": // 斩巴丹尼亚氏族
                    return "battania"; // 巴丹尼亚村子（肖农附近）
                case "recover_banner": // 寻回失旗
                    return "vlandia"; // 默认瓦兰迪亚
                default:
                    return "vlandia";
            }
        }

        /// <summary>
        /// 应用远征骑士Node1效果
        /// </summary>
        private static void ApplyExpeditionKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_fall")) return;

            string fall = nodes["vla_node_expedition_fall"];
            switch (fall)
            {
                case "erased_defeat":
                    PresetOriginSystem.AddSkill(hero, "tactics", 30);
                    PresetOriginSystem.AddSkill(hero, "leadership", 20);
                    ApplyRandomNobleRelation(hero, -30);
                    break;
                case "annexed":
                    PresetOriginSystem.AddSkill(hero, "charm", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "debt":
                    PresetOriginSystem.AddSkill(hero, "trade", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 2500);
                    break;
                case "political":
                    PresetOriginSystem.AddSkill(hero, "scouting", 20);
                    PresetOriginSystem.AddSkill(hero, "tactics", 20);
                    ApplyRoyalFactionRelation(hero, -30);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node2效果（誓言任务）
        /// </summary>
        private static void ApplyExpeditionKnightNode2(Hero hero, Clan clan, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_oath")) return;

            string oath = nodes["vla_node_expedition_oath"];
            
            // 创建誓言任务
            CreateExpeditionKnightOathQuest(hero, oath);
        }

        /// <summary>
        /// 应用远征骑士Node3效果
        /// </summary>
        private static void ApplyExpeditionKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_chivalry")) return;

            string chivalry = nodes["vla_node_expedition_chivalry"];
            switch (chivalry)
            {
                case "mercy": // 仁慈之誓
                    PresetOriginSystem.AddGold(hero, -2000);
                    break;
                case "valor": // 勇武之誓
                    // 开局多2-4精锐随骑（在Node4中处理）
                    break;
                case "prudence": // 谨慎之誓
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "cynical": // 犬儒之誓
                    PresetOriginSystem.AddGold(hero, 3000);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node4效果
        /// </summary>
        private static void ApplyExpeditionKnightNode4(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_division")) return;

            string division = nodes["vla_node_expedition_division"];
            switch (division)
            {
                case "brother_commands":
                    // 哥哥掌军，你主外交：哥哥偏战术/统御，你偏魅力/管理；随从偏步弩
                    break;
                case "you_command":
                    // 你掌军，哥哥做见证人：你拿骑兵核心，哥哥提供稳定后勤
                    break;
                case "joint_decision":
                    // 共同决策：队伍更均衡
                    break;
            }
        }

        /// <summary>
        /// 创建远征骑士的哥哥NPC
        /// </summary>
        private static void CreateExpeditionKnightBrother(Hero hero, Clan clan)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 需要创建哥哥NPC（待实现）");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建哥哥NPC失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 创建远征骑士的誓言任务
        /// </summary>
        private static void CreateExpeditionKnightOathQuest(Hero hero, string oath)
        {
            try
            {
                OriginLog.Info($"[ExpeditionKnight] 创建誓言任务: {oath}");
                // 任务系统需要单独实现，这里先记录
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建任务失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用堕落无赖骑士出身
        /// </summary>
        public static void ApplyDegradedRogueKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[DegradedRogueKnight] 开始应用堕落无赖骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 2);
                PresetOriginSystem.GainRenown(hero, 10);
                PresetOriginSystem.AddGold(hero, 4000);

                // 设置初始瓦兰迪亚风格装备（败落骑士风格）
                SetVlandiaInitialEquipment(hero, "degraded_rogue_knight");

                // 设置敌人关系
                ApplyDegradedRogueKnightEnemyRelations(hero);

                // 设置犯罪度（罪犯标记）
                ApplyDegradedRogueKnightCrimeStatus(hero);

                // 设置出生位置（瓦兰迪亚附近，但作为罪犯）
                PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");

                // 应用节点效果
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                ApplyDegradedRogueKnightNode1(hero, selectedNodes);
                ApplyDegradedRogueKnightNode2(hero, selectedNodes);
                ApplyDegradedRogueKnightNode3(hero, selectedNodes);
                ApplyDegradedRogueKnightNode4(hero, selectedNodes);

                OriginLog.Info("[DegradedRogueKnight] 堕落无赖骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[DegradedRogueKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的敌人关系
        /// </summary>
        private static void ApplyDegradedRogueKnightEnemyRelations(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                var empireKingdom = FindKingdom("kingdom_empire");
                var battaniaKingdom = FindKingdom("kingdom_battania");

                // 与瓦兰迪亚敌对
                if (vlandiaKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, vlandiaKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, vlandiaKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与瓦兰迪亚宣战");
                    }

                    foreach (var lord in vlandiaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与帝国敌对
                if (empireKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, empireKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, empireKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与帝国宣战");
                    }

                    foreach (var lord in empireKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与巴丹尼亚贵族关系-80（但不一定开战）
                if (battaniaKingdom != null)
                {
                    foreach (var lord in battaniaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                OriginLog.Info("[DegradedRogueKnight] 敌人关系设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置敌人关系失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的罪犯状态
        /// </summary>
        private static void ApplyDegradedRogueKnightCrimeStatus(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(vlandiaKingdom, 100);
                }

                var empireKingdom = FindKingdom("kingdom_empire");
                if (empireKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(empireKingdom, 100);
                }

                OriginLog.Info("[DegradedRogueKnight] 罪犯状态设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置罪犯状态失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士Node1效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_crime")) return;

            string crime = nodes["vla_node_degraded_crime"];
            switch (crime)
            {
                case "tyranny": // 暴虐之罪
                    ApplyVillageRelations(hero, -20);
                    break;
                case "indulgence": // 沉溺之罪
                    PresetOriginSystem.AddGold(hero, 3500);
                    break;
                case "filth": // 污秽之罪
                    break;
                case "conspiracy": // 阴谋之罪
                    break;
            }
        }

        /// <summary>
        /// 应用无赖骑士Node2效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode2(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_first_case")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node3效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_view")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node4效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode4(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_goal")) return;
        }

        /// <summary>
        /// 设置瓦兰迪亚初始装备
        /// </summary>
        private static void SetVlandiaInitialEquipment(Hero hero, string originType)
        {
            try
            {
                if (hero == null || hero.BattleEquipment == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] Hero或BattleEquipment为空");
                    return;
                }

                var allItems = MBObjectManager.Instance?.GetObjectTypeList<ItemObject>();
                if (allItems == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] 无法获取物品列表");
                    return;
                }

                if (originType == "expedition_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else if (originType == "degraded_rogue_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, false);
                }

                OriginLog.Info($"[SetVlandiaInitialEquipment] {originType} 装备设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[SetVlandiaInitialEquipment] 设置装备失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据档位设置瓦兰迪亚装备
        /// </summary>
        private static void SetVlandiaEquipmentByTier(Hero hero, IEnumerable<ItemObject> allItems, int tier, bool damaged)
        {
            var vlandiaItems = allItems.Where(item =>
                (item.Culture != null && item.Culture.StringId == "vlandia") ||
                item.StringId.Contains("vlandia") ||
                item.StringId.Contains("vlandian"))
                .ToList();

            // 身体护甲
            if (hero.BattleEquipment[EquipmentIndex.Body].IsEmpty)
            {
                var bodyArmor = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.BodyArmor &&
                    (tier == 2 ? item.StringId.Contains("t2") : tier == 3 ? item.StringId.Contains("t3") : item.StringId.Contains("t4")));
                if (bodyArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Body] = new EquipmentElement(bodyArmor);
                }
            }

            // 腿部护甲
            if (hero.BattleEquipment[EquipmentIndex.Leg].IsEmpty)
            {
                var legArmor = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.LegArmor);
                if (legArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Leg] = new EquipmentElement(legArmor);
                }
            }

            // 武器：长剑+盾
            if (hero.BattleEquipment[EquipmentIndex.Weapon0].IsEmpty)
            {
                var sword = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.OneHandedWeapon &&
                    (item.StringId.Contains("sword") || item.StringId.Contains("blade")));
                if (sword != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon0] = new EquipmentElement(sword);
                }
            }

            // 盾牌
            if (hero.BattleEquipment[EquipmentIndex.Weapon1].IsEmpty)
            {
                var shield = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.Shield);
                if (shield != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon1] = new EquipmentElement(shield);
                }
            }

            // 骑枪
            if (hero.BattleEquipment[EquipmentIndex.Weapon2].IsEmpty)
            {
                var lance = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.Polearm &&
                    (item.StringId.Contains("lance") || item.StringId.Contains("spear")));
                if (lance != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon2] = new EquipmentElement(lance);
                }
            }
        }

        /// <summary>
        /// 应用随机贵族关系
        /// </summary>
        private static void ApplyRandomNobleRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    var lords = vlandiaKingdom.Lords.Where(h => h.IsLord && h != hero).ToList();
                    if (lords.Count > 0)
                    {
                        var randomLord = lords[new Random().Next(lords.Count)];
                        ChangeRelationAction.ApplyPlayerRelation(randomLord, relationChange);
                        OriginLog.Info($"[ApplyRandomNobleRelation] 与 {randomLord.Name} 关系 {relationChange}");
                    }
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRandomNobleRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用王室派系关系
        /// </summary>
        private static void ApplyRoyalFactionRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null && vlandiaKingdom.Leader != null)
                {
                    ChangeRelationAction.ApplyPlayerRelation(vlandiaKingdom.Leader, relationChange);
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRoyalFactionRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用村庄关系
        /// </summary>
        private static void ApplyVillageRelations(Hero hero, int relationChange)
        {
            try
            {
                OriginLog.Info($"[ApplyVillageRelations] 村庄关系变化: {relationChange}");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyVillageRelations] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 查找王国
        /// </summary>
        private static Kingdom FindKingdom(string kingdomId)
        {
            try
            {
                return Campaign.Current?.Kingdoms?.FirstOrDefault(k =>
                    k.StringId == kingdomId ||
                    k.StringId == $"kingdom_{kingdomId}" ||
                    (k.Culture != null && k.Culture.StringId == kingdomId));
            }
            catch
            {
                return null;
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace OriginSystemMod
{
    /// <summary>
    /// 瓦兰迪亚预设出身应用逻辑
    /// </summary>
    public static class VlandiaOriginSystem
    {
        /// <summary>
        /// 应用远征的骑士出身
        /// </summary>
        public static void ApplyExpeditionKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 开始应用远征的骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 3);
                PresetOriginSystem.GainRenown(hero, 30);
                PresetOriginSystem.AddGold(hero, 3500);

                // 设置初始瓦兰迪亚风格装备
                SetVlandiaInitialEquipment(hero, "expedition_knight");

                // 设置出生位置（根据Node2的誓言选择决定）
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                if (selectedNodes.ContainsKey("vla_node_expedition_oath"))
                {
                    string oath = selectedNodes["vla_node_expedition_oath"];
                    string startLocation = GetExpeditionKnightStartLocation(oath);
                    if (!string.IsNullOrEmpty(startLocation))
                    {
                        PresetOriginSystem.SetPresetOriginStartingLocation(party, startLocation);
                    }
                }
                else
                {
                    PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");
                }

                // 应用节点效果
                ApplyExpeditionKnightNode1(hero, selectedNodes);
                ApplyExpeditionKnightNode2(hero, clan, party, selectedNodes);
                ApplyExpeditionKnightNode3(hero, selectedNodes);
                ApplyExpeditionKnightNode4(hero, party, selectedNodes);

                // 创建哥哥NPC（固定存在）
                CreateExpeditionKnightBrother(hero, clan);

                OriginLog.Info("[ExpeditionKnight] 远征的骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[ExpeditionKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 根据誓言选择获取出生位置
        /// </summary>
        private static string GetExpeditionKnightStartLocation(string oath)
        {
            switch (oath)
            {
                case "kill_sea_raiders": // 杀1000海寇
                    return "nord"; // 诺德村子
                case "conquer_gyaz": // 远征古亚兹
                    return "aserai"; // 阿塞莱村子（靠近古亚兹）
                case "kill_battanian_lord": // 斩巴丹尼亚氏族
                    return "battania"; // 巴丹尼亚村子（肖农附近）
                case "recover_banner": // 寻回失旗
                    return "vlandia"; // 默认瓦兰迪亚
                default:
                    return "vlandia";
            }
        }

        /// <summary>
        /// 应用远征骑士Node1效果
        /// </summary>
        private static void ApplyExpeditionKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_fall")) return;

            string fall = nodes["vla_node_expedition_fall"];
            switch (fall)
            {
                case "erased_defeat":
                    PresetOriginSystem.AddSkill(hero, "tactics", 30);
                    PresetOriginSystem.AddSkill(hero, "leadership", 20);
                    ApplyRandomNobleRelation(hero, -30);
                    break;
                case "annexed":
                    PresetOriginSystem.AddSkill(hero, "charm", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "debt":
                    PresetOriginSystem.AddSkill(hero, "trade", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 2500);
                    break;
                case "political":
                    PresetOriginSystem.AddSkill(hero, "scouting", 20);
                    PresetOriginSystem.AddSkill(hero, "tactics", 20);
                    ApplyRoyalFactionRelation(hero, -30);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node2效果（誓言任务）
        /// </summary>
        private static void ApplyExpeditionKnightNode2(Hero hero, Clan clan, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_oath")) return;

            string oath = nodes["vla_node_expedition_oath"];
            
            // 创建誓言任务
            CreateExpeditionKnightOathQuest(hero, oath);
        }

        /// <summary>
        /// 应用远征骑士Node3效果
        /// </summary>
        private static void ApplyExpeditionKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_chivalry")) return;

            string chivalry = nodes["vla_node_expedition_chivalry"];
            switch (chivalry)
            {
                case "mercy": // 仁慈之誓
                    PresetOriginSystem.AddGold(hero, -2000);
                    break;
                case "valor": // 勇武之誓
                    // 开局多2-4精锐随骑（在Node4中处理）
                    break;
                case "prudence": // 谨慎之誓
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "cynical": // 犬儒之誓
                    PresetOriginSystem.AddGold(hero, 3000);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node4效果
        /// </summary>
        private static void ApplyExpeditionKnightNode4(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_division")) return;

            string division = nodes["vla_node_expedition_division"];
            switch (division)
            {
                case "brother_commands":
                    // 哥哥掌军，你主外交：哥哥偏战术/统御，你偏魅力/管理；随从偏步弩
                    break;
                case "you_command":
                    // 你掌军，哥哥做见证人：你拿骑兵核心，哥哥提供稳定后勤
                    break;
                case "joint_decision":
                    // 共同决策：队伍更均衡
                    break;
            }
        }

        /// <summary>
        /// 创建远征骑士的哥哥NPC
        /// </summary>
        private static void CreateExpeditionKnightBrother(Hero hero, Clan clan)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 需要创建哥哥NPC（待实现）");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建哥哥NPC失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 创建远征骑士的誓言任务
        /// </summary>
        private static void CreateExpeditionKnightOathQuest(Hero hero, string oath)
        {
            try
            {
                OriginLog.Info($"[ExpeditionKnight] 创建誓言任务: {oath}");
                // 任务系统需要单独实现，这里先记录
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建任务失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用堕落无赖骑士出身
        /// </summary>
        public static void ApplyDegradedRogueKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[DegradedRogueKnight] 开始应用堕落无赖骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 2);
                PresetOriginSystem.GainRenown(hero, 10);
                PresetOriginSystem.AddGold(hero, 4000);

                // 设置初始瓦兰迪亚风格装备（败落骑士风格）
                SetVlandiaInitialEquipment(hero, "degraded_rogue_knight");

                // 设置敌人关系
                ApplyDegradedRogueKnightEnemyRelations(hero);

                // 设置犯罪度（罪犯标记）
                ApplyDegradedRogueKnightCrimeStatus(hero);

                // 设置出生位置（瓦兰迪亚附近，但作为罪犯）
                PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");

                // 应用节点效果
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                ApplyDegradedRogueKnightNode1(hero, selectedNodes);
                ApplyDegradedRogueKnightNode2(hero, selectedNodes);
                ApplyDegradedRogueKnightNode3(hero, selectedNodes);
                ApplyDegradedRogueKnightNode4(hero, selectedNodes);

                OriginLog.Info("[DegradedRogueKnight] 堕落无赖骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[DegradedRogueKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的敌人关系
        /// </summary>
        private static void ApplyDegradedRogueKnightEnemyRelations(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                var empireKingdom = FindKingdom("kingdom_empire");
                var battaniaKingdom = FindKingdom("kingdom_battania");

                // 与瓦兰迪亚敌对
                if (vlandiaKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, vlandiaKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, vlandiaKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与瓦兰迪亚宣战");
                    }

                    foreach (var lord in vlandiaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与帝国敌对
                if (empireKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, empireKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, empireKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与帝国宣战");
                    }

                    foreach (var lord in empireKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与巴丹尼亚贵族关系-80（但不一定开战）
                if (battaniaKingdom != null)
                {
                    foreach (var lord in battaniaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                OriginLog.Info("[DegradedRogueKnight] 敌人关系设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置敌人关系失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的罪犯状态
        /// </summary>
        private static void ApplyDegradedRogueKnightCrimeStatus(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(vlandiaKingdom, 100);
                }

                var empireKingdom = FindKingdom("kingdom_empire");
                if (empireKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(empireKingdom, 100);
                }

                OriginLog.Info("[DegradedRogueKnight] 罪犯状态设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置罪犯状态失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士Node1效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_crime")) return;

            string crime = nodes["vla_node_degraded_crime"];
            switch (crime)
            {
                case "tyranny": // 暴虐之罪
                    ApplyVillageRelations(hero, -20);
                    break;
                case "indulgence": // 沉溺之罪
                    PresetOriginSystem.AddGold(hero, 3500);
                    break;
                case "filth": // 污秽之罪
                    break;
                case "conspiracy": // 阴谋之罪
                    break;
            }
        }

        /// <summary>
        /// 应用无赖骑士Node2效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode2(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_first_case")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node3效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_view")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node4效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode4(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_goal")) return;
        }

        /// <summary>
        /// 设置瓦兰迪亚初始装备
        /// </summary>
        private static void SetVlandiaInitialEquipment(Hero hero, string originType)
        {
            try
            {
                if (hero == null || hero.BattleEquipment == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] Hero或BattleEquipment为空");
                    return;
                }

                var allItems = MBObjectManager.Instance?.GetObjectTypeList<ItemObject>();
                if (allItems == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] 无法获取物品列表");
                    return;
                }

                if (originType == "expedition_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else if (originType == "degraded_rogue_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, false);
                }

                OriginLog.Info($"[SetVlandiaInitialEquipment] {originType} 装备设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[SetVlandiaInitialEquipment] 设置装备失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据档位设置瓦兰迪亚装备
        /// </summary>
        private static void SetVlandiaEquipmentByTier(Hero hero, IEnumerable<ItemObject> allItems, int tier, bool damaged)
        {
            var vlandiaItems = allItems.Where(item =>
                (item.Culture != null && item.Culture.StringId == "vlandia") ||
                item.StringId.Contains("vlandia") ||
                item.StringId.Contains("vlandian"))
                .ToList();

            // 身体护甲
            if (hero.BattleEquipment[EquipmentIndex.Body].IsEmpty)
            {
                var bodyArmor = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.BodyArmor &&
                    (tier == 2 ? item.StringId.Contains("t2") : tier == 3 ? item.StringId.Contains("t3") : item.StringId.Contains("t4")));
                if (bodyArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Body] = new EquipmentElement(bodyArmor);
                }
            }

            // 腿部护甲
            if (hero.BattleEquipment[EquipmentIndex.Leg].IsEmpty)
            {
                var legArmor = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.LegArmor);
                if (legArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Leg] = new EquipmentElement(legArmor);
                }
            }

            // 武器：长剑+盾
            if (hero.BattleEquipment[EquipmentIndex.Weapon0].IsEmpty)
            {
                var sword = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.OneHandedWeapon &&
                    (item.StringId.Contains("sword") || item.StringId.Contains("blade")));
                if (sword != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon0] = new EquipmentElement(sword);
                }
            }

            // 盾牌
            if (hero.BattleEquipment[EquipmentIndex.Weapon1].IsEmpty)
            {
                var shield = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.Shield);
                if (shield != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon1] = new EquipmentElement(shield);
                }
            }

            // 骑枪
            if (hero.BattleEquipment[EquipmentIndex.Weapon2].IsEmpty)
            {
                var lance = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.Polearm &&
                    (item.StringId.Contains("lance") || item.StringId.Contains("spear")));
                if (lance != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon2] = new EquipmentElement(lance);
                }
            }
        }

        /// <summary>
        /// 应用随机贵族关系
        /// </summary>
        private static void ApplyRandomNobleRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    var lords = vlandiaKingdom.Lords.Where(h => h.IsLord && h != hero).ToList();
                    if (lords.Count > 0)
                    {
                        var randomLord = lords[new Random().Next(lords.Count)];
                        ChangeRelationAction.ApplyPlayerRelation(randomLord, relationChange);
                        OriginLog.Info($"[ApplyRandomNobleRelation] 与 {randomLord.Name} 关系 {relationChange}");
                    }
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRandomNobleRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用王室派系关系
        /// </summary>
        private static void ApplyRoyalFactionRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null && vlandiaKingdom.Leader != null)
                {
                    ChangeRelationAction.ApplyPlayerRelation(vlandiaKingdom.Leader, relationChange);
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRoyalFactionRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用村庄关系
        /// </summary>
        private static void ApplyVillageRelations(Hero hero, int relationChange)
        {
            try
            {
                OriginLog.Info($"[ApplyVillageRelations] 村庄关系变化: {relationChange}");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyVillageRelations] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 查找王国
        /// </summary>
        private static Kingdom FindKingdom(string kingdomId)
        {
            try
            {
                return Campaign.Current?.Kingdoms?.FirstOrDefault(k =>
                    k.StringId == kingdomId ||
                    k.StringId == $"kingdom_{kingdomId}" ||
                    (k.Culture != null && k.Culture.StringId == kingdomId));
            }
            catch
            {
                return null;
            }
        }
    }
}


using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace OriginSystemMod
{
    /// <summary>
    /// 瓦兰迪亚预设出身应用逻辑
    /// </summary>
    public static class VlandiaOriginSystem
    {
        /// <summary>
        /// 应用远征的骑士出身
        /// </summary>
        public static void ApplyExpeditionKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 开始应用远征的骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 3);
                PresetOriginSystem.GainRenown(hero, 30);
                PresetOriginSystem.AddGold(hero, 3500);

                // 设置初始瓦兰迪亚风格装备
                SetVlandiaInitialEquipment(hero, "expedition_knight");

                // 设置出生位置（根据Node2的誓言选择决定）
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                if (selectedNodes.ContainsKey("vla_node_expedition_oath"))
                {
                    string oath = selectedNodes["vla_node_expedition_oath"];
                    string startLocation = GetExpeditionKnightStartLocation(oath);
                    if (!string.IsNullOrEmpty(startLocation))
                    {
                        PresetOriginSystem.SetPresetOriginStartingLocation(party, startLocation);
                    }
                }
                else
                {
                    PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");
                }

                // 应用节点效果
                ApplyExpeditionKnightNode1(hero, selectedNodes);
                ApplyExpeditionKnightNode2(hero, clan, party, selectedNodes);
                ApplyExpeditionKnightNode3(hero, selectedNodes);
                ApplyExpeditionKnightNode4(hero, party, selectedNodes);

                // 创建哥哥NPC（固定存在）
                CreateExpeditionKnightBrother(hero, clan);

                OriginLog.Info("[ExpeditionKnight] 远征的骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[ExpeditionKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 根据誓言选择获取出生位置
        /// </summary>
        private static string GetExpeditionKnightStartLocation(string oath)
        {
            switch (oath)
            {
                case "kill_sea_raiders": // 杀1000海寇
                    return "nord"; // 诺德村子
                case "conquer_gyaz": // 远征古亚兹
                    return "aserai"; // 阿塞莱村子（靠近古亚兹）
                case "kill_battanian_lord": // 斩巴丹尼亚氏族
                    return "battania"; // 巴丹尼亚村子（肖农附近）
                case "recover_banner": // 寻回失旗
                    return "vlandia"; // 默认瓦兰迪亚
                default:
                    return "vlandia";
            }
        }

        /// <summary>
        /// 应用远征骑士Node1效果
        /// </summary>
        private static void ApplyExpeditionKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_fall")) return;

            string fall = nodes["vla_node_expedition_fall"];
            switch (fall)
            {
                case "erased_defeat":
                    PresetOriginSystem.AddSkill(hero, "tactics", 30);
                    PresetOriginSystem.AddSkill(hero, "leadership", 20);
                    ApplyRandomNobleRelation(hero, -30);
                    break;
                case "annexed":
                    PresetOriginSystem.AddSkill(hero, "charm", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "debt":
                    PresetOriginSystem.AddSkill(hero, "trade", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 2500);
                    break;
                case "political":
                    PresetOriginSystem.AddSkill(hero, "scouting", 20);
                    PresetOriginSystem.AddSkill(hero, "tactics", 20);
                    ApplyRoyalFactionRelation(hero, -30);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node2效果（誓言任务）
        /// </summary>
        private static void ApplyExpeditionKnightNode2(Hero hero, Clan clan, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_oath")) return;

            string oath = nodes["vla_node_expedition_oath"];
            
            // 创建誓言任务
            CreateExpeditionKnightOathQuest(hero, oath);
        }

        /// <summary>
        /// 应用远征骑士Node3效果
        /// </summary>
        private static void ApplyExpeditionKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_chivalry")) return;

            string chivalry = nodes["vla_node_expedition_chivalry"];
            switch (chivalry)
            {
                case "mercy": // 仁慈之誓
                    PresetOriginSystem.AddGold(hero, -2000);
                    break;
                case "valor": // 勇武之誓
                    // 开局多2-4精锐随骑（在Node4中处理）
                    break;
                case "prudence": // 谨慎之誓
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "cynical": // 犬儒之誓
                    PresetOriginSystem.AddGold(hero, 3000);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node4效果
        /// </summary>
        private static void ApplyExpeditionKnightNode4(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_division")) return;

            string division = nodes["vla_node_expedition_division"];
            switch (division)
            {
                case "brother_commands":
                    // 哥哥掌军，你主外交：哥哥偏战术/统御，你偏魅力/管理；随从偏步弩
                    break;
                case "you_command":
                    // 你掌军，哥哥做见证人：你拿骑兵核心，哥哥提供稳定后勤
                    break;
                case "joint_decision":
                    // 共同决策：队伍更均衡
                    break;
            }
        }

        /// <summary>
        /// 创建远征骑士的哥哥NPC
        /// </summary>
        private static void CreateExpeditionKnightBrother(Hero hero, Clan clan)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 需要创建哥哥NPC（待实现）");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建哥哥NPC失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 创建远征骑士的誓言任务
        /// </summary>
        private static void CreateExpeditionKnightOathQuest(Hero hero, string oath)
        {
            try
            {
                OriginLog.Info($"[ExpeditionKnight] 创建誓言任务: {oath}");
                // 任务系统需要单独实现，这里先记录
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建任务失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用堕落无赖骑士出身
        /// </summary>
        public static void ApplyDegradedRogueKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[DegradedRogueKnight] 开始应用堕落无赖骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 2);
                PresetOriginSystem.GainRenown(hero, 10);
                PresetOriginSystem.AddGold(hero, 4000);

                // 设置初始瓦兰迪亚风格装备（败落骑士风格）
                SetVlandiaInitialEquipment(hero, "degraded_rogue_knight");

                // 设置敌人关系
                ApplyDegradedRogueKnightEnemyRelations(hero);

                // 设置犯罪度（罪犯标记）
                ApplyDegradedRogueKnightCrimeStatus(hero);

                // 设置出生位置（瓦兰迪亚附近，但作为罪犯）
                PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");

                // 应用节点效果
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                ApplyDegradedRogueKnightNode1(hero, selectedNodes);
                ApplyDegradedRogueKnightNode2(hero, selectedNodes);
                ApplyDegradedRogueKnightNode3(hero, selectedNodes);
                ApplyDegradedRogueKnightNode4(hero, selectedNodes);

                OriginLog.Info("[DegradedRogueKnight] 堕落无赖骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[DegradedRogueKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的敌人关系
        /// </summary>
        private static void ApplyDegradedRogueKnightEnemyRelations(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                var empireKingdom = FindKingdom("kingdom_empire");
                var battaniaKingdom = FindKingdom("kingdom_battania");

                // 与瓦兰迪亚敌对
                if (vlandiaKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, vlandiaKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, vlandiaKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与瓦兰迪亚宣战");
                    }

                    foreach (var lord in vlandiaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与帝国敌对
                if (empireKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, empireKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, empireKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与帝国宣战");
                    }

                    foreach (var lord in empireKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与巴丹尼亚贵族关系-80（但不一定开战）
                if (battaniaKingdom != null)
                {
                    foreach (var lord in battaniaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                OriginLog.Info("[DegradedRogueKnight] 敌人关系设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置敌人关系失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的罪犯状态
        /// </summary>
        private static void ApplyDegradedRogueKnightCrimeStatus(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(vlandiaKingdom, 100);
                }

                var empireKingdom = FindKingdom("kingdom_empire");
                if (empireKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(empireKingdom, 100);
                }

                OriginLog.Info("[DegradedRogueKnight] 罪犯状态设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置罪犯状态失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士Node1效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_crime")) return;

            string crime = nodes["vla_node_degraded_crime"];
            switch (crime)
            {
                case "tyranny": // 暴虐之罪
                    ApplyVillageRelations(hero, -20);
                    break;
                case "indulgence": // 沉溺之罪
                    PresetOriginSystem.AddGold(hero, 3500);
                    break;
                case "filth": // 污秽之罪
                    break;
                case "conspiracy": // 阴谋之罪
                    break;
            }
        }

        /// <summary>
        /// 应用无赖骑士Node2效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode2(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_first_case")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node3效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_view")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node4效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode4(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_goal")) return;
        }

        /// <summary>
        /// 设置瓦兰迪亚初始装备
        /// </summary>
        private static void SetVlandiaInitialEquipment(Hero hero, string originType)
        {
            try
            {
                if (hero == null || hero.BattleEquipment == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] Hero或BattleEquipment为空");
                    return;
                }

                var allItems = MBObjectManager.Instance?.GetObjectTypeList<ItemObject>();
                if (allItems == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] 无法获取物品列表");
                    return;
                }

                if (originType == "expedition_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else if (originType == "degraded_rogue_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, false);
                }

                OriginLog.Info($"[SetVlandiaInitialEquipment] {originType} 装备设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[SetVlandiaInitialEquipment] 设置装备失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据档位设置瓦兰迪亚装备
        /// </summary>
        private static void SetVlandiaEquipmentByTier(Hero hero, IEnumerable<ItemObject> allItems, int tier, bool damaged)
        {
            var vlandiaItems = allItems.Where(item =>
                (item.Culture != null && item.Culture.StringId == "vlandia") ||
                item.StringId.Contains("vlandia") ||
                item.StringId.Contains("vlandian"))
                .ToList();

            // 身体护甲
            if (hero.BattleEquipment[EquipmentIndex.Body].IsEmpty)
            {
                var bodyArmor = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.BodyArmor &&
                    (tier == 2 ? item.StringId.Contains("t2") : tier == 3 ? item.StringId.Contains("t3") : item.StringId.Contains("t4")));
                if (bodyArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Body] = new EquipmentElement(bodyArmor);
                }
            }

            // 腿部护甲
            if (hero.BattleEquipment[EquipmentIndex.Leg].IsEmpty)
            {
                var legArmor = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.LegArmor);
                if (legArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Leg] = new EquipmentElement(legArmor);
                }
            }

            // 武器：长剑+盾
            if (hero.BattleEquipment[EquipmentIndex.Weapon0].IsEmpty)
            {
                var sword = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.OneHandedWeapon &&
                    (item.StringId.Contains("sword") || item.StringId.Contains("blade")));
                if (sword != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon0] = new EquipmentElement(sword);
                }
            }

            // 盾牌
            if (hero.BattleEquipment[EquipmentIndex.Weapon1].IsEmpty)
            {
                var shield = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.Shield);
                if (shield != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon1] = new EquipmentElement(shield);
                }
            }

            // 骑枪
            if (hero.BattleEquipment[EquipmentIndex.Weapon2].IsEmpty)
            {
                var lance = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.Polearm &&
                    (item.StringId.Contains("lance") || item.StringId.Contains("spear")));
                if (lance != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon2] = new EquipmentElement(lance);
                }
            }
        }

        /// <summary>
        /// 应用随机贵族关系
        /// </summary>
        private static void ApplyRandomNobleRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    var lords = vlandiaKingdom.Lords.Where(h => h.IsLord && h != hero).ToList();
                    if (lords.Count > 0)
                    {
                        var randomLord = lords[new Random().Next(lords.Count)];
                        ChangeRelationAction.ApplyPlayerRelation(randomLord, relationChange);
                        OriginLog.Info($"[ApplyRandomNobleRelation] 与 {randomLord.Name} 关系 {relationChange}");
                    }
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRandomNobleRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用王室派系关系
        /// </summary>
        private static void ApplyRoyalFactionRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null && vlandiaKingdom.Leader != null)
                {
                    ChangeRelationAction.ApplyPlayerRelation(vlandiaKingdom.Leader, relationChange);
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRoyalFactionRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用村庄关系
        /// </summary>
        private static void ApplyVillageRelations(Hero hero, int relationChange)
        {
            try
            {
                OriginLog.Info($"[ApplyVillageRelations] 村庄关系变化: {relationChange}");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyVillageRelations] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 查找王国
        /// </summary>
        private static Kingdom FindKingdom(string kingdomId)
        {
            try
            {
                return Campaign.Current?.Kingdoms?.FirstOrDefault(k =>
                    k.StringId == kingdomId ||
                    k.StringId == $"kingdom_{kingdomId}" ||
                    (k.Culture != null && k.Culture.StringId == kingdomId));
            }
            catch
            {
                return null;
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace OriginSystemMod
{
    /// <summary>
    /// 瓦兰迪亚预设出身应用逻辑
    /// </summary>
    public static class VlandiaOriginSystem
    {
        /// <summary>
        /// 应用远征的骑士出身
        /// </summary>
        public static void ApplyExpeditionKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 开始应用远征的骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 3);
                PresetOriginSystem.GainRenown(hero, 30);
                PresetOriginSystem.AddGold(hero, 3500);

                // 设置初始瓦兰迪亚风格装备
                SetVlandiaInitialEquipment(hero, "expedition_knight");

                // 设置出生位置（根据Node2的誓言选择决定）
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                if (selectedNodes.ContainsKey("vla_node_expedition_oath"))
                {
                    string oath = selectedNodes["vla_node_expedition_oath"];
                    string startLocation = GetExpeditionKnightStartLocation(oath);
                    if (!string.IsNullOrEmpty(startLocation))
                    {
                        PresetOriginSystem.SetPresetOriginStartingLocation(party, startLocation);
                    }
                }
                else
                {
                    PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");
                }

                // 应用节点效果
                ApplyExpeditionKnightNode1(hero, selectedNodes);
                ApplyExpeditionKnightNode2(hero, clan, party, selectedNodes);
                ApplyExpeditionKnightNode3(hero, selectedNodes);
                ApplyExpeditionKnightNode4(hero, party, selectedNodes);

                // 创建哥哥NPC（固定存在）
                CreateExpeditionKnightBrother(hero, clan);

                OriginLog.Info("[ExpeditionKnight] 远征的骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[ExpeditionKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 根据誓言选择获取出生位置
        /// </summary>
        private static string GetExpeditionKnightStartLocation(string oath)
        {
            switch (oath)
            {
                case "kill_sea_raiders": // 杀1000海寇
                    return "nord"; // 诺德村子
                case "conquer_gyaz": // 远征古亚兹
                    return "aserai"; // 阿塞莱村子（靠近古亚兹）
                case "kill_battanian_lord": // 斩巴丹尼亚氏族
                    return "battania"; // 巴丹尼亚村子（肖农附近）
                case "recover_banner": // 寻回失旗
                    return "vlandia"; // 默认瓦兰迪亚
                default:
                    return "vlandia";
            }
        }

        /// <summary>
        /// 应用远征骑士Node1效果
        /// </summary>
        private static void ApplyExpeditionKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_fall")) return;

            string fall = nodes["vla_node_expedition_fall"];
            switch (fall)
            {
                case "erased_defeat":
                    PresetOriginSystem.AddSkill(hero, "tactics", 30);
                    PresetOriginSystem.AddSkill(hero, "leadership", 20);
                    ApplyRandomNobleRelation(hero, -30);
                    break;
                case "annexed":
                    PresetOriginSystem.AddSkill(hero, "charm", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "debt":
                    PresetOriginSystem.AddSkill(hero, "trade", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 2500);
                    break;
                case "political":
                    PresetOriginSystem.AddSkill(hero, "scouting", 20);
                    PresetOriginSystem.AddSkill(hero, "tactics", 20);
                    ApplyRoyalFactionRelation(hero, -30);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node2效果（誓言任务）
        /// </summary>
        private static void ApplyExpeditionKnightNode2(Hero hero, Clan clan, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_oath")) return;

            string oath = nodes["vla_node_expedition_oath"];
            
            // 创建誓言任务
            CreateExpeditionKnightOathQuest(hero, oath);
        }

        /// <summary>
        /// 应用远征骑士Node3效果
        /// </summary>
        private static void ApplyExpeditionKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_chivalry")) return;

            string chivalry = nodes["vla_node_expedition_chivalry"];
            switch (chivalry)
            {
                case "mercy": // 仁慈之誓
                    PresetOriginSystem.AddGold(hero, -2000);
                    break;
                case "valor": // 勇武之誓
                    // 开局多2-4精锐随骑（在Node4中处理）
                    break;
                case "prudence": // 谨慎之誓
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "cynical": // 犬儒之誓
                    PresetOriginSystem.AddGold(hero, 3000);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node4效果
        /// </summary>
        private static void ApplyExpeditionKnightNode4(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_division")) return;

            string division = nodes["vla_node_expedition_division"];
            switch (division)
            {
                case "brother_commands":
                    // 哥哥掌军，你主外交：哥哥偏战术/统御，你偏魅力/管理；随从偏步弩
                    break;
                case "you_command":
                    // 你掌军，哥哥做见证人：你拿骑兵核心，哥哥提供稳定后勤
                    break;
                case "joint_decision":
                    // 共同决策：队伍更均衡
                    break;
            }
        }

        /// <summary>
        /// 创建远征骑士的哥哥NPC
        /// </summary>
        private static void CreateExpeditionKnightBrother(Hero hero, Clan clan)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 需要创建哥哥NPC（待实现）");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建哥哥NPC失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 创建远征骑士的誓言任务
        /// </summary>
        private static void CreateExpeditionKnightOathQuest(Hero hero, string oath)
        {
            try
            {
                OriginLog.Info($"[ExpeditionKnight] 创建誓言任务: {oath}");
                // 任务系统需要单独实现，这里先记录
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建任务失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用堕落无赖骑士出身
        /// </summary>
        public static void ApplyDegradedRogueKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[DegradedRogueKnight] 开始应用堕落无赖骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 2);
                PresetOriginSystem.GainRenown(hero, 10);
                PresetOriginSystem.AddGold(hero, 4000);

                // 设置初始瓦兰迪亚风格装备（败落骑士风格）
                SetVlandiaInitialEquipment(hero, "degraded_rogue_knight");

                // 设置敌人关系
                ApplyDegradedRogueKnightEnemyRelations(hero);

                // 设置犯罪度（罪犯标记）
                ApplyDegradedRogueKnightCrimeStatus(hero);

                // 设置出生位置（瓦兰迪亚附近，但作为罪犯）
                PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");

                // 应用节点效果
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                ApplyDegradedRogueKnightNode1(hero, selectedNodes);
                ApplyDegradedRogueKnightNode2(hero, selectedNodes);
                ApplyDegradedRogueKnightNode3(hero, selectedNodes);
                ApplyDegradedRogueKnightNode4(hero, selectedNodes);

                OriginLog.Info("[DegradedRogueKnight] 堕落无赖骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[DegradedRogueKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的敌人关系
        /// </summary>
        private static void ApplyDegradedRogueKnightEnemyRelations(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                var empireKingdom = FindKingdom("kingdom_empire");
                var battaniaKingdom = FindKingdom("kingdom_battania");

                // 与瓦兰迪亚敌对
                if (vlandiaKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, vlandiaKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, vlandiaKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与瓦兰迪亚宣战");
                    }

                    foreach (var lord in vlandiaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与帝国敌对
                if (empireKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, empireKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, empireKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与帝国宣战");
                    }

                    foreach (var lord in empireKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与巴丹尼亚贵族关系-80（但不一定开战）
                if (battaniaKingdom != null)
                {
                    foreach (var lord in battaniaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                OriginLog.Info("[DegradedRogueKnight] 敌人关系设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置敌人关系失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的罪犯状态
        /// </summary>
        private static void ApplyDegradedRogueKnightCrimeStatus(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(vlandiaKingdom, 100);
                }

                var empireKingdom = FindKingdom("kingdom_empire");
                if (empireKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(empireKingdom, 100);
                }

                OriginLog.Info("[DegradedRogueKnight] 罪犯状态设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置罪犯状态失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士Node1效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_crime")) return;

            string crime = nodes["vla_node_degraded_crime"];
            switch (crime)
            {
                case "tyranny": // 暴虐之罪
                    ApplyVillageRelations(hero, -20);
                    break;
                case "indulgence": // 沉溺之罪
                    PresetOriginSystem.AddGold(hero, 3500);
                    break;
                case "filth": // 污秽之罪
                    break;
                case "conspiracy": // 阴谋之罪
                    break;
            }
        }

        /// <summary>
        /// 应用无赖骑士Node2效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode2(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_first_case")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node3效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_view")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node4效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode4(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_goal")) return;
        }

        /// <summary>
        /// 设置瓦兰迪亚初始装备
        /// </summary>
        private static void SetVlandiaInitialEquipment(Hero hero, string originType)
        {
            try
            {
                if (hero == null || hero.BattleEquipment == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] Hero或BattleEquipment为空");
                    return;
                }

                var allItems = MBObjectManager.Instance?.GetObjectTypeList<ItemObject>();
                if (allItems == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] 无法获取物品列表");
                    return;
                }

                if (originType == "expedition_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else if (originType == "degraded_rogue_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, false);
                }

                OriginLog.Info($"[SetVlandiaInitialEquipment] {originType} 装备设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[SetVlandiaInitialEquipment] 设置装备失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据档位设置瓦兰迪亚装备
        /// </summary>
        private static void SetVlandiaEquipmentByTier(Hero hero, IEnumerable<ItemObject> allItems, int tier, bool damaged)
        {
            var vlandiaItems = allItems.Where(item =>
                (item.Culture != null && item.Culture.StringId == "vlandia") ||
                item.StringId.Contains("vlandia") ||
                item.StringId.Contains("vlandian"))
                .ToList();

            // 身体护甲
            if (hero.BattleEquipment[EquipmentIndex.Body].IsEmpty)
            {
                var bodyArmor = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.BodyArmor &&
                    (tier == 2 ? item.StringId.Contains("t2") : tier == 3 ? item.StringId.Contains("t3") : item.StringId.Contains("t4")));
                if (bodyArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Body] = new EquipmentElement(bodyArmor);
                }
            }

            // 腿部护甲
            if (hero.BattleEquipment[EquipmentIndex.Leg].IsEmpty)
            {
                var legArmor = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.LegArmor);
                if (legArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Leg] = new EquipmentElement(legArmor);
                }
            }

            // 武器：长剑+盾
            if (hero.BattleEquipment[EquipmentIndex.Weapon0].IsEmpty)
            {
                var sword = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.OneHandedWeapon &&
                    (item.StringId.Contains("sword") || item.StringId.Contains("blade")));
                if (sword != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon0] = new EquipmentElement(sword);
                }
            }

            // 盾牌
            if (hero.BattleEquipment[EquipmentIndex.Weapon1].IsEmpty)
            {
                var shield = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.Shield);
                if (shield != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon1] = new EquipmentElement(shield);
                }
            }

            // 骑枪
            if (hero.BattleEquipment[EquipmentIndex.Weapon2].IsEmpty)
            {
                var lance = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.Polearm &&
                    (item.StringId.Contains("lance") || item.StringId.Contains("spear")));
                if (lance != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon2] = new EquipmentElement(lance);
                }
            }
        }

        /// <summary>
        /// 应用随机贵族关系
        /// </summary>
        private static void ApplyRandomNobleRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    var lords = vlandiaKingdom.Lords.Where(h => h.IsLord && h != hero).ToList();
                    if (lords.Count > 0)
                    {
                        var randomLord = lords[new Random().Next(lords.Count)];
                        ChangeRelationAction.ApplyPlayerRelation(randomLord, relationChange);
                        OriginLog.Info($"[ApplyRandomNobleRelation] 与 {randomLord.Name} 关系 {relationChange}");
                    }
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRandomNobleRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用王室派系关系
        /// </summary>
        private static void ApplyRoyalFactionRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null && vlandiaKingdom.Leader != null)
                {
                    ChangeRelationAction.ApplyPlayerRelation(vlandiaKingdom.Leader, relationChange);
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRoyalFactionRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用村庄关系
        /// </summary>
        private static void ApplyVillageRelations(Hero hero, int relationChange)
        {
            try
            {
                OriginLog.Info($"[ApplyVillageRelations] 村庄关系变化: {relationChange}");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyVillageRelations] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 查找王国
        /// </summary>
        private static Kingdom FindKingdom(string kingdomId)
        {
            try
            {
                return Campaign.Current?.Kingdoms?.FirstOrDefault(k =>
                    k.StringId == kingdomId ||
                    k.StringId == $"kingdom_{kingdomId}" ||
                    (k.Culture != null && k.Culture.StringId == kingdomId));
            }
            catch
            {
                return null;
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace OriginSystemMod
{
    /// <summary>
    /// 瓦兰迪亚预设出身应用逻辑
    /// </summary>
    public static class VlandiaOriginSystem
    {
        /// <summary>
        /// 应用远征的骑士出身
        /// </summary>
        public static void ApplyExpeditionKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 开始应用远征的骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 3);
                PresetOriginSystem.GainRenown(hero, 30);
                PresetOriginSystem.AddGold(hero, 3500);

                // 设置初始瓦兰迪亚风格装备
                SetVlandiaInitialEquipment(hero, "expedition_knight");

                // 设置出生位置（根据Node2的誓言选择决定）
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                if (selectedNodes.ContainsKey("vla_node_expedition_oath"))
                {
                    string oath = selectedNodes["vla_node_expedition_oath"];
                    string startLocation = GetExpeditionKnightStartLocation(oath);
                    if (!string.IsNullOrEmpty(startLocation))
                    {
                        PresetOriginSystem.SetPresetOriginStartingLocation(party, startLocation);
                    }
                }
                else
                {
                    PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");
                }

                // 应用节点效果
                ApplyExpeditionKnightNode1(hero, selectedNodes);
                ApplyExpeditionKnightNode2(hero, clan, party, selectedNodes);
                ApplyExpeditionKnightNode3(hero, selectedNodes);
                ApplyExpeditionKnightNode4(hero, party, selectedNodes);

                // 创建哥哥NPC（固定存在）
                CreateExpeditionKnightBrother(hero, clan);

                OriginLog.Info("[ExpeditionKnight] 远征的骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[ExpeditionKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 根据誓言选择获取出生位置
        /// </summary>
        private static string GetExpeditionKnightStartLocation(string oath)
        {
            switch (oath)
            {
                case "kill_sea_raiders": // 杀1000海寇
                    return "nord"; // 诺德村子
                case "conquer_gyaz": // 远征古亚兹
                    return "aserai"; // 阿塞莱村子（靠近古亚兹）
                case "kill_battanian_lord": // 斩巴丹尼亚氏族
                    return "battania"; // 巴丹尼亚村子（肖农附近）
                case "recover_banner": // 寻回失旗
                    return "vlandia"; // 默认瓦兰迪亚
                default:
                    return "vlandia";
            }
        }

        /// <summary>
        /// 应用远征骑士Node1效果
        /// </summary>
        private static void ApplyExpeditionKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_fall")) return;

            string fall = nodes["vla_node_expedition_fall"];
            switch (fall)
            {
                case "erased_defeat":
                    PresetOriginSystem.AddSkill(hero, "tactics", 30);
                    PresetOriginSystem.AddSkill(hero, "leadership", 20);
                    ApplyRandomNobleRelation(hero, -30);
                    break;
                case "annexed":
                    PresetOriginSystem.AddSkill(hero, "charm", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "debt":
                    PresetOriginSystem.AddSkill(hero, "trade", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 2500);
                    break;
                case "political":
                    PresetOriginSystem.AddSkill(hero, "scouting", 20);
                    PresetOriginSystem.AddSkill(hero, "tactics", 20);
                    ApplyRoyalFactionRelation(hero, -30);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node2效果（誓言任务）
        /// </summary>
        private static void ApplyExpeditionKnightNode2(Hero hero, Clan clan, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_oath")) return;

            string oath = nodes["vla_node_expedition_oath"];
            
            // 创建誓言任务
            CreateExpeditionKnightOathQuest(hero, oath);
        }

        /// <summary>
        /// 应用远征骑士Node3效果
        /// </summary>
        private static void ApplyExpeditionKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_chivalry")) return;

            string chivalry = nodes["vla_node_expedition_chivalry"];
            switch (chivalry)
            {
                case "mercy": // 仁慈之誓
                    PresetOriginSystem.AddGold(hero, -2000);
                    break;
                case "valor": // 勇武之誓
                    // 开局多2-4精锐随骑（在Node4中处理）
                    break;
                case "prudence": // 谨慎之誓
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "cynical": // 犬儒之誓
                    PresetOriginSystem.AddGold(hero, 3000);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node4效果
        /// </summary>
        private static void ApplyExpeditionKnightNode4(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_division")) return;

            string division = nodes["vla_node_expedition_division"];
            switch (division)
            {
                case "brother_commands":
                    // 哥哥掌军，你主外交：哥哥偏战术/统御，你偏魅力/管理；随从偏步弩
                    break;
                case "you_command":
                    // 你掌军，哥哥做见证人：你拿骑兵核心，哥哥提供稳定后勤
                    break;
                case "joint_decision":
                    // 共同决策：队伍更均衡
                    break;
            }
        }

        /// <summary>
        /// 创建远征骑士的哥哥NPC
        /// </summary>
        private static void CreateExpeditionKnightBrother(Hero hero, Clan clan)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 需要创建哥哥NPC（待实现）");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建哥哥NPC失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 创建远征骑士的誓言任务
        /// </summary>
        private static void CreateExpeditionKnightOathQuest(Hero hero, string oath)
        {
            try
            {
                OriginLog.Info($"[ExpeditionKnight] 创建誓言任务: {oath}");
                // 任务系统需要单独实现，这里先记录
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建任务失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用堕落无赖骑士出身
        /// </summary>
        public static void ApplyDegradedRogueKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[DegradedRogueKnight] 开始应用堕落无赖骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 2);
                PresetOriginSystem.GainRenown(hero, 10);
                PresetOriginSystem.AddGold(hero, 4000);

                // 设置初始瓦兰迪亚风格装备（败落骑士风格）
                SetVlandiaInitialEquipment(hero, "degraded_rogue_knight");

                // 设置敌人关系
                ApplyDegradedRogueKnightEnemyRelations(hero);

                // 设置犯罪度（罪犯标记）
                ApplyDegradedRogueKnightCrimeStatus(hero);

                // 设置出生位置（瓦兰迪亚附近，但作为罪犯）
                PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");

                // 应用节点效果
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                ApplyDegradedRogueKnightNode1(hero, selectedNodes);
                ApplyDegradedRogueKnightNode2(hero, selectedNodes);
                ApplyDegradedRogueKnightNode3(hero, selectedNodes);
                ApplyDegradedRogueKnightNode4(hero, selectedNodes);

                OriginLog.Info("[DegradedRogueKnight] 堕落无赖骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[DegradedRogueKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的敌人关系
        /// </summary>
        private static void ApplyDegradedRogueKnightEnemyRelations(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                var empireKingdom = FindKingdom("kingdom_empire");
                var battaniaKingdom = FindKingdom("kingdom_battania");

                // 与瓦兰迪亚敌对
                if (vlandiaKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, vlandiaKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, vlandiaKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与瓦兰迪亚宣战");
                    }

                    foreach (var lord in vlandiaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与帝国敌对
                if (empireKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, empireKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, empireKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与帝国宣战");
                    }

                    foreach (var lord in empireKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与巴丹尼亚贵族关系-80（但不一定开战）
                if (battaniaKingdom != null)
                {
                    foreach (var lord in battaniaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                OriginLog.Info("[DegradedRogueKnight] 敌人关系设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置敌人关系失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的罪犯状态
        /// </summary>
        private static void ApplyDegradedRogueKnightCrimeStatus(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(vlandiaKingdom, 100);
                }

                var empireKingdom = FindKingdom("kingdom_empire");
                if (empireKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(empireKingdom, 100);
                }

                OriginLog.Info("[DegradedRogueKnight] 罪犯状态设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置罪犯状态失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士Node1效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_crime")) return;

            string crime = nodes["vla_node_degraded_crime"];
            switch (crime)
            {
                case "tyranny": // 暴虐之罪
                    ApplyVillageRelations(hero, -20);
                    break;
                case "indulgence": // 沉溺之罪
                    PresetOriginSystem.AddGold(hero, 3500);
                    break;
                case "filth": // 污秽之罪
                    break;
                case "conspiracy": // 阴谋之罪
                    break;
            }
        }

        /// <summary>
        /// 应用无赖骑士Node2效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode2(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_first_case")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node3效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_view")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node4效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode4(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_goal")) return;
        }

        /// <summary>
        /// 设置瓦兰迪亚初始装备
        /// </summary>
        private static void SetVlandiaInitialEquipment(Hero hero, string originType)
        {
            try
            {
                if (hero == null || hero.BattleEquipment == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] Hero或BattleEquipment为空");
                    return;
                }

                var allItems = MBObjectManager.Instance?.GetObjectTypeList<ItemObject>();
                if (allItems == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] 无法获取物品列表");
                    return;
                }

                if (originType == "expedition_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else if (originType == "degraded_rogue_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, false);
                }

                OriginLog.Info($"[SetVlandiaInitialEquipment] {originType} 装备设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[SetVlandiaInitialEquipment] 设置装备失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据档位设置瓦兰迪亚装备
        /// </summary>
        private static void SetVlandiaEquipmentByTier(Hero hero, IEnumerable<ItemObject> allItems, int tier, bool damaged)
        {
            var vlandiaItems = allItems.Where(item =>
                (item.Culture != null && item.Culture.StringId == "vlandia") ||
                item.StringId.Contains("vlandia") ||
                item.StringId.Contains("vlandian"))
                .ToList();

            // 身体护甲
            if (hero.BattleEquipment[EquipmentIndex.Body].IsEmpty)
            {
                var bodyArmor = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.BodyArmor &&
                    (tier == 2 ? item.StringId.Contains("t2") : tier == 3 ? item.StringId.Contains("t3") : item.StringId.Contains("t4")));
                if (bodyArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Body] = new EquipmentElement(bodyArmor);
                }
            }

            // 腿部护甲
            if (hero.BattleEquipment[EquipmentIndex.Leg].IsEmpty)
            {
                var legArmor = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.LegArmor);
                if (legArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Leg] = new EquipmentElement(legArmor);
                }
            }

            // 武器：长剑+盾
            if (hero.BattleEquipment[EquipmentIndex.Weapon0].IsEmpty)
            {
                var sword = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.OneHandedWeapon &&
                    (item.StringId.Contains("sword") || item.StringId.Contains("blade")));
                if (sword != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon0] = new EquipmentElement(sword);
                }
            }

            // 盾牌
            if (hero.BattleEquipment[EquipmentIndex.Weapon1].IsEmpty)
            {
                var shield = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.Shield);
                if (shield != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon1] = new EquipmentElement(shield);
                }
            }

            // 骑枪
            if (hero.BattleEquipment[EquipmentIndex.Weapon2].IsEmpty)
            {
                var lance = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.Polearm &&
                    (item.StringId.Contains("lance") || item.StringId.Contains("spear")));
                if (lance != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon2] = new EquipmentElement(lance);
                }
            }
        }

        /// <summary>
        /// 应用随机贵族关系
        /// </summary>
        private static void ApplyRandomNobleRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    var lords = vlandiaKingdom.Lords.Where(h => h.IsLord && h != hero).ToList();
                    if (lords.Count > 0)
                    {
                        var randomLord = lords[new Random().Next(lords.Count)];
                        ChangeRelationAction.ApplyPlayerRelation(randomLord, relationChange);
                        OriginLog.Info($"[ApplyRandomNobleRelation] 与 {randomLord.Name} 关系 {relationChange}");
                    }
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRandomNobleRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用王室派系关系
        /// </summary>
        private static void ApplyRoyalFactionRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null && vlandiaKingdom.Leader != null)
                {
                    ChangeRelationAction.ApplyPlayerRelation(vlandiaKingdom.Leader, relationChange);
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRoyalFactionRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用村庄关系
        /// </summary>
        private static void ApplyVillageRelations(Hero hero, int relationChange)
        {
            try
            {
                OriginLog.Info($"[ApplyVillageRelations] 村庄关系变化: {relationChange}");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyVillageRelations] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 查找王国
        /// </summary>
        private static Kingdom FindKingdom(string kingdomId)
        {
            try
            {
                return Campaign.Current?.Kingdoms?.FirstOrDefault(k =>
                    k.StringId == kingdomId ||
                    k.StringId == $"kingdom_{kingdomId}" ||
                    (k.Culture != null && k.Culture.StringId == kingdomId));
            }
            catch
            {
                return null;
            }
        }
    }
}


using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace OriginSystemMod
{
    /// <summary>
    /// 瓦兰迪亚预设出身应用逻辑
    /// </summary>
    public static class VlandiaOriginSystem
    {
        /// <summary>
        /// 应用远征的骑士出身
        /// </summary>
        public static void ApplyExpeditionKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 开始应用远征的骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 3);
                PresetOriginSystem.GainRenown(hero, 30);
                PresetOriginSystem.AddGold(hero, 3500);

                // 设置初始瓦兰迪亚风格装备
                SetVlandiaInitialEquipment(hero, "expedition_knight");

                // 设置出生位置（根据Node2的誓言选择决定）
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                if (selectedNodes.ContainsKey("vla_node_expedition_oath"))
                {
                    string oath = selectedNodes["vla_node_expedition_oath"];
                    string startLocation = GetExpeditionKnightStartLocation(oath);
                    if (!string.IsNullOrEmpty(startLocation))
                    {
                        PresetOriginSystem.SetPresetOriginStartingLocation(party, startLocation);
                    }
                }
                else
                {
                    PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");
                }

                // 应用节点效果
                ApplyExpeditionKnightNode1(hero, selectedNodes);
                ApplyExpeditionKnightNode2(hero, clan, party, selectedNodes);
                ApplyExpeditionKnightNode3(hero, selectedNodes);
                ApplyExpeditionKnightNode4(hero, party, selectedNodes);

                // 创建哥哥NPC（固定存在）
                CreateExpeditionKnightBrother(hero, clan);

                OriginLog.Info("[ExpeditionKnight] 远征的骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[ExpeditionKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 根据誓言选择获取出生位置
        /// </summary>
        private static string GetExpeditionKnightStartLocation(string oath)
        {
            switch (oath)
            {
                case "kill_sea_raiders": // 杀1000海寇
                    return "nord"; // 诺德村子
                case "conquer_gyaz": // 远征古亚兹
                    return "aserai"; // 阿塞莱村子（靠近古亚兹）
                case "kill_battanian_lord": // 斩巴丹尼亚氏族
                    return "battania"; // 巴丹尼亚村子（肖农附近）
                case "recover_banner": // 寻回失旗
                    return "vlandia"; // 默认瓦兰迪亚
                default:
                    return "vlandia";
            }
        }

        /// <summary>
        /// 应用远征骑士Node1效果
        /// </summary>
        private static void ApplyExpeditionKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_fall")) return;

            string fall = nodes["vla_node_expedition_fall"];
            switch (fall)
            {
                case "erased_defeat":
                    PresetOriginSystem.AddSkill(hero, "tactics", 30);
                    PresetOriginSystem.AddSkill(hero, "leadership", 20);
                    ApplyRandomNobleRelation(hero, -30);
                    break;
                case "annexed":
                    PresetOriginSystem.AddSkill(hero, "charm", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "debt":
                    PresetOriginSystem.AddSkill(hero, "trade", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 2500);
                    break;
                case "political":
                    PresetOriginSystem.AddSkill(hero, "scouting", 20);
                    PresetOriginSystem.AddSkill(hero, "tactics", 20);
                    ApplyRoyalFactionRelation(hero, -30);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node2效果（誓言任务）
        /// </summary>
        private static void ApplyExpeditionKnightNode2(Hero hero, Clan clan, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_oath")) return;

            string oath = nodes["vla_node_expedition_oath"];
            
            // 创建誓言任务
            CreateExpeditionKnightOathQuest(hero, oath);
        }

        /// <summary>
        /// 应用远征骑士Node3效果
        /// </summary>
        private static void ApplyExpeditionKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_chivalry")) return;

            string chivalry = nodes["vla_node_expedition_chivalry"];
            switch (chivalry)
            {
                case "mercy": // 仁慈之誓
                    PresetOriginSystem.AddGold(hero, -2000);
                    break;
                case "valor": // 勇武之誓
                    // 开局多2-4精锐随骑（在Node4中处理）
                    break;
                case "prudence": // 谨慎之誓
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "cynical": // 犬儒之誓
                    PresetOriginSystem.AddGold(hero, 3000);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node4效果
        /// </summary>
        private static void ApplyExpeditionKnightNode4(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_division")) return;

            string division = nodes["vla_node_expedition_division"];
            switch (division)
            {
                case "brother_commands":
                    // 哥哥掌军，你主外交：哥哥偏战术/统御，你偏魅力/管理；随从偏步弩
                    break;
                case "you_command":
                    // 你掌军，哥哥做见证人：你拿骑兵核心，哥哥提供稳定后勤
                    break;
                case "joint_decision":
                    // 共同决策：队伍更均衡
                    break;
            }
        }

        /// <summary>
        /// 创建远征骑士的哥哥NPC
        /// </summary>
        private static void CreateExpeditionKnightBrother(Hero hero, Clan clan)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 需要创建哥哥NPC（待实现）");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建哥哥NPC失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 创建远征骑士的誓言任务
        /// </summary>
        private static void CreateExpeditionKnightOathQuest(Hero hero, string oath)
        {
            try
            {
                OriginLog.Info($"[ExpeditionKnight] 创建誓言任务: {oath}");
                // 任务系统需要单独实现，这里先记录
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建任务失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用堕落无赖骑士出身
        /// </summary>
        public static void ApplyDegradedRogueKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[DegradedRogueKnight] 开始应用堕落无赖骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 2);
                PresetOriginSystem.GainRenown(hero, 10);
                PresetOriginSystem.AddGold(hero, 4000);

                // 设置初始瓦兰迪亚风格装备（败落骑士风格）
                SetVlandiaInitialEquipment(hero, "degraded_rogue_knight");

                // 设置敌人关系
                ApplyDegradedRogueKnightEnemyRelations(hero);

                // 设置犯罪度（罪犯标记）
                ApplyDegradedRogueKnightCrimeStatus(hero);

                // 设置出生位置（瓦兰迪亚附近，但作为罪犯）
                PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");

                // 应用节点效果
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                ApplyDegradedRogueKnightNode1(hero, selectedNodes);
                ApplyDegradedRogueKnightNode2(hero, selectedNodes);
                ApplyDegradedRogueKnightNode3(hero, selectedNodes);
                ApplyDegradedRogueKnightNode4(hero, selectedNodes);

                OriginLog.Info("[DegradedRogueKnight] 堕落无赖骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[DegradedRogueKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的敌人关系
        /// </summary>
        private static void ApplyDegradedRogueKnightEnemyRelations(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                var empireKingdom = FindKingdom("kingdom_empire");
                var battaniaKingdom = FindKingdom("kingdom_battania");

                // 与瓦兰迪亚敌对
                if (vlandiaKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, vlandiaKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, vlandiaKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与瓦兰迪亚宣战");
                    }

                    foreach (var lord in vlandiaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与帝国敌对
                if (empireKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, empireKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, empireKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与帝国宣战");
                    }

                    foreach (var lord in empireKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与巴丹尼亚贵族关系-80（但不一定开战）
                if (battaniaKingdom != null)
                {
                    foreach (var lord in battaniaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                OriginLog.Info("[DegradedRogueKnight] 敌人关系设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置敌人关系失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的罪犯状态
        /// </summary>
        private static void ApplyDegradedRogueKnightCrimeStatus(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(vlandiaKingdom, 100);
                }

                var empireKingdom = FindKingdom("kingdom_empire");
                if (empireKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(empireKingdom, 100);
                }

                OriginLog.Info("[DegradedRogueKnight] 罪犯状态设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置罪犯状态失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士Node1效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_crime")) return;

            string crime = nodes["vla_node_degraded_crime"];
            switch (crime)
            {
                case "tyranny": // 暴虐之罪
                    ApplyVillageRelations(hero, -20);
                    break;
                case "indulgence": // 沉溺之罪
                    PresetOriginSystem.AddGold(hero, 3500);
                    break;
                case "filth": // 污秽之罪
                    break;
                case "conspiracy": // 阴谋之罪
                    break;
            }
        }

        /// <summary>
        /// 应用无赖骑士Node2效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode2(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_first_case")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node3效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_view")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node4效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode4(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_goal")) return;
        }

        /// <summary>
        /// 设置瓦兰迪亚初始装备
        /// </summary>
        private static void SetVlandiaInitialEquipment(Hero hero, string originType)
        {
            try
            {
                if (hero == null || hero.BattleEquipment == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] Hero或BattleEquipment为空");
                    return;
                }

                var allItems = MBObjectManager.Instance?.GetObjectTypeList<ItemObject>();
                if (allItems == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] 无法获取物品列表");
                    return;
                }

                if (originType == "expedition_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else if (originType == "degraded_rogue_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, false);
                }

                OriginLog.Info($"[SetVlandiaInitialEquipment] {originType} 装备设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[SetVlandiaInitialEquipment] 设置装备失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据档位设置瓦兰迪亚装备
        /// </summary>
        private static void SetVlandiaEquipmentByTier(Hero hero, IEnumerable<ItemObject> allItems, int tier, bool damaged)
        {
            var vlandiaItems = allItems.Where(item =>
                (item.Culture != null && item.Culture.StringId == "vlandia") ||
                item.StringId.Contains("vlandia") ||
                item.StringId.Contains("vlandian"))
                .ToList();

            // 身体护甲
            if (hero.BattleEquipment[EquipmentIndex.Body].IsEmpty)
            {
                var bodyArmor = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.BodyArmor &&
                    (tier == 2 ? item.StringId.Contains("t2") : tier == 3 ? item.StringId.Contains("t3") : item.StringId.Contains("t4")));
                if (bodyArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Body] = new EquipmentElement(bodyArmor);
                }
            }

            // 腿部护甲
            if (hero.BattleEquipment[EquipmentIndex.Leg].IsEmpty)
            {
                var legArmor = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.LegArmor);
                if (legArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Leg] = new EquipmentElement(legArmor);
                }
            }

            // 武器：长剑+盾
            if (hero.BattleEquipment[EquipmentIndex.Weapon0].IsEmpty)
            {
                var sword = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.OneHandedWeapon &&
                    (item.StringId.Contains("sword") || item.StringId.Contains("blade")));
                if (sword != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon0] = new EquipmentElement(sword);
                }
            }

            // 盾牌
            if (hero.BattleEquipment[EquipmentIndex.Weapon1].IsEmpty)
            {
                var shield = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.Shield);
                if (shield != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon1] = new EquipmentElement(shield);
                }
            }

            // 骑枪
            if (hero.BattleEquipment[EquipmentIndex.Weapon2].IsEmpty)
            {
                var lance = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.Polearm &&
                    (item.StringId.Contains("lance") || item.StringId.Contains("spear")));
                if (lance != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon2] = new EquipmentElement(lance);
                }
            }
        }

        /// <summary>
        /// 应用随机贵族关系
        /// </summary>
        private static void ApplyRandomNobleRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    var lords = vlandiaKingdom.Lords.Where(h => h.IsLord && h != hero).ToList();
                    if (lords.Count > 0)
                    {
                        var randomLord = lords[new Random().Next(lords.Count)];
                        ChangeRelationAction.ApplyPlayerRelation(randomLord, relationChange);
                        OriginLog.Info($"[ApplyRandomNobleRelation] 与 {randomLord.Name} 关系 {relationChange}");
                    }
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRandomNobleRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用王室派系关系
        /// </summary>
        private static void ApplyRoyalFactionRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null && vlandiaKingdom.Leader != null)
                {
                    ChangeRelationAction.ApplyPlayerRelation(vlandiaKingdom.Leader, relationChange);
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRoyalFactionRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用村庄关系
        /// </summary>
        private static void ApplyVillageRelations(Hero hero, int relationChange)
        {
            try
            {
                OriginLog.Info($"[ApplyVillageRelations] 村庄关系变化: {relationChange}");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyVillageRelations] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 查找王国
        /// </summary>
        private static Kingdom FindKingdom(string kingdomId)
        {
            try
            {
                return Campaign.Current?.Kingdoms?.FirstOrDefault(k =>
                    k.StringId == kingdomId ||
                    k.StringId == $"kingdom_{kingdomId}" ||
                    (k.Culture != null && k.Culture.StringId == kingdomId));
            }
            catch
            {
                return null;
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace OriginSystemMod
{
    /// <summary>
    /// 瓦兰迪亚预设出身应用逻辑
    /// </summary>
    public static class VlandiaOriginSystem
    {
        /// <summary>
        /// 应用远征的骑士出身
        /// </summary>
        public static void ApplyExpeditionKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 开始应用远征的骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 3);
                PresetOriginSystem.GainRenown(hero, 30);
                PresetOriginSystem.AddGold(hero, 3500);

                // 设置初始瓦兰迪亚风格装备
                SetVlandiaInitialEquipment(hero, "expedition_knight");

                // 设置出生位置（根据Node2的誓言选择决定）
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                if (selectedNodes.ContainsKey("vla_node_expedition_oath"))
                {
                    string oath = selectedNodes["vla_node_expedition_oath"];
                    string startLocation = GetExpeditionKnightStartLocation(oath);
                    if (!string.IsNullOrEmpty(startLocation))
                    {
                        PresetOriginSystem.SetPresetOriginStartingLocation(party, startLocation);
                    }
                }
                else
                {
                    PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");
                }

                // 应用节点效果
                ApplyExpeditionKnightNode1(hero, selectedNodes);
                ApplyExpeditionKnightNode2(hero, clan, party, selectedNodes);
                ApplyExpeditionKnightNode3(hero, selectedNodes);
                ApplyExpeditionKnightNode4(hero, party, selectedNodes);

                // 创建哥哥NPC（固定存在）
                CreateExpeditionKnightBrother(hero, clan);

                OriginLog.Info("[ExpeditionKnight] 远征的骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[ExpeditionKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 根据誓言选择获取出生位置
        /// </summary>
        private static string GetExpeditionKnightStartLocation(string oath)
        {
            switch (oath)
            {
                case "kill_sea_raiders": // 杀1000海寇
                    return "nord"; // 诺德村子
                case "conquer_gyaz": // 远征古亚兹
                    return "aserai"; // 阿塞莱村子（靠近古亚兹）
                case "kill_battanian_lord": // 斩巴丹尼亚氏族
                    return "battania"; // 巴丹尼亚村子（肖农附近）
                case "recover_banner": // 寻回失旗
                    return "vlandia"; // 默认瓦兰迪亚
                default:
                    return "vlandia";
            }
        }

        /// <summary>
        /// 应用远征骑士Node1效果
        /// </summary>
        private static void ApplyExpeditionKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_fall")) return;

            string fall = nodes["vla_node_expedition_fall"];
            switch (fall)
            {
                case "erased_defeat":
                    PresetOriginSystem.AddSkill(hero, "tactics", 30);
                    PresetOriginSystem.AddSkill(hero, "leadership", 20);
                    ApplyRandomNobleRelation(hero, -30);
                    break;
                case "annexed":
                    PresetOriginSystem.AddSkill(hero, "charm", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "debt":
                    PresetOriginSystem.AddSkill(hero, "trade", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 2500);
                    break;
                case "political":
                    PresetOriginSystem.AddSkill(hero, "scouting", 20);
                    PresetOriginSystem.AddSkill(hero, "tactics", 20);
                    ApplyRoyalFactionRelation(hero, -30);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node2效果（誓言任务）
        /// </summary>
        private static void ApplyExpeditionKnightNode2(Hero hero, Clan clan, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_oath")) return;

            string oath = nodes["vla_node_expedition_oath"];
            
            // 创建誓言任务
            CreateExpeditionKnightOathQuest(hero, oath);
        }

        /// <summary>
        /// 应用远征骑士Node3效果
        /// </summary>
        private static void ApplyExpeditionKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_chivalry")) return;

            string chivalry = nodes["vla_node_expedition_chivalry"];
            switch (chivalry)
            {
                case "mercy": // 仁慈之誓
                    PresetOriginSystem.AddGold(hero, -2000);
                    break;
                case "valor": // 勇武之誓
                    // 开局多2-4精锐随骑（在Node4中处理）
                    break;
                case "prudence": // 谨慎之誓
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "cynical": // 犬儒之誓
                    PresetOriginSystem.AddGold(hero, 3000);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node4效果
        /// </summary>
        private static void ApplyExpeditionKnightNode4(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_division")) return;

            string division = nodes["vla_node_expedition_division"];
            switch (division)
            {
                case "brother_commands":
                    // 哥哥掌军，你主外交：哥哥偏战术/统御，你偏魅力/管理；随从偏步弩
                    break;
                case "you_command":
                    // 你掌军，哥哥做见证人：你拿骑兵核心，哥哥提供稳定后勤
                    break;
                case "joint_decision":
                    // 共同决策：队伍更均衡
                    break;
            }
        }

        /// <summary>
        /// 创建远征骑士的哥哥NPC
        /// </summary>
        private static void CreateExpeditionKnightBrother(Hero hero, Clan clan)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 需要创建哥哥NPC（待实现）");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建哥哥NPC失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 创建远征骑士的誓言任务
        /// </summary>
        private static void CreateExpeditionKnightOathQuest(Hero hero, string oath)
        {
            try
            {
                OriginLog.Info($"[ExpeditionKnight] 创建誓言任务: {oath}");
                // 任务系统需要单独实现，这里先记录
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建任务失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用堕落无赖骑士出身
        /// </summary>
        public static void ApplyDegradedRogueKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[DegradedRogueKnight] 开始应用堕落无赖骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 2);
                PresetOriginSystem.GainRenown(hero, 10);
                PresetOriginSystem.AddGold(hero, 4000);

                // 设置初始瓦兰迪亚风格装备（败落骑士风格）
                SetVlandiaInitialEquipment(hero, "degraded_rogue_knight");

                // 设置敌人关系
                ApplyDegradedRogueKnightEnemyRelations(hero);

                // 设置犯罪度（罪犯标记）
                ApplyDegradedRogueKnightCrimeStatus(hero);

                // 设置出生位置（瓦兰迪亚附近，但作为罪犯）
                PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");

                // 应用节点效果
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                ApplyDegradedRogueKnightNode1(hero, selectedNodes);
                ApplyDegradedRogueKnightNode2(hero, selectedNodes);
                ApplyDegradedRogueKnightNode3(hero, selectedNodes);
                ApplyDegradedRogueKnightNode4(hero, selectedNodes);

                OriginLog.Info("[DegradedRogueKnight] 堕落无赖骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[DegradedRogueKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的敌人关系
        /// </summary>
        private static void ApplyDegradedRogueKnightEnemyRelations(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                var empireKingdom = FindKingdom("kingdom_empire");
                var battaniaKingdom = FindKingdom("kingdom_battania");

                // 与瓦兰迪亚敌对
                if (vlandiaKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, vlandiaKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, vlandiaKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与瓦兰迪亚宣战");
                    }

                    foreach (var lord in vlandiaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与帝国敌对
                if (empireKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, empireKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, empireKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与帝国宣战");
                    }

                    foreach (var lord in empireKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与巴丹尼亚贵族关系-80（但不一定开战）
                if (battaniaKingdom != null)
                {
                    foreach (var lord in battaniaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                OriginLog.Info("[DegradedRogueKnight] 敌人关系设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置敌人关系失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的罪犯状态
        /// </summary>
        private static void ApplyDegradedRogueKnightCrimeStatus(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(vlandiaKingdom, 100);
                }

                var empireKingdom = FindKingdom("kingdom_empire");
                if (empireKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(empireKingdom, 100);
                }

                OriginLog.Info("[DegradedRogueKnight] 罪犯状态设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置罪犯状态失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士Node1效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_crime")) return;

            string crime = nodes["vla_node_degraded_crime"];
            switch (crime)
            {
                case "tyranny": // 暴虐之罪
                    ApplyVillageRelations(hero, -20);
                    break;
                case "indulgence": // 沉溺之罪
                    PresetOriginSystem.AddGold(hero, 3500);
                    break;
                case "filth": // 污秽之罪
                    break;
                case "conspiracy": // 阴谋之罪
                    break;
            }
        }

        /// <summary>
        /// 应用无赖骑士Node2效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode2(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_first_case")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node3效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_view")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node4效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode4(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_goal")) return;
        }

        /// <summary>
        /// 设置瓦兰迪亚初始装备
        /// </summary>
        private static void SetVlandiaInitialEquipment(Hero hero, string originType)
        {
            try
            {
                if (hero == null || hero.BattleEquipment == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] Hero或BattleEquipment为空");
                    return;
                }

                var allItems = MBObjectManager.Instance?.GetObjectTypeList<ItemObject>();
                if (allItems == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] 无法获取物品列表");
                    return;
                }

                if (originType == "expedition_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else if (originType == "degraded_rogue_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, false);
                }

                OriginLog.Info($"[SetVlandiaInitialEquipment] {originType} 装备设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[SetVlandiaInitialEquipment] 设置装备失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据档位设置瓦兰迪亚装备
        /// </summary>
        private static void SetVlandiaEquipmentByTier(Hero hero, IEnumerable<ItemObject> allItems, int tier, bool damaged)
        {
            var vlandiaItems = allItems.Where(item =>
                (item.Culture != null && item.Culture.StringId == "vlandia") ||
                item.StringId.Contains("vlandia") ||
                item.StringId.Contains("vlandian"))
                .ToList();

            // 身体护甲
            if (hero.BattleEquipment[EquipmentIndex.Body].IsEmpty)
            {
                var bodyArmor = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.BodyArmor &&
                    (tier == 2 ? item.StringId.Contains("t2") : tier == 3 ? item.StringId.Contains("t3") : item.StringId.Contains("t4")));
                if (bodyArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Body] = new EquipmentElement(bodyArmor);
                }
            }

            // 腿部护甲
            if (hero.BattleEquipment[EquipmentIndex.Leg].IsEmpty)
            {
                var legArmor = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.LegArmor);
                if (legArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Leg] = new EquipmentElement(legArmor);
                }
            }

            // 武器：长剑+盾
            if (hero.BattleEquipment[EquipmentIndex.Weapon0].IsEmpty)
            {
                var sword = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.OneHandedWeapon &&
                    (item.StringId.Contains("sword") || item.StringId.Contains("blade")));
                if (sword != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon0] = new EquipmentElement(sword);
                }
            }

            // 盾牌
            if (hero.BattleEquipment[EquipmentIndex.Weapon1].IsEmpty)
            {
                var shield = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.Shield);
                if (shield != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon1] = new EquipmentElement(shield);
                }
            }

            // 骑枪
            if (hero.BattleEquipment[EquipmentIndex.Weapon2].IsEmpty)
            {
                var lance = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.Polearm &&
                    (item.StringId.Contains("lance") || item.StringId.Contains("spear")));
                if (lance != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon2] = new EquipmentElement(lance);
                }
            }
        }

        /// <summary>
        /// 应用随机贵族关系
        /// </summary>
        private static void ApplyRandomNobleRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    var lords = vlandiaKingdom.Lords.Where(h => h.IsLord && h != hero).ToList();
                    if (lords.Count > 0)
                    {
                        var randomLord = lords[new Random().Next(lords.Count)];
                        ChangeRelationAction.ApplyPlayerRelation(randomLord, relationChange);
                        OriginLog.Info($"[ApplyRandomNobleRelation] 与 {randomLord.Name} 关系 {relationChange}");
                    }
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRandomNobleRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用王室派系关系
        /// </summary>
        private static void ApplyRoyalFactionRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null && vlandiaKingdom.Leader != null)
                {
                    ChangeRelationAction.ApplyPlayerRelation(vlandiaKingdom.Leader, relationChange);
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRoyalFactionRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用村庄关系
        /// </summary>
        private static void ApplyVillageRelations(Hero hero, int relationChange)
        {
            try
            {
                OriginLog.Info($"[ApplyVillageRelations] 村庄关系变化: {relationChange}");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyVillageRelations] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 查找王国
        /// </summary>
        private static Kingdom FindKingdom(string kingdomId)
        {
            try
            {
                return Campaign.Current?.Kingdoms?.FirstOrDefault(k =>
                    k.StringId == kingdomId ||
                    k.StringId == $"kingdom_{kingdomId}" ||
                    (k.Culture != null && k.Culture.StringId == kingdomId));
            }
            catch
            {
                return null;
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace OriginSystemMod
{
    /// <summary>
    /// 瓦兰迪亚预设出身应用逻辑
    /// </summary>
    public static class VlandiaOriginSystem
    {
        /// <summary>
        /// 应用远征的骑士出身
        /// </summary>
        public static void ApplyExpeditionKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 开始应用远征的骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 3);
                PresetOriginSystem.GainRenown(hero, 30);
                PresetOriginSystem.AddGold(hero, 3500);

                // 设置初始瓦兰迪亚风格装备
                SetVlandiaInitialEquipment(hero, "expedition_knight");

                // 设置出生位置（根据Node2的誓言选择决定）
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                if (selectedNodes.ContainsKey("vla_node_expedition_oath"))
                {
                    string oath = selectedNodes["vla_node_expedition_oath"];
                    string startLocation = GetExpeditionKnightStartLocation(oath);
                    if (!string.IsNullOrEmpty(startLocation))
                    {
                        PresetOriginSystem.SetPresetOriginStartingLocation(party, startLocation);
                    }
                }
                else
                {
                    PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");
                }

                // 应用节点效果
                ApplyExpeditionKnightNode1(hero, selectedNodes);
                ApplyExpeditionKnightNode2(hero, clan, party, selectedNodes);
                ApplyExpeditionKnightNode3(hero, selectedNodes);
                ApplyExpeditionKnightNode4(hero, party, selectedNodes);

                // 创建哥哥NPC（固定存在）
                CreateExpeditionKnightBrother(hero, clan);

                OriginLog.Info("[ExpeditionKnight] 远征的骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[ExpeditionKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 根据誓言选择获取出生位置
        /// </summary>
        private static string GetExpeditionKnightStartLocation(string oath)
        {
            switch (oath)
            {
                case "kill_sea_raiders": // 杀1000海寇
                    return "nord"; // 诺德村子
                case "conquer_gyaz": // 远征古亚兹
                    return "aserai"; // 阿塞莱村子（靠近古亚兹）
                case "kill_battanian_lord": // 斩巴丹尼亚氏族
                    return "battania"; // 巴丹尼亚村子（肖农附近）
                case "recover_banner": // 寻回失旗
                    return "vlandia"; // 默认瓦兰迪亚
                default:
                    return "vlandia";
            }
        }

        /// <summary>
        /// 应用远征骑士Node1效果
        /// </summary>
        private static void ApplyExpeditionKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_fall")) return;

            string fall = nodes["vla_node_expedition_fall"];
            switch (fall)
            {
                case "erased_defeat":
                    PresetOriginSystem.AddSkill(hero, "tactics", 30);
                    PresetOriginSystem.AddSkill(hero, "leadership", 20);
                    ApplyRandomNobleRelation(hero, -30);
                    break;
                case "annexed":
                    PresetOriginSystem.AddSkill(hero, "charm", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "debt":
                    PresetOriginSystem.AddSkill(hero, "trade", 20);
                    PresetOriginSystem.AddSkill(hero, "steward", 20);
                    PresetOriginSystem.AddGold(hero, 2500);
                    break;
                case "political":
                    PresetOriginSystem.AddSkill(hero, "scouting", 20);
                    PresetOriginSystem.AddSkill(hero, "tactics", 20);
                    ApplyRoyalFactionRelation(hero, -30);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node2效果（誓言任务）
        /// </summary>
        private static void ApplyExpeditionKnightNode2(Hero hero, Clan clan, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_oath")) return;

            string oath = nodes["vla_node_expedition_oath"];
            
            // 创建誓言任务
            CreateExpeditionKnightOathQuest(hero, oath);
        }

        /// <summary>
        /// 应用远征骑士Node3效果
        /// </summary>
        private static void ApplyExpeditionKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_chivalry")) return;

            string chivalry = nodes["vla_node_expedition_chivalry"];
            switch (chivalry)
            {
                case "mercy": // 仁慈之誓
                    PresetOriginSystem.AddGold(hero, -2000);
                    break;
                case "valor": // 勇武之誓
                    // 开局多2-4精锐随骑（在Node4中处理）
                    break;
                case "prudence": // 谨慎之誓
                    PresetOriginSystem.AddGold(hero, 1500);
                    break;
                case "cynical": // 犬儒之誓
                    PresetOriginSystem.AddGold(hero, 3000);
                    break;
            }
        }

        /// <summary>
        /// 应用远征骑士Node4效果
        /// </summary>
        private static void ApplyExpeditionKnightNode4(Hero hero, MobileParty party, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_expedition_division")) return;

            string division = nodes["vla_node_expedition_division"];
            switch (division)
            {
                case "brother_commands":
                    // 哥哥掌军，你主外交：哥哥偏战术/统御，你偏魅力/管理；随从偏步弩
                    break;
                case "you_command":
                    // 你掌军，哥哥做见证人：你拿骑兵核心，哥哥提供稳定后勤
                    break;
                case "joint_decision":
                    // 共同决策：队伍更均衡
                    break;
            }
        }

        /// <summary>
        /// 创建远征骑士的哥哥NPC
        /// </summary>
        private static void CreateExpeditionKnightBrother(Hero hero, Clan clan)
        {
            try
            {
                OriginLog.Info("[ExpeditionKnight] 需要创建哥哥NPC（待实现）");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建哥哥NPC失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 创建远征骑士的誓言任务
        /// </summary>
        private static void CreateExpeditionKnightOathQuest(Hero hero, string oath)
        {
            try
            {
                OriginLog.Info($"[ExpeditionKnight] 创建誓言任务: {oath}");
                // 任务系统需要单独实现，这里先记录
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建任务失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用堕落无赖骑士出身
        /// </summary>
        public static void ApplyDegradedRogueKnightOrigin(Hero hero, Clan clan, MobileParty party)
        {
            try
            {
                OriginLog.Info("[DegradedRogueKnight] 开始应用堕落无赖骑士出身");

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 2);
                PresetOriginSystem.GainRenown(hero, 10);
                PresetOriginSystem.AddGold(hero, 4000);

                // 设置初始瓦兰迪亚风格装备（败落骑士风格）
                SetVlandiaInitialEquipment(hero, "degraded_rogue_knight");

                // 设置敌人关系
                ApplyDegradedRogueKnightEnemyRelations(hero);

                // 设置犯罪度（罪犯标记）
                ApplyDegradedRogueKnightCrimeStatus(hero);

                // 设置出生位置（瓦兰迪亚附近，但作为罪犯）
                PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");

                // 应用节点效果
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                ApplyDegradedRogueKnightNode1(hero, selectedNodes);
                ApplyDegradedRogueKnightNode2(hero, selectedNodes);
                ApplyDegradedRogueKnightNode3(hero, selectedNodes);
                ApplyDegradedRogueKnightNode4(hero, selectedNodes);

                OriginLog.Info("[DegradedRogueKnight] 堕落无赖骑士出身应用完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 应用失败: {ex.Message}");
                OriginLog.Error($"[DegradedRogueKnight] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的敌人关系
        /// </summary>
        private static void ApplyDegradedRogueKnightEnemyRelations(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                var empireKingdom = FindKingdom("kingdom_empire");
                var battaniaKingdom = FindKingdom("kingdom_battania");

                // 与瓦兰迪亚敌对
                if (vlandiaKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, vlandiaKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, vlandiaKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与瓦兰迪亚宣战");
                    }

                    foreach (var lord in vlandiaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与帝国敌对
                if (empireKingdom != null)
                {
                    var playerFaction = hero.Clan;
                    if (playerFaction != null && !FactionManager.IsAtWarAgainstFaction(playerFaction, empireKingdom))
                    {
                        DeclareWarAction.ApplyByPlayerHostility(playerFaction, empireKingdom);
                        OriginLog.Info("[DegradedRogueKnight] 已与帝国宣战");
                    }

                    foreach (var lord in empireKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                // 与巴丹尼亚贵族关系-80（但不一定开战）
                if (battaniaKingdom != null)
                {
                    foreach (var lord in battaniaKingdom.Lords.Where(h => h.IsLord))
                    {
                        ChangeRelationAction.ApplyPlayerRelation(lord, -80);
                    }
                }

                OriginLog.Info("[DegradedRogueKnight] 敌人关系设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置敌人关系失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士的罪犯状态
        /// </summary>
        private static void ApplyDegradedRogueKnightCrimeStatus(Hero hero)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(vlandiaKingdom, 100);
                }

                var empireKingdom = FindKingdom("kingdom_empire");
                if (empireKingdom != null)
                {
                    PresetOriginSystem.SetCrimeRating(empireKingdom, 100);
                }

                OriginLog.Info("[DegradedRogueKnight] 罪犯状态设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[DegradedRogueKnight] 设置罪犯状态失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用无赖骑士Node1效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode1(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_crime")) return;

            string crime = nodes["vla_node_degraded_crime"];
            switch (crime)
            {
                case "tyranny": // 暴虐之罪
                    ApplyVillageRelations(hero, -20);
                    break;
                case "indulgence": // 沉溺之罪
                    PresetOriginSystem.AddGold(hero, 3500);
                    break;
                case "filth": // 污秽之罪
                    break;
                case "conspiracy": // 阴谋之罪
                    break;
            }
        }

        /// <summary>
        /// 应用无赖骑士Node2效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode2(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_first_case")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node3效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode3(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_view")) return;
        }

        /// <summary>
        /// 应用无赖骑士Node4效果
        /// </summary>
        private static void ApplyDegradedRogueKnightNode4(Hero hero, Dictionary<string, string> nodes)
        {
            if (!nodes.ContainsKey("vla_node_degraded_goal")) return;
        }

        /// <summary>
        /// 设置瓦兰迪亚初始装备
        /// </summary>
        private static void SetVlandiaInitialEquipment(Hero hero, string originType)
        {
            try
            {
                if (hero == null || hero.BattleEquipment == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] Hero或BattleEquipment为空");
                    return;
                }

                var allItems = MBObjectManager.Instance?.GetObjectTypeList<ItemObject>();
                if (allItems == null)
                {
                    OriginLog.Warning("[SetVlandiaInitialEquipment] 无法获取物品列表");
                    return;
                }

                if (originType == "expedition_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else if (originType == "degraded_rogue_knight")
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, true);
                }
                else
                {
                    SetVlandiaEquipmentByTier(hero, allItems, 3, false);
                }

                OriginLog.Info($"[SetVlandiaInitialEquipment] {originType} 装备设置完成");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[SetVlandiaInitialEquipment] 设置装备失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据档位设置瓦兰迪亚装备
        /// </summary>
        private static void SetVlandiaEquipmentByTier(Hero hero, IEnumerable<ItemObject> allItems, int tier, bool damaged)
        {
            var vlandiaItems = allItems.Where(item =>
                (item.Culture != null && item.Culture.StringId == "vlandia") ||
                item.StringId.Contains("vlandia") ||
                item.StringId.Contains("vlandian"))
                .ToList();

            // 身体护甲
            if (hero.BattleEquipment[EquipmentIndex.Body].IsEmpty)
            {
                var bodyArmor = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.BodyArmor &&
                    (tier == 2 ? item.StringId.Contains("t2") : tier == 3 ? item.StringId.Contains("t3") : item.StringId.Contains("t4")));
                if (bodyArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Body] = new EquipmentElement(bodyArmor);
                }
            }

            // 腿部护甲
            if (hero.BattleEquipment[EquipmentIndex.Leg].IsEmpty)
            {
                var legArmor = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.LegArmor);
                if (legArmor != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Leg] = new EquipmentElement(legArmor);
                }
            }

            // 武器：长剑+盾
            if (hero.BattleEquipment[EquipmentIndex.Weapon0].IsEmpty)
            {
                var sword = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.OneHandedWeapon &&
                    (item.StringId.Contains("sword") || item.StringId.Contains("blade")));
                if (sword != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon0] = new EquipmentElement(sword);
                }
            }

            // 盾牌
            if (hero.BattleEquipment[EquipmentIndex.Weapon1].IsEmpty)
            {
                var shield = vlandiaItems.FirstOrDefault(item => item.Type == ItemObject.ItemTypeEnum.Shield);
                if (shield != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon1] = new EquipmentElement(shield);
                }
            }

            // 骑枪
            if (hero.BattleEquipment[EquipmentIndex.Weapon2].IsEmpty)
            {
                var lance = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.Polearm &&
                    (item.StringId.Contains("lance") || item.StringId.Contains("spear")));
                if (lance != null)
                {
                    hero.BattleEquipment[EquipmentIndex.Weapon2] = new EquipmentElement(lance);
                }
            }
        }

        /// <summary>
        /// 应用随机贵族关系
        /// </summary>
        private static void ApplyRandomNobleRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null)
                {
                    var lords = vlandiaKingdom.Lords.Where(h => h.IsLord && h != hero).ToList();
                    if (lords.Count > 0)
                    {
                        var randomLord = lords[new Random().Next(lords.Count)];
                        ChangeRelationAction.ApplyPlayerRelation(randomLord, relationChange);
                        OriginLog.Info($"[ApplyRandomNobleRelation] 与 {randomLord.Name} 关系 {relationChange}");
                    }
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRandomNobleRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用王室派系关系
        /// </summary>
        private static void ApplyRoyalFactionRelation(Hero hero, int relationChange)
        {
            try
            {
                var vlandiaKingdom = FindKingdom("kingdom_vlandia");
                if (vlandiaKingdom != null && vlandiaKingdom.Leader != null)
                {
                    ChangeRelationAction.ApplyPlayerRelation(vlandiaKingdom.Leader, relationChange);
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyRoyalFactionRelation] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用村庄关系
        /// </summary>
        private static void ApplyVillageRelations(Hero hero, int relationChange)
        {
            try
            {
                OriginLog.Info($"[ApplyVillageRelations] 村庄关系变化: {relationChange}");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ApplyVillageRelations] 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 查找王国
        /// </summary>
        private static Kingdom FindKingdom(string kingdomId)
        {
            try
            {
                return Campaign.Current?.Kingdoms?.FirstOrDefault(k =>
                    k.StringId == kingdomId ||
                    k.StringId == $"kingdom_{kingdomId}" ||
                    (k.Culture != null && k.Culture.StringId == kingdomId));
            }
            catch
            {
                return null;
            }
        }
    }
}







