using Host.Application;
using Host.Register.Adapter;
using Microsoft.AspNetCore.Mvc;

namespace Host.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class HostController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetApplication(
        ICacheService service,
        CancellationToken cancellationToken)
    {
        var result = service.GetFromCache<Dictionary<string, Domain.Models.Application>>("App");
        return await Task<IActionResult>.FromResult(Ok(result));
    }

    [HttpGet("map")]
    public async Task<IActionResult> GetMap(
        ICacheService service,
        CancellationToken cancellationToken)
    {
        var result = service.GetFromCache<Domain.Models.Application>("App");
        return await Task<IActionResult>.FromResult(Ok(result));
    }
}