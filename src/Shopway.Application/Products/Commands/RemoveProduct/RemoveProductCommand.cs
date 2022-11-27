using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.StronglyTypedIds;

namespace Shopway.Application.Products.Commands.RemoveProduct;

public sealed record RemoveProductCommand(ProductId Id) : ICommand<Guid>;