using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Products;

namespace Shopway.Application.Features.Products.Commands.RemoveProduct;

public sealed record RemoveProductCommand(ProductId Id) : ICommand<RemoveProductResponse>;