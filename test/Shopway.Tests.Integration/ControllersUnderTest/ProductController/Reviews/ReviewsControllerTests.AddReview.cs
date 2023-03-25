using RestSharp;
using Shopway.Domain.Entities;
using Shopway.Application.CQRS.Products.Commands.AddReview;
using static System.Net.HttpStatusCode;

namespace Shopway.Tests.Integration.ControllersUnderTest.Reviews;

public partial class ReviewsControllerTests
{
    [Fact]
    public async Task AddReview_ShouldAddReview_WhenValidData()
    {
        //Arrange
        var generatedProduct = await _fixture.DataGenerator.AddProduct();

        var reviewTitle = "CustomTitle";
        var reviewDescription = "Description";
        var body = new AddReviewCommand.AddReviewRequestBody(2, reviewTitle, reviewDescription);

        var request = PostRequest($"{generatedProduct.Id}/{Presentation.Controllers.ProductsController.Reviews}", body);

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
        addedReview!.Title.Value.Should().Be(reviewTitle);
        addedReview!.Description.Value.Should().Be(reviewDescription);
    }
}