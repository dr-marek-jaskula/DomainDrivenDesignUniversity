using MediatR;
using Shopway.Application.Behaviors;

namespace Shopway.App.Registration;

public static class MediatorRegistration
{
    public static IServiceCollection RegisterMediator(this IServiceCollection services)
    {
        services.AddMediatR(Shopway.Application.AssemblyReference.Assembly);

        //Register Pipeline Behaviors

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

        return services;
    }
}
