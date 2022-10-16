using Shopway.Domain.Entities.Parents;

namespace Shopway.Domain.Entities;

public sealed class Issue : WorkItem
{
    public decimal Cost { get; set; }
}