using Shopway.Application.Features.Proxy.GenericQuery;
using Shopway.Domain.Common.DataProcessing.Proxy;

namespace Shopway.Application.Features.Proxy;

public sealed record GenericProxyByIdQuery
(
    string Entity,
    Ulid Id,
    DynamicMapping? Mapping
)
    : IProxyQueryWithMapping
{
    public string GetCacheKey()
    {
        return GetCacheKey(Entity);
    }

    public static string GetCacheKey(string entity)
    {
        return $"id-{entity}";
    }
}
