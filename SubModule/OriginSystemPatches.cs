using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.CampaignSystem;
using OriginSystemMod;

namespace OriginSystemMod


{


    /// <summary>


    /// Harmony Patches 用于修改角色创建流程


    /// 按照 ChatGPT 教诲:只 patch 确定存在的方法,不要动TargetMethod


    /// </summary>


    public static partial class OriginSystemPatches
    {


        // 注意:已移除 _isProcessingPresetOrigin 和 _isProcessingNonPresetOrigin
        // 因为按照 ChatGPT 建议,OnSelect 回调不再切菜单,所以不需要这些标志位
        // 日志工具已移动到 Util/OriginLog.cs
        // 出身类型选择菜单已移动到 Menus/OriginTypeSelectionMenu.cs


        #region 预设出身选择菜单


        /// <summary>


        /// 创建"预设出身选择"菜单


        /// </summary>


        // CreatePresetOriginSelectionMenu 和相关回调已移动到 Menus/Preset/PresetOriginSelectionMenu.cs


        #endregion


        #region 预设出身私有节点菜单


        // 草原叛酋节点菜单已移动到 Menus/Preset/Nodes/RebelChiefNodes.cs
        // 迁徙王族节点菜单已移动到 Menus/Preset/Nodes/WanderingPrinceNodes.cs
        // 战奴逃亡节点菜单已移动到 Menus/Preset/Nodes/SlaveEscapeNodes.cs
        // 汗廷旁支贵族节点菜单已移动到 Menus/Preset/Nodes/MinorNobleNodes.cs
        // 西迁部族首领节点菜单已移动到 Menus/Preset/Nodes/MigrantChiefNodes.cs
        // 汗廷军叛逃者节点菜单已移动到 Menus/Preset/Nodes/ArmyDeserterNodes.cs
        // 草原商路守护者节点菜单已移动到 Menus/Preset/Nodes/TradeProtectorNodes.cs
        // 可汗的雇佣战帮节点菜单已移动到 Menus/Preset/Nodes/KhansMercenaryNodes.cs
        // 草原自由民节点菜单已移动到 Menus/Preset/Nodes/FreeCossackNodes.cs
        // 东方旧部复仇者节点菜单已移动到 Menus/Preset/Nodes/OldGuardAvengerNodes.cs

        #endregion

        // 非预设出身菜单已移动到 Menus/NonPreset/ 目录下
        // - 核心菜单：NonPresetCultureAnchorMenu, NonPresetSocialOriginMenu, NonPresetSkillBackgroundMenu, NonPresetStartingConditionMenu
        // - 角色特定菜单：NonPresetNomadRoleMenu, NonPresetCraftsmanTradeMenu, NonPresetCaravanRoleMenu, NonPresetPeasantRoleMenu, NonPresetUrbanPoorRoleMenu
        // - 可选公共节点菜单：NonPresetMinorClanPositionMenu, NonPresetRefugeeBurdenMenu, NonPresetStartLocationTypeMenu, NonPresetValuableItemMenu, NonPresetContactTypeMenu

        #region 辅助方法

        /// <summary>
        /// 获取文化名称
        /// </summary>
        private static string GetCultureName(string cultureId)
        {
            switch (cultureId.ToLowerInvariant())
            {
                case "empire": return "帝国";
                case "sturgia": return "斯特吉亚";
                case "battania": return "巴丹尼亚";
                case "khuzait": return "库塞特";
                case "aserai": return "阿塞莱";
                case "vlandia": return "瓦兰迪亚";
                default: return cultureId;
            }
        }

        /// <summary>
        /// 重新创建父母菜单,修复InputMenuId
        /// </summary>


        private static NarrativeMenu RecreateParentMenuWithNewInputId(NarrativeMenu originalMenu, string newInputMenuId)


        {


            try


            {


                // 使用反射获取原菜单的所有选项


                var optionsField = typeof(NarrativeMenu).GetField("_characterCreationMenuOptions", 


                    BindingFlags.NonPublic | BindingFlags.Instance);


                var originalOptions = optionsField?.GetValue(originalMenu);


                // 重新创建菜单,使用新的InputMenuId


                var recreatedMenu = new NarrativeMenu(


                    originalMenu.StringId,


                    newInputMenuId,  // 新的 InputMenuId


                    originalMenu.OutputMenuId,


                    originalMenu.Title,


                    originalMenu.Description,


                    originalMenu.Characters,


                    originalMenu.GetNarrativeMenuCharacterArgs


                );


                // 重新添加所有选项(通过反射获取,使用IEnumerable 遍历)
                if (originalOptions != null && originalOptions is System.Collections.IEnumerable enumerable)


                {


                    foreach (NarrativeMenuOption option in enumerable)


                    {


                        if (option != null)


                        {


                            recreatedMenu.AddNarrativeMenuOption(option);


                        }


                    }


                }


                return recreatedMenu;


            }


            catch (Exception ex)


            {


                OriginLog.Error($"RecreateParentMenuWithNewInputId 失败: {ex.Message}");
                OriginLog.Error($"StackTrace: {ex.StackTrace}");


                // 如果失败,返回一个空的菜单(至少不会崩溃)
                return new NarrativeMenu(


                    originalMenu.StringId,


                    newInputMenuId,


                    originalMenu.OutputMenuId,


                    originalMenu.Title,


                    originalMenu.Description,


                    new List<NarrativeMenuCharacter>(),


                    originalMenu.GetNarrativeMenuCharacterArgs


                );


            }


        }


        #endregion

        /// <summary>
        /// Harmony Patch for CharacterCreationManager.OnNarrativeMenuOptionSelected
        /// 使用 Prefix 拦截,在引擎处理前根据 option.StringId 直接决定目标菜单
        /// </summary>


        [HarmonyPatch(typeof(CharacterCreationManager), "OnNarrativeMenuOptionSelected")]


        public static class OnNarrativeMenuOptionSelectedPatch


        {


            /// <summary>
            /// Prefix: 记录选项选择时的状态（防蠢日志）
            /// </summary>
            static void Prefix(CharacterCreationManager __instance, NarrativeMenuOption option)
            {
                // E1. 每次点击选项时的日志（固定格式）
                string menuId = __instance.CurrentMenu?.StringId ?? "NULL";
                string optionId = option?.StringId ?? "NULL";
                bool hasSelectedKey = __instance.CurrentMenu != null && __instance.SelectedOptions != null && __instance.SelectedOptions.ContainsKey(__instance.CurrentMenu);
                OriginLog.Info($"Select: menu={menuId} option={optionId} hasSelectedKey={hasSelectedKey}");
            }


            static void Postfix(CharacterCreationManager __instance, NarrativeMenuOption option)
            {
                // 记录 OnNarrativeMenuOptionSelected 的执行情况
                OriginLog.Info("========== OnNarrativeMenuOptionSelected Postfix ==========");
                
                if (__instance.CurrentMenu != null)
                {
                    OriginLog.Info($"Postfix: 当前菜单 = {__instance.CurrentMenu.StringId}");
                    OriginLog.Info($"Postfix:   InputMenuId = {__instance.CurrentMenu.InputMenuId}");
                    OriginLog.Info($"Postfix:   OutputMenuId = {__instance.CurrentMenu.OutputMenuId}");
                }
                else
                {
                    OriginLog.Warning("Postfix: 当前菜单 = null");
                }
                
                if (option != null)
                {
                    OriginLog.Info($"Postfix: 选项 = {option.StringId} ({option.Text})");
                }
                else
                {
                    OriginLog.Warning("Postfix: 选项 = null");
                }
                
                // 检查 SelectedOptions 是否已更新
                if (__instance.CurrentMenu != null && __instance.SelectedOptions != null)
                {
                    var hasKey = __instance.SelectedOptions.ContainsKey(__instance.CurrentMenu);
                    OriginLog.Info($"Postfix: SelectedOptions 包含当前菜单 = {hasKey}");
                    if (hasKey && __instance.SelectedOptions[__instance.CurrentMenu] != null)
                    {
                        var selectedOpt = __instance.SelectedOptions[__instance.CurrentMenu];
                        OriginLog.Info($"Postfix: SelectedOption = {selectedOpt.StringId} ({selectedOpt.Text})");
                    }
                }
                
                OriginLog.Info("========== OnNarrativeMenuOptionSelected Postfix 结束 ==========");
                
                // 注意：不要在这里清理 PendingMenuSwitch！
                // PendingMenuSwitch 应该由 TrySwitchToNextMenuPatch 中的 ResolveNextMenuId 来使用和清空
                // 如果在这里清空，TrySwitchToNextMenu 就读取不到它了
                if (!string.IsNullOrEmpty(OriginSystemHelper.PendingMenuSwitch))
                {
                    OriginLog.Info($"Postfix: PendingMenuSwitch={OriginSystemHelper.PendingMenuSwitch} (保留给 TrySwitchToNextMenu 使用)");
                }
            }
        }
        
        // TrySwitchToNextMenuPatch 已移动到 Patches/TrySwitchToNextMenuPatch.cs
        // ResolveNextMenuId, DumpCandidates 已移动到 Routing/OriginMenuRouter.cs
        // TrySetCurrentMenuByReflection, InvokeModifyMenuCharacters 已移动到 Util/ReflectionUtil.cs

        /// <summary>
        /// Harmony Patch for SandboxCharacterCreationContent.OnCharacterCreationFinalized
        /// 在角色创建 finalize 时设置逃奴的出生位置（正确时机）
        /// 
        /// 按照 old-good 版本：简单实现，不添加 try-catch（Harmony 会处理 null 返回值）
        /// </summary>
        [HarmonyPatch]
        public static class OnCharacterCreationFinalizedPatch
        {
            static Type TargetType()
            {
                // 尝试查找 SandboxCharacterCreationContent 或 CharacterCreationContent
                var type = Type.GetType("SandBox.CharacterCreationContent.SandboxCharacterCreationContent, SandBox");
                if (type == null)
                {
                    type = Type.GetType("TaleWorlds.CampaignSystem.CharacterCreationContent.CharacterCreationContent, TaleWorlds.CampaignSystem");
                }
                return type;
            }

            static MethodBase TargetMethod()
            {
                var t = TargetType();
                if (t == null) return null;
                
                // 尝试查找 OnCharacterCreationFinalized 方法
                var method = AccessTools.Method(t, "OnCharacterCreationFinalized");
                if (method == null)
                {
                    // 如果找不到，尝试 FinalizeCharacterCreation
                    method = AccessTools.Method(t, "FinalizeCharacterCreation");
                }
                return method;
            }

            [HarmonyPostfix]
            static void Postfix()
            {
                try
                {
                    OriginLog.Info("[SlaveEscape][Finalize] OnCharacterCreationFinalized Postfix called");

                    // 首先应用预设出身效果（如果还没应用）
                    if (OriginSystemHelper.IsPresetOrigin && 
                        !string.IsNullOrEmpty(OriginSystemHelper.SelectedPresetOriginId))
                    {
                        OriginLog.Info($"[Finalize] 开始应用预设出身: {OriginSystemHelper.SelectedPresetOriginId}");
                        PresetOriginSystem.ApplyPresetOrigin(OriginSystemHelper.SelectedPresetOriginId);
                        OriginLog.Info($"[Finalize] 已应用预设出身: {OriginSystemHelper.SelectedPresetOriginId}");
                    }
                    else if (!OriginSystemHelper.IsPresetOrigin)
                    {
                        OriginLog.Info("[Finalize] 开始应用非预设出身");
                        NonPresetOriginSystem.ApplyNonPresetOrigin();
                        OriginLog.Info("[Finalize] 已应用非预设出身");
                    }

                    // 检查是否有待处理的出生位置设置
                    if (string.IsNullOrEmpty(OriginSystemHelper.PendingStartDirection))
                    {
                        OriginLog.Info("[SlaveEscape][Finalize] 无待处理的出生位置设置");
                        return;
                    }

                    var direction = OriginSystemHelper.PendingStartDirection;
                    OriginLog.Info($"[SlaveEscape][Finalize] 开始设置出生位置: direction={direction}");

                    // 检查必要对象是否存在
                    if (Hero.MainHero == null)
                    {
                        OriginLog.Warning("[SlaveEscape][Finalize] Hero.MainHero 为空，延迟到 OnTick");
                        return;
                    }

                    if (MobileParty.MainParty == null)
                    {
                        OriginLog.Warning("[SlaveEscape][Finalize] MobileParty.MainParty 为空，延迟到 OnTick");
                        return;
                    }

                    if (Campaign.Current == null)
                    {
                        OriginLog.Warning("[SlaveEscape][Finalize] Campaign.Current 为空，延迟到 OnTick");
                        return;
                    }

                    // 执行 teleport
                    bool success = PresetOriginSystem.SetSlaveEscapeStartingLocation(
                        MobileParty.MainParty,
                        direction,
                        OriginSystemHelper.PendingStartSettlementId
                    );

                    if (success)
                    {
                        // 只有成功才清空 Pending
                        OriginSystemHelper.PendingStartDirection = null;
                        OriginSystemHelper.PendingStartSettlementId = null;
                        OriginLog.Info("[SlaveEscape][Finalize] 出生位置设置成功，已清空 Pending");
                    }
                    else
                    {
                        OriginLog.Warning("[SlaveEscape][Finalize] 出生位置设置失败，保留 Pending 以便 OnTick 重试");
                    }
                }
                catch (Exception ex)
                {
                    OriginLog.Error($"[SlaveEscape][Finalize] Postfix 异常: {ex.Message}");
                    OriginLog.Error($"[SlaveEscape][Finalize] StackTrace: {ex.StackTrace}");
                }
            }
        }

        // 以下非预设菜单代码已移动到 Menus/NonPreset/Optional/ 目录下
        // CreateNonPresetMinorClanPositionMenu, CreateNonPresetRefugeeBurdenMenu,
        // CreateNonPresetStartLocationTypeMenu, CreateNonPresetValuableItemMenu, CreateNonPresetContactTypeMenu
        // 及其所有回调方法已移动到对应文件

    }
}

