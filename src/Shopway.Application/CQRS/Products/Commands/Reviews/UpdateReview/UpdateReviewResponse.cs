using Shopway.Application.Abstractions;

namespace Shopway.Application.CQRS.Products.Commands.Reviews.UpdateReview;

public sealed record UpdateReviewResponse
(
    Guid Id
) : IResponse;