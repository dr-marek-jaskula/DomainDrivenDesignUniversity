namespace Shopway.Domain.StronglyTypedIds;

public readonly record struct PremissionId : IEntityId<PremissionId>
{
    private PremissionId(Guid id)
    {
        Value = id;
    }

    public Guid Value { get; init; }

    public static PremissionId New()
    {
        return new PremissionId(Guid.NewGuid());
    }

    public static PremissionId New(Guid id)
    {
        return new PremissionId(id);
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