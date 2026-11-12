namespace RavKavKiosk.WinUI.Services;

public sealed class AppSettings
{
    public bool AllowPopups { get; set; } = true;
    public bool AllowExternalLinks { get; set; }
    public bool KeepActivity { get; set; } = true;
    public bool EnforceWhitelist { get; set; }
    public string Theme { get; set; } = "Dark";
    public string StartPage { get; set; } = "https://ravkavonline.co.il/";
    public List<string> Blacklist { get; set; } = [];
    public List<string> Whitelist { get; set; } = [];
    public List<string> HiddenElementSelectors { get; set; } = [];
}
