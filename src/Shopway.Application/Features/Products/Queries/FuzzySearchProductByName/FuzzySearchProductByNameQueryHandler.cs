using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Products;
using Shopway.Domain.Products.DataProcessing.Filtering;
using Shopway.Domain.Products.DataProcessing.Sorting;
using ZiggyCreatures.Caching.Fusion;

namespace Shopway.Application.Features.Products.Queries.FuzzySearchProductByName;

internal sealed class FuzzySearchProductByNameQueryHandler
(
    IProductRepository productRepository,
    IFusionCache fusionCache,
    IFuzzySearchFactory fuzzySearchFactory
)
    : IOffsetPageQueryHandler<FuzzySearchProductByNameQuery, ProductResponse, OffsetPage>
{
    private const string ProductNames = nameof(ProductNames);
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IFusionCache _fusionCache = fusionCache;
    private readonly IFuzzySearchFactory _fuzzySearchFactory = fuzzySearchFactory;

    public async Task<IResult<OffsetPageResponse<ProductResponse>>> Handle(FuzzySearchProductByNameQuery query, CancellationToken cancellationToken)
    {
        var approximatedNameResult = await ApproximateProductName(query, cancellationToken);

        if (approximatedNameResult.IsFailure)
        {
            return Result.Failure<OffsetPageResponse<ProductResponse>>(approximatedNameResult.Error);
        }

        var productByNameFilter = new ProductFuzzyFilter()
        {
            FuzzyFilter = product => (string)(object)product.ProductName == approximatedNameResult.Value
        };

        var page = await _productRepository
            .PageAsync(query.Page, cancellationToken, filter: productByNameFilter, sort: CommonProductSortBy.Instance, mapping: ProductMapping.ProductResponse);

        return page
            .ToPageResponse(query.Page)
            .ToResult();
    }

    private async Task<Result<string>> ApproximateProductName(FuzzySearchProductByNameQuery query, CancellationToken cancellationToken)
    {
        var productNames = await _fusionCache.GetOrSetAsync
        (
            ProductNames,
            _productRepository.GetNamesAsync!,
            TimeSpan.FromMinutes(1),
            cancellationToken
        );

        var fuzzySearch = _fuzzySearchFactory.Create(productNames!);

        return fuzzySearch.FindBestSuggestion(query.ProductName);
    }
}