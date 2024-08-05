using Shopway.Domain.Common.DataProcessing.Proxy;

namespace Shopway.Application.Features.Proxy.GenericQuery;

public interface IProxyQueryWithMapping
{
    string Entity { get; }
    DynamicMapping? Mapping { get; }
    string GetCacheKey();
}
