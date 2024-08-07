﻿using Shopway.Domain.Common.DataProcessing.Proxy;

namespace Shopway.Application.Features.Proxy;

public sealed record GenericProxyPageQuery
(
    string Entity,
    OffsetOrCursorPage Page,
    DynamicFilter? Filter,
    DynamicSortBy? SortBy,
    DynamicMapping? Mapping
)
{
    public static string GetCacheKey(string pageName, string entity)
    {
        return $"{pageName}-{entity}";
    }
}
