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
        private static NarrativeMenu CreateNonPresetSocialOriginMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "non_preset_social_origin",
                "origin_type_selection",
                "non_preset_skill_background",
                new TextObject("选择社会出身"),
                new TextObject("选择你来自社会的哪一层"),
                characters,
                GetNonPresetSocialOriginCharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "social_origin_rural_peasant",
                new TextObject("农村平民"),
                new TextObject("你从田地和牲口之间长大，见过饥荒，也见过征召"),
                GetSocialOriginArgs,
                (m) => true,
                (m) => NonPresetSocialOriginOnSelect(m, "social_origin_rural_peasant"),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "social_origin_nomad_camp",
                new TextObject("游牧营地成员"),
                new TextObject("你熟悉营地的规矩、马群的脾气、以及草原的风向"),
                GetSocialOriginArgs,
                (m) => true,
                (m) => NonPresetSocialOriginOnSelect(m, "social_origin_nomad_camp"),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "social_origin_urban_poor",
                new TextObject("城镇贫民"),
                new TextObject("你在城墙阴影里讨生活，懂得察言观色，也更懂得饥饿"),
                GetSocialOriginArgs,
                (m) => true,
                (m) => NonPresetSocialOriginOnSelect(m, "social_origin_urban_poor"),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "social_origin_urban_artisan",
                new TextObject("城镇手工业者"),
                new TextObject("你有一门手艺，哪怕只是学徒，也比很多人更接近\"稳定\""),
                GetSocialOriginArgs,
                (m) => true,
                (m) => NonPresetSocialOriginOnSelect(m, "social_origin_urban_artisan"),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "social_origin_caravan_follower",
                new TextObject("商队随行者"),
                new TextObject("你在商路上见过更多世界：税口、盗匪、以及真正的价格"),
                GetSocialOriginArgs,
                (m) => true,
                (m) => NonPresetSocialOriginOnSelect(m, "social_origin_caravan_follower"),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "social_origin_minor_clan",
                new TextObject("小氏族旁支"),
                new TextObject("你不是大人物，但你至少知道\"谁该点头、谁该闭嘴\""),
                GetSocialOriginArgs,
                (m) => true,
                (m) => NonPresetSocialOriginOnSelect(m, "social_origin_minor_clan"),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "social_origin_wanderer",
                new TextObject("流浪难民"),
                new TextObject("你习惯了背着家当走路。你活下来本身就是一种能力"),
                GetSocialOriginArgs,
                (m) => true,
                (m) => NonPresetSocialOriginOnSelect(m, "social_origin_wanderer"),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetNonPresetSocialOriginCharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetSocialOriginArgs(NarrativeMenuOptionArgs args)
        {
            // 社会出身影响初始金钱和装备,在角色创建阶段应用
        }

        private static void NonPresetSocialOriginOnSelect(CharacterCreationManager manager, string socialOriginId)
        {
            try
            {
                OriginLog.Info($"用户选择了社会出身 {socialOriginId}");
                OriginSystemHelper.SelectedNonPresetOriginNodes["np_node_social_origin"] = socialOriginId;
                OriginSystemHelper.NonPresetOrigin.SocialOrigin = socialOriginId;
            }
            catch (Exception ex)
            {
                OriginLog.Error($"NonPresetSocialOriginOnSelect 失败: {ex.Message}");
            }
        }
    }
}
