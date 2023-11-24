using Shopway.Domain.Common.DataProcessing;
using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Products.Queries.FuzzySearchProductByName;

public sealed record FuzzySearchProductByNameQuery(OffsetPage Page, string ProductName) 
    : IOffsetPageQuery<ProductResponse, OffsetPage>;