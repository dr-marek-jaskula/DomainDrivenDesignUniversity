using Microsoft.AspNetCore.Authorization;
using Shopway.Domain.EntityIds;
using Shopway.Domain.Results;
using Shopway.Infrastructure.Authentication.PermissionAuthentication;

namespace Shopway.Tests.Integration.Container.Persistance;

public sealed class TestPermissionService : IPermissionService
{
    private static readonly Guid _testUserGuid = Guid.Parse("2df39cb3-8645-4462-8d3f-06b6f1547b9f");

    public Result<UserId> GetUserId(AuthorizationHandlerContext context)
    {
        return UserId.Create(_testUserGuid);
    }

    public Task<bool> HasPermissionAsync(UserId userId, string permission)
    {
        return Task.FromResult(true);
    }
}