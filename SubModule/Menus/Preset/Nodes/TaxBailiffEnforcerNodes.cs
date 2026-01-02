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
        #region 王室税吏的护卫节点菜单

        private static NarrativeMenu CreateTaxBailiffEnforcerNode1Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "tax_bailiff_enforcer_node1",
                "preset_origin_selection",
                "tax_bailiff_enforcer_node2",
                new TextObject("你如何收税"),
                new TextObject("家族编年史中，记录着你成为王室税吏护卫的那一天。你见过饥荒与暴动，你知道\'秩序是怎么来的\'。你如何收税？是严苛一分不少，是协商用话术拿到更多，还是腐败截留一部分？"),
                characters,
                GetTaxBailiffEnforcerNode1CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "tax_collect_harsh",
                new TextObject("严苛：一分不少"),
                new TextObject("你选择严苛地收税，一分不少。虽然这让你获得了更多的资金，但也让你与村庄产生了隔阂。你认为只有严格执行法律，才能维护秩序。金币+4000，村庄关系-15，标记：严厉收税者"),
                GetTaxCollectHarshArgs,
                (m) => true,
                (m) => TaxCollectHarshOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "tax_collect_negotiate",
                new TextObject("协商：用话术拿到更多"),
                new TextObject("你选择用话术协商，拿到更多的税。你熟悉如何在不激怒农民的情况下收取税费，这让你的工作更加顺利。魅力+25，金币+2000，标记：银舌"),
                GetTaxCollectNegotiateArgs,
                (m) => true,
                (m) => TaxCollectNegotiateOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "tax_collect_corrupt",
                new TextObject("腐败：你截留一部分"),
                new TextObject("你选择截留一部分税收，为自己谋取私利。虽然这让你获得了巨大的财富，但也让你面临着被查的风险。你认为只有利用自己的权力，才能在这个混乱的世界中生存。金币+8000，标记：脏手（未来可能被查）"),
                GetTaxCollectCorruptArgs,
                (m) => true,
                (m) => TaxCollectCorruptOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetTaxBailiffEnforcerNode1CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetTaxCollectHarshArgs(NarrativeMenuOptionArgs args) { }
        private static void TaxCollectHarshOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了王室税吏的护卫-Node1-严苛：一分不少");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_tax_collect"] = "harsh";
        }

        private static void GetTaxCollectNegotiateArgs(NarrativeMenuOptionArgs args) { }
        private static void TaxCollectNegotiateOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了王室税吏的护卫-Node1-协商：用话术拿到更多");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_tax_collect"] = "negotiate";
        }

        private static void GetTaxCollectCorruptArgs(NarrativeMenuOptionArgs args) { }
        private static void TaxCollectCorruptOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了王室税吏的护卫-Node1-腐败：你截留一部分");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_tax_collect"] = "corrupt";
        }

        private static NarrativeMenu CreateTaxBailiffEnforcerNode2Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "tax_bailiff_enforcer_node2",
                "tax_bailiff_enforcer_node1",
                "tax_bailiff_enforcer_node3",
                new TextObject("你见过最黑的一次是什么"),
                new TextObject("族人后来一直在说：在收税的过程中，你见过最黑暗的一次是什么？是饥荒，是暴动，还是权贵掩盖罪行？这段经历不仅塑造了你的性格，也影响了你的技能"),
                characters,
                GetTaxBailiffEnforcerNode2CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "tax_dark_famine",
                new TextObject("饥荒"),
                new TextObject("你见过饥荒，你见过人们因为饥饿而绝望。虽然你仍然要收税，但你也学会了如何管理资源，如何同情那些真正需要帮助的人。管理+20，村庄关系+5（你同情）"),
                GetTaxDarkFamineArgs,
                (m) => true,
                (m) => TaxDarkFamineOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "tax_dark_riot",
                new TextObject("暴动"),
                new TextObject("你见过暴动，你见过人们因为愤怒而反抗。虽然你不得不镇压他们，但你也学会了如何在混乱中保持冷静，如何在战斗中生存。战术+15，单手+10，长柄+10（你镇压过）"),
                GetTaxDarkRiotArgs,
                (m) => true,
                (m) => TaxDarkRiotOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "tax_dark_noble_crime",
                new TextObject("权贵掩盖罪行"),
                new TextObject("你见过权贵掩盖罪行，你见过他们如何利用权力来逃避责任。虽然你无法公开对抗他们，但你也学会了如何用话术和智慧来应对这些复杂的情况。魅力+15，标记：知道太多"),
                GetTaxDarkNobleCrimeArgs,
                (m) => true,
                (m) => TaxDarkNobleCrimeOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetTaxBailiffEnforcerNode2CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetTaxDarkFamineArgs(NarrativeMenuOptionArgs args) { }
        private static void TaxDarkFamineOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了王室税吏的护卫-Node2-饥荒");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_tax_dark"] = "famine";
        }

        private static void GetTaxDarkRiotArgs(NarrativeMenuOptionArgs args) { }
        private static void TaxDarkRiotOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了王室税吏的护卫-Node2-暴动");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_tax_dark"] = "riot";
        }

        private static void GetTaxDarkNobleCrimeArgs(NarrativeMenuOptionArgs args) { }
        private static void TaxDarkNobleCrimeOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了王室税吏的护卫-Node2-权贵掩盖罪行");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_tax_dark"] = "noble_crime";
        }

        private static NarrativeMenu CreateTaxBailiffEnforcerNode3Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "tax_bailiff_enforcer_node3",
                "tax_bailiff_enforcer_node2",
                "tax_bailiff_enforcer_node4",
                new TextObject("你的护卫队是什么"),
                new TextObject("部族编年史中记录，你的护卫队是什么配置？是盾卫，是弩卫，还是骑巡？你的选择不仅决定了你最初的战斗力，也影响了你在收税过程中的表现"),
                characters,
                GetTaxBailiffEnforcerNode3CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "tax_guard_shield",
                new TextObject("盾卫"),
                new TextObject("你的护卫队以盾卫为主，这种配置依靠坚固的防御来保护你和税吏。你的盾步兵能够在混乱中保护重要人物，但你在攻击力上有所不足。额外兵力：盾步×14，短矛×6"),
                GetTaxGuardShieldArgs,
                (m) => true,
                (m) => TaxGuardShieldOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "tax_guard_crossbow",
                new TextObject("弩卫"),
                new TextObject("你的护卫队以弩卫为主，这种配置依靠远程火力来威慑和攻击。你的弩手能够在远距离威胁敌人，但你在近战中较弱。额外兵力：弩手×10，盾步×6"),
                GetTaxGuardCrossbowArgs,
                (m) => true,
                (m) => TaxGuardCrossbowOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "tax_guard_cavalry",
                new TextObject("骑巡"),
                new TextObject("你的护卫队以骑巡为主，这种配置依靠快速机动来巡逻和追捕。你的轻骑兵能够在广阔的地区快速移动，但你在防御力上有所不足。额外兵力：轻骑×8，盾步×6"),
                GetTaxGuardCavalryArgs,
                (m) => true,
                (m) => TaxGuardCavalryOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetTaxBailiffEnforcerNode3CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetTaxGuardShieldArgs(NarrativeMenuOptionArgs args) { }
        private static void TaxGuardShieldOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了王室税吏的护卫-Node3-盾卫");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_tax_guard"] = "shield";
        }

        private static void GetTaxGuardCrossbowArgs(NarrativeMenuOptionArgs args) { }
        private static void TaxGuardCrossbowOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了王室税吏的护卫-Node3-弩卫");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_tax_guard"] = "crossbow";
        }

        private static void GetTaxGuardCavalryArgs(NarrativeMenuOptionArgs args) { }
        private static void TaxGuardCavalryOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了王室税吏的护卫-Node3-骑巡");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_tax_guard"] = "cavalry";
        }

        private static NarrativeMenu CreateTaxBailiffEnforcerNode4Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "tax_bailiff_enforcer_node4",
                "tax_bailiff_enforcer_node3",
                null,
                new TextObject("你要不要洗白"),
                new TextObject("家族记忆中最重要的一页：在收税的过程中，你要不要洗白？是向国王递交账册清算，是把账册埋掉继续黑，还是暗中救济走灰白路线？你的选择不仅决定了你未来的道路，也影响了你与各方的关系"),
                characters,
                GetTaxBailiffEnforcerNode4CharacterArgs
            );

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "tax_cleanse_account",
                new TextObject("向国王递交账册（清算）"),
                new TextObject("你选择向国王递交账册，进行清算。虽然这让你失去了部分赃款，但也让你与王室建立了良好的关系。你相信只有诚实地面对过去，才能获得真正的自由。关系：王室+15，金币-3000（你交出赃款）"),
                GetTaxCleanseAccountArgs,
                (m) => true,
                (m) => TaxCleanseAccountOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "tax_cleanse_bury",
                new TextObject("把账册埋掉（继续黑）"),
                new TextObject("你选择把账册埋掉，继续走黑道路线。虽然这让你保留了资金，但也让你永远无法洗白。你相信只有保持现状，才能在这个混乱的世界中生存。金币+2000，标记：隐藏账册"),
                GetTaxCleanseBuryArgs,
                (m) => true,
                (m) => TaxCleanseBuryOnSelect(m),
                null
            ));

            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "tax_cleanse_mercy",
                new TextObject("暗中救济（灰白）"),
                new TextObject("你选择暗中救济，走灰白路线。你用自己的资金来帮助那些真正需要帮助的人，虽然这让你损失了财富，但也让你赢得了村庄的尊重。村庄关系+15，金币-2000，标记：安静仁慈"),
                GetTaxCleanseMercyArgs,
                (m) => true,
                (m) => TaxCleanseMercyOnSelect(m),
                null
            ));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetTaxBailiffEnforcerNode4CharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetTaxCleanseAccountArgs(NarrativeMenuOptionArgs args) { }
        private static void TaxCleanseAccountOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了王室税吏的护卫-Node4-向国王递交账册（清算）");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_tax_cleanse"] = "account";
            OriginSystemHelper.OriginSelectionDone = true;
        }

        private static void GetTaxCleanseBuryArgs(NarrativeMenuOptionArgs args) { }
        private static void TaxCleanseBuryOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了王室税吏的护卫-Node4-把账册埋掉（继续黑）");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_tax_cleanse"] = "bury";
            OriginSystemHelper.OriginSelectionDone = true;
        }

        private static void GetTaxCleanseMercyArgs(NarrativeMenuOptionArgs args) { }
        private static void TaxCleanseMercyOnSelect(CharacterCreationManager manager)
        {
            OriginLog.Info("用户选择了王室税吏的护卫-Node4-暗中救济（灰白）");
            OriginSystemHelper.SelectedPresetOriginNodes["vla_node_tax_cleanse"] = "mercy";
            OriginSystemHelper.OriginSelectionDone = true;
        }

        #endregion
    }
}
