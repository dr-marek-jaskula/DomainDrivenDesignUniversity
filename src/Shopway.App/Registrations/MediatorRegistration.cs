using Shopway.Application.Pipelines;
using Shopway.Persistence.Pipelines;

namespace Microsoft.Extensions.DependencyInjection;

public static class MediatorRegistration
{
    public static IServiceCollection RegisterMediator(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(Shopway.Application.AssemblyReference.Assembly);
            configuration.AddOpenBehavior(typeof(LoggingPipeline<,>));
            configuration.AddOpenBehavior(typeof(FluentValidationPipeline<,>));
            configuration.AddOpenBehavior(typeof(QueryTransactionPipeline<,>));
            configuration.AddOpenBehavior(typeof(CommandTransactionPipeline<,>));
            configuration.AddOpenBehavior(typeof(CommandWithResponseTransactionPipeline<,>));
            configuration.AddOpenBehavior(typeof(ReferenceValidationPipeline<,>));
        });

        return services;
    }
}
