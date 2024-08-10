using Microsoft.AspNetCore.Authorization;

namespace Shopway.Presentation.Authentication.ApiKeyAuthentication;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
public sealed class RequiredApiKeyAttribute<TEnum>(TEnum requiredApiKey) : Attribute, IAuthorizationRequirement, IAuthorizationRequirementData
    where TEnum : struct, Enum
{
    public TEnum RequiredApiKey { get; } = requiredApiKey;

    public IEnumerable<IAuthorizationRequirement> GetRequirements()
    {
        yield return this;
    }
}
