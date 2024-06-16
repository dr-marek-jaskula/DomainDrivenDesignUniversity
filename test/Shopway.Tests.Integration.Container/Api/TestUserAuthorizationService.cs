using Microsoft.AspNetCore.Authorization;
using Shopway.Domain.Common.Enums;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Users;
using Shopway.Domain.Users.Authorization;
using Shopway.Presentation.Authentication.RolePermissionAuthentication;

namespace Shopway.Tests.Integration.Container.Api;

public sealed class TestUserAuthorizationService : IUserAuthorizationService
{
    private static readonly Ulid _testUserUlid = Ulid.Parse("01AN4Z07BY79KA1307SR9X4MV3");

    public Result<UserId> GetUserId(AuthorizationHandlerContext context)
    {
        return UserId.Create(_testUserUlid);
    }

    public Task<bool> HasPermissionsAsync(UserId userId, LogicalOperation logicalOperation, params PermissionName[] permissions)
    {
        return Task.FromResult(true);
    }

    public Task<bool> HasPermissionToReadAsync(UserId userId, string entity, List<string> requestedProperties)
    {
        return Task.FromResult(true);
    }

    public Task<bool> HasRolesAsync(UserId userId, params RoleName[] roles)
    {
        return Task.FromResult(true);
    }
}
