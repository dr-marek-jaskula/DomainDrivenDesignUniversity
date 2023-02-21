using Shopway.Application.Abstractions;

namespace Shopway.Application.CQRS.Users.Commands.RegisterUser;

public sealed record RegisterUserResponse
(
    Guid Id
) : IResponse;