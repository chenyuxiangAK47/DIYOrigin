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
        #region 城镇弩匠行会的执旗人节点菜单

        private static NarrativeMenu CreateCrossbowGuildNode1Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "crossbow_guild_node1",
                "preset_origin_selection",
                "crossbow_guild_node2",
                new TextObject("行会给你的身份是什么"),
                new TextObject("家族编年史中，记录着你加入弩匠行会的那一天。行会给了你什么样的身份？是负责军需承包的合同王，是负责收债的行会执法，还是专注于改良弩具的工程脑？这个身份不仅决定了你在行会中的地位，也塑造了你的技能与性格"),
                characters,
                GetCrossbowGuildNode1CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "guild_role_contractor",
                new TextObject("军需承包人（合同王）"),
                new TextObject("族人后来一直在说：你成为了行会的军需承包人，负责与军队签订合同，组织生产。你熟悉贸易与管理的每一个环节，知道如何在战争中找到商机。虽然你不如骑士那样受人尊敬，但你的合同和产能影响着战争的走向。贸易+30，管理+30，金币+3000，标记：战争承包商"),
                GetGuildRoleContractorArgs,
                (m) => true,
                (m) => GuildRoleContractorOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "guild_role_enforcer",
                new TextObject("行会执法（收债人）"),
                new TextObject("部族编年史记载，你成为了行会的执法者，负责收债和维护行会秩序。你熟悉如何用话术说服债务人，也知道如何在必要时使用武力。虽然你的手段让一些人不悦，但你维护了行会的利益。魅力+15，单手+20，与商人关系+10，与贫民关系-10，标记：行会执法者"),
                GetGuildRoleEnforcerArgs,
                (m) => true,
                (m) => GuildRoleEnforcerOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "guild_role_innovator",
                new TextObject("弩匠改良者（工程脑）"),
                new TextObject("家族记忆中最骄傲的一页：你成为了行会的弩匠改良者，专注于改良弩具的设计和工艺。你对工程和锻造有着深厚的理解，你的改良让行会的弩具更加精良。虽然你不是战士，但你的智慧影响着战争的武器。锻造+30，工程+30，标记：试制弩原型"),
                GetGuildRoleInnovatorArgs,
                (m) => true,
                (m) => GuildRoleInnovatorOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetCrossbowGuildNode1CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetGuildRoleContractorArgs(NarrativeMenuOptionArgs args) { }
        private static void GuildRoleContractorOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了城镇弩匠行会的执旗人-Node1-军需承包人");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_guild_role"] = "contractor";
        }

        private static void GetGuildRoleEnforcerArgs(NarrativeMenuOptionArgs args) { }
        private static void GuildRoleEnforcerOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了城镇弩匠行会的执旗人-Node1-行会执法");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_guild_role"] = "enforcer";
        }

        private static void GetGuildRoleInnovatorArgs(NarrativeMenuOptionArgs args) { }
        private static void GuildRoleInnovatorOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了城镇弩匠行会的执旗人-Node1-弩匠改良者");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_guild_role"] = "innovator";
        }

        private static NarrativeMenu CreateCrossbowGuildNode2Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "crossbow_guild_node2",
                "crossbow_guild_node1",
                "crossbow_guild_node3",
                new TextObject("你更重视什么"),
                new TextObject("族人后来一直在说：在行会中，你更重视什么？是利润第一，把合同当成生意，是名声第一，维护行会的荣耀，还是忠诚供军，确保前线战士的装备？你的选择不仅决定了你的价值观，也影响了行会与你之间的关系"),
                characters,
                GetCrossbowGuildNode2CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "guild_value_profit",
                new TextObject("利润第一"),
                new TextObject("你选择利润第一，把合同当成纯粹的生意。你不在乎骑士的空话，只在乎金币和合同。这种态度让你积累了大量财富，但也让你与骑士阶层产生了隔阂。金币+4000，标记：反骑士情绪"),
                GetGuildValueProfitArgs,
                (m) => true,
                (m) => GuildValueProfitOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "guild_value_reputation",
                new TextObject("名声第一（行会荣耀）"),
                new TextObject("你选择名声第一，维护行会的荣耀和声誉。你认为行会的名声比利润更重要，你愿意为了行会的荣耀而牺牲一些利益。这种态度让你赢得了行会成员的尊重，也让商人们对你有更好的印象。声望+1，与商人关系+15，标记：行会骄傲"),
                GetGuildValueReputationArgs,
                (m) => true,
                (m) => GuildValueReputationOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "guild_value_loyalty",
                new TextObject("忠诚供军（别坑前线）"),
                new TextObject("你选择忠诚供军，确保前线战士的装备质量。你认为行会的职责是为战争提供可靠的武器，而不是为了利润而偷工减料。这种态度让你赢得了瓦兰迪亚军官和领主的尊重。与瓦兰迪亚军官/领主关系+10，标记：前线尊重"),
                GetGuildValueLoyaltyArgs,
                (m) => true,
                (m) => GuildValueLoyaltyOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetCrossbowGuildNode2CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetGuildValueProfitArgs(NarrativeMenuOptionArgs args) { }
        private static void GuildValueProfitOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了城镇弩匠行会的执旗人-Node2-利润第一");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_guild_value"] = "profit";
        }

        private static void GetGuildValueReputationArgs(NarrativeMenuOptionArgs args) { }
        private static void GuildValueReputationOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了城镇弩匠行会的执旗人-Node2-名声第一");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_guild_value"] = "reputation";
        }

        private static void GetGuildValueLoyaltyArgs(NarrativeMenuOptionArgs args) { }
        private static void GuildValueLoyaltyOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了城镇弩匠行会的执旗人-Node2-忠诚供军");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_guild_value"] = "loyalty";
        }

        private static NarrativeMenu CreateCrossbowGuildNode3Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "crossbow_guild_node3",
                "crossbow_guild_node2",
                "crossbow_guild_node4",
                new TextObject("开局资源包是什么"),
                new TextObject("部族编年史中记录，行会给了你什么样的开局资源包？是充足的弹药补给，是丰厚的现金流，还是广泛的人脉网络？你的选择不仅决定了你最初的资源，也影响了你在战争中的表现"),
                characters,
                GetCrossbowGuildNode3CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "guild_resource_ammunition",
                new TextObject("弹药充足"),
                new TextObject("你选择了充足的弹药补给，这些弩矢和螺栓能够让你在战斗中发挥强大的火力。虽然你失去了部分现金流，但你在战场上的表现更加出色。额外兵力：弩手×8，标记：弹药补给充足"),
                GetGuildResourceAmmunitionArgs,
                (m) => true,
                (m) => GuildResourceAmmunitionOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "guild_resource_cash",
                new TextObject("现金流"),
                new TextObject("你选择了丰厚的现金流，这些金币能够让你在贸易和投资中有更多的选择。虽然你没有额外的兵力，但你的财富让你能够在战争中灵活应对。金币+7000，额外兵力：护卫×6"),
                GetGuildResourceCashArgs,
                (m) => true,
                (m) => GuildResourceCashOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "guild_resource_network",
                new TextObject("人脉"),
                new TextObject("你选择了广泛的人脉网络，这些关系能够让你在城镇和行会中获得更多的支持。虽然你没有额外的兵力和财富，但你的关系网络让你能够在战争中获得更多的信息和支持。与1~2个城镇行会/商人关系+10，标记：行会网络"),
                GetGuildResourceNetworkArgs,
                (m) => true,
                (m) => GuildResourceNetworkOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetCrossbowGuildNode3CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetGuildResourceAmmunitionArgs(NarrativeMenuOptionArgs args) { }
        private static void GuildResourceAmmunitionOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了城镇弩匠行会的执旗人-Node3-弹药充足");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_guild_resource"] = "ammunition";
        }

        private static void GetGuildResourceCashArgs(NarrativeMenuOptionArgs args) { }
        private static void GuildResourceCashOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了城镇弩匠行会的执旗人-Node3-现金流");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_guild_resource"] = "cash";
        }

        private static void GetGuildResourceNetworkArgs(NarrativeMenuOptionArgs args) { }
        private static void GuildResourceNetworkOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了城镇弩匠行会的执旗人-Node3-人脉");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_guild_resource"] = "network";
        }

        private static NarrativeMenu CreateCrossbowGuildNode4Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "crossbow_guild_node4",
                "crossbow_guild_node3",
                null,
                new TextObject("你对骑士阶层的态度"),
                new TextObject("家族记忆中最重要的一页：你对骑士阶层持有什么态度？是尊重他们但不跪拜，是嘲笑他们的骑士道认为合同才是法律，还是两头吃把骑士和行会都当成客户？你的态度不仅决定了你与骑士阶层的关系，也影响了你在瓦兰迪亚社会中的地位"),
                characters,
                GetCrossbowGuildNode4CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "guild_knight_attitude_respect",
                new TextObject("尊骑（但不跪）"),
                new TextObject("你尊重骑士，但你不愿意向他们跪拜。你认为行会与骑士是平等的，只是选择了不同的道路。这种态度让你与骑士阶层保持了良好的关系，但你仍然保持着自己的独立性。标记：尊重骑士"),
                GetGuildKnightAttitudeRespectArgs,
                (m) => true,
                (m) => GuildKnightAttitudeRespectOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "guild_knight_attitude_mock",
                new TextObject("嘲笑骑士道（合同才是法律）"),
                new TextObject("你嘲笑骑士道，认为合同和法律才是真正的秩序。你认为骑士的空话和荣誉都不如金币和合同实际。这种态度让你与骑士阶层产生了隔阂，但你也因此更加专注于行会的利益。标记：嘲笑骑士"),
                GetGuildKnightAttitudeMockArgs,
                (m) => true,
                (m) => GuildKnightAttitudeMockOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "guild_knight_attitude_opportunist",
                new TextObject("两头吃（你最像战争资本）"),
                new TextObject("你选择两头吃，把骑士和行会都当成客户。你不关心骑士道或行会荣耀，只关心如何从战争中获利。这种态度让你成为了真正的战争资本，你利用骑士和行会的矛盾来获取利益。标记：机会主义者"),
                GetGuildKnightAttitudeOpportunistArgs,
                (m) => true,
                (m) => GuildKnightAttitudeOpportunistOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetCrossbowGuildNode4CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetGuildKnightAttitudeRespectArgs(NarrativeMenuOptionArgs args) { }
        private static void GuildKnightAttitudeRespectOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了城镇弩匠行会的执旗人-Node4-尊骑（但不跪）");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_guild_knight_attitude"] = "respect";
            OriginSystemHelper.OriginSelectionDone = true;
        }

        private static void GetGuildKnightAttitudeMockArgs(NarrativeMenuOptionArgs args) { }
        private static void GuildKnightAttitudeMockOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了城镇弩匠行会的执旗人-Node4-嘲笑骑士道");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_guild_knight_attitude"] = "mock";
            OriginSystemHelper.OriginSelectionDone = true;
        }

        private static void GetGuildKnightAttitudeOpportunistArgs(NarrativeMenuOptionArgs args) { }
        private static void GuildKnightAttitudeOpportunistOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了城镇弩匠行会的执旗人-Node4-两头吃");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_guild_knight_attitude"] = "opportunist";
            OriginSystemHelper.OriginSelectionDone = true;
        }

        #endregion
    }
}
