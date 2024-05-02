using Microsoft.Extensions.DependencyInjection;
using Shopway.Application.Features.Orders.Commands.CreateHeaderOrder;
using Shopway.Application.Pipelines;

namespace Shopway.Application.Registration;

internal static class MediatorRegistration
{
    internal static IServiceCollection RegisterMediator(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(AssemblyReference.Assembly);

            configuration.AddOpenBehavior(typeof(FluentValidationPipeline<,>));
            configuration.AddOpenBehavior(typeof(LoggingPipeline<,>));
            configuration.AddOpenBehavior(typeof(QueryCachingPipeline<,>));

            configuration.AddBehavior<CreateOrderHeaderOpenTelemetryPipeline>();
        });

        return services;
    }
}
