using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.DataProcessing;

namespace Shopway.Application.Features.Products.Queries.GetProductById;

public sealed record GetProductByNameLikePageQuery(OffsetPage Page, string ProductNameLikePattern)
    : IOffsetPageQuery<ProductResponse, OffsetPage>;