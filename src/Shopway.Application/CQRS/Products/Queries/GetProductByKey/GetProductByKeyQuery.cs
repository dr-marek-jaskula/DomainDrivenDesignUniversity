using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.EntityBusinessKeys;

namespace Shopway.Application.CQRS.Products.Queries.GetProductByKey;

public sealed record GetProductByKeyQuery(ProductKey Key) : IQuery<ProductResponse>;