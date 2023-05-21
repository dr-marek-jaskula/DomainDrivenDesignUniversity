using Shopway.Domain.Common;
using Shopway.Application.Abstractions;
using Shopway.Persistence.Specifications.Products.Sorting;
using Shopway.Persistence.Specifications.Products.Filtering;

namespace Shopway.Application.CQRS.Products.Queries.GetProductsDictionary;

internal sealed class ProductDictionaryPageQueryValidator : PageQueryValidator<ProductDictionaryPageQuery, DictionaryResponseEntry, ProductDictionaryFilter, ProductOrder, Page>
{
    public ProductDictionaryPageQueryValidator()
        : base()
    {
    }
}