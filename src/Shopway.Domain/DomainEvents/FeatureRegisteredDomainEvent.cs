using Shopway.Domain.BaseTypes;
using Shopway.Domain.EntityIds;

namespace Shopway.Domain.DomainEvents;

public sealed record FeatureRegisteredDomainEvent(Guid Id, WorkItemId FeatureId) : DomainEvent(Id)
{
    public static FeatureRegisteredDomainEvent New(WorkItemId FeatureId)
    {
        return new FeatureRegisteredDomainEvent(Guid.NewGuid(), FeatureId);
    }
}