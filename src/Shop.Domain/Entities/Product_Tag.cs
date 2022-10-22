using Shopway.Domain.Primitives;

namespace Shopway.Domain.Entities;

public sealed class Product_Tag : Entity
{
    public Product? Product { get; private set; }
    public int? ProductId { get; private set; }
    public Tag? Tag { get; private set; }
    public int? TagId { get; private set; }
}