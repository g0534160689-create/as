using System.Text;

namespace RavKavKiosk.WinUI.Services;

public static class StartupDiagnostics
{
    public static string LogPath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "DaatAharon", "RavKavKiosk", "startup-errors.log");

    public static void Write(Exception exception, string stage)
    {
        try
        {
            var folder = Path.GetDirectoryName(LogPath)!;
            Directory.CreateDirectory(folder);
            var entry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {stage}{Environment.NewLine}{exception}{Environment.NewLine}{new string('-', 72)}{Environment.NewLine}";
            File.AppendAllText(LogPath, entry, new UTF8Encoding(true));
        }
        catch { }
    }
}
