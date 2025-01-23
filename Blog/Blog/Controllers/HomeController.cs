using Blog.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        [ApiKey]
        [HttpGet("/health-check")]
        public IActionResult Get()
        {
            return Ok(new
            {
                message = "It's alive!"
            });
        }
    }
}
