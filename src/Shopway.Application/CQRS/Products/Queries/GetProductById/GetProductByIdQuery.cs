using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.EntityIds;

namespace Shopway.Application.CQRS.Products.Queries.GetProductById;

public sealed record GetProductByIdQuery(ProductId Id) : IQuery<ProductResponse>;