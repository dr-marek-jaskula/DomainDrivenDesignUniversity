using Shopway.Presentation.Authentication.ApiKeyAuthentication;

namespace Shopway.Tests.Integration.Container.Api;

public sealed class TestApiKeyService<TEnum> : IApiKeyService<TEnum>
    where TEnum : struct, Enum
{
    public bool IsProvidedApiKeyEqualToRequiredApiKey(TEnum requiredApiKey, string? apiKeyFromHeader)
    {
        return true;
    }
}
