namespace com.b_velop.microfe.Commands;

public record ProcessMessageCommand
{
    public string Topic { get; init; }
    public string Payload { get; init; }
}