using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Categories
{
    public class EditorCategoryViewModel
    {
        [Required(ErrorMessage = "Category name is required")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Category slug is required")]
        public string? Slug { get; set; }
    }
}
