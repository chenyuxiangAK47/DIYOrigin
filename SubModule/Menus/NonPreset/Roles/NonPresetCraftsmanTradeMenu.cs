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
        private static NarrativeMenu CreateNonPresetCraftsmanTradeMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "non_preset_artisan_trade",
                "non_preset_social_origin",
                "non_preset_skill_background",
                new TextObject("你的手艺是什么"),
                new TextObject("选择你学习的手艺，这会影响你的技能和装备"),
                characters,
                GetNonPresetCraftsmanTradeCharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "artisan_trade_smith",
                new TextObject("铁匠学徒"),
                new TextObject("你最懂\"耐用\"二字"),
                GetArtisanTradeSmithArgs,
                (m) => true,
                (m) => ArtisanTradeSmithOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "artisan_trade_leather",
                new TextObject("皮匠/制甲学徒"),
                new TextObject("你知道一层皮能救命，也能挣钱"),
                GetArtisanTradeLeatherArgs,
                (m) => true,
                (m) => ArtisanTradeLeatherOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "artisan_trade_carpenter",
                new TextObject("木匠/车架学徒"),
                new TextObject("你懂结构，也懂怎么省力"),
                GetArtisanTradeCarpenterArgs,
                (m) => true,
                (m) => ArtisanTradeCarpenterOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "artisan_trade_clerk",
                new TextObject("账房/跑腿学徒"),
                new TextObject("你学会了记账、算数、和说话的分寸"),
                GetArtisanTradeClerkArgs,
                (m) => true,
                (m) => ArtisanTradeClerkOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetNonPresetCraftsmanTradeCharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetArtisanTradeSmithArgs(NarrativeMenuOptionArgs args) { }

        private static void ArtisanTradeSmithOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择了手工业者手艺-铁匠学徒");
                OriginSystemHelper.SelectedNonPresetOriginNodes["np_node_artisan_trade"] = "artisan_trade_smith";
                OriginSystemHelper.NonPresetOrigin.FlavorNode = "artisan_trade_smith";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ArtisanTradeSmithOnSelect 失败: {ex.Message}");
            }
        }

        private static void GetArtisanTradeLeatherArgs(NarrativeMenuOptionArgs args) { }

        private static void ArtisanTradeLeatherOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择了手工业者手艺-皮匠/制甲学徒");
                OriginSystemHelper.SelectedNonPresetOriginNodes["np_node_artisan_trade"] = "artisan_trade_leather";
                OriginSystemHelper.NonPresetOrigin.FlavorNode = "artisan_trade_leather";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ArtisanTradeLeatherOnSelect 失败: {ex.Message}");
            }
        }

        private static void GetArtisanTradeCarpenterArgs(NarrativeMenuOptionArgs args) { }

        private static void ArtisanTradeCarpenterOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择了手工业者手艺-木匠/车架学徒");
                OriginSystemHelper.SelectedNonPresetOriginNodes["np_node_artisan_trade"] = "artisan_trade_carpenter";
                OriginSystemHelper.NonPresetOrigin.FlavorNode = "artisan_trade_carpenter";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ArtisanTradeCarpenterOnSelect 失败: {ex.Message}");
            }
        }

        private static void GetArtisanTradeClerkArgs(NarrativeMenuOptionArgs args) { }

        private static void ArtisanTradeClerkOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择了手工业者手艺-账房/跑腿学徒");
                OriginSystemHelper.SelectedNonPresetOriginNodes["np_node_artisan_trade"] = "artisan_trade_clerk";
                OriginSystemHelper.NonPresetOrigin.FlavorNode = "artisan_trade_clerk";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ArtisanTradeClerkOnSelect 失败: {ex.Message}");
            }
        }
    }
}
