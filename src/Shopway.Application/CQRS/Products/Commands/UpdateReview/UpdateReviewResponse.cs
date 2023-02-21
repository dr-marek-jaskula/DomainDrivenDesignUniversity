using Shopway.Application.Abstractions;

namespace Shopway.Application.CQRS.Products.Commands.UpdateReview;

public sealed record UpdateReviewResponse
(
    Guid Id
) : IResponse;