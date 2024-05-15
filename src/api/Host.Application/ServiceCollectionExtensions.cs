using Host.Application.Commands;
using Host.Application.Handler;
using Microsoft.Extensions.DependencyInjection;

namespace Host.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHostApplication(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<ProcessMessageCommand>, ProcessMessageCommandHandler>();
        return services;
    }
}