using FastEndpoints.Security;
using Microsoft.AspNetCore.Authorization;
using Shopway.Domain.Common.Enums;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Users;
using Shopway.Domain.Users.Authorization;
using Shopway.Presentation.Authentication.RolePermissionAuthentication;
using System.Security.Claims;

namespace Shopway.Presentation.Authentication.Services;

public sealed class UserAuthorizationService(IAuthorizationRepository authorizationRepository) : IUserAuthorizationService
{
    private readonly IAuthorizationRepository _authorizationRepository = authorizationRepository;

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

    public async Task<bool> HasRolesAsync(UserId userId, params RoleName[] roles)
    {
        return await _authorizationRepository
            .HasRolesAsync(userId, roles);
    }

    public async Task<bool> HasPermissionsAsync(UserId userId, LogicalOperation logicalOperation, params PermissionName[] permissions)
    {
        if (permissions.Length is 0)
        {
            return true;
        }

        return await _authorizationRepository
            .HasPermissionsAsync(userId, permissions, logicalOperation);
    }

    public async Task<bool> HasPermissionToReadAsync(UserId userId, string entity, List<string> requestedProperties)
    {
        if (requestedProperties.Count is 0)
        {
            return true;
        }

        return await _authorizationRepository
            .HasPermissionToReadAsync(userId, entity, requestedProperties);
    }
}
