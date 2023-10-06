using Shopway.Application.Abstractions;

namespace Shopway.Application.CQRS.Products.Commands.RemoveReview;

public sealed record RemoveReviewResponse
(
    Ulid Id
) : IResponse;