namespace UrlShortener.Domain.Entities;

public class UrlEntity
{
    public Guid Id { get; set; }
    public string LongUrl { get; set; } = string.Empty;
    public string ShortUrl { get; set; } = string.Empty;

    public DateTime CreatedData { get; set; } = DateTime.Now;
    public Guid CreatedBy { get; set; }
}