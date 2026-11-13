using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace RavKavKiosk.WinUI.Pages;

public sealed partial class DashboardPage : Page
{
    public DashboardPage()
    {
        // Dashboard is constructed in code to avoid runtime XAML loading.
        FlowDirection = FlowDirection.RightToLeft;
        var root = new ScrollViewer();
        var layout = new StackPanel { Padding = new Thickness(30), Spacing = 20 };
        layout.Children.Add(new TextBlock { Text = "ברוכים הבאים", FontSize = 32, FontWeight = Microsoft.UI.Text.FontWeights.SemiBold });
        layout.Children.Add(new TextBlock { Text = "מרכז השליטה של עמדת הרב־קו", FontSize = 16, TextWrapping = TextWrapping.Wrap });

        var cards = new Grid { ColumnSpacing = 14 };
        for (var i = 0; i < 3; i++) cards.ColumnDefinitions.Add(new ColumnDefinition());
        AddCard(cards, 0, "עמדת רב־קו", "פתיחת ממשק הגלישה", "כניסה לעמדה", () => Frame?.Navigate(typeof(RavKavPage)));
        AddCard(cards, 1, "ניהול מערכת", "שליטה בהרשאות ובהגדרות", "פתיחת הניהול", () => Frame?.Navigate(typeof(AdminPage)));
        AddCard(cards, 2, "יומן פעילות", "מעקב אחר פעולות ושינויים", "צפייה ביומן", () => Frame?.Navigate(typeof(ActivityPage)));
        layout.Children.Add(cards);

        var infoPanel = new StackPanel { Spacing = 8 };
        infoPanel.Children.Add(new TextBlock { Text = "תשתית המערכת", FontSize = 20, FontWeight = Microsoft.UI.Text.FontWeights.SemiBold });
        infoPanel.Children.Add(new TextBlock { Text = "המערכת כוללת דפדפן WebView2 פנימי, מדיניות אתרים, בחירת אלמנטים חזותית, שמירת הגדרות עם גיבוי ויומן פעילות.", TextWrapping = TextWrapping.Wrap });
        infoPanel.Children.Add(new InfoBar { IsOpen = true, Severity = InfoBarSeverity.Informational, Title = "מצב המערכת", Message = "חיבור קורא הכרטיסים הפיזי דורש בדיקת חומרה נפרדת." });
        layout.Children.Add(new Border { Padding = new Thickness(20), CornerRadius = new CornerRadius(12), Child = infoPanel });

        root.Content = layout;
        Content = root;
    }

    // Retained because the compiled XAML still declares these Click handlers.
    // The runtime dashboard uses the equivalent code-created buttons above.
    private void OpenRavKav_Click(object sender, RoutedEventArgs e) => Frame?.Navigate(typeof(RavKavPage));
    private void OpenAdmin_Click(object sender, RoutedEventArgs e) => Frame?.Navigate(typeof(AdminPage));
    private void OpenActivity_Click(object sender, RoutedEventArgs e) => Frame?.Navigate(typeof(ActivityPage));

    private static void AddCard(Grid grid, int column, string title, string description, string buttonText, Action action)
    {
        var panel = new StackPanel { Spacing = 10 };
        panel.Children.Add(new TextBlock { Text = title, FontSize = 19, FontWeight = Microsoft.UI.Text.FontWeights.SemiBold });
        panel.Children.Add(new TextBlock { Text = description, TextWrapping = TextWrapping.Wrap });
        var button = new Button { Content = buttonText, HorizontalAlignment = HorizontalAlignment.Stretch };
        button.Click += (_, _) => action();
        panel.Children.Add(button);
        var border = new Border { Padding = new Thickness(18), CornerRadius = new CornerRadius(12), BorderThickness = new Thickness(1), BorderBrush = new SolidColorBrush(Microsoft.UI.Colors.Gray), Child = panel };
        Grid.SetColumn(border, column);
        grid.Children.Add(border);
    }
}
