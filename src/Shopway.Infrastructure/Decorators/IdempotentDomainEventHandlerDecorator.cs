using Shopway.Domain.DomainEvents;
using Shopway.Application.Abstractions;
using Shopway.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Shopway.Persistence.Framework;

namespace Shopway.Infrastructure.Decoratos;

public sealed class IdempotentDomainEventHandlerDecorator<TDomainEvent> : IDomainEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    private readonly INotificationHandler<TDomainEvent> _decorated;
    private readonly ShopwayDbContext _dbContext;

    public IdempotentDomainEventHandlerDecorator(
        INotificationHandler<TDomainEvent> decorated,
        ShopwayDbContext dbContext)
    {
        _decorated = decorated;
        _dbContext = dbContext;
    }

    public async Task Handle(TDomainEvent notification, CancellationToken cancellationToken)
    {
        string consumer = _decorated.GetType().Name;

        bool isConsumerAlreadyProcessed = await _dbContext.Set<OutboxMessageConsumer>()
                            .AnyAsync(outboxMessageConsumer =>
                            outboxMessageConsumer.Id == notification.Id 
                            && outboxMessageConsumer.Name == consumer,
                            cancellationToken);

        if (isConsumerAlreadyProcessed)
        {
            return;
        }

        await _decorated.Handle(notification, cancellationToken);

        _dbContext.Set<OutboxMessageConsumer>()
            .Add(new OutboxMessageConsumer
            {
                Id = notification.Id,
                Name = consumer
            });

        //At this stage we do not want to cancel the operation, so we will pass the CancellationToken.None.
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }
}
