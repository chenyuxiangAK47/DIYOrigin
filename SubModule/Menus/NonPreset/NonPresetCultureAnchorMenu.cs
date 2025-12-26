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
        private static NarrativeMenu CreateNonPresetCultureAnchorMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "non_preset_culture_anchor",
                "origin_type_selection",
                "non_preset_social_origin",
                new TextObject("选择文化锚点"),
                new TextObject("选择你的文化背景(不影响外交关系)"),
                characters,
                GetNonPresetCultureAnchorCharacterArgs
            );

            var cultures = new[] { "empire", "sturgia", "battania", "khuzait", "aserai", "vlandia" };
            foreach (var cultureId in cultures)
            {
                menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                    $"culture_anchor_{cultureId}",
                    new TextObject(GetCultureName(cultureId)),
                    new TextObject($"选择 {GetCultureName(cultureId)} 文化背景"),
                    GetCultureAnchorArgs,
                    (m) => true,
                    (m) => NonPresetCultureAnchorOnSelect(m, cultureId),
                    null
                ));
            }

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetNonPresetCultureAnchorCharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetCultureAnchorArgs(NarrativeMenuOptionArgs args)
        {
            // 文化锚点不影响技能属性
        }

        private static void NonPresetCultureAnchorOnSelect(CharacterCreationManager manager, string cultureId)
        {
            try
            {
                OriginLog.Info($"用户选择了文化锚点 {cultureId}");
                OriginSystemHelper.NonPresetOrigin.CultureAnchor = cultureId;
            }
            catch (Exception ex)
            {
                OriginLog.Error($"NonPresetCultureAnchorOnSelect 失败: {ex.Message}");
            }
        }
    }
}
