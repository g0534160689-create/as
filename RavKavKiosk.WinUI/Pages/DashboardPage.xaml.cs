using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace RavKavKiosk.WinUI.Pages;

public sealed partial class DashboardPage : Page
{
    public DashboardPage() => InitializeComponent();

    private void OpenRavKav_Click(object sender, RoutedEventArgs e) =>
        Frame?.Navigate(typeof(RavKavPage));

    private void OpenAdmin_Click(object sender, RoutedEventArgs e) =>
        Frame?.Navigate(typeof(AdminPage));

    private void OpenActivity_Click(object sender, RoutedEventArgs e) =>
        Frame?.Navigate(typeof(ActivityPage));
}
