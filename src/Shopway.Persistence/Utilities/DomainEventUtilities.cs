using Newtonsoft.Json;
using Shopway.Domain.Abstractions;

namespace Shopway.Persistence.Utilities;

public static class DomainEventUtilities
{
    public static string Serialize(this IDomainEvent domainEvent, TypeNameHandling typeNameHandling = TypeNameHandling.None)
    {
        return JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings
        {
            TypeNameHandling = typeNameHandling
        });
    }
}