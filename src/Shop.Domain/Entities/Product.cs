using Shopway.Domain.Primitives;

namespace Shopway.Domain.Entities;

public sealed class Product : Entity
{
    public Product(Guid id, string name, decimal price) : base(id)
    {
        Name = name;
        Price = price;
    }

    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public List<Tag> Tags { get; private set; } = new();
    public List<Review> Reviews { get; private set; } = new();
    public List<Shop> Shops { get; private set; } = new();
    public List<Order> Orders { get; private set; } = new();
}