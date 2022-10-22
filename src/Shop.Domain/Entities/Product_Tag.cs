using Shopway.Domain.Primitives;

namespace Shopway.Domain.Entities;

public sealed class Product_Tag : Entity
{
    public virtual Product? Product { get; private set; }
    public int? ProductId { get; private set; }
    public virtual Tag? Tag { get; private set; }
    public int? TagId { get; private set; }
}