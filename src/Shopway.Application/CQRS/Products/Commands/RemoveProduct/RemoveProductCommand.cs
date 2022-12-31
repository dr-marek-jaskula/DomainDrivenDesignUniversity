using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.StronglyTypedIds;

namespace Shopway.Application.CQRS.Products.Commands.RemoveProduct;

public sealed record RemoveProductCommand(ProductId Id) : ICommand<RemoveProductResponse>;