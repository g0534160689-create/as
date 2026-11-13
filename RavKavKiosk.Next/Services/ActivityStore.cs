using System.Text.Json;

namespace RavKavKiosk.Next.Services;

public sealed record ActivityEntry(DateTimeOffset Timestamp, string Category, string Message);

public static class ActivityStore
{
    private static readonly object Gate = new();
    private static readonly string Folder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DaatAharon", "RavKavKiosk");
    private static string LogPath => Path.Combine(Folder, "activity.jsonl");

    public static void Write(string category, string message)
    {
        try
        {
            lock (Gate)
            {
                Directory.CreateDirectory(Folder);
                File.AppendAllText(LogPath, JsonSerializer.Serialize(new ActivityEntry(DateTimeOffset.Now, category, message)) + Environment.NewLine);
            }
        }
        catch { /* Logging must never crash the application. */ }
    }

    public static IReadOnlyList<ActivityEntry> Read(int limit)
    {
        lock (Gate)
        {
            if (!File.Exists(LogPath)) return [];
            return File.ReadLines(LogPath).Reverse().Take(Math.Clamp(limit, 1, 5000))
                .Select(line => { try { return JsonSerializer.Deserialize<ActivityEntry>(line); } catch { return null; } })
                .Where(x => x is not null).Cast<ActivityEntry>().ToList();
        }
    }
}
