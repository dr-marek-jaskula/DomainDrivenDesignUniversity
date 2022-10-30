using Shopway.Domain.DomainEvents;
using Shopway.Domain.Primitives;
using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Entities;

public sealed class Product : AggregateRoot
{
    private readonly List<Review> _reviews = new();

    public ProductName ProductName { get; private set; }
    public Revision Revision { get; private set; }
    public Price Price { get; private set; }
    public UomCode UomCode { get; private set; }
    
    public IReadOnlyCollection<Review> Reviews => _reviews;

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

    public void AddReview(Review review)
    {
        _reviews.Add(review);
    }

    public bool RemoveReview(Review review)
    {
        return _reviews.Remove(review);
    }
}