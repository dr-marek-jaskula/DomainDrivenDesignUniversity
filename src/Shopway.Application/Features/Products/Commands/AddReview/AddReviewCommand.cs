using Shopway.Domain.Products;
using Shopway.Application.Abstractions.CQRS;
using static Shopway.Application.Features.Products.Commands.AddReview.AddReviewCommand;

namespace Shopway.Application.Features.Products.Commands.AddReview;

public sealed record AddReviewCommand
(
    ProductId ProductId,
    AddReviewRequestBody Body
) : ICommand<AddReviewResponse>
{
    public sealed record AddReviewRequestBody(decimal Stars, string Title, string Description);
}
