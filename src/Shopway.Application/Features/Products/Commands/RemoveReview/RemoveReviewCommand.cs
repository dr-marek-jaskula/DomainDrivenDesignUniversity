using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Products;

namespace Shopway.Application.Features.Products.Commands.RemoveReview;

public sealed record RemoveReviewCommand
(
    ProductId ProductId,
    ReviewId ReviewId
) : ICommand<RemoveReviewResponse>;