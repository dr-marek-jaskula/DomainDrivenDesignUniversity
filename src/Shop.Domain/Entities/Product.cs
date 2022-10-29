using Shopway.Domain.DomainEvents;
using Shopway.Domain.Primitives;
using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Entities;

public sealed class Product : AggregateRoot
{
    public ProductName ProductName { get; private set; }
    public Revision Revision { get; private set; }
    public Price Price { get; private set; }
    public UomCode UomCode { get; private set; }
    public List<Tag> Tags { get; private set; } = new();
    public List<Review> Reviews { get; private set; } = new();
    public List<Order> Orders { get; private set; } = new();

    internal Product(
        Guid id,
        ProductName productName,
        Price price,
        UomCode uomCode,
        Revision revision) 
        : base(id)
    {
        ProductName = productName;
        Price = price;
        UomCode = uomCode;
        Revision = revision;
    }

    // Empty constructor in this case is required by EF Core
    private Product()
    {
    }

    public static Product Create(
        Guid id,
        ProductName productName,
        Price price,
        UomCode uomCode,
        Revision revision)
    {
        var product = new Product(
            id,
            productName,
            price,
            uomCode,
            revision);

        product.RaiseDomainEvent(new ProductCreatedDomainEvent(Guid.NewGuid(), product.Id));

        return product;
    }
}