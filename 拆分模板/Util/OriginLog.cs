// Util/OriginLog.cs
// 统一的日志工具类，避免日志代码分散

using System;
using TaleWorlds.Core;

namespace OriginSystemMod
{
    /// <summary>
    /// 统一的日志工具，提供节流和格式化功能
    /// </summary>
    public static class OriginLog
    {
        private static DateTime _lastLogTime = DateTime.MinValue;
        private static int _logThrottleCount = 0;
        private const double LogThrottleIntervalSeconds = 0.5; // 节流间隔（秒）
        private const int MaxThrottleCount = 50; // 最大节流计数

        /// <summary>
        /// 节流日志输出（避免日志刷屏）
        /// </summary>
        public static void ThrottledLog(string message, Debug.DebugColor color = Debug.DebugColor.Green)
        {
            var now = DateTime.Now;
            var timeSinceLastLog = (now - _lastLogTime).TotalSeconds;

            if (timeSinceLastLog >= LogThrottleIntervalSeconds)
            {
                // 如果有被节流的日志，先输出计数
                if (_logThrottleCount > 0)
                {
                    Debug.Print($"[OriginSystem] (节流了 {_logThrottleCount} 条日志)", 0, Debug.DebugColor.Yellow);
                    _logThrottleCount = 0;
                }

                Debug.Print($"[OriginSystem] {message}", 0, color);
                _lastLogTime = now;
            }
            else
            {
                _logThrottleCount++;
                if (_logThrottleCount >= MaxThrottleCount)
                {
                    Debug.Print($"[OriginSystem] 日志节流达到上限 ({MaxThrottleCount})，停止输出", 0, Debug.DebugColor.Red);
                }
            }
        }

        /// <summary>
        /// 标准信息日志（固定格式）
        /// </summary>
        public static void Info(string message)
        {
            Debug.Print($"[OS] {message}", 0, Debug.DebugColor.Cyan);
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        public static void Error(string message)
        {
            Debug.Print($"[OS][ERR] {message}", 0, Debug.DebugColor.Red);
        }

        /// <summary>
        /// 警告日志
        /// </summary>
        public static void Warning(string message)
        {
            Debug.Print($"[OS][WARN] {message}", 0, Debug.DebugColor.Yellow);
        }

        /// <summary>
        /// 路由日志 - 点击时
        /// </summary>
        public static void LogClick(string menuId, string optId, bool hasSelected)
        {
            Info($"Click: menu={menuId} opt={optId} selected={hasSelected}");
        }

        /// <summary>
        /// 路由日志 - 路由前
        /// </summary>
        public static void LogRouteBefore(string curMenuId, string optId, List<string> candidates)
        {
            var candidateList = candidates != null && candidates.Count > 0 
                ? string.Join(",", candidates) 
                : "NONE";
            Info($"Route: cur={curMenuId} opt={optId} candidates=[{candidateList}]");
        }

        /// <summary>
        /// 路由日志 - 路由后
        /// </summary>
        public static void LogRouteAfter(string nextMenuId, bool success)
        {
            Info($"Route: resolved={nextMenuId ?? "null"} switched={success}");
        }
    }
}
















































