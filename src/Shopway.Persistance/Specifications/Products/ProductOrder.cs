using Shopway.Domain.Abstractions;
using Shopway.Domain.Entities;
using Shopway.Domain.Enums;
using Shopway.Domain.Utilities;
using static Shopway.Domain.Utilities.OrderUtilities;

namespace Shopway.Persistence.Specifications.Products;

public sealed record ProductOrder : ISortBy<Product>
{
    public SortDirection? ByProductName { get; init; }
    public SortDirection? ByRevision { get; init; }
    public SortDirection? ByPrice { get; init; }
    public SortDirection? ByUomCode { get; init; }

    public SortDirection? ThenByProductName { get; init; }
    public SortDirection? ThenByRevision { get; init; }
    public SortDirection? ThenByPrice { get; init; }
    public SortDirection? ThenByUomCode { get; init; }

    public IQueryable<Product> Apply(IQueryable<Product> queryable)
    {
        (SortDirection SortByDirection, string SortBy) = DetermineSortBy
        (
            ByProductName.WithName(),
            ByRevision.WithName(),
            ByPrice.WithName(),
            ByUomCode.WithName()
        );

        (SortDirection ThenByDirection, string ThenBy) = DetermineThenBy
        (
            ThenByProductName.WithName(),
            ThenByRevision.WithName(),
            ThenByPrice.WithName(),
            ThenByUomCode.WithName()
        );

        return queryable
            .SortBy(SortBy, SortByDirection)
            .ThenSortBy(ThenBy, ThenByDirection);
    }
}