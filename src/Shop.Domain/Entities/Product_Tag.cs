using Shopway.Domain.Primitives;

namespace Shopway.Domain.Entities;

public sealed class Product_Tag : Entity
{
    public virtual Product? Product { get; set; }
    public int? ProductId { get; set; }
    public virtual Tag? Tag { get; set; }
    public int? TagId { get; set; }
}