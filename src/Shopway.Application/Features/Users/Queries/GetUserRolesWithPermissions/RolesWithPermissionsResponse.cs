using Shopway.Application.Abstractions;

namespace Shopway.Application.Features.Users.Queries.GetUserRolesWithPermissions;

public sealed record RolesWithPermissionsResponse(Dictionary<string, List<string>> RolesWithPermissions) : IResponse;
