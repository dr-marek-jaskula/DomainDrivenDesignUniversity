using MediatR;
using Shopway.Domain.Abstractions.BaseTypes;

namespace Shopway.Application.Abstractions;

public interface IDomainEventHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : IDomainEvent
{
}

