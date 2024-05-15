using com.b_velop.microfe.Models;

namespace com.b_velop.microfe;

public class RegisterApplication : IRegisterApplication
{
    private readonly ICacheService _cacheService;

    public RegisterApplication(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public Task Register(Application application)
    {
        var d = _cacheService.GetFromCache<Dictionary<string, Application>>("App") ??
                new Dictionary<string, Application>();
        if (!d.TryAdd(application.Name, application))
        {
            d[application.Name] = application;
        }
        _cacheService.AddToCache("App", d);
        return Task.CompletedTask;
    }
}