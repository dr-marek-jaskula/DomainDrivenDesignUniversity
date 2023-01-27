using Microsoft.Extensions.Options;
using Shopway.App.Options;
using Shopway.Infrastructure.Authentication;

namespace Microsoft.Extensions.DependencyInjection;

public static class OptionsRegistration
{
    public static IServiceCollection RegisterOptions(this IServiceCollection services)
    {
        services.ConfigureOptions<DatabaseOptionsSetup>();
        services.ConfigureOptions<CacheOptionsSetup>();
        services.ConfigureOptions<AuthenticationOptionsSetup>();
        services.ConfigureOptions<BearerAuthenticationOptionsSetup>();
        services.ConfigureOptions<HealthCheckOptionsSetup>();

        services.AddSingleton<IValidateOptions<DatabaseOptions>, DatabaseOptionsValidator>();
        services.AddSingleton<IValidateOptions<CacheOptions>, CacheOptionsValidator>();
        services.AddSingleton<IValidateOptions<AuthenticationOptions>, AuthenticationOptionsValidator>();
        services.AddSingleton<IValidateOptions<HealthOptions>, HealthCheckOptionsValidator>();

        return services;
    }
}
