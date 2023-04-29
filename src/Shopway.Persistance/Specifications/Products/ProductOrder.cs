using Shopway.Domain.Abstractions;
using Shopway.Domain.Entities;
using Shopway.Domain.Enums;
using Shopway.Domain.Utilities;
using System.Collections.ObjectModel;
using static Shopway.Domain.Utilities.ListUtilities;

namespace Shopway.Persistence.Specifications.Products;

public record OrderData(string PropertyName, SortDirection SortDirection, int Priority);

public sealed record ProductOrder : ISortBy<Product>
{
    private static readonly ReadOnlyCollection<string> _allowedSortProperties = AsList
    (
         nameof(Product.ProductName),
         nameof(Product.Revision),
         nameof(Product.Price),
         nameof(Product.UomCode)
    )
    .AsReadOnly();

    public IList<OrderData> SortOrder { get; init; } = new List<OrderData>();

    public IQueryable<Product> Apply(IQueryable<Product> queryable)
    {
        bool sortOrderContainsInvalidPropertyName = SortOrder
            .Select(x => x.PropertyName)
            .Except(_allowedSortProperties)
            .Any();

        if (sortOrderContainsInvalidPropertyName)
        {
            throw new InvalidOperationException($"{nameof(SortOrder)} contains invalid property name.");
        }

        var somelist = SortOrder
            .Distinct()
            .OrderBy(x => x.Priority);

        var firstElement = somelist.FirstOrDefault();

        if (firstElement is null)
        {
            return queryable;
        }

        queryable = queryable
            .SortBy(firstElement.SortDirection, product => firstElement.PropertyName);

        foreach (var item in somelist.Skip(1))
        {
            queryable = ((IOrderedQueryable<Product>)queryable)
                .ThenSortBy(item.SortDirection, product => item.PropertyName);
        }

        return queryable;
    }
}