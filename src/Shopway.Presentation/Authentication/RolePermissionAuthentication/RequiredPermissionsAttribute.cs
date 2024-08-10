using Microsoft.AspNetCore.Authorization;
using Shopway.Domain.Common.Enums;

namespace Shopway.Presentation.Authentication.RolePermissionAuthentication;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
public sealed class RequiredPermissionsAttribute<TPermission>(params TPermission[] permissions)
    : AuthorizeAttribute, IAuthorizationRequirement, IAuthorizationRequirementData
    where TPermission : struct, Enum
{
    public TPermission[] Permissions { get; } = permissions;
    public LogicalOperation LogicalOperation { get; } = LogicalOperation.And;

    public RequiredPermissionsAttribute(LogicalOperation logicalOperation, params TPermission[] permissions)
        : this(permissions)
    {
        LogicalOperation = logicalOperation;
    }

    public IEnumerable<IAuthorizationRequirement> GetRequirements()
    {
        yield return this;
    }
}
