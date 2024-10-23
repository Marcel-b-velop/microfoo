using com.b_velop.microfe.Commands;
using com.b_velop.microfe.Models;

namespace com.b_velop.microfe.Handler;

public class RegisterApplicationCommandHandler : ICommandHandler<RegisterApplicationCommand>
{
    public Task Handle(RegisterApplicationCommand command, CancellationToken cancellationToken)
    {
        var application = Application.Get(command.ApplicationConfiguration);
        if (ApplicationStore.Applications.TryGetValue(application.Name, out var app))
        {
            if (!ApplicationStore.Applications.TryUpdate(
                    application.Name,
                    application,
                    app))
            {
                throw new InvalidOperationException($"Could not update application {application.Name}.");
            }

            Console.WriteLine($"Updated application {application.Name}.");
            return Task.CompletedTask;
        }

        if (!ApplicationStore.Applications.TryAdd(
                application.Name,
                application))
        {
            throw new InvalidOperationException($"Could not add application {application.Name}.");
        }

        Console.WriteLine($"Added application {application.Name}.");

        return Task.CompletedTask;
    }
}