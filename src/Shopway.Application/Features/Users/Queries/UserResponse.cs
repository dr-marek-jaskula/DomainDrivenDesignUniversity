using Shopway.Application.Abstractions;
using Shopway.Domain.Common.DataProcessing.Abstractions;

namespace Shopway.Application.Features.Users.Queries;

public sealed record UserResponse
(
    Ulid Id,
    string Username,
    string Email,
    Ulid? CustomerId
)
    : IResponse, IHasCursor;