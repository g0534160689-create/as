using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using RavKavKiosk.WinUI.Services;
using System.Text.Json;

namespace RavKavKiosk.WinUI.Pages;

public sealed partial class RavKavPage : Page
{
    private AppSettings _settings = LocalSettingsStore.Load();
    private string? _selectedSelector;
    private bool _initialized;
    private bool _inspecting;

    public RavKavPage()
    {
        InitializeComponent();
        Browser.Loaded += Browser_Loaded;
    }

    private async void Browser_Loaded(object sender, RoutedEventArgs e)
    {
        if (_initialized) return;
        _initialized = true;
        try
        {
            await Browser.EnsureCoreWebView2Async();
            var core = Browser.CoreWebView2;
            core.Settings.AreDevToolsEnabled = false;
            core.Settings.AreDefaultContextMenusEnabled = false;
            core.NavigationStarting += Core_NavigationStarting;
            core.NewWindowRequested += Core_NewWindowRequested;
            core.WebMessageReceived += Core_WebMessageReceived;
            core.DOMContentLoaded += Core_DOMContentLoaded;
            BrowserStatus.Text = "הדפדפן המובנה מוכן";
            Navigate(_settings.StartPage);
            ActivityLogStore.Write("Browser", "סביבת WebView2 הופעלה");
        }
        catch (Exception ex)
        {
            BrowserStatus.Text = "לא ניתן להפעיל את רכיב הגלישה: " + ex.Message;
            ActivityLogStore.Write("Browser", ex.Message, "Error");
        }
    }

    private void Navigate(string address)
    {
        if (!_initialized || Browser.CoreWebView2 is null) return;
        if (!SitePolicy.IsAllowed(address, _settings, out var reason))
        {
            BrowserStatus.Text = "הניווט נחסם: " + reason;
            ActivityLogStore.Write("Policy", $"נחסם ניווט אל {address}: {reason}", "Warning");
            return;
        }
        Browser.CoreWebView2.Navigate(address);
    }

    private void Core_NavigationStarting(object? sender, CoreWebView2NavigationStartingEventArgs e)
    {
        if (SitePolicy.IsAllowed(e.Uri, _settings, out var reason))
        {
            BrowserStatus.Text = e.Uri;
            return;
        }

        e.Cancel = true;
        BrowserStatus.Text = "הניווט נחסם: " + reason;
        ActivityLogStore.Write("Policy", $"נחסם ניווט אל {e.Uri}: {reason}", "Warning");
    }

    private void Core_NewWindowRequested(object? sender, CoreWebView2NewWindowRequestedEventArgs e)
    {
        e.Handled = true;
        if (!_settings.AllowPopups)
        {
            ActivityLogStore.Write("Popup", $"חלון קופץ נחסם: {e.Uri}", "Warning");
            BrowserStatus.Text = "חלון קופץ נחסם לפי הגדרות הניהול";
            return;
        }

        if (SitePolicy.IsAllowed(e.Uri, _settings, out var reason))
        {
            ActivityLogStore.Write("Popup", $"חלון קופץ אושר: {e.Uri}");
            Navigate(e.Uri);
        }
        else
        {
            ActivityLogStore.Write("Popup", $"חלון קופץ נחסם: {e.Uri} ({reason})", "Warning");
            BrowserStatus.Text = "חלון קופץ נחסם: " + reason;
        }
    }

    private async void Core_DOMContentLoaded(object? sender, CoreWebView2DOMContentLoadedEventArgs e)
    {
        await ApplyHiddenElementsAsync();
        if (_inspecting) await SetInspectorAsync(true);
    }

    private void Core_WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
    {
        try
        {
            _selectedSelector = e.TryGetWebMessageAsString();
            SelectedElementText.Text = string.IsNullOrWhiteSpace(_selectedSelector)
                ? "לא ניתן לזהות את האלמנט"
                : "נבחר: " + _selectedSelector;
        }
        catch (InvalidOperationException) { }
    }

    private async void Inspect_Click(object sender, RoutedEventArgs e)
    {
        if (Browser.CoreWebView2 is null) return;
        _inspecting = !_inspecting;
        await SetInspectorAsync(_inspecting);
        (sender as Button)!.Content = _inspecting ? "סיים בחירת אלמנט" : "התחל בחירת אלמנט";
        BrowserStatus.Text = _inspecting ? "לחץ על אלמנט בדף כדי לבחור אותו" : "מצב בחירת אלמנטים הסתיים";
    }

    private async Task SetInspectorAsync(bool enabled)
    {
        if (Browser.CoreWebView2 is null) return;
        var script = """
        (() => {
          if (window.__daatInspectorCleanup) window.__daatInspectorCleanup();
          if (!ENABLED) return;
          const path = el => {
            if (el.id) return '#' + CSS.escape(el.id);
            const parts = [];
            while (el && el.nodeType === 1 && el !== document.documentElement) {
              let part = el.tagName.toLowerCase();
              if (el.classList.length) part += '.' + [...el.classList].slice(0,2).map(CSS.escape).join('.');
              const siblings = el.parentElement ? [...el.parentElement.children].filter(x => x.tagName === el.tagName) : [];
              if (siblings.length > 1) part += ':nth-of-type(' + (siblings.indexOf(el)+1) + ')';
              parts.unshift(part); el = el.parentElement;
            }
            return parts.join(' > ');
          };
          const click = ev => {
            ev.preventDefault(); ev.stopPropagation();
            const el = ev.target; const selector = path(el);
            el.style.outline = '3px solid #00a896';
            window.chrome.webview.postMessage(selector);
          };
          document.addEventListener('click', click, true);
          window.__daatInspectorCleanup = () => document.removeEventListener('click', click, true);
        })();
        """.Replace("ENABLED", enabled ? "true" : "false");
        await Browser.CoreWebView2.ExecuteScriptAsync(script);
    }

    private async Task ApplyHiddenElementsAsync()
    {
        if (Browser.CoreWebView2 is null) return;
        var selectors = JsonSerializer.Serialize(_settings.HiddenElementSelectors);
        var script = """
        (() => {
          const selectors = __SELECTORS__;
          for (const selector of selectors) {
            try {
              document.querySelectorAll(selector).forEach(el => {
                if (!el.hasAttribute('data-daat-hidden')) {
                  el.setAttribute('data-daat-hidden', '1');
                  el.dataset.daatDisplay = el.style.getPropertyValue('display');
                  el.dataset.daatPriority = el.style.getPropertyPriority('display');
                }
                el.style.setProperty('display', 'none', 'important');
              });
            } catch { }
          }
        })();
        """.Replace("__SELECTORS__", selectors);
        await Browser.CoreWebView2.ExecuteScriptAsync(script);
    }

    private async void HideSelected_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_selectedSelector)) return;
        if (!_settings.HiddenElementSelectors.Contains(_selectedSelector, StringComparer.Ordinal))
            _settings.HiddenElementSelectors.Add(_selectedSelector);
        SaveSettings("הוסתר אלמנט: " + _selectedSelector);
        await ApplyHiddenElementsAsync();
        SelectedElementText.Text = "האלמנט נשמר ברשימת ההסתרה";
    }

    private async void ShowAll_Click(object sender, RoutedEventArgs e)
    {
        _settings.HiddenElementSelectors.Clear();
        SaveSettings("נוקו כל כללי הסתרת האלמנטים");
        if (Browser.CoreWebView2 is not null)
            await Browser.CoreWebView2.ExecuteScriptAsync("""
            document.querySelectorAll('[data-daat-hidden="1"]').forEach(el => {
              el.style.removeProperty('display');
              if (el.dataset.daatDisplay) el.style.setProperty('display', el.dataset.daatDisplay, el.dataset.daatPriority || '');
              delete el.dataset.daatDisplay;
              delete el.dataset.daatPriority;
              el.removeAttribute('data-daat-hidden');
            });
            """);
        SelectedElementText.Text = "רשימת האלמנטים המוסתרים נוקתה";
    }

    private void SaveSettings(string message)
    {
        try
        {
            LocalSettingsStore.Save(_settings);
            ActivityLogStore.Write("VisualEditor", message);
        }
        catch (Exception ex)
        {
            BrowserStatus.Text = "שמירת ההגדרות נכשלה: " + ex.Message;
            ActivityLogStore.Write("Settings", ex.Message, "Error");
        }
    }

    private void Back_Click(object sender, RoutedEventArgs e)
    {
        if (Browser.CoreWebView2?.CanGoBack == true) Browser.CoreWebView2.GoBack();
    }

    private void Refresh_Click(object sender, RoutedEventArgs e) => Browser.CoreWebView2?.Reload();
}
