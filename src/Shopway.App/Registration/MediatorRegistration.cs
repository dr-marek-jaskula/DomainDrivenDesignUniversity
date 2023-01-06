using MediatR;
using Shopway.Application.Pipelines;
using Shopway.Application.Pipelines.CQRS;

namespace Shopway.App.Registration;

public static class MediatorRegistration
{
    public static IServiceCollection RegisterMediator(this IServiceCollection services)
    {
        services.AddMediatR(Shopway.Application.AssemblyReference.Assembly);

        //Register Pipeline Behaviors

        services
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(FluentValidationPipeline<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(QueryTransactionPipeline<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ListQueryTransactionPipeline<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(CommandTransactionPipeline<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(CommandWithResponseTransactionPipeline<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ReferenceValidationPipeline<,>));

        return services;
    }
}
