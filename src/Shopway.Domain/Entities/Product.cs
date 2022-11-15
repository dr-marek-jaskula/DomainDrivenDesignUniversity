using Shopway.Domain.DomainEvents;
using Shopway.Domain.Errors;
using Shopway.Domain.Primitives;
using Shopway.Domain.Results;
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

    public Review AddReview(Title title, Description description, Username username, Stars stars)
    {
        Review reviewToAdd = Review.Create(Guid.NewGuid(), Id, title, description, username, stars);

        _reviews.Add(reviewToAdd);
        
        RaiseDomainEvent(new ReviewAddedDomainEvent(Guid.NewGuid(), reviewToAdd.Id, Id));

        return reviewToAdd;
    }

    public bool RemoveReview(Review review)
    {
        return _reviews.Remove(review);
    }

    public void UpdatePrice(Price price)
    {
        Price = price;
    }
}