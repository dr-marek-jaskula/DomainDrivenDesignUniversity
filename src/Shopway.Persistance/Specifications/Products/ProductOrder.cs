using Shopway.Domain.Abstractions;
using Shopway.Domain.Entities;
using Shopway.Domain.Enums;
using Shopway.Domain.Utilities;

namespace Shopway.Persistence.Specifications.Products;

public sealed record ProductOrder : ISortBy<Product>
{
    public SortDirection? ProductName { get; init; }
    public SortDirection? Revision { get; init; }
    public SortDirection? Price { get; init; }
    public SortDirection? UomCode { get; init; }

    public SortDirection? ThenProductName { get; init; }
    public SortDirection? ThenRevision { get; init; }
    public SortDirection? ThenPrice { get; init; }
    public SortDirection? ThenUomCode { get; init; }

    public IQueryable<Product> Apply(IQueryable<Product> queryable)
    {
        queryable = queryable
            .SortBy(ProductName, product => product.ProductName.Value)
            .SortBy(Revision, product => product.Revision.Value)
            .SortBy(Price, product => product.Price.Value)
            .SortBy(UomCode, product => product.UomCode.Value);

        return ((IOrderedQueryable<Product>)queryable)
            .ThenSortBy(ThenProductName, product => product.ProductName.Value)
            .ThenSortBy(ThenRevision, product => product.Revision.Value)
            .ThenSortBy(ThenPrice, product => product.Price.Value)
            .ThenSortBy(ThenUomCode, product => product.UomCode.Value);
    }
}