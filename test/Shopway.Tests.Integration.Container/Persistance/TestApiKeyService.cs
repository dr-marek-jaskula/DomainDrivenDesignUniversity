using Shopway.Infrastructure.Authentication.ApiKeyAuthentication;

namespace Shopway.Tests.Integration.Container.Persistance;

public sealed class TestApiKeyService : IApiKeyService
{
    public bool IsProvidedApiKeyEqualToRequiredApiKey(RequiredApiKey requiredApiKey, string? apiKeyFromHeader)
    {
        return true;
    }
}