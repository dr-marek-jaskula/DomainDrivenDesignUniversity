namespace Shopway.Presentation.Authentication.ApiKeyAuthentication;

public interface IApiKeyService
{
    bool IsProvidedApiKeyEqualToRequiredApiKey(RequiredApiKey requiredApiKey, string? apiKeyFromHeader);
}
