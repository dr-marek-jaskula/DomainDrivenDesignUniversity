using Shopway.Domain.StronglyTypedIds;

namespace Shopway.Domain.Entities;

public class RoleUser
{
    public int RoleId { get; }

    public UserId UserId { get; }
}
