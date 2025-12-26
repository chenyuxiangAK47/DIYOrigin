using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace OriginSystemMod
{
    public static partial class OriginSystemPatches
    {
        #region Khan's Mercenary Warrior Nodes

        private static NarrativeMenu CreateKhansMercenaryNode1Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();

            var menu = new NarrativeMenu(
                "khans_mercenary_node1",
                "preset_origin_selection",
                "khans_mercenary_node2",
                new TextObject("Why were you hired by the Khan"),
                new TextObject("Choose why you were hired by the Khan. This affects your initial reputation and relations."),
                characters,
                GetKhansMercenaryNode1CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "mercenary_reason_raider",
                new TextObject("You can fight and dare to raid"),
                new TextObject("You can fight and dare to raid. Cunning+4, Leadership+2, Relations with Khan+2, Tag: Mercenary Mindset"),
                GetMercenaryReasonRaiderArgs,
                (m) => true,
                (m) => MercenaryReasonRaiderOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "mercenary_reason_rescue",
                new TextObject("You saved someone from the Khan's court"),
                new TextObject("You saved someone from the Khan's court. Charm+2, Leadership+3, Relations with Khan+4, Tag: Seeking Khan's Favor"),
                GetMercenaryReasonRescueArgs,
                (m) => true,
                (m) => MercenaryReasonRescueOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "mercenary_reason_patron",
                new TextObject("You have a major clan as your patron"),
                new TextObject("You have a major clan as your patron. Relations with Elite Clan+4, Relations with Khan+1, Tag: Seeking Protection"),
                GetMercenaryReasonPatronArgs,
                (m) => true,
                (m) => MercenaryReasonPatronOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetKhansMercenaryNode1CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetMercenaryReasonRaiderArgs(NarrativeMenuOptionArgs args) { }
        private static void MercenaryReasonRaiderOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("User selected Khan's Mercenary Node1 - Raider Reputation");
            OriginSystemHelper.SelectedPresetOriginNodes["khz_node_mercenary_reason"] = "raider";
        }

        private static void GetMercenaryReasonRescueArgs(NarrativeMenuOptionArgs args) { }
        private static void MercenaryReasonRescueOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("User selected Khan's Mercenary Node1 - Saved Court Member");
            OriginSystemHelper.SelectedPresetOriginNodes["khz_node_mercenary_reason"] = "rescue";
        }

        private static void GetMercenaryReasonPatronArgs(NarrativeMenuOptionArgs args) { }
        private static void MercenaryReasonPatronOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("User selected Khan's Mercenary Node1 - Clan Patronage");
            OriginSystemHelper.SelectedPresetOriginNodes["khz_node_mercenary_reason"] = "patron";
        }

        private static NarrativeMenu CreateKhansMercenaryNode2Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();

            var menu = new NarrativeMenu(
                "khans_mercenary_node2",
                "khans_mercenary_node1",
                "khans_mercenary_node3",
                new TextObject("Your Warband Style"),
                new TextObject("Choose your warband style. This affects your initial troops."),
                characters,
                GetKhansMercenaryNode2CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "mercenary_style_horse_archer",
                new TextObject("Horse Archer Mobile"),
                new TextObject("Your warband is a horse archer mobile unit. Initial troops: Horse Archers x4, Nomad Warriors x4, Food x7"),
                GetMercenaryStyleHorseArcherArgs,
                (m) => true,
                (m) => MercenaryStyleHorseArcherOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "mercenary_style_javelin",
                new TextObject("Javelin Assault"),
                new TextObject("Your warband is a javelin assault unit. Initial troops: Light Cavalry x10, Horse Archers x6, Food x7"),
                GetMercenaryStyleJavelinArgs,
                (m) => true,
                (m) => MercenaryStyleJavelinOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "mercenary_style_mixed",
                new TextObject("Mixed Warband"),
                new TextObject("Your warband is a mixed unit. Initial troops: Horse Archers x6, Light Cavalry x6, Nomad Warriors x4, Food x8"),
                GetMercenaryStyleMixedArgs,
                (m) => true,
                (m) => MercenaryStyleMixedOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetKhansMercenaryNode2CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetMercenaryStyleHorseArcherArgs(NarrativeMenuOptionArgs args) { }
        private static void MercenaryStyleHorseArcherOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("User selected Khan's Mercenary Node2 - Horse Archer Main");
            OriginSystemHelper.SelectedPresetOriginNodes["khz_node_mercenary_style"] = "horse_archer";
        }

        private static void GetMercenaryStyleJavelinArgs(NarrativeMenuOptionArgs args) { }
        private static void MercenaryStyleJavelinOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("User selected Khan's Mercenary Node2 - Javelin Assault");
            OriginSystemHelper.SelectedPresetOriginNodes["khz_node_mercenary_style"] = "javelin";
        }

        private static void GetMercenaryStyleMixedArgs(NarrativeMenuOptionArgs args) { }
        private static void MercenaryStyleMixedOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("User selected Khan's Mercenary Node2 - Mixed Mercenary");
            OriginSystemHelper.SelectedPresetOriginNodes["khz_node_mercenary_style"] = "mixed";
        }

        private static NarrativeMenu CreateKhansMercenaryNode3Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();

            var menu = new NarrativeMenu(
                "khans_mercenary_node3",
                "khans_mercenary_node2",
                "khans_mercenary_node4",
                new TextObject("What Advance Did You Receive"),
                new TextObject("Choose what advance you received. This affects your initial resources, equipment and relations."),
                characters,
                GetKhansMercenaryNode3CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "mercenary_advance_loot",
                new TextObject("A Loot Advance"),
                new TextObject("You received a loot advance. Gold +450"),
                GetMercenaryAdvanceLootArgs,
                (m) => true,
                (m) => MercenaryAdvanceLootOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "mercenary_advance_gear",
                new TextObject("A Batch of Military Equipment"),
                new TextObject("You received a batch of military equipment. Horse +1, Gold +150"),
                GetMercenaryAdvanceGearArgs,
                (m) => true,
                (m) => MercenaryAdvanceGearOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "mercenary_advance_pass",
                new TextObject("A Pass and Supply Right"),
                new TextObject("You received a pass and supply right. Food +10, Charm +2, Tag: Has Pass Right"),
                GetMercenaryAdvancePassArgs,
                (m) => true,
                (m) => MercenaryAdvancePassOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetKhansMercenaryNode3CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetMercenaryAdvanceLootArgs(NarrativeMenuOptionArgs args) { }
        private static void MercenaryAdvanceLootOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("User selected Khan's Mercenary Node3 - Loot Advance");
            OriginSystemHelper.SelectedPresetOriginNodes["khz_node_mercenary_advance"] = "loot";
        }

        private static void GetMercenaryAdvanceGearArgs(NarrativeMenuOptionArgs args) { }
        private static void MercenaryAdvanceGearOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("User selected Khan's Mercenary Node3 - Military Equipment");
            OriginSystemHelper.SelectedPresetOriginNodes["khz_node_mercenary_advance"] = "gear";
        }

        private static void GetMercenaryAdvancePassArgs(NarrativeMenuOptionArgs args) { }
        private static void MercenaryAdvancePassOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("User selected Khan's Mercenary Node3 - Pass and Supply");
            OriginSystemHelper.SelectedPresetOriginNodes["khz_node_mercenary_advance"] = "pass";
        }

        private static NarrativeMenu CreateKhansMercenaryNode4Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();

            var menu = new NarrativeMenu(
                "khans_mercenary_node4",
                "khans_mercenary_node3",
                "narrative_parent_menu",
                new TextObject("Your Bottom Line"),
                new TextObject("Choose your bottom line. This affects your initial relations and tags."),
                characters,
                GetKhansMercenaryNode4CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "mercenary_bottomline_loyal",
                new TextObject("Take money but don't betray"),
                new TextObject("You choose to take money but don't betray. Charm +2, Relations with Khan +2, Tag: Loyal"),
                GetMercenaryBottomlineLoyalArgs,
                (m) => true,
                (m) => MercenaryBottomlineLoyalOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "mercenary_bottomline_profit",
                new TextObject("Profit First"),
                new TextObject("You choose profit first. Trade +2, Cunning +2, Gold +100, Tag: Profit Priority"),
                GetMercenaryBottomlineProfitArgs,
                (m) => true,
                (m) => MercenaryBottomlineProfitOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "mercenary_bottomline_ambition",
                new TextObject("Will be the master someday"),
                new TextObject("You choose to be the master someday. Leadership +3, Reputation +1, Tag: Ambition"),
                GetMercenaryBottomlineAmbitionArgs,
                (m) => true,
                (m) => MercenaryBottomlineAmbitionOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetKhansMercenaryNode4CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetMercenaryBottomlineLoyalArgs(NarrativeMenuOptionArgs args) { }
        private static void MercenaryBottomlineLoyalOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("User selected Khan's Mercenary Node4 - Loyal");
            OriginSystemHelper.SelectedPresetOriginNodes["khz_node_mercenary_bottomline"] = "loyal";
            OriginSystemHelper.OriginSelectionDone = true;
        }

        private static void GetMercenaryBottomlineProfitArgs(NarrativeMenuOptionArgs args) { }
        private static void MercenaryBottomlineProfitOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("User selected Khan's Mercenary Node4 - Profit");
            OriginSystemHelper.SelectedPresetOriginNodes["khz_node_mercenary_bottomline"] = "profit";
            OriginSystemHelper.OriginSelectionDone = true;
        }

        private static void GetMercenaryBottomlineAmbitionArgs(NarrativeMenuOptionArgs args) { }
        private static void MercenaryBottomlineAmbitionOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("User selected Khan's Mercenary Node4 - Ambition");
            OriginSystemHelper.SelectedPresetOriginNodes["khz_node_mercenary_bottomline"] = "ambition";
            OriginSystemHelper.OriginSelectionDone = true;
        }

        #endregion
    }
}