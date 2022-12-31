using Shopway.Application.Abstractions;

namespace Shopway.Application.CQRS.Products.Commands.Reviews.RemoveReview;

public sealed record RemoveReviewResponse
(
    Guid Id
) : IResponse;