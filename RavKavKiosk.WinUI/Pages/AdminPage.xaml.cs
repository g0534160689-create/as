using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using RavKavKiosk.WinUI.Services;

namespace RavKavKiosk.WinUI.Pages;

public sealed partial class AdminPage : Page
{
    public AdminPage()
    {
        InitializeComponent();
        var settings = LocalSettingsStore.Load();
        AllowPopupsSwitch.IsOn = settings.AllowPopups;
        AllowExternalLinksSwitch.IsOn = settings.AllowExternalLinks;
        KeepActivitySwitch.IsOn = settings.KeepActivity;
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        LocalSettingsStore.Save(new AppSettings
        {
            AllowPopups = AllowPopupsSwitch.IsOn,
            AllowExternalLinks = AllowExternalLinksSwitch.IsOn,
            KeepActivity = KeepActivitySwitch.IsOn
        });
        SaveStatus.IsOpen = true;
    }
}
