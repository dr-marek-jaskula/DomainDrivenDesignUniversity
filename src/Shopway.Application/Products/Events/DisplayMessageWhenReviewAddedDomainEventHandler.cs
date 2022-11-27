using Shopway.Application.Abstractions;
using Shopway.Domain.DomainEvents;
using Shopway.Domain.Repositories;

namespace Shopway.Application.Products.Events;

internal sealed class DisplayMessageWhenReviewAddedDomainEventHandler : IDomainEventHandler<ReviewAddedDomainEvent>
{
    private readonly IProductRepository _productRepository;

    public DisplayMessageWhenReviewAddedDomainEventHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Handle(ReviewAddedDomainEvent notification, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(notification.ProductId, cancellationToken);

        if (product is null)
        {
            return;
        }

        if (notification.ReviewId.Value == Guid.Empty)
        {
            return;
        }

        Console.WriteLine($"Review with id: {notification.Id} was added!");
    }
}