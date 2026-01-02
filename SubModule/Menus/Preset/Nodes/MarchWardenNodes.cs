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
        #region 边境行省的守境骑长节点菜单

        private static NarrativeMenu CreateMarchWardenNode1Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "march_warden_node1",
                "preset_origin_selection",
                "march_warden_node2",
                new TextObject("你守的是什么线"),
                new TextObject("家族编年史中，记录着你成为边境守境骑长的那一天。你负责守卫的是什么边境线？是防范伏击的林线，是收取渡税的河口通道，还是保护补给的城堡大道？这段经历不仅决定了你的技能与经验，也塑造了你的骑士道与品格"),
                characters,
                GetMarchWardenNode1CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "warden_line_forest",
                new TextObject("林线（防伏击）"),
                new TextObject("族人后来一直在说：你负责守卫林线，防范敌军的伏击。你熟悉每一处可以设伏的地点，知道如何在密林中侦察和战斗。这种经历让你成为了一个出色的侦察兵和弓箭手，你的骑士道也因此得到了提升。侦察+35，弓弩+10，骑士道+5"),
                GetWardenLineForestArgs,
                (m) => true,
                (m) => WardenLineForestOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "warden_line_river",
                new TextObject("河口渡税（守通道）"),
                new TextObject("部族编年史记载，你负责守卫河口通道，收取渡税。你熟悉贸易和管理的每一个环节，知道如何在不激怒商人的情况下收取税费。这种经历让你成为了一个现实的管理者，你的骑士道也因此有所下降。管理+25，贸易+15，骑士道-5"),
                GetWardenLineRiverArgs,
                (m) => true,
                (m) => WardenLineRiverOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "warden_line_castle",
                new TextObject("城堡大道（护补给）"),
                new TextObject("家族记忆中最骄傲的一页：你负责守卫城堡大道，保护补给药草和军需。你熟悉如何组织补给线，也知道如何在战斗中保护重要物资。这种经历让你成为了一个出色的领导者和战术家，你的骑士道也因此得到了显著提升。领导+25，战术+15，骑士道+10"),
                GetWardenLineCastleArgs,
                (m) => true,
                (m) => WardenLineCastleOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetMarchWardenNode1CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetWardenLineForestArgs(NarrativeMenuOptionArgs args) { }
        private static void WardenLineForestOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了边境行省的守境骑长-Node1-林线（防伏击）");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_warden_line"] = "forest";
        }

        private static void GetWardenLineRiverArgs(NarrativeMenuOptionArgs args) { }
        private static void WardenLineRiverOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了边境行省的守境骑长-Node1-河口渡税（守通道）");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_warden_line"] = "river";
        }

        private static void GetWardenLineCastleArgs(NarrativeMenuOptionArgs args) { }
        private static void WardenLineCastleOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了边境行省的守境骑长-Node1-城堡大道（护补给）");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_warden_line"] = "castle";
        }

        private static NarrativeMenu CreateMarchWardenNode2Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "march_warden_node2",
                "march_warden_node1",
                "march_warden_node3",
                new TextObject("你如何执法"),
                new TextObject("族人后来一直在说：在边境守卫中，你如何执法？是使用铁血军法，用严厉的惩罚来维护秩序，是公平裁断，用魅力和正义来赢得民心，还是实用主义，用金钱和协商来解决问题？你的选择不仅决定了你与平民的关系，也影响了你的骑士道"),
                characters,
                GetMarchWardenNode2CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "warden_law_iron",
                new TextObject("铁血军法"),
                new TextObject("你选择使用铁血军法，用严厉的惩罚来维护边境秩序。虽然这种手段让你的部下更加服从，但也让你与农民产生了隔阂。你的骑士道也因此下降，因为你更注重纪律而不是仁慈。士气+3，与农民关系-10，骑士道-10"),
                GetWardenLawIronArgs,
                (m) => true,
                (m) => WardenLawIronOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "warden_law_fair",
                new TextObject("公平裁断"),
                new TextObject("你选择公平裁断，用魅力和正义来赢得民心。你相信正义和仁慈比武力更有效，你的公平让你赢得了村庄的尊重。你的骑士道也因此得到了显著提升，因为你展现了真正的骑士品格。魅力+20，与村庄关系+10，骑士道+15"),
                GetWardenLawFairArgs,
                (m) => true,
                (m) => WardenLawFairOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "warden_law_pragmatic",
                new TextObject("实用主义（能用钱解决就别流血）"),
                new TextObject("你选择实用主义，用金钱和协商来解决问题。你认为能用钱解决的问题就不应该流血，这种现实的态度让你积累了财富，但也让你的骑士道有所下降。金币+2000，骑士道-5，标记：实用主义守卫者"),
                GetWardenLawPragmaticArgs,
                (m) => true,
                (m) => WardenLawPragmaticOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetMarchWardenNode2CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetWardenLawIronArgs(NarrativeMenuOptionArgs args) { }
        private static void WardenLawIronOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了边境行省的守境骑长-Node2-铁血军法");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_warden_law"] = "iron";
        }

        private static void GetWardenLawFairArgs(NarrativeMenuOptionArgs args) { }
        private static void WardenLawFairOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了边境行省的守境骑长-Node2-公平裁断");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_warden_law"] = "fair";
        }

        private static void GetWardenLawPragmaticArgs(NarrativeMenuOptionArgs args) { }
        private static void WardenLawPragmaticOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了边境行省的守境骑长-Node2-实用主义");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_warden_law"] = "pragmatic";
        }

        private static NarrativeMenu CreateMarchWardenNode3Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "march_warden_node3",
                "march_warden_node2",
                "march_warden_node4",
                new TextObject("你的边境部队是什么"),
                new TextObject("部族编年史中记录，你的边境部队是什么配置？是反骑长枪阵，专门对付骑兵冲击，是盾墙硬兵，依靠坚固的防御，还是侦骑巡逻，依靠快速机动？你的选择不仅决定了你最初的战斗力，也影响了你在边境守卫中的战术风格"),
                characters,
                GetMarchWardenNode3CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "warden_troops_anti_cav",
                new TextObject("反骑长枪阵"),
                new TextObject("你的边境部队以反骑长枪阵为主，这种配置专门对付骑兵冲击。你的长枪兵能够有效阻挡敌人的骑兵，但你在机动性上有所不足。这种配置让你在防御战中非常强大，但也限制了你的进攻能力。额外兵力：长枪×12，盾步×6"),
                GetWardenTroopsAntiCavArgs,
                (m) => true,
                (m) => WardenTroopsAntiCavOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "warden_troops_shield_wall",
                new TextObject("盾墙硬兵"),
                new TextObject("你的边境部队以盾墙硬兵为主，这种配置依靠坚固的防御来抵抗敌人的进攻。你的盾步兵能够形成坚固的盾墙，但你在攻击力上有所不足。这种配置让你在阵地战中非常强大，但也需要更多的训练和装备。额外兵力：盾步×14，短矛×4"),
                GetWardenTroopsShieldWallArgs,
                (m) => true,
                (m) => WardenTroopsShieldWallOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "warden_troops_scout",
                new TextObject("侦骑巡逻"),
                new TextObject("你的边境部队以侦骑巡逻为主，这种配置依靠快速机动来侦察和巡逻。你的轻骑兵和散兵能够快速移动，但你在防御力上有所不足。这种配置让你在侦察和快速反应中非常出色，但也需要更多的马匹和补给。额外兵力：轻骑×10，散兵×6"),
                GetWardenTroopsScoutArgs,
                (m) => true,
                (m) => WardenTroopsScoutOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetMarchWardenNode3CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetWardenTroopsAntiCavArgs(NarrativeMenuOptionArgs args) { }
        private static void WardenTroopsAntiCavOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了边境行省的守境骑长-Node3-反骑长枪阵");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_warden_troops"] = "anti_cav";
        }

        private static void GetWardenTroopsShieldWallArgs(NarrativeMenuOptionArgs args) { }
        private static void WardenTroopsShieldWallOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了边境行省的守境骑长-Node3-盾墙硬兵");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_warden_troops"] = "shield_wall";
        }

        private static void GetWardenTroopsScoutArgs(NarrativeMenuOptionArgs args) { }
        private static void WardenTroopsScoutOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了边境行省的守境骑长-Node3-侦骑巡逻");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_warden_troops"] = "scout";
        }

        private static NarrativeMenu CreateMarchWardenNode4Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "march_warden_node4",
                "march_warden_node3",
                null,
                new TextObject("你背负的仇/债是什么"),
                new TextObject("家族记忆中最沉重的一页：在边境守卫中，你背负着什么样的仇恨或债务？是与某个氏族的血仇，是必须完成的王命清单，还是护送难民的人道责任？你的选择不仅决定了你与某些势力的关系，也影响了你的骑士道和未来的目标"),
                characters,
                GetMarchWardenNode4CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "warden_burden_blood_feud",
                new TextObject("血仇氏族"),
                new TextObject("你与某个氏族有着血仇，这个仇恨让你时刻警惕着他们的报复。虽然这个仇恨让你与敌人保持着敌对关系，但也让你的骑士道得到了提升，因为你愿意为了正义而战斗。与指定敌对氏族关系-30，骑士道+10，标记：血仇"),
                GetWardenBurdenBloodFeudArgs,
                (m) => true,
                (m) => WardenBurdenBloodFeudOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "warden_burden_royal_duty",
                new TextObject("王命清单（必须完成的边境任务）"),
                new TextObject("你背负着王命清单，必须完成一系列边境任务。这些任务包括巡逻、清匪、护送等，你必须完成它们才能获得国王的认可。虽然这些任务让你忙碌，但也让你的骑士道得到了提升，因为你履行了自己的职责。骑士道+5，标记：王命任务"),
                GetWardenBurdenRoyalDutyArgs,
                (m) => true,
                (m) => WardenBurdenRoyalDutyOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "warden_burden_refugees",
                new TextObject("护送难民（你见过太多）"),
                new TextObject("你背负着护送难民的人道责任，你见过太多的战争和苦难。你愿意用自己的金钱来帮助这些难民，虽然这让你损失了财富，但也让你赢得了村庄的尊重，你的骑士道也因此得到了显著提升。与村庄关系+15，骑士道+20，金币-1000"),
                GetWardenBurdenRefugeesArgs,
                (m) => true,
                (m) => WardenBurdenRefugeesOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetMarchWardenNode4CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetWardenBurdenBloodFeudArgs(NarrativeMenuOptionArgs args) { }
        private static void WardenBurdenBloodFeudOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了边境行省的守境骑长-Node4-血仇氏族");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_warden_burden"] = "blood_feud";
            OriginSystemHelper.OriginSelectionDone = true;
        }

        private static void GetWardenBurdenRoyalDutyArgs(NarrativeMenuOptionArgs args) { }
        private static void WardenBurdenRoyalDutyOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了边境行省的守境骑长-Node4-王命清单");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_warden_burden"] = "royal_duty";
            OriginSystemHelper.OriginSelectionDone = true;
        }

        private static void GetWardenBurdenRefugeesArgs(NarrativeMenuOptionArgs args) { }
        private static void WardenBurdenRefugeesOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了边境行省的守境骑长-Node4-护送难民");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_warden_burden"] = "refugees";
            OriginSystemHelper.OriginSelectionDone = true;
        }

        #endregion
    }
}
