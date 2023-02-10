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
    public ApiKeyAttribute(RequiredApiKeyName requiredApiKeyName) : base(typeof(ApiKeyFilter))
    {
        Arguments = new object[] { requiredApiKeyName };
    }

    /// <summary>
    /// Api key filter, used to handle the api key authorization
    /// </summary>
    public sealed class ApiKeyFilter : IAuthorizationFilter
    {
        private readonly IConfiguration _configuration;
        private readonly RequiredApiKeyName _requiredApiKeyName;

        public ApiKeyFilter(RequiredApiKeyName requiredApiKeyName, IConfiguration configuration)
        {
            _requiredApiKeyName = requiredApiKeyName;
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
                .GetValue<string>($"{ApiKeySection}:{_requiredApiKeyName}")!;

            bool isRequestApiKeyEqualToRequiredApiKey = requiredApiKeyValue.Equals(apiKeyFromHeader);

            if (isRequestApiKeyEqualToRequiredApiKey is false)
            {
                context.Result = new UnauthorizedObjectResult("Invalid Api Key");
                return;
            }
        }
    }
}
