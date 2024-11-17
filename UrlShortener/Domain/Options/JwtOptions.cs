namespace UrlShortener.Domain.Options;

public class JwtOptions
{
    public int ExpirationDays { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
}