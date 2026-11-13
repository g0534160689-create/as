using System.Text.Json;

namespace RavKavKiosk.Next.Services;

public static class SettingsStore
{
    private static readonly object Gate = new();
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };
    private static readonly string Folder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DaatAharon", "RavKavKiosk");
    public static string FilePath => Path.Combine(Folder, "settings.json");

    public static AppSettings Load()
    {
        lock (Gate)
        {
            try
            {
                if (!File.Exists(FilePath)) return new AppSettings();
                return JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(FilePath)) ?? new AppSettings();
            }
            catch
            {
                var backup = FilePath + ".bak";
                if (File.Exists(backup))
                {
                    try { return JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(backup)) ?? new AppSettings(); }
                    catch { }
                }
                return new AppSettings();
            }
        }
    }

    public static void Save(AppSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);
        lock (Gate)
        {
            Directory.CreateDirectory(Folder);
            var temp = FilePath + ".tmp";
            var json = JsonSerializer.Serialize(settings, JsonOptions);
            File.WriteAllText(temp, json);
            if (File.Exists(FilePath))
            {
                File.Copy(FilePath, FilePath + ".bak", true);
                File.Move(temp, FilePath, true);
            }
            else File.Move(temp, FilePath);
        }
    }
}
