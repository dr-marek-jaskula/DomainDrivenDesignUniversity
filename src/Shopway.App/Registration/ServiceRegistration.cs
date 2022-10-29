using Scrutor;
using Shopway.Infrastructure.Providers;
using Shopway.Infrastructure.Adapters;
using Shopway.Persistence.Interceptors;

namespace Shopway.App.Registration;

public static class ServiceRegistration
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.Scan(selector => selector
                    .FromAssemblies(
                        Shopway.Infrastructure.AssemblyReference.Assembly,
                        Shopway.Persistence.AssemblyReference.Assembly)
                    .AddClasses(false)
                    .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

        //Interceptors

        services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
        services.AddSingleton<UpdateAuditableEntitiesInterceptor>();

        //Providers

        services.AddScoped<IDateTimeProvider, DateTimeProvider>();

        //Adapters

        services.AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
    }
}
