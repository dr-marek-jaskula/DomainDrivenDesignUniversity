using MediatR;
using Shopway.Application.Behaviors;

namespace Shopway.App.Registration;

public static class MediatorRegistration
{
    public static void RegisterMediator(this IServiceCollection services)
    {
        services.AddMediatR(Shopway.Application.AssemblyReference.Assembly);

        //Register Pipeline Behaviors

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
    }
}
