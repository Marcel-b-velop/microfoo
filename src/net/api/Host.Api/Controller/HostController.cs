using com.b_velop.microfe.Models;
using Microsoft.AspNetCore.Mvc;

namespace com.b_velop.microfe.Controller;

[ApiController]
[Route("api/[controller]")]
public class HostController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> RegisterApplication(ApplicationConfiguration applicationConfiguration,
        CancellationToken cancellationToken)
    {
        
    }
    [HttpGet]
    public async Task<IActionResult> GetApplication(
        ICacheService service,
        CancellationToken cancellationToken)
    {
        var result = service.GetFromCache<Dictionary<string, Application>>("App");
        return await Task<IActionResult>.FromResult(Ok(result));
    }

    [HttpGet("map")]
    public async Task<IActionResult> GetMap(
        ICacheService service,
        CancellationToken cancellationToken)
    {
        var result = service.GetFromCache<Application>("App");
        return await Task<IActionResult>.FromResult(Ok(result));
    }
}