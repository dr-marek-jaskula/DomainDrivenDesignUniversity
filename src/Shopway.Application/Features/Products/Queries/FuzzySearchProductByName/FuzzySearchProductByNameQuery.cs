using Shopway.Domain.Common;
using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Products.Queries.FuzzySearchProductByName;

public sealed record FuzzySearchProductByNameQuery(OffsetPage Page, string ProductName) 
    : IOffsetPageQuery<ProductResponse, OffsetPage>;