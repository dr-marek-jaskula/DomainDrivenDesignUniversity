using Shopway.Application.Abstractions;

namespace Shopway.Application.CQRS.Products.Commands.AddReview;

public sealed record AddReviewResponse
(
    Ulid Id
) : IResponse;