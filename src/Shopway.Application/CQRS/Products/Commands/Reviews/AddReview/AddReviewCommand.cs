using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.EntityIds;
using static Shopway.Application.CQRS.Products.Commands.Reviews.AddReview.AddReviewCommand;

namespace Shopway.Application.CQRS.Products.Commands.Reviews.AddReview;

public sealed record AddReviewCommand
(
    ProductId ProductId,
    AddReviewRequestBody Body
) : ICommand<AddReviewResponse>
{
    public sealed record AddReviewRequestBody(decimal Stars, string Title, string Description);
}
