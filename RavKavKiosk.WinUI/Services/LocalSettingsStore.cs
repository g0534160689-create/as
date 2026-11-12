using System.Text.Json;

namespace RavKavKiosk.WinUI.Services;

public static class LocalSettingsStore
{
    private static readonly string Folder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "DaatAharon", "RavKavKiosk");
    private static readonly string FilePath = Path.Combine(Folder, "settings.json");
    private static readonly string BackupPath = FilePath + ".bak";
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true, PropertyNameCaseInsensitive = true };

    public static string SettingsPath => FilePath;

    public static AppSettings Load()
    {
        Directory.CreateDirectory(Folder);
        var loaded = TryRead(FilePath) ?? TryRead(BackupPath) ?? new AppSettings();
        loaded.Blacklist ??= [];
        loaded.Whitelist ??= [];
        loaded.HiddenElementSelectors ??= [];
        loaded.Theme = loaded.Theme is "Dark" or "Light" ? loaded.Theme : "Dark";
        return loaded;
    }

    public static void Save(AppSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);
        Directory.CreateDirectory(Folder);
        settings.Blacklist ??= [];
        settings.Whitelist ??= [];
        settings.HiddenElementSelectors ??= [];
        var json = JsonSerializer.Serialize(settings, JsonOptions);
        _ = JsonSerializer.Deserialize<AppSettings>(json, JsonOptions)
            ?? throw new InvalidDataException("Settings validation failed.");

        var temp = FilePath + ".tmp";
        File.WriteAllText(temp, json);
        if (File.Exists(FilePath)) File.Copy(FilePath, BackupPath, true);
        File.Move(temp, FilePath, true);
    }

    private static AppSettings? TryRead(string path)
    {
        try
        {
            return File.Exists(path)
                ? JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(path), JsonOptions)
                : null;
        }
        catch (JsonException) { return null; }
        catch (IOException) { return null; }
        catch (UnauthorizedAccessException) { return null; }
    }
}
