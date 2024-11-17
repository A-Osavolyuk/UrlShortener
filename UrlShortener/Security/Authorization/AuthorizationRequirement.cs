namespace UrlShortener.Security.Authorization;

public class AuthorizationRequirement : IAuthorizationRequirement
{
    public string RoleName { get; set; } = string.Empty;
}

public class AuthorizationHandler : AuthorizationHandler<AuthorizationRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationRequirement requirement)
    {
        if (context.User.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == requirement.RoleName) || context.User.IsInRole("Admin")) 
        { 
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}