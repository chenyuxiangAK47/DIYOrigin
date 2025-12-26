using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using OriginSystemMod;

namespace OriginSystemMod
{
    public static partial class OriginSystemPatches
    {
        private static NarrativeMenu CreateNonPresetCaravanRoleMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "non_preset_caravan_duty",
                "non_preset_social_origin",
                "non_preset_skill_background",
                new TextObject("你在商队里负责什么"),
                new TextObject("选择你在商队中的职责，这会影响你的技能和状态"),
                characters,
                GetNonPresetCaravanRoleCharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "caravan_duty_guide",
                new TextObject("带路与看路"),
                new TextObject("路线和天气，有时比刀更致命"),
                GetCaravanDutyGuideArgs,
                (m) => true,
                (m) => CaravanDutyGuideOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "caravan_duty_guard",
                new TextObject("护卫"),
                new TextObject("你站在车队两侧，盯着草丛与山口"),
                GetCaravanDutyGuardArgs,
                (m) => true,
                (m) => CaravanDutyGuardOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "caravan_duty_muleteer",
                new TextObject("驮兽与货物管理"),
                new TextObject("你知道货在哪里，就知道钱在哪里"),
                GetCaravanDutyMuleteerArgs,
                (m) => true,
                (m) => CaravanDutyMuleteerOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "caravan_duty_broker",
                new TextObject("讨价还价/联络"),
                new TextObject("你不靠肌肉吃饭，靠嘴和信誉吃饭"),
                GetCaravanDutyBrokerArgs,
                (m) => true,
                (m) => CaravanDutyBrokerOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetNonPresetCaravanRoleCharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetCaravanDutyGuideArgs(NarrativeMenuOptionArgs args) { }

        private static void CaravanDutyGuideOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择了商队职责-带路与看路");
                OriginSystemHelper.SelectedNonPresetOriginNodes["np_node_caravan_duty"] = "caravan_duty_guide";
                OriginSystemHelper.NonPresetOrigin.FlavorNode = "caravan_duty_guide";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"CaravanDutyGuideOnSelect 失败: {ex.Message}");
            }
        }

        private static void GetCaravanDutyGuardArgs(NarrativeMenuOptionArgs args) { }

        private static void CaravanDutyGuardOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择了商队职责-护卫");
                OriginSystemHelper.SelectedNonPresetOriginNodes["np_node_caravan_duty"] = "caravan_duty_guard";
                OriginSystemHelper.NonPresetOrigin.FlavorNode = "caravan_duty_guard";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"CaravanDutyGuardOnSelect 失败: {ex.Message}");
            }
        }

        private static void GetCaravanDutyMuleteerArgs(NarrativeMenuOptionArgs args) { }

        private static void CaravanDutyMuleteerOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择了商队职责-驮兽与货物管理");
                OriginSystemHelper.SelectedNonPresetOriginNodes["np_node_caravan_duty"] = "caravan_duty_muleteer";
                OriginSystemHelper.NonPresetOrigin.FlavorNode = "caravan_duty_muleteer";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"CaravanDutyMuleteerOnSelect 失败: {ex.Message}");
            }
        }

        private static void GetCaravanDutyBrokerArgs(NarrativeMenuOptionArgs args) { }

        private static void CaravanDutyBrokerOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择了商队职责-讨价还价/联络");
                OriginSystemHelper.SelectedNonPresetOriginNodes["np_node_caravan_duty"] = "caravan_duty_broker";
                OriginSystemHelper.NonPresetOrigin.FlavorNode = "caravan_duty_broker";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"CaravanDutyBrokerOnSelect 失败: {ex.Message}");
            }
        }
    }
}
