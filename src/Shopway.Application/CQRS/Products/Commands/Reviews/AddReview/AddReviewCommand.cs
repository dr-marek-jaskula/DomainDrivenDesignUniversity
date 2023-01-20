using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.StronglyTypedIds;

namespace Shopway.Application.CQRS.Products.Commands.Reviews.AddReview;

public sealed record AddReviewCommand
(
    ProductId ProductId,
    decimal Stars,
    string Title,
    string Description
) : ICommand<AddReviewResponse>;
