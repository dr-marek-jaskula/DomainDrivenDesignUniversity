using Shopway.Domain.Abstractions;

namespace Shopway.Domain.EntityIds;

public readonly record struct ProductId : IEntityId<ProductId>
{
    private ProductId(Ulid id)
    {
        Value = id;
    }

    public Ulid Value { get; }

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

    public int CompareTo(IEntityId? other)
    {
        if (other is null)
        {
            return 1;
        }

        if (other is not ProductId otherProductId)
        {
            throw new ArgumentNullException($"IEntity is not {GetType().FullName}");
        }

        return Value.CompareTo(otherProductId.Value);
    }

    public static bool operator >(ProductId a, ProductId b) => a.CompareTo(b) is 1;
    public static bool operator <(ProductId a, ProductId b) => a.CompareTo(b) is -1;
    public static bool operator >=(ProductId a, ProductId b) => a.CompareTo(b) >= 0;
    public static bool operator <=(ProductId a, ProductId b) => a.CompareTo(b) <= 0;
}