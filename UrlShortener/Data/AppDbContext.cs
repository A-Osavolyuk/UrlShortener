using UrlShortener.Domain.Entities;

namespace UrlShortener.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<IdentityUser>(options)
{
    public DbSet<UrlEntity> Urls { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UrlEntity>(e =>
        {
            e.HasKey(url => url.Id);
            e.Property(url => url.LongUrl).HasColumnType("VARCHAR(1000)");
            e.Property(url => url.ShortUrl).HasColumnType("VARCHAR(100)");
        });

        builder.Entity<IdentityRole>(e =>
        {
            e.HasData(
                new IdentityRole()
                {
                    Id = Guid.Parse("c1a9a028-18bf-426f-be45-da11c34ce394").ToString(),
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole()
                {
                    Id = Guid.Parse("b2ccc4ed-5df1-42ad-8687-bcece0259be1").ToString(),
                    Name = "User",
                    NormalizedName = "USER"
                });
        });

        builder.Entity<IdentityUser>(e =>
        {
            e.HasData(
                new IdentityUser()
                {
                    Id = Guid.Parse("681edf0b-3b35-4f42-a642-caf9c5308cc4").ToString(),
                    UserName = "user@user.com",
                    NormalizedUserName = "user@user.com".ToUpper(),
                    Email = "user@user.com",
                    NormalizedEmail = "user@user.com".ToUpper(),
                    EmailConfirmed = true,
                    PasswordHash =
                        "AQAAAAIAAYagAAAAEBcsfzJQeO/IXG72GXqGwfTSe4I4d21bNQnsuz+9SDfEEBW8Ee4FdLlBqk59USObTA==",
                },
                new IdentityUser()
                {
                    Id = Guid.Parse("0850aa25-da1d-4134-b99a-b90274b96900").ToString(),
                    UserName = "admin@admin.com",
                    NormalizedUserName = "admin@admin.com".ToUpper(),
                    Email = "admin@admin.com",
                    NormalizedEmail = "admin@admin.com".ToUpper(),
                    EmailConfirmed = true,
                    PasswordHash =
                        "AQAAAAIAAYagAAAAEBcsfzJQeO/IXG72GXqGwfTSe4I4d21bNQnsuz+9SDfEEBW8Ee4FdLlBqk59USObTA==",
                }
            );
        });

        builder.Entity<IdentityUserRole<string>>(e =>
        {
            e.HasData(
                new IdentityUserRole<string>() { RoleId = "b2ccc4ed-5df1-42ad-8687-bcece0259be1", UserId = "681edf0b-3b35-4f42-a642-caf9c5308cc4"},
                new IdentityUserRole<string>() { RoleId = "c1a9a028-18bf-426f-be45-da11c34ce394", UserId = "0850aa25-da1d-4134-b99a-b90274b96900"}
            );
        });
    }
}