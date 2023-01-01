namespace Shopway.Domain.StronglyTypedIds;

public readonly record struct ReviewId : IEntityId<ReviewId>
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

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}