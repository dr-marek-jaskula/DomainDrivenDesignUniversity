﻿using Microsoft.AspNetCore.Authorization;
using Shopway.Domain.Common.Enums;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Enums;
using Shopway.Domain.Users;

namespace Shopway.Presentation.Authentication.RolePermissionAuthentication;

public interface IUserAuthorizationService
{
    Result<UserId> GetUserId(AuthorizationHandlerContext context);
    Task<bool> HasPermissionsAsync(UserId userId, LogicalOperation logicalOperation, params Permission[] permissions);
    Task<bool> HasRolesAsync(UserId userId, params Role[] roles);
}