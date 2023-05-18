using RestSharp;
using Shopway.Application.CQRS.Products.Commands.CreateProduct;
using Shopway.Domain.EntityKeys;
using Shopway.Domain.ValueObjects;
using Shopway.Tests.Integration.Utilities;
using static System.Net.HttpStatusCode;
using static Shopway.Domain.Errors.HttpErrors;
using static Shopway.Tests.Integration.Abstractions.TestDataGeneratorBase;

namespace Shopway.Tests.Integration.ControllersUnderTest.ProductController;

public partial class ProductsControllerTests
{
    [Fact]
    public async Task Create_ShouldReturnFailure_WhenProductExists()
    {
        //Arrange
        var productName = TestString(20);
        var revision = TestString(2);
        var key = ProductKey.Create(productName, revision);

        await _fixture.DataGenerator.AddProduct(ProductName.Create(productName).Value, Revision.Create(revision).Value);

        var body = new CreateProductCommand(key, 10, "kg");
        var request = PostRequest(string.Empty, body);
        request.AddApiKeyAuthentication(apiKeys.PRODUCT_CREATE);

        //Act
        var response = await _restClient!.ExecutePostAsync(request);

        //Assert
        response.StatusCode.Should().Be(BadRequest);

        var deserializedResponse = response.Deserialize<ValidationProblemDetails>();
        deserializedResponse!.Errors.Should().Contain(error => error == AlreadyExists(key));
    }
}