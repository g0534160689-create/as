using Microsoft.UI.Xaml;
using RavKavKiosk.WinUI.Services;
using System.Runtime.InteropServices;

namespace RavKavKiosk.WinUI;

public partial class App : Application
{
    private Window? _window;

    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int MessageBoxW(IntPtr hWnd, string text, string caption, uint type);

    public App()
    {
        InitializeComponent();
        UnhandledException += App_UnhandledException;
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
            MessageBoxW(IntPtr.Zero,
                "לא ניתן להפעיל את התוכנה. פרטי התקלה נשמרו בקובץ:\n" + StartupDiagnostics.LogPath +
                "\n\n" + ex.Message,
                "אוצר החכמה – עמדת רב־קו", 0x10);
        }
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        StartupDiagnostics.Write(e.Exception, "Unhandled WinUI exception");
    }
}
