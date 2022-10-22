using Shopway.Domain.Primitives;

namespace Shopway.Domain.Entities;

public sealed class Product_Amount : Entity
{
    public int Amount { get; private set; }
    public virtual Product? Product { get; private set; }
    public int? ProductId { get; private set; }
    public virtual Shop? Shop { get; private set; }
    public int? ShopId { get; private set; }
}