using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using OriginSystemMod;

namespace OriginSystemMod
{
    public static partial class OriginSystemPatches
    {
        private static NarrativeMenu CreatePresetOriginSelectionMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "preset_origin_selection",
                "origin_type_selection",
                "narrative_parent_menu",
                new TextObject("Choose Preset Origin"),
                new TextObject("Choose a preset origin background"),
                characters,
                GetPresetOriginMenuCharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "khuzait_rebel_chief",
                new TextObject("Steppe Rebel Chief"),
                new TextObject("Illegal tribe chief, hostile to Khuzait Khanate. Initial troops: Nomad Warriors x20, Light Cavalry x8"),
                GetRebelChiefArgs,
                RebelChiefOnCondition,
                RebelChiefOnSelect,
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "khuzait_minor_noble",
                new TextObject("Minor Khanate Noble"),
                new TextObject("Minor noble of the Khanate, political inclination, has some noble followers, stronger social charm route"),
                GetMinorNobleArgs,
                MinorNobleOnCondition,
                MinorNobleOnSelect,
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "khuzait_migrant_chief",
                new TextObject("Westward Migrant Chief"),
                new TextObject("Westward migrating tribe chief, survival/exile inclination, low resources, high population pressure"),
                GetMigrantChiefArgs,
                MigrantChiefOnCondition,
                MigrantChiefOnSelect,
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "khuzait_army_deserter",
                new TextObject("Khanate Army Deserter"),
                new TextObject("Soldier who deserted from Khanate army, military inclination, high proficiency in mounted javelin, obvious tactical tendency"),
                GetArmyDeserterArgs,
                ArmyDeserterOnCondition,
                ArmyDeserterOnSelect,
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "khuzait_trade_protector",
                new TextObject("Steppe Trade Route Protector"),
                new TextObject("Guardian protecting steppe trade routes, economic/neutral inclination, trade and social tendency, large cross-faction space"),
                GetTradeProtectorArgs,
                TradeProtectorOnCondition,
                TradeProtectorOnSelect,
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "khuzait_wandering_prince",
                new TextObject("Wandering Prince"),
                new TextObject("Exiled Khuzait prince, clan level. Initial clan members: 3-4 people"),
                GetWanderingPrinceArgs,
                WanderingPrinceOnCondition,
                WanderingPrinceOnSelect,
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "khuzait_khans_mercenary",
                new TextObject("Khan's Mercenary Warrior"),
                new TextObject("War band hired by Khan, fast-paced combat, strong troops, fast combat rhythm, higher Khanate relations"),
                GetKhansMercenaryArgs,
                KhansMercenaryOnCondition,
                KhansMercenaryOnSelect,
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "khuzait_slave_escape",
                new TextObject("War Slave Escape"),
                new TextObject("Former war slave who escaped from Aserai, being hunted, initial troops: Former War Slave Warriors x18"),
                GetSlaveEscapeArgs,
                SlaveEscapeOnCondition,
                SlaveEscapeOnSelect,
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "khuzait_free_cossack",
                new TextObject("Steppe Free People"),
                new TextObject("Free people on the steppe, Cossack prototype, high freedom, mixed units, strong gray survival ability"),
                GetFreeCossackArgs,
                FreeCossackOnCondition,
                FreeCossackOnSelect,
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "khuzait_old_guard_avenger",
                new TextObject("Eastern Old Guard Avenger"),
                new TextObject("Veteran seeking revenge for old master, rich combat experience, many veterans, high combat experience, clear enemies"),
                GetOldGuardAvengerArgs,
                OldGuardAvengerOnCondition,
                OldGuardAvengerOnSelect,
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetPresetOriginMenuCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetRebelChiefArgs(NarrativeMenuOptionArgs args)
        {
        }

        private static bool RebelChiefOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        private static void RebelChiefOnSelect(CharacterCreationManager characterCreationManager)
        {
            try
            {
                OriginLog.Info("User selected Steppe Rebel Chief");
                OriginSystemHelper.SelectedPresetOriginId = "khuzait_rebel_chief";
                OriginSystemHelper.IsPresetOrigin = true;
            }
            catch (Exception ex)
            {
                OriginLog.Error($"RebelChiefOnSelect failed: {ex.Message}");
            }
        }

        private static void GetWanderingPrinceArgs(NarrativeMenuOptionArgs args)
        {
        }

        private static bool WanderingPrinceOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        private static void WanderingPrinceOnSelect(CharacterCreationManager characterCreationManager)
        {
            OriginLog.Info("User selected Wandering Prince");
            OriginSystemHelper.SelectedPresetOriginId = "khuzait_wandering_prince";
            OriginSystemHelper.IsPresetOrigin = true;
        }

        private static void GetSlaveEscapeArgs(NarrativeMenuOptionArgs args)
        {
        }

        private static bool SlaveEscapeOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        private static void SlaveEscapeOnSelect(CharacterCreationManager characterCreationManager)
        {
            try
            {
                OriginLog.Info("User selected War Slave Escape");
                OriginSystemHelper.SelectedPresetOriginId = "khuzait_slave_escape";
                OriginSystemHelper.IsPresetOrigin = true;
            }
            catch (Exception ex)
            {
                OriginLog.Error($"SlaveEscapeOnSelect failed: {ex.Message}");
            }
        }

        private static void GetMinorNobleArgs(NarrativeMenuOptionArgs args)
        {
        }

        private static bool MinorNobleOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        private static void MinorNobleOnSelect(CharacterCreationManager characterCreationManager)
        {
            OriginLog.Info("User selected Minor Khanate Noble");
            OriginSystemHelper.SelectedPresetOriginId = "khuzait_minor_noble";
            OriginSystemHelper.IsPresetOrigin = true;
        }

        private static void GetMigrantChiefArgs(NarrativeMenuOptionArgs args)
        {
        }

        private static bool MigrantChiefOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        private static void MigrantChiefOnSelect(CharacterCreationManager characterCreationManager)
        {
            OriginLog.Info("User selected Westward Migrant Chief");
            OriginSystemHelper.SelectedPresetOriginId = "khuzait_migrant_chief";
            OriginSystemHelper.IsPresetOrigin = true;
        }

        private static void GetArmyDeserterArgs(NarrativeMenuOptionArgs args)
        {
        }

        private static bool ArmyDeserterOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        private static void ArmyDeserterOnSelect(CharacterCreationManager characterCreationManager)
        {
            OriginLog.Info("User selected Khanate Army Deserter");
            OriginSystemHelper.SelectedPresetOriginId = "khuzait_army_deserter";
            OriginSystemHelper.IsPresetOrigin = true;
        }

        private static void GetTradeProtectorArgs(NarrativeMenuOptionArgs args)
        {
        }

        private static bool TradeProtectorOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        private static void TradeProtectorOnSelect(CharacterCreationManager characterCreationManager)
        {
            OriginLog.Info("User selected Steppe Trade Route Protector");
            OriginSystemHelper.SelectedPresetOriginId = "khuzait_trade_protector";
            OriginSystemHelper.IsPresetOrigin = true;
        }

        private static void GetKhansMercenaryArgs(NarrativeMenuOptionArgs args)
        {
        }

        private static bool KhansMercenaryOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        private static void KhansMercenaryOnSelect(CharacterCreationManager characterCreationManager)
        {
            OriginLog.Info("User selected Khan's Mercenary Warrior");
            OriginSystemHelper.SelectedPresetOriginId = "khuzait_khans_mercenary";
            OriginSystemHelper.IsPresetOrigin = true;
        }

        private static void GetFreeCossackArgs(NarrativeMenuOptionArgs args)
        {
        }

        private static bool FreeCossackOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        private static void FreeCossackOnSelect(CharacterCreationManager characterCreationManager)
        {
            OriginLog.Info("User selected Steppe Free People");
            OriginSystemHelper.SelectedPresetOriginId = "khuzait_free_cossack";
            OriginSystemHelper.IsPresetOrigin = true;
        }

        private static void GetOldGuardAvengerArgs(NarrativeMenuOptionArgs args)
        {
        }

        private static bool OldGuardAvengerOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        private static void OldGuardAvengerOnSelect(CharacterCreationManager characterCreationManager)
        {
            OriginLog.Info("User selected Eastern Old Guard Avenger");
            OriginSystemHelper.SelectedPresetOriginId = "khuzait_old_guard_avenger";
            OriginSystemHelper.IsPresetOrigin = true;
        }
    }
}
