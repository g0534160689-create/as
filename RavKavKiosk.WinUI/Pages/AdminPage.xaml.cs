using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using RavKavKiosk.WinUI.Services;

namespace RavKavKiosk.WinUI.Pages;

public sealed partial class AdminPage : Page
{
    public AdminPage()
    {
        InitializeComponent();
        LoadSettings();
    }

    private void LoadSettings()
    {
        var settings = LocalSettingsStore.Load();
        AllowPopupsSwitch.IsOn = settings.AllowPopups;
        AllowExternalLinksSwitch.IsOn = settings.AllowExternalLinks;
        KeepActivitySwitch.IsOn = settings.KeepActivity;
        BlacklistText.Text = string.Join(Environment.NewLine, settings.Blacklist);
        WhitelistText.Text = string.Join(Environment.NewLine, settings.Whitelist);
        SaveStatus.IsOpen = false;
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var settings = new AppSettings
            {
                AllowPopups = AllowPopupsSwitch.IsOn,
                AllowExternalLinks = AllowExternalLinksSwitch.IsOn,
                KeepActivity = KeepActivitySwitch.IsOn,
                Blacklist = ParseDomains(BlacklistText.Text),
                Whitelist = ParseDomains(WhitelistText.Text)
            };

            LocalSettingsStore.Save(settings);
            SaveStatus.Severity = InfoBarSeverity.Success;
            SaveStatus.Title = "כל ההגדרות נשמרו במחשב";
            SaveStatus.Message = $"נשמרו {settings.Blacklist.Count} דומיינים ברשימה השחורה ו-{settings.Whitelist.Count} ברשימה הלבנה.";
            SaveStatus.IsOpen = true;
        }
        catch (Exception ex)
        {
            SaveStatus.Severity = InfoBarSeverity.Error;
            SaveStatus.Title = "שמירת ההגדרות נכשלה";
            SaveStatus.Message = ex.Message;
            SaveStatus.IsOpen = true;
        }
    }

    private void Reload_Click(object sender, RoutedEventArgs e) => LoadSettings();

    private static List<string> ParseDomains(string? text)
    {
        var domains = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var raw in (text ?? string.Empty).Split(
                     new[] { '\\r', '\\n', ',', ';', ' ', '\\t' },
                     StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var domain = raw.Trim().TrimEnd('.');
            if (domain.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                domain.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ||
                domain.Contains('/') || domain.Contains(':') || domain.Contains('*'))
                throw new InvalidDataException($"יש להזין שם מתחם בלבד, ללא פרוטוקול או נתיב: {raw}");

            if (domain.Length > 253 ||
                !System.Text.RegularExpressions.Regex.IsMatch(
                    domain,
                    @"^(?=.{1,253}$)(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?\\.)+[a-zA-Z]{2,63}$"))
                throw new InvalidDataException($"שם מתחם לא תקין: {raw}");

            domains.Add(domain.ToLowerInvariant());
        }

        return domains.OrderBy(x => x, StringComparer.OrdinalIgnoreCase).ToList();
    }
}
