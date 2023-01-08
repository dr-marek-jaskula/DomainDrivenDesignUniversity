namespace Shopway.Domain.StronglyTypedIds;

public readonly record struct PersonId : IEntityId<PersonId>, IEquatable<PersonId>
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

    public static PersonId New(Guid id)
    {
        return new PersonId(id);
    }

    public static bool operator ==(PersonId? first, PersonId? second)
    {
        return first is not null
            && second is not null
            && first.Equals(second);
    }

    public static bool operator !=(PersonId? first, PersonId? second)
    {
        return !(first == second);
    }

    public bool Equals(PersonId? other)
    {
        if (other is null)
        {
            return false;
        }

        if (other.GetType() != GetType())
        {
            return false;
        }

        return other.Value.Value == Value;
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