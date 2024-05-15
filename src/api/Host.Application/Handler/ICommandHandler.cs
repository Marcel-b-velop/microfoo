namespace Host.Application.Handler;

public interface ICommandHandler<T>
{
    Task Handle(T command);
}