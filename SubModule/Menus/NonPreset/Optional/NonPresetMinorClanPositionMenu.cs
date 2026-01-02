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
        private static NarrativeMenu CreateNonPresetMinorClanPositionMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "non_preset_minor_clan_position",
                "non_preset_social_origin",
                "non_preset_skill_background",
                new TextObject("你在氏族旁支中是什么位置"),
                new TextObject("选择你在小氏族中的位置，这会影响你的技能和状态"),
                characters,
                (c, o, m) => new List<NarrativeMenuCharacterArgs>()
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "clan_pos_hostage",
                new TextObject("礼节性\"人质\""),
                new TextObject("你学会了忍耐与礼数，至少你不会轻易失态"),
                (args) => { },
                (m) => true,
                (m) => MinorClanPositionHostageOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "clan_pos_client",
                new TextObject("门客/侍从"),
                new TextObject("你知道如何服务强者，也知道如何借势"),
                (args) => { },
                (m) => true,
                (m) => MinorClanPositionClientOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "clan_pos_bastard",
                new TextObject("私生子/边缘血统"),
                new TextObject("你不被承认，但你也不必守他们的规矩"),
                (args) => { },
                (m) => true,
                (m) => MinorClanPositionBastardOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "clan_pos_ignored",
                new TextObject("被忽视的旁支"),
                new TextObject("没人管你，你也学会了自己管自己"),
                (args) => { },
                (m) => true,
                (m) => MinorClanPositionIgnoredOnSelect(m),
                null
            ));

            return menu;
        }

        private static void MinorClanPositionHostageOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择了小氏族位置-礼节性人质");
                OriginSystemHelper.SelectedNonPresetOriginNodes["np_node_minor_clan_position"] = "clan_pos_hostage";
                OriginSystemHelper.NonPresetOrigin.FlavorNode = "clan_pos_hostage";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"MinorClanPositionHostageOnSelect 失败: {ex.Message}");
            }
        }

        private static void MinorClanPositionClientOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择了小氏族位置-门客/侍从");
                OriginSystemHelper.SelectedNonPresetOriginNodes["np_node_minor_clan_position"] = "clan_pos_client";
                OriginSystemHelper.NonPresetOrigin.FlavorNode = "clan_pos_client";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"MinorClanPositionClientOnSelect 失败: {ex.Message}");
            }
        }

        private static void MinorClanPositionBastardOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择了小氏族位置-私生子/边缘血统");
                OriginSystemHelper.SelectedNonPresetOriginNodes["np_node_minor_clan_position"] = "clan_pos_bastard";
                OriginSystemHelper.NonPresetOrigin.FlavorNode = "clan_pos_bastard";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"MinorClanPositionBastardOnSelect 失败: {ex.Message}");
            }
        }

        private static void MinorClanPositionIgnoredOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择了小氏族位置-被忽视的旁支");
                OriginSystemHelper.SelectedNonPresetOriginNodes["np_node_minor_clan_position"] = "clan_pos_ignored";
                OriginSystemHelper.NonPresetOrigin.FlavorNode = "clan_pos_ignored";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"MinorClanPositionIgnoredOnSelect 失败: {ex.Message}");
            }
        }
    }
}

















