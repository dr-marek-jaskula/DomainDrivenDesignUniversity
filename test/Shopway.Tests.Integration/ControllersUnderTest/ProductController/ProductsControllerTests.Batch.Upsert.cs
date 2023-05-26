using RestSharp;
using Shopway.Domain.EntityKeys;
using Shopway.Tests.Integration.Utilities;
using Shopway.Tests.Integration.ControllersUnderTest.ProductController.Utilities;
using static System.Net.HttpStatusCode;
using static Shopway.Application.CQRS.BatchEntryStatus;
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
        batchResponse!.ShouldHaveCount(batchCommand.Requests.Count, Inserted);
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
        batchResponse!.ShouldHaveCount(batchCommand.Requests.Count);
        batchResponse!.ShouldContainEntryWithErrors(TooLong, ContainsIllegalCharacter);
    }

    [Fact]
    public async Task Batch_Upsert_ShouldReturnProblemDetailsWithTwoErrors_WhenProductNameAndRevisionFromProductKeyAreNull()
    {
        //Arrange
        var batchCommand = CreateProductBatchUpsertCommandWithSingleRequest(new ProductKey(), 100m, "pcs");

        var request = PostRequest($"batch/upsert", batchCommand);

        //Act
        var response = await _restClient!.ExecutePostAsync(request);

        //Assert
        response.StatusCode.Should().Be(BadRequest);

        var problemDetails = response.Deserialize<ModelProblemDetails>();
        problemDetails!.ShouldHaveErrorCount(2);
    }
}