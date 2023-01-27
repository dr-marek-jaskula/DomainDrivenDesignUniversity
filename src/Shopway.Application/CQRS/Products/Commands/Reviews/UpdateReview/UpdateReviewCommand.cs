using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.EntityIds;
using static Shopway.Application.CQRS.Products.Commands.Reviews.UpdateReview.UpdateReviewCommand;

namespace Shopway.Application.CQRS.Products.Commands.Reviews.UpdateReview;

public sealed record UpdateReviewCommand
(
    ProductId ProductId,
    ReviewId ReviewId,
    UpdateReviewRequestBody Body
) : ICommand<UpdateReviewResponse>
{
    public sealed record UpdateReviewRequestBody(decimal? Stars, string? Description);
}
