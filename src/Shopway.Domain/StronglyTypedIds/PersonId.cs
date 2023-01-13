using Shopway.Domain.Abstractions;

namespace Shopway.Domain.StronglyTypedIds;

public readonly record struct PersonId : IEntityId<PersonId>
{
    private PersonId(Guid id)
    {
        Value = id;
    }

    public Guid Value { get; init; }

    public static PersonId New()
    {
        return new PersonId(Guid.NewGuid());
    }

    public static PersonId Create(Guid id)
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