using System.Linq.Expressions;
using static Shopway.Domain.Utilities.EnumUtilities;
using static Shopway.Domain.Utilities.ListUtilities;
using static Shopway.Domain.Utilities.CollectionUtilities;

namespace Shopway.Domain.Constants;

public static partial class Constants
{
    public static partial class Filtering
    {
        public static class Product
        {
            public readonly static IReadOnlyCollection<string> AllowedProductFilterProperties = AsReadOnlyCollection
            (
                 nameof(Shopway.Domain.Entities.Product.ProductName),
                 nameof(Shopway.Domain.Entities.Product.Revision),
                 nameof(Shopway.Domain.Entities.Product.Price),
                 nameof(Shopway.Domain.Entities.Product.UomCode)
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