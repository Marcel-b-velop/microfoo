using System.Text.Json;
using Host.Application.Commands;
using Host.Register.Adapter;

namespace Host.Application.Handler;

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
                JsonSerializer.Deserialize<Domain.Models.Application>(command.Payload)),
            _ => throw new InvalidOperationException("Invalid topic.")
        };
    }
}