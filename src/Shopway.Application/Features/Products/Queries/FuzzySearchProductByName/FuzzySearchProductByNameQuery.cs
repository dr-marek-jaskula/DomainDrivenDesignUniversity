using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.DataProcessing;

namespace Shopway.Application.Features.Products.Queries.FuzzySearchProductByName;

public sealed record FuzzySearchProductByNameQuery(OffsetPage Page, string ProductName)
    : IOffsetPageQuery<ProductResponse, OffsetPage>;