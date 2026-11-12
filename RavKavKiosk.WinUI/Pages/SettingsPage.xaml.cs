using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using RavKavKiosk.WinUI.Services;

namespace RavKavKiosk.WinUI.Pages;

public sealed partial class SettingsPage : Page
{
    private bool _loading;

    public SettingsPage()
    {
        InitializeComponent();
        _loading = true;
        var settings = LocalSettingsStore.Load();
        ThemeCombo.SelectedIndex = settings.Theme == "Light" ? 1 : 0;
        SettingsPathText.Text = "קובץ הגדרות: " + LocalSettingsStore.SettingsPath;
        _loading = false;
    }

    private void ThemeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_loading || ThemeCombo.SelectedItem is not ComboBoxItem item || item.Tag is not string theme) return;
        var settings = LocalSettingsStore.Load();
        settings.Theme = theme;
        LocalSettingsStore.Save(settings);
        ActivityLogStore.Write("Settings", "ערכת נושא שונתה ל-" + theme);
        SettingsStatus.Severity = InfoBarSeverity.Success;
        SettingsStatus.Title = "ההעדפה נשמרה";
        SettingsStatus.Message = "העיצוב החדש יופעל בהפעלה הבאה.";
        SettingsStatus.IsOpen = true;
    }

    private void Backup_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            LocalSettingsStore.Save(LocalSettingsStore.Load());
            ActivityLogStore.Write("Backup", "נוצר גיבוי הגדרות");
            SettingsStatus.Severity = InfoBarSeverity.Success;
            SettingsStatus.Title = "הגיבוי נוצר";
            SettingsStatus.Message = "נשמר עותק settings.json.bak.";
        }
        catch (Exception ex)
        {
            SettingsStatus.Severity = InfoBarSeverity.Error;
            SettingsStatus.Title = "יצירת הגיבוי נכשלה";
            SettingsStatus.Message = ex.Message;
        }
        SettingsStatus.IsOpen = true;
    }
}
