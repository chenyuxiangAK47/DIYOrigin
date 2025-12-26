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
        private static NarrativeMenu CreateNonPresetNomadRoleMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "non_preset_nomad_role",
                "non_preset_social_origin",
                "non_preset_skill_background",
                new TextObject("你在部族里做什么"),
                new TextObject("选择你在游牧营地中的角色,这会影响你的技能和装备"),
                characters,
                GetNonPresetNomadRoleCharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "nomad_role_scout",
                new TextObject("放哨与探路"),
                new TextObject("你习惯了把\"危险\"提前半天发现"),
                GetNomadRoleScoutArgs,
                (m) => true,
                (m) => NomadRoleScoutOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "nomad_role_vet",
                new TextObject("兽医/照料牲畜"),
                new TextObject("你不一定能杀人，但你能让马活下来"),
                GetNomadRoleVetArgs,
                (m) => true,
                (m) => NomadRoleVetOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "nomad_role_horse_handler",
                new TextObject("牵马与驭马"),
                new TextObject("你比别人更懂马，也更懂撤退的路线"),
                GetNomadRoleHorseHandlerArgs,
                (m) => true,
                (m) => NomadRoleHorseHandlerOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "nomad_role_guard",
                new TextObject("营地护卫"),
                new TextObject("你站在篝火外侧，手里握着短矛"),
                GetNomadRoleGuardArgs,
                (m) => true,
                (m) => NomadRoleGuardOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetNonPresetNomadRoleCharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetNomadRoleScoutArgs(NarrativeMenuOptionArgs args) { }

        private static void NomadRoleScoutOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择了游牧营地角色-放哨与探路");
                OriginSystemHelper.SelectedNonPresetOriginNodes["np_node_nomad_role"] = "nomad_role_scout";
                OriginSystemHelper.NonPresetOrigin.FlavorNode = "nomad_role_scout";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"NomadRoleScoutOnSelect 失败: {ex.Message}");
            }
        }

        private static void GetNomadRoleVetArgs(NarrativeMenuOptionArgs args) { }

        private static void NomadRoleVetOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择了游牧营地角色-兽医/照料牲畜");
                OriginSystemHelper.SelectedNonPresetOriginNodes["np_node_nomad_role"] = "nomad_role_vet";
                OriginSystemHelper.NonPresetOrigin.FlavorNode = "nomad_role_vet";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"NomadRoleVetOnSelect 失败: {ex.Message}");
            }
        }

        private static void GetNomadRoleHorseHandlerArgs(NarrativeMenuOptionArgs args) { }

        private static void NomadRoleHorseHandlerOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择了游牧营地角色-牵马与驭马");
                OriginSystemHelper.SelectedNonPresetOriginNodes["np_node_nomad_role"] = "nomad_role_horse_handler";
                OriginSystemHelper.NonPresetOrigin.FlavorNode = "nomad_role_horse_handler";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"NomadRoleHorseHandlerOnSelect 失败: {ex.Message}");
            }
        }

        private static void GetNomadRoleGuardArgs(NarrativeMenuOptionArgs args) { }

        private static void NomadRoleGuardOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择了游牧营地角色-营地护卫");
                OriginSystemHelper.SelectedNonPresetOriginNodes["np_node_nomad_role"] = "nomad_role_guard";
                OriginSystemHelper.NonPresetOrigin.FlavorNode = "nomad_role_guard";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"NomadRoleGuardOnSelect 失败: {ex.Message}");
            }
        }
    }
}
