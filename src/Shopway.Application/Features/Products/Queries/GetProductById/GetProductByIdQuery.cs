using Shopway.Domain.Products;
using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Products.Queries.GetProductById;

public sealed record GetProductByIdQuery(ProductId Id) : IQuery<ProductResponse>;