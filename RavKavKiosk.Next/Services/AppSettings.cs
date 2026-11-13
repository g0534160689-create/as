namespace RavKavKiosk.Next.Services;

public sealed class AppSettings
{
    public string StartUrl { get; set; } = "https://www.google.com";
    public bool AllowPopups { get; set; }
    public bool EnforceAllowList { get; set; }
    public List<string> AllowedDomains { get; set; } = [];
    public List<string> BlockedDomains { get; set; } = [];
}
