using Microsoft.AspNetCore.Authentication.Cookies;
using UrlShortener.Domain.Options;

namespace UrlShortener.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddCors(cfg =>
        {
            cfg.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.WithExposedHeaders("http://127.0.0.1:4200");
            });
        });
        builder.Services.AddControllersWithViews();
        builder.Services.AddDbContext<AppDbContext>(cfg => cfg.UseSqlite("Data Source=app.db"));
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.SlidingExpiration = true;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = builder.Configuration["JwtOptions:Audience"],
                    ValidIssuer = builder.Configuration["JwtOptions:Issuer"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtOptions:Key"]!))
                };
            });
        builder.Services.AddAuthorization();
        builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = false;
        }).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();

        builder.Services.AddScoped<ITokenProvider, TokenProvider>();
        builder.Services.AddScoped<IUrlService, UrlService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));
        builder.Services.AddRazorPages();
    }
}