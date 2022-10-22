using Shopway.Domain.Entities.Parents;
using Shopway.Domain.Enums;
using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Entities;

public sealed class Issue : WorkItem
{
    public Issue(
        Guid id,
        int priority,
        Status status,
        string title,
        Description description,
        decimal cost)
        : base(id, priority, status, title, description)
    {
        Cost = cost;
    }

    // Empty constructor in this case is required by EF Core
    private Issue()
    {
    }

    public decimal Cost { get; private set; }
}