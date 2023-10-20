using Shopway.Domain.EntityKeys;
using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Products.Queries.GetProductByKey;

public sealed record GetProductByKeyQuery(ProductKey Key) : IQuery<ProductResponse>;