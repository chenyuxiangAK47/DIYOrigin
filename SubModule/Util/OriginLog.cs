using System;
using System.IO;
using System.Linq;
using TaleWorlds.Library;

public static class OriginLog
{
    private const string Prefix = "[OriginSystem] ";

    // 直接写入到 ProgramData 的日志目录（和 rgl_log 同一个目录）
    private static readonly string ProgramDataLogDir = 
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                     "Mount and Blade II Bannerlord", "logs");
    
    private static readonly string ProgramDataLogPath = 
        Path.Combine(ProgramDataLogDir, "origin_system.log");

    // 备用：MyDocuments 目录
    private static readonly string MyDocumentsLogPath =
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
        // 格式：匹配 rgl_log 的格式 [HH:mm:ss.mmm] message
        var now = DateTime.Now;
        string rglFormatLine = $"[{now:HH:mm:ss.fff}] {Prefix}{msg}";
        string detailedLine = $"{now:yyyy-MM-dd HH:mm:ss.fff} {level} {Prefix}{msg}";
        
        // 1. 使用 Debug.Print 输出到 rgl_log（使用 rgl 格式）
        try 
        { 
            Debug.Print(rglFormatLine, 0, Debug.DebugColor.White); 
        } 
        catch { /* ignore */ }

        // 2. 直接写入到 ProgramData 日志目录（和 rgl_log 同一个目录）
        try 
        {
            if (!Directory.Exists(ProgramDataLogDir))
            {
                Directory.CreateDirectory(ProgramDataLogDir);
            }
            File.AppendAllText(ProgramDataLogPath, detailedLine + Environment.NewLine);
        } 
        catch { /* ignore */ }

        // 3. 备用：写入到 MyDocuments（如果 ProgramData 失败）
        try 
        { 
            var myDocsDir = Path.GetDirectoryName(MyDocumentsLogPath);
            if (!Directory.Exists(myDocsDir))
            {
                Directory.CreateDirectory(myDocsDir);
            }
            File.AppendAllText(MyDocumentsLogPath, detailedLine + Environment.NewLine); 
        } 
        catch { /* ignore */ }
    }
}
