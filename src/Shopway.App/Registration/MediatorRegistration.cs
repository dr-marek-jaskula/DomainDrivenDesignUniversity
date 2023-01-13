using MediatR;
using Shopway.Application.Pipelines;
using Shopway.Application.Pipelines.CQRS;
using Shopway.Application.Pipelines.ValidationPipelines;

namespace Microsoft.Extensions.DependencyInjection;

public static class MediatorRegistration
{
    public static IServiceCollection RegisterMediator(this IServiceCollection services)
    {
        services.AddMediatR(Shopway.Application.AssemblyReference.Assembly);

        //Register Pipeline Behaviors
        services
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipeline<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(FluentValidationPipeline<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(QueryTransactionPipeline<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ListQueryTransactionPipeline<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(CommandTransactionPipeline<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(CommandWithResponseTransactionPipeline<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ReferenceValidationPipeline<,>));

        return services;
    }
}
