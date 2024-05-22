using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Products.ValueObjects;
using Shopway.Domain.Users.ValueObjects;

namespace Shopway.Domain.Products;

[GenerateEntityId]
public sealed class Review : Entity<ReviewId>, IAuditable
{
    private Review
    (
        ReviewId reviewId,
        Title title,
        Description description,
        Username username,
        Stars stars
    )
        : base(reviewId)
    {
        Username = username;
        Stars = stars;
        Title = title;
        Description = description;
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

    public static Review Create
    (
        ReviewId reviewId,
        Title title,
        Description description,
        Username username,
        Stars stars
    )
    {
        var review = new Review(reviewId, title, description, username, stars);

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