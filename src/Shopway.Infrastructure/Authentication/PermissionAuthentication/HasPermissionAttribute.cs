using Microsoft.AspNetCore.Authorization;
using Shopway.Domain.Enums;

namespace Shopway.Infrastructure.Authentication.PermissionAuthentication;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(Permission permission)
        : base(policy: permission.ToString())
    {
    }
}
