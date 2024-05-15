using Microsoft.Extensions.DependencyInjection;

namespace com.b_velop.microfe;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRegisterAdapter(this IServiceCollection services)
    {
        services.AddSingleton<ICacheService, CacheService>();
        services.AddScoped<IRegisterApplication, RegisterApplication>();
        return services;
    }
}