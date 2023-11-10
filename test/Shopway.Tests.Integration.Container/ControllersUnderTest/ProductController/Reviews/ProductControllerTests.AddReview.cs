using RestSharp;
using Shopway.Application.Features.Products.Commands.AddReview;
using Shopway.Tests.Integration.Utilities;
using static System.Net.HttpStatusCode;
using static Shopway.Tests.Integration.Container.ControllersUnderTest.ProductController.Utilities.AddReviewCommandUtility;

namespace Shopway.Tests.Integration.ControllersUnderTest.ProductController;

public partial class ProductsControllerTests
{
    [Fact]
    public async Task AddReview_ShouldAddReview_WhenValidData()
    {
        //Arrange
        var product = await fixture.DataGenerator.AddProductAsync();
        var body = CreateAddReviewCommand();

        var request = PostRequest($"{product.Id}/{Presentation.Controllers.ProductsController.Reviews}", body);

        //Act
        var response = await _restClient!.PostAsync(request);

        //Assert
        response.StatusCode.Should().Be(OK);

        var actual = response.Deserialize<AddReviewResponse>();
        actual.Id.Should().NotBe(Ulid.Empty);
    }
}