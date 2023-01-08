using Shopway.Domain.Primitives;

namespace Shopway.Domain.Entities;

public sealed class Permission : Enumeration<Permission>
{
    public static readonly Permission Read = new(1, nameof(Read));
    public static readonly Permission Create = new(2, nameof(Create));
    public static readonly Permission Update = new(3, nameof(Update));
    public static readonly Permission Delete = new(4, nameof(Delete));

    public Permission(int id, string name)
    : base(id, name)
    {
    }

    // Empty constructor in this case is required by EF Core
    private Permission()
    {
    }
}
