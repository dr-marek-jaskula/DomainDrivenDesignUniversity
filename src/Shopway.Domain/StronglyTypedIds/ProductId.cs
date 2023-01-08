namespace Shopway.Domain.StronglyTypedIds;

public readonly record struct ProductId : IEntityId<ProductId>, IEquatable<ProductId>
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

    public static bool operator ==(ProductId? first, ProductId? second)
    {
        return first is not null
            && second is not null
            && first.Equals(second);
    }

    public static bool operator !=(ProductId? first, ProductId? second)
    {
        return !(first == second);
    }

    public bool Equals(ProductId? other)
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