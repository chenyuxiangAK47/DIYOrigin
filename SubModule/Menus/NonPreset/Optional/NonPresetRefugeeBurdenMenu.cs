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
        private static NarrativeMenu CreateNonPresetRefugeeBurdenMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "non_preset_refugee_burden",
                "non_preset_social_origin",
                "non_preset_skill_background",
                new TextObject("你背负了什么"),
                new TextObject("选择你作为流浪难民时的负担，这会影响你的状态和资源"),
                characters,
                (c, o, m) => new List<NarrativeMenuCharacterArgs>()
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "ref_burden_sick",
                new TextObject("有病人要照顾"),
                new TextObject("你走得更慢，但你不会丢下他们"),
                (args) => { },
                (m) => true,
                (m) => RefugeeBurdenSickOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "ref_burden_child",
                new TextObject("带着孩子/家眷"),
                new TextObject("你更缺粮，但你更像\"一个活着的人\""),
                (args) => { },
                (m) => true,
                (m) => RefugeeBurdenChildOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "ref_burden_owed",
                new TextObject("欠人情/欠命"),
                new TextObject("你拿到了一点帮助，但你心里知道迟早要还"),
                (args) => { },
                (m) => true,
                (m) => RefugeeBurdenOwedOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "ref_burden_nothing",
                new TextObject("什么都没了"),
                new TextObject("你轻装上路，至少你还能跑"),
                (args) => { },
                (m) => true,
                (m) => RefugeeBurdenNothingOnSelect(m),
                null
            ));

            return menu;
        }

        private static void RefugeeBurdenSickOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择了流浪难民负担-有病人要照顾");
                OriginSystemHelper.SelectedNonPresetOriginNodes["np_node_refugee_burden"] = "ref_burden_sick";
                OriginSystemHelper.NonPresetOrigin.FlavorNode = "ref_burden_sick";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"RefugeeBurdenSickOnSelect 失败: {ex.Message}");
            }
        }

        private static void RefugeeBurdenChildOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择了流浪难民负担-带着孩子/家眷");
                OriginSystemHelper.SelectedNonPresetOriginNodes["np_node_refugee_burden"] = "ref_burden_child";
                OriginSystemHelper.NonPresetOrigin.FlavorNode = "ref_burden_child";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"RefugeeBurdenChildOnSelect 失败: {ex.Message}");
            }
        }

        private static void RefugeeBurdenOwedOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择了流浪难民负担-欠人情/欠命");
                OriginSystemHelper.SelectedNonPresetOriginNodes["np_node_refugee_burden"] = "ref_burden_owed";
                OriginSystemHelper.NonPresetOrigin.FlavorNode = "ref_burden_owed";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"RefugeeBurdenOwedOnSelect 失败: {ex.Message}");
            }
        }

        private static void RefugeeBurdenNothingOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择了流浪难民负担-什么都没了");
                OriginSystemHelper.SelectedNonPresetOriginNodes["np_node_refugee_burden"] = "ref_burden_nothing";
                OriginSystemHelper.NonPresetOrigin.FlavorNode = "ref_burden_nothing";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"RefugeeBurdenNothingOnSelect 失败: {ex.Message}");
            }
        }
    }
}






