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
        private static NarrativeMenu CreateNonPresetStartLocationTypeMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "non_preset_start_location_type",
                "non_preset_starting_condition",
                "narrative_parent_menu",
                new TextObject("此刻你身在何处"),
                new TextObject("选择你的起始位置，这会影响你的初始资源和状态"),
                characters,
                (c, o, m) => new List<NarrativeMenuCharacterArgs>()
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "loc_rural_edge",
                new TextObject("乡村边缘"),
                new TextObject("更容易找到廉价粮食与零工，但消息闭塞"),
                (args) => { },
                (m) => true,
                (m) => StartLocationTypeRuralEdgeOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "loc_town_outskirts",
                new TextObject("城镇外围"),
                new TextObject("更容易接触市场与雇佣，但开销更高"),
                (args) => { },
                (m) => true,
                (m) => StartLocationTypeTownOutskirtsOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "loc_trade_road",
                new TextObject("商路沿线"),
                new TextObject("你在路上，机会多，危险也多"),
                (args) => { },
                (m) => true,
                (m) => StartLocationTypeTradeRoadOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "loc_border_wild",
                new TextObject("边境荒野"),
                new TextObject("你离秩序很远，离自由也很近"),
                (args) => { },
                (m) => true,
                (m) => StartLocationTypeBorderWildOnSelect(m),
                null
            ));

            return menu;
        }

        private static void StartLocationTypeRuralEdgeOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择了起始位置类型-乡村边缘");
                OriginSystemHelper.SelectedNonPresetOriginNodes["np_node_start_location_type"] = "loc_rural_edge";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"StartLocationTypeRuralEdgeOnSelect 失败: {ex.Message}");
            }
        }

        private static void StartLocationTypeTownOutskirtsOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择了起始位置类型-城镇外围");
                OriginSystemHelper.SelectedNonPresetOriginNodes["np_node_start_location_type"] = "loc_town_outskirts";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"StartLocationTypeTownOutskirtsOnSelect 失败: {ex.Message}");
            }
        }

        private static void StartLocationTypeTradeRoadOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择了起始位置类型-商路沿线");
                OriginSystemHelper.SelectedNonPresetOriginNodes["np_node_start_location_type"] = "loc_trade_road";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"StartLocationTypeTradeRoadOnSelect 失败: {ex.Message}");
            }
        }

        private static void StartLocationTypeBorderWildOnSelect(CharacterCreationManager manager)
        {
            try
            {
                OriginLog.Info("用户选择了起始位置类型-边境荒野");
                OriginSystemHelper.SelectedNonPresetOriginNodes["np_node_start_location_type"] = "loc_border_wild";
            }
            catch (Exception ex)
            {
                OriginLog.Error($"StartLocationTypeBorderWildOnSelect 失败: {ex.Message}");
            }
        }
    }
}






