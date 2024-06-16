using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Users.Queries.GetPermissionDetails;

public sealed record GetPermissionDetailsQuery(string Permission) : IQuery<PermissionDetailsResponse>;
