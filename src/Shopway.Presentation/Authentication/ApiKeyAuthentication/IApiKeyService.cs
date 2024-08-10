namespace Shopway.Presentation.Authentication.ApiKeyAuthentication;

public interface IApiKeyService<in TEnum> 
    where TEnum : struct, Enum
{
    bool IsProvidedApiKeyEqualToRequiredApiKey(TEnum requiredApiKey, string? apiKeyFromHeader);
}
