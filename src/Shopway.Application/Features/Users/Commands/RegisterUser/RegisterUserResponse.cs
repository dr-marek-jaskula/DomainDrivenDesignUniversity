using Shopway.Application.Abstractions;

namespace Shopway.Application.Features.Users.Commands.RegisterUser;

public sealed record RegisterUserResponse
(
    Ulid Id
) : IResponse;
