using Shopway.Domain.Abstractions;
using Shopway.Domain.BaseTypes;
using Shopway.Domain.DomainEvents;
using Shopway.Domain.EntityIds;
using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Entities;

public sealed class Product : AggregateRoot<ProductId>, IAuditable
{
    private readonly List<Review> _reviews = new();

    private Product(
        ProductId id,
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

    public ProductName ProductName { get; private set; }
    public Revision Revision { get; private set; }
    public Price Price { get; private set; }
    public UomCode UomCode { get; private set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    public IReadOnlyCollection<Review> Reviews => _reviews.AsReadOnly();

    public static Product Create(
        ProductId id,
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

        product.RaiseDomainEvent(ProductCreatedDomainEvent.New(product.Id));

        return product;
    }

    public Review AddReview(Review review)
    {
        _reviews.Add(review);
        
        RaiseDomainEvent(ReviewAddedDomainEvent.New(review.Id, Id));

        return review;
    }

    public bool RemoveReview(Review review)
    {
        return _reviews.Remove(review);
    }

    public void UpdatePrice(Price price)
    {
        Price = price;
    }

    public void UpdateName(ProductName name)
    {
        ProductName = name;
    }

    public void UpdateRevision(Revision revision)
    {
        Revision = revision;
    }

    public void UpdateUomCode(UomCode uomCode)
    {
        UomCode = uomCode;
    }

    public bool AnyReview(Title title)
    {
        return Reviews
            .Any(x => x.Title == title);
    }
}