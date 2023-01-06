using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Enums;
using Shopway.Domain.Utilities;
using static Shopway.Application.CQRS.Products.Queries.QueryProduct.ProductPageQuery;

namespace Shopway.Application.CQRS.Products.Queries.QueryProduct;

public sealed record ProductPageQuery
(
    int PageNumber,
    int PageSize,
    ProductPageFilter Filter,
    ProductPageOrder Order
)
    : IPageQuery<PageResponse<ProductResponse>, ProductPageFilter, ProductPageOrder>
{
    public sealed record ProductPageFilter : IFilter
    {
        public string? ProductName { get; init; }
        public string? Revision { get; init; }
        public int? Price { get; init; }
        public string? UomCode { get; init; }

        public bool ByProductName => ProductName.IsNullOrEmptyOrWhiteSpace();
        public bool ByRevision => Revision.IsNullOrEmptyOrWhiteSpace();
        public bool ByPrice => Price.HasValue;
        public bool ByUomCode => UomCode.IsNullOrEmptyOrWhiteSpace();
    }

    public sealed record ProductPageOrder : ISortBy
    {
        public SortDirection? ByProductName { get; init; }
        public SortDirection? ByRevision { get; init; }
        public SortDirection? ByPrice { get; init; }
        public SortDirection? ByUomCode { get; init; }

        public SortDirection? ThenByProductName { get; init; }
        public SortDirection? ThenByRevision { get; init; }
        public SortDirection? ThenByPrice { get; init; }
        public SortDirection? ThenByUomCode { get; init; }

        public (SortDirection Direction, string Property) SortBy => ISortBy.DetermineSortBy
        (
            ByProductName.WithName(), 
            ByRevision.WithName(),
            ByPrice.WithName(),
            ByUomCode.WithName()
        );

        public (SortDirection Direction, string Property) ThenBy => ISortBy.DetermineThenBy
        (
            ThenByProductName.WithName(),
            ThenByRevision.WithName(),
            ThenByPrice.WithName(),
            ThenByUomCode.WithName()
        );
    }
}