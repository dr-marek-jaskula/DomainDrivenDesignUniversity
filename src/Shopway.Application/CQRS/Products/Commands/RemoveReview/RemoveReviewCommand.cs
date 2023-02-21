using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.EntityIds;

namespace Shopway.Application.CQRS.Products.Commands.RemoveReview;

public sealed record RemoveReviewCommand
(
    ProductId ProductId,
    ReviewId ReviewId
) : ICommand<RemoveReviewResponse>;