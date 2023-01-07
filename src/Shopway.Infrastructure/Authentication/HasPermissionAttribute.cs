using Microsoft.AspNetCore.Authorization;
using Shopway.Domain.Enums;

namespace Shopway.Infrastructure.Authentication;

public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(Permissiond permission)
        : base(policy: permission.ToString())
    {
    }
}
