using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Products.Queries.GetProductById;

public sealed record GetProductByIdQuery(Guid ProductId) : IQuery<ProductResponse>;