using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models;

public class LoginViewModel
{
    [Required]
    [Display(Name = "Login")]
    public string Login { get; set; } = string.Empty;
    
    [Required]
    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}