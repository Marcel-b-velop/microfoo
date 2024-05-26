using System.Text.Json;
using com.b_velop.microfe.Commands;
using com.b_velop.microfe.Models;

namespace com.b_velop.microfe.Handler;

public class ProcessMessageCommandHandler : ICommandHandler<ProcessMessageCommand>
{
    private readonly IRegisterApplication _registerApplication;

    public ProcessMessageCommandHandler(IRegisterApplication registerApplication)
    {
        _registerApplication = registerApplication;
    }

    public Task Handle(ProcessMessageCommand command)
    {
        return command.Topic switch
        {
            "apphost/connect/application" => _registerApplication.Register(
                JsonSerializer.Deserialize<Application>(command.Payload)),
            _ => throw new InvalidOperationException("Invalid topic.")
        };
    }
}