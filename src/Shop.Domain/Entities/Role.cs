using Shopway.Domain.Primitives;
using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Entities;

public sealed class Role : Entity
{
    private readonly List<User> _users = new();

    public RoleName RoleName { get; private set; }
    public IReadOnlyCollection<User> Users => _users;

    internal Role(Guid Id, RoleName roleName)
        : base(Id)
    {
        RoleName = roleName;
    }

    // Empty constructor in this case is required by EF Core
    private Role()
    {
    }

    public static Role Create(
        Guid id,
        RoleName roleName)
    {
        var role = new Role(
            id,
            roleName);

        return role;
    }
}