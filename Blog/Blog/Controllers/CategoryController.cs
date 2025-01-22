using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [ApiController]
    [Route("v1")]
    public class CategoryController : ControllerBase
    {
        [HttpGet("/categories")]
        public async Task<IActionResult> GetAsync([FromServices] BlogDataContext context)
        {
            try
            {
                var categories = await context.Categories.ToListAsync();
                return Ok(new ResultViewModel<List<Category>>(categories));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<List<Category>>(ex.Message));
            }
        }

        [HttpGet("/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromServices] BlogDataContext context,
                                                      [FromRoute] int id)
        {
            try
            {
                var category = await context.Categories
                    .FirstOrDefaultAsync(x => x.Id == id);
                
                return category is not null ? Ok(new ResultViewModel<Category>(category))
                    : NotFound(new ResultViewModel<Category>("Category not found"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<Category>(ex.Message));
            }
        }

        [HttpPost("/categories")]
        public async Task<IActionResult> PostAsync([FromServices] BlogDataContext context, 
                                                   [FromBody] EditorCategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErros()));
            
            try
            {
                var category = new Category
                {
                    Name = model.Name,
                    Slug = model.Slug
                };

                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();

                return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500,  new ResultViewModel<Category>($"05EX09 - {ex.Message}"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<Category>($"05EX10 - {ex.InnerException}"));
            }
        }

        [HttpPut("/categories/{id:int}")]
        public async Task<IActionResult> PutAsync([FromServices] BlogDataContext context,
                                                   [FromRoute] int id,
                                                   [FromBody] EditorCategoryViewModel model)
        {
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category is null) return NotFound(new ResultViewModel<Category>("Category not found"));

                category.Name = model.Name;
                category.Slug = model.Slug;

                context.Categories.Update(category);
                await context.SaveChangesAsync();

                return Ok( new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Category>($"05EX09 - {ex.Message}"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<Category>($"05EX10- {ex.Message}"));
            }
        }

        [HttpDelete("/categories/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromServices] BlogDataContext context,
                                                     [FromRoute] int id)
        {
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category is null) return NotFound(new ResultViewModel<Category>("Category not found"));

                context.Categories.Remove(category);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Category>($"05EX09 - {ex.Message}"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<Category>(ex.Message));
            }
        }
    }
}
