using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.CQRS.Users.Commands.CreateUser;

public sealed record CreateUserCommand
(
    string Username,
    string Email,
    string Password,
    string ConfirmPassword

) 
    : ICommand<CreateUserResponse>;