using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Products.DataProcessing.Filtering;
using Shopway.Domain.Products.DataProcessing.Mapping;
using Shopway.Domain.Products.DataProcessing.Sorting;

namespace Shopway.Application.Features.Products.Queries.QueryOffsetPageProductWithMapping;

public sealed record ProductOffsetPageWithMappingQuery(OffsetPage Page) : IOffsetPageQuery<DataTransferObjectResponse, ProductStaticFilter, ProductStaticSortBy, ProductStaticMapping, OffsetPage>
{
    public ProductStaticFilter? Filter { get; init; }
    public ProductStaticSortBy? SortBy { get; init; }
    public ProductStaticMapping? Mapping { get; init; } = ProductStaticMapping.Instance;
}
