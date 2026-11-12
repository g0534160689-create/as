using System.Text.Json;

namespace RavKavKiosk.WinUI.Services;

public sealed record ActivityEntry(DateTime Timestamp, string Category, string Message, string Level = "Info");

public static class ActivityLogStore
{
    private static readonly string Folder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "DaatAharon", "RavKavKiosk");
    private static readonly string FilePath = Path.Combine(Folder, "activity.jsonl");
    private static readonly object Sync = new();
    private static readonly JsonSerializerOptions Options = new() { PropertyNameCaseInsensitive = true };

    public static void Write(string category, string message, string level = "Info")
    {
        if (!LocalSettingsStore.Load().KeepActivity) return;
        try
        {
            Directory.CreateDirectory(Folder);
            var line = JsonSerializer.Serialize(new ActivityEntry(DateTime.Now, category, message, level));
            lock (Sync) File.AppendAllText(FilePath, line + Environment.NewLine);
        }
        catch (IOException) { }
        catch (UnauthorizedAccessException) { }
    }

    public static IReadOnlyList<ActivityEntry> Read(int limit = 500)
    {
        try
        {
            if (!File.Exists(FilePath)) return [];
            lock (Sync)
            {
                return File.ReadLines(FilePath)
                    .Where(line => !string.IsNullOrWhiteSpace(line))
                    .TakeLast(Math.Clamp(limit, 1, 5000))
                    .Select(line => { try { return JsonSerializer.Deserialize<ActivityEntry>(line, Options); } catch (JsonException) { return null; } })
                    .Where(entry => entry is not null)
                    .Cast<ActivityEntry>()
                    .OrderByDescending(entry => entry.Timestamp)
                    .ToList();
            }
        }
        catch (IOException) { return []; }
        catch (UnauthorizedAccessException) { return []; }
    }

    public static void Clear()
    {
        lock (Sync)
            if (File.Exists(FilePath)) File.Delete(FilePath);
    }
}
