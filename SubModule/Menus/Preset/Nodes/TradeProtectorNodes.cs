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
        #region 草原商路守护者节点菜单
        private static NarrativeMenu CreateTradeProtectorNode1Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu("trade_protector_node1", "preset_origin_selection", "trade_protector_node2", new TextObject("你守护哪条商路"), new TextObject("选择你守护的商路,这会影响你的初始关系和通缉状态"), characters, GetTradeProtectorNode1CharacterArgs);

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("trade_route_empire", new TextObject("通往帝国边境"), new TextObject("你守护通往帝国边境的商路。贸易+3，侦察+2，标记：在路上"), GetTradeRouteEmpireArgs, (m) => true, (m) => TradeRouteEmpireOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("trade_route_desert", new TextObject("通往沙漠诸邦"), new TextObject("你守护通往沙漠诸邦的商路。贸易+3，侦察+2，标记：在路上"), GetTradeRouteDesertArgs, (m) => true, (m) => TradeRouteDesertOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("trade_route_internal", new TextObject("汗国内部走廊"), new TextObject("你守护汗国内部走廊的商路。管理+3，魅力+2"), GetTradeRouteInternalArgs, (m) => true, (m) => TradeRouteInternalOnSelect(m), null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetTradeProtectorNode1CharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetTradeRouteEmpireArgs(NarrativeMenuOptionArgs args) { }
        private static void TradeRouteEmpireOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择草原商路守护Node1-草原帝国边境"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_trade_route"] = "empire"; }

        private static void GetTradeRouteDesertArgs(NarrativeMenuOptionArgs args) { }
        private static void TradeRouteDesertOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择草原商路守护Node1-草原沙漠"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_trade_route"] = "desert"; }

        private static void GetTradeRouteInternalArgs(NarrativeMenuOptionArgs args) { }
        private static void TradeRouteInternalOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择草原商路守护Node1-草原内部"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_trade_route"] = "internal"; }

        private static NarrativeMenu CreateTradeProtectorNode2Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu("trade_protector_node2", "trade_protector_node1", "trade_protector_node3", new TextObject("你靠什么吃饭"), new TextObject("选择你的收入来源,这会影响你的初始金币和名声"), characters, GetTradeProtectorNode2CharacterArgs);

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("trade_model_guard_fee", new TextObject("正规护路费"), new TextObject("你靠收取正规护路费为生。金钱+250，贸易+3，与商人关系+3，标记：合法生意"), GetTradeModelGuardFeeArgs, (m) => true, (m) => TradeModelGuardFeeOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("trade_model_smuggling", new TextObject("夹带走私"), new TextObject("你靠夹带走私为生。金钱+350，贸易+3，狡诈+3，与商人关系-2，标记：灰色贸易"), GetTradeModelSmugglingArgs, (m) => true, (m) => TradeModelSmugglingOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("trade_model_monopoly", new TextObject("垄断一段商路"), new TextObject("你靠垄断一段商路为生。金钱+400，贸易+2，领导力+2，与商人关系-4，标记：争议"), GetTradeModelMonopolyArgs, (m) => true, (m) => TradeModelMonopolyOnSelect(m), null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetTradeProtectorNode2CharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetTradeModelGuardFeeArgs(NarrativeMenuOptionArgs args) { }
        private static void TradeModelGuardFeeOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择草原商路守护Node2-护卫"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_trade_model"] = "guard_fee"; }

        private static void GetTradeModelSmugglingArgs(NarrativeMenuOptionArgs args) { }
        private static void TradeModelSmugglingOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择草原商路守护Node2-灰色走私"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_trade_model"] = "smuggling"; }

        private static void GetTradeModelMonopolyArgs(NarrativeMenuOptionArgs args) { }
        private static void TradeModelMonopolyOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了草原商路守护Node2-垄断补给"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_trade_model"] = "monopoly"; }

        private static NarrativeMenu CreateTradeProtectorNode3Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu("trade_protector_node3", "trade_protector_node2", "trade_protector_node4", new TextObject("你的护卫队是什么风格"), new TextObject("选择你的护卫队风格,这会影响你的初始兵力"), characters, GetTradeProtectorNode3CharacterArgs);

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("trade_guard_light_cav", new TextObject("轻骑巡逻"), new TextObject("你的护卫队是轻骑巡逻。初始兵力：轻骑兵×10，游牧战士×4，食物×6"), GetTradeGuardLightCavArgs, (m) => true, (m) => TradeGuardLightCavOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("trade_guard_horse_archer", new TextObject("骑射护路"), new TextObject("你的护卫队是骑射护路。初始兵力：骑射手×12，游牧战士×4，食物×6"), GetTradeGuardHorseArcherArgs, (m) => true, (m) => TradeGuardHorseArcherOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("trade_guard_mixed", new TextObject("混编护卫"), new TextObject("你的护卫队是混编护卫。初始兵力：骑射手×8，轻骑兵×6，游牧战士×4，食物×7"), GetTradeGuardMixedArgs, (m) => true, (m) => TradeGuardMixedOnSelect(m), null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetTradeProtectorNode3CharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetTradeGuardLightCavArgs(NarrativeMenuOptionArgs args) { }
        private static void TradeGuardLightCavOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了草原商路守护Node3-轻骑护卫"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_trade_guard"] = "light_cav"; }

        private static void GetTradeGuardHorseArcherArgs(NarrativeMenuOptionArgs args) { }
        private static void TradeGuardHorseArcherOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了草原商路守护Node3-弓骑护卫"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_trade_guard"] = "horse_archer"; }

        private static void GetTradeGuardMixedArgs(NarrativeMenuOptionArgs args) { }
        private static void TradeGuardMixedOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了草原商路守护Node3-混编护卫"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_trade_guard"] = "mixed"; }

        private static NarrativeMenu CreateTradeProtectorNode4Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu("trade_protector_node4", "trade_protector_node3", "narrative_parent_menu", new TextObject("你对汗廷的态度"), new TextObject("选择你对汗廷的态度,这会影响你的初始关系和名声"), characters, GetTradeProtectorNode4CharacterArgs);

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("trade_attitude_cooperate", new TextObject("合作（拿合法性）"), new TextObject("你选择与汗廷合作，拿合法性。与库塞特关系+2，与可汗关系+1，标记：寻求可汗青睐"), GetTradeAttitudeCooperateArgs, (m) => true, (m) => TradeAttitudeCooperateOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("trade_attitude_neutral", new TextObject("中立（两边不得罪）"), new TextObject("你保持中立，两边不得罪。魅力+2，标记：低调行事"), GetTradeAttitudeNeutralArgs, (m) => true, (m) => TradeAttitudeNeutralOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("trade_attitude_resist", new TextObject("抗拒（不想被管）"), new TextObject("你选择抗拒，不想被管。狡诈+2，与库塞特关系-2，标记：被怀疑"), GetTradeAttitudeResistArgs, (m) => true, (m) => TradeAttitudeResistOnSelect(m), null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetTradeProtectorNode4CharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetTradeAttitudeCooperateArgs(NarrativeMenuOptionArgs args) { }
        private static void TradeAttitudeCooperateOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了草原商路守护Node4-合作"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_trade_attitude"] = "cooperate"; OriginSystemHelper.OriginSelectionDone = true; }

        private static void GetTradeAttitudeNeutralArgs(NarrativeMenuOptionArgs args) { }
        private static void TradeAttitudeNeutralOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了草原商路守护Node4-中立"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_trade_attitude"] = "neutral"; OriginSystemHelper.OriginSelectionDone = true; }

        private static void GetTradeAttitudeResistArgs(NarrativeMenuOptionArgs args) { }
        private static void TradeAttitudeResistOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了草原商路守护Node4-对抗"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_trade_attitude"] = "resist"; OriginSystemHelper.OriginSelectionDone = true; }

        #endregion
    }
}
