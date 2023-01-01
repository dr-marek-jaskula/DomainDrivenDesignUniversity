namespace Shopway.Domain.StronglyTypedIds;

public readonly record struct ProductId : IEntityId<ProductId>
{
    private ProductId(Guid id)
    {
        Value = id;
    }

    public Guid Value { get; init; }

    public static ProductId New()
    {
        return new ProductId(Guid.NewGuid());
    }

    public static ProductId New(Guid id)
    {
        return new ProductId(id);
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