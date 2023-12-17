using Microsoft.AspNetCore.Authorization;

namespace Shopway.Presentation.Authentication.ApiKeyAuthentication;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
public sealed class RequiredApiKeyAttribute(RequiredApiKey requiredApiKey)
    : Attribute, IAuthorizationRequirement, IAuthorizationRequirementData
{
    public RequiredApiKey RequiredApiKey { get; } = requiredApiKey;

    public IEnumerable<IAuthorizationRequirement> GetRequirements()
    {
        yield return this;
    }
}