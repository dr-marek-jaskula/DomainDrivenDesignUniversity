using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Products.Commands.RemoveProduct;

public sealed record RemoveProductCommand(Guid Id) : ICommand<Guid>;