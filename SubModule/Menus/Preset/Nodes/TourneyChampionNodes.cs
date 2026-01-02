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
        #region 比武场的落魄冠军节点菜单

        private static NarrativeMenu CreateTourneyChampionNode1Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "tourney_champion_node1",
                "preset_origin_selection",
                "tourney_champion_node2",
                new TextObject("你怎么跌下神坛"),
                new TextObject("家族编年史中，记录着你从竞技场神话跌下神坛的那一天。你是如何失去往日荣耀的？是假赛背锅，是重伤复出失败，还是当众羞辱权贵？这段经历不仅决定了你现在的处境，也塑造了你的性格与价值观"),
                characters,
                GetTourneyChampionNode1CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "tourney_fall_fake_match",
                new TextObject("假赛背锅"),
                new TextObject("家族记忆中最黑暗的一页：你因为假赛背锅而失去了荣耀。虽然你得到了封口费，但你也因此与权贵产生了矛盾。那些曾经敬仰你的人们开始质疑你的品格，而你的骑士道也因此受损。与某权贵关系-20，金币+3000（封口费），骑士道-15"),
                GetTourneyFallFakeMatchArgs,
                (m) => true,
                (m) => TourneyFallFakeMatchOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "tourney_fall_injury",
                new TextObject("重伤复出失败"),
                new TextObject("族人后来一直在说：你在一次重伤后尝试复出，但最终失败了。虽然你没有再次赢得冠军，但你在恢复过程中学会了医疗，也在体能训练中变得更加坚韧。这段经历让你学会了如何在逆境中坚持，你的骑士道也因此得到了提升。医疗+20，体能+10，骑士道+10"),
                GetTourneyFallInjuryArgs,
                (m) => true,
                (m) => TourneyFallInjuryOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "tourney_fall_humiliate",
                new TextObject("当众羞辱权贵"),
                new TextObject("家族编年史记载，你当众羞辱了权贵，因此失去了他们的支持。虽然这让你与权贵产生了深仇大恨，但也让你在平民中获得了更多的尊重。你的魅力也因此得到了提升，因为你敢于为正义发声。魅力+25，与权贵关系-30，骑士道+15"),
                GetTourneyFallHumiliateArgs,
                (m) => true,
                (m) => TourneyFallHumiliateOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetTourneyChampionNode1CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetTourneyFallFakeMatchArgs(NarrativeMenuOptionArgs args) { }
        private static void TourneyFallFakeMatchOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了比武场的落魄冠军-Node1-假赛背锅");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_tourney_fall"] = "fake_match";
        }

        private static void GetTourneyFallInjuryArgs(NarrativeMenuOptionArgs args) { }
        private static void TourneyFallInjuryOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了比武场的落魄冠军-Node1-重伤复出失败");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_tourney_fall"] = "injury";
        }

        private static void GetTourneyFallHumiliateArgs(NarrativeMenuOptionArgs args) { }
        private static void TourneyFallHumiliateOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了比武场的落魄冠军-Node1-当众羞辱权贵");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_tourney_fall"] = "humiliate";
        }

        private static NarrativeMenu CreateTourneyChampionNode2Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "tourney_champion_node2",
                "tourney_champion_node1",
                "tourney_champion_node3",
                new TextObject("你靠什么翻身"),
                new TextObject("族人后来一直在说：在跌下神坛之后，你选择如何翻身？是坚持纯骑士道拒绝黑幕，是不择手段赢回来，还是把冠军经验变成纪律？你的选择不仅决定了你未来的道路，也影响了你的骑士道"),
                characters,
                GetTourneyChampionNode2CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "tourney_recover_pure_chivalry",
                new TextObject("纯骑士道：拒绝黑幕"),
                new TextObject("你选择坚持纯骑士道，拒绝一切黑幕和赞助。虽然你失去了金钱上的支持，但你的骑士道也因此得到了显著提升。你相信真正的荣耀来自公平的竞争，而不是肮脏的交易。骑士道+30，金币-1000（你拒绝赞助）"),
                GetTourneyRecoverPureChivalryArgs,
                (m) => true,
                (m) => TourneyRecoverPureChivalryOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "tourney_recover_any_means",
                new TextObject("不择手段赢回来"),
                new TextObject("你选择不择手段赢回来，只要能够重新获得荣耀，你愿意做任何事情。虽然这让你积累了财富，但你的骑士道也因此受到了损害。你认为结果比过程更重要，即使这意味着要违背骑士的准则。骑士道-25，金币+4000，标记：不择手段"),
                GetTourneyRecoverAnyMeansArgs,
                (m) => true,
                (m) => TourneyRecoverAnyMeansOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "tourney_recover_discipline",
                new TextObject("把冠军经验变纪律"),
                new TextObject("你选择把冠军经验变成纪律，用你在竞技场中学到的技巧来训练部队。虽然你没有再次赢得冠军，但你成为了一个出色的领导者和战术家。你的骑士道也因此得到了提升，因为你把个人的荣耀转化为了团队的力量。领导+30，训练+15，战术+15，骑士道+10"),
                GetTourneyRecoverDisciplineArgs,
                (m) => true,
                (m) => TourneyRecoverDisciplineOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetTourneyChampionNode2CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetTourneyRecoverPureChivalryArgs(NarrativeMenuOptionArgs args) { }
        private static void TourneyRecoverPureChivalryOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了比武场的落魄冠军-Node2-纯骑士道：拒绝黑幕");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_tourney_recover"] = "pure_chivalry";
        }

        private static void GetTourneyRecoverAnyMeansArgs(NarrativeMenuOptionArgs args) { }
        private static void TourneyRecoverAnyMeansOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了比武场的落魄冠军-Node2-不择手段赢回来");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_tourney_recover"] = "any_means";
        }

        private static void GetTourneyRecoverDisciplineArgs(NarrativeMenuOptionArgs args) { }
        private static void TourneyRecoverDisciplineOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了比武场的落魄冠军-Node2-把冠军经验变纪律");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_tourney_recover"] = "discipline";
        }

        private static NarrativeMenu CreateTourneyChampionNode3Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "tourney_champion_node3",
                "tourney_champion_node2",
                "tourney_champion_node4",
                new TextObject("开局随队风格是什么"),
                new TextObject("部族编年史中记录，你开局时的随队风格是什么？是枪骑小队，是表演型随从（民心），还是硬汉步兵？你的选择不仅决定了你最初的战斗力，也影响了你在战场上的表现"),
                characters,
                GetTourneyChampionNode3CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "tourney_team_lance_cavalry",
                new TextObject("枪骑小队"),
                new TextObject("你的随队以枪骑小队为主，这种配置依靠快速机动和冲击力来战斗。你的轻骑兵和骑士侍从能够在战场上快速移动和攻击，给敌人造成巨大的威胁。虽然你在防御战中较弱，但你在进攻战中非常出色。额外兵力：轻骑×8，骑士侍从×4"),
                GetTourneyTeamLanceCavalryArgs,
                (m) => true,
                (m) => TourneyTeamLanceCavalryOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "tourney_team_performance",
                new TextObject("表演型随从（民心）"),
                new TextObject("你的随队以表演型随从为主，这些跟随你的旧粉丝虽然战斗力一般，但士气很高。他们相信你能够再次获得荣耀，这种信念让他们在战场上表现更加出色。虽然你的部队战斗力不强，但你的魅力让你能够获得更多的支持。额外兵力：民兵×12，士气+4，魅力+10"),
                GetTourneyTeamPerformanceArgs,
                (m) => true,
                (m) => TourneyTeamPerformanceOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "tourney_team_infantry",
                new TextObject("硬汉步兵"),
                new TextObject("你的随队以硬汉步兵为主，这种配置依靠坚固的防御和强大的攻击力来战斗。你的盾步兵和长枪兵能够在战场上形成坚固的阵线，给敌人造成巨大的威胁。虽然你在机动性上较弱，但你在阵地战中非常强大。额外兵力：盾步×10，长枪×6"),
                GetTourneyTeamInfantryArgs,
                (m) => true,
                (m) => TourneyTeamInfantryOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetTourneyChampionNode3CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetTourneyTeamLanceCavalryArgs(NarrativeMenuOptionArgs args) { }
        private static void TourneyTeamLanceCavalryOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了比武场的落魄冠军-Node3-枪骑小队");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_tourney_team"] = "lance_cavalry";
        }

        private static void GetTourneyTeamPerformanceArgs(NarrativeMenuOptionArgs args) { }
        private static void TourneyTeamPerformanceOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了比武场的落魄冠军-Node3-表演型随从");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_tourney_team"] = "performance";
        }

        private static void GetTourneyTeamInfantryArgs(NarrativeMenuOptionArgs args) { }
        private static void TourneyTeamInfantryOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了比武场的落魄冠军-Node3-硬汉步兵");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_tourney_team"] = "infantry";
        }

        private static NarrativeMenu CreateTourneyChampionNode4Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "tourney_champion_node4",
                "tourney_champion_node3",
                null,
                new TextObject("你欠谁的债"),
                new TextObject("家族记忆中最重要的一页：在跌下神坛之后，你欠下了什么样的债务？是赌徒债主，是贵族债主，还是你欠的不是金钱而是名声？你的选择不仅决定了你现在的财务状况，也影响了你的骑士道和未来的目标"),
                characters,
                GetTourneyChampionNode4CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "tourney_debt_gambler",
                new TextObject("赌徒债主"),
                new TextObject("你欠下了赌徒债主的债务，这些高利贷让你背负了沉重的负担。虽然你暂时获得了资金，但你也面临着未来追债的风险。你的骑士道也因此受到了损害，因为你不得不与那些不光彩的人打交道。金币+4000，骑士道-10，标记：债主追债"),
                GetTourneyDebtGamblerArgs,
                (m) => true,
                (m) => TourneyDebtGamblerOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "tourney_debt_noble",
                new TextObject("贵族债主"),
                new TextObject("你欠下了贵族债主的债务，虽然这让你与贵族保持了关系，但也让你受到了他们的控制。你必须在他们的要求下行事，否则就会失去他们的支持。虽然这没有直接影响你的骑士道，但你失去了部分的自由。与某贵族关系+10，骑士道0，标记：贵族束缚"),
                GetTourneyDebtNobleArgs,
                (m) => true,
                (m) => TourneyDebtNobleOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "tourney_debt_reputation",
                new TextObject("你不欠债，你欠的是名声"),
                new TextObject("你不欠任何金钱债务，但你欠的是名声。你必须在战场上证明自己仍然配得上冠军的荣耀，否则你就会永远失去人们的尊重。虽然这没有给你带来金钱，但你的声望和骑士道也因此得到了提升。声望+10，骑士道+10，金币0"),
                GetTourneyDebtReputationArgs,
                (m) => true,
                (m) => TourneyDebtReputationOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetTourneyChampionNode4CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetTourneyDebtGamblerArgs(NarrativeMenuOptionArgs args) { }
        private static void TourneyDebtGamblerOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了比武场的落魄冠军-Node4-赌徒债主");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_tourney_debt"] = "gambler";
            OriginSystemHelper.OriginSelectionDone = true;
        }

        private static void GetTourneyDebtNobleArgs(NarrativeMenuOptionArgs args) { }
        private static void TourneyDebtNobleOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了比武场的落魄冠军-Node4-贵族债主");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_tourney_debt"] = "noble";
            OriginSystemHelper.OriginSelectionDone = true;
        }

        private static void GetTourneyDebtReputationArgs(NarrativeMenuOptionArgs args) { }
        private static void TourneyDebtReputationOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了比武场的落魄冠军-Node4-你不欠债，你欠的是名声");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_tourney_debt"] = "reputation";
            OriginSystemHelper.OriginSelectionDone = true;
        }

        #endregion
    }
}
