using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Products;

namespace Shopway.Application.Features.Products.Queries.GetProductById;

public sealed record GetProductByIdQuery(ProductId Id) : IQuery<ProductResponse>;