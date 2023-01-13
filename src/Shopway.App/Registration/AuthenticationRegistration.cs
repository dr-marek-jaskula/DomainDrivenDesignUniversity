using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Shopway.Infrastructure.Abstractions;
using Shopway.Infrastructure.Authentication.Requirements;
using Shopway.Infrastructure.Providers;
using Shopway.Infrastructure.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class AuthenticationRegistration
{
    public static IServiceCollection RegisterAuthentication(this IServiceCollection services)
    {
        services.AddScoped<IUserContextService, UserContextService>();
        services.AddHttpContextAccessor();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        services.AddAuthorization();

        services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        return services;
    }
}