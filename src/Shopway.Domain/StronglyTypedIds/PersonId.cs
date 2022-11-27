namespace Shopway.Domain.StronglyTypedIds;

public readonly record struct PersonId : IEntityId
{
    public PersonId()
    {
    }

    public Guid Value { get; init; }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}