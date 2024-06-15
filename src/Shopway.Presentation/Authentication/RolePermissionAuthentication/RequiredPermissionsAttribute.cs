using Microsoft.AspNetCore.Authorization;
using Shopway.Domain.Common.Enums;
using Shopway.Domain.Users.Authorization;

namespace Shopway.Presentation.Authentication.RolePermissionAuthentication;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
public sealed class RequiredPermissionsAttribute(params Permission[] permissions)
    : AuthorizeAttribute, IAuthorizationRequirement, IAuthorizationRequirementData
{
    public Permission[] Permissions { get; } = permissions;
    public LogicalOperation LogicalOperation { get; } = LogicalOperation.And;

    public RequiredPermissionsAttribute(LogicalOperation logicalOperation, params Permission[] permissions)
        : this(permissions)
    {
        LogicalOperation = logicalOperation;
    }

    public IEnumerable<IAuthorizationRequirement> GetRequirements()
    {
        yield return this;
    }
}
