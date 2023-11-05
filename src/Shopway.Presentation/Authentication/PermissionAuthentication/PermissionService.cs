using Shopway.Domain.Errors;
using System.Security.Claims;
using Shopway.Domain.Results;
using Shopway.Domain.EntityIds;
using Microsoft.AspNetCore.Authorization;
using Shopway.Domain.Abstractions.Repositories;

namespace Shopway.Presentation.Authentication.PermissionAuthentication;

public sealed class PermissionService : IPermissionService
{
    private readonly IPermissionRepository _permissionRepository;

    public PermissionService(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public Result<UserId> GetUserId(AuthorizationHandlerContext context)
    {
        string? userIdentifier = context
            .User
            .Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)
            ?.Value;

        if (Ulid.TryParse(userIdentifier, out Ulid userUlid) is false)
        {
            return Result.Failure<UserId>(Error.ParseFailure<Ulid>(nameof(ClaimTypes.NameIdentifier)));
        }

        return UserId.Create(userUlid);
    }

    public async Task<bool> HasPermissionAsync(UserId userId, string permission)
    {
        return await _permissionRepository
            .HasPermissionAsync(userId, permission);
    }
}