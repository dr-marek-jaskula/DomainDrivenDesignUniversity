using Shopway.Domain.Abstractions;
using Shopway.Domain.Enums;
using Shopway.Domain.Utilities;

namespace Shopway.Persistence.Specifications.Products;

public sealed record ProductOrder : ISortBy
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