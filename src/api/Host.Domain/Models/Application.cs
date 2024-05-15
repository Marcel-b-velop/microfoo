namespace Host.Domain.Models;

public record Application
{
    public string? Name { get; init; }
    public string? DisplayName { get; init; }
    public string? Server { get; init; }
    public int? Port { get; init; }
    public string? ApplicationRoot { get; init; }
    public string? AppIcon { get; init; }
}