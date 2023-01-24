using Shopway.Domain.Abstractions;
using Shopway.Domain.Entities;
using Shopway.Domain.Enums;
using Shopway.Domain.Utilities;

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
        queryable = queryable
            .SortBy(ByProductName, product => product.ProductName.Value)
            .SortBy(ByRevision, product => product.Revision.Value)
            .SortBy(ByPrice, product => product.Price.Value)
            .SortBy(ByUomCode, product => product.UomCode.Value);

        return ((IOrderedQueryable<Product>)queryable)
            .ThenSortBy(ThenByProductName, product => product.ProductName.Value)
            .ThenSortBy(ThenByRevision, product => product.Revision.Value)
            .ThenSortBy(ThenByPrice, product => product.Price.Value)
            .ThenSortBy(ThenByUomCode, product => product.UomCode.Value);
    }
}