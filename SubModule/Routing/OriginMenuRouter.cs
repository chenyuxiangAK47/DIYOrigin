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
        /// 优先使用 PendingMenuSwitch（如果存在），否则根据 curMenuId 和 optId 路由
        /// </summary>
        public static string ResolveNextMenuId(string curMenuId, string optId)
        {
            // ====== 0) 优先检查 PendingMenuSwitch（OnSelect 中设置的目标菜单）=====
            // 这是"能跑"版本的核心：Node 的 OnSelect 只记录状态，路由由这里统一接管
            if (!string.IsNullOrEmpty(OriginSystemHelper.PendingMenuSwitch))
            {
                string targetMenuId = OriginSystemHelper.PendingMenuSwitch;
                OriginLog.Info($"[Route] 使用 PendingMenuSwitch: {targetMenuId} (cur={curMenuId} opt={optId})");
                // 清空 PendingMenuSwitch，避免影响后续路由
                OriginSystemHelper.PendingMenuSwitch = null;
                return targetMenuId;
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
                // 解析选项ID，提取社会出身ID
                // 选项ID格式: social_origin_{socialOriginId}
                if (optId == null || !optId.StartsWith("social_origin_"))
                {
                    OriginLog.Error($"[Route] non_preset_social_origin: 选项ID格式错误: {optId ?? "NULL"}");
                    return null;
                }
                
                string socialOriginId = optId.Substring("social_origin_".Length);
                
                // 检查是否需要风味节点
                OriginSystemHelper.NonPresetOrigin.SocialOrigin = socialOriginId;
                bool needsFlavorNode = OriginSystemHelper.NonPresetOrigin.NeedsFlavorNode();
                
                if (needsFlavorNode)
                {
                    // 需要风味节点，根据社会出身ID获取对应的菜单
                    string flavorMenuId = NonPresetRoutes.GetFlavorNode(optId);
                    if (!string.IsNullOrEmpty(flavorMenuId))
                    {
                        return flavorMenuId;
                    }
                    else
                    {
                        // 如果没有风味节点，直接进入技能来源菜单
                        return "non_preset_skill_background";
                    }
                }
                else
                {
                    // 不需要风味节点，直接进入技能来源菜单
                    return "non_preset_skill_background";
                }
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
            // 可选节点在OnSelect中处理，这里直接返回null让原版处理
            if (curMenuId == "non_preset_starting_condition")
            {
                // 可选节点菜单的InputMenuId应该设置为non_preset_starting_condition
                // 这样原版会找到它们，但我们可以在这里控制是否显示
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






