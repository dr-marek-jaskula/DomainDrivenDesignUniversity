using Shopway.Domain.Primitives;
using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Entities;

public sealed class Review : AggregateRoot, IAuditableEntity
{
    public Username Username { get; private set; }
    public Stars Stars { get; private set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public Title Title { get; private set; }
    public Description Description { get; private set; }
    public Product? Product { get; private set; }
    public Guid? ProductId { get; private set; }

    internal Review(
        Guid id,
        Title title,
        Description description,
        Username username,
        Stars stars,
        DateTimeOffset createdDate,
        DateTimeOffset updatedDate)
        : base(id)
    {
        Username = username;
        Stars = stars;
        CreatedOn = createdDate;
        UpdatedOn = updatedDate;
        Title = title;
        Description = description;
    }

    // Empty constructor in this case is required by EF Core
    private Review()
    {
    }
}