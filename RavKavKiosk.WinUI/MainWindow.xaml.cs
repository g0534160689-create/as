using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
using RavKavKiosk.WinUI.Pages;
using RavKavKiosk.WinUI.Services;

namespace RavKavKiosk.WinUI;

public sealed partial class MainWindow : Window
{
    private readonly NavigationView _rootNavigation = new();
    private readonly Frame _contentFrame = new();

    public MainWindow()
    {
        InitializeComponent();

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

        AddMenuItem("ראשי", "home", "\\uE80F");
        AddMenuItem("עמדת רב־קו", "ravkav", "\\uE8D7");
        AddMenuItem("ניהול מערכת", "admin", "\\uE713");
        AddMenuItem("יומן פעילות", "activity", "\\uE81C");

        _rootNavigation.Content = _contentFrame;
        RootGrid.Children.Add(_rootNavigation);

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
