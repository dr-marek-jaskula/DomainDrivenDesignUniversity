using Microsoft.EntityFrameworkCore;
using RestSharp;
using Shopway.Application.CQRS.Products.Commands.CreateProduct;
using Shopway.Application.CQRS.Products.Queries;
using Shopway.Domain.Entities;
using Shopway.Domain.EntityBusinessKeys;
using Shopway.Tests.Integration.Utilities;
using static System.Net.HttpStatusCode;

namespace Shopway.Tests.Integration.ControllersUnderTest.ProductController;

public partial class ProductsControllerTests
{
    [Fact]
    public async Task Create_ShouldReturnFailure_WhenProductExists()
    {
        //Arrange
        var productName = "ExistingNamE";
        var revision = "RevisioN";
        var key = ProductKey.Create(productName, revision);

        var body = new CreateProductCommand(key, 10, "kg");
        var request = PostRequest("/api/products", body);
        request.AddApiKeyAuthentication(apiKeys.PRODUCT_CREATE);

        //Act
        var response = await _restClient!.ExecutePostAsync(request);

        //Assert
        var deserialized = response.Deserialize<CreateProductResponse>();
        deserialized!.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Create_ShouldReturnFailure_WhenProductExists2()
    {
        //Arrange
        var generatedProductId = await fixture.DataGenerator.AddProductWithoutReviews();

        var get = await fixture.Context.Set<Product>().ToListAsync();

        var request = GetRequest(generatedProductId.Value.ToString());
        request.AddApiKeyAuthentication(apiKeys.PRODUCT_GET);

        //Act
        var response = await _restClient!.GetAsync(request);

        //Assert
        response.StatusCode.Should().Be(OK);

        var deserializedResponse = response.DeserializeResponseResult<ProductResponse>();
        deserializedResponse.Id.Should().Be(generatedProductId.Value);
    }
}