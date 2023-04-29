using Shopway.Domain.Common;
using Shopway.Domain.Entities;
using Shopway.Domain.Abstractions;
using Shopway.Persistence.Utilities;
using static Shopway.Domain.Utilities.CollectionUtilities;

namespace Shopway.Persistence.Specifications.Products;

public sealed record ProductOrder : ISortBy<Product>
{
    public IReadOnlyCollection<string> AllowedSortProperties { get; init; } = AsReadOnlyCollection
    (
         nameof(Product.ProductName),
         nameof(Product.Revision),
         nameof(Product.Price),
         nameof(Product.UomCode)
    );

    public IList<SortByEntry> SortProperties { get; init; } = new List<SortByEntry>();

    public IQueryable<Product> Apply(IQueryable<Product> queryable)
    {
        return queryable.Apply(SortProperties);
    }
}