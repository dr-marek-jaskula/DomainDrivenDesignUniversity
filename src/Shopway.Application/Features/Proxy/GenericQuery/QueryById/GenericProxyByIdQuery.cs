using Shopway.Domain.Common.DataProcessing.Proxy;

namespace Shopway.Application.Features.Proxy;

public sealed record GenericProxyByIdQuery
(
    string Entity,
    Ulid Id,
    DynamicMapping? Mapping
);
