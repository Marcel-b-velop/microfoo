using Microsoft.AspNetCore.Mvc;

namespace Host.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class HostController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetGello(CancellationToken cancellationToken)
    {
        var result = new
        {
            Hello = "World"
        };
        return await Task<IActionResult>.FromResult(Ok(result));
    }
}