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
        #region 草原自由民节点菜单
        private static NarrativeMenu CreateFreeCossackNode1Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu("free_cossack_node1", "preset_origin_selection", "free_cossack_node2", new TextObject("你为何来到草原"), new TextObject("选择你来到草原的原因,这会影响你的初始名声和关系"), characters, GetFreeCossackNode1CharacterArgs);

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("cossack_arrival_fugitive", new TextObject("逃亡者"), new TextObject("你因犯事而逃亡。侦察+4，狡诈+2，标记：在路上"), GetCossackArrivalFugitiveArgs, (m) => true, (m) => CossackArrivalFugitiveOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("cossack_arrival_profit", new TextObject("逐利者"), new TextObject("你为逐利而来。贸易+4，金钱+200，标记：利益优先"), GetCossackArrivalProfitArgs, (m) => true, (m) => CossackArrivalProfitOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("cossack_arrival_exile", new TextObject("流放者"), new TextObject("你被放逐，政治失败。管理+3，体力+2，标记：流离失所"), GetCossackArrivalExileArgs, (m) => true, (m) => CossackArrivalExileOnSelect(m), null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetFreeCossackNode1CharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetCossackArrivalFugitiveArgs(NarrativeMenuOptionArgs args) { }
        private static void CossackArrivalFugitiveOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了草原自由民-Node1-逃亡者"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_cossack_arrival"] = "fugitive"; }

        private static void GetCossackArrivalProfitArgs(NarrativeMenuOptionArgs args) { }
        private static void CossackArrivalProfitOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了草原自由民-Node1-逐利者"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_cossack_arrival"] = "profit"; }

        private static void GetCossackArrivalExileArgs(NarrativeMenuOptionArgs args) { }
        private static void CossackArrivalExileOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了草原自由民-Node1-被放逐"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_cossack_arrival"] = "exile"; }

        private static NarrativeMenu CreateFreeCossackNode2Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu("free_cossack_node2", "free_cossack_node1", "free_cossack_node3", new TextObject("你怎么活下来"), new TextObject("选择你的生存方式,这会影响你的初始资源和技能倾向"), characters, GetFreeCossackNode2CharacterArgs);

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("cossack_survive_mercenary", new TextObject("靠刀吃饭"), new TextObject("你靠刀吃饭。领导力+3，战术+3"), GetCossackSurviveMercenaryArgs, (m) => true, (m) => CossackSurviveMercenaryOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("cossack_survive_hunt", new TextObject("靠狩猎与护路"), new TextObject("你靠狩猎与护路。弓术+3，侦察+4"), GetCossackSurviveHuntArgs, (m) => true, (m) => CossackSurviveHuntOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("cossack_survive_smuggle", new TextObject("靠走私与灰市"), new TextObject("你靠走私与灰市。贸易+2，狡诈+3，金钱+150，标记：灰色贸易"), GetCossackSurviveSmuggleArgs, (m) => true, (m) => CossackSurviveSmuggleOnSelect(m), null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetFreeCossackNode2CharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetCossackSurviveMercenaryArgs(NarrativeMenuOptionArgs args) { }
        private static void CossackSurviveMercenaryOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择草原自由民-Node2-当雇佣骑兵"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_cossack_survive"] = "mercenary"; }

        private static void GetCossackSurviveHuntArgs(NarrativeMenuOptionArgs args) { }
        private static void CossackSurviveHuntOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择草原自由民-Node2-打猎放牧"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_cossack_survive"] = "hunt"; }

        private static void GetCossackSurviveSmuggleArgs(NarrativeMenuOptionArgs args) { }
        private static void CossackSurviveSmuggleOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了草原自由民-Node2-走私抢掠"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_cossack_survive"] = "smuggle"; }

        private static NarrativeMenu CreateFreeCossackNode3Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu("free_cossack_node3", "free_cossack_node2", "free_cossack_node4", new TextObject("你的战团是什么样"), new TextObject("选择你的战团组成,这会影响你的初始兵力"), characters, GetFreeCossackNode3CharacterArgs);

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("cossack_band_mixed", new TextObject("混编游骑"), new TextObject("你的战团是混编游骑。初始兵力：自由骑手×8，流亡者×4，食物×7"), GetCossackBandMixedArgs, (m) => true, (m) => CossackBandMixedOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("cossack_band_outcast", new TextObject("流亡者居多（不稳定）"), new TextObject("你的战团中流亡者居多，不稳定。初始兵力：流亡者×12，食物×6，士气+2，标记：风险开局"), GetCossackBandOutcastArgs, (m) => true, (m) => CossackBandOutcastOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("cossack_band_elite", new TextObject("精干小队"), new TextObject("你的战团是精干小队。初始兵力：资深自由骑手×8，食物×6，声望+1"), GetCossackBandEliteArgs, (m) => true, (m) => CossackBandEliteOnSelect(m), null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetFreeCossackNode3CharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetCossackBandMixedArgs(NarrativeMenuOptionArgs args) { }
        private static void CossackBandMixedOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了草原自由民-Node3-混装自由骑手"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_cossack_band"] = "mixed"; }

        private static void GetCossackBandOutcastArgs(NarrativeMenuOptionArgs args) { }
        private static void CossackBandOutcastOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了草原自由民-Node3-流亡者杂兵"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_cossack_band"] = "outcast"; }

        private static void GetCossackBandEliteArgs(NarrativeMenuOptionArgs args) { }
        private static void CossackBandEliteOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择草原自由民-Node3-精悍小队"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_cossack_band"] = "elite"; }

        private static NarrativeMenu CreateFreeCossackNode4Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu("free_cossack_node4", "free_cossack_node3", "narrative_parent_menu", new TextObject("你对汗廷是什么态度"), new TextObject("选择你对汗廷的态度,这会影响你的初始关系和标记"), characters, GetFreeCossackNode4CharacterArgs);

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("cossack_khanate_legitimacy", new TextObject("承认汗廷，换生路"), new TextObject("你承认汗廷，换生路。与库塞特关系+2，标记：寻求可汗青睐"), GetCossackKhanateLegitimacyArgs, (m) => true, (m) => CossackKhanateLegitimacyOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("cossack_khanate_independent", new TextObject("保持独立"), new TextObject("你保持独立。侦察+2，魅力+2，标记：低调行事"), GetCossackKhanateIndependentArgs, (m) => true, (m) => CossackKhanateIndependentOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("cossack_khanate_oppose", new TextObject("必要时反抗"), new TextObject("你必要时反抗。狡诈+2，领导力+2，标记：被怀疑"), GetCossackKhanateOpposeArgs, (m) => true, (m) => CossackKhanateOpposeOnSelect(m), null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetFreeCossackNode4CharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetCossackKhanateLegitimacyArgs(NarrativeMenuOptionArgs args) { }
        private static void CossackKhanateLegitimacyOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了草原自由民-Node4-愿被吸纳"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_cossack_khanate"] = "legitimacy"; OriginSystemHelper.OriginSelectionDone = true; }

        private static void GetCossackKhanateIndependentArgs(NarrativeMenuOptionArgs args) { }
        private static void CossackKhanateIndependentOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了草原自由民-Node4-保持独立"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_cossack_khanate"] = "independent"; OriginSystemHelper.OriginSelectionDone = true; }

        private static void GetCossackKhanateOpposeArgs(NarrativeMenuOptionArgs args) { }
        private static void CossackKhanateOpposeOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了草原自由民-Node4-反感统治"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_cossack_khanate"] = "oppose"; OriginSystemHelper.OriginSelectionDone = true; }

        #endregion
    }
}
