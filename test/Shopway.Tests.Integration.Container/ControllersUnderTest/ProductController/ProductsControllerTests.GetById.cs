using RestSharp;
using Shopway.Application.CQRS.Products.Queries;
using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Shopway.Tests.Integration.Utilities;
using static System.Net.HttpStatusCode;
using static Shopway.Domain.Errors.HttpErrors;

namespace Shopway.Tests.Integration.ControllersUnderTest.ProductController;

public partial class ProductsControllerTests
{
    [Fact]
    public async Task GetById_ShouldReturnProduct_WhenProductExists()
    {
        //Arrange
        var generatedProduct = await fixture.DataGenerator.AddProduct();

        var request = GetRequest(generatedProduct.Id.Value.ToString());
        request.AddApiKeyAuthentication(apiKeys.PRODUCT_GET);

        //Act
        var response = await _restClient!.GetAsync(request);

        //Assert
        response.StatusCode.Should().Be(OK);

        var deserializedResponse = response.DeserializeResponseResult<ProductResponse>();
        AssertProductResponse(deserializedResponse, generatedProduct, assertReviews: false);
    }

    [Fact]
    public async Task GetById_ShouldReturnErrorResponse_WhenProductNotExists()
    {
        //Arrange
        var invalidProductId = ProductId.New();

        var request = GetRequest(invalidProductId.Value.ToString());
        request.AddApiKeyAuthentication(apiKeys.PRODUCT_GET);

        //Act
        var response = await _restClient!.ExecuteGetAsync(request);

        //Assert
        response.StatusCode.Should().Be(BadRequest);

        var deserializedResponse = response.Deserialize<ValidationProblemDetails>();
        AssertProblemDetails(deserializedResponse!);
        deserializedResponse!.Errors.Should().Contain(InvalidReference(invalidProductId.Value, nameof(Product)));
    }
}