using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UrlShortener.Pages;

public class About : PageModel
{
    [BindProperty] public string Description { get; set; } = string.Empty;

    public void OnGet()
    {
        HttpContext.Request.Cookies.TryGetValue("description", out var value);

        if (!string.IsNullOrEmpty(value))
        {
            Description = value.ToString();
        }
        else
        {
            Description = "Hi, I will introduce you to the work of this algorithm for shortening urls. " +
                          "<br>First, follow the link in the navigation panel 'Urls', but first authorize. " +
                          "<br>There you can enter your original link and get a shortened one, of course, if you are authorized, " +
                          "you can also delete it and see its details. <br>How to use this url? Just give it to someone or follow it yourself and voila. " +
                          "<br>How does it work? You enter the original url, the algorithm creates a shortened " +
                          "version for it and when you follow it, our server automatically redirects you to it.";
        }
    }

    public IActionResult OnPost()
    {
        HttpContext.Response.Cookies.Append("description", Description);
        return Page();
    }
}