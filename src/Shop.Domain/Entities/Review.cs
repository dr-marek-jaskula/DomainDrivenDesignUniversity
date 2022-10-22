using Shopway.Domain.Primitives;

namespace Shopway.Domain.Entities;

public sealed class Review : Entity
{
    public Review(Guid id, string userName, int stars, DateTime createdDate, DateTime updatedDate) : base(id)
    {
        UserName = userName;
        Stars = stars;
        CreatedOn = createdDate;
        UpdatedOn = updatedDate;
    }

    public string UserName { get; private set; } = string.Empty;
    public int Stars { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public DateTime UpdatedOn { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; } = string.Empty;
    public Product? Product { get; private set; }
    public int? ProductId { get; private set; }
    public Employee? Employee { get; private set; }
    public int? EmployeeId { get; private set; }
}