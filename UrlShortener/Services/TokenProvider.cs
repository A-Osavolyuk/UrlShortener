using UrlShortener.Domain.Options;
using UrlShortener.Domain.Responses;

namespace UrlShortener.Services;

public class TokenProvider(IOptions<JwtOptions> options) : ITokenProvider
{
    private readonly JwtOptions _jwtOptions = options.Value;

    public string GenerateToken(IdentityUser user, string role)
    {
        var handler = new JwtSecurityTokenHandler();

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(key: Encoding.UTF8.GetBytes(_jwtOptions.Key)),
            algorithm: SecurityAlgorithms.HmacSha256Signature);

        var token = handler.WriteToken(new JwtSecurityToken(
            audience: _jwtOptions.Audience,
            issuer: _jwtOptions.Issuer,
            claims: SetClaims(user, role),
            expires: DateTime.UtcNow.AddDays(_jwtOptions.ExpirationDays),
            signingCredentials: signingCredentials));
        
        return token;
    }

    private List<Claim> SetClaims(IdentityUser user, string role)
    {
        var claims = new List<Claim>()
        {
            new("name", user.UserName ?? "None"),
            new("email", user.Email ?? "None"),
            new("id", user.Id),
            new("role", role)
        };

        return claims;
    }
}