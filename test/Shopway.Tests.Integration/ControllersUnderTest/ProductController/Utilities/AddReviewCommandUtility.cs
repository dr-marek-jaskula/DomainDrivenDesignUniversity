using Shopway.Application.Features.Products.Commands.AddReview;
using static Shopway.Tests.Integration.Constants.Constants.Review;

namespace Shopway.Tests.Integration.ControllersUnderTest.ProductController.Utilities;

public static class AddReviewCommandUtility
{
    public static AddReviewCommand.AddReviewRequestBody CreateAddReviewCommand(int? stars = null, string? title = null, string? description = null)
    {
        return new AddReviewCommand.AddReviewRequestBody
        (
            stars ?? Stars,
            title ?? Title,
            description ?? Description
        );
    }
}
