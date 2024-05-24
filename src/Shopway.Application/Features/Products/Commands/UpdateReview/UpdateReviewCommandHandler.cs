using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Products;
using Shopway.Domain.Products.ValueObjects;

namespace Shopway.Application.Features.Products.Commands.UpdateReview;

internal sealed class UpdateReviewCommandHandler(IProductRepository productRepository)
    : ICommandHandler<UpdateReviewCommand, UpdateReviewResponse>
{
    private readonly IProductRepository _productRepository = productRepository;

    //It is not preferred to make partial updates, but for tutorial purpose it is done here
    public async Task<IResult<UpdateReviewResponse>> Handle(UpdateReviewCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdWithReviewAsync(command.ProductId, command.ReviewId, cancellationToken);

        var reviewToUpdate = product
            .Reviews
            .First(x => x.Id == command.ReviewId);

        if (command.Body.Description is string description)
        {
            reviewToUpdate.UpdateDescription(Description.Create(description).Value);
        }

        if (command.Body.Stars is decimal stars)
        {
            reviewToUpdate.UpdateStars(Stars.Create(stars).Value);
        }

        return reviewToUpdate
            .ToUpdateResponse()
            .ToResult();
    }
}
