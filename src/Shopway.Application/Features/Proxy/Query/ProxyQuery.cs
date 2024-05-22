using Shopway.Domain.Common.DataProcessing.Proxy;

namespace Shopway.Application.Features.Proxy.Query;

public sealed record ProxyQuery
(
    string Entity,
    Ulid Id,
    DynamicMapping? Mapping
);
