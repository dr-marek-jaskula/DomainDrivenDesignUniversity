using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Products;

namespace Shopway.Application.Features.Products.Commands.RemoveReview;

internal sealed class RemoveReviewCommandHandler(IProductRepository productRepository)
    : ICommandHandler<RemoveReviewCommand, RemoveReviewResponse>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<IResult<RemoveReviewResponse>> Handle(RemoveReviewCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdWithReviewAsync(command.ProductId, command.ReviewId, cancellationToken);

        var reviewToRemove = product
            .Reviews
            .First(x => x.Id == command.ReviewId);

        product.RemoveReview(reviewToRemove);

        return reviewToRemove
            .ToRemoveResponse()
            .ToResult();
    }
}
