using Shopway.Domain.Enums;
using Shopway.Domain.Primitives;

namespace Shopway.Domain.Entities;

public class Tag : Entity
{
    public ProductTag ProductTag { get; set; }
    public virtual List<Product> Products { get; set; } = new();
}