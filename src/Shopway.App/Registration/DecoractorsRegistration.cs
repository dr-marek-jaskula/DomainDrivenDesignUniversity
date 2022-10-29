using Shopway.Infrastructure.Decoratos;
using MediatR;

namespace Shopway.App.Registration;

public static class DecoractorsRegistration
{
    public static void RegisterServiceDecorators(this IServiceCollection services)
    {
        services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentDomainEventHandlerDecorator<>));
    }
}