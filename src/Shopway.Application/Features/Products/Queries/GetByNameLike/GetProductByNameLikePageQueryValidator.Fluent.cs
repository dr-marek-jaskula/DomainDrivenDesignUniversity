using Shopway.Application.Abstractions;
using Shopway.Application.Features.Products.Queries.GetProductById;
using Shopway.Domain.Common.DataProcessing;

namespace Shopway.Application.Features.Products.Queries.GetByNameLike;

internal sealed class GetProductByNameLikePageQueryValidator : OffsetPageQueryValidator<GetProductByNameLikePageQuery, ProductResponse, OffsetPage>
{
    public GetProductByNameLikePageQueryValidator()
        : base()
    {
    }
}