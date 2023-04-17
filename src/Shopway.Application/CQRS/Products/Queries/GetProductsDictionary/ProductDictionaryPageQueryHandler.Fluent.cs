using Shopway.Application.Abstractions;
using Shopway.Persistence.Specifications.Products;

namespace Shopway.Application.CQRS.Products.Queries.GetProductsDictionary;

internal sealed class ProductDictionaryPageQueryValidator : PageQueryValidator<ProductDictionaryPageQuery, DictionaryResponseEntry, ProductDictionaryFilter, ProductOrder>
{
    public ProductDictionaryPageQueryValidator()
        : base()
    {
    }
}