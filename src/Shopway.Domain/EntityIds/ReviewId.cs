using Shopway.Domain.Abstractions;

namespace Shopway.Domain.EntityIds;

public readonly record struct ReviewId : IEntityId<ReviewId>
{
    private ReviewId(Ulid id)
    {
        Value = id;
    }

    public Ulid Value { get; init; }

    public static ReviewId New()
    {
        return new ReviewId(Ulid.NewUlid());
    }

    public static ReviewId Create(Ulid id)
    {
        return new ReviewId(id);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    public int CompareTo(IEntityId? other)
    {
        if (other is null)
        {
            return 1;
        }

        if (other is not ReviewId otherReviewId)
        {
            throw new ArgumentNullException($"IEntity is not {GetType().FullName}");
        }

        return Value.CompareTo(otherReviewId.Value);
    }

    public static bool operator >(ReviewId a, ReviewId b) => a.CompareTo(b) is 1;
    public static bool operator <(ReviewId a, ReviewId b) => a.CompareTo(b) is -1;
    public static bool operator >=(ReviewId a, ReviewId b) => a.CompareTo(b) >= 0;
    public static bool operator <=(ReviewId a, ReviewId b) => a.CompareTo(b) <= 0;
}