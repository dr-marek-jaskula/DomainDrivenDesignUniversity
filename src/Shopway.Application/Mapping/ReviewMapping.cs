using Shopway.Application.CQRS.Products.Commands.AddReview;
using Shopway.Application.CQRS.Products.Commands.RemoveReview;
using Shopway.Application.CQRS.Products.Commands.UpdateReview;
using Shopway.Application.CQRS.Products.Queries;
using Shopway.Domain.Entities;

namespace Shopway.Application.Mapping;

public static class ReviewMapping
{
    public static ReviewResponse ToResponse(this Review review)
    {
        return new ReviewResponse
        (
            Id: review.Id.Value,
            Username: review.Username.Value,
            Stars: review.Stars.Value,
            Title: review.Title.Value,
            Description: review.Description.Value
        );
    }

    public static IReadOnlyCollection<ReviewResponse> ToResponses(this IReadOnlyCollection<Review> reviews)
    {
        return reviews
            .Select(review => review.ToResponse())
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