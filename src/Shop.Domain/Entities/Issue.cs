using Shopway.Domain.Entities.Parents;
using Shopway.Domain.Enums;

namespace Shopway.Domain.Entities;

public sealed class Issue : WorkItem
{
    public Issue(
        Guid id,
        int priority,
        Status status,
        string title,
        string description,
        decimal cost)
        : base(id, priority, status, title, description)
    {
        Cost = cost;
    }

    public decimal Cost { get; private set; }
}