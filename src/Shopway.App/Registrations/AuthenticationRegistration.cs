using Shopway.Infrastructure.Services;
using Shopway.Infrastructure.Providers;
using Shopway.Persistence.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Shopway.Infrastructure.Authentication.PermissionAuthentication.Requirements;

namespace Microsoft.Extensions.DependencyInjection;

public static class AuthenticationRegistration
{
    public static IServiceCollection RegisterAuthentication(this IServiceCollection services)
    {
        services.AddScoped<IUserContextService, UserContextService>();
        services.AddHttpContextAccessor();

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer();

        services.AddAuthorization();

        services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        return services;
    }
}