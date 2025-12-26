using System;
using System.Linq;
using HarmonyLib;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using OriginSystemMod;

namespace OriginSystemMod
{
    /// <summary>
    /// Harmony Patch for CharacterCreationManager.TrySwitchToNextMenu
    /// 核心路由逻辑：在引擎真正切换菜单时，根据 SelectedOptions 决定目标菜单
    /// 不再修改 OutputMenuId（可能是只读的），而是直接设置 CurrentMenu
    /// </summary>
    public static partial class OriginSystemPatches
    {
        [HarmonyPatch(typeof(CharacterCreationManager), "TrySwitchToNextMenu")]
        public static class TrySwitchToNextMenuPatch
        {
            private static bool _routing = false; // 防重入标志

            static bool Prefix(CharacterCreationManager __instance, ref bool __result)
            {
                // 防重入：如果正在路由中，直接交给原版处理
                if (_routing)
                {
                    OriginLog.Warning("Switch: REENTRY DETECTED, fallback to vanilla");
                    return true;
                }
                
                _routing = true;
                
                try
                {
                    var cm = __instance.CurrentMenu;
                    if (cm == null)
                    {
                        OriginLog.Warning("Switch: cur=NULL opt=NULL resolved=NULL (CurrentMenu is null)");
                        return true;
                    }

                    // E2. 每次 TrySwitchToNextMenu 触发时的日志（固定格式）
                    string optId = null;
                    if (__instance.SelectedOptions.TryGetValue(cm, out var opt) && opt != null)
                    {
                        optId = opt.StringId;
                    }
                    
                    // 打印候选菜单（引擎原本会选谁）- E3
                    OriginMenuRouter.DumpCandidates(__instance, cm.StringId);
                    
                    // 解析下一个菜单ID
                    string nextMenuId = OriginMenuRouter.ResolveNextMenuId(cm.StringId, optId);
                    
                    // E2 日志输出
                    OriginLog.Info($"Switch: cur={cm.StringId} opt={optId ?? "NULL"} resolved={nextMenuId ?? "NULL"}");
                    
                    if (string.IsNullOrEmpty(nextMenuId))
                    {
                        // 没有自定义路由，交给原版处理（原版会选 candidate[0]）
                        return true; // 交给原版
                    }
                    
                    // 查找目标菜单
                    var nextMenu = __instance.GetNarrativeMenuWithId(nextMenuId);
                    bool found = nextMenu != null;
                    bool setCurrent = false;
                    bool modifyCalled = false;
                    
                    if (!found)
                    {
                        OriginLog.Error($"[Route] 错误：找不到目标菜单 {nextMenuId}");
                        __result = false;
                        return false;
                    }
                    
                    // 通过反射设置 CurrentMenu
                    setCurrent = ReflectionUtil.TrySetCurrentMenu(__instance, nextMenu);
                    if (!setCurrent)
                    {
                        OriginLog.Error("[Route] 反射设置 CurrentMenu 失败");
                        __result = false;
                        return false;
                    }
                    
                    // 调用 ModifyMenuCharacters
                    ReflectionUtil.InvokeModifyMenuCharacters(__instance);
                    modifyCalled = true;
                    
                    // E4. 强制切换结果日志（固定格式）
                    OriginLog.Info($"ForceSwitch: target={nextMenuId} found={found} setCurrent={setCurrent} modifyCalled={modifyCalled}");
                    
                    __result = true;
                    return false; // 跳过原函数
                }
                catch (Exception ex)
                {
                    OriginLog.Error($"[Route] 异常: {ex}");
                    return true; // 异常时交给原版处理
                }
                finally
                {
                    _routing = false;
                }
            }
        }
    }
}

