using Shopway.Domain.Common.DataProcessing.Proxy;

namespace Shopway.Application.Features.Proxy.GenericQuery.QueryByKey;

public sealed record GenericProxyByKeyQuery
(
    string Entity,
    Dictionary<string, string> Key,
    DynamicMapping? Mapping
);
