using Shopway.Domain.Abstractions;

namespace Shopway.Domain.StronglyTypedIds;

public readonly record struct ReviewId : IEntityId<ReviewId>, IEquatable<ReviewId>
{
    private ReviewId(Guid id)
    {
        Value = id;
    }

    public Guid Value { get; init; }

    public static ReviewId New()
    {
        return new ReviewId(Guid.NewGuid());
    }

    public static ReviewId New(Guid id)
    {
        return new ReviewId(id);
    }

    public static bool operator ==(ReviewId? first, ReviewId? second)
    {
        return first is not null
            && second is not null
            && first.Equals(second);
    }

    public static bool operator !=(ReviewId? first, ReviewId? second)
    {
        return !(first == second);
    }

    public bool Equals(ReviewId? other)
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