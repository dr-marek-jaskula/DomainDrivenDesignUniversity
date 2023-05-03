using Shopway.Application.Abstractions;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.DomainEvents;

namespace Shopway.Application.CQRS.Products.Events;

internal sealed class DisplayMessageWhenReviewAddedDomainEventHandler : IDomainEventHandler<ReviewAddedDomainEvent>
{
    private readonly IProductRepository _productRepository;

    public DisplayMessageWhenReviewAddedDomainEventHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Handle(ReviewAddedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var product = await _productRepository
            .GetByIdAsync(domainEvent.ProductId, cancellationToken);

        if (product is null)
        {
            return;
        }

        if (domainEvent.ReviewId.Value == Guid.Empty)
        {
            return;
        }

        Console.WriteLine($"Review with id: {domainEvent.Id} was added!");
    }
}