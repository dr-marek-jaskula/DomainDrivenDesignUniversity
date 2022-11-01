using Scrutor;
using Shopway.Infrastructure.Providers;
using Shopway.Infrastructure.Adapters;
using Shopway.Persistence.Interceptors;
using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Microsoft.Extensions.Logging;
using Shopway.Domain.Repositories;
using Shopway.Persistence.Repositories;

namespace Shopway.App.Registration;

public static class ServiceRegistration
{
    public static void RegisterServices(this IServiceCollection services)
    {
        //TODO this scans also for adapters and providers?
        services.Scan(selector => selector
                    .FromAssemblies(
                        Shopway.Infrastructure.AssemblyReference.Assembly,
                        Shopway.Persistence.AssemblyReference.Assembly)
                    .AddClasses(false)
                    .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

        //Repositories

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

        //Interceptors

        services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
        services.AddSingleton<UpdateAuditableEntitiesInterceptor>();

        //Providers

        services.AddScoped<IDateTimeProvider, DateTimeProvider>();

        //Adapters

        services.AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
    }
}
