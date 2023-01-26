using Shopway.Domain.Abstractions;

namespace Shopway.Domain.EntityIds;

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

    public static ProductId Create(Guid id)
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