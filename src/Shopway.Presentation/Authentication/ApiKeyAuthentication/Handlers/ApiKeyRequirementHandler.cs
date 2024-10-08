﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using static Shopway.Presentation.Authentication.ApiKeyAuthentication.Constants.ApiKey;

namespace Shopway.Presentation.Authentication.ApiKeyAuthentication.Handlers;

public sealed class ApiKeyRequirementHandler<TEnum>(IHttpContextAccessor httpContextAccessor, IApiKeyService<TEnum> apiKeyService) : AuthorizationHandler<RequiredApiKeyAttribute<TEnum>>
    where TEnum : struct, Enum
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IApiKeyService<TEnum> _apiKeyService = apiKeyService;

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RequiredApiKeyAttribute<TEnum> requirement)
    {
        var httpContext = _httpContextAccessor.HttpContext ?? throw new NullReferenceException("HTTP context is not accessible.");

        bool apiKeyHeaderPresentInTheRequest = httpContext
            .Request
            .Headers
            .TryGetValue(ApiKeyHeader, out var apiKeyFromHeader);

        if (apiKeyHeaderPresentInTheRequest is false)
        {
            context.Fail(new AuthorizationFailureReason(this, "Api Key not found."));
            return Task.FromResult(requirement);
        }

        if (_apiKeyService.IsProvidedApiKeyEqualToRequiredApiKey(requirement.RequiredApiKey, apiKeyFromHeader) is false)
        {
            context.Fail(new AuthorizationFailureReason(this, "Invalid Api Key."));
            return Task.FromResult(requirement);
        }

        context.Succeed(requirement);
        return Task.FromResult(requirement);
    }
}
