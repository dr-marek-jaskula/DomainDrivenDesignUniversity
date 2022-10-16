using Shopway.Domain.Entities.Parents;
using Shopway.Domain.Enums;

namespace Shopway.Domain.Entities;

public sealed class Customer : Person
{
    public Rank Rank { get; set; }
    public virtual List<Order> Orders { get; set; } = new();
}