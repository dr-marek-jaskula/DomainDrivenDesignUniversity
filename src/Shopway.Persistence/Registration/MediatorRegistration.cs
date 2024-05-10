using Shopway.Persistence;
using Shopway.Persistence.Pipelines;

namespace Microsoft.Extensions.DependencyInjection;

internal static class MediatorRegistration
{
    internal static IServiceCollection RegisterMediator(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(AssemblyReference.Assembly);

            configuration.AddOpenBehavior(typeof(QueryTransactionPipeline<,>));
            configuration.AddOpenBehavior(typeof(CommandTransactionPipeline<,>));
            configuration.AddOpenBehavior(typeof(CommandWithResponseTransactionPipeline<,>));
            configuration.AddOpenBehavior(typeof(ReferenceValidationPipeline<,>));
        });

        return services;
    }
}
