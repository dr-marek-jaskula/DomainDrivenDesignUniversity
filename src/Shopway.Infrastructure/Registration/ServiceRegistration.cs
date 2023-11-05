using Scrutor;
using Shopway.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Shopway.Application.Abstractions;
using Shopway.Infrastructure.Services;
using Shopway.Infrastructure.Providers;
using Shopway.Infrastructure.Validators;
using Shopway.Infrastructure.Builders.Batch;
using Shopway.Application.Abstractions.CQRS.Batch;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceRegistration
{
    internal static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        //Services

        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IUserContextService, UserContextService>();

        //Validators

        services.AddScoped<IValidator, Validator>();

        //Builders

        services.AddScoped(typeof(IBatchResponseBuilder<,>), typeof(BatchResponseBuilder<,>));

        //Providers

        services.AddScoped<IDateTimeProvider, DateTimeProvider>();

        //Scan for the rest

        services.Scan(selector => selector
            .FromAssemblies(
                Shopway.Infrastructure.AssemblyReference.Assembly)
            .AddClasses(false)
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsMatchingInterface()
            .WithScopedLifetime());

        return services;
    }
}
