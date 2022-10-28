using MediatR;

namespace Shopway.App.Registration;

public static class DecoractorsRegistration
{
    public static void RegisterServiceDecorators(this IServiceCollection services)
    {
        //TODO DomainEventHandler!
        services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentDomainEventHandler<>));
    }
}
