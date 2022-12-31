namespace Shopway.Domain.StronglyTypedIds;

public interface IEntityId
{
    public Guid Value { get; init; }
    public abstract static IEntityId Create(Guid id);
}