using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.CQRS.Products.Commands.CreateProduct;

public sealed record CreateUserCommand
(
    string Username,
    string Email
) 
    : ICommand;