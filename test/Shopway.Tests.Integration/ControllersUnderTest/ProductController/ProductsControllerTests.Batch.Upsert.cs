using RestSharp;
using Shopway.Domain.EntityKeys;
using Shopway.Application.CQRS;
using Shopway.Tests.Integration.Utilities;
using static System.Net.HttpStatusCode;
using static Shopway.Domain.Errors.Domain.DomainErrors.ProductNameError;
using static Shopway.Tests.Integration.ControllersUnderTest.ProductController.Utilities.ProductBatchUpsertCommandUtility;

namespace Shopway.Tests.Integration.ControllersUnderTest.ProductController;

public partial class ProductsControllerTests
{
    [Fact]
    public async Task Batch_Upsert_ShouldReturnValidResponseEntries_WhenRequestsAreValid()
    {
        //Arrange
        var batchCommand = CreateProductBatchUpsertCommand();
        var request = PostRequest($"batch/upsert", batchCommand);

        //Act
        var response = await _restClient!.PostAsync(request);

        //Assert
        response.StatusCode.Should().Be(OK);

        var batchResponse = response.Deserialize<ProductBatchResponseResult>();

        batchResponse!
            .Entries
            .Where(x => x.Status is BatchEntryStatus.Inserted)
            .Should()
            .HaveCount(3);
    }

    [Fact]
    public async Task Batch_Upsert_ShouldReturnOneErrorResponseEntry_WhenOneRequestIsInvalid()
    {
        //Arrange
        var batchCommand = CreateProductBatchUpsertCommand
        (
            CreateProductBatchUpsertRequest(ProductKey.Create("firstTest+=.,ProductfirstTestProductfirstTestProductfirstTestProductfirstTestProduct", "1,0"), 100m, "pcs"),
            CreateProductBatchUpsertRequest()
        );

        var request = PostRequest($"batch/upsert", batchCommand);

        //Act
        var response = await _restClient!.ExecutePostAsync(request);

        //Assert
        response.StatusCode.Should().Be(BadRequest);

        var batchResponse = response.Deserialize<ProductBatchResponseResult>();
        
        batchResponse!
            .Entries
            .Should()
            .HaveCount(2);

        var errorEntry = batchResponse!
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
        var batchCommand = CreateProductBatchUpsertCommandWithSingleRequest(new ProductKey(), 100m, "pcs2");

        var request = PostRequest($"batch/upsert", batchCommand);

        //Act
        var response = await _restClient!.ExecutePostAsync(request);

        //Assert
        response.StatusCode.Should().Be(BadRequest);

        var problemDetails = response.Deserialize<ModelProblemDetails>();
        problemDetails!.ShouldHaveErrorCount(2);
    }
}