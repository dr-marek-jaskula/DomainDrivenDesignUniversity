namespace Shopway.Domain.StronglyTypedIds;

public readonly record struct PersonId : IEntityId
{
    private PersonId(Guid id)
    {
        Value = id;
    }

    public Guid Value { get; init; }

    public static IEntityId Create(Guid id)
    {
        return new PersonId(id);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}