using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;
using TaleWorlds.CampaignSystem.Settlements;

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
                Debug.Print("[OriginSystem] [ExpeditionKnight] 开始应用远征的骑士出身", 0, Debug.DebugColor.Green);

                // 基础设置
                PresetOriginSystem.SetClanTier(clan, 3);
                PresetOriginSystem.GainRenown(hero, 30);
                PresetOriginSystem.AddGold(hero, 3500);

                // 设置初始瓦兰迪亚风格装备
                SetVlandiaInitialEquipment(hero, "expedition_knight");

                // 设置出生位置（根据Node2的誓言选择决定）
                var selectedNodes = OriginSystemHelper.SelectedPresetOriginNodes;
                // 注意：节点键名是 "vla_node_expedition_knight_oath"，不是 "vla_node_expedition_oath"
                string oath = null;
                if (selectedNodes.ContainsKey("vla_node_expedition_knight_oath"))
                {
                    oath = selectedNodes["vla_node_expedition_knight_oath"];
                }
                else if (selectedNodes.ContainsKey("vla_node_expedition_oath"))
                {
                    // 兼容旧键名
                    oath = selectedNodes["vla_node_expedition_oath"];
                }
                
                if (!string.IsNullOrEmpty(oath))
                {
                    // 映射值：节点保存的是 "kill_1000_sea_raiders"，但函数期望 "kill_sea_raiders"
                    string normalizedOath = oath;
                    if (oath == "kill_1000_sea_raiders")
                    {
                        normalizedOath = "kill_sea_raiders";
                    }
                    else if (oath == "conquer_quyaz")
                    {
                        normalizedOath = "conquer_gyaz";
                    }
                    else if (oath == "kill_battania_noble")
                    {
                        normalizedOath = "kill_battanian_lord";
                    }
                    else if (oath == "recover_relic")
                    {
                        normalizedOath = "recover_banner";
                    }
                    
                    string startLocation = GetExpeditionKnightStartLocation(normalizedOath);
                    if (!string.IsNullOrEmpty(startLocation))
                    {
                        OriginSystemHelper.PendingVlandiaStartLocation = startLocation;
                        OriginLog.Info($"[ExpeditionKnight] Movement pending to: {startLocation} (oath={oath}, normalized={normalizedOath})");
                        Debug.Print($"[OriginSystem] [ExpeditionKnight] Movement pending to: {startLocation}", 0, Debug.DebugColor.Green);
                    }
                    else
                    {
                        OriginSystemHelper.PendingVlandiaStartLocation = "vlandia";
                        OriginLog.Warning($"[ExpeditionKnight] 未找到对应位置，使用默认 vlandia (oath={oath})");
                    }
                }
                else
                {
                    // PresetOriginSystem.SetPresetOriginStartingLocation(party, "vlandia");
                    OriginSystemHelper.PendingVlandiaStartLocation = "vlandia";
                    OriginLog.Info($"[ExpeditionKnight] Movement pending to: vlandia (default, no oath selected)");
                    Debug.Print($"[OriginSystem] [ExpeditionKnight] Movement pending to: vlandia (default)", 0, Debug.DebugColor.Green);
                }

                // 应用节点效果
                ApplyExpeditionKnightNode1(hero, selectedNodes);
                ApplyExpeditionKnightNode2(hero, clan, party, selectedNodes);
                ApplyExpeditionKnightNode3(hero, selectedNodes);
                ApplyExpeditionKnightNode4(hero, party, selectedNodes);

                // 创建哥哥NPC（固定存在）
                CreateExpeditionKnightBrother(hero, clan);

                // 创建誓言任务（如果选择了杀1000海寇的誓言）
                if (!string.IsNullOrEmpty(oath) && (oath == "kill_sea_raiders" || oath == "kill_1000_sea_raiders"))
                {
                    CreateExpeditionKnightOathQuest(hero, oath);
                }

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
                OriginLog.Info("[ExpeditionKnight] 开始创建哥哥NPC");
                Debug.Print("[OriginSystem] [ExpeditionKnight] 开始创建哥哥NPC", 0, Debug.DebugColor.Green);

                if (hero == null || clan == null)
                {
                    OriginLog.Warning("[ExpeditionKnight] hero 或 clan 为 null，无法创建哥哥");
                    return;
                }

                // 获取家族定居点
                Settlement birthSettlement = clan.HomeSettlement;
                if (birthSettlement == null)
                {
                    // 如果没有定居点，使用瓦兰迪亚的随机城镇
                    birthSettlement = Settlement.All.FirstOrDefault(s => s != null && s.IsTown && s.Culture != null && s.Culture.StringId == "vlandia");
                    if (birthSettlement == null)
                    {
                        OriginLog.Warning("[ExpeditionKnight] 找不到合适的定居点创建哥哥");
                        return;
                    }
                }

                // 创建哥哥Hero（使用CreateChild，但年龄设置为比玩家大2-5岁）
                int playerAge = (int)hero.Age;
                int brotherAge = playerAge + MBRandom.RandomInt(2, 6); // 哥哥比玩家大2-6岁
                
                Hero brother = HeroCreator.CreateChild(hero.CharacterObject, birthSettlement, clan, brotherAge);
                
                if (brother == null)
                {
                    OriginLog.Error("[ExpeditionKnight] HeroCreator.CreateChild 返回 null");
                    return;
                }

                // 设置哥哥的属性
                brother.UpdateHomeSettlement();
                brother.HeroDeveloper.InitializeHeroDeveloper();
                
                // 设置名字（使用文化相关的名字）
                // 注意：名字通常由游戏自动生成，但我们可以通过CharacterObject设置
                
                // 将哥哥添加到玩家队伍作为同伴
                AddCompanionAction.Apply(Clan.PlayerClan, brother);
                
                // 如果玩家队伍存在，将哥哥添加到队伍中
                if (MobileParty.MainParty != null)
                {
                    MobileParty.MainParty.MemberRoster.AddToCounts(brother.CharacterObject, 1);
                }

                OriginLog.Info($"[ExpeditionKnight] ✅ 哥哥NPC创建成功: {brother.Name} (年龄: {brother.Age}, 玩家年龄: {playerAge})");
                Debug.Print($"[OriginSystem] [ExpeditionKnight] ✅ 哥哥NPC创建成功: {brother.Name}", 0, Debug.DebugColor.Green);
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建哥哥NPC失败: {ex.Message}");
                OriginLog.Error($"[ExpeditionKnight] StackTrace: {ex.StackTrace}");
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
                Debug.Print($"[OriginSystem] [ExpeditionKnight] 创建誓言任务: {oath}", 0, Debug.DebugColor.Green);
                
                if (oath == "kill_sea_raiders" || oath == "kill_1000_sea_raiders")
                {
                    // 根据反编译结果：QuestBase(string questId, Hero questGiver, CampaignTime dueTime, int rewardGold)
                    string questId = "expedition_oath_quest_kill_sea_raiders";
                    
                    // 创建任务实例
                    var quest = new OriginSystemMod.Quests.ExpeditionOathQuest(questId, hero);
                    
                    // 启动任务（StartQuest会自动调用）
                    quest.StartQuest();
                    
                    OriginLog.Info("[ExpeditionKnight] ✅ 誓言任务创建并启动成功");
                    Debug.Print("[OriginSystem] [ExpeditionKnight] ✅ 誓言任务创建并启动成功", 0, Debug.DebugColor.Green);
                }
                else
                {
                    OriginLog.Info($"[ExpeditionKnight] 誓言类型 '{oath}' 暂不支持创建任务");
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[ExpeditionKnight] 创建任务失败: {ex.Message}");
                OriginLog.Error($"[ExpeditionKnight] StackTrace: {ex.StackTrace}");
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

                    // 获取王国所有领主（通过氏族）
                    foreach (var clan in vlandiaKingdom.Clans)
                    {
                        if (clan != null && clan.Leader != null && clan.Leader.IsLord)
                        {
                            ChangeRelationAction.ApplyPlayerRelation(clan.Leader, -80);
                        }
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

                    // 获取王国所有领主（通过氏族）
                    foreach (var clan in empireKingdom.Clans)
                    {
                        if (clan != null && clan.Leader != null && clan.Leader.IsLord)
                        {
                            ChangeRelationAction.ApplyPlayerRelation(clan.Leader, -80);
                        }
                    }
                }

                // 与巴丹尼亚贵族关系-80（但不一定开战）
                if (battaniaKingdom != null)
                {
                    // 获取王国所有领主（通过氏族）
                    foreach (var clan in battaniaKingdom.Clans)
                    {
                        if (clan != null && clan.Leader != null && clan.Leader.IsLord)
                        {
                            ChangeRelationAction.ApplyPlayerRelation(clan.Leader, -80);
                        }
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
                // Robust string matching
                var bodyArmor = vlandiaItems.FirstOrDefault(item =>
                    item.Type == ItemObject.ItemTypeEnum.BodyArmor &&
                    (
                        (tier == 2 && (item.StringId.Contains("t2") || item.StringId.Contains("tier_2") || item.Name.ToString().Contains("Tier 2"))) ||
                        (tier == 3 && (item.StringId.Contains("t3") || item.StringId.Contains("tier_3") || item.Name.ToString().Contains("Tier 3"))) ||
                        (tier == 4 && (item.StringId.Contains("t4") || item.StringId.Contains("tier_4") || item.Name.ToString().Contains("Tier 4")))
                    ));

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
                    // 获取王国所有领主（通过氏族）
                    var lords = vlandiaKingdom.Clans
                        .Where(c => c != null && c.Leader != null && c.Leader.IsLord && c.Leader != hero)
                        .Select(c => c.Leader)
                        .ToList();
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