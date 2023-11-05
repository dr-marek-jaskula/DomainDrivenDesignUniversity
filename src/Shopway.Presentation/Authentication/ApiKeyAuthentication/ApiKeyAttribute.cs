using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using static Shopway.Presentation.Authentication.ApiKeyAuthentication.ApiKeyConstants;

namespace Shopway.Presentation.Authentication.ApiKeyAuthentication;

/// <summary>
/// Use to verify the api key in header "X-Api-Key"
/// </summary>
public sealed class ApiKeyAttribute : TypeFilterAttribute
{
    public ApiKeyAttribute(RequiredApiKey requiredApiKey) : base(typeof(ApiKeyFilter))
    {
        Arguments = new object[] { requiredApiKey };
    }

    /// <summary>
    /// Api key filter, used to handle the api key authorization
    /// </summary>
    private sealed class ApiKeyFilter : IAuthorizationFilter
    {
        private readonly IApiKeyService _apiKeyService;
        private readonly RequiredApiKey _requiredApiKey;

        public ApiKeyFilter(RequiredApiKey requiredApiKey, IApiKeyService apiKeyService)
        {
            _requiredApiKey = requiredApiKey;
            _apiKeyService = apiKeyService;
        }

        /// <summary>
        /// Compare the "X-Api-Key" header value with the required one
        /// </summary>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool isApiKeyHeaderPresentInTheRequest = context
                .HttpContext
                .Request
                .Headers
                .TryGetValue(ApiKeyHeader, out var apiKeyFromHeader);

            if (isApiKeyHeaderPresentInTheRequest is false)
            {
                context.Result = new UnauthorizedObjectResult($"Api Key not found");
                return;
            }

            if (_apiKeyService.IsProvidedApiKeyEqualToRequiredApiKey(_requiredApiKey, apiKeyFromHeader) is false)
            {
                context.Result = new UnauthorizedObjectResult("Invalid Api Key");
                return;
            }
        }
    }
}
