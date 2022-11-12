using Scrutor;
using Shopway.Infrastructure.Providers;
using Shopway.Infrastructure.Adapters;
using Shopway.Persistence.Interceptors;
using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Microsoft.Extensions.Logging;
using Shopway.Domain.Repositories;
using Shopway.Persistence.Repositories;
using Shopway.Persistence.Specifications.Products;

namespace Shopway.App.Registration;

public static class ServiceRegistration
{
    public static void RegisterServices(this IServiceCollection services)
    {
        //Repositories

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

        //Interceptors
        //I use the UnitOfWork instead of interceptors, but they are an alternate way of doing things

        //services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
        //services.AddSingleton<UpdateAuditableEntitiesInterceptor>();

        //Providers

        services.AddScoped<IDateTimeProvider, DateTimeProvider>();

        //Adapters

        services.AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));

        //Specification

        services.AddScoped<ProductByIdWithIncludesSpecification>();
        services.AddScoped<ProductByIdWithReviewsSpecification>();

        //Scan for the rest

        services.Scan(selector => selector
            .FromAssemblies(
                Shopway.Infrastructure.AssemblyReference.Assembly,
                Shopway.Persistence.AssemblyReference.Assembly)
            .AddClasses(false)
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsImplementedInterfaces()
            .WithScopedLifetime());
    }
}
