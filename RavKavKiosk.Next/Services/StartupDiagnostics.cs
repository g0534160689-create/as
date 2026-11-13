namespace RavKavKiosk.Next.Services;

public static class StartupDiagnostics
{
    private static readonly object Gate = new();
    private static readonly string Folder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DaatAharon", "RavKavKiosk");
    public static string LogPath => Path.Combine(Folder, "startup-errors.log");

    public static void Write(Exception exception, string source)
    {
        try
        {
            lock (Gate)
            {
                Directory.CreateDirectory(Folder);
                File.AppendAllText(LogPath, $"[{DateTimeOffset.Now:O}] {source}{Environment.NewLine}{exception}{Environment.NewLine}---{Environment.NewLine}");
            }
        }
        catch { }
    }
}
