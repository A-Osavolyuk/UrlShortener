namespace UrlShortener.Domain.Responses;

public class Response
{
    public string StatusMessage { get; set; } = string.Empty;
    public string StatusCode { get; set; } = string.Empty;
    public object? Result { get; set; }
    public bool IsSucceeded { get; set; }
}