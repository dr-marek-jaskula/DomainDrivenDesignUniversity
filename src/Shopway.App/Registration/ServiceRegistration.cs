using Scrutor;
using Shopway.Infrastructure.Providers;
using Shopway.Infrastructure.Adapters;
using Shopway.Application.Abstractions;
using Shopway.Domain.Repositories;
using Shopway.Persistence.Repositories;
using CustomTools.Validator;

namespace Shopway.App.Registration;

public static class ServiceRegistration
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        //Repositories

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

        //Validators

        services.AddScoped<Validator>();
        services.AddScoped(typeof(Validator<>));

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
