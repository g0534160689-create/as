using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using RavKavKiosk.WinUI.Pages;
using RavKavKiosk.WinUI.Services;

namespace RavKavKiosk.WinUI;

public sealed partial class MainWindow : Window
{
    private readonly NavigationView _rootNavigation = new();
    private readonly Frame _contentFrame = new();

    public MainWindow()
    {
        // Build the window entirely in code. This deliberately avoids loading
        // MainWindow.xaml at runtime, so a XAML resource/parser issue cannot
        // prevent the application window from opening.
        Title = "אוצר החכמה | עמדת רב־קו";
        _rootNavigation.IsBackButtonVisible = NavigationViewBackButtonVisible.Collapsed;
        _rootNavigation.IsSettingsVisible = true;
        _rootNavigation.PaneDisplayMode = NavigationViewPaneDisplayMode.Left;
        _rootNavigation.OpenPaneLength = 252;
        _rootNavigation.FlowDirection = FlowDirection.RightToLeft;
        _rootNavigation.RequestedTheme = LocalSettingsStore.Load().Theme == "Light"
            ? ElementTheme.Light
            : ElementTheme.Dark;
        _rootNavigation.SelectionChanged += RootNavigation_SelectionChanged;

        AddMenuItem("ראשי", "home", char.ConvertFromUtf32(0xE80F));
        AddMenuItem("עמדת רב־קו", "ravkav", char.ConvertFromUtf32(0xE8D7));
        AddMenuItem("ניהול מערכת", "admin", char.ConvertFromUtf32(0xE713));
        AddMenuItem("יומן פעילות", "activity", char.ConvertFromUtf32(0xE81C));

        _rootNavigation.Content = _contentFrame;
        Content = _rootNavigation;

        _rootNavigation.SelectedItem = _rootNavigation.MenuItems[0];
        Navigate(typeof(DashboardPage));
    }

    private void AddMenuItem(string title, string tag, string glyph)
    {
        _rootNavigation.MenuItems.Add(new NavigationViewItem
        {
            Content = title,
            Tag = tag,
            Icon = new FontIcon { Glyph = glyph }
        });
    }

    private void RootNavigation_SelectionChanged(
        NavigationView sender,
        NavigationViewSelectionChangedEventArgs args)
    {
        if (args.IsSettingsSelected)
        {
            Navigate(typeof(SettingsPage));
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
            _ => null
        };

        if (target is not null)
            Navigate(target);
    }

    private void Navigate(Type pageType)
    {
        if (_contentFrame.CurrentSourcePageType != pageType)
            _contentFrame.Navigate(pageType);
    }
}
