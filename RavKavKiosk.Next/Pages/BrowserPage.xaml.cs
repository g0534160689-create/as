using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using RavKavKiosk.Next.Services;

namespace RavKavKiosk.Next.Pages;

public sealed partial class BrowserPage : Page
{
    private readonly WebView2 _webView = new();
    private bool _ready;

    public BrowserPage()
    {
        InitializeComponent();
        BrowserHost.Children.Add(_webView);
        Loaded += BrowserPage_Loaded;
    }

    private async void BrowserPage_Loaded(object sender, RoutedEventArgs e)
    {
        if (_ready) return;
        try
        {
            await _webView.EnsureCoreWebView2Async();
            var core = _webView.CoreWebView2;
            core.Settings.AreDevToolsEnabled = false;
            core.Settings.AreDefaultContextMenusEnabled = false;
            core.NavigationStarting += (_, args) =>
            {
                var settings = SettingsStore.Load();
                if (!SitePolicy.IsAllowed(args.Uri, settings, out var reason))
                {
                    args.Cancel = true;
                    DispatcherQueue.TryEnqueue(() => BrowserStatus.Text = "הניווט נחסם: " + reason);
                    ActivityStore.Write("Policy", $"חסימת {args.Uri}: {reason}");
                }
            };
            core.NewWindowRequested += (sender, args) =>
            {
                args.Handled = true;
                var settings = SettingsStore.Load();
                if (settings.AllowPopups && SitePolicy.IsAllowed(args.Uri, settings, out _))
                    core.Navigate(args.Uri);
                else
                    ActivityStore.Write("Popup", $"חלון קופץ נדחה: {args.Uri}");
            };
            _ready = true;
            BrowserStatus.Text = "הדפדפן מוכן";
            var start = SettingsStore.Load().StartUrl;
            AddressBox.Text = start;
            Navigate(start);
        }
        catch (Exception ex)
        {
            BrowserStatus.Text = "רכיב הדפדפן לא זמין: " + ex.Message;
            ActivityStore.Write("BrowserError", ex.ToString());
        }
    }

    private void Go_Click(object sender, RoutedEventArgs e) => Navigate(AddressBox.Text);

    private void Navigate(string input)
    {
        if (!_ready) return;
        var value = input.Trim();
        if (!value.Contains("://", StringComparison.Ordinal)) value = "https://" + value;
        if (!Uri.TryCreate(value, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttps && uri.Scheme != Uri.UriSchemeHttp))
        {
            BrowserStatus.Text = "כתובת לא תקינה";
            return;
        }

        var settings = SettingsStore.Load();
        if (!SitePolicy.IsAllowed(uri.ToString(), settings, out var reason))
        {
            BrowserStatus.Text = "הניווט נחסם: " + reason;
            ActivityStore.Write("Policy", $"חסימת {uri}: {reason}");
            return;
        }

        AddressBox.Text = uri.ToString();
        _webView.CoreWebView2.Navigate(uri.ToString());
    }
}
