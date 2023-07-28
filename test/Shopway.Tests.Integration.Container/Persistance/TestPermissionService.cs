using Microsoft.AspNetCore.Authorization;
using Shopway.Domain.EntityIds;
using Shopway.Domain.Results;
using Shopway.Infrastructure.Authentication.PermissionAuthentication;

namespace Shopway.Tests.Integration.Container.Persistance;

public sealed class TestPermissionService : IPermissionService
{
    private static readonly Ulid _testUserUlid = Ulid.Parse("01AN4Z07BY79KA1307SR9X4MV3");

    public Result<UserId> GetUserId(AuthorizationHandlerContext context)
    {
        return UserId.Create(_testUserUlid);
    }

    public Task<bool> HasPermissionAsync(UserId userId, string permission)
    {
        return Task.FromResult(true);
    }
}