using Shopway.Domain.Abstractions;
using Shopway.Domain.BaseTypes;
using Shopway.Domain.EntityIds;
using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Entities;

public sealed class Review : Entity<ReviewId>, IAuditableEntity
{
    private Review(
        ReviewId reviewId,
        ProductId productId,
        Title title,
        Description description,
        Username username,
        Stars stars)
        : base(reviewId)
    {
        Username = username;
        Stars = stars;
        Title = title;
        Description = description;
        ProductId = productId;
    }

    // Empty constructor in this case is required by EF Core
    private Review()
    {
    }

    public Username Username { get; private set; }
    public Stars Stars { get; private set; }
    public Title Title { get; private set; }
    public Description Description { get; private set; }
    public ProductId ProductId { get; private set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    internal static Review Create(
        ReviewId reviewId,
        ProductId productId,
        Title title,
        Description description,
        Username username,
        Stars stars)
    {
        var review = new Review(reviewId, productId, title, description, username, stars);

        return review;
    }

    public void UpdateDescription(Description description)
    {
        Description = description;
    }

    public void UpdateStars(Stars stars)
    {
        Stars = stars;
    }
}