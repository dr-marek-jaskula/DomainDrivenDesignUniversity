using Shopway.Domain.Common.DataProcessing.Abstractions;
using Shopway.Domain.Common.Enums;
using Shopway.Domain.Common.Utilities;

namespace Shopway.Domain.Products.DataProcessing.Sorting;

public sealed record ProductStaticSortBy : ISortBy<Product>
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
            .SortBy(ProductName, product => product.ProductName)
            .SortBy(Revision, product => product.Revision)
            .SortBy(Price, product => product.Price)
            .SortBy(UomCode, product => product.UomCode);

        return ((IOrderedQueryable<Product>)queryable)
            .ThenSortBy(ThenProductName, product => product.ProductName)
            .ThenSortBy(ThenRevision, product => product.Revision)
            .ThenSortBy(ThenPrice, product => product.Price)
            .ThenSortBy(ThenUomCode, product => product.UomCode);
    }
}
