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
        
        [HttpGet("/env")]
        public IActionResult GetEnv([FromServices] IConfiguration config)
        {
            return Ok(new
            {
                Environment = config.GetValue<string>("Env")
            });
        }
    }
}
