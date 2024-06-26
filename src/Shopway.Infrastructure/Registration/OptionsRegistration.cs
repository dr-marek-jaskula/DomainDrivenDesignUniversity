﻿using Microsoft.Extensions.Options;
using Shopway.Infrastructure.Options;
using Shopway.Infrastructure.Validators;

namespace Microsoft.Extensions.DependencyInjection;

internal static class OptionsRegistration
{
    internal static IServiceCollection RegisterOptions(this IServiceCollection services)
    {
        services.ConfigureOptions<DatabaseOptionsSetup>();
        services.ConfigureOptions<CacheOptionsSetup>();
        services.ConfigureOptions<AuthenticationOptionsSetup>();
        services.ConfigureOptions<BearerAuthenticationOptionsSetup>();
        services.ConfigureOptions<GoogleAuthorizationOptionsSetup>();
        services.ConfigureOptions<GoogleOptionsPostSetup>();
        services.ConfigureOptions<HealthCheckOptionsSetup>();
        services.ConfigureOptions<MailSenderOptionsSetup>();
        services.ConfigureOptions<OpenTelemetryOptionsSetup>();

        services.AddSingleton<IValidateOptions<GoogleAuthorizationOptions>, GoogleAuthorizationOptionsValidator>();
        services.AddSingleton<IValidateOptions<DatabaseOptions>, DatabaseOptionsValidator>();
        services.AddSingleton<IValidateOptions<AuthenticationOptions>, AuthenticationOptionsValidator>();
        services.AddSingleton<IValidateOptions<HealthOptions>, HealthOptionsValidator>();
        services.AddSingleton<IValidateOptions<CacheOptions>, CacheOptionsValidator>();
        services.AddSingleton<IValidateOptions<MailSenderOptions>, MailSenderOptionsValidator>();
        services.AddSingleton<IValidateOptions<OpenTelemetryOptions>, OpenTelemetryOptionsValidator>();

        return services;
    }
}
