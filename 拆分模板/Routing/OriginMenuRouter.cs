// Routing/OriginMenuRouter.cs
// 核心路由逻辑，决定菜单流转

using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.CharacterCreationContent;

namespace OriginSystemMod
{
    /// <summary>
    /// 菜单路由核心逻辑
    /// 根据当前菜单ID和选项ID决定下一个菜单
    /// </summary>
    public static class OriginMenuRouter
    {
        /// <summary>
        /// 根据当前菜单ID和选项ID解析下一个菜单ID
        /// </summary>
        public static string ResolveNextMenuId(string curMenuId, string optId)
        {
            if (string.IsNullOrEmpty(curMenuId) || string.IsNullOrEmpty(optId))
            {
                OriginLog.Warning($"ResolveNextMenuId: 参数为空 cur={curMenuId} opt={optId}");
                return null;
            }

            // ====== A) 出身类型选择：分流预设/非预设 ======
            if (curMenuId == "origin_type_selection")
            {
                if (optId == "preset_origin_option")
                    return "preset_origin_selection";
                if (optId == "non_preset_origin_option")
                    return "non_preset_social_origin"; // 直接跳过文化选择
                return null;
            }
            
            // ====== B) 预设出身选择：按选项分流到不同 Node1 ======
            if (curMenuId == "preset_origin_selection")
            {
                return KhuzaitRoutes.GetPresetOriginNode1(optId);
            }
            
            // ====== C) 非预设社会出身：根据选项分流到不同的风味节点 ======
            if (curMenuId == "non_preset_social_origin")
            {
                return NonPresetRoutes.GetFlavorNode(optId);
            }
            
            // ====== D) 非预设风味节点：选择后进入技能背景菜单 ======
            if (NonPresetRoutes.IsFlavorNode(curMenuId))
            {
                return "non_preset_skill_background";
            }
            
            // ====== E) 非预设技能背景：选择后进入当前状态菜单 ======
            if (curMenuId == "non_preset_skill_background")
            {
                return "non_preset_starting_condition";
            }
            
            // ====== F) 非预设当前状态：选择后可以进入可选节点或直接完成 ======
            if (curMenuId == "non_preset_starting_condition")
            {
                // 可选节点菜单的InputMenuId应该设置为non_preset_starting_condition
                // 目前简化处理：直接完成，可选节点后续可以添加
                return null; // 让原版处理，通常会是narrative_parent_menu
            }
            
            // ====== G) 预设出身的节点流转 ======
            var presetNext = KhuzaitRoutes.GetPresetNodeTransition(curMenuId, optId);
            if (presetNext != null)
            {
                return presetNext;
            }
            
            // 其它菜单走原版逻辑
            return null;
        }

        /// <summary>
        /// 打印所有候选菜单（引擎原本会选谁）
        /// </summary>
        public static void DumpCandidates(CharacterCreationManager mgr, string inputId)
        {
            try
            {
                if (mgr.NarrativeMenus == null)
                {
                    OriginLog.Warning($"Candidates for input={inputId}: [0]=NONE (NarrativeMenus is null)");
                    return;
                }
                
                var candidates = mgr.NarrativeMenus
                    .Where(m => m.InputMenuId == inputId)
                    .ToList();
                
                if (candidates.Count == 0)
                {
                    OriginLog.Warning($"Candidates for input={inputId}: [0]=NONE");
                }
                else
                {
                    var candidateList = string.Join(" ", candidates.Select((m, i) => $"[{i}]={m.StringId}"));
                    OriginLog.Info($"Candidates for input={inputId}: {candidateList}");
                }
            }
            catch (Exception ex)
            {
                OriginLog.Error($"DumpCandidates 异常: {ex}");
            }
        }
    }
}

































