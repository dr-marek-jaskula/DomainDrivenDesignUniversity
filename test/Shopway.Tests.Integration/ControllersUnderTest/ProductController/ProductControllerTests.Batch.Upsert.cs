using RestSharp;
using Shopway.Application.Batch;
using Shopway.Application.Batch.Products;
using Shopway.Tests.Integration.Utilities;
using Shopway.Domain.ValueObjects;
using static System.Net.HttpStatusCode;
using static Shopway.Domain.Utilities.ListUtilities;
using static Shopway.Application.Batch.Products.ProductBatchUpsertCommand;

namespace Shopway.Tests.Integration.ControllersUnderTest.ProductController;

public partial class ProductControllerTests
{
    [Fact]
    public async Task Batch_Upsert_ShouldReturnValidResponseEntries_WhenRequestsAreValid()
    {
        //Arrange
        var batchRequests = AsList
        (
            new ProductBatchUpsertRequest("firstTestProduct", 100m, "pcs", "1.0"),
            new ProductBatchUpsertRequest("secondTestProduct", 50m, "kg", "2.0"),
            new ProductBatchUpsertRequest("thirdTestProduct", 10m, "pcs", "3.0")
        );

        var batchCommand = new ProductBatchUpsertCommand(batchRequests);

        var request = PostRequest($"batch/upsert", batchCommand);

        //Act
        var response = await _restClient!.PostAsync(request);

        //Assert
        response.StatusCode.Should().Be(OK);

        var deserializedResponse = response.Deserialize<ProductBatchResponseResult>();
        
        deserializedResponse!
            .Entries
            .Should()
            .HaveCount(3);

        deserializedResponse
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
            new ProductBatchUpsertRequest("firstTest+=.,ProductfirstTestProductfirstTestProductfirstTestProductfirstTestProduct", 100m, "pcs", "1.0"),
            new ProductBatchUpsertRequest("secondTestProduct", 50m, "kg", "2.0"),
            new ProductBatchUpsertRequest("thirdTestProduct", 10m, "pcs", "3.0")
        );

        var batchCommand = new ProductBatchUpsertCommand(batchRequests);

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
            .Contain($"ProductName must be at most {ProductName.MaxLength} characters long");

        errorEntry
            .Errors
            .Should()
            .Contain("ProductName contains illegal character");
    }
}