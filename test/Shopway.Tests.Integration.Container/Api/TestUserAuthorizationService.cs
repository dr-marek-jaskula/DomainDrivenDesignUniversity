using Microsoft.AspNetCore.Authorization;
using Shopway.Domain.Common.Enums;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Enums;
using Shopway.Domain.Users;
using Shopway.Presentation.Authentication.RolePermissionAuthentication;

namespace Shopway.Tests.Integration.Container.Api;

public sealed class TestUserAuthorizationService : IUserAuthorizationService
{
    private static readonly Ulid _testUserUlid = Ulid.Parse("01AN4Z07BY79KA1307SR9X4MV3");

    public Result<UserId> GetUserId(AuthorizationHandlerContext context)
    {
        return UserId.Create(_testUserUlid);
    }

    public Task<bool> HasPermissionsAsync(UserId userId, LogicalOperation logicalOperation, params Permission[] permissions)
    {
        return Task.FromResult(true);
    }

    public Task<bool> HasRolesAsync(UserId userId, params Role[] roles)
    {
        return Task.FromResult(true);
    }
}