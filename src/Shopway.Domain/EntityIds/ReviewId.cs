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
}