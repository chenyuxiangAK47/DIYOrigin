using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TaleWorlds.CampaignSystem;

namespace OriginSystemMod
{
    /// <summary>
    /// 出身系统全局辅助类
    /// 存储用户选择的出身类型和具体选项
    /// </summary>
    public static class OriginSystemHelper
    {
        // 出身类型：true = 预设出身，false = 非预设出身
        public static bool IsPresetOrigin { get; set; } = false;

        // 预设出身 ID（如 "khuzait_rebel_chief"）
        public static string SelectedPresetOriginId { get; set; } = null;

        // 非预设出身数据
        public static NonPresetOriginData NonPresetOrigin { get; set; } = new NonPresetOriginData();

        // 是否已经完成出身选择
        public static bool OriginSelectionDone { get; set; } = false;

        // 待处理的菜单切换（由 OnSelect 设置，由 Postfix 处理）
        public static string PendingMenuSwitch { get; set; } = null;

        // 预设出身的私有节点选择（如：流浪王族的子选项）
        // 格式：nodeId:choiceId（例如："khz_node_rebel_reason:refuse_tribute"）
        public static Dictionary<string, string> SelectedPresetOriginNodes { get; set; } = new Dictionary<string, string>();

        // 非预设出身的节点选择
        // 格式：nodeId:choiceId（例如："np_node_social_origin:social_origin_rural_peasant"）
        public static Dictionary<string, string> SelectedNonPresetOriginNodes { get; set; } = new Dictionary<string, string>();

        // 逃奴出身：待处理的出生位置（延迟执行）
        public static string PendingStartDirection { get; set; } = null; // "desert" 或 "empire"
        public static string PendingStartSettlementId { get; set; } = null; // 目标定居点 ID

        // minor_noble 出身：待处理的加入库塞特王国（延迟到 OnSessionLaunched 执行）
        public static bool PendingMinorNobleJoinKhuzait { get; set; } = false;

        /// <summary>
        /// 兼容获取所有英雄列表（跨版本兼容）
        /// </summary>
        public static IEnumerable<Hero> GetAllHeroesSafe()
        {
            var campaign = Campaign.Current;
            if (campaign != null)
            {
                var prop = campaign.GetType().GetProperty("AllHeroes",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (prop != null)
                {
                    if (prop.GetValue(campaign) is IEnumerable<Hero> all)
                        return all;
                }
            }

            // fallback：几乎所有版本都存在
            return Hero.AllAliveHeroes ?? Enumerable.Empty<Hero>();
        }

        /// <summary>
        /// 重置所有状态（在新游戏开始时调用）
        /// </summary>
        public static void ResetState()
        {
            // 打印调用栈（关键：确认是谁调用的）
            OriginLog.Warning($"[ResetState] called\n{Environment.StackTrace}");
            
            // 如果还在预设出身流程中，禁止清空关键字段（最小修复方案1）
            if (IsPresetOrigin || !string.IsNullOrEmpty(SelectedPresetOriginId) || !string.IsNullOrEmpty(PendingStartDirection))
            {
                OriginLog.Warning($"[ResetState] SKIP because preset flow not finished. IsPresetOrigin={IsPresetOrigin} SelectedPresetOriginId={SelectedPresetOriginId ?? "null"} PendingStartDirection={PendingStartDirection ?? "null"}");
                return;
            }
            
            // 记录清空前的状态（用于调试）
            var oldPendingDirection = PendingStartDirection;
            var oldPendingSettlementId = PendingStartSettlementId;
            
            IsPresetOrigin = false;
            SelectedPresetOriginId = null;
            NonPresetOrigin = new NonPresetOriginData();
            OriginSelectionDone = false;
            PendingMenuSwitch = null;
            SelectedPresetOriginNodes = new Dictionary<string, string>();
            SelectedNonPresetOriginNodes = new Dictionary<string, string>();
            PendingStartDirection = null;
            PendingStartSettlementId = null;
            
            // 如果清空了 PendingStartDirection，记录警告
            if (!string.IsNullOrEmpty(oldPendingDirection))
            {
                OriginLog.Warning($"[ResetState] 清空了 PendingStartDirection={oldPendingDirection} PendingStartSettlementId={oldPendingSettlementId ?? "null"}");
            }
        }
    }

    /// <summary>
    /// 非预设出身数据类
    /// 存储所有选择的结果
    /// </summary>
    public class NonPresetOriginData
    {
        // Step 0: 文化锚点
        public string CultureAnchor { get; set; } = null;

        // Step 1: 社会出身
        public string SocialOrigin { get; set; } = null;

        // 文化风味节点（根据社会出身触发）
        public string FlavorNode { get; set; } = null;

        // Step 2: 技能来源
        public string SkillBackground { get; set; } = null;

        // Step 3: 当前状态
        public string StartingCondition { get; set; } = null;

        // 可选节点
        public string StartLocationType { get; set; } = null;
        public string ValuableItem { get; set; } = null;
        public string ContactType { get; set; } = null;

        /// <summary>
        /// 检查是否已完成所有必需步骤
        /// </summary>
        public bool IsComplete()
        {
            return !string.IsNullOrEmpty(CultureAnchor) &&
                   !string.IsNullOrEmpty(SocialOrigin) &&
                   !string.IsNullOrEmpty(SkillBackground) &&
                   !string.IsNullOrEmpty(StartingCondition);
        }

        /// <summary>
        /// 检查是否需要显示风味节点
        /// </summary>
        public bool NeedsFlavorNode()
        {
            if (string.IsNullOrEmpty(SocialOrigin)) return false;
            
            // 需要风味节点的社会出身
            return SocialOrigin == "nomad_camp" ||      // 游牧营地成员
                   SocialOrigin == "urban_artisan" ||   // 城镇手工业者
                   SocialOrigin == "caravan_follower" || // 商队随行者
                   SocialOrigin == "rural_peasant" ||    // 农村平民
                   SocialOrigin == "urban_poor";         // 城镇贫民（新增）
        }
    }
}

