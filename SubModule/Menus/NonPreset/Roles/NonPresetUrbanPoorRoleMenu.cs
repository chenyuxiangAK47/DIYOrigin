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
        private static NarrativeMenu CreateNonPresetUrbanPoorRoleMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "non_preset_urban_poor_role",
                "non_preset_social_origin",
                "non_preset_skill_background",
                new TextObject("你在城市里做什么"),
                new TextObject("选择你在城市中的工作,这会影响你的技能和状态"),
                characters,
                GetNonPresetUrbanPoorRoleCharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "urban_poor_role_errand",
                new TextObject("跑腿"),
                new TextObject("你靠跑腿为生.敏捷社交技能,认识一些店主"),
                GetUrbanPoorRoleErrandArgs,
                (m) => true,
                (m) => UrbanPoorRoleErrandOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "urban_poor_role_dockworker",
                new TextObject("码头搬运"),
                new TextObject("你在码头做搬运,体力耐力技能,疲惫状态"),
                GetUrbanPoorRoleDockworkerArgs,
                (m) => true,
                (m) => UrbanPoorRoleDockworkerOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "urban_poor_role_gambler",
                new TextObject("赌徒"),
                new TextObject("你靠赌博为生.狡诈社交技能,可能欠债"),
                GetUrbanPoorRoleGamblerArgs,
                (m) => true,
                (m) => UrbanPoorRoleGamblerOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "urban_poor_role_fighter",
                new TextObject("地下拳手"),
                new TextObject("你在地下拳场打拳.单手体力技能,可能轻伤"),
                GetUrbanPoorRoleFighterArgs,
                (m) => true,
                (m) => UrbanPoorRoleFighterOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "urban_poor_role_shop_apprentice",
                new TextObject("店铺学徒"),
                new TextObject("你在店铺做学徒,贸易工艺技能,认识店主"),
                GetUrbanPoorRoleShopApprenticeArgs,
                (m) => true,
                (m) => UrbanPoorRoleShopApprenticeOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetNonPresetUrbanPoorRoleCharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetUrbanPoorRoleErrandArgs(NarrativeMenuOptionArgs args) { }

        private static void UrbanPoorRoleErrandOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了城镇贫民角色-跑腿");
            OriginSystemHelper.NonPresetOrigin.FlavorNode = "urban_poor_role_errand";
        }

        private static void GetUrbanPoorRoleDockworkerArgs(NarrativeMenuOptionArgs args) { }

        private static void UrbanPoorRoleDockworkerOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了城镇贫民角色-码头搬运");
            OriginSystemHelper.NonPresetOrigin.FlavorNode = "urban_poor_role_dockworker";
        }

        private static void GetUrbanPoorRoleGamblerArgs(NarrativeMenuOptionArgs args) { }

        private static void UrbanPoorRoleGamblerOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了城镇贫民角色-赌徒");
            OriginSystemHelper.NonPresetOrigin.FlavorNode = "urban_poor_role_gambler";
        }

        private static void GetUrbanPoorRoleFighterArgs(NarrativeMenuOptionArgs args) { }

        private static void UrbanPoorRoleFighterOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了城镇贫民角色-地下拳手");
            OriginSystemHelper.NonPresetOrigin.FlavorNode = "urban_poor_role_fighter";
        }

        private static void GetUrbanPoorRoleShopApprenticeArgs(NarrativeMenuOptionArgs args) { }

        private static void UrbanPoorRoleShopApprenticeOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了城镇贫民角色-店铺学徒");
            OriginSystemHelper.NonPresetOrigin.FlavorNode = "urban_poor_role_shop_apprentice";
        }
    }
}
