using System;
using HarmonyLib;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using OriginSystemMod;

namespace OriginSystemMod
{
    /// <summary>
    /// Patch AddParentsMenu 的Postfix,在原有菜单添加后插入"出身类型选择"菜单
    /// 使用 partial class 允许访问 OriginSystemPatches 中的其他方法
    /// </summary>
    public static partial class OriginSystemPatches
    {
        [HarmonyPatch(typeof(CharacterCreationCampaignBehavior), "AddParentsMenu")]
        public static class AddParentsMenuPatch
        {
            static void Postfix(CharacterCreationManager characterCreationManager)
            {
                try
                {
                    OriginLog.Info("AddParentsMenu Postfix 被调用");

                    // 2. 将新菜单插入到第一个位置(确保 InputMenuId == "start")
                    var parentMenu = characterCreationManager.GetNarrativeMenuWithId("narrative_parent_menu");

                    if (parentMenu != null)
                    {
                        // 删除原菜单
                        characterCreationManager.DeleteNarrativeMenuWithId("narrative_parent_menu");
                        OriginLog.Info("已删除原父母菜单");
                    }

                    // 3. 按顺序添加所有菜单
                    // 顺序很重要:第一个菜单的 InputMenuId 必须为"start"
                    var originTypeMenu = CreateOriginTypeSelectionMenu(characterCreationManager);
                    characterCreationManager.AddNewMenu(originTypeMenu);
                    OriginLog.Info("已添出身类型选择菜单");

                    var presetOriginMenu = CreatePresetOriginSelectionMenu(characterCreationManager);
                    characterCreationManager.AddNewMenu(presetOriginMenu);
                    OriginLog.Info("已添加预设出身选择菜单");

                    // 添加预设出身的私有节点菜单
                    // 草原叛酋(4个节点)
                    var rebelChiefNode1Menu = CreateRebelChiefNode1Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(rebelChiefNode1Menu);
                    var rebelChiefNode2Menu = CreateRebelChiefNode2Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(rebelChiefNode2Menu);
                    var rebelChiefNode3Menu = CreateRebelChiefNode3Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(rebelChiefNode3Menu);
                    var rebelChiefNode4Menu = CreateRebelChiefNode4Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(rebelChiefNode4Menu);
                    OriginLog.Info("已添草原叛酋'4个节点菜");

                    // 汗廷旁支贵族个节点)
                    var minorNobleNode1Menu = CreateMinorNobleNode1Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(minorNobleNode1Menu);
                    var minorNobleNode2Menu = CreateMinorNobleNode2Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(minorNobleNode2Menu);
                    var minorNobleNode3Menu = CreateMinorNobleNode3Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(minorNobleNode3Menu);
                    var minorNobleNode4Menu = CreateMinorNobleNode4Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(minorNobleNode4Menu);
                    OriginLog.Info("已添汗廷旁支贵族'4个节点菜");

                    // 西迁部族首领(4个节点)
                    var migrantChiefNode1Menu = CreateMigrantChiefNode1Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(migrantChiefNode1Menu);
                    var migrantChiefNode2Menu = CreateMigrantChiefNode2Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(migrantChiefNode2Menu);
                    var migrantChiefNode3Menu = CreateMigrantChiefNode3Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(migrantChiefNode3Menu);
                    var migrantChiefNode4Menu = CreateMigrantChiefNode4Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(migrantChiefNode4Menu);
                    OriginLog.Info("已添西迁部族首领'4个节点菜");

                    // 汗廷军叛逃者(4个节点)
                    var armyDeserterNode1Menu = CreateArmyDeserterNode1Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(armyDeserterNode1Menu);
                    var armyDeserterNode2Menu = CreateArmyDeserterNode2Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(armyDeserterNode2Menu);
                    var armyDeserterNode3Menu = CreateArmyDeserterNode3Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(armyDeserterNode3Menu);
                    var armyDeserterNode4Menu = CreateArmyDeserterNode4Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(armyDeserterNode4Menu);
                    OriginLog.Info("已添汗廷军叛逃4个节点菜");

                    // 草原商路守护者(4个节点)
                    var tradeProtectorNode1Menu = CreateTradeProtectorNode1Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(tradeProtectorNode1Menu);
                    var tradeProtectorNode2Menu = CreateTradeProtectorNode2Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(tradeProtectorNode2Menu);
                    var tradeProtectorNode3Menu = CreateTradeProtectorNode3Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(tradeProtectorNode3Menu);
                    var tradeProtectorNode4Menu = CreateTradeProtectorNode4Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(tradeProtectorNode4Menu);
                    OriginLog.Info("已添草原商路守护4个节点菜");

                    // 东方迁徙王族(4个节点)
                    var wanderingPrinceNode1Menu = CreateWanderingPrinceNode1Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(wanderingPrinceNode1Menu);
                    var wanderingPrinceSubNodeMenu = CreateWanderingPrinceSubNodeMenu(characterCreationManager);
                    characterCreationManager.AddNewMenu(wanderingPrinceSubNodeMenu);
                    var wanderingPrinceNode3Menu = CreateWanderingPrinceNode3Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(wanderingPrinceNode3Menu);
                    var wanderingPrinceNode4Menu = CreateWanderingPrinceNode4Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(wanderingPrinceNode4Menu);
                    OriginLog.Info("已添东方迁徙王族'4个节点菜");

                    // 可汗的雇佣战帮(4个节点)
                    var khansMercenaryNode1Menu = CreateKhansMercenaryNode1Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(khansMercenaryNode1Menu);
                    var khansMercenaryNode2Menu = CreateKhansMercenaryNode2Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(khansMercenaryNode2Menu);
                    var khansMercenaryNode3Menu = CreateKhansMercenaryNode3Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(khansMercenaryNode3Menu);
                    var khansMercenaryNode4Menu = CreateKhansMercenaryNode4Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(khansMercenaryNode4Menu);
                    OriginLog.Info("已添可汗的雇佣战4个节点菜");

                    // 战奴逃亡(5个节点)
                    var slaveEscapeNode1Menu = CreateSlaveEscapeNode1Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(slaveEscapeNode1Menu);
                    var slaveEscapeNode2Menu = CreateSlaveEscapeNode2Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(slaveEscapeNode2Menu);
                    var slaveEscapeNode3Menu = CreateSlaveEscapeNode3Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(slaveEscapeNode3Menu);
                    var slaveEscapeNode4Menu = CreateSlaveEscapeNode4Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(slaveEscapeNode4Menu);
                    var slaveEscapeNode5Menu = CreateSlaveEscapeNode5Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(slaveEscapeNode5Menu);
                    OriginLog.Info("已添战奴逃亡'5个节点菜");

                    // 草原自由民(4个节点)
                    var freeCossackNode1Menu = CreateFreeCossackNode1Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(freeCossackNode1Menu);
                    var freeCossackNode2Menu = CreateFreeCossackNode2Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(freeCossackNode2Menu);
                    var freeCossackNode3Menu = CreateFreeCossackNode3Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(freeCossackNode3Menu);
                    var freeCossackNode4Menu = CreateFreeCossackNode4Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(freeCossackNode4Menu);
                    OriginLog.Info("已添加草原自由4个节点菜");

                    // 东方旧部复仇者(4个节点)
                    var oldGuardAvengerNode1Menu = CreateOldGuardAvengerNode1Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(oldGuardAvengerNode1Menu);
                    var oldGuardAvengerNode2Menu = CreateOldGuardAvengerNode2Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(oldGuardAvengerNode2Menu);
                    var oldGuardAvengerNode3Menu = CreateOldGuardAvengerNode3Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(oldGuardAvengerNode3Menu);
                    var oldGuardAvengerNode4Menu = CreateOldGuardAvengerNode4Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(oldGuardAvengerNode4Menu);
                    OriginLog.Info("已添东方旧部复仇4个节点菜");

                    // 瓦兰迪亚预设出身节点菜单
                    // 远征的骑士(4个节点)
                    var expeditionKnightNode1Menu = CreateExpeditionKnightNode1Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(expeditionKnightNode1Menu);
                    var expeditionKnightNode2Menu = CreateExpeditionKnightNode2Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(expeditionKnightNode2Menu);
                    var expeditionKnightNode3Menu = CreateExpeditionKnightNode3Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(expeditionKnightNode3Menu);
                    var expeditionKnightNode4Menu = CreateExpeditionKnightNode4Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(expeditionKnightNode4Menu);
                    OriginLog.Info("已添加远征的骑士4个节点菜单");

                    // 破产的旗主继承人(4个节点)
                    var bankruptBanneretNode1Menu = CreateBankruptBanneretNode1Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(bankruptBanneretNode1Menu);
                    var bankruptBanneretNode2Menu = CreateBankruptBanneretNode2Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(bankruptBanneretNode2Menu);
                    var bankruptBanneretNode3Menu = CreateBankruptBanneretNode3Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(bankruptBanneretNode3Menu);
                    var bankruptBanneretNode4Menu = CreateBankruptBanneretNode4Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(bankruptBanneretNode4Menu);
                    OriginLog.Info("已添加破产的旗主继承人4个节点菜单");

                    // 城镇弩匠行会的执旗人(4个节点)
                    var crossbowGuildNode1Menu = CreateCrossbowGuildNode1Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(crossbowGuildNode1Menu);
                    var crossbowGuildNode2Menu = CreateCrossbowGuildNode2Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(crossbowGuildNode2Menu);
                    var crossbowGuildNode3Menu = CreateCrossbowGuildNode3Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(crossbowGuildNode3Menu);
                    var crossbowGuildNode4Menu = CreateCrossbowGuildNode4Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(crossbowGuildNode4Menu);
                    OriginLog.Info("已添加城镇弩匠行会的执旗人4个节点菜单");

                    // 边境行省的守境骑长(4个节点)
                    var marchWardenNode1Menu = CreateMarchWardenNode1Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(marchWardenNode1Menu);
                    var marchWardenNode2Menu = CreateMarchWardenNode2Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(marchWardenNode2Menu);
                    var marchWardenNode3Menu = CreateMarchWardenNode3Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(marchWardenNode3Menu);
                    var marchWardenNode4Menu = CreateMarchWardenNode4Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(marchWardenNode4Menu);
                    OriginLog.Info("已添加边境行省的守境骑长4个节点菜单");

                    // 黑旗匪首(4个节点)
                    var blackPathCaptainNode1Menu = CreateBlackPathCaptainNode1Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(blackPathCaptainNode1Menu);
                    var blackPathCaptainNode2Menu = CreateBlackPathCaptainNode2Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(blackPathCaptainNode2Menu);
                    var blackPathCaptainNode3Menu = CreateBlackPathCaptainNode3Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(blackPathCaptainNode3Menu);
                    var blackPathCaptainNode4Menu = CreateBlackPathCaptainNode4Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(blackPathCaptainNode4Menu);
                    OriginLog.Info("已添加黑旗匪首4个节点菜单");

                    // 比武场的落魄冠军(4个节点)
                    var tourneyChampionNode1Menu = CreateTourneyChampionNode1Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(tourneyChampionNode1Menu);
                    var tourneyChampionNode2Menu = CreateTourneyChampionNode2Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(tourneyChampionNode2Menu);
                    var tourneyChampionNode3Menu = CreateTourneyChampionNode3Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(tourneyChampionNode3Menu);
                    var tourneyChampionNode4Menu = CreateTourneyChampionNode4Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(tourneyChampionNode4Menu);
                    OriginLog.Info("已添加比武场的落魄冠军4个节点菜单");

                    // 侍奉骑士团的扈从(4个节点)
                    var orderSquireNode1Menu = CreateOrderSquireNode1Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(orderSquireNode1Menu);
                    var orderSquireNode2Menu = CreateOrderSquireNode2Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(orderSquireNode2Menu);
                    var orderSquireNode3Menu = CreateOrderSquireNode3Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(orderSquireNode3Menu);
                    var orderSquireNode4Menu = CreateOrderSquireNode4Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(orderSquireNode4Menu);
                    OriginLog.Info("已添加侍奉骑士团的扈从4个节点菜单");

                    // 雇佣军连队副官(4个节点) - TODO: 实现菜单创建方法
                    // var sellswordLieutenantNode1Menu = CreateSellswordLieutenantNode1Menu(characterCreationManager);
                    // characterCreationManager.AddNewMenu(sellswordLieutenantNode1Menu);
                    // var sellswordLieutenantNode2Menu = CreateSellswordLieutenantNode2Menu(characterCreationManager);
                    // characterCreationManager.AddNewMenu(sellswordLieutenantNode2Menu);
                    // var sellswordLieutenantNode3Menu = CreateSellswordLieutenantNode3Menu(characterCreationManager);
                    // characterCreationManager.AddNewMenu(sellswordLieutenantNode3Menu);
                    // var sellswordLieutenantNode4Menu = CreateSellswordLieutenantNode4Menu(characterCreationManager);
                    // characterCreationManager.AddNewMenu(sellswordLieutenantNode4Menu);
                    OriginLog.Info("已添加雇佣军连队副官4个节点菜单 (暂未实现)");

                    // 王室税吏的护卫(4个节点)
                    var taxBailiffEnforcerNode1Menu = CreateTaxBailiffEnforcerNode1Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(taxBailiffEnforcerNode1Menu);
                    var taxBailiffEnforcerNode2Menu = CreateTaxBailiffEnforcerNode2Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(taxBailiffEnforcerNode2Menu);
                    var taxBailiffEnforcerNode3Menu = CreateTaxBailiffEnforcerNode3Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(taxBailiffEnforcerNode3Menu);
                    var taxBailiffEnforcerNode4Menu = CreateTaxBailiffEnforcerNode4Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(taxBailiffEnforcerNode4Menu);
                    OriginLog.Info("已添加王室税吏的护卫4个节点菜单");

                    // 自由民团的推旗人(4个节点)
                    var freeMilitiaLeaderNode1Menu = CreateFreeMilitiaLeaderNode1Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(freeMilitiaLeaderNode1Menu);
                    var freeMilitiaLeaderNode2Menu = CreateFreeMilitiaLeaderNode2Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(freeMilitiaLeaderNode2Menu);
                    var freeMilitiaLeaderNode3Menu = CreateFreeMilitiaLeaderNode3Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(freeMilitiaLeaderNode3Menu);
                    var freeMilitiaLeaderNode4Menu = CreateFreeMilitiaLeaderNode4Menu(characterCreationManager);
                    characterCreationManager.AddNewMenu(freeMilitiaLeaderNode4Menu);
                    OriginLog.Info("已添加自由民团的推旗人4个节点菜单");

                    // 堕落无赖骑士(4个节点，黑旗匪首的分支) - TODO: 实现菜单创建方法
                    // var degradedRogueKnightNode1Menu = CreateDegradedRogueKnightNode1Menu(characterCreationManager);
                    // characterCreationManager.AddNewMenu(degradedRogueKnightNode1Menu);
                    // var degradedRogueKnightNode2Menu = CreateDegradedRogueKnightNode2Menu(characterCreationManager);
                    // characterCreationManager.AddNewMenu(degradedRogueKnightNode2Menu);
                    // var degradedRogueKnightNode3Menu = CreateDegradedRogueKnightNode3Menu(characterCreationManager);
                    // characterCreationManager.AddNewMenu(degradedRogueKnightNode3Menu);
                    // var degradedRogueKnightNode4Menu = CreateDegradedRogueKnightNode4Menu(characterCreationManager);
                    // characterCreationManager.AddNewMenu(degradedRogueKnightNode4Menu);
                    OriginLog.Info("已添加堕落无赖骑士4个节点菜单 (暂未实现)");

                    var nonPresetCultureMenu = CreateNonPresetCultureAnchorMenu(characterCreationManager);
                    characterCreationManager.AddNewMenu(nonPresetCultureMenu);
                    OriginLog.Info("已添非预文化锚点菜单");

                    var nonPresetSocialMenu = CreateNonPresetSocialOriginMenu(characterCreationManager);
                    characterCreationManager.AddNewMenu(nonPresetSocialMenu);
                    OriginLog.Info("已添加非预社会出身菜单");

                    // 添加非预设出身的风味节点菜单
                    var nonPresetNomadRoleMenu = CreateNonPresetNomadRoleMenu(characterCreationManager);
                    characterCreationManager.AddNewMenu(nonPresetNomadRoleMenu);
                    OriginLog.Info("已添加非预游牧营地角色菜单");

                    var nonPresetCraftsmanTradeMenu = CreateNonPresetCraftsmanTradeMenu(characterCreationManager);
                    characterCreationManager.AddNewMenu(nonPresetCraftsmanTradeMenu);
                    OriginLog.Info("已添加非预手工业者手菜单");

                    var nonPresetCaravanRoleMenu = CreateNonPresetCaravanRoleMenu(characterCreationManager);
                    characterCreationManager.AddNewMenu(nonPresetCaravanRoleMenu);
                    OriginLog.Info("已添非预商队角色菜单");

                    // 添加小氏族位置菜单
                    var nonPresetMinorClanPositionMenu = CreateNonPresetMinorClanPositionMenu(characterCreationManager);
                    characterCreationManager.AddNewMenu(nonPresetMinorClanPositionMenu);
                    OriginLog.Info("已添加非预设小氏族位置菜单");

                    // 添加流浪难民负担菜单
                    var nonPresetRefugeeBurdenMenu = CreateNonPresetRefugeeBurdenMenu(characterCreationManager);
                    characterCreationManager.AddNewMenu(nonPresetRefugeeBurdenMenu);
                    OriginLog.Info("已添加非预设流浪难民负担菜单");

                    // 添加可选公共节点菜单
                    var nonPresetStartLocationTypeMenu = CreateNonPresetStartLocationTypeMenu(characterCreationManager);
                    characterCreationManager.AddNewMenu(nonPresetStartLocationTypeMenu);
                    OriginLog.Info("已添加非预设起始位置类型菜单");

                    var nonPresetValuableItemMenu = CreateNonPresetValuableItemMenu(characterCreationManager);
                    characterCreationManager.AddNewMenu(nonPresetValuableItemMenu);
                    OriginLog.Info("已添加非预设贵重物品菜单");

                    var nonPresetContactTypeMenu = CreateNonPresetContactTypeMenu(characterCreationManager);
                    characterCreationManager.AddNewMenu(nonPresetContactTypeMenu);
                    OriginLog.Info("已添加非预设联系人类型菜单");

                    var nonPresetPeasantRoleMenu = CreateNonPresetPeasantRoleMenu(characterCreationManager);
                    characterCreationManager.AddNewMenu(nonPresetPeasantRoleMenu);
                    OriginLog.Info("已添加非预农村平民角色菜单");

                    var nonPresetUrbanPoorRoleMenu = CreateNonPresetUrbanPoorRoleMenu(characterCreationManager);
                    characterCreationManager.AddNewMenu(nonPresetUrbanPoorRoleMenu);
                    OriginLog.Info("已添加非预城镇贫民角色菜单");

                    var nonPresetSkillMenu = CreateNonPresetSkillBackgroundMenu(characterCreationManager);
                    characterCreationManager.AddNewMenu(nonPresetSkillMenu);
                    OriginLog.Info("已添加非预设技能背景菜单");

                    var nonPresetConditionMenu = CreateNonPresetStartingConditionMenu(characterCreationManager);
                    characterCreationManager.AddNewMenu(nonPresetConditionMenu);
                    OriginLog.Info("已添加非预设当前状态菜单");

                    // 4. 重新添加"父母菜单"(InputMenuId = "origin_type_selection")
                    if (parentMenu != null)
                    {
                        var recreatedParentMenu = RecreateParentMenuWithNewInputId(parentMenu, "origin_type_selection");
                        characterCreationManager.AddNewMenu(recreatedParentMenu);
                        OriginLog.Info("已重新添加父母菜单(InputMenuId = origin_type_selection)");
                    }
                }
                catch (Exception ex)
                {
                    OriginLog.Error($"AddParentsMenu Postfix 失败: {ex.Message}");
                    OriginLog.Error($"StackTrace: {ex.StackTrace}");
                }
            }
        }
    }
}
