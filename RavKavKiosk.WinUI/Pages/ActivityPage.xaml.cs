using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using RavKavKiosk.WinUI.Services;

namespace RavKavKiosk.WinUI.Pages;

public sealed partial class ActivityPage : Page
{
    private IReadOnlyList<ActivityEntry> _entries = [];

    public ActivityPage()
    {
        InitializeComponent();
        Refresh();
    }

    private void Refresh()
    {
        _entries = ActivityLogStore.Read(1000);
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        var query = SearchText.Text?.Trim() ?? string.Empty;
        var filtered = _entries.Where(entry =>
            string.IsNullOrEmpty(query) ||
            entry.Category.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            entry.Message.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            entry.Level.Contains(query, StringComparison.OrdinalIgnoreCase));
        ActivityList.ItemsSource = filtered.Select(entry => new ActivityRow(
            entry.Timestamp.ToString("dd/MM/yyyy HH:mm:ss"), entry.Category, entry.Message)).ToList();
        ActivitySummary.Text = $"מוצגות {filtered.Count()} מתוך {_entries.Count} פעולות";
    }

    private void SearchText_TextChanged(object sender, TextChangedEventArgs e) => ApplyFilter();
    private void Refresh_Click(object sender, RoutedEventArgs e) => Refresh();

    private async void Clear_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new ContentDialog
        {
            Title = "לנקות את יומן הפעילות?",
            Content = "הפעולה תמחק את קובץ היומן המקומי.",
            PrimaryButtonText = "נקה יומן",
            CloseButtonText = "ביטול",
            DefaultButton = ContentDialogButton.Close,
            XamlRoot = XamlRoot
        };
        if (await dialog.ShowAsync() == ContentDialogResult.Primary)
        {
            ActivityLogStore.Clear();
            ActivityLogStore.Write("System", "יומן הפעילות נוקה");
            Refresh();
        }
    }

    private sealed record ActivityRow(string Time, string Category, string Message);
}
