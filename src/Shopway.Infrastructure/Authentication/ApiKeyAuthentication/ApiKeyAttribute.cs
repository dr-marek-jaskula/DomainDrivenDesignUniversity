using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using static Shopway.Infrastructure.Authentication.ApiKeyAuthentication.ApiKeyConstants;

namespace Shopway.Infrastructure.Authentication.ApiKeyAuthentication;

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
        private readonly IConfiguration _configuration;
        private readonly RequiredApiKey _requiredApiKey;

        public ApiKeyFilter(RequiredApiKey requiredApiKey, IConfiguration configuration)
        {
            _requiredApiKey = requiredApiKey;
            _configuration = configuration;
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

            //For tutorial purpose, api keys are stored in appsettings
            var requiredApiKeyValue = _configuration
                .GetValue<string>($"{ApiKeySection}:{_requiredApiKey}")!;

            bool isRequestApiKeyEqualToRequiredApiKey = requiredApiKeyValue.Equals(apiKeyFromHeader);

            if (isRequestApiKeyEqualToRequiredApiKey is false)
            {
                context.Result = new UnauthorizedObjectResult("Invalid Api Key");
                return;
            }
        }
    }
}
