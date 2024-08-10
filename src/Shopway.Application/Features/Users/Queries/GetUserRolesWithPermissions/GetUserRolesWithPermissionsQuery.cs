using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Users.Queries.GetUserRolesWithPermissions;

public sealed record GetUserRolesWithPermissionsQuery(string Username) : IQuery<RolesWithPermissionsResponse>;
