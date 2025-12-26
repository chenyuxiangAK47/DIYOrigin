using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using OriginSystemMod;

namespace OriginSystemMod
{
    public static partial class OriginSystemPatches
    {
        #region 战奴逃亡节点菜单

        private static NarrativeMenu CreateSlaveEscapeNode1Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu("slave_escape_node1", "preset_origin_selection", "slave_escape_node2", new TextObject("你被奴役前是什么人"), new TextObject("在成为奴隶之前，你曾经是谁？家族的记忆中，你有着怎样的过去？这些过往不仅塑造了你的技能与性格，也决定了你在奴役中如何保持自我，如何在绝望中寻找希望"), characters, GetSlaveEscapeNode1CharacterArgs);

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("slave_before_border_warrior", new TextObject("边境战士"), new TextObject("家族记忆说，你曾是库塞特边境的战士，在草原与沙漠的交界处守卫着部族的疆域。你熟悉长矛与盾牌，在无数次与阿塞莱劫掠者的战斗中磨练了体魄与意志。然而，在一次深入沙漠的追击中，你中了埋伏，力战不敌后被俘，从此沦为奴隶。那些战斗的技艺与不屈的意志，成为你在奴役中唯一的精神支柱。体力+4，单手+3，士气+2"), GetSlaveBeforeBorderWarriorArgs, (m) => true, (m) => SlaveBeforeBorderWarriorOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("slave_before_steppe_bandit", new TextObject("草原响马"), new TextObject("族人后来回忆，你曾是草原上的响马，在库塞特与帝国之间的商路上劫掠为生。你熟悉每一处可以设伏的山谷，知道如何快速来去如风。你曾从商队中夺走不少财富，也曾在追捕中逃脱。但最终，在一次分赃不均的内讧中，你被同伴出卖，卖给了阿塞莱的奴隶贩子。那些在草原上生存的狡诈与侦察技巧，在奴役中反而成了你观察机会的眼睛。狡诈+5，侦察+3，金钱+100"), GetSlaveBeforeSteppeBanditArgs, (m) => true, (m) => SlaveBeforeSteppeBanditOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("slave_before_horse_thief", new TextObject("偷马贼"), new TextObject("部族中流传着你的故事：你曾是草原上最灵巧的偷马贼，能在夜色中悄无声息地接近马群，挑选最好的战马带走。你熟悉每一匹马的习性，知道如何安抚它们，让它们跟随你离开。你曾从库塞特那颜的马厩中偷走名驹，也曾从商队的驮马中挑选良种。但一次失手，你被守卫发现，在追逐中受伤被擒，最终被卖为奴隶。那些与马匹的默契和骑术，成为你逃亡时最宝贵的技能。狡诈+3，骑术+4，马匹×1"), GetSlaveBeforeHorseThiefArgs, (m) => true, (m) => SlaveBeforeHorseThiefOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("slave_before_mercenary", new TextObject("雇佣兵"), new TextObject("你的过去被记录在雇佣兵的账册中：你曾是一名经验丰富的雇佣兵，为不同的雇主战斗，从库塞特到帝国，从草原到沙漠。你懂得如何指挥小队作战，如何在混乱的战场上保持冷静。你曾参与过多次战斗，积累了战术经验与领导能力。然而，在一次深入阿塞莱沙漠的任务中，你的雇主背叛了你，将你交给了奴隶贩子以换取和平。那些战斗的经验与指挥的才能，在奴役中成为你组织同伴的资本。领导力+3，战术+3，金钱+80"), GetSlaveBeforeMercenaryArgs, (m) => true, (m) => SlaveBeforeMercenaryOnSelect(m), null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetSlaveEscapeNode1CharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetSlaveBeforeBorderWarriorArgs(NarrativeMenuOptionArgs args) { }
        private static void SlaveBeforeBorderWarriorOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了战奴逃亡-Node1-边境战士"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_ex_slave_before"] = "border_warrior"; }

        private static void GetSlaveBeforeSteppeBanditArgs(NarrativeMenuOptionArgs args) { }
        private static void SlaveBeforeSteppeBanditOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了战奴逃亡-Node1-草原响马"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_ex_slave_before"] = "steppe_bandit"; }

        private static void GetSlaveBeforeHorseThiefArgs(NarrativeMenuOptionArgs args) { }
        private static void SlaveBeforeHorseThiefOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了战奴逃亡-Node1-偷马"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_ex_slave_before"] = "horse_thief"; }

        private static void GetSlaveBeforeMercenaryArgs(NarrativeMenuOptionArgs args) { }
        private static void SlaveBeforeMercenaryOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择战奴逃亡-Node1-护商佣兵"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_ex_slave_before"] = "mercenary"; }

        private static NarrativeMenu CreateSlaveEscapeNode2Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu("slave_escape_node2", "slave_escape_node1", "slave_escape_node3", new TextObject("你为什么会被抓"), new TextObject("家族编年史中，记录着你被俘的那一天。是什么让你落入了奴隶贩子的手中？是战斗的失败，是背叛的陷阱，还是命运的捉弄？这段经历不仅决定了你与某些势力的关系，也影响了追捕者对你的仇恨程度"), characters, GetSlaveEscapeNode2CharacterArgs);

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("slave_captured_raid_village", new TextObject("劫掠村子被反杀"), new TextObject("族人后来讲述，你是在劫掠一个边境村庄时被反杀的。你本以为那是个容易得手的目标，但村里的商人们早有准备，他们雇佣了护卫，设下了陷阱。你冲进去时中了埋伏，虽然奋力抵抗，但最终还是被制服。那些商人对你恨之入骨，将你卖给了奴隶贩子，并警告其他商人要提防你这样的人。与商人关系-2，体力+2，标记：轻伤"), GetSlaveCapturedRaidVillageArgs, (m) => true, (m) => SlaveCapturedRaidVillageOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("slave_captured_caravan_ambush", new TextObject("伏击商队失手"), new TextObject("部族编年史记载，你是在伏击一支商队时失手的。你精心选择了伏击地点，等待商队进入陷阱。然而，商队的护卫比你想象的更加警惕，他们提前发现了你的埋伏，反而将你包围。在混乱的战斗中，你虽然逃脱了致命一击，但最终还是被俘虏。那些商队护卫将你交给了奴隶贩子，而你则在饥饿与疲惫中开始了奴役生涯。狡诈+2，士气+2，金钱+50，标记：饥饿"), GetSlaveCapturedCaravanAmbushArgs, (m) => true, (m) => SlaveCapturedCaravanAmbushOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("slave_captured_scapegoat", new TextObject("被人出卖当替罪羊"), new TextObject("家族记忆中最痛苦的一页：你被人出卖，成为了替罪羊。你原本信任的同伴或上级，为了掩盖自己的错误或换取利益，将所有的责任推到了你身上。你被指控犯下了你没有犯过的罪行，被剥夺了为自己辩护的机会，最终被卖为奴隶。这段经历让你学会了如何在困境中保持魅力与说服力，但也让你对精英氏族的信任彻底破灭。魅力+2，与精英氏族关系-1，标记：政治债务"), GetSlaveCapturedScapegoatArgs, (m) => true, (m) => SlaveCapturedScapegoatOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("slave_captured_lost", new TextObject("迷路/受伤被捕奴队捡走"), new TextObject("族人后来才知道，你是在迷路或受伤时被捕奴队捡走的。也许你是在草原上迷了路，也许你是在战斗中受了伤，无法继续前行。就在你最脆弱的时候，一支专门捕猎落单者的奴隶贩子队伍发现了你。他们像秃鹫一样围了上来，将你制服并带走。这段经历让你学会了如何在困境中保持警觉，但也给你留下了身体与精神上的创伤。侦察+2，标记：轻伤，疲惫"), GetSlaveCapturedLostArgs, (m) => true, (m) => SlaveCapturedLostOnSelect(m), null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetSlaveEscapeNode2CharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetSlaveCapturedRaidVillageArgs(NarrativeMenuOptionArgs args) { }
        private static void SlaveCapturedRaidVillageOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了战奴逃亡-Node2-劫掠村庄"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_ex_slave_captured"] = "raid_village"; }

        private static void GetSlaveCapturedCaravanAmbushArgs(NarrativeMenuOptionArgs args) { }
        private static void SlaveCapturedCaravanAmbushOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了战奴逃亡-Node2-保护商队"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_ex_slave_captured"] = "caravan_ambush"; }

        private static void GetSlaveCapturedScapegoatArgs(NarrativeMenuOptionArgs args) { }
        private static void SlaveCapturedScapegoatOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了战奴逃亡-Node2-替罪"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_ex_slave_captured"] = "scapegoat"; }

        private static void GetSlaveCapturedLostArgs(NarrativeMenuOptionArgs args) { }
        private static void SlaveCapturedLostOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择战奴逃亡-Node2-沙漠迷路"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_ex_slave_captured"] = "lost"; }

        private static NarrativeMenu CreateSlaveEscapeNode3Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu("slave_escape_node3", "slave_escape_node2", "slave_escape_node4", new TextObject("你是怎么逃出来的"), new TextObject("族人后来一直在传颂：你逃出奴隶营的那一夜，究竟发生了什么？是暴动的血与火，是潜行的影与暗，还是组织的智慧与团结？你选择的方式不仅决定了你获得的技能与特性，也决定了有多少同伴会跟随你一起获得自由"), characters, GetSlaveEscapeNode3CharacterArgs);

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("slave_escape_revolt", new TextObject("奴隶暴动杀出血路"), new TextObject("家族传说中，你是在一次奴隶暴动中杀出血路的。当奴隶主对你们的压迫达到极限时，你站了出来，号召其他奴隶一起反抗。你们趁着守卫松懈的时机，夺取了武器，在混乱中杀出了一条血路。你挥舞着从守卫手中夺来的刀剑，带领着其他奴隶冲出了奴隶营。虽然你们付出了代价，但你们获得了自由。这段经历让你变得更加坚强，但也让追捕者记住了你的面孔。单手+3，体力+3，士气+3，标记：被怀疑"), GetSlaveEscapeRevoltArgs, (m) => true, (m) => SlaveEscapeRevoltOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("slave_escape_stealth", new TextObject("趁夜潜逃"), new TextObject("族人后来才知道，你是趁夜潜逃的。在一个没有月亮的夜晚，你仔细观察了守卫的巡逻路线，找到了一个空隙。你悄无声息地解开了锁链，避开了所有的守卫，像影子一样消失在夜色中。你没有惊动任何人，没有留下任何痕迹，就这样悄无声息地获得了自由。这段经历让你成为了一个完美的潜行者，但也让你习惯了低调行事，不愿引人注目。侦察+5，狡诈+2，标记：低调行事"), GetSlaveEscapeStealthArgs, (m) => true, (m) => SlaveEscapeStealthOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("slave_escape_organized", new TextObject("串联同伴一起跑"), new TextObject("部族编年史记载，你是通过串联同伴一起逃跑的。你花了很长时间观察和了解其他奴隶，找到了那些值得信任、有勇气、有能力的同伴。你秘密地与他们联系，制定了一个周密的逃跑计划。在约定的夜晚，你们一起行动，互相掩护，最终成功逃脱。这段经历让你展现出了领导才能，也让那些跟随你逃脱的同伴成为了你最初的追随者。领导力+4，魅力+2，标记：有追随者"), GetSlaveEscapeOrganizedArgs, (m) => true, (m) => SlaveEscapeOrganizedOnSelect(m), null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetSlaveEscapeNode3CharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetSlaveEscapeRevoltArgs(NarrativeMenuOptionArgs args) { }
        private static void SlaveEscapeRevoltOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了战奴逃亡-Node3-暴动逃亡"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_ex_slave_escape"] = "revolt"; }

        private static void GetSlaveEscapeStealthArgs(NarrativeMenuOptionArgs args) { }
        private static void SlaveEscapeStealthOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了战奴逃亡-Node3-潜行逃亡"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_ex_slave_escape"] = "stealth"; }

        private static void GetSlaveEscapeOrganizedArgs(NarrativeMenuOptionArgs args) { }
        private static void SlaveEscapeOrganizedOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了战奴逃亡-Node3-组织逃亡"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_ex_slave_escape"] = "organized"; }

        private static NarrativeMenu CreateSlaveEscapeNode4Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu("slave_escape_node4", "slave_escape_node3", "slave_escape_node5", new TextObject("逃跑时你带走了什么"), new TextObject("族人后来一直在说：你逃出那夜，究竟带走了什么？是复仇的象征，是生存的财富，是忠诚的同伴，还是改变命运的秘密？你带走的东西不仅成为你逃亡路上的资源，也成为了你家族故事的起点，决定了你将以怎样的姿态重新面对这个世界"), characters, GetSlaveEscapeNode4CharacterArgs);

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("slave_loot_sword_head", new TextObject("砍下奴隶主的头，带走好刀"), new TextObject("族人后来一直在说：你逃出那夜，究竟带走了什么？家族传说中，你不仅逃出了奴隶营，还找到了那个曾经折磨你的奴隶主。你砍下了他的头，带走了他珍藏的那把好刀。这把刀成为了你复仇的象征，也成为了你自由的证明。每当有人质疑你的过去时，你都可以拿出这把刀，告诉他们你已经用血洗清了耻辱。这段经历让你获得了声望，也让你的士气更加高昂。声望+1，士气+2，标记：拥有象征物"), GetSlaveLootSwordHeadArgs, (m) => true, (m) => SlaveLootSwordHeadOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("slave_loot_money", new TextObject("混乱中抓走沉甸甸的钱袋"), new TextObject("部族编年史中记录，你在混乱中抓走了一个沉甸甸的钱袋。当奴隶营陷入混乱时，你看到了机会。你冲进了奴隶主的房间，找到了他存放金币的箱子。你抓走了一个装满金币的钱袋，这些钱成为了你逃亡路上的第一笔财富。虽然这些钱不能买回你失去的自由时光，但它们至少能让你在逃亡的路上不至于饿死。金钱+450，标记：短期富有"), GetSlaveLootMoneyArgs, (m) => true, (m) => SlaveLootMoneyOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("slave_loot_companions", new TextObject("救走几名同伴"), new TextObject("家族记忆中最温暖的一页：你不仅自己逃了出来，还救走了几名同伴。在混乱中，你没有只顾自己，而是打开了其他奴隶的锁链，带着他们一起逃跑。这些同伴成为了你最初的追随者，他们感激你的救命之恩，愿意跟随你一起面对未知的未来。虽然带着他们让你的逃亡更加困难，但你也因此不再孤单。初始兵力：逃奴×6，士气+4，标记：有依赖者"), GetSlaveLootCompanionsArgs, (m) => true, (m) => SlaveLootCompanionsOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("slave_loot_papers", new TextObject("偷走交易凭据/名册"), new TextObject("族人后来才知道，你偷走了奴隶主的交易凭据和名册。这些文件记录了奴隶主与其他商人的交易，也记录了其他奴隶的信息。你意识到这些文件的价值，它们不仅能证明你的身份，还能成为你与商人谈判的筹码。你小心地将这些文件藏好，带着它们一起逃亡。这些文件后来成为了你了解奴隶贸易网络的钥匙，也让你学会了如何与商人打交道。魅力+2，贸易+2，标记：拥有证明"), GetSlaveLootPapersArgs, (m) => true, (m) => SlaveLootPapersOnSelect(m), null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetSlaveEscapeNode4CharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetSlaveLootSwordHeadArgs(NarrativeMenuOptionArgs args) { }
        private static void SlaveLootSwordHeadOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了战奴逃亡-Node4-人头+好刀"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_ex_slave_loot"] = "sword_head"; }

        private static void GetSlaveLootMoneyArgs(NarrativeMenuOptionArgs args) { }
        private static void SlaveLootMoneyOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了战奴逃亡-Node4-钱袋"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_ex_slave_loot"] = "money"; }

        private static void GetSlaveLootCompanionsArgs(NarrativeMenuOptionArgs args) { }
        private static void SlaveLootCompanionsOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择战奴逃亡-Node4-救走同伴"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_ex_slave_loot"] = "companions"; }

        private static void GetSlaveLootPapersArgs(NarrativeMenuOptionArgs args) { }
        private static void SlaveLootPapersOnSelect(CharacterCreationManager manager) { OriginLog.Info("用户选择了战奴逃亡-Node4-钥匙/地图/文书"); OriginSystemHelper.SelectedPresetOriginNodes["khz_node_ex_slave_loot"] = "papers"; }

        private static NarrativeMenu CreateSlaveEscapeNode5Menu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu("slave_escape_node5", "slave_escape_node4", "narrative_parent_menu", new TextObject("你往哪里逃"), new TextObject("自由就在眼前，但前路茫茫。你要逃向何方？是回到熟悉的草原，是深入危险的沙漠，还是投向陌生的帝国？你的选择不仅决定了你将在哪里开始新的生活，也决定了你将面临怎样的挑战与机遇。每一个方向都有不同的故事在等待着你"), characters, GetSlaveEscapeNode5CharacterArgs);

            menu.AddNarrativeMenuOption(new NarrativeMenuOption("slave_direction_steppe", new TextObject("逃回草原"), new TextObject("你选择逃回草原，回到你熟悉的地方。那里有你的族人，有你的记忆，有你的根。虽然草原上可能还有追捕者，但至少你熟悉那里的每一寸土地，知道如何在那里生存。你沿着商路向北，穿过沙漠的边缘，最终回到了库塞特草原。在那里，你可以重新开始，用你在奴役中磨练的意志和技能，在草原上找到属于你的位置。侦察+3，骑术+2，标记：在路上"), GetSlaveDirectionSteppeArgs, (m) => true, (m) => SlaveDirectionSteppeOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("slave_direction_desert", new TextObject("逃向沙漠深处"), new TextObject("你选择逃向沙漠深处，消失在茫茫沙海之中。你明白，最危险的地方往往也是最安全的地方。追捕者不会想到你会深入沙漠，那里远离他们的势力范围，但也意味着更艰难的生存。你来到了古亚兹最南端的村庄附近，然后继续往南深入，消失在无边的沙海之中。在那里，你需要学会如何在极端环境中生存，如何管理有限的资源，如何找到水源和食物。这段经历让你变得更加坚韧，但也让你远离了文明世界。管理+2，侦察+3，标记：在路上"), GetSlaveDirectionDesertArgs, (m) => true, (m) => SlaveDirectionDesertOnSelect(m), null));
            menu.AddNarrativeMenuOption(new NarrativeMenuOption("slave_direction_empire", new TextObject("投向帝国边境"), new TextObject("你选择投向帝国边境，寻求一个新的开始。你明白，帝国虽然与库塞特和阿塞莱都有复杂的关系，但至少在那里，奴隶贸易是受到限制的。你沿着商路向西，穿过沙漠与草原的交界，最终到达了帝国最南端的边境城市。在那里，你可以用你的魅力和口才，说服帝国人接纳你，或者至少给你一个重新开始的机会。虽然你仍然需要小心隐藏你的过去，但至少你有了一个新的起点。魅力+2，侦察+2，标记：在路上"), GetSlaveDirectionEmpireArgs, (m) => true, (m) => SlaveDirectionEmpireOnSelect(m), null));

            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetSlaveEscapeNode5CharacterArgs(CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager) { return new List<NarrativeMenuCharacterArgs>(); }

        private static void GetSlaveDirectionSteppeArgs(NarrativeMenuOptionArgs args) { }
        private static void SlaveDirectionSteppeOnSelect(CharacterCreationManager manager) 
        { 
            OriginLog.Info("用户选择了战奴逃亡-Node5-回草原"); 
            OriginSystemHelper.SelectedPresetOriginNodes["khz_node_ex_slave_direction"] = "steppe"; 
            OriginSystemHelper.OriginSelectionDone = true; 
            // 逃回草原：位置保持默认，不需要设置
        }

        private static void GetSlaveDirectionDesertArgs(NarrativeMenuOptionArgs args) { }
        private static void SlaveDirectionDesertOnSelect(CharacterCreationManager manager) 
        { 
            OriginLog.Info("用户选择战奴逃亡-Node5-留沙漠"); 
            OriginSystemHelper.SelectedPresetOriginNodes["khz_node_ex_slave_direction"] = "desert"; 
            OriginSystemHelper.OriginSelectionDone = true; 
            
            // 保存期望出生位置：沙漠深处（阿塞莱的沙漠城市）
            SaveSlaveEscapeStartingLocation("desert");
        }

        private static void GetSlaveDirectionEmpireArgs(NarrativeMenuOptionArgs args) { }
        private static void SlaveDirectionEmpireOnSelect(CharacterCreationManager manager) 
        { 
            OriginLog.Info("用户选择战奴逃亡-Node5-投奔帝国"); 
            OriginSystemHelper.SelectedPresetOriginNodes["khz_node_ex_slave_direction"] = "empire"; 
            OriginSystemHelper.OriginSelectionDone = true; 
            
            // 保存期望出生位置：帝国最南端城市
            SaveSlaveEscapeStartingLocation("empire");
        }

        /// <summary>
        /// 保存逃奴的期望出生位置（延迟执行）
        /// 在 node 的 OnSelect 中调用，保存期望位置到 OriginSystemHelper
        /// </summary>
        private static void SaveSlaveEscapeStartingLocation(string direction)
        {
            try
            {
                // 在角色创建阶段，Campaign.Current 可能还不存在，所以只保存 direction
                // 实际的 settlement 查找和位置设置会在游戏开始后执行
                OriginSystemHelper.PendingStartDirection = direction;
                OriginSystemHelper.PendingStartSettlementId = null; // 延迟到游戏开始后查找
                
                OriginLog.Info($"[SlaveEscape][Node5] 已保存期望出生位置 direction={direction}");
            }
            catch (Exception ex)
            {
                OriginLog.Error($"[SlaveEscape][Node5] 保存出生位置时出错 {ex.Message}");
            }
        }

        #endregion
    }
}
