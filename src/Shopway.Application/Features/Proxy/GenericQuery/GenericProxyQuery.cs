using Shopway.Domain.Common.DataProcessing.Proxy;

namespace Shopway.Application.Features.Proxy;

public sealed record GenericProxyQuery
(
    string Entity,
    Ulid Id,
    DynamicMapping? Mapping
);
