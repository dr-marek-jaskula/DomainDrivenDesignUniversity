using Shopway.Domain.Common;
using Shopway.Domain.Results;
using Shopway.Domain.Abstractions;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using ZiggyCreatures.Caching.Fusion;
using Shopway.Domain.Filering.Products;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Abstractions.Repositories;

namespace Shopway.Application.Features.Products.Queries.FuzzySearchProductByName;

internal sealed class FuzzySearchProductByNameQueryHandler : IOffsetPageQueryHandler<FuzzySearchProductByNameQuery, ProductResponse, OffsetPage>
{
    private readonly IProductRepository _productRepository;
    private readonly IFusionCache _fusionCache;
    private readonly IFuzzySearchFactory _fuzzySearchFactory;
    private const string ProductNames = nameof(ProductNames);

    public FuzzySearchProductByNameQueryHandler(IProductRepository productRepository, IFusionCache fusionCache, IFuzzySearchFactory fuzzySearchFactory)
    {
        _productRepository = productRepository;
        _fusionCache = fusionCache;
        _fuzzySearchFactory = fuzzySearchFactory;
    }

    public async Task<IResult<OffsetPageResponse<ProductResponse>>> Handle(FuzzySearchProductByNameQuery query, CancellationToken cancellationToken)
    {
        var productNames = await _fusionCache.GetOrSetAsync
        (
            ProductNames,
            _productRepository.GetNamesAsync!,
            TimeSpan.FromMinutes(1),
            cancellationToken
        );

        var fuzzySearch = _fuzzySearchFactory.Create(productNames!);

        var approximatedNameResult = fuzzySearch.FindBestSuggestion(query.ProductName);

        if (approximatedNameResult.IsFailure)
        {
            return Result.Failure<OffsetPageResponse<ProductResponse>>(approximatedNameResult.Error);
        }

        var productByNameFilter = new ProductFuzzyFilter()
        {
            FuzzyFilter = product => (string)(object)product.ProductName == approximatedNameResult.Value
        };

        var page = await _productRepository
            .PageAsync(query.Page, cancellationToken, filter: productByNameFilter, mapping: ProductMapping.ProductResponse);

        return page
            .ToPageResponse(query.Page)
            .ToResult();
    }
}