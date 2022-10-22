using Shopway.Domain.Enums;
using Shopway.Domain.Primitives;

namespace Shopway.Domain.Entities;

public sealed class Tag : Entity
{
    public Tag(Guid id, ProductTag productTag) : base(id)
    {
        ProductTag = productTag;
    }

    // Empty constructor in this case is required by EF Core
    private Tag()
    {
    }

    public ProductTag ProductTag { get; private set; }
    public List<Product> Products { get; private set; } = new();
}