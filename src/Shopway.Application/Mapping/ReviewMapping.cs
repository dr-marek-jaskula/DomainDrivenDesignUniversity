using Shopway.Application.CQRS.Products.Commands.CreateProduct;
using Shopway.Application.CQRS.Products.Commands.RemoveProduct;
using Shopway.Application.CQRS.Products.Commands.Reviews.AddReview;
using Shopway.Application.CQRS.Products.Commands.Reviews.RemoveReview;
using Shopway.Application.CQRS.Products.Commands.Reviews.UpdateReview;
using Shopway.Application.CQRS.Products.Commands.UpdateProduct;
using Shopway.Application.CQRS.Products.Queries.GetProductById;
using Shopway.Domain.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Shopway.Application.Mapping;

public static class ReviewMapping
{
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