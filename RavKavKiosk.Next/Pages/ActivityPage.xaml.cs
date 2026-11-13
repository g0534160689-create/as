using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using RavKavKiosk.Next.Services;

namespace RavKavKiosk.Next.Pages;

public sealed partial class ActivityPage : Page
{
    public ActivityPage()
    {
        InitializeComponent();
        Refresh();
    }

    private void Refresh() => EntriesList.ItemsSource = ActivityStore.Read(500);

    private void Refresh_Click(object sender, RoutedEventArgs e) => Refresh();
}
