using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Shopway.Infrastructure.Authentication.PermissionAuthentication.Requirements;
using Shopway.Infrastructure.Providers;
using Shopway.Infrastructure.Services;
using Shopway.Persistence.Abstractions;

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