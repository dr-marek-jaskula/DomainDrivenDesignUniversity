using Shopway.Application.Abstractions;

namespace Shopway.Application.Features.Users.Queries.GetPermissionDetails;

public sealed record PermissionDetailsResponse
(
    int Id,
    string Name,
    string? RelatedAggregateRoot,
    string? RelatedEntity,
    string Type,
    List<string>? Properties
)
    : IResponse;
