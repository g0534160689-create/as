using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using RavKavKiosk.Next.Pages;

namespace RavKavKiosk.Next;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Title = "עמדת שירות";
        ContentFrame.Navigate(typeof(DashboardPage));
    }

    private void Navigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItemContainer?.Tag is not string page) return;

        var target = page switch
        {
            "dashboard" => typeof(DashboardPage),
            "browser" => typeof(BrowserPage),
            "admin" => typeof(AdminPage),
            "activity" => typeof(ActivityPage),
            _ => null
        };

        if (target is not null && ContentFrame.CurrentSourcePageType != target)
            ContentFrame.Navigate(target);
    }
}
