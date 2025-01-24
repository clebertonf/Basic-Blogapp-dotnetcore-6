using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Accounts;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; }
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; }
}