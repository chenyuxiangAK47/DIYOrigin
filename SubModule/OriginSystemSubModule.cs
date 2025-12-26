using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem;
namespace OriginSystemMod
{
    /// <summary>
    /// 出身系统 Mod 主入口
    /// 按照 ChatGPT 教诲：代码结构清晰、使用 Harmony Patch、详细日志
    /// </summary>
    public class OriginSystemSubModule : MBSubModuleBase
    {
        private Harmony _harmony;
        private const string BUILD_ID = "2025-12-20-001";

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            try
            {
                // 日志：DLL 加载信息
                var thisAssembly = typeof(OriginSystemSubModule).Assembly;
                var location = thisAssembly.Location;
                var version = thisAssembly.GetName().Version;
                var fileTime = System.IO.File.Exists(location)
                    ? System.IO.File.GetLastWriteTime(location).ToString("yyyy-MM-dd HH:mm:ss")
                    : "FILE NOT FOUND";

                Debug.Print("[OriginSystem] ========== DLL 加载信息 ==========", 0, Debug.DebugColor.Yellow);
                Debug.Print("[OriginSystem] BUILD=" + BUILD_ID, 0, Debug.DebugColor.Yellow);
                Debug.Print("[OriginSystem] DLL Location: " + location, 0, Debug.DebugColor.Yellow);
                Debug.Print("[OriginSystem] DLL Version: " + (version != null ? version.ToString() : "null"), 0, Debug.DebugColor.Yellow);
                Debug.Print("[OriginSystem] DLL LastWriteTime: " + fileTime, 0, Debug.DebugColor.Yellow);
                Debug.Print("[OriginSystem] =====================================", 0, Debug.DebugColor.Yellow);

                // 初始化 Harmony
                _harmony = new Harmony("OriginSystemMod");
                
                // 先尝试 PatchAll，如果失败则手动注册其他 Patch
                try
                {
                    _harmony.PatchAll(typeof(OriginSystemMod.OriginSystemPatches).Assembly);
                    Debug.Print("[OriginSystem] Harmony PatchAll 完成", 0, Debug.DebugColor.Green);
                }
                catch (Exception patchEx)
                {
                    Debug.Print($"[OriginSystem] Harmony PatchAll 部分失败: {patchEx.Message}", 0, Debug.DebugColor.Yellow);
                    Debug.Print($"[OriginSystem] 尝试手动注册其他 Patch...", 0, Debug.DebugColor.Yellow);
                    
                    // 如果 PatchAll 失败，尝试手动注册其他 Patch（除了 OnCharacterCreationFinalizedPatch）
                    // 这样可以确保路由相关的 Patch 仍然能工作
                    try
                    {
                        var assembly = typeof(OriginSystemMod.OriginSystemPatches).Assembly;
                        var patchTypes = assembly.GetTypes()
                            .Where(t => t.IsClass && 
                                       t.GetCustomAttributes(typeof(HarmonyPatch), false).Length > 0 &&
                                       t.Name != "OnCharacterCreationFinalizedPatch");
                        
                        foreach (var patchType in patchTypes)
                        {
                            try
                            {
                                _harmony.CreateClassProcessor(patchType).Patch();
                                Debug.Print($"[OriginSystem] 手动注册 Patch: {patchType.Name}", 0, Debug.DebugColor.Green);
                            }
                            catch (Exception ex)
                            {
                                Debug.Print($"[OriginSystem] 手动注册 Patch {patchType.Name} 失败: {ex.Message}", 0, Debug.DebugColor.Yellow);
                            }
                        }
                    }
                    catch (Exception manualEx)
                    {
                        Debug.Print($"[OriginSystem] 手动注册 Patch 失败: {manualEx.Message}", 0, Debug.DebugColor.Red);
                    }
                }

                Debug.Print("[OriginSystem] OnSubModuleLoad 执行完成", 0, Debug.DebugColor.Green);
            }
            catch (Exception ex)
            {
                Debug.Print("[OriginSystem] OnSubModuleLoad 失败: " + ex.Message, 0, Debug.DebugColor.Red);
                Debug.Print("[OriginSystem] StackTrace: " + ex.StackTrace, 0, Debug.DebugColor.Red);
            }
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();
            
            // 重置状态（每次进入主菜单时）
            // 但如果还在预设出身流程中，不要清空状态（方案2：在调用处加 guard）
            if (!OriginSystemHelper.IsPresetOrigin && string.IsNullOrEmpty(OriginSystemHelper.PendingStartDirection))
            {
                OriginSystemHelper.ResetState();
                Debug.Print("[OriginSystem] 状态已重置", 0, Debug.DebugColor.Green);
            }
            else
            {
                OriginLog.Info($"[OnBeforeInitialModuleScreenSetAsRoot] SKIP ResetState: IsPresetOrigin={OriginSystemHelper.IsPresetOrigin} PendingStartDirection={OriginSystemHelper.PendingStartDirection ?? "null"}");
            }
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarter)
        {
            base.OnGameStart(game, gameStarter);

            if (game.GameType is Campaign)
            {
                // 添加 CampaignBehavior
                ((CampaignGameStarter)gameStarter).AddBehavior(new OriginSystemCampaignBehavior());
                Debug.Print("[OriginSystem] CampaignBehavior 已添加", 0, Debug.DebugColor.Green);
                
                // 添加调试监视器（用于对照实验）
                ((CampaignGameStarter)gameStarter).AddBehavior(new OriginSystemDebugBehavior());
                Debug.Print("[OriginSystem] DebugBehavior 已添加（监视器已启用）", 0, Debug.DebugColor.Green);
            }
        }
    }
}



















