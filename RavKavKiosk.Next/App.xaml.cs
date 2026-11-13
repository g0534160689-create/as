using Microsoft.UI.Xaml;
using RavKavKiosk.Next.Services;
using System.Runtime.InteropServices;

namespace RavKavKiosk.Next;

public partial class App : Application
{
    private Window? _window;

    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int MessageBoxW(IntPtr hWnd, string text, string caption, uint type);

    public App() => InitializeComponent();

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
                "לא ניתן להפעיל את התוכנה. פרטי התקלה נשמרו כאן:\n" +
                StartupDiagnostics.LogPath + "\n\n" + ex.Message,
                "עמדת שירות", 0x10);
        }
    }
}
