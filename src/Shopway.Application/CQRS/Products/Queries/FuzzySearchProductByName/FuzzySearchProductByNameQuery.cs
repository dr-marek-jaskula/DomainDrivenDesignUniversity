using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common;

namespace Shopway.Application.CQRS.Products.Queries.FuzzySearchProductByName;

public sealed record FuzzySearchProductByNameQuery(OffsetPage Page, string ProductName) 
    : IOffsetPageQuery<ProductResponse, OffsetPage>;