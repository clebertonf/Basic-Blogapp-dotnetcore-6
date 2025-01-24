using Blog.Data;
using Blog.Models;
using Blog.ViewModels;
using Blog.ViewModels.Posts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

[ApiController]
public class PostController : ControllerBase
{
    [HttpGet("v1/posts")]
    public async Task<IActionResult> GetAsync([FromServices] BlogDataContext dataContext, [FromQuery] int page = 0, [FromQuery] int pageSize = 25)
    {
        try
        {
            // var posts = await dataContext.Posts.AsNoTracking().Select(p => new { id = p.Id, title = p.Title }).ToListAsync();
            var posts = await dataContext.Posts
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Author)
                .Select(p => new ListPostsViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Slug = p.Slug,
                    Category = p.Category!.Name,
                    Author = $"{p.Author!.Name} {p.Author.Email}"
                })
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new ResultViewModel<dynamic>(new
            {
                posts,
                page,
                pageSize,
            }));
        }
        catch (Exception e)
        {
            return StatusCode(500, new ResultViewModel<List<Post>>("One or more errors occurred."));
        }
    }
    
    [HttpGet("v1/posts/{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromServices] BlogDataContext dataContext, [FromRoute] int id)
    {
        try
        {
            // var posts = await dataContext.Posts.AsNoTracking().Select(p => new { id = p.Id, title = p.Title }).ToListAsync();
            var post = await dataContext.Posts
                .AsNoTracking()
                .Include(p => p.Author)
                .ThenInclude(u => u.Roles)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post is null)
                return NotFound(new ResultViewModel<Post>("No post found."));
            
            return Ok(new ResultViewModel<Post>(post));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Post>("One or more errors occurred."));
        }
    }
}