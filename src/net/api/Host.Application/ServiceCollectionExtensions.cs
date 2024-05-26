using com.b_velop.microfe.Commands;
using com.b_velop.microfe.Handler;
using Microsoft.Extensions.DependencyInjection;

namespace com.b_velop.microfe;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHostApplication(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<ProcessMessageCommand>, ProcessMessageCommandHandler>();
        return services;
    }
}