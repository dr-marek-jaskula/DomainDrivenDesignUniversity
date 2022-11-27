using Shopway.Domain.Primitives;
using Shopway.Domain.StronglyTypedIds;
using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Entities;

public sealed class Review : Entity<ReviewId>, IAuditableEntity
{
    public Username Username { get; private set; }
    public Stars Stars { get; private set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public Title Title { get; private set; }
    public Description Description { get; private set; }
    public Guid? ProductId { get; private set; }

    private Review(
        Guid reviewId,
        Guid productId,
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

    internal static Review Create(
        Guid reviewId,
        Guid productId,
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
        Description= description;
    }

    public void UpdateStars(Stars stars)
    {
        Stars = stars;
    }
}