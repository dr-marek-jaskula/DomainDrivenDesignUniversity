using RestSharp;
using Shopway.Domain.EntityKeys;
using Shopway.Domain.Errors;
using Shopway.Tests.Integration.Container.Utilities;
using Shopway.Tests.Integration.Utilities;
using static Shopway.Tests.Integration.Container.ControllersUnderTest.ProductController.Utilities.CreateProductCommandUtility;
using static System.Net.HttpStatusCode;

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
        problemDetails!.ShouldConsistOf(Error.AlreadyExists(productKey));
    }
}