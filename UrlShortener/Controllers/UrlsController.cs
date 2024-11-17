using UrlShortener.Domain.Request;
using UrlShortener.Domain.Responses;

namespace UrlShortener.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UrlsController(
    IUrlService urlService) : ControllerBase
{
    private readonly IUrlService _urlService = urlService;

    [HttpGet("get-urls")]
    [AllowAnonymous]
    public async Task<ActionResult<Response>> GetUrlsAsync()
    {
        var response = await _urlService.GetUrlsAsync();
        return Ok(response);
    }

    [HttpPost("create-url")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User,Admin")]
    public async Task<ActionResult<Response>> CreateUrlAsync(CreateUrlRequest request)
    {
        var response = await _urlService.CreateUrlAsync(request);

        if (response.IsSucceeded)
        {
            return Ok(response);
        }
        else
        {
            return HandleError(response);
        }
    }
    
    [HttpDelete("delete-url/{shortUrl}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User,Admin")]
    public async Task<ActionResult<Response>> DeleteUrlAsync(string shortUrl)
    {
        var response = await _urlService.DeleteUrlAsync(shortUrl);

        if (response.IsSucceeded)
        {
            return Ok(response);
        }
        else
        {
            return HandleError(response);
        }
    }

    [HttpDelete("delete-all-urls")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<ActionResult<Response>> DeleteAllUrlsAsync()
    {
        var response = await _urlService.DeleteAllUrlsAsync();

        if (response.IsSucceeded)
        {
            return Ok(response);
        }
        else
        {
            return HandleError(response);
        }
    }

    private ActionResult HandleError(Response response)
    {
        return response.StatusCode switch
        {
            "400" => BadRequest(response),
            "401" => Unauthorized(response),
            "403" => StatusCode(403, response),
            "404" => NotFound(response),
            "405" => StatusCode(405, response),
            _ or "500" => StatusCode(500, response),
        };
    }
}