using Shopway.Domain.Common.DataProcessing.Proxy;

namespace Shopway.Application.Features;

public record ProxyQuery
(
    string Entity,
    OffsetOrCursorPage Page, 
    DynamicFilter? Filter, 
    DynamicSortBy? SortBy, 
    DynamicMapping? Mapping
);
