using UrlShortener.Domain.Entities;

namespace UrlShortener.Controllers;

public class HomeController(
    IUrlService urlService,
    IConfiguration configuration) : Controller
{
    private readonly IUrlService _urlService = urlService;
    private readonly IConfiguration _configuration = configuration;

    [HttpGet("/{shortUrl}")]
    public async Task<IActionResult> RedirectByShortUrl(string shortUrl)
    {
        var url = await _urlService.GetUrlAsync(shortUrl);

        if (url is null)
        {
            ViewBag.Url = shortUrl;
            return RedirectToAction(nameof(NotFoundUrl));
        }
        
        return Redirect("https://" + url?.LongUrl);
    }
    
    public IActionResult NotFoundUrl()
    {
        return View();
    }
    
    public IActionResult ExternalRedirect(string externalUrl)
    {
        return Redirect("https://" + externalUrl);
    }
    
    [HttpGet("/short-urls-table")]
    public IActionResult UrlsTable()
    {
        if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
        {
            HttpContext.Request.Cookies.TryGetValue("token", out string? token);
            return Redirect($"{_configuration["AngularPageUrl"]}?token={token}");
        }
        
        return Redirect(_configuration["AngularPageUrl"]!);
    }

    [HttpGet]
    public async Task<IActionResult> Details(string url)
    {
        var model = await _urlService.GetUrlAsync(url);

        if (model is null)
        {
            return RedirectToAction(nameof(NotFoundUrl));
        }
        
        return View(model);
    }
}