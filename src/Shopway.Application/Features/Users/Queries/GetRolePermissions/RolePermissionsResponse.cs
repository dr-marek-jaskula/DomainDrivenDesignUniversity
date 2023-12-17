using Shopway.Application.Abstractions;

namespace Shopway.Application.Features.Users.Queries.GetRolePermissions;

public sealed record RolePermissionsResponse(List<string> Permissions) : IResponse;