using Shopway.Domain.Enums;
using Shopway.Domain.Primitives;

namespace Shopway.Domain.Entities;

public sealed class Payment : Entity
{
    public decimal? Discount { get; set; }
    public decimal Total { get; set; }
    public Status Status { get; set; }
    public DateTime Deadline { get; set; }
    public virtual Order? Order { get; set; }
}