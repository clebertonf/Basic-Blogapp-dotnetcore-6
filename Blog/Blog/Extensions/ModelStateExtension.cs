using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Blog.Extensions;

public static class ModelStateExtension
{
    public static List<string> GetErros(this ModelStateDictionary modelState)
    {
        var errors = new List<string>();
        foreach (var item in modelState.Values)
        {
            errors.AddRange(item.Errors.Select(error => error.ErrorMessage));
        }
        
        return errors;
    }
}