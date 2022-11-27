namespace Shopway.Domain.StronglyTypedIds;

public interface IEntityId
{
    public Guid Value { get; init; }
}