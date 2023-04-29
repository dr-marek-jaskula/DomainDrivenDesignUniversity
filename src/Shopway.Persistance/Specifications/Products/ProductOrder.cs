using Shopway.Domain.Abstractions;
using Shopway.Domain.Entities;
using Shopway.Domain.Common;
using Shopway.Persistence.Utilities;
using System.Collections.ObjectModel;
using static Shopway.Domain.Utilities.ListUtilities;

namespace Shopway.Persistence.Specifications.Products;

public sealed record ProductOrder : ISortBy<Product>
{
    public ReadOnlyCollection<string> AllowedSortProperties { get; init; } = AsList
    (
         nameof(Product.ProductName),
         nameof(Product.Revision),
         nameof(Product.Price),
         nameof(Product.UomCode)
    )
    .AsReadOnly();

    public IList<SortByEntry> SortProperties { get; init; } = new List<SortByEntry>();

    public IQueryable<Product> Apply(IQueryable<Product> queryable)
    {
        return queryable.Apply(SortProperties);
    }
}