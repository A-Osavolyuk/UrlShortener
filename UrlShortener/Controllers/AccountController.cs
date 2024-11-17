using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Domain.Request;
using UrlShortener.Models;

namespace UrlShortener.Controllers;

public class AccountController(
    IAuthService service) : Controller
{
    private readonly IAuthService _service = service;

    [HttpGet]
    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _service.LoginAsync(new LoginRequest()
            {
                Login = model.Login,
                Password = model.Password
            });

            if (result.Succeeded)
            {
                HttpContext.Request.Cookies.TryGetValue("token", out string? token);
                return Redirect($"http://127.0.0.1:4200/urls-table?token={token}");
            }

            ModelState.AddModelError(string.Empty, result.Errors.First().Description);
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _service.LogoutAsync();
        return RedirectToAction("Login");
    }
}