namespace com.b_velop.microfe.connect.Models;

public record Application
{
    public string? Name { get; init; }
    public string? DisplayName { get; init; }
    public string? Server { get; init; }
    public int? Port { get; init; }
    public string? ApplicationRoot { get; init; }
    public string? AppIcon { get; init; }
    public string? AppIconColor { get; init; }
    public Dictionary<string, string> Props { get; init; } = new();
    public Dictionary<string, bool> Flags { get; init; } = new();
}