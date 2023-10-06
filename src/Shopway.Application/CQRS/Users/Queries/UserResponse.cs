using Shopway.Application.Abstractions;
using Shopway.Domain.Abstractions.Common;

namespace Shopway.Application.CQRS.Users.Queries;

public sealed record UserResponse
(
    Ulid Id,
    string Username,
    string Email,
    Ulid? CustomerId
)
    : IResponse, IHasCursor;