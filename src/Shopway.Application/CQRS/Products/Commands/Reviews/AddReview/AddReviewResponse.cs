using Shopway.Application.Abstractions;

namespace Shopway.Application.CQRS.Products.Commands.Reviews.AddReview;

public sealed record AddReviewResponse
(
    Guid Id
) : IResponse;