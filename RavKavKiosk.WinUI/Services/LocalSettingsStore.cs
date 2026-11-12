using System.Text.Json;

namespace RavKavKiosk.WinUI.Services;

public static class LocalSettingsStore
{
    private static readonly string Folder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "DaatAharon", "RavKavKiosk");
    private static readonly string FilePath = Path.Combine(Folder, "settings.json");

    public static AppSettings Load()
    {
        try
        {
            if (!File.Exists(FilePath)) return new AppSettings();
            return JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(FilePath)) ?? new AppSettings();
        }
        catch
        {
            return new AppSettings();
        }
    }

    public static void Save(AppSettings settings)
    {
        Directory.CreateDirectory(Folder);
        var temp = FilePath + ".tmp";
        var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(temp, json);
        _ = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(temp))
            ?? throw new InvalidDataException("Settings validation failed.");
        File.Move(temp, FilePath, true);
    }
}
