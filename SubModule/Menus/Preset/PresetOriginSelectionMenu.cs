using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using OriginSystemMod;

namespace OriginSystemMod
{
    public static partial class OriginSystemPatches
    {
        /// <summary>
        /// 运行时获取当前选择的文化ID（多层fallback）
        /// </summary>
        private static string GetSelectedCultureId(CharacterCreationManager m)
        {
            try
            {
                // 方案A：有时 Hero.MainHero.Culture 在创建流程中已被写入
                var heroCulture = Hero.MainHero?.Culture?.StringId;
                if (!string.IsNullOrEmpty(heroCulture))
                    return heroCulture.ToLowerInvariant();
            }
            catch { }

            try
            {
                // 方案B：反射路径（CharacterCreationContent.SelectedCulture）
                var contentProp = m.GetType().GetProperty("CharacterCreationContent",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                var content = contentProp?.GetValue(m, null);
                if (content != null)
                {
                    var cultureProp = content.GetType().GetProperty("SelectedCulture",
                        System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                    var cultureObj = cultureProp?.GetValue(content, null) as CultureObject;
                    var id = cultureObj?.StringId;
                    if (!string.IsNullOrEmpty(id))
                        return id.ToLowerInvariant();
                }
            }
            catch { }

            return "";
        }

        private static bool IsVlandia(CharacterCreationManager m)
        {
            var id = GetSelectedCultureId(m);
            return id == "vlandia" || id.Contains("vlandia");
        }

        private static bool IsKhuzait(CharacterCreationManager m)
        {
            var id = GetSelectedCultureId(m);
            return id == "khuzait" || id.Contains("khuzait");
        }

        private static bool IsEmpire(CharacterCreationManager m)
        {
            var id = GetSelectedCultureId(m);
            return id == "empire" || id.Contains("empire");
        }

        private static bool IsSturgia(CharacterCreationManager m)
        {
            var id = GetSelectedCultureId(m);
            return id == "sturgia" || id.Contains("sturgia");
        }

        private static bool IsBattania(CharacterCreationManager m)
        {
            var id = GetSelectedCultureId(m);
            return id == "battania" || id.Contains("battania");
        }

        private static bool IsAserai(CharacterCreationManager m)
        {
            var id = GetSelectedCultureId(m);
            return id == "aserai" || id.Contains("aserai");
        }

        private static NarrativeMenu CreatePresetOriginSelectionMenu(CharacterCreationManager manager)
        {
            var characters = new List<NarrativeMenuCharacter>();
            var menu = new NarrativeMenu(
                "preset_origin_selection",
                "origin_type_selection",
                "narrative_parent_menu",
                new TextObject("Choose Preset Origin"),
                new TextObject("Choose a preset origin background"),
                characters,
                GetPresetOriginMenuCharacterArgs
            );

            // 调试日志：记录创建时的文化状态
            string createTimeCultureId = GetSelectedCultureId(manager);
            OriginLog.Info($"[PresetMenu] Create time cultureId='{createTimeCultureId}' isVlandia={IsVlandia(manager)}");
            OriginLog.Info($"[PresetMenu] characters.Count={characters.Count}");

            // ✅ 修复：选项永远添加，不根据创建时的culture决定
            // 运行时通过OnCondition根据当前culture过滤显示

            // 瓦兰迪亚预设出身选项（永远添加，运行时通过OnCondition过滤）
            OriginLog.Info($"[PresetMenu] Adding Vlandia options (always add, filter by OnCondition)...");
            
            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                "vlandia_expedition_knight",
                new TextObject("远征的骑士"),
                new TextObject("远征归来的骑士，拥有丰富的战斗经验和声望"),
                GetExpeditionKnightArgs,
                ExpeditionKnightOnCondition,
                ExpeditionKnightOnSelect,
                null
            ));

                menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                    "vlandia_bankrupt_banneret",
                    new TextObject("破产的旗主继承人"),
                    new TextObject("纹章还在，封地没了；你要'保体面'还是'卖祖产'"),
                    GetBankruptBanneretArgs,
                    BankruptBanneretOnCondition,
                    BankruptBanneretOnSelect,
                    null
                ));

                menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                    "vlandia_tourney_champion",
                    new TextObject("比武场的落魄冠军"),
                    new TextObject("曾经的比武场冠军，如今落魄"),
                    GetTourneyChampionArgs,
                    TourneyChampionOnCondition,
                    TourneyChampionOnSelect,
                    null
                ));

                menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                    "vlandia_black_path_captain",
                    new TextObject("黑旗匪首"),
                    new TextObject("黑道线的首领，永久无法加入瓦兰迪亚"),
                    GetBlackPathCaptainArgs,
                    BlackPathCaptainOnCondition,
                    BlackPathCaptainOnSelect,
                    null
                ));

                menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                    "vlandia_crossbow_guild",
                    new TextObject("弩手行会的推旗人"),
                    new TextObject("弩手行会的领导者"),
                    GetCrossbowGuildArgs,
                    CrossbowGuildOnCondition,
                    CrossbowGuildOnSelect,
                    null
                ));

                menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                    "vlandia_march_warden",
                    new TextObject("边境的巡守官"),
                    new TextObject("边境的巡守官，负责守卫边境"),
                    GetMarchWardenArgs,
                    MarchWardenOnCondition,
                    MarchWardenOnSelect,
                    null
                ));

                menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                    "vlandia_order_squire",
                    new TextObject("侍奉骑士团的扈从"),
                    new TextObject("骑士团的扈从，侍奉骑士"),
                    GetOrderSquireArgs,
                    OrderSquireOnCondition,
                    OrderSquireOnSelect,
                    null
                ));

                menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                    "vlandia_sellsword_lieutenant",
                    new TextObject("雇佣军连队副官"),
                    new TextObject("雇佣军连队的副官，拥有丰富的战斗经验"),
                    GetSellswordLieutenantArgs,
                    SellswordLieutenantOnCondition,
                    SellswordLieutenantOnSelect,
                    null
                ));

                menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                    "vlandia_tax_bailiff_enforcer",
                    new TextObject("王室税吏的护卫"),
                    new TextObject("王室税吏的护卫，负责收税和保护"),
                    GetTaxBailiffEnforcerArgs,
                    TaxBailiffEnforcerOnCondition,
                    TaxBailiffEnforcerOnSelect,
                    null
                ));

                menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                    "vlandia_free_militia_leader",
                    new TextObject("自由民团的推旗人"),
                    new TextObject("自由民团的领导者，组织民团"),
                    GetFreeMilitiaLeaderArgs,
                    FreeMilitiaLeaderOnCondition,
                    FreeMilitiaLeaderOnSelect,
                    null
                ));

                menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                    "vlandia_degraded_rogue_knight",
                    new TextObject("堕落无赖骑士"),
                    new TextObject("你曾经是一名骑士，但被瓦兰迪亚王国公开剥夺了贵族身份。你的纹章被抹去，你的誓言被宣布无效。你不再是'爵士'，但你仍能骑马冲锋。"),
                    GetDegradedRogueKnightArgs,
                    DegradedRogueKnightOnCondition,
                    DegradedRogueKnightOnSelect,
                    null
                ));
            
            OriginLog.Info($"[PresetMenu] Added 11 Vlandia options");

            // 库塞特预设出身选项（永远添加，运行时通过OnCondition过滤）
            OriginLog.Info($"[PresetMenu] Adding Khuzait options (always add, filter by OnCondition)...");
            
            menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                    "khuzait_rebel_chief",
                    new TextObject("Steppe Rebel Chief"),
                    new TextObject("Illegal tribe chief, hostile to Khuzait Khanate. Initial troops: Nomad Warriors x20, Light Cavalry x8"),
                    GetRebelChiefArgs,
                    RebelChiefOnCondition,
                    RebelChiefOnSelect,
                    null
                ));

                menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                    "khuzait_minor_noble",
                    new TextObject("Minor Khanate Noble"),
                    new TextObject("Minor noble of the Khanate, political inclination, has some noble followers, stronger social charm route"),
                    GetMinorNobleArgs,
                    MinorNobleOnCondition,
                    MinorNobleOnSelect,
                    null
                ));

                menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                    "khuzait_migrant_chief",
                    new TextObject("Westward Migrant Chief"),
                    new TextObject("Westward migrating tribe chief, survival/exile inclination, low resources, high population pressure"),
                    GetMigrantChiefArgs,
                    MigrantChiefOnCondition,
                    MigrantChiefOnSelect,
                    null
                ));

                menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                    "khuzait_army_deserter",
                    new TextObject("Khanate Army Deserter"),
                    new TextObject("Soldier who deserted from Khanate army, military inclination, high proficiency in mounted javelin, obvious tactical tendency"),
                    GetArmyDeserterArgs,
                    ArmyDeserterOnCondition,
                    ArmyDeserterOnSelect,
                    null
                ));

                menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                    "khuzait_trade_protector",
                    new TextObject("Steppe Trade Route Protector"),
                    new TextObject("Guardian protecting steppe trade routes, economic/neutral inclination, trade and social tendency, large cross-faction space"),
                    GetTradeProtectorArgs,
                    TradeProtectorOnCondition,
                    TradeProtectorOnSelect,
                    null
                ));

                menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                    "khuzait_wandering_prince",
                    new TextObject("Wandering Prince"),
                    new TextObject("Exiled Khuzait prince, clan level. Initial clan members: 3-4 people"),
                    GetWanderingPrinceArgs,
                    WanderingPrinceOnCondition,
                    WanderingPrinceOnSelect,
                    null
                ));

                menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                    "khuzait_khans_mercenary",
                    new TextObject("Khan's Mercenary Warrior"),
                    new TextObject("War band hired by Khan, fast-paced combat, strong troops, fast combat rhythm, higher Khanate relations"),
                    GetKhansMercenaryArgs,
                    KhansMercenaryOnCondition,
                    KhansMercenaryOnSelect,
                    null
                ));

                menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                    "khuzait_slave_escape",
                    new TextObject("War Slave Escape"),
                    new TextObject("Former war slave who escaped from Aserai, being hunted, initial troops: Former War Slave Warriors x18"),
                    GetSlaveEscapeArgs,
                    SlaveEscapeOnCondition,
                    SlaveEscapeOnSelect,
                    null
                ));

                menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                    "khuzait_free_cossack",
                    new TextObject("Steppe Free People"),
                    new TextObject("Free people on the steppe, Cossack prototype, high freedom, mixed units, strong gray survival ability"),
                    GetFreeCossackArgs,
                    FreeCossackOnCondition,
                    FreeCossackOnSelect,
                    null
                ));

                menu.AddNarrativeMenuOption(new NarrativeMenuOption(
                    "khuzait_old_guard_avenger",
                    new TextObject("Eastern Old Guard Avenger"),
                    new TextObject("Veteran seeking revenge for old master, rich combat experience, many veterans, high combat experience, clear enemies"),
                    GetOldGuardAvengerArgs,
                    OldGuardAvengerOnCondition,
                    OldGuardAvengerOnSelect,
                    null
                ));
            
            OriginLog.Info($"[PresetMenu] Added 10 Khuzait options");

            // 其他文化的预设出身选项（暂未实现，后续可以添加）
            // 注意：目前只实现了瓦兰迪亚和库塞特的preset选项

            OriginLog.Info($"[PresetMenu] Final menu created");
            
            return menu;
        }

        private static List<NarrativeMenuCharacterArgs> GetPresetOriginMenuCharacterArgs(
            CultureObject culture, string occupationType, CharacterCreationManager characterCreationManager)
        {
            return new List<NarrativeMenuCharacterArgs>();
        }

        private static void GetRebelChiefArgs(NarrativeMenuOptionArgs args)
        {
        }

        private static bool RebelChiefOnCondition(CharacterCreationManager characterCreationManager)
        {
            return IsKhuzait(characterCreationManager);
        }

        private static void RebelChiefOnSelect(CharacterCreationManager characterCreationManager)
        {
            try
            {
                OriginLog.Info("User selected Steppe Rebel Chief");
                OriginSystemHelper.SelectedPresetOriginId = "khuzait_rebel_chief";
                OriginSystemHelper.IsPresetOrigin = true;
            }
            catch (Exception ex)
            {
                OriginLog.Error($"RebelChiefOnSelect failed: {ex.Message}");
            }
        }

        private static void GetWanderingPrinceArgs(NarrativeMenuOptionArgs args)
        {
        }

        private static bool WanderingPrinceOnCondition(CharacterCreationManager characterCreationManager)
        {
            return IsKhuzait(characterCreationManager);
        }

        private static void WanderingPrinceOnSelect(CharacterCreationManager characterCreationManager)
        {
            OriginLog.Info("User selected Wandering Prince");
            OriginSystemHelper.SelectedPresetOriginId = "khuzait_wandering_prince";
            OriginSystemHelper.IsPresetOrigin = true;
        }

        private static void GetSlaveEscapeArgs(NarrativeMenuOptionArgs args)
        {
        }

        private static bool SlaveEscapeOnCondition(CharacterCreationManager characterCreationManager)
        {
            return IsKhuzait(characterCreationManager);
        }

        private static void SlaveEscapeOnSelect(CharacterCreationManager characterCreationManager)
        {
            try
            {
                OriginLog.Info("User selected War Slave Escape");
                OriginSystemHelper.SelectedPresetOriginId = "khuzait_slave_escape";
                OriginSystemHelper.IsPresetOrigin = true;
            }
            catch (Exception ex)
            {
                OriginLog.Error($"SlaveEscapeOnSelect failed: {ex.Message}");
            }
        }

        private static void GetMinorNobleArgs(NarrativeMenuOptionArgs args)
        {
        }

        private static bool MinorNobleOnCondition(CharacterCreationManager characterCreationManager)
        {
            return IsKhuzait(characterCreationManager);
        }

        private static void MinorNobleOnSelect(CharacterCreationManager characterCreationManager)
        {
            OriginLog.Info("User selected Minor Khanate Noble");
            OriginSystemHelper.SelectedPresetOriginId = "khuzait_minor_noble";
            OriginSystemHelper.IsPresetOrigin = true;
        }

        private static void GetMigrantChiefArgs(NarrativeMenuOptionArgs args)
        {
        }

        private static bool MigrantChiefOnCondition(CharacterCreationManager characterCreationManager)
        {
            return IsKhuzait(characterCreationManager);
        }

        private static void MigrantChiefOnSelect(CharacterCreationManager characterCreationManager)
        {
            OriginLog.Info("User selected Westward Migrant Chief");
            OriginSystemHelper.SelectedPresetOriginId = "khuzait_migrant_chief";
            OriginSystemHelper.IsPresetOrigin = true;
        }

        private static void GetArmyDeserterArgs(NarrativeMenuOptionArgs args)
        {
        }

        private static bool ArmyDeserterOnCondition(CharacterCreationManager characterCreationManager)
        {
            return IsKhuzait(characterCreationManager);
        }

        private static void ArmyDeserterOnSelect(CharacterCreationManager characterCreationManager)
        {
            OriginLog.Info("User selected Khanate Army Deserter");
            OriginSystemHelper.SelectedPresetOriginId = "khuzait_army_deserter";
            OriginSystemHelper.IsPresetOrigin = true;
        }

        private static void GetTradeProtectorArgs(NarrativeMenuOptionArgs args)
        {
        }

        private static bool TradeProtectorOnCondition(CharacterCreationManager characterCreationManager)
        {
            return IsKhuzait(characterCreationManager);
        }

        private static void TradeProtectorOnSelect(CharacterCreationManager characterCreationManager)
        {
            OriginLog.Info("User selected Steppe Trade Route Protector");
            OriginSystemHelper.SelectedPresetOriginId = "khuzait_trade_protector";
            OriginSystemHelper.IsPresetOrigin = true;
        }

        private static void GetKhansMercenaryArgs(NarrativeMenuOptionArgs args)
        {
        }

        private static bool KhansMercenaryOnCondition(CharacterCreationManager characterCreationManager)
        {
            return IsKhuzait(characterCreationManager);
        }

        private static void KhansMercenaryOnSelect(CharacterCreationManager characterCreationManager)
        {
            OriginLog.Info("User selected Khan's Mercenary Warrior");
            OriginSystemHelper.SelectedPresetOriginId = "khuzait_khans_mercenary";
            OriginSystemHelper.IsPresetOrigin = true;
        }

        private static void GetFreeCossackArgs(NarrativeMenuOptionArgs args)
        {
        }

        private static bool FreeCossackOnCondition(CharacterCreationManager characterCreationManager)
        {
            return IsKhuzait(characterCreationManager);
        }

        private static void FreeCossackOnSelect(CharacterCreationManager characterCreationManager)
        {
            OriginLog.Info("User selected Steppe Free People");
            OriginSystemHelper.SelectedPresetOriginId = "khuzait_free_cossack";
            OriginSystemHelper.IsPresetOrigin = true;
        }

        private static void GetOldGuardAvengerArgs(NarrativeMenuOptionArgs args)
        {
        }

        private static bool OldGuardAvengerOnCondition(CharacterCreationManager characterCreationManager)
        {
            return IsKhuzait(characterCreationManager);
        }

        private static void OldGuardAvengerOnSelect(CharacterCreationManager characterCreationManager)
        {
            OriginLog.Info("User selected Eastern Old Guard Avenger");
            OriginSystemHelper.SelectedPresetOriginId = "khuzait_old_guard_avenger";
            OriginSystemHelper.IsPresetOrigin = true;
        }

        // 瓦兰迪亚预设出身的方法
        private static void GetExpeditionKnightArgs(NarrativeMenuOptionArgs args) { }
        private static bool ExpeditionKnightOnCondition(CharacterCreationManager characterCreationManager)
        {
            bool result = IsVlandia(characterCreationManager);
            OriginLog.Info($"[PresetMenu] ExpeditionKnightOnCondition cultureId='{GetSelectedCultureId(characterCreationManager)}' isVlandia={result}");
            return result;
        }
        private static void ExpeditionKnightOnSelect(CharacterCreationManager characterCreationManager)
        {
            OriginLog.Info("User selected Expedition Knight");
            OriginSystemHelper.SelectedPresetOriginId = "vlandia_expedition_knight";
            OriginSystemHelper.IsPresetOrigin = true;
        }

        private static void GetBankruptBanneretArgs(NarrativeMenuOptionArgs args) { }
        private static bool BankruptBanneretOnCondition(CharacterCreationManager characterCreationManager) { return IsVlandia(characterCreationManager); }
        private static void BankruptBanneretOnSelect(CharacterCreationManager characterCreationManager)
        {
            OriginLog.Info("User selected Bankrupt Banneret");
            OriginSystemHelper.SelectedPresetOriginId = "vlandia_bankrupt_banneret";
            OriginSystemHelper.IsPresetOrigin = true;
        }

        private static void GetTourneyChampionArgs(NarrativeMenuOptionArgs args) { }
        private static bool TourneyChampionOnCondition(CharacterCreationManager characterCreationManager) { return IsVlandia(characterCreationManager); }
        private static void TourneyChampionOnSelect(CharacterCreationManager characterCreationManager)
        {
            OriginLog.Info("User selected Tourney Champion");
            OriginSystemHelper.SelectedPresetOriginId = "vlandia_tourney_champion";
            OriginSystemHelper.IsPresetOrigin = true;
        }

        private static void GetBlackPathCaptainArgs(NarrativeMenuOptionArgs args) { }
        private static bool BlackPathCaptainOnCondition(CharacterCreationManager characterCreationManager) { return IsVlandia(characterCreationManager); }
        private static void BlackPathCaptainOnSelect(CharacterCreationManager characterCreationManager)
        {
            OriginLog.Info("User selected Black Path Captain");
            OriginSystemHelper.SelectedPresetOriginId = "vlandia_black_path_captain";
            OriginSystemHelper.IsPresetOrigin = true;
        }

        private static void GetCrossbowGuildArgs(NarrativeMenuOptionArgs args) { }
        private static bool CrossbowGuildOnCondition(CharacterCreationManager characterCreationManager) { return IsVlandia(characterCreationManager); }
        private static void CrossbowGuildOnSelect(CharacterCreationManager characterCreationManager)
        {
            OriginLog.Info("User selected Crossbow Guild");
            OriginSystemHelper.SelectedPresetOriginId = "vlandia_crossbow_guild";
            OriginSystemHelper.IsPresetOrigin = true;
        }

        private static void GetMarchWardenArgs(NarrativeMenuOptionArgs args) { }
        private static bool MarchWardenOnCondition(CharacterCreationManager characterCreationManager) { return IsVlandia(characterCreationManager); }
        private static void MarchWardenOnSelect(CharacterCreationManager characterCreationManager)
        {
            OriginLog.Info("User selected March Warden");
            OriginSystemHelper.SelectedPresetOriginId = "vlandia_march_warden";
            OriginSystemHelper.IsPresetOrigin = true;
        }

        private static void GetOrderSquireArgs(NarrativeMenuOptionArgs args) { }
        private static bool OrderSquireOnCondition(CharacterCreationManager characterCreationManager) { return IsVlandia(characterCreationManager); }
        private static void OrderSquireOnSelect(CharacterCreationManager characterCreationManager)
        {
            OriginLog.Info("User selected Order Squire");
            OriginSystemHelper.SelectedPresetOriginId = "vlandia_order_squire";
            OriginSystemHelper.IsPresetOrigin = true;
        }

        private static void GetSellswordLieutenantArgs(NarrativeMenuOptionArgs args) { }
        private static bool SellswordLieutenantOnCondition(CharacterCreationManager characterCreationManager) { return IsVlandia(characterCreationManager); }
        private static void SellswordLieutenantOnSelect(CharacterCreationManager characterCreationManager)
        {
            OriginLog.Info("User selected Sellsword Lieutenant");
            OriginSystemHelper.SelectedPresetOriginId = "vlandia_sellsword_lieutenant";
            OriginSystemHelper.IsPresetOrigin = true;
        }

        private static void GetTaxBailiffEnforcerArgs(NarrativeMenuOptionArgs args) { }
        private static bool TaxBailiffEnforcerOnCondition(CharacterCreationManager characterCreationManager) { return IsVlandia(characterCreationManager); }
        private static void TaxBailiffEnforcerOnSelect(CharacterCreationManager characterCreationManager)
        {
            OriginLog.Info("User selected Tax Bailiff Enforcer");
            OriginSystemHelper.SelectedPresetOriginId = "vlandia_tax_bailiff_enforcer";
            OriginSystemHelper.IsPresetOrigin = true;
        }

        private static void GetFreeMilitiaLeaderArgs(NarrativeMenuOptionArgs args) { }
        private static bool FreeMilitiaLeaderOnCondition(CharacterCreationManager characterCreationManager) { return IsVlandia(characterCreationManager); }
        private static void FreeMilitiaLeaderOnSelect(CharacterCreationManager characterCreationManager)
        {
            OriginLog.Info("User selected Free Militia Leader");
            OriginSystemHelper.SelectedPresetOriginId = "vlandia_free_militia_leader";
            OriginSystemHelper.IsPresetOrigin = true;
        }

        private static void GetDegradedRogueKnightArgs(NarrativeMenuOptionArgs args) { }
        private static bool DegradedRogueKnightOnCondition(CharacterCreationManager characterCreationManager) { return IsVlandia(characterCreationManager); }
        private static void DegradedRogueKnightOnSelect(CharacterCreationManager characterCreationManager)
        {
            OriginLog.Info("User selected Degraded Rogue Knight");
            OriginSystemHelper.SelectedPresetOriginId = "vlandia_degraded_rogue_knight";
            OriginSystemHelper.IsVlandiaRogueKnight = true;
            OriginSystemHelper.IsPresetOrigin = true;
        }

        // 其他文化的占位符方法
        private static void GetEmpirePlaceholderArgs(NarrativeMenuOptionArgs args) { }
        private static bool EmpirePlaceholderOnCondition(CharacterCreationManager characterCreationManager) { return false; } // 暂时禁用
        private static void EmpirePlaceholderOnSelect(CharacterCreationManager characterCreationManager)
        {
            OriginLog.Info("User selected Empire placeholder (not implemented yet)");
        }

        private static void GetSturgiaPlaceholderArgs(NarrativeMenuOptionArgs args) { }
        private static bool SturgiaPlaceholderOnCondition(CharacterCreationManager characterCreationManager) { return false; } // 暂时禁用
        private static void SturgiaPlaceholderOnSelect(CharacterCreationManager characterCreationManager)
        {
            OriginLog.Info("User selected Sturgia placeholder (not implemented yet)");
        }

        private static void GetBattaniaPlaceholderArgs(NarrativeMenuOptionArgs args) { }
        private static bool BattaniaPlaceholderOnCondition(CharacterCreationManager characterCreationManager) { return false; } // 暂时禁用
        private static void BattaniaPlaceholderOnSelect(CharacterCreationManager characterCreationManager)
        {
            OriginLog.Info("User selected Battania placeholder (not implemented yet)");
        }

        private static void GetAseraiPlaceholderArgs(NarrativeMenuOptionArgs args) { }
        private static bool AseraiPlaceholderOnCondition(CharacterCreationManager characterCreationManager) { return false; } // 暂时禁用
        private static void AseraiPlaceholderOnSelect(CharacterCreationManager characterCreationManager)
        {
            OriginLog.Info("User selected Aserai placeholder (not implemented yet)");
        }

        private static void GetNoCultureArgs(NarrativeMenuOptionArgs args) { }
        private static bool NoCultureOnCondition(CharacterCreationManager characterCreationManager) { return false; } // 暂时禁用
        private static void NoCultureOnSelect(CharacterCreationManager characterCreationManager)
        {
            OriginLog.Info("User selected no culture option");
        }
    }
}
