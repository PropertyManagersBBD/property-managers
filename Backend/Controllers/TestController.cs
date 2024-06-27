
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("Test")]
public class TestController : ControllerBase
{
    [HttpGet()]
    public IActionResult Test()
    {
        return Ok("Test");
    }
}