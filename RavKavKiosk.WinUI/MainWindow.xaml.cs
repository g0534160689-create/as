using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using RavKavKiosk.WinUI.Pages;
using RavKavKiosk.WinUI.Services;

namespace RavKavKiosk.WinUI;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        RootNavigation.RequestedTheme = LocalSettingsStore.Load().Theme == "Light" ? ElementTheme.Light : ElementTheme.Dark;
        ExtendsContentIntoTitleBar = false;
        RootNavigation.SelectedItem = RootNavigation.MenuItems[0];
        ContentFrame.Navigate(typeof(DashboardPage));
    }

    private void RootNavigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.IsSettingsSelected)
        {
            ContentFrame.Navigate(typeof(SettingsPage));
            return;
        }

        if (args.SelectedItemContainer?.Tag is not string page)
            return;

        Type? target = page switch
        {
            "home" => typeof(DashboardPage),
            "ravkav" => typeof(RavKavPage),
            "admin" => typeof(AdminPage),
            "activity" => typeof(ActivityPage),
            "settings" => typeof(SettingsPage),
            _ => null
        };

        if (target is not null && ContentFrame.CurrentSourcePageType != target)
            ContentFrame.Navigate(target);
    }
}
