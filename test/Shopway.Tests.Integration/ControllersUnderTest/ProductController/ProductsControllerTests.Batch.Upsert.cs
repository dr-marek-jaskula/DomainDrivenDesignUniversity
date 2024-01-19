using RestSharp;
using Shopway.Domain.EntityKeys;
using Shopway.Tests.Integration.ControllersUnderTest.ProductController.Utilities;
using Shopway.Tests.Integration.Utilities;
using static Shopway.Application.Features.BatchEntryStatus;
using static Shopway.Domain.Products.Errors.DomainErrors.ProductNameError;
using static Shopway.Tests.Integration.Constants.Constants;
using static Shopway.Tests.Integration.ControllersUnderTest.ProductController.Utilities.ProductBatchUpsertCommandUtility;
using static System.Net.HttpStatusCode;

namespace Shopway.Tests.Integration.ControllersUnderTest.ProductController;

public partial class ProductsControllerTests
{
    [Fact]
    [Trait(nameof(IntegrationTest), IntegrationTest.PublicApi)]
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
    [Trait(nameof(IntegrationTest), IntegrationTest.PublicApi)]
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
    [Trait(nameof(IntegrationTest), IntegrationTest.PublicApi)]
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