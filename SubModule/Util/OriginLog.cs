using System;
using System.IO;
using TaleWorlds.Library;

public static class OriginLog
{
    private const string Prefix = "[OriginSystem] ";

    // 可选：写文件（方便你看日志）
    private static readonly string LogPath =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), 
                     "Mount and Blade II Bannerlord", "Logs", "origin_system.log");

    public static void Info(string msg) => Write("INFO", msg);
    public static void Warn(string msg) => Write("WARN", msg);
    public static void Warning(string msg) => Write("WARN", msg); // 兼容旧代码
    public static void Error(string msg) => Write("ERR ", msg);

    public static void Exception(Exception ex, string context = null)
        => Write("EXCP", (context == null ? "" : context + " | ") + ex);

    public static void ThrottledLog(string message, Debug.DebugColor color = Debug.DebugColor.Green)
    {
        Write("INFO", message);
    }

    private static void Write(string level, object msg)
    {
        string line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {level} {Prefix}{msg}";
        try { Debug.Print(line); } catch { /* ignore */ }

        // 文件日志失败也不要影响游戏
        try { File.AppendAllText(LogPath, line + Environment.NewLine); } catch { /* ignore */ }
    }
}
