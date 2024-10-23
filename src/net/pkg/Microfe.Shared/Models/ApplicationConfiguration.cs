namespace com.b_velop.microfe.shared.Models;

public record ApplicationConfiguration
{
    public string Name { get; init; } = string.Empty;
    public string Prefix { get; init; } = string.Empty;
    public string? Version { get; init; }
    public string? DisplayName { get; init; }
    public string? Server { get; init; }
    public int? Port { get; init; }
    public string? Location { get; init; }
    public string? Icon { get; init; }
    public string? IconColor { get; init; }
    public int Order { get; init; }
    public string? Group { get; init; }
    public IReadOnlyCollection<string> Roles { get; init; } = [];
    public IReadOnlyDictionary<string, bool> Features { get; init; }
    public IReadOnlyDictionary<string, string> Props { get; init; }
}