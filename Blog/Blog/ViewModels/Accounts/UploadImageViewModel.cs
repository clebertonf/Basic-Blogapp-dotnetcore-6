using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Accounts;

public class UploadImageViewModel
{
    [Required(ErrorMessage = "Image is required")]
    public string Base64Image { get; set; }
}