using System.Text.Json;
using com.b_velop.microfe.Commands;
using com.b_velop.microfe.Models;

namespace com.b_velop.microfe.Handler;

public class ProcessMessageCommandHandler(IRegisterApplication registerApplication)
    : ICommandHandler<ProcessMessageCommand>
{
    public Task Handle(ProcessMessageCommand command, CancellationToken cancellationToken)
    {
        return command.Topic switch
        {
            "apphost/connect/application" => registerApplication.Register(
                JsonSerializer.Deserialize<Application>(command.Payload)),
            _ => throw new InvalidOperationException("Invalid topic.")
        };
    }
}