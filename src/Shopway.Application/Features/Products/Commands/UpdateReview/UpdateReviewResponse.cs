using Shopway.Application.Abstractions;

namespace Shopway.Application.Features.Products.Commands.UpdateReview;

public sealed record UpdateReviewResponse
(
    Ulid Id
) : IResponse;
