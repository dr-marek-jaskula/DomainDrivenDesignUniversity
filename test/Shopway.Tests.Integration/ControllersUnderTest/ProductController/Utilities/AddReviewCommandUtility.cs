using Shopway.Application.Features.Products.Commands.AddReview;
using Shopway.Tests.Integration.Constants;

namespace Shopway.Tests.Integration.ControllersUnderTest.ProductController.Utilities;

public static class AddReviewCommandUtility
{
    public static AddReviewCommand.AddReviewRequestBody CreateAddReviewCommand(int? stars = null, string? title = null, string? description = null)
    {
        return new AddReviewCommand.AddReviewRequestBody
        (
            stars ?? ReviewConstants.Stars,
            title ?? ReviewConstants.Title, 
            description ?? ReviewConstants.Description
        );
    }
}