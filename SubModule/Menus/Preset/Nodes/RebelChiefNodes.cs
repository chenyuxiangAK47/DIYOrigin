using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace OriginSystemMod
{
    public static partial class OriginSystemPatches
    {
        #region 草原叛酋节点菜单

        private static NarrativeMenu CreateRebelChiefNode1Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "rebel_chief_node1",
                "preset_origin_selection",
                "rebel_chief_node2",
                new TextObject("你为何被定为叛酋"),
                new TextObject("家族编年史中，记录着你被汗廷定为叛酋的那一天。是什么让你走上了这条流亡之路？是拒绝纳贡的坚持，是劫掠商队的生存，是拒绝征兵的独立，还是保护族人的勇气？这段经历不仅决定了你与汗廷的关系，也塑造了你在流亡中的技能与意志"),
                characters,
                GetRebelChiefNode1CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "rebel_reason_refuse_tribute",
                new TextObject("拒绝纳贡"),
                new TextObject("族人后来一直在说：你拒绝向汗廷缴纳过重的贡赋，认为这是对部族的不公。你的拒绝引起了汗廷的不满，最终被定为叛酋。你学会了如何在资源有限的情况下管理部族，如何在压力下保持领导力。虽然你失去了与汗廷的关系，但你赢得了族人的尊敬。领导力+5，管理+3，声望+1，与库塞特可汗关系-7，标记：被怀疑"),
                GetRebelReasonRefuseTributeArgs,
                (m) => true,
                (m) => RebelReasonRefuseTributeOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "rebel_reason_raid_caravan",
                new TextObject("劫掠商队被点名"),
                new TextObject("部族编年史记载，你为了部族的生存，劫掠了一支商队。虽然这在草原上并不罕见，但这次行动引起了商人们的强烈反应，他们向汗廷告发，你因此被定为叛酋。你获得了战利品，但也失去了商人们的信任。那些商人将你视为敌人，而汗廷也对你下达了追捕令。狡诈+5，侦察+3，仁慈-1，声望+1，金币+300，与商人关系-7，与库塞特可汗关系-7，标记：被怀疑"),
                GetRebelReasonRaidCaravanArgs,
                (m) => true,
                (m) => RebelReasonRaidCaravanOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "rebel_reason_refuse_service",
                new TextObject("拒绝服兵役/拒绝出征"),
                new TextObject("家族记忆中最艰难的一页：你拒绝服从汗廷的征兵令，认为这会让你的部族失去劳动力。你的拒绝被视为对汗廷的挑战，最终被定为叛酋。你学会了如何在压力下保护族人，如何在困境中保持独立。虽然你失去了与汗廷的关系，但你保护了族人的利益。领导力+3，战术+3，与库塞特可汗关系-7，标记：被怀疑"),
                GetRebelReasonRefuseServiceArgs,
                (m) => true,
                (m) => RebelReasonRefuseServiceOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "rebel_reason_protect_tribe",
                new TextObject("为保护族人顶撞汗廷"),
                new TextObject("族人后来一直在传颂：你为了保护族人，顶撞了汗廷的使者。虽然你的行为赢得了族人的尊敬，但也激怒了汗廷，你因此被定为叛酋。你获得了声望和士气，但也失去了与汗廷的关系。这段经历让你展现出了真正的领导才能，也让你的族人更加忠诚。领导力+3，魅力+2，勇敢+1，声望+1，士气+5，与库塞特可汗关系-3"),
                GetRebelReasonProtectTribeArgs,
                (m) => true,
                (m) => RebelReasonProtectTribeOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetRebelChiefNode1CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetRebelReasonRefuseTributeArgs(NarrativeMenuOptionArgs args) { }
        private static void RebelReasonRefuseTributeOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了草原叛酋-Node1-拒绝纳贡");
            OriginSystemHelper.SelectedPresetOriginNodes["khz_node_rebel_reason"] = "refuse_tribute";
        }

        private static void GetRebelReasonRaidCaravanArgs(NarrativeMenuOptionArgs args) { }
        private static void RebelReasonRaidCaravanOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了草原叛酋-Node1-劫掠商队");
            OriginSystemHelper.SelectedPresetOriginNodes["khz_node_rebel_reason"] = "raid_caravan";
        }

        private static void GetRebelReasonRefuseServiceArgs(NarrativeMenuOptionArgs args) { }
        private static void RebelReasonRefuseServiceOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了草原叛酋-Node1-拒绝服兵役");
            OriginSystemHelper.SelectedPresetOriginNodes["khz_node_rebel_reason"] = "refuse_service";
        }

        private static void GetRebelReasonProtectTribeArgs(NarrativeMenuOptionArgs args) { }
        private static void RebelReasonProtectTribeOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了草原叛酋-Node1-保护族人");
            OriginSystemHelper.SelectedPresetOriginNodes["khz_node_rebel_reason"] = "protect_tribe";
        }

        private static NarrativeMenu CreateRebelChiefNode2Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "rebel_chief_node2",
                "rebel_chief_node1",
                "rebel_chief_node3",
                new TextObject("你的叛乱战帮由谁组成"),
                new TextObject("族人后来一直在说：当你带着族人离开时，你的战帮是由谁组成的？是熟悉草原生活的游牧战士，是擅长快速移动的轻骑兵，还是混编的平衡配置？你的选择不仅决定了你最初的战斗力，也影响了你在流亡中的生存方式"),
                characters,
                GetRebelChiefNode2CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "rebel_warband_nomads",
                new TextObject("以游牧战士为主"),
                new TextObject("你的战帮主要由游牧战士组成，这些战士熟悉草原的生活，擅长在艰苦的环境中生存。他们虽然装备简单，但战斗意志坚定，愿意为你而战。这些战士是你部族最忠诚的成员，他们跟随你流亡，是因为他们相信你能带领他们找到新的家园。初始兵力：游牧战士×16，部落青年×6，食物×8"),
                GetRebelWarbandNomadsArgs,
                (m) => true,
                (m) => RebelWarbandNomadsOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "rebel_warband_light_cav",
                new TextObject("以轻骑/骑射为主"),
                new TextObject("你的战帮主要由轻骑兵和骑射手组成，这些骑兵擅长快速移动和远程攻击。他们能够在草原上快速来去，给敌人造成困扰。这些骑兵是你部族最精锐的战士，他们跟随你流亡，是因为他们相信你的战术才能。初始兵力：骑射手×10，轻骑兵×6，马匹×1，食物×6"),
                GetRebelWarbandLightCavArgs,
                (m) => true,
                (m) => RebelWarbandLightCavOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "rebel_warband_mixed",
                new TextObject("混编（更稳）"),
                new TextObject("你的战帮是混编的，包括游牧战士、骑射手和轻骑兵。这种配置更加平衡，能够在各种情况下应对。虽然不如单一兵种专业，但在流亡中，这种平衡配置让你能够应对各种挑战。初始兵力：游牧战士×10，骑射手×6，轻骑兵×4，食物×7"),
                GetRebelWarbandMixedArgs,
                (m) => true,
                (m) => RebelWarbandMixedOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetRebelChiefNode2CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetRebelWarbandNomadsArgs(NarrativeMenuOptionArgs args) { }
        private static void RebelWarbandNomadsOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了草原叛酋-Node2-游牧战士为主");
            OriginSystemHelper.SelectedPresetOriginNodes["khz_node_rebel_warband_makeup"] = "nomads";
        }

        private static void GetRebelWarbandLightCavArgs(NarrativeMenuOptionArgs args) { }
        private static void RebelWarbandLightCavOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了草原叛酋-Node2-轻骑兵为主");
            OriginSystemHelper.SelectedPresetOriginNodes["khz_node_rebel_warband_makeup"] = "light_cav";
        }

        private static void GetRebelWarbandMixedArgs(NarrativeMenuOptionArgs args) { }
        private static void RebelWarbandMixedOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了草原叛酋-Node2-混编");
            OriginSystemHelper.SelectedPresetOriginNodes["khz_node_rebel_warband_makeup"] = "mixed";
        }

        private static NarrativeMenu CreateRebelChiefNode3Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "rebel_chief_node3",
                "rebel_chief_node2",
                "rebel_chief_node4",
                new TextObject("你带走了什么\"叛酋象征\""),
                new TextObject("族人后来一直在说：当你带着族人离开时，你带走了什么？是部族的传世战旗，是积累的巨额财富，是证明身份的黄金印记，还是忠诚的伙伴？你带走的东西不仅成为你流亡路上的资源，也成为了你家族故事的起点，决定了你将以怎样的姿态重新面对这个世界"),
                characters,
                GetRebelChiefNode3CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "rebel_symbol_banner",
                new TextObject("部族的传世战旗（顶级宝贝）"),
                new TextObject("家族传说中，你带走了部族的传世战旗，这面旗帜是部族最珍贵的宝物，代表着你的部族传统和荣誉。它能够激励你的部下，让他们在战斗中保持高昂的士气。每当你在战场上举起这面旗帜，你的族人就会想起他们的传统，想起他们的荣誉，想起他们为什么要跟随你流亡。声望+2，士气+6，领导力+5，标记：拥有象征物"),
                GetRebelSymbolBannerArgs,
                (m) => true,
                (m) => RebelSymbolBannerOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "rebel_symbol_treasure",
                new TextObject("部族积累的巨额财富（大）"),
                new TextObject("部族编年史中记录，你带走了部族积累的金银和马具，这些物资能够帮助你在流亡中生存。虽然数量有限，但足以支撑你度过最初的困难时期。这些财富是你部族多年积累的成果，你带着它们离开，是为了在流亡中能够维持部族的基本运转。金币+15000，马匹+3，标记：短期富有"),
                GetRebelSymbolTreasureArgs,
                (m) => true,
                (m) => RebelSymbolTreasureOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "rebel_symbol_seal",
                new TextObject("部族传承的黄金印记（顶级宝贝）"),
                new TextObject("族人后来才知道，你带走了证明你身份的黄金印记，这枚印记是部族传承的宝物，能够证明你的部族身份和地位。它能够帮助你在与其他部族交往时获得认可，也能让你在流亡中保持你的身份认同。这枚印记是你部族最珍贵的传承，你带着它离开，是为了证明你仍然是部族的首领。声望+2，魅力+5，标记：拥有证明"),
                GetRebelSymbolSealArgs,
                (m) => true,
                (m) => RebelSymbolSealOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "rebel_symbol_people",
                new TextObject("忠诚的伙伴：鹰（the Hawk）"),
                new TextObject("家族记忆中最温暖的一页：你带走了最忠诚的族人，其中有一位名叫\"鹰\"的勇士，他曾经跟随可汗征战多年，因为血仇而被迫离开。他愿意跟随你流亡，成为你最可靠的伙伴。这些同伴成为了你最初的追随者，他们感激你的信任，愿意跟随你一起面对未知的未来。初始兵力：游牧战士×6，士气+3，标记：拥有追随者"),
                GetRebelSymbolPeopleArgs,
                (m) => true,
                (m) => RebelSymbolPeopleOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetRebelChiefNode3CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetRebelSymbolBannerArgs(NarrativeMenuOptionArgs args) { }
        private static void RebelSymbolBannerOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了草原叛酋-Node3-传世战旗");
            OriginSystemHelper.SelectedPresetOriginNodes["khz_node_rebel_symbol"] = "banner";
        }

        private static void GetRebelSymbolTreasureArgs(NarrativeMenuOptionArgs args) { }
        private static void RebelSymbolTreasureOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了草原叛酋-Node3-巨额财富");
            OriginSystemHelper.SelectedPresetOriginNodes["khz_node_rebel_symbol"] = "treasure";
        }

        private static void GetRebelSymbolSealArgs(NarrativeMenuOptionArgs args) { }
        private static void RebelSymbolSealOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了草原叛酋-Node3-黄金印记");
            OriginSystemHelper.SelectedPresetOriginNodes["khz_node_rebel_symbol"] = "seal";
        }

        private static void GetRebelSymbolPeopleArgs(NarrativeMenuOptionArgs args) { }
        private static void RebelSymbolPeopleOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了草原叛酋-Node3-忠诚伙伴");
            OriginSystemHelper.SelectedPresetOriginNodes["khz_node_rebel_symbol"] = "people";
        }

        private static NarrativeMenu CreateRebelChiefNode4Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "rebel_chief_node4",
                "rebel_chief_node3",
                "narrative_parent_menu",
                new TextObject("你第一步要做什么"),
                new TextObject("自由就在眼前，但前路茫茫。你要如何开始你的流亡生涯？是隐入草原先活下来，是以战养战先打一票，还是找人结盟换一口气？你的选择不仅决定了你最初的资源与技能，也决定了你将面临怎样的挑战与机遇。每一个方向都有不同的故事在等待着你"),
                characters,
                GetRebelChiefNode4CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "rebel_first_move_hide",
                new TextObject("隐入草原，先活下来"),
                new TextObject("你选择隐入草原，先确保自己和族人的生存。你学会了如何在草原上隐藏，如何通过侦察来避开追捕。你低调行事，等待合适的时机。你知道，在流亡中，生存比什么都重要。你带着族人深入草原的深处，避开所有可能的追捕者，用你的侦察技能来保护自己和族人。食物×6，侦察+6，标记：低调行事"),
                GetRebelFirstMoveHideArgs,
                (m) => true,
                (m) => RebelFirstMoveHideOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "rebel_first_move_raid",
                new TextObject("以战养战，先打一票"),
                new TextObject("你选择以战养战，通过劫掠来获取资源。你知道这有风险，但也知道这是快速获得资源的方式。你愿意承担风险，换取快速的发展。你带着族人袭击了一支商队，获得了急需的物资和金币。虽然这让你更加危险，但也让你有了继续流亡的资本。金币+300，声望+1，标记：风险开局"),
                GetRebelFirstMoveRaidArgs,
                (m) => true,
                (m) => RebelFirstMoveRaidOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "rebel_first_move_ally",
                new TextObject("找人结盟，换一口气"),
                new TextObject("你选择寻找盟友，希望通过结盟来获得庇护。你知道单打独斗很难生存，因此希望通过魅力和关系来建立联盟。你带着族人找到了一个愿意庇护你的精英氏族，他们愿意给你一个喘息的机会。虽然这让你欠下了人情，但也让你有了一个安全的起点。魅力+4，与精英氏族关系+2，标记：寻求庇护者"),
                GetRebelFirstMoveAllyArgs,
                (m) => true,
                (m) => RebelFirstMoveAllyOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetRebelChiefNode4CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetRebelFirstMoveHideArgs(NarrativeMenuOptionArgs args) { }
        private static void RebelFirstMoveHideOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了草原叛酋-Node4-隐入草原");
            OriginSystemHelper.SelectedPresetOriginNodes["khz_node_rebel_first_move"] = "hide";
            OriginSystemHelper.OriginSelectionDone = true;
        }

        private static void GetRebelFirstMoveRaidArgs(NarrativeMenuOptionArgs args) { }
        private static void RebelFirstMoveRaidOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了草原叛酋-Node4-以战养战");
            OriginSystemHelper.SelectedPresetOriginNodes["khz_node_rebel_first_move"] = "raid";
            OriginSystemHelper.OriginSelectionDone = true;
        }

        private static void GetRebelFirstMoveAllyArgs(NarrativeMenuOptionArgs args) { }
        private static void RebelFirstMoveAllyOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了草原叛酋-Node4-找人结盟");
            OriginSystemHelper.SelectedPresetOriginNodes["khz_node_rebel_first_move"] = "ally";
            OriginSystemHelper.OriginSelectionDone = true;
        }

        #endregion
    }
}
