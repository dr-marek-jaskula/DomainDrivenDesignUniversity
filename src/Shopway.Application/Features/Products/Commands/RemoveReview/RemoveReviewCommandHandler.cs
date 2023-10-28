﻿using Shopway.Domain.Abstractions;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Abstractions.Repositories;

namespace Shopway.Application.Features.Products.Commands.RemoveReview;

internal sealed class RemoveReviewCommandHandler : ICommandHandler<RemoveReviewCommand, RemoveReviewResponse>
{
    private readonly IProductRepository _productRepository;

    public RemoveReviewCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

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
