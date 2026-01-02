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
        #region 自由民团的推旗人节点菜单

        private static NarrativeMenu CreateFreeMilitiaLeaderNode1Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "free_militia_leader_node1",
                "preset_origin_selection",
                "free_militia_leader_node2",
                new TextObject("你为何举旗"),
                new TextObject("家族编年史中，记录着你举起自由民团旗帜的那一天。你不是骑士，但你举旗保护家园——你要不要\'求封骑\'？你为何举旗？是海寇/强盗逼村，是领主失职，还是饥荒？"),
                characters,
                GetFreeMilitiaLeaderNode1CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "militia_reason_bandits",
                new TextObject("海寇/强盗逼村"),
                new TextObject("海寇和强盗威胁着你的村庄，你不得不举起旗帜来保护家园。虽然你没有受过正规训练，但你有强烈的求生意志和守护家园的决心。侦察+15，士气+3，标记：保卫家园"),
                GetMilitiaReasonBanditsArgs,
                (m) => true,
                (m) => MilitiaReasonBanditsOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "militia_reason_lord_fail",
                new TextObject("领主失职"),
                new TextObject("你的领主失职，无法保护人民，你决定自己举旗。虽然这让你与领主产生了矛盾，但也让你学会了如何用自己的力量来保护自己。魅力+15，对该领主关系-20，标记：反领主"),
                GetMilitiaReasonLordFailArgs,
                (m) => true,
                (m) => MilitiaReasonLordFailOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "militia_reason_famine",
                new TextObject("饥荒"),
                new TextObject("饥荒威胁着你的村庄，你举起旗帜来组织人民自救。虽然这让你面临更多的困难，但你也学会了如何管理资源和组织人民。管理+15，食物+30，标记：饥荒岁月"),
                GetMilitiaReasonFamineArgs,
                (m) => true,
                (m) => MilitiaReasonFamineOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetFreeMilitiaLeaderNode1CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetMilitiaReasonBanditsArgs(NarrativeMenuOptionArgs args) { }
        private static void MilitiaReasonBanditsOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了自由民团的推旗人-Node1-海寇/强盗逼村");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_militia_reason"] = "bandits";
        }

        private static void GetMilitiaReasonLordFailArgs(NarrativeMenuOptionArgs args) { }
        private static void MilitiaReasonLordFailOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了自由民团的推旗人-Node1-领主失职");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_militia_reason"] = "lord_fail";
        }

        private static void GetMilitiaReasonFamineArgs(NarrativeMenuOptionArgs args) { }
        private static void MilitiaReasonFamineOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了自由民团的推旗人-Node1-饥荒");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_militia_reason"] = "famine";
        }

        private static NarrativeMenu CreateFreeMilitiaLeaderNode2Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "free_militia_leader_node2",
                "free_militia_leader_node1",
                "free_militia_leader_node3",
                new TextObject("你的\'乡土道义\'是什么"),
                new TextObject("族人后来一直在说：作为自由民团的推旗人，你的\'乡土道义\'是什么？是先护弱，是先惩恶，还是先活命？你的选择不仅决定了你的价值观，也影响了你在村庄中的声望"),
                characters,
                GetFreeMilitiaLeaderNode2CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "militia_code_protect",
                new TextObject("先护弱"),
                new TextObject("你的乡土道义是先保护弱者。你相信真正的领导者应该保护那些无法保护自己的人，而不是仅仅追求个人的利益。村庄关系+10，标记：保护弱者"),
                GetMilitiaCodeProtectArgs,
                (m) => true,
                (m) => MilitiaCodeProtectOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "militia_code_punish",
                new TextObject("先惩恶"),
                new TextObject("你的乡土道义是先惩罚邪恶。你相信只有通过惩罚那些作恶的人，才能真正保护人民。战斗技能+10，标记：惩罚邪恶"),
                GetMilitiaCodePunishArgs,
                (m) => true,
                (m) => MilitiaCodePunishOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "militia_code_survive",
                new TextObject("先活命"),
                new TextObject("你的乡土道义是先活命。你相信只有先保证自己和人民的生存，才能谈其他。虽然这让你看起来有些功利，但也让你在困难的环境中能够生存下来。金币+2000，标记：生存第一"),
                GetMilitiaCodeSurviveArgs,
                (m) => true,
                (m) => MilitiaCodeSurviveOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetFreeMilitiaLeaderNode2CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetMilitiaCodeProtectArgs(NarrativeMenuOptionArgs args) { }
        private static void MilitiaCodeProtectOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了自由民团的推旗人-Node2-先护弱");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_militia_code"] = "protect";
        }

        private static void GetMilitiaCodePunishArgs(NarrativeMenuOptionArgs args) { }
        private static void MilitiaCodePunishOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了自由民团的推旗人-Node2-先惩恶");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_militia_code"] = "punish";
        }

        private static void GetMilitiaCodeSurviveArgs(NarrativeMenuOptionArgs args) { }
        private static void MilitiaCodeSurviveOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了自由民团的推旗人-Node2-先活命");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_militia_code"] = "survive";
        }

        private static NarrativeMenu CreateFreeMilitiaLeaderNode3Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "free_militia_leader_node3",
                "free_militia_leader_node2",
                "free_militia_leader_node4",
                new TextObject("民团武装是什么"),
                new TextObject("部族编年史中记录，你的民团武装是什么配置？是长矛阵，是家用弩，还是斧与木盾？你的选择不仅决定了你最初的战斗力，也影响了你在战场上的表现"),
                characters,
                GetFreeMilitiaLeaderNode3CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "militia_army_spear",
                new TextObject("长矛阵"),
                new TextObject("你的民团以长矛阵为主，这种配置依靠密集的阵型来防御和攻击。你的长矛民兵能够在近距离形成强大的防御力，但你在远程火力上有所不足。额外兵力：长矛民兵×16"),
                GetMilitiaArmySpearArgs,
                (m) => true,
                (m) => MilitiaArmySpearOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "militia_army_crossbow",
                new TextObject("家用弩"),
                new TextObject("你的民团以家用弩为主，这种配置依靠远程火力来攻击敌人。你的弩民兵能够在远距离威胁敌人，但你在近战中较弱。额外兵力：弩民兵×12，长矛×6"),
                GetMilitiaArmyCrossbowArgs,
                (m) => true,
                (m) => MilitiaArmyCrossbowOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "militia_army_axe",
                new TextObject("斧与木盾"),
                new TextObject("你的民团以斧与木盾为主，这种配置依靠近战肉搏来战斗。你的斧民兵能够在近战中造成巨大伤害，但你在防御力上有所不足。额外兵力：斧民兵×14，盾×6（偏肉搏）"),
                GetMilitiaArmyAxeArgs,
                (m) => true,
                (m) => MilitiaArmyAxeOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetFreeMilitiaLeaderNode3CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetMilitiaArmySpearArgs(NarrativeMenuOptionArgs args) { }
        private static void MilitiaArmySpearOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了自由民团的推旗人-Node3-长矛阵");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_militia_army"] = "spear";
        }

        private static void GetMilitiaArmyCrossbowArgs(NarrativeMenuOptionArgs args) { }
        private static void MilitiaArmyCrossbowOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了自由民团的推旗人-Node3-家用弩");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_militia_army"] = "crossbow";
        }

        private static void GetMilitiaArmyAxeArgs(NarrativeMenuOptionArgs args) { }
        private static void MilitiaArmyAxeOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了自由民团的推旗人-Node3-斧与木盾");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_militia_army"] = "axe";
        }

        private static NarrativeMenu CreateFreeMilitiaLeaderNode4Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "free_militia_leader_node4",
                "free_militia_leader_node3",
                null,
                new TextObject("你要不要成为真正的骑士"),
                new TextObject("家族记忆中最重要的一页：作为自由民团的推旗人，你要不要成为真正的骑士？是求封骑向上爬，是保持自由民不当狗，还是投奔某大氏族当门客？你的选择不仅决定了你未来的道路，也影响了你与各方的关系"),
                characters,
                GetFreeMilitiaLeaderNode4CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "militia_goal_knighthood",
                new TextObject("求封骑（向上爬）"),
                new TextObject("你的目标是成为真正的骑士，向上爬。你相信只有获得贵族的认可，才能真正保护人民。虽然这条路充满了挑战，但它也给了你一个明确的目标。标记：请封骑士（未来任务链：立功→授封）"),
                GetMilitiaGoalKnighthoodArgs,
                (m) => true,
                (m) => MilitiaGoalKnighthoodOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "militia_goal_free",
                new TextObject("保持自由民（不当狗）"),
                new TextObject("你的目标是保持自由民的身份，不当贵族的狗。你相信只有保持独立，才能真正代表人民的利益。虽然这条路充满了风险，但它也给了你更多的自由。标记：自由民团，村庄关系+10"),
                GetMilitiaGoalFreeArgs,
                (m) => true,
                (m) => MilitiaGoalFreeOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "militia_goal_client",
                new TextObject("投奔某大氏族（当门客）"),
                new TextObject("你的目标是投奔某个大氏族，成为他们的门客。你相信只有依附于强大的势力，才能获得更多的资源和支持。虽然这条路会让你失去部分独立，但它也给了你更多的机会。关系：某领主+10，标记：贵族门客"),
                GetMilitiaGoalClientArgs,
                (m) => true,
                (m) => MilitiaGoalClientOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetFreeMilitiaLeaderNode4CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetMilitiaGoalKnighthoodArgs(NarrativeMenuOptionArgs args) { }
        private static void MilitiaGoalKnighthoodOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了自由民团的推旗人-Node4-求封骑（向上爬）");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_militia_goal"] = "knighthood";
            OriginSystemHelper.OriginSelectionDone = true;
        }

        private static void GetMilitiaGoalFreeArgs(NarrativeMenuOptionArgs args) { }
        private static void MilitiaGoalFreeOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了自由民团的推旗人-Node4-保持自由民（不当狗）");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_militia_goal"] = "free";
            OriginSystemHelper.OriginSelectionDone = true;
        }

        private static void GetMilitiaGoalClientArgs(NarrativeMenuOptionArgs args) { }
        private static void MilitiaGoalClientOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了自由民团的推旗人-Node4-投奔某大氏族（当门客）");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_militia_goal"] = "client";
            OriginSystemHelper.OriginSelectionDone = true;
        }

        #endregion
    }
}
