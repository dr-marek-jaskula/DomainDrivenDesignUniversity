using Shopway.Application.Abstractions;

namespace Shopway.Application.CQRS.Users.Queries;

public sealed record UserResponse
(
    Ulid Id,
    string Username,
    string Email,
    Ulid? CustomerId
)
    : IResponse;