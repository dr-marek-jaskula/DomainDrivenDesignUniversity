using Microsoft.AspNetCore.Authorization;

namespace Shopway.Presentation.Authentication.GenericProxy;

public sealed class GenericProxyPropertiesRequirement : IAuthorizationRequirement
{
    public const string PolicyName = nameof(GenericProxy);
}
