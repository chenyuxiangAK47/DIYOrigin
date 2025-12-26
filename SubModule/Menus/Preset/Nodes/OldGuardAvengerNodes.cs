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
        #region 东方旧部复仇者节点菜单
        private static NarrativeMenu CreateOldGuardAvengerNode1Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu("old_guard_avenger_node1", "preset_origin_selection", "old_guard_avenger_node2", new TextObject("你侍奉的旧主是谁"), new TextObject("选择你侍奉的旧主,这会影响你的初始声望和标记"), characters, GetOldGuardAvengerNode1CharacterArgs);

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("avenger_old_master_royal", new TextObject("亡国王族"), new TextObject("你侍奉亡国王族。声望+1，魅力+3，标记：拥有证明"), GetAvengerOldMasterRoyalArgs, (m) => true, (m) => AvengerOldMasterRoyalOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("avenger_old_master_clan", new TextObject("大氏族"), new TextObject("你侍奉大氏族。与精英氏族关系+2，标记：寻求庇护者"), GetAvengerOldMasterClanArgs, (m) => true, (m) => AvengerOldMasterClanOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("avenger_old_master_general", new TextObject("旧将军/督军"), new TextObject("你侍奉旧将军/督军。战术+4，领导力+3，标记：军事背景"), GetAvengerOldMasterGeneralArgs, (m) => true, (m) => AvengerOldMasterGeneralOnSelect(m), null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetOldGuardAvengerNode1CharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetAvengerOldMasterRoyalArgs(NarrativeMenuOptionArgs args) { }
        private static void AvengerOldMasterRoyalOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了东方旧部复仇-Node1-东方旧王"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_avenger_old_master"] = "royal"; }

        private static void GetAvengerOldMasterClanArgs(NarrativeMenuOptionArgs args) { }
        private static void AvengerOldMasterClanOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择东方旧部复仇-Node1-被灭的强部族"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_avenger_old_master"] = "clan"; }

        private static void GetAvengerOldMasterGeneralArgs(NarrativeMenuOptionArgs args) { }
        private static void AvengerOldMasterGeneralOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了东方旧部复仇-Node1-传奇将军"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_avenger_old_master"] = "general"; }

        private static NarrativeMenu CreateOldGuardAvengerNode2Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu("old_guard_avenger_node2", "old_guard_avenger_node1", "old_guard_avenger_node3", new TextObject("你的仇敌是谁"), new TextObject("选择你的仇敌,这会影响你的初始关系和通缉状态"), characters, GetOldGuardAvengerNode2CharacterArgs);

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("avenger_enemy_clan", new TextObject("某个氏族（不点名）"), new TextObject("你的仇敌是某个氏族，不点名。标记：有怨恨"), GetAvengerEnemyClanArgs, (m) => true, (m) => AvengerEnemyClanOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("avenger_enemy_khanate", new TextObject("汗廷体系（你恨的是制度）"), new TextObject("你的仇敌是汗廷体系，你恨的是制度。与库塞特关系-2，标记：有怨恨"), GetAvengerEnemyKhanateArgs, (m) => true, (m) => AvengerEnemyKhanateOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("avenger_enemy_neighbor", new TextObject("邻邦劫掠者"), new TextObject("你的仇敌是邻邦劫掠者。侦察+2，标记：有怨恨"), GetAvengerEnemyNeighborArgs, (m) => true, (m) => AvengerEnemyNeighborOnSelect(m), null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetOldGuardAvengerNode2CharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetAvengerEnemyClanArgs(NarrativeMenuOptionArgs args) { }
        private static void AvengerEnemyClanOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了东方旧部复仇-Node2-特定汗廷氏族"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_avenger_enemy"] = "clan"; }

        private static void GetAvengerEnemyKhanateArgs(NarrativeMenuOptionArgs args) { }
        private static void AvengerEnemyKhanateOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了东方旧部复仇-Node2-汗廷本身"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_avenger_enemy"] = "khanate"; }

        private static void GetAvengerEnemyNeighborArgs(NarrativeMenuOptionArgs args) { }
        private static void AvengerEnemyNeighborOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了东方旧部复仇-Node2-外来侵略者"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_avenger_enemy"] = "neighbor"; }

        private static NarrativeMenu CreateOldGuardAvengerNode3Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu("old_guard_avenger_node3", "old_guard_avenger_node2", "old_guard_avenger_node4", new TextObject("你带来的旧部是什么样"), new TextObject("选择你带来的旧部,这会影响你的初始兵力"), characters, GetOldGuardAvengerNode3CharacterArgs);

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("avenger_veterans_archer", new TextObject("资深骑射手"), new TextObject("你的旧部是资深骑射手。初始兵力：资深骑射手×12，食物×7"), GetAvengerVeteransArcherArgs, (m) => true, (m) => AvengerVeteransArcherOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("avenger_veterans_infantry", new TextObject("老步兵与护卫"), new TextObject("你的旧部是老步兵与护卫。初始兵力：草原步兵老兵×12，食物×7"), GetAvengerVeteransInfantryArgs, (m) => true, (m) => AvengerVeteransInfantryOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("avenger_veterans_mixed", new TextObject("混编老兵"), new TextObject("你的旧部是混编老兵。初始兵力：骑射手×8，草原步兵老兵×6，游牧战士×4，食物×8"), GetAvengerVeteransMixedArgs, (m) => true, (m) => AvengerVeteransMixedOnSelect(m), null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetOldGuardAvengerNode3CharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetAvengerVeteransArcherArgs(NarrativeMenuOptionArgs args) { }
        private static void AvengerVeteransArcherOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择东方旧部复仇-Node3-资深骑射手"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_avenger_veterans"] = "archer"; }

        private static void GetAvengerVeteransInfantryArgs(NarrativeMenuOptionArgs args) { }
        private static void AvengerVeteransInfantryOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择东方旧部复仇-Node3-老兵步兵"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_avenger_veterans"] = "infantry"; }

        private static void GetAvengerVeteransMixedArgs(NarrativeMenuOptionArgs args) { }
        private static void AvengerVeteransMixedOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了东方旧部复仇-Node3-混编残部"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_avenger_veterans"] = "mixed"; }

        private static NarrativeMenu CreateOldGuardAvengerNode4Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu("old_guard_avenger_node4", "old_guard_avenger_node3", "narrative_parent_menu", new TextObject("你如何开局复仇"), new TextObject("选择你的开局复仇策略,这会影响你的初始名声和资源"), characters, GetOldGuardAvengerNode4CharacterArgs);

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("avenger_opening_stealth", new TextObject("暗中积蓄"), new TextObject("你选择暗中积蓄。侦察+4，狡诈+2，标记：低调行事"), GetAvengerOpeningStealthArgs, (m) => true, (m) => AvengerOpeningStealthOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("avenger_opening_public", new TextObject("公开宣誓复仇"), new TextObject("你选择公开宣誓复仇。声望+1，士气+3，标记：拥有象征物"), GetAvengerOpeningPublicArgs, (m) => true, (m) => AvengerOpeningPublicOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("avenger_opening_ally", new TextObject("先找盟友"), new TextObject("你选择先找盟友。魅力+3，与精英氏族关系+2，标记：寻求庇护者"), GetAvengerOpeningAllyArgs, (m) => true, (m) => AvengerOpeningAllyOnSelect(m), null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetOldGuardAvengerNode4CharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetAvengerOpeningStealthArgs(NarrativeMenuOptionArgs args) { }
        private static void AvengerOpeningStealthOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了东方旧部复仇-Node4-暗中积蓄"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_avenger_opening"] = "stealth"; OriginSystemHelper.OriginSelectionDone = true; }

        private static void GetAvengerOpeningPublicArgs(NarrativeMenuOptionArgs args) { }
        private static void AvengerOpeningPublicOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了东方旧部复仇-Node4-公开宣言"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_avenger_opening"] = "public"; OriginSystemHelper.OriginSelectionDone = true; }

        private static void GetAvengerOpeningAllyArgs(NarrativeMenuOptionArgs args) { }
        private static void AvengerOpeningAllyOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了东方旧部复仇-Node4-先找外援"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_avenger_opening"] = "ally"; OriginSystemHelper.OriginSelectionDone = true; }

        #endregion
    }
}
