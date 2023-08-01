using RestSharp;
using Shopway.Domain.EntityKeys;
using Shopway.Tests.Integration.Container.Utilities;
using Shopway.Tests.Integration.Utilities;
using static System.Net.HttpStatusCode;
using static Shopway.Domain.Errors.HttpErrors;
using static Shopway.Tests.Integration.Container.ControllersUnderTest.ProductController.Utilities.CreateProductCommandUtility;

namespace Shopway.Tests.Integration.ControllersUnderTest.ProductController;

public partial class ProductsControllerTests
{
    [Fact]
    public async Task Create_ShouldReturnFailure_WhenProductAlreadyExists()
    {
        //Arrange
        var product = await fixture.DataGenerator.AddProductAsync();
        var productKey = ProductKey.From(product);

        var body = CreateProductCommand(productKey);
        var request = PostRequest(string.Empty, body);

        //Act
        var response = await _restClient!.ExecutePostAsync(request);

        //Assert
        response.StatusCode.Should().Be(BadRequest);

        var problemDetails = response.Deserialize<ValidationProblemDetails>();
        problemDetails!.ShouldConsistOf(AlreadyExists(productKey));
    }
}