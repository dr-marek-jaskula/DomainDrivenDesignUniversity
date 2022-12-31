using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.StronglyTypedIds;

namespace Shopway.Application.CQRS.Products.Commands.Reviews.RemoveReview;

public sealed record RemoveReviewCommand
(
    ProductId ProductId,
    ReviewId ReviewId
) : ICommand<RemoveReviewResponse>;