using com.b_velop.microfe.Commands;
using com.b_velop.microfe.Handler;
using com.b_velop.microfe.Models;
using com.b_velop.microfe.shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace com.b_velop.microfe.Controller;

[ApiController]
[Route("api/[controller]")]
public class HostController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> RegisterApplication(
        [FromBody] ApplicationConfiguration applicationConfiguration,
        [FromServices] ICommandHandler<RegisterApplicationCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler
            .Handle(
                new RegisterApplicationCommand
                {
                    ApplicationConfiguration = applicationConfiguration
                },
                cancellationToken);
        return Ok();
    }

    [HttpGet("alive/{name}")]
    public IActionResult GetAliveSignal([FromRoute] string name, CancellationToken cancellationToken)
    {
        var applications = ApplicationStore.Applications;
        if (applications.TryGetValue(name, out var app))
        {
            var update = app with
            {
                LastSeen = DateTime.UtcNow
            };
            ApplicationStore.Applications.TryUpdate(name, update, app);
            return Ok();
        }

        return NotFound();
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetApplicationByName([FromRoute] string name, CancellationToken cancellationToken)
    {
        if (ApplicationStore.Applications.TryGetValue(name, out var app))
        {
            return Ok(app);
        }

        return NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> GetApplication(
        ICacheService service,
        CancellationToken cancellationToken)
    {
        var result = service.GetFromCache<Dictionary<string, Application>>("App");
        return await Task<IActionResult>.FromResult(Ok(result));
    }

    record ImportMap
    {
        public Dictionary<string, string> Imports { get; init; }
    }

    [HttpGet("importmap")]
    public async Task<IActionResult> GetImportmap(
        ICacheService service,
        CancellationToken cancellationToken)
    {
        var result = ApplicationStore.Applications;
        var r = new ImportMap
        {
            Imports = new Dictionary<string, string>()
        };
        // r.Imports.TryAdd("@b-velop/root-config", "//localhost:9000/b-velop-root-config.js");
        // r.Imports.TryAdd("@b-velop/frame", "//localhost:9077/b-velop-frame.js");
        r.Imports.TryAdd("@b-velop/app2", "//localhost:9011/b-velop-app2.js");
        foreach (var (key, value) in result)
        {
            r.Imports.Add($"@{value.Prefix}/{key}", $"//{value.Server}:{value.Port}/{value.Prefix}-{value.Name}.js");
        }

        return await Task<IActionResult>.FromResult(Ok(r));
    }

    record LocationMap
    {
        public Dictionary<string, string> Locations { get; init; }
    }

    [HttpGet("locationmap")]
    public IActionResult GetLocationmap()
    {
        var applications = ApplicationStore.Applications;
        var r = new LocationMap
        {
            Locations = new Dictionary<string, string>()
        };
        foreach (var (key, value) in applications)
        {
            r.Locations.Add($"@{value.Prefix}/{key}", $"/{value.Location}");
        }

        return Ok(r);
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