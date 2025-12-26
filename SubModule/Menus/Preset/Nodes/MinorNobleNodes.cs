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
        #region 汗廷旁支贵族节点菜单

        private static NarrativeMenu CreateMinorNobleNode1Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu("minor_noble_node1", "preset_origin_selection", "minor_noble_node2", new TextObject("你为何被边缘化"), new TextObject("选择你被边缘化的原因,这会影响你的初始关系和技能倾向"), characters, GetMinorNobleNode1CharacterArgs);

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("minor_noble_maternal", new TextObject("母系旁支，名分尴尬"), new TextObject("你是母系旁支，名分尴尬。魅力+4，与库塞特关系-1，标记：合法性低"), GetMinorNobleMaternalArgs, (m) => true, (m) => MinorNobleMaternalOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("minor_noble_wrong_side", new TextObject("站错队，成了旧账"), new TextObject("你站错队，成了旧账。与精英氏族关系-4，侦察+3，战术+3，标记：政治债务"), GetMinorNobleWrongSideArgs, (m) => true, (m) => MinorNobleWrongSideOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("minor_noble_unrewarded", new TextObject("战功被吞，心怀不平"), new TextObject("你的战功被吞，心怀不平。领导力+4，声望+1，与库塞特关系-2，标记：怨恨"), GetMinorNobleUnrewardedArgs, (m) => true, (m) => MinorNobleUnrewardedOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("minor_noble_too_dangerous", new TextObject("才名过盛，遭人忌惮"), new TextObject("你才名过盛，遭人忌惮。声望+1，魅力+2，与精英氏族关系-2，标记：被监视"), GetMinorNobleTooDangerousArgs, (m) => true, (m) => MinorNobleTooDangerousOnSelect(m), null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetMinorNobleNode1CharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetMinorNobleMaternalArgs(NarrativeMenuOptionArgs args) { }
        private static void MinorNobleMaternalOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了汗廷旁支贵族-Node1-母系旁支"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_minor_noble_marginalized"] = "maternal"; }

        private static void GetMinorNobleWrongSideArgs(NarrativeMenuOptionArgs args) { }
        private static void MinorNobleWrongSideOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了汗廷旁支贵族-Node1-父辈站错队"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_minor_noble_marginalized"] = "wrong_side"; }

        private static void GetMinorNobleUnrewardedArgs(NarrativeMenuOptionArgs args) { }
        private static void MinorNobleUnrewardedOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择汗廷旁支贵族-Node1-封赏不足"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_minor_noble_marginalized"] = "unrewarded"; }

        private static void GetMinorNobleTooDangerousArgs(NarrativeMenuOptionArgs args) { }
        private static void MinorNobleTooDangerousOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了汗廷旁支贵族-Node1-你太危险"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_minor_noble_marginalized"] = "too_dangerous"; }

        private static NarrativeMenu CreateMinorNobleNode2Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu("minor_noble_node2", "minor_noble_node1", "minor_noble_node3", new TextObject("你更擅长\"面子\"还是\"刀子\""), new TextObject("选择你的风格，这会影响你的初始资源和技能倾向"), characters, GetMinorNobleNode2CharacterArgs);

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("minor_noble_style_etiquette", new TextObject("礼法与人情"), new TextObject("你带来礼法与人情。魅力+6，管理+3，金钱+200"), GetMinorNobleStyleEtiquetteArgs, (m) => true, (m) => MinorNobleStyleEtiquetteOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("minor_noble_style_military", new TextObject("军功与威望"), new TextObject("你带来军功与威望。领导力+5，战术+4，金钱+100"), GetMinorNobleStyleMilitaryArgs, (m) => true, (m) => MinorNobleStyleMilitaryOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("minor_noble_style_trade", new TextObject("账本与马队"), new TextObject("你带来账本与马队。贸易+6，管理+3，金钱+250，马匹×1"), GetMinorNobleStyleTradeArgs, (m) => true, (m) => MinorNobleStyleTradeOnSelect(m), null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetMinorNobleNode2CharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetMinorNobleStyleEtiquetteArgs(NarrativeMenuOptionArgs args) { }
        private static void MinorNobleStyleEtiquetteOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了汗廷旁支贵族-Node2-礼仪与赠礼"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_minor_noble_style"] = "etiquette"; }

        private static void GetMinorNobleStyleMilitaryArgs(NarrativeMenuOptionArgs args) { }
        private static void MinorNobleStyleMilitaryOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择汗廷旁支贵族-Node2-战功与伤疤"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_minor_noble_style"] = "military"; }

        private static void GetMinorNobleStyleTradeArgs(NarrativeMenuOptionArgs args) { }
        private static void MinorNobleStyleTradeOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择汗廷旁支贵族-Node2-交易与人脉"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_minor_noble_style"] = "trade"; }

        private static NarrativeMenu CreateMinorNobleNode3Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu("minor_noble_node3", "minor_noble_node2", "minor_noble_node4", new TextObject("开局随骑是哪一种"), new TextObject("选择你的开局随骑,这会影响你的初始兵力"), characters, GetMinorNobleNode3CharacterArgs);

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("minor_noble_retinue_noble", new TextObject("少量贵族随骑（精锐）"), new TextObject("你带来少量贵族随骑，精锐但数量少。初始兵力：贵族子弟×4，骑射手×6，食物×6，士气+3"), GetMinorNobleRetinueNobleArgs, (m) => true, (m) => MinorNobleRetinueNobleOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("minor_noble_retinue_light", new TextObject("轻骑为主（快速）"), new TextObject("你带来轻骑为主，速度快。初始兵力：轻骑兵×8，骑射手×6，食物×6"), GetMinorNobleRetinueLightArgs, (m) => true, (m) => MinorNobleRetinueLightOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("minor_noble_retinue_mixed", new TextObject("混编（更稳定）"), new TextObject("你带来混编家兵，更稳定。初始兵力：骑射手×8，游牧战士×4，食物×7"), GetMinorNobleRetinueMixedArgs, (m) => true, (m) => MinorNobleRetinueMixedOnSelect(m), null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetMinorNobleNode3CharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetMinorNobleRetinueNobleArgs(NarrativeMenuOptionArgs args) { }
        private static void MinorNobleRetinueNobleOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择汗廷旁支贵族-Node3-贵族随骑"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_minor_noble_retinue"] = "noble"; }

        private static void GetMinorNobleRetinueLightArgs(NarrativeMenuOptionArgs args) { }
        private static void MinorNobleRetinueLightOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了汗廷旁支贵族-Node3-轻骑护卫"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_minor_noble_retinue"] = "light"; }

        private static void GetMinorNobleRetinueMixedArgs(NarrativeMenuOptionArgs args) { }
        private static void MinorNobleRetinueMixedOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了汗廷旁支贵族-Node3-混编家兵"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_minor_noble_retinue"] = "mixed"; }

        private static NarrativeMenu CreateMinorNobleNode4Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu("minor_noble_node4", "minor_noble_node3", "narrative_parent_menu", new TextObject("你要先靠近谁"), new TextObject("选择你的政治立场,这会影响你的初始关系"), characters, GetMinorNobleNode4CharacterArgs);

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("minor_noble_alignment_khan", new TextObject("先靠近可汗"), new TextObject("你选择先靠近可汗。与可汗关系+6，与精英氏族关系+1，标记：寻求可汗青睐"), GetMinorNobleAlignmentKhanArgs, (m) => true, (m) => MinorNobleAlignmentKhanOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("minor_noble_alignment_clan", new TextObject("先靠近大氏族"), new TextObject("你选择先靠近大氏族。与精英氏族关系+5，与可汗关系-1，标记：寻求庇护者"), GetMinorNobleAlignmentClanArgs, (m) => true, (m) => MinorNobleAlignmentClanOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("minor_noble_alignment_neutral", new TextObject("先中立观望"), new TextObject("你选择先中立观望。侦察+3，魅力+2，标记：低调行事"), GetMinorNobleAlignmentNeutralArgs, (m) => true, (m) => MinorNobleAlignmentNeutralOnSelect(m), null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetMinorNobleNode4CharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetMinorNobleAlignmentKhanArgs(NarrativeMenuOptionArgs args) { }
        private static void MinorNobleAlignmentKhanOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了汗廷旁支贵族-Node4-靠近可汗"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_minor_noble_alignment"] = "khan"; OriginSystemHelper.OriginSelectionDone = true; }

        private static void GetMinorNobleAlignmentClanArgs(NarrativeMenuOptionArgs args) { }
        private static void MinorNobleAlignmentClanOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了汗廷旁支贵族-Node4-靠近大氏族"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_minor_noble_alignment"] = "clan"; OriginSystemHelper.OriginSelectionDone = true; }

        private static void GetMinorNobleAlignmentNeutralArgs(NarrativeMenuOptionArgs args) { }
        private static void MinorNobleAlignmentNeutralOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择汗廷旁支贵族-Node4-保持中立"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_minor_noble_alignment"] = "neutral"; OriginSystemHelper.OriginSelectionDone = true; }

        #endregion
    }
}
