using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Users.Queries.GetRolePermissions;

public sealed record GetRolePermissionsQuery(string Role) : IQuery<RolePermissionsResponse>;