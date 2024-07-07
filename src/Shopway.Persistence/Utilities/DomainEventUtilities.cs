using Shopway.Domain.Common.BaseTypes.Abstractions;
using System.Text.Json;

namespace Shopway.Persistence.Utilities;

public static class DomainEventUtilities
{
    public static string Serialize(this IDomainEvent domainEvent)
    {
        return JsonSerializer.Serialize(domainEvent);
    }
}
