namespace Shopway.Domain.StronglyTypedIds;

public readonly record struct ProductId : IEntityId
{
    private ProductId(Guid id)
    {
        Value = id;
    }

    public Guid Value { get; init; }

    public static IEntityId Create(Guid id)
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