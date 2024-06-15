using Shopway.Application.Abstractions;

namespace Shopway.Application.Features.Products.Commands.RemoveReview;

public sealed record RemoveReviewResponse
(
    Ulid Id
) : IResponse;
