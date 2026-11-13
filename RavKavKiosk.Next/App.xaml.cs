using Microsoft.UI.Xaml;
using RavKavKiosk.Next.Services;

namespace RavKavKiosk.Next;

public partial class App : Application
{
    private Window? _window;

    public App()
    {
        InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        try
        {
            _window = new MainWindow();
            _window.Activate();
        }
        catch (Exception ex)
        {
            StartupDiagnostics.Write(ex, "Application launch");
            _ = new Microsoft.UI.Xaml.Controls.ContentDialog
            {
                Title = "שגיאה בהפעלת התוכנה",
                Content = $"{ex.Message}\n\nיומן שגיאות: {StartupDiagnostics.LogPath}",
                CloseButtonText = "סגירה",
                XamlRoot = (_window?.Content as Microsoft.UI.Xaml.FrameworkElement)?.XamlRoot
            };
            throw;
        }
    }
}
