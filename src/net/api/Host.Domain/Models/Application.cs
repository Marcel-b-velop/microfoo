using com.b_velop.microfe.shared.Models;

namespace com.b_velop.microfe.Models;

public record Application : ApplicationConfiguration
{
    private Application()
    {
    }

    public static Application Get(ApplicationConfiguration app)
    {
        return new Application()
        {
            Name = app.Name,
            Prefix = app.Prefix,
            Version = app.Version,
            DisplayName = app.DisplayName,
            Server = app.Server,
            Port = app.Port,
            Location = app.Location,
            Icon = app.Icon,
            IconColor = app.IconColor,
            Order = app.Order,
            Group = app.Group,
            Roles = app.Roles,
            Features = app.Features,
            Props = app.Props,
            LastSeen = DateTimeOffset.UtcNow
        };
    }

    public DateTimeOffset LastSeen { get; init; }
}