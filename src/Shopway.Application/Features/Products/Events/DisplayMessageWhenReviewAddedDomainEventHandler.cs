using Shopway.Application.Abstractions;
using Shopway.Domain.Products.Events;

namespace Shopway.Application.Features.Products.Events;

internal sealed class DisplayMessageWhenReviewAddedDomainEventHandler : IDomainEventHandler<ReviewAddedDomainEvent>
{
    public async Task Handle(ReviewAddedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await Console.Out.WriteLineAsync($"Review with id: {domainEvent.ReviewId} was added to product with id: {domainEvent.ProductId}!");
    }
}