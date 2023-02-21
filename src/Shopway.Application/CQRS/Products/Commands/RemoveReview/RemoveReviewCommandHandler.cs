using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mapping;
using Shopway.Domain.Abstractions;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Application.Utilities;

namespace Shopway.Application.CQRS.Products.Commands.RemoveReview;

internal sealed class RemoveReviewCommandHandler : ICommandHandler<RemoveReviewCommand, RemoveReviewResponse>
{
    private readonly IProductRepository _productRepository;

    public RemoveReviewCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IResult<RemoveReviewResponse>> Handle(RemoveReviewCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);

        var reviewToRemove = product
            .Reviews
            .First(x => x.Id == command.ReviewId);

        product.RemoveReview(reviewToRemove);

        return reviewToRemove
            .ToRemoveResponse()
            .ToResult();
    }
}

