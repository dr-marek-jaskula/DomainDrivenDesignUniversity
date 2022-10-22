using Shopway.Domain.Primitives;
using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Entities;

public sealed class Review : Entity, IAuditableEntity
{
    public Review(Guid id, Username username, int stars, DateTime createdDate, DateTime updatedDate) : base(id)
    {
        Username = username;
        Stars = stars;
        CreatedOn = createdDate;
        UpdatedOn = updatedDate;
    }

    // Empty constructor in this case is required by EF Core
    private Review()
    {
    }
    //TODO something here also (Stars)
    public Username Username { get; private set; }
    public int Stars { get; private set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public Title Title { get; private set; }
    public Description? Description { get; private set; }
    public Product? Product { get; private set; }
    public int? ProductId { get; private set; }
    public Employee? Employee { get; private set; }
    public int? EmployeeId { get; private set; }
}