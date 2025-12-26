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
        private static NarrativeMenu CreateNonPresetSkillBackgroundMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "non_preset_skill_background",
                "non_preset_social_origin",
                "non_preset_starting_condition",
                new TextObject("选择技能来源"),
                new TextObject("选择你靠什么活下来"),
                characters,
                GetNonPresetSkillBackgroundCharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "bg_herd_hunt",
                new TextObject("放牧与狩猎"),
                new TextObject("你靠观察、耐心、和一支还能拉开的弓活下来"),
                GetSkillBackgroundArgs,
                (m) => true,
                (m) => NonPresetSkillBackgroundOnSelect(m, "bg_herd_hunt"),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "bg_village_defense",
                new TextObject("村庄自卫"),
                new TextObject("你学会拿起盾与刀，因为你不想再看见家门被烧"),
                GetSkillBackgroundArgs,
                (m) => true,
                (m) => NonPresetSkillBackgroundOnSelect(m, "bg_village_defense"),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "bg_caravan_guard",
                new TextObject("商队护卫"),
                new TextObject("你护过货，也护过命。你知道什么时候冲，什么时候撤"),
                GetSkillBackgroundArgs,
                (m) => true,
                (m) => NonPresetSkillBackgroundOnSelect(m, "bg_caravan_guard"),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "bg_merc_helper",
                new TextObject("雇佣兵杂役"),
                new TextObject("你不是军官，但你见过真正的战争：烂泥、血、和逃兵"),
                GetSkillBackgroundArgs,
                (m) => true,
                (m) => NonPresetSkillBackgroundOnSelect(m, "bg_merc_helper"),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "bg_border_patrol",
                new TextObject("边境巡逻"),
                new TextObject("你习惯了荒野、风雪、和看不见的敌意"),
                GetSkillBackgroundArgs,
                (m) => true,
                (m) => NonPresetSkillBackgroundOnSelect(m, "bg_border_patrol"),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "bg_city_apprentice",
                new TextObject("城镇学徒"),
                new TextObject("你学会了如何把一件东西修好，也学会了如何把账算清"),
                GetSkillBackgroundArgs,
                (m) => true,
                (m) => NonPresetSkillBackgroundOnSelect(m, "bg_city_apprentice"),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "bg_poach_smuggle",
                new TextObject("偷猎/走私"),
                new TextObject("你不喜欢惹麻烦，但你更不喜欢饿死。你学会绕开规则"),
                GetSkillBackgroundArgs,
                (m) => true,
                (m) => NonPresetSkillBackgroundOnSelect(m, "bg_poach_smuggle"),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "bg_conscripted",
                new TextObject("临时征召兵"),
                new TextObject("你被征召、服役、又被遣散。你不欠任何人，也没人欠你"),
                GetSkillBackgroundArgs,
                (m) => true,
                (m) => NonPresetSkillBackgroundOnSelect(m, "bg_conscripted"),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetNonPresetSkillBackgroundCharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetSkillBackgroundArgs(NarrativeMenuOptionArgs args)
        {
            // 技能来源影响技能和熟练度,在角色创建阶段应用
        }

        private static void NonPresetSkillBackgroundOnSelect(CharacterCreationManager manager, string skillBackgroundId)
        {
            try
            {
                OriginLog.Info($"用户选择了技能背景 {skillBackgroundId}");
                OriginSystemHelper.SelectedNonPresetOriginNodes["np_node_skill_background"] = skillBackgroundId;
                OriginSystemHelper.NonPresetOrigin.SkillBackground = skillBackgroundId;
            }
            catch (Exception ex)
            {
                OriginLog.Error($"NonPresetSkillBackgroundOnSelect 失败: {ex.Message}");
            }
        }
    }
}
