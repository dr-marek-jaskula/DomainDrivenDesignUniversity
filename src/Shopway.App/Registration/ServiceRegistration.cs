using Scrutor;
using Shopway.Infrastructure.Providers;
using Shopway.Infrastructure.Adapters;
using Shopway.Application.Abstractions;
using Shopway.Persistence.Repositories;
using Shopway.Infrastructure.Validators;
using Shopway.Domain.Abstractions.Repositories;
using Microsoft.AspNetCore.Identity;
using Shopway.Domain.Entities;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        //Services

        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

        //Repositories

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        //Validators

        services.AddScoped<IValidator, Validator>();

        //Providers

        services.AddScoped<IDateTimeProvider, DateTimeProvider>();

        //Adapters

        services.AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));

        //Scan for the rest

        services.Scan(selector => selector
            .FromAssemblies(
                Shopway.Infrastructure.AssemblyReference.Assembly,
                Shopway.Persistence.AssemblyReference.Assembly)
            .AddClasses(false)
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}
