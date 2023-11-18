using Microsoft.AspNetCore.Authorization;

namespace Shopway.Presentation.Authentication.PermissionAuthentication.Requirements;

public sealed class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}
