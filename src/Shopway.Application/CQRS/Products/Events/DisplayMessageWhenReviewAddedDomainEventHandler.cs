using Shopway.Application.Abstractions;
using Shopway.Domain.DomainEvents;

namespace Shopway.Application.CQRS.Products.Events;

internal sealed class DisplayMessageWhenReviewAddedDomainEventHandler : IDomainEventHandler<ReviewAddedDomainEvent>
{

    public DisplayMessageWhenReviewAddedDomainEventHandler()
    {
    }

    public async Task Handle(ReviewAddedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await Console.Out.WriteLineAsync($"Review with id: {domainEvent.ReviewId} was added to product with id: {domainEvent.ProductId}!");
    }
}