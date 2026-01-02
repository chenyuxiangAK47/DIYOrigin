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
        #region 破产的旗主继承人节点菜单

        private static NarrativeMenu CreateBankruptBanneretNode1Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "bankrupt_banneret_node1",
                "preset_origin_selection",
                "bankrupt_banneret_node2",
                new TextObject("你为何破产"),
                new TextObject("家族编年史中，记录着你家族破产的那一天。是什么让你失去了封地和财富？是为赎回亲族的倾家荡产，是押错商路导致的工坊亏空，还是战败后的赔款与罚金？这段经历不仅决定了你现在的财务状况，也塑造了你的技能与品格"),
                characters,
                GetBankruptBanneretNode1CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "bankrupt_reason_ransom",
                new TextObject("为赎回亲族倾家荡产"),
                new TextObject("家族记忆中最沉重的一页：你为了赎回被俘的亲族，不惜倾家荡产。你卖掉了封地，抵押了家产，甚至借下了高利贷。虽然你成功救回了亲人，但你的家族也因此破产。那些被你救回的亲族永远感激你，而你也因此背负了沉重的债务。金币-2000，与某贵族氏族关系+10，骑士道+15"),
                GetBankruptReasonRansomArgs,
                (m) => true,
                (m) => BankruptReasonRansomOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "bankrupt_reason_trade_loss",
                new TextObject("押错商路/工坊亏空"),
                new TextObject("部族编年史记载，你押错了商路，投资的工坊也遭遇了亏空。你本以为能够通过贸易重振家族，但市场的变化让你措手不及。虽然你失去了大部分财富，但你保留了账本与渠道，这些经验让你学会了如何在商场上生存。贸易+35，管理+20，金币+3000，骑士道-5"),
                GetBankruptReasonTradeLossArgs,
                (m) => true,
                (m) => BankruptReasonTradeLossOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "bankrupt_reason_defeat",
                new TextObject("战败赔款/罚金"),
                new TextObject("族人后来一直在说：你在一次战斗中战败，被迫支付了巨额赔款和罚金。你的家族因此破产，但那些跟随你战斗的残兵却更加忠诚。他们知道你没有逃避责任，而是承担了失败的后果。战术+25，领导+20，士气+2，骑士道+5"),
                GetBankruptReasonDefeatArgs,
                (m) => true,
                (m) => BankruptReasonDefeatOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetBankruptBanneretNode1CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetBankruptReasonRansomArgs(NarrativeMenuOptionArgs args) { }
        private static void BankruptReasonRansomOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了破产的旗主继承人-Node1-为赎回亲族倾家荡产");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_bankrupt_reason"] = "ransom";
        }

        private static void GetBankruptReasonTradeLossArgs(NarrativeMenuOptionArgs args) { }
        private static void BankruptReasonTradeLossOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了破产的旗主继承人-Node1-押错商路/工坊亏空");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_bankrupt_reason"] = "trade_loss";
        }

        private static void GetBankruptReasonDefeatArgs(NarrativeMenuOptionArgs args) { }
        private static void BankruptReasonDefeatOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了破产的旗主继承人-Node1-战败赔款/罚金");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_bankrupt_reason"] = "defeat";
        }

        private static NarrativeMenu CreateBankruptBanneretNode2Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "bankrupt_banneret_node2",
                "bankrupt_banneret_node1",
                "bankrupt_banneret_node3",
                new TextObject("你怎么\'守住体面\'"),
                new TextObject("家族记忆中最艰难的选择：在破产之后，你如何守住家族的体面？是卖掉祖传的盔甲换取现金，是死保盔甲但让家族陷入更深的困境，还是抵押纹章与税权进行合法借贷？你的选择不仅决定了你现在的财务状况，也影响了你的骑士道与家族荣誉"),
                characters,
                GetBankruptBanneretNode2CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "bankrupt_face_sell_armor",
                new TextObject("卖祖传甲换现金"),
                new TextObject("族人后来一直在说：你卖掉了祖传的盔甲，换取了急需的现金。虽然这让你暂时缓解了财务危机，但你也因此失去了家族的象征。那些曾经敬仰你家族的人开始质疑你的品格，而你的骑士道也因此受损。金币+9000，护甲降一档，骑士道-10"),
                GetBankruptFaceSellArmorArgs,
                (m) => true,
                (m) => BankruptFaceSellArmorOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "bankrupt_face_keep_armor",
                new TextObject("死保盔甲，钱紧"),
                new TextObject("家族编年史记载，你死保盔甲，宁愿让家族陷入更深的财务困境也不愿卖掉家族的象征。虽然这让你的财务状况更加困难，但你的部下却因此更加忠诚。他们知道你把家族的荣誉看得比金钱更重要。金币-1500，士气+3，骑士道+10"),
                GetBankruptFaceKeepArmorArgs,
                (m) => true,
                (m) => BankruptFaceKeepArmorOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "bankrupt_face_mortgage",
                new TextObject("抵押纹章与税权（合法借贷）"),
                new TextObject("部族记忆中最现实的一页：你选择了抵押纹章与税权，通过合法借贷来缓解财务危机。虽然这让你背负了债务，但你也保留了家族的象征。这是一种现实的选择，既不损害家族荣誉，也不让家族陷入绝境。金币+6000，标记：债务绑定，骑士道0"),
                GetBankruptFaceMortgageArgs,
                (m) => true,
                (m) => BankruptFaceMortgageOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetBankruptBanneretNode2CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetBankruptFaceSellArmorArgs(NarrativeMenuOptionArgs args) { }
        private static void BankruptFaceSellArmorOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了破产的旗主继承人-Node2-卖祖传甲换现金");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_bankrupt_face"] = "sell_armor";
        }

        private static void GetBankruptFaceKeepArmorArgs(NarrativeMenuOptionArgs args) { }
        private static void BankruptFaceKeepArmorOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了破产的旗主继承人-Node2-死保盔甲，钱紧");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_bankrupt_face"] = "keep_armor";
        }

        private static void GetBankruptFaceMortgageArgs(NarrativeMenuOptionArgs args) { }
        private static void BankruptFaceMortgageOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了破产的旗主继承人-Node2-抵押纹章与税权");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_bankrupt_face"] = "mortgage";
        }

        private static NarrativeMenu CreateBankruptBanneretNode3Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "bankrupt_banneret_node3",
                "bankrupt_banneret_node2",
                "bankrupt_banneret_node4",
                new TextObject("你的家兵剩下什么"),
                new TextObject("族人后来一直在说：在破产之后，你的家兵还剩下什么？是以弩手为主的守财配置，是以步兵为主的硬骨配置，还是混编的平衡配置？你的选择不仅决定了你最初的战斗力，也影响了你在困境中的生存方式"),
                characters,
                GetBankruptBanneretNode3CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "bankrupt_troops_crossbow",
                new TextObject("弩手为主（守财）"),
                new TextObject("你的家兵以弩手为主，这种配置更适合守财和防御。弩手虽然移动较慢，但在防御战中能够发挥强大的火力。这种配置让你能够在困境中更好地保护自己，但也限制了你的机动性。额外兵力：弩手×10，盾步×4，食物+15"),
                GetBankruptTroopsCrossbowArgs,
                (m) => true,
                (m) => BankruptTroopsCrossbowOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "bankrupt_troops_infantry",
                new TextObject("步兵硬骨（保命）"),
                new TextObject("你的家兵以步兵为主，这些硬骨头的战士是你家族最忠诚的成员。他们虽然装备简单，但战斗意志坚定，愿意为你而战。这种配置让你能够在困境中保持强大的近战能力，但也需要更多的训练和装备。额外兵力：盾步×12，长枪×6，士气+2"),
                GetBankruptTroopsInfantryArgs,
                (m) => true,
                (m) => BankruptTroopsInfantryOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "bankrupt_troops_mixed",
                new TextObject("混编（更稳）"),
                new TextObject("你的家兵是混编的，包括弩手、盾步和轻骑。这种配置更加平衡，能够在各种情况下应对。虽然不如单一兵种专业，但在困境中，这种平衡配置让你能够应对各种挑战。额外兵力：弩手×6，盾步×6，轻骑×3，马匹+1"),
                GetBankruptTroopsMixedArgs,
                (m) => true,
                (m) => BankruptTroopsMixedOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetBankruptBanneretNode3CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetBankruptTroopsCrossbowArgs(NarrativeMenuOptionArgs args) { }
        private static void BankruptTroopsCrossbowOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了破产的旗主继承人-Node3-弩手为主");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_bankrupt_troops"] = "crossbow";
        }

        private static void GetBankruptTroopsInfantryArgs(NarrativeMenuOptionArgs args) { }
        private static void BankruptTroopsInfantryOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了破产的旗主继承人-Node3-步兵硬骨");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_bankrupt_troops"] = "infantry";
        }

        private static void GetBankruptTroopsMixedArgs(NarrativeMenuOptionArgs args) { }
        private static void BankruptTroopsMixedOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了破产的旗主继承人-Node3-混编");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_bankrupt_troops"] = "mixed";
        }

        private static NarrativeMenu CreateBankruptBanneretNode4Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "bankrupt_banneret_node4",
                "bankrupt_banneret_node3",
                null,
                new TextObject("你要向谁借势"),
                new TextObject("家族记忆中最关键的选择：在破产之后，你要向谁借势？是向国王宣誓求翻盘，是投靠行会活下去，还是不站队先赚钱？你的选择不仅决定了你未来的政治立场，也影响了你的骑士道与家族命运"),
                characters,
                GetBankruptBanneretNode4CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "bankrupt_patron_king",
                new TextObject("向国王宣誓（求翻盘）"),
                new TextObject("族人后来一直在说：你选择向国王宣誓，希望通过效忠来获得翻盘的机会。你知道国王是权力的中心，如果能够得到他的认可，你的家族地位将会大大提升。虽然这需要你承担更多的责任，但也给了你重建家族的机会。与瓦兰迪亚王关系+20，标记：王室请愿，骑士道+10"),
                GetBankruptPatronKingArgs,
                (m) => true,
                (m) => BankruptPatronKingOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "bankrupt_patron_guild",
                new TextObject("投靠行会（活下去）"),
                new TextObject("部族编年史记载，你选择投靠行会，希望通过商人的支持来活下去。你知道行会在城镇中有着巨大的影响力，如果能够得到他们的支持，你就能在困境中站稳脚跟。虽然这让你失去了部分骑士的尊严，但也给了你生存的机会。与城镇商人关系+20，贸易+20，骑士道-5"),
                GetBankruptPatronGuildArgs,
                (m) => true,
                (m) => BankruptPatronGuildOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "bankrupt_patron_none",
                new TextObject("不站队，先赚钱（犬儒贵族）"),
                new TextObject("家族记忆中最现实的一页：你选择不站队，先通过赚钱来重建家族。你知道在政治斗争中，过早站队可能会带来危险。你希望通过低调行事来保护自己，等待合适的时机。虽然这让你失去了部分政治支持，但也给了你更多的自由。金币+2000，骑士道-15，标记：犬儒骑士"),
                GetBankruptPatronNoneArgs,
                (m) => true,
                (m) => BankruptPatronNoneOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetBankruptBanneretNode4CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetBankruptPatronKingArgs(NarrativeMenuOptionArgs args) { }
        private static void BankruptPatronKingOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了破产的旗主继承人-Node4-向国王宣誓");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_bankrupt_patron"] = "king";
            OriginSystemHelper.OriginSelectionDone = true;
        }

        private static void GetBankruptPatronGuildArgs(NarrativeMenuOptionArgs args) { }
        private static void BankruptPatronGuildOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了破产的旗主继承人-Node4-投靠行会");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_bankrupt_patron"] = "guild";
            OriginSystemHelper.OriginSelectionDone = true;
        }

        private static void GetBankruptPatronNoneArgs(NarrativeMenuOptionArgs args) { }
        private static void BankruptPatronNoneOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了破产的旗主继承人-Node4-不站队，先赚钱");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_bankrupt_patron"] = "none";
            OriginSystemHelper.OriginSelectionDone = true;
        }

        #endregion
    }
}
