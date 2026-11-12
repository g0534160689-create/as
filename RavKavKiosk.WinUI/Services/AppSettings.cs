namespace RavKavKiosk.WinUI.Services;

public sealed class AppSettings
{
    public bool AllowPopups { get; set; } = true;
    public bool AllowExternalLinks { get; set; }
    public bool KeepActivity { get; set; } = true;
}
