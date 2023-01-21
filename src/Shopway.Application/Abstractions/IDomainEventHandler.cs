using MediatR;
using Shopway.Domain.BaseTypes;

namespace Shopway.Application.Abstractions;

public interface IDomainEventHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : IDomainEvent
{
}

