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
        #region 汗廷军叛逃者节点菜单
        private static NarrativeMenu CreateArmyDeserterNode1Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu("army_deserter_node1", "preset_origin_selection", "army_deserter_node2", new TextObject("你为何叛逃"), new TextObject("选择你从汗廷军叛逃的原因,这会影响你的初始关系和名声"), characters, GetArmyDeserterNode1CharacterArgs);

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("deserter_reason_scapegoat", new TextObject("被当替罪羊"), new TextObject("你被当作替罪羊，蒙受冤案。与库塞特关系-4，战术+4，侦察+2，标记：被怀疑"), GetDeserterReasonScapegoatArgs, (m) => true, (m) => DeserterReasonScapegoatOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("deserter_reason_unpaid", new TextObject("军饷被克扣"), new TextObject("你的军饷被克扣，愤怒离开。金钱+200，与库塞特关系-3，领导力+3，狡诈+2，标记：怨恨"), GetDeserterReasonUnpaidArgs, (m) => true, (m) => DeserterReasonUnpaidOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("deserter_reason_massacre", new TextObject("不愿执行屠村命令"), new TextObject("你拒绝执行屠村命令，道德冲突。声望+1，与库塞特关系-2，领导力+3，魅力+2，标记：道德立场"), GetDeserterReasonMassacreArgs, (m) => true, (m) => DeserterReasonMassacreOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("deserter_reason_murder", new TextObject("军中斗殴闹出人命"), new TextObject("你在军中斗殴闹出人命，被迫逃亡。金钱+150，与库塞特关系-5，狡诈+4，体力+2，标记：被怀疑"), GetDeserterReasonMurderArgs, (m) => true, (m) => DeserterReasonMurderOnSelect(m), null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetArmyDeserterNode1CharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetDeserterReasonScapegoatArgs(NarrativeMenuOptionArgs args) { }
        private static void DeserterReasonScapegoatOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择汗廷军叛逃Node1-被当替罪羊"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_deserter_reason"] = "scapegoat"; }

        private static void GetDeserterReasonUnpaidArgs(NarrativeMenuOptionArgs args) { }
        private static void DeserterReasonUnpaidOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择汗廷军叛逃Node1-军饷被克扣"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_deserter_reason"] = "unpaid"; }

        private static void GetDeserterReasonMassacreArgs(NarrativeMenuOptionArgs args) { }
        private static void DeserterReasonMassacreOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择汗廷军叛逃Node1-屠杀命令"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_deserter_reason"] = "massacre"; }

        private static void GetDeserterReasonMurderArgs(NarrativeMenuOptionArgs args) { }
        private static void DeserterReasonMurderOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了汗廷军叛逃Node1-私斗杀人"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_deserter_reason"] = "murder"; }

        private static NarrativeMenu CreateArmyDeserterNode2Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu("army_deserter_node2", "army_deserter_node1", "army_deserter_node3", new TextObject("你带走了哪些军中人员"), new TextObject("选择你带走的军中人员,这会影响你的初始兵力"), characters, GetArmyDeserterNode2CharacterArgs);

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("deserter_followers_veteran_archer", new TextObject("老练骑射手为主"), new TextObject("你带走了老练骑射手为主。初始兵力：骑射手×12，食物×6"), GetDeserterFollowersVeteranArcherArgs, (m) => true, (m) => DeserterFollowersVeteranArcherOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("deserter_followers_javelin", new TextObject("标枪轻骑为主"), new TextObject("你带走了标枪轻骑为主。初始兵力：轻骑兵×10，游牧战士×4，食物×6"), GetDeserterFollowersJavelinArgs, (m) => true, (m) => DeserterFollowersJavelinOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("deserter_followers_mixed", new TextObject("混编旧部"), new TextObject("你带走了混编旧部。初始兵力：骑射手×8，游牧战士×4，食物×7"), GetDeserterFollowersMixedArgs, (m) => true, (m) => DeserterFollowersMixedOnSelect(m), null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetArmyDeserterNode2CharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetDeserterFollowersVeteranArcherArgs(NarrativeMenuOptionArgs args) { }
        private static void DeserterFollowersVeteranArcherOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择汗廷军叛逃Node2-骑射老兵"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_deserter_followers"] = "veteran_archer"; }

        private static void GetDeserterFollowersJavelinArgs(NarrativeMenuOptionArgs args) { }
        private static void DeserterFollowersJavelinOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了汗廷军叛逃Node2-标枪轻骑"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_deserter_followers"] = "javelin"; }

        private static void GetDeserterFollowersMixedArgs(NarrativeMenuOptionArgs args) { }
        private static void DeserterFollowersMixedOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择汗廷军叛逃Node2-混编"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_deserter_followers"] = "mixed"; }

        private static NarrativeMenu CreateArmyDeserterNode3Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu("army_deserter_node3", "army_deserter_node2", "army_deserter_node4", new TextObject("你带走了什么军需"), new TextObject("选择你带走的军需,这会影响你的初始装备,金币和名声"), characters, GetArmyDeserterNode3CharacterArgs);

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("deserter_supplies_gear", new TextObject("带走一批甲械"), new TextObject("你带走了甲械。金钱+100"), GetDeserterSuppliesGearArgs, (m) => true, (m) => DeserterSuppliesGearOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("deserter_supplies_pay", new TextObject("带走一袋军饷"), new TextObject("你带走了军饷。金钱+350"), GetDeserterSuppliesPayArgs, (m) => true, (m) => DeserterSuppliesPayOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("deserter_supplies_docs", new TextObject("带走军令/名册"), new TextObject("你带走了军令/名册。魅力+2，战术+2，标记：拥有证明"), GetDeserterSuppliesDocsArgs, (m) => true, (m) => DeserterSuppliesDocsOnSelect(m), null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetArmyDeserterNode3CharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetDeserterSuppliesGearArgs(NarrativeMenuOptionArgs args) { }
        private static void DeserterSuppliesGearOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了汗廷军叛逃Node3-军械"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_deserter_supplies"] = "gear"; }

        private static void GetDeserterSuppliesPayArgs(NarrativeMenuOptionArgs args) { }
        private static void DeserterSuppliesPayOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了汗廷军叛逃Node3-军饷"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_deserter_supplies"] = "pay"; }

        private static void GetDeserterSuppliesDocsArgs(NarrativeMenuOptionArgs args) { }
        private static void DeserterSuppliesDocsOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了汗廷军叛逃Node3-军令/名册"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_deserter_supplies"] = "docs"; }

        private static NarrativeMenu CreateArmyDeserterNode4Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu("army_deserter_node4", "army_deserter_node3", "narrative_parent_menu", new TextObject("你打算洗白还是彻底当军阀"), new TextObject("选择你的未来道路,这会影响你的开局策略和关系"), characters, GetArmyDeserterNode4CharacterArgs);

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("deserter_path_redemption", new TextObject("找机会洗白"), new TextObject("你选择找机会洗白。魅力+3，与可汗关系+1，标记：寻求救赎"), GetDeserterPathRedemptionArgs, (m) => true, (m) => DeserterPathRedemptionOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("deserter_path_warlord", new TextObject("彻底当军阀"), new TextObject("你选择彻底当军阀。狡诈+3，领导力+3，声望+1，标记：军阀之路"), GetDeserterPathWarlordArgs, (m) => true, (m) => DeserterPathWarlordOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("deserter_path_exile", new TextObject("离开草原"), new TextObject("你选择离开草原。侦察+3，贸易+2，标记：在路上"), GetDeserterPathExileArgs, (m) => true, (m) => DeserterPathExileOnSelect(m), null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetArmyDeserterNode4CharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetDeserterPathRedemptionArgs(NarrativeMenuOptionArgs args) { }
        private static void DeserterPathRedemptionOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了汗廷军叛逃Node4-洗白"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_deserter_path"] = "redemption"; OriginSystemHelper.OriginSelectionDone = true; }

        private static void GetDeserterPathWarlordArgs(NarrativeMenuOptionArgs args) { }
        private static void DeserterPathWarlordOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了汗廷军叛逃Node4-军阀"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_deserter_path"] = "warlord"; OriginSystemHelper.OriginSelectionDone = true; }

        private static void GetDeserterPathExileArgs(NarrativeMenuOptionArgs args) { }
        private static void DeserterPathExileOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了汗廷军叛逃Node4-流亡"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_deserter_path"] = "exile"; OriginSystemHelper.OriginSelectionDone = true; }

        #endregion
    }
}
