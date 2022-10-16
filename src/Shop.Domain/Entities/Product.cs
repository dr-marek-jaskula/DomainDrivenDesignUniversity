using Shopway.Domain.Primitives;

namespace Shopway.Domain.Entities;

public class Product : Entity
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public virtual List<Tag> Tags { get; set; } = new();
    public virtual List<Review> Reviews { get; set; } = new();
    public virtual List<Shop> Shops { get; set; } = new();
    public virtual List<Order> Orders { get; set; } = new();
}