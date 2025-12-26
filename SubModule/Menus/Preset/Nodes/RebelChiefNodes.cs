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
        #region Steppe Rebel Chief Node Menus

        private static NarrativeMenu CreateRebelChiefNode1Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu("rebel_chief_node1", "preset_origin_selection", "rebel_chief_node2", new TextObject("Why Were You Declared a Rebel Chief"), new TextObject("Choose the reason you were declared a rebel chief, this will affect your initial relations and wanted status"), characters, GetRebelChiefNode1CharacterArgs);

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("rebel_reason_refuse_tribute", new TextObject("Refused Tribute"), new TextObject("You refused to pay tribute to the Khanate. Renown+1, Khuzait relations-5, Mark: Suspected"), GetRebelReasonRefuseTributeArgs, (m) => true, (m) => RebelReasonRefuseTributeOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("rebel_reason_raid_caravan", new TextObject("Raided Caravan and Named"), new TextObject("You raided a caravan and were named. Money+250, Renown+1, Merchant relations-10, Khuzait relations-6, Mark: Suspected"), GetRebelReasonRaidCaravanArgs, (m) => true, (m) => RebelReasonRaidCaravanOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("rebel_reason_refuse_service", new TextObject("Refused Military Service/Refused Campaign"), new TextObject("You refused military service or refused to campaign. Khuzait relations-4, Mark: Suspected"), GetRebelReasonRefuseServiceArgs, (m) => true, (m) => RebelReasonRefuseServiceOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("rebel_reason_protect_tribe", new TextObject("Protected Tribe by Defying Khanate"), new TextObject("You protected your tribe by defying the Khanate. Renown+1, Khuzait relations-3, Morale+5"), GetRebelReasonProtectTribeArgs, (m) => true, (m) => RebelReasonProtectTribeOnSelect(m), null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetRebelChiefNode1CharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetRebelReasonRefuseTributeArgs(NarrativeMenuOptionArgs args) { }
        private static void RebelReasonRefuseTributeOnSelect(CharacterCreationManager manager) { OriginLog.Info("User selected Steppe Rebel Chief-Node1-Refused Tribute"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_rebel_reason"] = "refuse_tribute"; }

        private static void GetRebelReasonRaidCaravanArgs(NarrativeMenuOptionArgs args) { }
        private static void RebelReasonRaidCaravanOnSelect(CharacterCreationManager manager) { OriginLog.Info("User selected Steppe Rebel Chief-Node1-Raided Caravan"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_rebel_reason"] = "raid_caravan"; }

        private static void GetRebelReasonRefuseServiceArgs(NarrativeMenuOptionArgs args) { }
        private static void RebelReasonRefuseServiceOnSelect(CharacterCreationManager manager) { OriginLog.Info("User selected Steppe Rebel Chief-Node1-Refused Service"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_rebel_reason"] = "refuse_service"; }

        private static void GetRebelReasonProtectTribeArgs(NarrativeMenuOptionArgs args) { }
        private static void RebelReasonProtectTribeOnSelect(CharacterCreationManager manager) { OriginLog.Info("User selected Steppe Rebel Chief-Node1-Protected Tribe"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_rebel_reason"] = "protect_tribe"; }

        private static NarrativeMenu CreateRebelChiefNode2Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu("rebel_chief_node2", "rebel_chief_node1", "rebel_chief_node3", new TextObject("Who Makes Up Your Rebel Warband"), new TextObject("Choose your rebel warband composition, this will affect your initial troops"), characters, GetRebelChiefNode2CharacterArgs);

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("rebel_warband_nomads", new TextObject("Nomad Warriors Mainly"), new TextObject("Your warband is mainly nomad warriors. Initial troops: Nomad Warriors x16, Tribal Youths x4, Food x8"), GetRebelWarbandNomadsArgs, (m) => true, (m) => RebelWarbandNomadsOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("rebel_warband_light_cav", new TextObject("Light Cavalry/Horse Archers Mainly"), new TextObject("Your warband is mainly light cavalry/horse archers. Initial troops: Horse Archers x10, Light Cavalry x6, Horses x1, Food x6"), GetRebelWarbandLightCavArgs, (m) => true, (m) => RebelWarbandLightCavOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("rebel_warband_mixed", new TextObject("Mixed (More Stable)"), new TextObject("Your warband is mixed, more stable. Initial troops: Nomad Warriors x10, Horse Archers x6, Light Cavalry x4, Food x7"), GetRebelWarbandMixedArgs, (m) => true, (m) => RebelWarbandMixedOnSelect(m), null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetRebelChiefNode2CharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetRebelWarbandNomadsArgs(NarrativeMenuOptionArgs args) { }
        private static void RebelWarbandNomadsOnSelect(CharacterCreationManager manager) { OriginLog.Info("User selected Steppe Rebel Chief-Node2-Nomad Warriors"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_rebel_warband_makeup"] = "nomads"; }

        private static void GetRebelWarbandLightCavArgs(NarrativeMenuOptionArgs args) { }
        private static void RebelWarbandLightCavOnSelect(CharacterCreationManager manager) { OriginLog.Info("User selected Steppe Rebel Chief-Node2-Light Cavalry"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_rebel_warband_makeup"] = "light_cav"; }

        private static void GetRebelWarbandMixedArgs(NarrativeMenuOptionArgs args) { }
        private static void RebelWarbandMixedOnSelect(CharacterCreationManager manager) { OriginLog.Info("User selected Steppe Rebel Chief-Node2-Mixed"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_rebel_warband_makeup"] = "mixed"; }

        private static NarrativeMenu CreateRebelChiefNode3Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu("rebel_chief_node3", "rebel_chief_node2", "rebel_chief_node4", new TextObject("What \"Rebel Symbol\" Did You Take"), new TextObject("Choose the rebel symbol you took, this will affect your initial renown and marks"), characters, GetRebelChiefNode3CharacterArgs);

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("rebel_symbol_banner", new TextObject("An Old War Banner"), new TextObject("You took an old war banner. Renown+1, Morale+4, Mark: Has Symbol"), GetRebelSymbolBannerArgs, (m) => true, (m) => RebelSymbolBannerOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("rebel_symbol_treasure", new TextObject("Urgently Needed Gold, Silver and Horse Gear"), new TextObject("You took urgently needed gold, silver and horse gear. Money+400, Horses x1, Mark: Short-term Wealthy"), GetRebelSymbolTreasureArgs, (m) => true, (m) => RebelSymbolTreasureOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("rebel_symbol_seal", new TextObject("An Identity-Proving Seal"), new TextObject("You took an identity-proving seal. Renown+1, Charm+3, Mark: Has Proof"), GetRebelSymbolSealArgs, (m) => true, (m) => RebelSymbolSealOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("rebel_symbol_people", new TextObject("Took a Small Group of Loyal Tribesmen"), new TextObject("You took loyal tribesmen. Initial troops: Nomad Warriors x6, Morale+3, Mark: Has Followers"), GetRebelSymbolPeopleArgs, (m) => true, (m) => RebelSymbolPeopleOnSelect(m), null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetRebelChiefNode3CharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetRebelSymbolBannerArgs(NarrativeMenuOptionArgs args) { }
        private static void RebelSymbolBannerOnSelect(CharacterCreationManager manager) { OriginLog.Info("User selected Steppe Rebel Chief-Node3-Banner"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_rebel_symbol"] = "banner"; }

        private static void GetRebelSymbolTreasureArgs(NarrativeMenuOptionArgs args) { }
        private static void RebelSymbolTreasureOnSelect(CharacterCreationManager manager) { OriginLog.Info("User selected Steppe Rebel Chief-Node3-Treasure"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_rebel_symbol"] = "treasure"; }

        private static void GetRebelSymbolSealArgs(NarrativeMenuOptionArgs args) { }
        private static void RebelSymbolSealOnSelect(CharacterCreationManager manager) { OriginLog.Info("User selected Steppe Rebel Chief-Node3-Tribe Seal"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_rebel_symbol"] = "seal"; }

        private static void GetRebelSymbolPeopleArgs(NarrativeMenuOptionArgs args) { }
        private static void RebelSymbolPeopleOnSelect(CharacterCreationManager manager) { OriginLog.Info("User selected Steppe Rebel Chief-Node3-People"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_rebel_symbol"] = "people"; }

        private static NarrativeMenu CreateRebelChiefNode4Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu("rebel_chief_node4", "rebel_chief_node3", "narrative_parent_menu", new TextObject("What Will You Do First"), new TextObject("Choose your first action, this will affect your initial relations and resources"), characters, GetRebelChiefNode4CharacterArgs);

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("rebel_first_move_hide", new TextObject("Hide in Steppe, Survive First"), new TextObject("You choose to hide in the steppe, survive first. Food x6, Scouting skill+6, Mark: Low Profile"), GetRebelFirstMoveHideArgs, (m) => true, (m) => RebelFirstMoveHideOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("rebel_first_move_raid", new TextObject("War Feeds War, Strike First"), new TextObject("You choose war feeds war, strike first. Money+200, Renown+1, Mark: Risky Start"), GetRebelFirstMoveRaidArgs, (m) => true, (m) => RebelFirstMoveRaidOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("rebel_first_move_ally", new TextObject("Find Allies, Catch Your Breath"), new TextObject("You choose to find allies, catch your breath. Charm+4, Elite Clan relations+2, Mark: Seeking Shelter"), GetRebelFirstMoveAllyArgs, (m) => true, (m) => RebelFirstMoveAllyOnSelect(m), null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetRebelChiefNode4CharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetRebelFirstMoveHideArgs(NarrativeMenuOptionArgs args) { }
        private static void RebelFirstMoveHideOnSelect(CharacterCreationManager manager) { OriginLog.Info("User selected Steppe Rebel Chief-Node4-Hide"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_rebel_first_move"] = "hide"; OriginSystemHelper.OriginSelectionDone = true; }

        private static void GetRebelFirstMoveRaidArgs(NarrativeMenuOptionArgs args) { }
        private static void RebelFirstMoveRaidOnSelect(CharacterCreationManager manager) { OriginLog.Info("User selected Steppe Rebel Chief-Node4-Continue Raiding"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_rebel_first_move"] = "raid"; OriginSystemHelper.OriginSelectionDone = true; }

        private static void GetRebelFirstMoveAllyArgs(NarrativeMenuOptionArgs args) { }
        private static void RebelFirstMoveAllyOnSelect(CharacterCreationManager manager) { OriginLog.Info("User selected Steppe Rebel Chief-Node4-Find External Support"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_rebel_first_move"] = "ally"; OriginSystemHelper.OriginSelectionDone = true; }

        #endregion
    }
}
