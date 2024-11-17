using UrlShortener.Domain.Entities;
using UrlShortener.Domain.Request;
using UrlShortener.Domain.Responses;

namespace UrlShortener.Services;

public class UrlService(
    AppDbContext context) : IUrlService
{
    private readonly AppDbContext _context = context;

    public async Task<Response> GetUrlsAsync()
    {
        var urls = await _context.Urls.ToListAsync();
        return new Response()
        {
            StatusCode = "200",
            Result = urls,
            IsSucceeded = true
        };
    }
    
    public async Task<Response> GetUrlsAsync(Guid userId)
    {
        var urls = await _context.Urls.Where(x => x.CreatedBy == userId).ToListAsync();

        if (!urls.Any())
        {
            return new Response()
            {
                StatusCode = "404",
                StatusMessage = $"Not found urls for user with ID {userId}",
                IsSucceeded = false
            };
        }
        
        return new Response()
        {
            StatusCode = "200",
            Result = urls,
            IsSucceeded = true
        };
    }

    public async Task<UrlEntity?> GetUrlAsync(string shortUrl)
    {
        var url = await _context.Urls.FirstOrDefaultAsync(x => x.ShortUrl == shortUrl);

        if (url is null)
        {
            return null;
        }

        return url;
    }

    public async Task<Response> CreateUrlAsync(CreateUrlRequest request)
    {
        var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var exists = await _context.Urls.AsNoTracking().AnyAsync(x => x.LongUrl == request.Url);

            if (exists)
            {
                return new Response()
                {
                    StatusCode = "400",
                    StatusMessage = "Url already exists",
                    IsSucceeded = false
                };
            }
            
            var shortUrl = Guid.NewGuid().ToString().Substring(0, 8);
            var entity = new UrlEntity()
            {
                Id = Guid.NewGuid(),
                ShortUrl = shortUrl,
                LongUrl = request.Url,
                CreatedData = DateTime.UtcNow,
                CreatedBy = request.UserId,
            };
            
            await _context.Urls.AddAsync(entity);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            
            return new Response()
            {
                StatusCode = "201",
                StatusMessage = "Url was successfully shortened",
                Result = shortUrl,
                IsSucceeded = true
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<Response> DeleteUrlAsync(string shortUrl)
    {
        var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var entity = await _context.Urls.AsNoTracking().FirstOrDefaultAsync(x => x.ShortUrl == shortUrl);

            if (entity is null)
            {
                return new Response()
                {
                    StatusCode = "404",
                    StatusMessage = "Not found url",
                    IsSucceeded = false
                };
            }
            
            _context.Urls.Remove(entity);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            
            return new Response()
            {
                StatusCode = "200",
                StatusMessage = "Url was successfully deleted",
                IsSucceeded = true
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<Response> DeleteAllUrlsAsync()
    {
        var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            var urls = await _context.Urls.ToListAsync();
            _context.Urls.RemoveRange(urls);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new Response()
            {
                StatusCode = "200",
                StatusMessage = "Urls were successfully deleted",
                IsSucceeded = true
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}