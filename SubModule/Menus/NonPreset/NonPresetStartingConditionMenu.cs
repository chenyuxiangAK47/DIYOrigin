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
        private static NarrativeMenu CreateNonPresetStartingConditionMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "non_preset_starting_condition",
                "non_preset_skill_background",
                "narrative_parent_menu",
                new TextObject("选择当前状态"),
                new TextObject("选择你的开局状态"),
                characters,
                GetNonPresetStartingConditionCharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "sc_alone",
                new TextObject("孤身一人（最高自由）"),
                new TextObject("你没有负担，也没有依靠。只有你自己"),
                GetStartingConditionArgs,
                (m) => true,
                (m) => NonPresetStartingConditionOnSelect(m, "sc_alone"),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "sc_with_friends",
                new TextObject("1-2名同伴"),
                new TextObject("你们互相看着背后，至少不会被一句话骗走性命"),
                GetStartingConditionArgs,
                (m) => true,
                (m) => NonPresetStartingConditionOnSelect(m, "sc_with_friends"),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "sc_with_commoners",
                new TextObject("3-5名平民"),
                new TextObject("人多更安全，但也更耗粮、更吵、更难管"),
                GetStartingConditionArgs,
                (m) => true,
                (m) => NonPresetStartingConditionOnSelect(m, "sc_with_commoners"),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "sc_cash_start",
                new TextObject("少量启动资金（无人）"),
                new TextObject("你决定先用钱打开局面：买马、买粮、买一条路"),
                GetStartingConditionArgs,
                (m) => true,
                (m) => NonPresetStartingConditionOnSelect(m, "sc_cash_start"),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "sc_in_debt",
                new TextObject("欠债开局（高压但有钱）"),
                new TextObject("你带着借来的钱上路。你不是穷，你是\"被催债\""),
                GetStartingConditionArgs,
                (m) => true,
                (m) => NonPresetStartingConditionOnSelect(m, "sc_in_debt"),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "sc_injured_or_tired",
                new TextObject("轻伤/疲惫开局（沉浸难度）"),
                new TextObject("你不是满状态上路：要么伤口没好，要么几天没睡"),
                GetStartingConditionArgs,
                (m) => true,
                (m) => NonPresetStartingConditionOnSelect(m, "sc_injured_or_tired"),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetNonPresetStartingConditionCharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetStartingConditionArgs(NarrativeMenuOptionArgs args)
        {
            // 当前状态影响初始队伍和金币,在角色创建阶段应用
        }

        private static void NonPresetStartingConditionOnSelect(CharacterCreationManager manager, string startingConditionId)
        {
            try
            {
                OriginLog.Info($"用户选择了当前状态 {startingConditionId}");
                OriginSystemHelper.SelectedNonPresetOriginNodes["np_node_starting_condition"] = startingConditionId;
                OriginSystemHelper.NonPresetOrigin.StartingCondition = startingConditionId;
                OriginSystemHelper.OriginSelectionDone = true;
            }
            catch (Exception ex)
            {
                OriginLog.Error($"NonPresetStartingConditionOnSelect 失败: {ex.Message}");
            }
        }
    }
}
