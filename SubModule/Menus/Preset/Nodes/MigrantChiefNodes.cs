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
        #region 西迁部族首领节点菜单

        private static NarrativeMenu CreateMigrantChiefNode1Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu("migrant_chief_node1", "preset_origin_selection", "migrant_chief_node2", new TextObject("你为何西迁"), new TextObject("选择你西迁的原因,这会影响你的初始关系和生存压力"), characters, GetMigrantChiefNode1CharacterArgs);

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("migrant_reason_annexed", new TextObject("被吞并，带人逃命"), new TextObject("你被更强的部族吞并，带人逃命。声望+1，与库塞特关系-1，侦察+4，管理+3，标记：流离失所"), GetMigrantReasonAnnexedArgs, (m) => true, (m) => MigrantReasonAnnexedOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("migrant_reason_drought", new TextObject("旱灾迫迁"), new TextObject("草场枯竭，旱灾迫迁。食物×3，侦察+3，管理+4，标记：艰难时世"), GetMigrantReasonDroughtArgs, (m) => true, (m) => MigrantReasonDroughtOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("migrant_reason_tax", new TextObject("税役太重，举族出走"), new TextObject("汗廷征税过重，你举族出走。金钱+150，与精英氏族关系-2，贸易+3，魅力+2，标记：怨恨"), GetMigrantReasonTaxArgs, (m) => true, (m) => MigrantReasonTaxOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("migrant_reason_blood_feud", new TextObject("血仇不止，只能远走"), new TextObject("你与某个氏族有血仇，只能远走。战术+4，领导力+3，标记：血仇"), GetMigrantReasonBloodFeudArgs, (m) => true, (m) => MigrantReasonBloodFeudOnSelect(m), null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetMigrantChiefNode1CharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetMigrantReasonAnnexedArgs(NarrativeMenuOptionArgs args) { }
        private static void MigrantReasonAnnexedOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了西迁部族首领-Node1-被更强部族吞并"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_migrant_reason"] = "annexed"; }

        private static void GetMigrantReasonDroughtArgs(NarrativeMenuOptionArgs args) { }
        private static void MigrantReasonDroughtOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择西迁部族首领-Node1-草场枯竭"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_migrant_reason"] = "drought"; }

        private static void GetMigrantReasonTaxArgs(NarrativeMenuOptionArgs args) { }
        private static void MigrantReasonTaxOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了西迁部族首领-Node1-汗廷征税过重"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_migrant_reason"] = "tax"; }

        private static void GetMigrantReasonBloodFeudArgs(NarrativeMenuOptionArgs args) { }
        private static void MigrantReasonBloodFeudOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了西迁部族首领-Node1-血仇驱使"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_migrant_reason"] = "blood_feud"; }

        private static NarrativeMenu CreateMigrantChiefNode2Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu("migrant_chief_node2", "migrant_chief_node1", "migrant_chief_node3", new TextObject("你的族人组成"), new TextObject("选择你的族人组成,这会影响你的初始兵力和生存压力"), characters, GetMigrantChiefNode2CharacterArgs);

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("migrant_people_warriors", new TextObject("以青壮战士为主"), new TextObject("你的族人以青壮战士为主。初始兵力：游牧战士×14，骑射手×6，食物×6"), GetMigrantPeopleWarriorsArgs, (m) => true, (m) => MigrantPeopleWarriorsOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("migrant_people_families", new TextObject("家眷居多（负担但稳）"), new TextObject("你的族人中家眷居多，负担但更稳定。初始兵力：游牧战士×10，部落青年×4，食物×10，标记：有依赖者"), GetMigrantPeopleFamiliesArgs, (m) => true, (m) => MigrantPeopleFamiliesOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("migrant_people_craftsmen", new TextObject("带着匠人（更会经营）"), new TextObject("你的族人带着匠人，更会经营。初始兵力：游牧战士×10，部落青年×4，金钱+200，管理+4，贸易+2"), GetMigrantPeopleCraftsmenArgs, (m) => true, (m) => MigrantPeopleCraftsmenOnSelect(m), null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetMigrantChiefNode2CharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetMigrantPeopleWarriorsArgs(NarrativeMenuOptionArgs args) { }
        private static void MigrantPeopleWarriorsOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择西迁部族首领-Node2-战士"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_migrant_people"] = "warriors"; }

        private static void GetMigrantPeopleFamiliesArgs(NarrativeMenuOptionArgs args) { }
        private static void MigrantPeopleFamiliesOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择西迁部族首领-Node2-妇孺"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_migrant_people"] = "families"; }

        private static void GetMigrantPeopleCraftsmenArgs(NarrativeMenuOptionArgs args) { }
        private static void MigrantPeopleCraftsmenOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择西迁部族首领-Node2-工匠与牧人多"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_migrant_people"] = "craftsmen"; }

        private static NarrativeMenu CreateMigrantChiefNode3Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu("migrant_chief_node3", "migrant_chief_node2", "migrant_chief_node4", new TextObject("你带走了什么家当"), new TextObject("选择你带走的家当,这会影响你的初始资源"), characters, GetMigrantChiefNode3CharacterArgs);

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("migrant_assets_livestock", new TextObject("牲畜与皮货"), new TextObject("你带走了牲畜与皮货。金钱+250，食物×6，标记：贸易商品"), GetMigrantAssetsLivestockArgs, (m) => true, (m) => MigrantAssetsLivestockOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("migrant_assets_horses", new TextObject("更多的马"), new TextObject("你带走了更多的马。马匹×2，金钱+100，骑术+3"), GetMigrantAssetsHorsesArgs, (m) => true, (m) => MigrantAssetsHorsesOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("migrant_assets_relics", new TextObject("祖物与誓言"), new TextObject("你带走了祖物与誓言。声望+1，士气+4，标记：拥有象征物，魅力+2"), GetMigrantAssetsRelicsArgs, (m) => true, (m) => MigrantAssetsRelicsOnSelect(m), null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetMigrantChiefNode3CharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetMigrantAssetsLivestockArgs(NarrativeMenuOptionArgs args) { }
        private static void MigrantAssetsLivestockOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了西迁部族首领-Node3-牲畜"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_migrant_assets"] = "livestock"; }

        private static void GetMigrantAssetsHorsesArgs(NarrativeMenuOptionArgs args) { }
        private static void MigrantAssetsHorsesOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了西迁部族首领-Node3-马匹"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_migrant_assets"] = "horses"; }

        private static void GetMigrantAssetsRelicsArgs(NarrativeMenuOptionArgs args) { }
        private static void MigrantAssetsRelicsOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了西迁部族首领-Node3-族中遗物"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_migrant_assets"] = "relics"; }

        private static NarrativeMenu CreateMigrantChiefNode4Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu("migrant_chief_node4", "migrant_chief_node3", "narrative_parent_menu", new TextObject("你想定居还是继续游牧"), new TextObject("选择你的目标,这会影响你的开局节奏和关系"), characters, GetMigrantChiefNode4CharacterArgs);

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("migrant_goal_settle", new TextObject("找地定居"), new TextObject("你选择找地定居。管理+4，魅力+2，标记：寻求定居"), GetMigrantGoalSettleArgs, (m) => true, (m) => MigrantGoalSettleOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("migrant_goal_mercenary", new TextObject("卖力换生路"), new TextObject("你选择卖力换生路。领导力+3，战术+3，金钱+150，标记：雇佣兵心态"), GetMigrantGoalMercenaryArgs, (m) => true, (m) => MigrantGoalMercenaryOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("migrant_goal_revenge", new TextObject("迟早复仇"), new TextObject("你选择迟早复仇。战术+4，士气+3，声望+1，标记：复仇"), GetMigrantGoalRevengeArgs, (m) => true, (m) => MigrantGoalRevengeOnSelect(m), null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetMigrantChiefNode4CharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetMigrantGoalSettleArgs(NarrativeMenuOptionArgs args) { }
        private static void MigrantGoalSettleOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了西迁部族首领-Node4-找边境领主当附庸"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_migrant_goal"] = "settle"; OriginSystemHelper.OriginSelectionDone = true; }

        private static void GetMigrantGoalMercenaryArgs(NarrativeMenuOptionArgs args) { }
        private static void MigrantGoalMercenaryOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了西迁部族首领-Node4-当边境雇佣部族"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_migrant_goal"] = "mercenary"; OriginSystemHelper.OriginSelectionDone = true; }

        private static void GetMigrantGoalRevengeArgs(NarrativeMenuOptionArgs args) { }
        private static void MigrantGoalRevengeOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择西迁部族首领-Node4-回草原复仇"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_migrant_goal"] = "revenge"; OriginSystemHelper.OriginSelectionDone = true; }

        #endregion
    }
}
