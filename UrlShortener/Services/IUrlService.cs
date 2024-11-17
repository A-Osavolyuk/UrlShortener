using UrlShortener.Domain.Entities;
using UrlShortener.Domain.Request;
using UrlShortener.Domain.Responses;

namespace UrlShortener.Services;

public interface IUrlService
{
    public Task<Response> GetUrlsAsync();
    public Task<Response> GetUrlsAsync(Guid userId);
    public Task<UrlEntity?> GetUrlAsync(string shortUrl);
    public Task<Response> CreateUrlAsync(CreateUrlRequest createUrlRequest);
    public Task<Response> DeleteUrlAsync(string shortUrl);
    public Task<Response> DeleteAllUrlsAsync();
}