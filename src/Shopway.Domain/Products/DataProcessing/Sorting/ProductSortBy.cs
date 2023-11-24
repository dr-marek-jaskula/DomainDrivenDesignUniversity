using Shopway.Domain.Common.DataProcessing.Abstractions;

namespace Shopway.Domain.Products.DataProcessing.Sorting;

public static class ProductSortBy
{
    public static readonly ISortBy<Product> Common = new CommonProductSortBy();
}