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
        private static NarrativeMenu CreateNonPresetPeasantRoleMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "non_preset_peasant_role",
                "non_preset_social_origin",
                "non_preset_skill_background",
                new TextObject("你平时做哪类活"),
                new TextObject("选择你在农村的工作,这会影响你的技能和装备"),
                characters,
                GetNonPresetPeasantRoleCharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "peasant_role_farmer",
                new TextObject("农作/砍柴"),
                new TextObject("你从事农作,体力耐力技能,装备:草叉"),
                GetPeasantRoleFarmerArgs,
                (m) => true,
                (m) => PeasantRoleFarmerOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "peasant_role_hunter",
                new TextObject("猎户学徒"),
                new TextObject("你学习打猎,弓术生存技能,可能获得短弓"),
                GetPeasantRoleHunterArgs,
                (m) => true,
                (m) => PeasantRoleHunterOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "peasant_role_carter",
                new TextObject("赶车/送货"),
                new TextObject("你负责赶车送货.骑术贸易技能,金币+50"),
                GetPeasantRoleCarterArgs,
                (m) => true,
                (m) => PeasantRoleCarterOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "peasant_role_militia",
                new TextObject("村里民兵"),
                new TextObject("你是村里的民兵,单手盾牌技能,装备:木盾"),
                GetPeasantRoleMilitiaArgs,
                (m) => true,
                (m) => PeasantRoleMilitiaOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetNonPresetPeasantRoleCharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetPeasantRoleFarmerArgs(NarrativeMenuOptionArgs args) { }

        private static void PeasantRoleFarmerOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了农村平民角色-农作");
            OriginSystemHelper.NonPresetOrigin.FlavorNode = "peasant_role_farmer";
        }

        private static void GetPeasantRoleHunterArgs(NarrativeMenuOptionArgs args) { }

        private static void PeasantRoleHunterOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了农村平民角色-猎户");
            OriginSystemHelper.NonPresetOrigin.FlavorNode = "peasant_role_hunter";
        }

        private static void GetPeasantRoleCarterArgs(NarrativeMenuOptionArgs args) { }

        private static void PeasantRoleCarterOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了农村平民角色-赶车");
            OriginSystemHelper.NonPresetOrigin.FlavorNode = "peasant_role_carter";
        }

        private static void GetPeasantRoleMilitiaArgs(NarrativeMenuOptionArgs args) { }

        private static void PeasantRoleMilitiaOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了农村平民角色-民兵");
            OriginSystemHelper.NonPresetOrigin.FlavorNode = "peasant_role_militia";
        }
    }
}
