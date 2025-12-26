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
        #region 迁徙王族节点菜单

        private static NarrativeMenu CreateWanderingPrinceNode1Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu("wandering_prince_node1", "preset_origin_selection", "wandering_prince_subnode", new TextObject("你为何流亡"), new TextObject("选择你流亡的原因,这会影响你的初始声望,关系和名声"), characters, GetWanderingPrinceNode1CharacterArgs);

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("royal_exile_usurped", new TextObject("被篡位者赶出故土"), new TextObject("你被篡位者赶出故土.声望+1,魅力+4,领导力+3,标记:政治债务"), GetRoyalExileUsurpedArgs, (m) => true, (m) => RoyalExileUsurpedOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("royal_exile_clan_war", new TextObject("内战败北,带人逃亡"), new TextObject("你在内战中败北,带人逃亡.声望+1,战术+4,领导力+3,标记:流离失所"), GetRoyalExileClanWarArgs, (m) => true, (m) => RoyalExileClanWarOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("royal_exile_foresight", new TextObject("预感风暴,先走一步"), new TextObject("你预感风暴,先走一步.侦察+4,管理+3,魅力+2,标记:谨慎"), GetRoyalExileForesightArgs, (m) => true, (m) => RoyalExileForesightOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("royal_exile_treasure", new TextObject("携宝逃亡,买路求生"), new TextObject("你携宝逃亡,买路求生.金钱+600,标记:短期富有,与精英氏族关系-1"), GetRoyalExileTreasureArgs, (m) => true, (m) => RoyalExileTreasureOnSelect(m), null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetWanderingPrinceNode1CharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetRoyalExileUsurpedArgs(NarrativeMenuOptionArgs args) { }
        private static void RoyalExileUsurpedOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择迁徙王族-Node1-王位被篡"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_royal_exile_reason"] = "usurped"; }

        private static void GetRoyalExileClanWarArgs(NarrativeMenuOptionArgs args) { }
        private static void RoyalExileClanWarOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择迁徙王族-Node1-宗族内斗失败"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_royal_exile_reason"] = "clan_war"; }

        private static void GetRoyalExileForesightArgs(NarrativeMenuOptionArgs args) { }
        private static void RoyalExileForesightOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了迁徙王族-Node1-避祸远走"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_royal_exile_reason"] = "foresight"; }

        private static void GetRoyalExileTreasureArgs(NarrativeMenuOptionArgs args) { }
        private static void RoyalExileTreasureOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了迁徙王族-Node1-携宝逃亡"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_royal_exile_reason"] = "treasure"; }

        private static NarrativeMenu CreateWanderingPrinceSubNodeMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "wandering_prince_subnode",
                "wandering_prince_node1",
                "wandering_prince_node3",
                new TextObject("选择你的部族来源"),
                new TextObject("你的家族来自哪个迁徙部族?这会影响你的初始装备,兵力和政治倾向"),
                characters,
                GetWanderingPrinceSubNodeCharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("wandering_prince_ochilan", new TextObject("斡赤兰诸部"), new TextObject("来自极北草原的部族联盟,以军功与长老议决维系统治,因草场衰退而整体西迁.骑射为核心,编制感强"), GetWanderingPrinceOchilanArgs, (m) => true, WanderingPrinceOchilanOnSelect, null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("wandering_prince_dashi", new TextObject("大石余庭"), new TextObject("内陆王庭覆灭后残存的贵族与军户联合体,仍保存旧法,印信与官制记忆.混编兵力,正统执念极强"), GetWanderingPrinceDashiArgs, (m) => true, WanderingPrinceDashiOnSelect, null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("wandering_prince_kashan", new TextObject("喀珊兀诸部"), new TextObject("活跃于湖泊与草原交界地带的部盟,熟悉贸易与掠袭,被强权挤出原生区域.轻骑为主,现实主义"), GetWanderingPrinceKashanArgs, (m) => true, WanderingPrinceKashanOnSelect, null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("wandering_prince_aigeli", new TextObject("艾格里军团"), new TextObject("原为边防军团与其家属形成的军事共同体,在宗主撤离后自行迁徙.重骑与军纪严明的步兵,效率至上"), GetWanderingPrinceAigeliArgs, (m) => true, WanderingPrinceAigeliOnSelect, null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("wandering_prince_tahun", new TextObject("蹋浑残部"), new TextObject("曾以速度与恐惧统治草原的旧部族,在内耗中崩解,只剩零散军群西走.超机动轻骑,装备混杂"), GetWanderingPrinceTahunArgs, (m) => true, WanderingPrinceTahunOnSelect, null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetWanderingPrinceSubNodeCharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetWanderingPrinceOchilanArgs(NarrativeMenuOptionArgs args) { }
        private static void WanderingPrinceOchilanOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择迁徙王族-斡赤兰诸部");
                OriginSystemHelper.SelectedPresetOriginNodes["khz_node_royal_followers"] = "ochilan";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"WanderingPrinceOchilanOnSelect 失败: {ex.Message}");
            }
        }

        private static void GetWanderingPrinceDashiArgs(NarrativeMenuOptionArgs args) { }
        private static void WanderingPrinceDashiOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择迁徙王族-大石余庭");
                OriginSystemHelper.SelectedPresetOriginNodes["khz_node_royal_followers"] = "dashi";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"WanderingPrinceDashiOnSelect 失败: {ex.Message}");
            }
        }

        private static void GetWanderingPrinceKashanArgs(NarrativeMenuOptionArgs args) { }
        private static void WanderingPrinceKashanOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择迁徙王族-喀珊兀诸部");
                OriginSystemHelper.SelectedPresetOriginNodes["khz_node_royal_followers"] = "kashan";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"WanderingPrinceKashanOnSelect 失败: {ex.Message}");
            }
        }

        private static void GetWanderingPrinceAigeliArgs(NarrativeMenuOptionArgs args) { }
        private static void WanderingPrinceAigeliOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择迁徙王族-艾格里军团");
                OriginSystemHelper.SelectedPresetOriginNodes["khz_node_royal_followers"] = "aigeli";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"WanderingPrinceAigeliOnSelect 失败: {ex.Message}");
            }
        }

        private static void GetWanderingPrinceTahunArgs(NarrativeMenuOptionArgs args) { }
        private static void WanderingPrinceTahunOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择迁徙王族-蹋浑残部");
                OriginSystemHelper.SelectedPresetOriginNodes["khz_node_royal_followers"] = "tahun";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"WanderingPrinceTahunOnSelect 失败: {ex.Message}");
            }
        }

        private static NarrativeMenu CreateWanderingPrinceNode3Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu("wandering_prince_node3", "wandering_prince_subnode", "wandering_prince_node4", new TextObject("你准备如何面对汗廷"), new TextObject("选择你对汗廷的策略,这会影响你的初始关系和标记"), characters, GetWanderingPrinceNode3CharacterArgs);

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("royal_strategy_infiltrate", new TextObject("潜入汗廷,慢慢夺权"), new TextObject("你选择潜入汗廷,慢慢夺权.魅力+5,管理+2,标记:寻求可汗青睐"), GetRoyalStrategyInfiltrateArgs, (m) => true, (m) => RoyalStrategyInfiltrateOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("royal_strategy_new_kingdom", new TextObject("自立新国,不认汗廷"), new TextObject("你选择自立新国,不认汗廷.领导力+4,战术+2,声望+1,标记:野心"), GetRoyalStrategyNewKingdomArgs, (m) => true, (m) => RoyalStrategyNewKingdomOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("royal_strategy_ally", new TextObject("先结盟,再谈野心"), new TextObject("你选择先结盟,再谈野心.魅力+3,与精英氏族关系+3,标记:寻求庇护者"), GetRoyalStrategyAllyArgs, (m) => true, (m) => RoyalStrategyAllyOnSelect(m), null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetWanderingPrinceNode3CharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetRoyalStrategyInfiltrateArgs(NarrativeMenuOptionArgs args) { }
        private static void RoyalStrategyInfiltrateOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了迁徙王族-Node3-融入汗廷"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_royal_strategy"] = "infiltrate"; }

        private static void GetRoyalStrategyNewKingdomArgs(NarrativeMenuOptionArgs args) { }
        private static void RoyalStrategyNewKingdomOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了迁徙王族-Node3-另立新国"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_royal_strategy"] = "new_kingdom"; }

        private static void GetRoyalStrategyAllyArgs(NarrativeMenuOptionArgs args) { }
        private static void RoyalStrategyAllyOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了迁徙王族-Node3-先投他国"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_royal_strategy"] = "ally"; }

        private static NarrativeMenu CreateWanderingPrinceNode4Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu("wandering_prince_node4", "wandering_prince_node3", "narrative_parent_menu", new TextObject("你用什么证明你是王"), new TextObject("选择你证明王权的象征物,这会影响你的声望和标记"), characters, GetWanderingPrinceNode4CharacterArgs);

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("royal_proof_seal", new TextObject("王印与家谱"), new TextObject("你拥有王印与家谱.声望+1,魅力+3,标记:拥有证明"), GetRoyalProofSealArgs, (m) => true, (m) => RoyalProofSealOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("royal_proof_banner", new TextObject("旧旗与誓言"), new TextObject("你拥有旧旗与誓言.声望+1,士气+4,标记:拥有象征物"), GetRoyalProofBannerArgs, (m) => true, (m) => RoyalProofBannerOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("royal_proof_wealth", new TextObject("金与马,能买到承认"), new TextObject("你用金与马,能买到承认.金钱+300,魅力+2,标记:短期富有"), GetRoyalProofWealthArgs, (m) => true, (m) => RoyalProofWealthOnSelect(m), null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetWanderingPrinceNode4CharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetRoyalProofSealArgs(NarrativeMenuOptionArgs args) { }
        private static void RoyalProofSealOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了迁徙王族-Node4-祖传印玺"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_royal_proof"] = "seal"; OriginSystemHelper.OriginSelectionDone = true; }

        private static void GetRoyalProofBannerArgs(NarrativeMenuOptionArgs args) { }
        private static void RoyalProofBannerOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了迁徙王族-Node4-史诗战旗"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_royal_proof"] = "banner"; OriginSystemHelper.OriginSelectionDone = true; }

        private static void GetRoyalProofWealthArgs(NarrativeMenuOptionArgs args) { }
        private static void RoyalProofWealthOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了迁徙王族-Node4-财富分赏"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_royal_proof"] = "wealth"; OriginSystemHelper.OriginSelectionDone = true; }

        #endregion
    }
}
