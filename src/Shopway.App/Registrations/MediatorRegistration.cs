using Shopway.Application.Pipelines;
using Shopway.Application.Pipelines.CQRS;
using Shopway.Application.Pipelines.CQRS.Batch;
using Shopway.Application.Pipelines.ValidationPipelines;

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
            configuration.AddOpenBehavior(typeof(PageQueryTransactionPipeline<,>));
            configuration.AddOpenBehavior(typeof(ListQueryTransactionPipeline<,>));
            configuration.AddOpenBehavior(typeof(CommandTransactionPipeline<,>));
            configuration.AddOpenBehavior(typeof(CommandWithResponseTransactionPipeline<,>));
            configuration.AddOpenBehavior(typeof(BatchCommandTransactionPipeline<,>));
            configuration.AddOpenBehavior(typeof(BatchCommandWithResponseTransactionPipeline<,>));
            configuration.AddOpenBehavior(typeof(ReferenceValidationPipeline<,>));
        });

        return services;
    }
}
