using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using OriginSystemMod;

namespace OriginSystemMod
{
    public static partial class OriginSystemPatches
    {
        private static NarrativeMenu CreateNonPresetValuableItemMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "non_preset_valuable_item",
                "non_preset_starting_condition",
                "narrative_parent_menu",
                new TextObject("你身上最值钱的是什么"),
                new TextObject("选择你拥有的最值钱的物品，这会影响你的初始装备和资源"),
                characters,
                (c, o, m) => new List<NarrativeMenuCharacterArgs>()
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "val_good_knife",
                new TextObject("一把还算像样的刀"),
                new TextObject("至少你不会拿农具去拼命。"),
                (args) => { },
                (m) => true,
                (m) => ValuableItemGoodKnifeOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "val_warm_coat",
                new TextObject("一件厚一点的外套"),
                new TextObject("你不怕冷，你只怕撑不到下一顿饭。"),
                (args) => { },
                (m) => true,
                (m) => ValuableItemWarmCoatOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "val_pack_mule",
                new TextObject("一匹破驮马"),
                new TextObject("你能带更多货，也更像个\"在做事的人\"。"),
                (args) => { },
                (m) => true,
                (m) => ValuableItemPackMuleOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "val_small_savings",
                new TextObject("一点积蓄"),
                new TextObject("你不富，但你至少能先撑一阵子。"),
                (args) => { },
                (m) => true,
                (m) => ValuableItemSmallSavingsOnSelect(m),
                null
            ));

            return menu;
        }

        private static void ValuableItemGoodKnifeOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择了贵重物品-一把还算像样的刀");
                OriginSystemHelper.SelectedNonPresetOriginNodes["np_node_valuable_item"] = "val_good_knife";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ValuableItemGoodKnifeOnSelect 失败: {ex.Message}");
            }
        }

        private static void ValuableItemWarmCoatOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择了贵重物品-一件厚一点的外套");
                OriginSystemHelper.SelectedNonPresetOriginNodes["np_node_valuable_item"] = "val_warm_coat";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ValuableItemWarmCoatOnSelect 失败: {ex.Message}");
            }
        }

        private static void ValuableItemPackMuleOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择了贵重物品-一匹破驮马");
                OriginSystemHelper.SelectedNonPresetOriginNodes["np_node_valuable_item"] = "val_pack_mule";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ValuableItemPackMuleOnSelect 失败: {ex.Message}");
            }
        }

        private static void ValuableItemSmallSavingsOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择了贵重物品-一点积蓄");
                OriginSystemHelper.SelectedNonPresetOriginNodes["np_node_valuable_item"] = "val_small_savings";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ValuableItemSmallSavingsOnSelect 失败: {ex.Message}");
            }
        }
    }
}














