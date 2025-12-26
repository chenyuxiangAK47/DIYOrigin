using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using OriginSystemMod;

namespace OriginSystemMod
{
    public static partial class OriginSystemPatches
    {
        private static NarrativeMenu CreateNonPresetContactTypeMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "non_preset_contact_type",
                "non_preset_starting_condition",
                "narrative_parent_menu",
                new TextObject("你有一位熟人吗"),
                new TextObject("选择你拥有的联系人类型，这会影响你的初始关系和资源"),
                characters,
                (c, o, m) => new List<NarrativeMenuCharacterArgs>()
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "ct_caravan_contact",
                new TextObject("商队熟人"),
                new TextObject("他不一定帮你打仗，但他知道哪里能赚钱。"),
                (args) => { },
                (m) => true,
                (m) => ContactTypeCaravanContactOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "ct_workshop_contact",
                new TextObject("工坊熟人"),
                new TextObject("他能让你\"做工换饭\"，也能让你认识更多人。"),
                (args) => { },
                (m) => true,
                (m) => ContactTypeWorkshopContactOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "ct_officer_contact",
                new TextObject("退伍军官熟人（泛化）"),
                new TextObject("他懂兵，也懂规矩，能教你少走弯路。"),
                (args) => { },
                (m) => true,
                (m) => ContactTypeOfficerContactOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "ct_none",
                new TextObject("没有"),
                new TextObject("你不欠谁人情，也没人会给你方便。"),
                (args) => { },
                (m) => true,
                (m) => ContactTypeNoneOnSelect(m),
                null
            ));

            return menu;
        }

        private static void ContactTypeCaravanContactOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择了联系人类型-商队熟人");
                OriginSystemHelper.SelectedNonPresetOriginNodes["np_node_contact_type"] = "ct_caravan_contact";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ContactTypeCaravanContactOnSelect 失败: {ex.Message}");
            }
        }

        private static void ContactTypeWorkshopContactOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择了联系人类型-工坊熟人");
                OriginSystemHelper.SelectedNonPresetOriginNodes["np_node_contact_type"] = "ct_workshop_contact";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ContactTypeWorkshopContactOnSelect 失败: {ex.Message}");
            }
        }

        private static void ContactTypeOfficerContactOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择了联系人类型-退伍军官熟人");
                OriginSystemHelper.SelectedNonPresetOriginNodes["np_node_contact_type"] = "ct_officer_contact";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ContactTypeOfficerContactOnSelect 失败: {ex.Message}");
            }
        }

        private static void ContactTypeNoneOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择了联系人类型-没有");
                OriginSystemHelper.SelectedNonPresetOriginNodes["np_node_contact_type"] = "ct_none";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"ContactTypeNoneOnSelect 失败: {ex.Message}");
            }
        }
    }
}














