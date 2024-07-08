using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;

namespace Shopway.Persistence.Utilities;

public static class DomainEventUtilities
{
    public static string Serialize(this IDomainEvent domainEvent)
    {
        var serialize = DomainEvent.SerializeCache[domainEvent.GetType()];
        return serialize(domainEvent);
    }
}
