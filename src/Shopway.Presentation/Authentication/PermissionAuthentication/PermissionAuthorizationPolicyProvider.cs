using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Shopway.Presentation.Authentication.PermissionAuthentication.Requirements;

namespace Shopway.Presentation.Authentication.PermissionAuthentication;

public sealed class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
    public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
        : base(options)
    {
    }

    /// <summary>
    /// Automatically registers the policies using PermissionRequirement when asked for the permission policy.
    /// </summary>
    /// <param name="policyName">Input policy</param>
    /// <returns>Authorization policy with permission requirement</returns>
    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        AuthorizationPolicy? policy = await base.GetPolicyAsync(policyName);

        if (policy is not null)
        {
            return policy;
        }

        return new AuthorizationPolicyBuilder()
            .AddRequirements(new PermissionRequirement(policyName))
            .Build();
    }
}
