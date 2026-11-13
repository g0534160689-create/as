namespace RavKavKiosk.Next.Services;

public static class SitePolicy
{
    public static bool IsAllowed(string address, AppSettings settings, out string reason)
    {
        reason = "";
        if (!Uri.TryCreate(address, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttps && uri.Scheme != Uri.UriSchemeHttp))
        {
            reason = "מותרות כתובות HTTP או HTTPS בלבד";
            return false;
        }

        var host = uri.IdnHost.TrimEnd('.').ToLowerInvariant();
        if (settings.BlockedDomains.Any(domain => Matches(host, domain)))
        {
            reason = "האתר נמצא ברשימת החסימה";
            return false;
        }
        if (settings.EnforceAllowList && !settings.AllowedDomains.Any(domain => Matches(host, domain)))
        {
            reason = "האתר אינו נמצא ברשימת ההרשאה";
            return false;
        }
        return true;
    }

    private static bool Matches(string host, string domain)
    {
        var normalized = domain.Trim().TrimStart('.').ToLowerInvariant();
        return normalized.Length > 0 && (host == normalized || host.EndsWith("." + normalized, StringComparison.Ordinal));
    }
}
