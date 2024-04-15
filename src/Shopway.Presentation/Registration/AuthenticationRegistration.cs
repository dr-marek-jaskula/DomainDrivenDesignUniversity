using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Shopway.Presentation.Authentication.ApiKeyAuthentication.Handlers;
using Shopway.Presentation.Authentication.OrderHeaders.OrderHeaderCreatedByUser;
using Shopway.Presentation.Authentication.RolePermissionAuthentication.Handlers;

namespace Microsoft.Extensions.DependencyInjection;

internal static class AuthenticationRegistration
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

        services.AddAuthorization(options =>
        {
            options.AddPolicy(OrderHeaderCreatedByUserRequirement.PolicyName, policy => policy.Requirements.Add(new OrderHeaderCreatedByUserRequirement()));
        });

        services.AddScoped<IAuthorizationHandler, OrderHeaderCreatedByUserRequirementHandler>();
        services.AddScoped<IAuthorizationHandler, PermissionRequirementHandler>();
        services.AddScoped<IAuthorizationHandler, RoleRequirementHandler>();
        services.AddScoped<IAuthorizationHandler, ApiKeyRequirementHandler>();

        return services;
    }
}