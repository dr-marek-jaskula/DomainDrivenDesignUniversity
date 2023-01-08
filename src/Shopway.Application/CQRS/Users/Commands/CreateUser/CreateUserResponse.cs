using Shopway.Application.Abstractions;

namespace Shopway.Application.CQRS.Users.Commands.CreateUser;

public sealed record CreateUserResponse
(
    Guid Id
) : IResponse;