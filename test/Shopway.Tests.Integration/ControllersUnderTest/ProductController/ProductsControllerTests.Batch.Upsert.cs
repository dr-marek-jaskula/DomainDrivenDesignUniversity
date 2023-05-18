using RestSharp;
using Shopway.Domain.EntityKeys;
using Shopway.Application.CQRS;
using Shopway.Tests.Integration.Utilities;
using Shopway.Application.CQRS.Products.Commands.BatchUpsertProduct;
using static System.Net.HttpStatusCode;
using static Shopway.Domain.Utilities.ListUtilities;
using static Shopway.Domain.Errors.Domain.DomainErrors.ProductNameError;
using static Shopway.Application.CQRS.Products.Commands.BatchUpsertProduct.BatchUpsertProductCommand;

namespace Shopway.Tests.Integration.ControllersUnderTest.ProductController;

public partial class ProductsControllerTests
{
    [Fact]
    public async Task Batch_Upsert_ShouldReturnValidResponseEntries_WhenRequestsAreValid()
    {
        //Arrange
        var batchRequests = AsList
        (
            new ProductBatchUpsertRequest(ProductKey.Create("firstTestProduct", "1,0"), 100m, "pcs"),
            new ProductBatchUpsertRequest(ProductKey.Create("secondTestProduct", "2,0"), 50m, "kg"),
            new ProductBatchUpsertRequest(ProductKey.Create("thirdTestProduct", "3,0"), 10m, "pcs")
        );

        var batchCommand = new BatchUpsertProductCommand(batchRequests);

        var request = PostRequest($"batch/upsert", batchCommand);

        //Act
        var response = await _restClient!.PostAsync(request);

        //Assert
        response.StatusCode.Should().Be(OK);

        var deserializedResponse = response.Deserialize<ProductBatchResponseResult>();

        deserializedResponse!
            .Entries
            .Where(x => x.Status is BatchEntryStatus.Inserted)
            .Should()
            .HaveCount(3);
    }

    [Fact]
    public async Task Batch_Upsert_ShouldReturnOneErrorResponseEntry_WhenOneRequestIsInvalid()
    {
        //Arrange
        var batchRequests = AsList
        (
            new ProductBatchUpsertRequest(ProductKey.Create("firstTest+=.,ProductfirstTestProductfirstTestProductfirstTestProductfirstTestProduct", "1,0"), 100m, "pcs"),
            new ProductBatchUpsertRequest(ProductKey.Create("secondTestProduct", "2,0"), 50m, "kg"),
            new ProductBatchUpsertRequest(ProductKey.Create("thirdTestProduct", "3,0"), 10m, "pcs")
        );

        var batchCommand = new BatchUpsertProductCommand(batchRequests);

        var request = PostRequest($"batch/upsert", batchCommand);

        //Act
        var response = await _restClient!.ExecutePostAsync(request);

        //Assert
        response.StatusCode.Should().Be(BadRequest);

        var deserializedResponse = response.Deserialize<ProductBatchResponseResult>();
        
        deserializedResponse!
            .Entries
            .Should()
            .HaveCount(3);

        var errorEntry = deserializedResponse!
            .Entries
            .Where(x => x.Status is BatchEntryStatus.Error)
            .First();

        errorEntry
            .Errors
            .Should()
            .Contain(TooLong);

        errorEntry
            .Errors
            .Should()
            .Contain(ContainsIllegalCharacter);
    }

    [Fact]
    public async Task Batch_Upsert_ShouldReturnProblemDetailsWithTwoErrors_WhenProductNameAndRevisionAreNull()
    {
        //Arrange
        var batchRequests = AsList
        (
            new ProductBatchUpsertRequest(new ProductKey(), 100m, "pcs2")
        );

        var batchCommand = new BatchUpsertProductCommand(batchRequests);

        var request = PostRequest($"batch/upsert", batchCommand);

        //Act
        var response = await _restClient!.ExecutePostAsync(request);

        //Assert
        response.StatusCode.Should().Be(BadRequest);

        var deserializedResponse = response.Deserialize<ModelProblemDetails>();
        deserializedResponse.Should().NotBeNull();
        deserializedResponse!.Errors.Should().HaveCount(2);
    }
}