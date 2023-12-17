using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Shopway.Presentation.Authentication.ApiKeyAuthentication.Handlers;
using Shopway.Presentation.Authentication.RolePermissionAuthentication.Handlers;

namespace Microsoft.Extensions.DependencyInjection;

public static class AuthenticationRegistration
{
    internal static IServiceCollection RegisterAuthentication(this IServiceCollection services)
    {

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

        services.AddScoped<IAuthorizationHandler, PermissionRequirementHandler>();
        services.AddScoped<IAuthorizationHandler, RoleRequirementHandler>();
        services.AddScoped<IAuthorizationHandler, ApiKeyRequirementHandler>();

        return services;
    }
}