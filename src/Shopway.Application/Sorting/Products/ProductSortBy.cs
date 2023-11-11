using Shopway.Domain.Entities;
using Shopway.Domain.Abstractions.Common;

namespace Shopway.Application.Sorting.Products;

public static class ProductSortBy
{
    public static readonly ISortBy<Product> Common = new CommonProductSortBy();
}