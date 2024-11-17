namespace UrlShortener.Domain.Request;

public class CreateUrlRequest
{
    public Guid UserId { get; set; }
    public string Url { get; set; } = string.Empty;
}