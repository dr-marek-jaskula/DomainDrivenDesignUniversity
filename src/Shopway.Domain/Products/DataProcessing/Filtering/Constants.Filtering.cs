using System.Linq.Expressions;
using static Shopway.Domain.Common.Utilities.CollectionUtilities;
using static Shopway.Domain.Common.Utilities.EnumUtilities;
using static Shopway.Domain.Common.Utilities.ListUtilities;

namespace Shopway.Domain.Constants;

public static partial class Constants
{
    public static partial class Filtering
    {
        public static class Product
        {
            public readonly static IReadOnlyCollection<string> AllowedProductFilterProperties = AsReadOnlyCollection
            (
                 nameof(Products.Product.ProductName),
                 nameof(Products.Product.Revision),
                 nameof(Products.Product.Price),
                 nameof(Products.Product.UomCode)
            );

            public readonly static IReadOnlyCollection<string> AllowedProductFilterOperations = AsList
            (
                 nameof(string.Contains),
                 nameof(string.StartsWith),
                 nameof(string.EndsWith)
            )
                .Concat(GetNamesOf<ExpressionType>())
                .ToList()
                .AsReadOnly();
        }
    }
}