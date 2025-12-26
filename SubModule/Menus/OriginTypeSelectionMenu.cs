using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace OriginSystemMod
{
    public static partial class OriginSystemPatches
    {
        #region Origin Type Selection Menu

        private static NarrativeMenu CreateOriginTypeSelectionMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();

            var menu = new NarrativeMenu(
                "origin_type_selection",
                "start",
                "preset_origin_selection",
                new TextObject("Choose Your Origin Type"),
                new TextObject("Preset origin (with story and faction) or non-preset origin (free assembly)."),
                characters,
                GetOriginTypeMenuCharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "preset_origin_option",
                new TextObject("Preset Origin"),
                new TextObject("Choose a preset origin background (with story and faction)."),
                GetPresetOriginArgs,
                PresetOriginOnCondition,
                PresetOriginOnSelect,
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "non_preset_origin_option",
                new TextObject("Non-Preset Origin"),
                new TextObject("Freely assemble your origin (high freedom)."),
                GetNonPresetOriginArgs,
                NonPresetOriginOnCondition,
                NonPresetOriginOnSelect,
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetOriginTypeMenuCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetPresetOriginArgs(NarrativeMenuOptionArgs args)
        {
        }

        private static bool PresetOriginOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        private static void PresetOriginOnSelect(CharacterCreationManager characterCreationManager)
        {
            try
            {
                OriginLog.ThrottledLog(
                    "[OriginSystem] User selected preset origin (mark only, no menu switch)",
                    Debug.DebugColor.Green
                );
                OriginSystemHelper.IsPresetOrigin = true;
                OriginSystemHelper.PendingMenuSwitch = "preset_origin_selection";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"PresetOriginOnSelect failed: {ex.Message}");
                OriginLog.Error($"StackTrace: {ex.StackTrace}");
            }
        }

        private static void GetNonPresetOriginArgs(NarrativeMenuOptionArgs args)
        {
        }

        private static bool NonPresetOriginOnCondition(CharacterCreationManager characterCreationManager)
        {
            return true;
        }

        private static void NonPresetOriginOnSelect(CharacterCreationManager characterCreationManager)
        {
            try
            {
                OriginLog.ThrottledLog(
                    "[OriginSystem] User selected non-preset origin (mark only, no menu switch)",
                    Debug.DebugColor.Green
                );
                OriginSystemHelper.IsPresetOrigin = false;
                OriginSystemHelper.PendingMenuSwitch = "non_preset_social_origin";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"NonPresetOriginOnSelect failed: {ex.Message}");
                OriginLog.Error($"StackTrace: {ex.StackTrace}");
            }
        }

        #endregion
    }
}
