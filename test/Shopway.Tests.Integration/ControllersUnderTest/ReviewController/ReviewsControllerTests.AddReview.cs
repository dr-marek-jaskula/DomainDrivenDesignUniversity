using RestSharp;
using Shopway.Domain.Entities;
using Shopway.Application.CQRS.Products.Commands.AddReview;
using static System.Net.HttpStatusCode;
using static Shopway.Presentation.Controllers.ProductsController;

namespace Shopway.Tests.Integration.ControllersUnderTest.ReviewController;

public partial class ReviewsControllerTests
{
    [Fact]
    public async Task AddReview_ShouldAddReview_WhenValidData()
    {
        //Arrange
        var generatedProductId = _fixture.DataGenerator.AddProductWithoutReviews();

        var body = new AddReviewCommand.AddReviewRequestBody(2, "CustomTitle", "Description");

        var request = PostRequest($"{generatedProductId.Result.Value}/{Reviews}", body);

        //Act
        var response = await _restClient!.PostAsync(request);

        //Assert
        response.StatusCode.Should().Be(OK);

        var addedReview = _fixture
            .Context
            .Set<Review>()
            .Where(r => r.Stars.Value == body.Stars)
            .Where(r => r.Title.Value == body.Title)
            .Where(r => r.Description.Value == body.Description)
            .FirstOrDefault();

        addedReview.Should().NotBeNull();
    }
}