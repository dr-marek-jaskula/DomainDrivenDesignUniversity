using MediatR;
using Shopway.Domain.Abstractions;

namespace Shopway.Application.Abstractions;

public interface IDomainEventHandler<in TEvent> : INotificationHandler<TEvent>
    where TEvent : IDomainEvent
{
}

