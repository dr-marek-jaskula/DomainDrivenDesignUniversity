using Shopway.Domain.Primitives;

namespace Shopway.Domain.Entities;

public sealed class Review : Entity
{
    public string UserName { get; set; } = string.Empty;
    public int Stars { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public virtual Product? Product { get; set; }
    public int? ProductId { get; set; }
    public virtual Employee? Employee { get; set; }
    public int? EmployeeId { get; set; }
}