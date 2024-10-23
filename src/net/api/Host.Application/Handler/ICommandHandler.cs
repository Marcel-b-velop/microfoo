namespace com.b_velop.microfe.Handler;

public interface ICommandHandler<T>
{
    Task Handle(T command, CancellationToken cancellationToken);
}