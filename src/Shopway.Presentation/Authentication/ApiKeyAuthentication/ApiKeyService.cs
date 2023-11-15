using Microsoft.Extensions.Configuration;
using static Shopway.Presentation.Authentication.ApiKeyAuthentication.ApiKeyConstants;

namespace Shopway.Presentation.Authentication.ApiKeyAuthentication;

public sealed class ApiKeyService(IConfiguration configuration) : IApiKeyService
{
    public bool IsProvidedApiKeyEqualToRequiredApiKey(RequiredApiKey requiredApiKey, string? apiKeyFromHeader)
    {
        //For tutorial purpose, api keys are stored in appsettings
        var requiredApiKeyValue = configuration
            .GetValue<string>($"{ApiKeySection}:{requiredApiKey}")!;

        return requiredApiKeyValue.Equals(apiKeyFromHeader);
    }
}