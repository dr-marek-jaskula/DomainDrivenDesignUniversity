using Shopway.Domain.Entities;
using Shopway.Application.Features.Products.Queries;
using Shopway.Application.Features.Products.Commands.AddReview;
using Shopway.Application.Features.Products.Commands.RemoveReview;
using Shopway.Application.Features.Products.Commands.UpdateReview;

namespace Shopway.Application.Mappings;

public static class ReviewMapping
{
    public static ReviewResponse ToResponse(this Review review)
    {
        return new ReviewResponse
        (
            review.Id.Value,
            review.Username.Value,
            review.Stars.Value,
            review.Title.Value,
            review.Description.Value
        );
    }

    public static IReadOnlyCollection<ReviewResponse> ToResponses(this IReadOnlyCollection<Review> reviews)
    {
        return reviews
            .Select(ToResponse)
            .ToList()
            .AsReadOnly();
    }

    public static UpdateReviewResponse ToUpdateResponse(this Review reviewToUpdate)
    {
        return new UpdateReviewResponse(reviewToUpdate.Id.Value);
    }

    public static RemoveReviewResponse ToRemoveResponse(this Review reviewToRemove)
    {
        return new RemoveReviewResponse(reviewToRemove.Id.Value);
    }

    public static AddReviewResponse ToAddResponse(this Review reviewToAdd)
    {
        return new AddReviewResponse(reviewToAdd.Id.Value);
    }
}