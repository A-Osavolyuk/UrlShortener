using UrlShortener.Domain.Request;
using UrlShortener.Domain.Responses;

namespace UrlShortener.Services;

public class AuthService(
    UserManager<IdentityUser> userManager,
    ITokenProvider tokenProvider,
    IHttpContextAccessor httpContextAccessor,
    SignInManager<IdentityUser> signInManager) : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;

    public async Task<IdentityResult> LoginAsync(LoginRequest request)
    {
        if (_httpContextAccessor!.HttpContext!.User.Identity!.IsAuthenticated)
        {
            await _signInManager.SignOutAsync();
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete("token");
        }
        
        var user = await _userManager.FindByEmailAsync(request.Login);

        if (user is null)
        {
            return IdentityResult.Failed(new IdentityError { Description = $"Not found user with login: {request.Login}", Code = "404" });
        }
        
        var isValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!isValidPassword)
        {
            return IdentityResult.Failed(new IdentityError { Description = "Invalid password", Code = "400" });
        }
        
        var result = await _signInManager.PasswordSignInAsync(request.Login, request.Password, isPersistent: true, lockoutOnFailure: false);
        
        if (!result.Succeeded)
        {
            return IdentityResult.Failed(new IdentityError { Description = "Invalid log in attempt", Code = "400" });
        }
        
        var userRoles = await _userManager.GetRolesAsync(user);
        var token = _tokenProvider.GenerateToken(user, userRoles[0]);
        
        _httpContextAccessor.HttpContext?.Response.Cookies.Append("token", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddDays(30)
        });
        
        return IdentityResult.Success;
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}