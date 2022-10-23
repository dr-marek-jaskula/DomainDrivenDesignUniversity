using MediatR;
using Shopway.Domain.DomainEvents;

namespace Shopway.Application.Abstractions;

public interface IDomainEventHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : IDomainEvent
{
}

