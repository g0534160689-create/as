using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using RavKavKiosk.Next.Services;

namespace RavKavKiosk.Next.Pages;

public sealed partial class AdminPage : Page
{
    public AdminPage()
    {
        InitializeComponent();
        var settings = SettingsStore.Load();
        AllowPopupsSwitch.IsOn = settings.AllowPopups;
        UseAllowListSwitch.IsOn = settings.EnforceAllowList;
        AllowListBox.Text = string.Join(Environment.NewLine, settings.AllowedDomains);
        BlockListBox.Text = string.Join(Environment.NewLine, settings.BlockedDomains);
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var settings = SettingsStore.Load();
            settings.AllowPopups = AllowPopupsSwitch.IsOn;
            settings.EnforceAllowList = UseAllowListSwitch.IsOn;
            settings.AllowedDomains = ParseLines(AllowListBox.Text);
            settings.BlockedDomains = ParseLines(BlockListBox.Text);
            SettingsStore.Save(settings);
            ActivityStore.Write("Admin", "הגדרות ניהול נשמרו");
            StatusBar.Severity = InfoBarSeverity.Success;
            StatusBar.Title = "נשמר בהצלחה";
            StatusBar.Message = "ההגדרות נשמרו במחשב המקומי.";
        }
        catch (Exception ex)
        {
            StatusBar.Severity = InfoBarSeverity.Error;
            StatusBar.Title = "השמירה נכשלה";
            StatusBar.Message = ex.Message;
            ActivityStore.Write("Error", ex.ToString());
        }
        StatusBar.IsOpen = true;
    }

    private static List<string> ParseLines(string? value) =>
        (value ?? "").Split(new[] { '\r', '\n', ',', ';' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(x => x.Trim().TrimStart('.').ToLowerInvariant())
            .Where(x => x.Length > 0)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
}
