namespace RavKavKiosk.WinUI.Services;

public static class SitePolicy
{
    public static bool IsAllowed(string? uriText, AppSettings settings, out string reason)
    {
        reason = string.Empty;
        if (!Uri.TryCreate(uriText, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttps && uri.Scheme != Uri.UriSchemeHttp))
        {
            reason = "כתובת לא תקינה או פרוטוקול שאינו HTTP/HTTPS";
            return false;
        }

        var host = uri.IdnHost.TrimEnd('.').ToLowerInvariant();
        if (settings.Blacklist.Any(domain => Matches(host, domain)))
        {
            reason = "האתר נמצא ברשימה השחורה";
            return false;
        }

        if (settings.EnforceWhitelist && !settings.Whitelist.Any(domain => Matches(host, domain)))
        {
            reason = "האתר אינו מופיע ברשימה הלבנה";
            return false;
        }

        if (!settings.AllowExternalLinks &&
            !host.Equals("ravkavonline.co.il", StringComparison.OrdinalIgnoreCase) &&
            !host.EndsWith(".ravkavonline.co.il", StringComparison.OrdinalIgnoreCase) &&
            !settings.Whitelist.Any(domain => Matches(host, domain)))
        {
            reason = "קישורים חיצוניים אינם מאושרים";
            return false;
        }

        return true;
    }

    private static bool Matches(string host, string domain)
    {
        var normalized = domain.Trim().TrimStart('.').ToLowerInvariant();
        return normalized.Length > 0 &&
               (host.Equals(normalized, StringComparison.OrdinalIgnoreCase) ||
                host.EndsWith("." + normalized, StringComparison.OrdinalIgnoreCase));
    }
}
