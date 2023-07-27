using Shopway.Domain.Abstractions;

namespace Shopway.Domain.EntityIds;

public readonly record struct ProductId : IEntityId<ProductId>
{
    private ProductId(Ulid id)
    {
        Value = id;
    }

    public Ulid Value { get; init; }

    public static ProductId New()
    {
        return new ProductId(Ulid.NewUlid());
    }

    public static ProductId Create(Ulid id)
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