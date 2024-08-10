using Microsoft.Extensions.Configuration;
using static Shopway.Presentation.Authentication.ApiKeyAuthentication.Constants.ApiKey;

namespace Shopway.Presentation.Authentication.ApiKeyAuthentication;

public sealed class ApiKeyService<TEnum>(IConfiguration configuration) : IApiKeyService<TEnum>
    where TEnum : struct, Enum
{
    private readonly IConfiguration _configuration = configuration;

    public bool IsProvidedApiKeyEqualToRequiredApiKey(TEnum requiredApiKey, string? apiKeyFromHeader)
    {
        //For tutorial purpose, api keys are stored in appsettings
        var requiredApiKeyValue = _configuration
            .GetValue<string>($"{ApiKeySection}:{requiredApiKey}")!;

        return requiredApiKeyValue.Equals(apiKeyFromHeader);
    }
}
