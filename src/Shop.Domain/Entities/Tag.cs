using Shopway.Domain.Enums;
using Shopway.Domain.Primitives;

namespace Shopway.Domain.Entities;

public sealed class Tag : Entity
{
    public Tag(Guid id, ProductTag productTag) : base(id)
    {
        ProductTag = productTag;
    }

    public ProductTag ProductTag { get; private set; }
    public List<Product> Products { get; private set; } = new();
}