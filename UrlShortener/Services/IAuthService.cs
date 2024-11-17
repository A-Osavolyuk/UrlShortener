using UrlShortener.Domain.Request;
using UrlShortener.Domain.Responses;

namespace UrlShortener.Services;

public interface IAuthService
{
    public Task<IdentityResult> LoginAsync(LoginRequest request);
    public Task LogoutAsync();
}