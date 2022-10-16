using Shopway.Domain.Primitives;

namespace Shopway.Domain.Entities;

public class Product_Amount : Entity
{
    public int Amount { get; set; }
    public virtual Product? Product { get; set; }
    public int? ProductId { get; set; }
    public virtual Shop? Shop { get; set; }
    public int? ShopId { get; set; }
}