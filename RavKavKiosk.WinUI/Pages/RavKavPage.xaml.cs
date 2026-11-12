using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.System;

namespace RavKavKiosk.WinUI.Pages;

public sealed partial class RavKavPage : Page
{
    public RavKavPage() => InitializeComponent();

    private async void OpenSite_Click(object sender, RoutedEventArgs e)
    {
        await Launcher.LaunchUriAsync(new Uri("https://ravkavonline.co.il/"));
    }
}
