using Shopway.Application.Abstractions;

namespace Shopway.Application.Features.Products.Commands.AddReview;

public sealed record AddReviewResponse
(
    Ulid Id
) : IResponse;