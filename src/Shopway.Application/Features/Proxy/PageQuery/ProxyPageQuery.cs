using Shopway.Domain.Common.DataProcessing.Proxy;

namespace Shopway.Application.Features.Proxy.PageQuery;

public sealed record ProxyPageQuery
(
    string Entity,
    OffsetOrCursorPage Page,
    DynamicFilter? Filter,
    DynamicSortBy? SortBy,
    DynamicMapping? Mapping
);
