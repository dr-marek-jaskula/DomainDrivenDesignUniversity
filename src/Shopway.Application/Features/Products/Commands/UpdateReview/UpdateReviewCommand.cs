using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Products;
using static Shopway.Application.Features.Products.Commands.UpdateReview.UpdateReviewCommand;

namespace Shopway.Application.Features.Products.Commands.UpdateReview;

public sealed record UpdateReviewCommand
(
    ProductId ProductId,
    ReviewId ReviewId,
    UpdateReviewRequestBody Body
) : ICommand<UpdateReviewResponse>
{
    public sealed record UpdateReviewRequestBody(decimal? Stars, string? Description);
}
