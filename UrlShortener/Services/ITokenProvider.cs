using UrlShortener.Domain.Responses;

namespace UrlShortener.Services;

public interface ITokenProvider
{
    public string GenerateToken(IdentityUser user, string role);
}