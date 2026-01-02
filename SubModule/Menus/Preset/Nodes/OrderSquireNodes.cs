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
        #region 侍奉骑士团的扈从节点菜单

        private static NarrativeMenu CreateOrderSquireNode1Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "order_squire_node1",
                "preset_origin_selection",
                "order_squire_node2",
                new TextObject("你为何离开骑士团"),
                new TextObject("家族编年史中，记录着你离开骑士团的那一天。你受过系统骑士教育，但被外放试炼——你要证明你配得上\'授剑礼\'。你为何离开骑士团？是外出立功试炼，是与团内权贵冲突，还是信念动摇想看真实世界？"),
                characters,
                GetOrderSquireNode1CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "order_squire_leave_trial",
                new TextObject("外出立功试炼"),
                new TextObject("你选择外出立功试炼，通过完成实际的战斗任务来证明自己配得上授剑礼。虽然这让你暂时离开了骑士团，但你的骑士道也因此得到了显著提升。开局任务：完成3个\'守护/救援\'类任务，骑士道+20"),
                GetOrderSquireLeaveTrialArgs,
                (m) => true,
                (m) => OrderSquireLeaveTrialOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "order_squire_leave_conflict",
                new TextObject("与团内权贵冲突"),
                new TextObject("你与团内的权贵产生了冲突，被迫离开骑士团。虽然这让你与某些人产生了矛盾，但也让你学会了如何在复杂的环境中生存。魅力+15，战术+15，骑士道+5，标记：骑士团敌人"),
                GetOrderSquireLeaveConflictArgs,
                (m) => true,
                (m) => OrderSquireLeaveConflictOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "order_squire_leave_doubt",
                new TextObject("信念动摇想看真实世界"),
                new TextObject("你的信念开始动摇，你想看看真实的世界，而不仅仅是骑士团里教给你的那些。虽然这让你的骑士道有所下降，但你也因此获得了更多的自由和经验。侦察+20，骑士道-5，标记：质疑信仰"),
                GetOrderSquireLeaveDoubtArgs,
                (m) => true,
                (m) => OrderSquireLeaveDoubtOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetOrderSquireNode1CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetOrderSquireLeaveTrialArgs(NarrativeMenuOptionArgs args) { }
        private static void OrderSquireLeaveTrialOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了侍奉骑士团的扈从-Node1-外出立功试炼");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_order_squire_leave"] = "trial";
        }

        private static void GetOrderSquireLeaveConflictArgs(NarrativeMenuOptionArgs args) { }
        private static void OrderSquireLeaveConflictOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了侍奉骑士团的扈从-Node1-与团内权贵冲突");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_order_squire_leave"] = "conflict";
        }

        private static void GetOrderSquireLeaveDoubtArgs(NarrativeMenuOptionArgs args) { }
        private static void OrderSquireLeaveDoubtOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了侍奉骑士团的扈从-Node1-信念动摇想看真实世界");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_order_squire_leave"] = "doubt";
        }

        private static NarrativeMenu CreateOrderSquireNode2Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "order_squire_node2",
                "order_squire_node1",
                "order_squire_node3",
                new TextObject("你的戒律是哪条"),
                new TextObject("族人后来一直在说：在骑士团中，你遵守的是哪条戒律？是仁慈（护弱），是荣誉（不抢平民），还是清修（拒绝灰色）？你的选择不仅决定了你的价值观，也影响了你的骑士道"),
                characters,
                GetOrderSquireNode2CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "order_squire_vow_mercy",
                new TextObject("仁慈（护弱）"),
                new TextObject("你遵守仁慈的戒律，保护弱者是你最重要的职责。你相信真正的骑士应该保护那些无法保护自己的人，而不是仅仅追求个人的荣耀。骑士道+35"),
                GetOrderSquireVowMercyArgs,
                (m) => true,
                (m) => OrderSquireVowMercyOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "order_squire_vow_honor",
                new TextObject("荣誉（不抢平民）"),
                new TextObject("你遵守荣誉的戒律，绝不劫掠平民是你的底线。你相信真正的骑士应该保护人民的财产和尊严，而不是为了自己的利益而伤害他们。骑士道+20，标记：不劫掠誓言（劫掠就扣骑士道）"),
                GetOrderSquireVowHonorArgs,
                (m) => true,
                (m) => OrderSquireVowHonorOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "order_squire_vow_ascetic",
                new TextObject("清修（拒绝灰色）"),
                new TextObject("你遵守清修的戒律，拒绝一切灰色的收入和手段。你相信真正的骑士应该保持纯洁，即使这意味着要拒绝某些看起来合理的收入。骑士道+25，金币-500（你拒绝黑钱）"),
                GetOrderSquireVowAsceticArgs,
                (m) => true,
                (m) => OrderSquireVowAsceticOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetOrderSquireNode2CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetOrderSquireVowMercyArgs(NarrativeMenuOptionArgs args) { }
        private static void OrderSquireVowMercyOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了侍奉骑士团的扈从-Node2-仁慈（护弱）");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_order_squire_vow"] = "mercy";
        }

        private static void GetOrderSquireVowHonorArgs(NarrativeMenuOptionArgs args) { }
        private static void OrderSquireVowHonorOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了侍奉骑士团的扈从-Node2-荣誉（不抢平民）");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_order_squire_vow"] = "honor";
        }

        private static void GetOrderSquireVowAsceticArgs(NarrativeMenuOptionArgs args) { }
        private static void OrderSquireVowAsceticOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了侍奉骑士团的扈从-Node2-清修（拒绝灰色）");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_order_squire_vow"] = "ascetic";
        }

        private static NarrativeMenu CreateOrderSquireNode3Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "order_squire_node3",
                "order_squire_node2",
                "order_squire_node4",
                new TextObject("骑士团给你的资源是什么"),
                new TextObject("部族编年史中记录，骑士团给了你什么样的资源？是更好的甲，是更多扈从，还是旅费？你的选择不仅决定了你最初的装备和兵力，也影响了你的财务状况"),
                characters,
                GetOrderSquireNode3CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "order_squire_resource_armor",
                new TextObject("更好的甲"),
                new TextObject("骑士团给了你更好的甲，这让你在战斗中更加安全。虽然你没有额外的资金，但你的防御能力得到了显著提升。护甲升一档（T3→T4），金币0"),
                GetOrderSquireResourceArmorArgs,
                (m) => true,
                (m) => OrderSquireResourceArmorOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "order_squire_resource_retinue",
                new TextObject("更多扈从"),
                new TextObject("骑士团给了你更多的扈从，这让你在战场上有了更多的支持。虽然你没有额外的装备，但你的兵力得到了显著提升。额外兵力：扈从×10，食物+10"),
                GetOrderSquireResourceRetinueArgs,
                (m) => true,
                (m) => OrderSquireResourceRetinueOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "order_squire_resource_travel",
                new TextObject("旅费"),
                new TextObject("骑士团给了你旅费，这让你能够自由行动。虽然这给了你更多的资金，但你也因此背负了某种债务。金币+5000，但骑士道-5（你带着\'债\'）"),
                GetOrderSquireResourceTravelArgs,
                (m) => true,
                (m) => OrderSquireResourceTravelOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetOrderSquireNode3CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetOrderSquireResourceArmorArgs(NarrativeMenuOptionArgs args) { }
        private static void OrderSquireResourceArmorOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了侍奉骑士团的扈从-Node3-更好的甲");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_order_squire_resource"] = "armor";
        }

        private static void GetOrderSquireResourceRetinueArgs(NarrativeMenuOptionArgs args) { }
        private static void OrderSquireResourceRetinueOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了侍奉骑士团的扈从-Node3-更多扈从");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_order_squire_resource"] = "retinue";
        }

        private static void GetOrderSquireResourceTravelArgs(NarrativeMenuOptionArgs args) { }
        private static void OrderSquireResourceTravelOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了侍奉骑士团的扈从-Node3-旅费");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_order_squire_resource"] = "travel";
        }

        private static NarrativeMenu CreateOrderSquireNode4Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "order_squire_node4",
                "order_squire_node3",
                null,
                new TextObject("你要证明什么"),
                new TextObject("家族记忆中最重要的一页：作为扈从，你要证明什么？是守护村庄免遭劫掠，是赢下一场正规战，还是击败一名成名骑士？你的选择不仅决定了你证明自己的方式，也预示了你将获得什么样的认可"),
                characters,
                GetOrderSquireNode4CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "order_squire_proof_protect",
                new TextObject("守护村庄免遭劫掠"),
                new TextObject("你要证明你能够守护村庄免遭劫掠。你相信真正的骑士应该保护人民，而不是仅仅追求个人的荣耀。开局任务链：守护村庄，完成奖励骑士道+10"),
                GetOrderSquireProofProtectArgs,
                (m) => true,
                (m) => OrderSquireProofProtectOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "order_squire_proof_victory",
                new TextObject("赢下一场正规战"),
                new TextObject("你要证明你能够在正规战斗中获胜。你相信真正的骑士应该在战场上证明自己的价值。开局任务：击败一支正规军/雇佣军，完成奖励声望+15"),
                GetOrderSquireProofVictoryArgs,
                (m) => true,
                (m) => OrderSquireProofVictoryOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "order_squire_proof_champion",
                new TextObject("击败一名成名骑士（竞技/单挑）"),
                new TextObject("你要证明你能够在单挑中击败一名成名骑士。你相信真正的骑士应该在公平的对决中证明自己的价值。开局任务：锦标赛夺冠/或特定NPC决斗"),
                GetOrderSquireProofChampionArgs,
                (m) => true,
                (m) => OrderSquireProofChampionOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetOrderSquireNode4CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetOrderSquireProofProtectArgs(NarrativeMenuOptionArgs args) { }
        private static void OrderSquireProofProtectOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了侍奉骑士团的扈从-Node4-守护村庄免遭劫掠");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_order_squire_proof"] = "protect";
            OriginSystemHelper.OriginSelectionDone = true;
        }

        private static void GetOrderSquireProofVictoryArgs(NarrativeMenuOptionArgs args) { }
        private static void OrderSquireProofVictoryOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了侍奉骑士团的扈从-Node4-赢下一场正规战");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_order_squire_proof"] = "victory";
            OriginSystemHelper.OriginSelectionDone = true;
        }

        private static void GetOrderSquireProofChampionArgs(NarrativeMenuOptionArgs args) { }
        private static void OrderSquireProofChampionOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了侍奉骑士团的扈从-Node4-击败一名成名骑士");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_order_squire_proof"] = "champion";
            OriginSystemHelper.OriginSelectionDone = true;
        }

        #endregion
    }
}
