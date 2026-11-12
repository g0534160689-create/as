using System.Text;

namespace RavKavKiosk.WinUI.Services;

public static class ActivityLogExport
{
    public static string ExportCsv(IEnumerable<ActivityEntry> entries)
    {
        var folder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "DaatAharon", "RavKavKiosk", "Exports");
        Directory.CreateDirectory(folder);

        var path = Path.Combine(folder, $"activity-{DateTime.Now:yyyyMMdd-HHmmss}.csv");
        var builder = new StringBuilder();
        builder.AppendLine("Timestamp,Level,Category,Message");
        foreach (var entry in entries.OrderBy(x => x.Timestamp))
        {
            builder.Append(Csv(entry.Timestamp.ToString("yyyy-MM-dd HH:mm:ss")));
            builder.Append(',').Append(Csv(entry.Level));
            builder.Append(',').Append(Csv(entry.Category));
            builder.Append(',').Append(Csv(entry.Message));
            builder.AppendLine();
        }

        File.WriteAllText(path, builder.ToString(), new UTF8Encoding(true));
        return path;
    }

    private static string Csv(string? value) =>
        "\"" + (value ?? string.Empty).Replace("\"", "\"\"") + "\"";
}
