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
        #region 远征的骑士节点菜单

        private static NarrativeMenu CreateExpeditionKnightNode1Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "expedition_knight_node1",
                "preset_origin_selection",
                "expedition_knight_node2",
                new TextObject("你为何没落"),
                new TextObject("家族编年史中，记录着你家族没落的那一天。你出身瓦兰迪亚没落骑士家族。家名仍被承认，但封地与家兵已散。家族只剩你与哥哥两人。你为何没落？是被抹掉的败仗，是封地被\'合法\'吞并，是债务压垮纹章，还是政治清算？"),
                characters,
                GetExpeditionKnightNode1CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "expedition_fall_erased_defeat",
                new TextObject("被抹掉的败仗"),
                new TextObject("你父辈替人背锅，在一场关键战役中战败，但这场败仗被官方记录抹去，仿佛从未发生。你的家族因此失去了地位和封地，但你也从这场失败中学会了战术和领导。那些受益于你家族败落的氏族对你保持着敌意。战术+30，领导+20，与某贵族氏族关系-30，骑士道+10（你仍认\'责任\'），标记：被抹掉的败仗"),
                GetExpeditionFallErasedDefeatArgs,
                (m) => true,
                (m) => ExpeditionFallErasedDefeatOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "expedition_fall_annexed",
                new TextObject("封地被\'合法\'吞并"),
                new TextObject("文书与法庭把你吃干抹净，你的家族封地被其他氏族以\'合法\'的方式吞并。你学会了如何在政治游戏中周旋，如何在失去一切后重新开始。魅力+20，管理+20，金币+1500（卖掉最后的地契），骑士道+5（更现实），标记：封地被吞并"),
                GetExpeditionFallAnnexedArgs,
                (m) => true,
                (m) => ExpeditionFallAnnexedOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "expedition_fall_debt",
                new TextObject("债务压垮纹章"),
                new TextObject("你们被债主围猎，家族因为债务而破产，纹章虽然还在，但已经失去了实际意义。你学会了如何在困境中经营，如何在商人和行会之间周旋。贸易+20，管理+20，开局金币+2500（借来的），骑士道-10（你开始觉得\'荣誉不值钱\'），标记：债务压垮"),
                GetExpeditionFallDebtArgs,
                (m) => true,
                (m) => ExpeditionFallDebtOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "expedition_fall_political",
                new TextObject("政治清算"),
                new TextObject("你们是站错队的遗孤，你的家族在政治清算中被边缘化。你学会了如何在危险的环境中生存，如何在被监视的情况下保持警惕。侦察+20，战术+20，与王室派系关系-30，骑士道+15（你更执拗于传统），标记：政治清算"),
                GetExpeditionFallPoliticalArgs,
                (m) => true,
                (m) => ExpeditionFallPoliticalOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetExpeditionKnightNode1CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetExpeditionFallErasedDefeatArgs(NarrativeMenuOptionArgs args) { }
        private static void ExpeditionFallErasedDefeatOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了远征的骑士-Node1-被抹掉的败仗");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_expedition_knight_fall"] = "erased_defeat";
        }

        private static void GetExpeditionFallAnnexedArgs(NarrativeMenuOptionArgs args) { }
        private static void ExpeditionFallAnnexedOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了远征的骑士-Node1-封地被合法吞并");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_expedition_knight_fall"] = "annexed";
        }

        private static void GetExpeditionFallDebtArgs(NarrativeMenuOptionArgs args) { }
        private static void ExpeditionFallDebtOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了远征的骑士-Node1-债务压垮纹章");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_expedition_knight_fall"] = "debt";
        }

        private static void GetExpeditionFallPoliticalArgs(NarrativeMenuOptionArgs args) { }
        private static void ExpeditionFallPoliticalOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了远征的骑士-Node1-政治清算");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_expedition_knight_fall"] = "political";
        }

        private static NarrativeMenu CreateExpeditionKnightNode2Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "expedition_knight_node2",
                "expedition_knight_node1",
                "expedition_knight_node3",
                new TextObject("你立下什么誓言"),
                new TextObject("族人后来一直在说：你立誓远征，不完成誓言，不以家名回归。你带着对家族荣耀的执着和对未来的希望，立下了什么誓言？"),
                characters,
                GetExpeditionKnightNode2CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "expedition_oath_sea_raiders",
                new TextObject("杀1000海寇（复仇/护民）"),
                new TextObject("你立誓要杀死1000名海寇，为家族洗刷耻辱。这个誓言让你获得了弩术和领导力的提升，也让你获得了更多的弩手支持。任务：累计击杀海寇1000（可分阶段200/500/1000）。完成奖励：声望+30，骑士道+20，哥哥好感+。标记：誓言目标-海寇"),
                GetExpeditionOathSeaRaidersArgs,
                (m) => true,
                (m) => ExpeditionOathSeaRaidersOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "expedition_oath_quyaz",
                new TextObject("远征古亚兹，占据城池立旗（野心/封地）"),
                new TextObject("你立誓要远征古亚兹，夺取城市并立下家族旗帜。这个誓言让你获得了工程学和战术的提升，也让你获得了更多的资源和精锐部队。任务：占领目标城市（或参与攻城并成为城主/家族持有）。完成奖励：声望+60，骑士道+10（偏现实），开新剧情。标记：誓言目标-古亚兹"),
                GetExpeditionOathQuyazArgs,
                (m) => true,
                (m) => ExpeditionOathQuyazOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "expedition_oath_battania",
                new TextObject("斩某巴丹尼亚氏族一人（血仇）"),
                new TextObject("你立誓要斩杀一名巴丹尼亚家族成员，为家族复仇。这个誓言让你获得了单手武器和侦察技能的提升，也让你获得了更多的侍从支持。任务：击杀/俘虏并处决指定氏族成员。完成奖励：声望+40，骑士道-15（很\'黑\'但仍是贵族复仇）。标记：誓言目标-巴丹尼亚"),
                GetExpeditionOathBattaniaArgs,
                (m) => true,
                (m) => ExpeditionOathBattaniaOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "expedition_oath_relic",
                new TextObject("寻回失旗与印玺（纯骑士道）"),
                new TextObject("你立誓要寻回家族失落的战旗和印玺，这些是家族荣耀的象征。这个誓言让你获得了魅力和劫掠技能的提升，也让你获得了更多的侦察兵支持。任务：找到传家信物（可做成黑市/竞技/劫匪营地链）。完成奖励：骑士道+35，士气+，哥哥提供一件家传装备。标记：誓言目标-遗物"),
                GetExpeditionOathRelicArgs,
                (m) => true,
                (m) => ExpeditionOathRelicOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetExpeditionKnightNode2CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetExpeditionOathSeaRaidersArgs(NarrativeMenuOptionArgs args) { }
        private static void ExpeditionOathSeaRaidersOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了远征的骑士-Node2-杀1000海寇");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_expedition_knight_oath"] = "kill_1000_sea_raiders";
        }

        private static void GetExpeditionOathQuyazArgs(NarrativeMenuOptionArgs args) { }
        private static void ExpeditionOathQuyazOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了远征的骑士-Node2-远征古亚兹，夺城立旗");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_expedition_knight_oath"] = "conquer_quyaz";
        }

        private static void GetExpeditionOathBattaniaArgs(NarrativeMenuOptionArgs args) { }
        private static void ExpeditionOathBattaniaOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了远征的骑士-Node2-斩巴丹尼亚家族一人");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_expedition_knight_oath"] = "kill_battania_noble";
        }

        private static void GetExpeditionOathRelicArgs(NarrativeMenuOptionArgs args) { }
        private static void ExpeditionOathRelicOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了远征的骑士-Node2-寻回失旗与印玺");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_expedition_knight_oath"] = "recover_relic";
        }

        private static NarrativeMenu CreateExpeditionKnightNode3Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "expedition_knight_node3",
                "expedition_knight_node2",
                "expedition_knight_node4",
                new TextObject("你的骑士道是什么"),
                new TextObject("部族编年史中记录，你信奉什么样的骑士道？是仁慈，是勇武，还是谨慎？你的选择不仅决定了你的价值观，也影响了你的骑士道数值"),
                characters,
                GetExpeditionKnightNode3CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "expedition_chivalry_mercy",
                new TextObject("仁慈之誓（圣杯型）"),
                new TextObject("你信奉仁慈的骑士道，认为真正的骑士应该保护弱者，而不是伤害无辜。这种信念让你在村民和市民中获得了良好的声誉。骑士道+50；开局钱更少（捐出家财）。标记：骑士道-仁慈"),
                GetExpeditionChivalryMercyArgs,
                (m) => true,
                (m) => ExpeditionChivalryMercyOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "expedition_chivalry_valor",
                new TextObject("勇武之誓（战功型）"),
                new TextObject("你信奉勇武的骑士道，认为真正的骑士应该在战场上证明自己的价值。这种信念让你在长杆武器和骑术方面有了显著的提升。骑士道+20；开局多2~4精锐随骑。标记：骑士道-勇武"),
                GetExpeditionChivalryValorArgs,
                (m) => true,
                (m) => ExpeditionChivalryValorOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "expedition_chivalry_prudence",
                new TextObject("谨慎之誓（现实型）"),
                new TextObject("你信奉谨慎的骑士道，认为真正的骑士应该智勇双全，而不是盲目冲锋。这种信念让你在战术和侦察方面有了显著的提升。骑士道+5；开局金币+1500（你留了后手）。标记：骑士道-谨慎"),
                GetExpeditionChivalryPrudenceArgs,
                (m) => true,
                (m) => ExpeditionChivalryPrudenceOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "expedition_chivalry_cynical",
                new TextObject("犬儒之誓（玷污骑士道）"),
                new TextObject("你把誓言当作交易，认为骑士道只是空洞的仪式。你更关注实际的利益和生存，而不是虚无缥缈的荣誉。骑士道-25；开局金币+3000（你把誓言当交易）。标记：骑士道-犬儒"),
                GetExpeditionChivalryCynicalArgs,
                (m) => true,
                (m) => ExpeditionChivalryCynicalOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetExpeditionKnightNode3CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetExpeditionChivalryMercyArgs(NarrativeMenuOptionArgs args) { }
        private static void ExpeditionChivalryMercyOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了远征的骑士-Node3-仁慈");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_expedition_knight_chivalry"] = "mercy";
        }

        private static void GetExpeditionChivalryValorArgs(NarrativeMenuOptionArgs args) { }
        private static void ExpeditionChivalryValorOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了远征的骑士-Node3-勇武");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_expedition_knight_chivalry"] = "valor";
        }

        private static void GetExpeditionChivalryPrudenceArgs(NarrativeMenuOptionArgs args) { }
        private static void ExpeditionChivalryPrudenceOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了远征的骑士-Node3-谨慎之誓");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_expedition_knight_chivalry"] = "prudence";
        }

        private static void GetExpeditionChivalryCynicalArgs(NarrativeMenuOptionArgs args) { }
        private static void ExpeditionChivalryCynicalOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了远征的骑士-Node3-犬儒之誓");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_expedition_knight_chivalry"] = "cynical";
        }

        private static NarrativeMenu CreateExpeditionKnightNode4Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "expedition_knight_node4",
                "expedition_knight_node3",
                null,
                new TextObject("你与哥哥如何分工"),
                new TextObject("家族记忆中最重要的一页：家族只剩你与哥哥两人。你与哥哥如何分工？是哥哥掌军你主外交，是你掌军哥哥做见证人，还是共同决策？"),
                characters,
                GetExpeditionKnightNode4CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "expedition_brother_commander",
                new TextObject("哥哥掌军，你主外交"),
                new TextObject("你让哥哥负责军事指挥，自己专注于外交和政治。这种分工让你们各司其职，也让你在领导力方面有了提升。哥哥偏战术/统御，你偏魅力/管理；随从偏步弩。与哥哥关系+20，标记：兄弟分工-指挥官"),
                GetExpeditionBrotherCommanderArgs,
                (m) => true,
                (m) => ExpeditionBrotherCommanderOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "expedition_brother_witness",
                new TextObject("你掌军，哥哥做见证人"),
                new TextObject("你负责军事指挥，哥哥作为见证人记录你的功绩。这种分工让你在名望方面有了提升，也让你获得了更多的侍从支持。你拿骑兵核心，哥哥提供稳定后勤。与哥哥关系+20，标记：兄弟分工-见证人"),
                GetExpeditionBrotherWitnessArgs,
                (m) => true,
                (m) => ExpeditionBrotherWitnessOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "expedition_brother_joint",
                new TextObject("共同决策"),
                new TextObject("你和哥哥共同决策，互相支持。这种合作让你们在管理方面有了提升，也让你获得了更多的食物支持。队伍更均衡；但誓言任务推进更\'慢但稳\'（可加一点限制/奖励）。与哥哥关系+75，标记：兄弟分工-共同决策"),
                GetExpeditionBrotherJointArgs,
                (m) => true,
                (m) => ExpeditionBrotherJointOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetExpeditionKnightNode4CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetExpeditionBrotherCommanderArgs(NarrativeMenuOptionArgs args) { }
        private static void ExpeditionBrotherCommanderOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了远征的骑士-Node4-哥哥掌军，你主外交");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_expedition_knight_brother"] = "brother_commands";
            OriginSystemHelper.OriginSelectionDone = true;
        }

        private static void GetExpeditionBrotherWitnessArgs(NarrativeMenuOptionArgs args) { }
        private static void ExpeditionBrotherWitnessOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了远征的骑士-Node4-你掌军，哥哥做见证人");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_expedition_knight_brother"] = "brother_witness";
            OriginSystemHelper.OriginSelectionDone = true;
        }

        private static void GetExpeditionBrotherJointArgs(NarrativeMenuOptionArgs args) { }
        private static void ExpeditionBrotherJointOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了远征的骑士-Node4-共同决策");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_expedition_knight_brother"] = "brother_joint";
            OriginSystemHelper.OriginSelectionDone = true;
        }

        #endregion
    }
}
