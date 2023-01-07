using Shopway.Domain.Primitives;
using Shopway.Domain.StronglyTypedIds;
using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Entities;

public sealed class Permission : Entity<PremissionId>
{
    public PremissionName PremissionName { get; private set; }

    internal Permission
    (
        PremissionId id,
        PremissionName premissionName
    )
        : base(id)
    {
        PremissionName = premissionName;
    }

    // Empty constructor in this case is required by EF Core
    private Permission()
    {
    }

    internal static Permission Create(PremissionName premissionName)
    {
        return new Permission
        (
            id: PremissionId.New(),
            premissionName: premissionName
        );
    }
}
