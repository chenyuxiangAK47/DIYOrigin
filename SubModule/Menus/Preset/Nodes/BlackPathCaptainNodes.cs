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
        #region 黑旗匪首节点菜单

        private static NarrativeMenu CreateBlackPathCaptainNode1Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "black_path_captain_node1",
                "preset_origin_selection",
                "black_path_captain_node2",
                new TextObject("你是什么盗匪"),
                new TextObject("家族编年史中，记录着你成为黑旗匪首的那一天。你是什么样的盗匪？是劫富济贫的侠盗，是拉纳格的恶种，是为贵族复仇的复仇匪，还是为钱办事的雇凶匪？这个身份不仅决定了你的恶名和关系，也塑造了你在黑道中的地位"),
                characters,
                GetBlackPathCaptainNode1CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "black_path_type_robin_hood",
                new TextObject("侠盗（劫富济贫）"),
                new TextObject("你是一名侠盗，专门劫富济贫。你虽然走上了黑道，但你仍然保持着一些良知，你不会伤害无辜的平民。这种态度让你在村庄中获得了尊重，但也让你与富人和贵族产生了冲突。恶名+10，与村庄关系+5，标记：侠盗"),
                GetBlackPathTypeRobinHoodArgs,
                (m) => true,
                (m) => BlackPathTypeRobinHoodOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "black_path_type_brutal",
                new TextObject("恶种（拉纳格坏种）"),
                new TextObject("你是一个恶种，是拉纳格最坏的盗匪。你不在乎什么道德和正义，只在乎金币和权力。你为了利益可以做任何事情，这种态度让你积累了财富，但也让你声名狼藉。恶名+35，金币+4000，标记：残暴盗匪"),
                GetBlackPathTypeBrutalArgs,
                (m) => true,
                (m) => BlackPathTypeBrutalOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "black_path_type_vengeance",
                new TextObject("复仇匪（仇贵族）"),
                new TextObject("你是一个复仇匪，专门与贵族为敌。你曾经被贵族欺压，现在你选择用暴力来报复。你不在乎法律和道德，只在乎如何让贵族付出代价。这种态度让你与贵族产生了深仇大恨，但也让你在平民中获得了一些支持。恶名+25，与贵族关系-30，标记：贵族仇恨者"),
                GetBlackPathTypeVengeanceArgs,
                (m) => true,
                (m) => BlackPathTypeVengeanceOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "black_path_type_hireling",
                new TextObject("雇凶匪（为钱办事）"),
                new TextObject("你是一个雇凶匪，专门为钱办事。你不关心正义或复仇，只关心如何赚钱。你愿意接受任何任务，只要价格合适。这种态度让你在黑白两道都有联系，但也让你失去了任何道德底线。恶名+15，金币+3000，标记：雇凶者"),
                GetBlackPathTypeHirelingArgs,
                (m) => true,
                (m) => BlackPathTypeHirelingOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetBlackPathCaptainNode1CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetBlackPathTypeRobinHoodArgs(NarrativeMenuOptionArgs args) { }
        private static void BlackPathTypeRobinHoodOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了黑旗匪首-Node1-侠盗（劫富济贫）");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_black_path_type"] = "robin_hood";
        }

        private static void GetBlackPathTypeBrutalArgs(NarrativeMenuOptionArgs args) { }
        private static void BlackPathTypeBrutalOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了黑旗匪首-Node1-恶种（拉纳格坏种）");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_black_path_type"] = "brutal";
        }

        private static void GetBlackPathTypeVengeanceArgs(NarrativeMenuOptionArgs args) { }
        private static void BlackPathTypeVengeanceOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了黑旗匪首-Node1-复仇匪（仇贵族）");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_black_path_type"] = "vengeance";
        }

        private static void GetBlackPathTypeHirelingArgs(NarrativeMenuOptionArgs args) { }
        private static void BlackPathTypeHirelingOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了黑旗匪首-Node1-雇凶匪（为钱办事）");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_black_path_type"] = "hireling";
        }

        private static NarrativeMenu CreateBlackPathCaptainNode2Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "black_path_captain_node2",
                "black_path_captain_node1",
                "black_path_captain_node3",
                new TextObject("你带领着什么队伍"),
                new TextObject("族人后来一直在说：你带领着什么样的队伍？是擅长林地伏击的伏击团，是擅长大道劫掠的劫掠团，是擅长城镇活动的城镇鼠群，还是由叛逃士兵组成的杂兵团？你的选择不仅决定了你最初的战斗力，也决定了你将永远无法加入瓦兰迪亚"),
                characters,
                GetBlackPathCaptainNode2CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "black_path_team_forest",
                new TextObject("林地伏击团"),
                new TextObject("你带领着林地伏击团，擅长在森林中设伏和战斗。你的散兵和弓匪能够在密林中隐藏和射击，给敌人造成巨大的困扰。虽然你在开阔地带的战斗力较弱，但你在林地战中非常出色。侦察+25，额外兵力：散兵/弓匪×12，标记：永远无法加入瓦兰迪亚"),
                GetBlackPathTeamForestArgs,
                (m) => true,
                (m) => BlackPathTeamForestOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "black_path_team_highway",
                new TextObject("大道劫掠团"),
                new TextObject("你带领着大道劫掠团，擅长在商路上劫掠。你的骑匪和步匪能够在道路上快速移动和攻击，给商队造成巨大的威胁。虽然你在防御战中较弱，但你在劫掠和贸易中非常出色。贸易+25，劫掠+25，额外兵力：骑匪×8，步匪×10，标记：永远无法加入瓦兰迪亚"),
                GetBlackPathTeamHighwayArgs,
                (m) => true,
                (m) => BlackPathTeamHighwayOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "black_path_team_urban",
                new TextObject("城镇鼠群"),
                new TextObject("你带领着城镇鼠群，擅长在城镇中活动和渗透。你的成员能够在城镇中隐藏和活动，收集情报和进行暗杀。虽然你在正面战斗中较弱，但你在城镇活动和情报收集中非常出色。偷窃+20，魅力+20，金币+3000，标记：地下网络，永远无法加入瓦兰迪亚"),
                GetBlackPathTeamUrbanArgs,
                (m) => true,
                (m) => BlackPathTeamUrbanOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "black_path_team_deserter",
                new TextObject("叛逃杂兵团"),
                new TextObject("你带领着叛逃杂兵团，由叛逃的士兵组成。你的成员虽然装备精良，但纪律性差，士气低落。虽然你在正面战斗中有一定的战斗力，但你的队伍很难管理。战术+15，额外兵力：重装匪×6，士气-1，标记：永远无法加入瓦兰迪亚"),
                GetBlackPathTeamDeserterArgs,
                (m) => true,
                (m) => BlackPathTeamDeserterOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetBlackPathCaptainNode2CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetBlackPathTeamForestArgs(NarrativeMenuOptionArgs args) { }
        private static void BlackPathTeamForestOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了黑旗匪首-Node2-林地伏击团");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_black_path_team"] = "forest";
            // 设置永远无法加入瓦兰迪亚的标记
            OriginSystemHelper.IsVlandiaOutlaw = true;
            OriginSystemHelper.NoVlandiaJoin = true;
        }

        private static void GetBlackPathTeamHighwayArgs(NarrativeMenuOptionArgs args) { }
        private static void BlackPathTeamHighwayOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了黑旗匪首-Node2-大道劫掠团");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_black_path_team"] = "highway";
            OriginSystemHelper.IsVlandiaOutlaw = true;
            OriginSystemHelper.NoVlandiaJoin = true;
        }

        private static void GetBlackPathTeamUrbanArgs(NarrativeMenuOptionArgs args) { }
        private static void BlackPathTeamUrbanOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了黑旗匪首-Node2-城镇鼠群");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_black_path_team"] = "urban";
            OriginSystemHelper.IsVlandiaOutlaw = true;
            OriginSystemHelper.NoVlandiaJoin = true;
        }

        private static void GetBlackPathTeamDeserterArgs(NarrativeMenuOptionArgs args) { }
        private static void BlackPathTeamDeserterOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了黑旗匪首-Node2-叛逃杂兵团");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_black_path_team"] = "deserter";
            OriginSystemHelper.IsVlandiaOutlaw = true;
            OriginSystemHelper.NoVlandiaJoin = true;
        }

        private static NarrativeMenu CreateBlackPathCaptainNode3Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "black_path_captain_node3",
                "black_path_captain_node2",
                "black_path_captain_node4",
                new TextObject("你的旗号是什么"),
                new TextObject("部族编年史中记录，你打出了什么样的旗号？是黑旗不归王法，完全与法律为敌，是只反贵族不伤农民，专注于与贵族对抗，还是随时换边，但不加入瓦兰迪亚？你的选择不仅决定了你与瓦兰迪亚的关系，也影响了你在黑道中的名声"),
                characters,
                GetBlackPathCaptainNode3CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "black_path_banner_lawless",
                new TextObject("黑旗不归王法"),
                new TextObject("你打出了黑旗不归王法的旗号，完全与法律和王权为敌。你不在乎什么法律和秩序，只在乎自己的自由和利益。这种态度让你与瓦兰迪亚产生了更深的敌对关系，但也让你在黑道中获得了更高的声望。恶名+15，与瓦兰迪亚关系-10"),
                GetBlackPathBannerLawlessArgs,
                (m) => true,
                (m) => BlackPathBannerLawlessOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "black_path_banner_anti_noble",
                new TextObject("只反贵族不伤农民"),
                new TextObject("你打出了只反贵族不伤农民的旗号，专注于与贵族对抗，但不伤害无辜的平民。你虽然走上了黑道，但你仍然保持着一些良知，你只对贵族和富人有仇恨。这种态度让你在村庄中获得了支持，但也让你与贵族产生了深仇大恨。恶名+5，与村庄关系+10"),
                GetBlackPathBannerAntiNobleArgs,
                (m) => true,
                (m) => BlackPathBannerAntiNobleOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "black_path_banner_flexible",
                new TextObject("随时换边（但不入瓦兰迪亚）"),
                new TextObject("你打出了随时换边的旗号，愿意与任何势力合作，但绝不加入瓦兰迪亚。你不在乎什么忠诚和立场，只在乎如何获得利益。这种态度让你在黑道中获得了灵活性，但也让你失去了任何坚定的盟友。恶名+10，标记：灵活盗匪"),
                GetBlackPathBannerFlexibleArgs,
                (m) => true,
                (m) => BlackPathBannerFlexibleOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetBlackPathCaptainNode3CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetBlackPathBannerLawlessArgs(NarrativeMenuOptionArgs args) { }
        private static void BlackPathBannerLawlessOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了黑旗匪首-Node3-黑旗不归王法");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_black_path_banner"] = "lawless";
        }

        private static void GetBlackPathBannerAntiNobleArgs(NarrativeMenuOptionArgs args) { }
        private static void BlackPathBannerAntiNobleOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了黑旗匪首-Node3-只反贵族不伤农民");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_black_path_banner"] = "anti_noble";
        }

        private static void GetBlackPathBannerFlexibleArgs(NarrativeMenuOptionArgs args) { }
        private static void BlackPathBannerFlexibleOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了黑旗匪首-Node3-随时换边");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_black_path_banner"] = "flexible";
        }

        private static NarrativeMenu CreateBlackPathCaptainNode4Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "black_path_captain_node4",
                "black_path_captain_node3",
                null,  // 默认结束
                new TextObject("你的大目标"),
                new TextObject("家族记忆中最重要的一页：你作为黑旗匪首，有什么大目标？是建立匪窝城邦，开创农奴共和国的雏形，是做最大的黑市，控制所有的非法贸易，还是复仇某个贵族氏族，直到他们彻底覆灭？你的选择不仅决定了你未来的发展方向，也预示了你将在卡拉迪亚历史上留下什么样的印记"),
                characters,
                GetBlackPathCaptainNode4CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "black_path_goal_republic",
                new TextObject("建匪窝城邦（农奴共和国雏形）"),
                new TextObject("你的目标是建立匪窝城邦，开创农奴共和国的雏形。你相信人民应该有自由和权利，你愿意为了这个理想而战斗。虽然这个目标充满了挑战，但它也给了你和你的追随者一个崇高的理想。标记：农奴共和国种子"),
                GetBlackPathGoalRepublicArgs,
                (m) => true,
                (m) => BlackPathGoalRepublicOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "black_path_goal_black_market",
                new TextObject("做最大黑市"),
                new TextObject("你的目标是做最大的黑市，控制所有的非法贸易。你相信金币和权力比任何理想都重要，你愿意为了这个目标而不择手段。虽然这个目标充满了危险，但它也给了你和你的追随者巨大的财富和影响力。标记：黑市之王"),
                GetBlackPathGoalBlackMarketArgs,
                (m) => true,
                (m) => BlackPathGoalBlackMarketOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "black_path_goal_vendetta",
                new TextObject("复仇某贵族氏族到倾覆"),
                new TextObject("你的目标是复仇某个贵族氏族，直到他们彻底覆灭。你不在乎什么理想和财富，只在乎如何让那些欺压你的人付出代价。虽然这个目标充满了仇恨，但它也给了你和你的追随者一个明确的方向。标记：复仇战役"),
                GetBlackPathGoalVendettaArgs,
                (m) => true,
                (m) => BlackPathGoalVendettaOnSelect(m),
                null
            ));

            // 注意：堕落无赖骑士现在是独立的preset（vlandia_degraded_rogue_knight），不再作为黑旗匪首的分支

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetBlackPathCaptainNode4CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetBlackPathGoalRepublicArgs(NarrativeMenuOptionArgs args) { }
        private static void BlackPathGoalRepublicOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了黑旗匪首-Node4-建匪窝城邦");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_black_path_goal"] = "republic";
            OriginSystemHelper.OriginSelectionDone = true;
        }

        private static void GetBlackPathGoalBlackMarketArgs(NarrativeMenuOptionArgs args) { }
        private static void BlackPathGoalBlackMarketOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了黑旗匪首-Node4-做最大黑市");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_black_path_goal"] = "black_market";
            OriginSystemHelper.OriginSelectionDone = true;
        }

        private static void GetBlackPathGoalVendettaArgs(NarrativeMenuOptionArgs args) { }
        private static void BlackPathGoalVendettaOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了黑旗匪首-Node4-复仇某贵族氏族");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_black_path_goal"] = "vendetta";
            OriginSystemHelper.OriginSelectionDone = true;
        }

        // 注意：GetBlackPathGoalDegradedKnightArgs 和 BlackPathGoalDegradedKnightOnSelect 已移除
        // 无赖骑士现在是独立的preset（vlandia_degraded_rogue_knight）

        #endregion
    }
}
