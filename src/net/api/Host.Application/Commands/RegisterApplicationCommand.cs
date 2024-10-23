using com.b_velop.microfe.shared.Models;

namespace com.b_velop.microfe.Commands;

public record RegisterApplicationCommand
{
    public ApplicationConfiguration ApplicationConfiguration { get; init; }
}