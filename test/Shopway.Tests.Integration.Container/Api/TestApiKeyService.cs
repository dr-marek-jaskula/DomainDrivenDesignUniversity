using Shopway.Presentation.Authentication.ApiKeyAuthentication;

namespace Shopway.Tests.Integration.Container.Api;

public sealed class TestApiKeyService : IApiKeyService
{
    public bool IsProvidedApiKeyEqualToRequiredApiKey(RequiredApiKey requiredApiKey, string? apiKeyFromHeader)
    {
        return true;
    }
}
