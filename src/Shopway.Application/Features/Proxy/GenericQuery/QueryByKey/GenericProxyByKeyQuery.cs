using Shopway.Domain.Common.DataProcessing.Proxy;

namespace Shopway.Application.Features.Proxy.GenericQuery.QueryByKey;

public sealed record GenericProxyByKeyQuery
(
    string Entity,
    Dictionary<string, string> Key,
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
        return $"key-{entity}";
    }
}
