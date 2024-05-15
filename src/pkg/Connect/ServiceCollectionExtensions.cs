using Microsoft.Extensions.DependencyInjection;

namespace com.b_velop.microfe.connect;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHostApplication(this IServiceCollection services)
    {
        services.AddHostedService<MqttWorker>();
        return services;
    }
}