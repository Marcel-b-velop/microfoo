using Microsoft.AspNetCore.Mvc;

namespace App1.Bff.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpaController : ControllerBase
{
    [HttpGet("alive")]
    public IActionResult Get()
    {
        return Ok("Hello from BFF");
    }
}