using Shopway.Application.Abstractions;

namespace Shopway.Application.CQRS.Users.Queries;

public sealed record UserResponse
(
    Guid Id,
    string Username,
    string Email,
    Guid? CustomerId
)
    : IResponse;