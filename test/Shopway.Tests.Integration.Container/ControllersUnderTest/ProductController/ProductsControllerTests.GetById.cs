﻿using RestSharp;
using Shopway.Application.Features.Products.Queries;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Products;
using Shopway.Tests.Integration.Container.ControllersUnderTest.ProductController.Utilities;
using Shopway.Tests.Integration.Container.Utilities;
using Shopway.Tests.Integration.Utilities;
using static System.Net.HttpStatusCode;

namespace Shopway.Tests.Integration.ControllersUnderTest.ProductController;

public partial class ProductsControllerTests
{
    [Fact]
    public async Task GetById_ShouldReturnProduct_WhenProductExists()
    {
        //Arrange
        var expected = await fixture.DataGenerator.AddProductAsync();

        var request = GetRequest(expected.Id.Value.ToString());

        //Act
        var response = await _restClient!.GetAsync(request);

        //Assert
        response.StatusCode.Should().Be(OK);

        var actual = response.Deserialize<ProductResponse>();
        actual.ShouldMatch(expected);
    }

    [Fact]
    public async Task GetById_ShouldReturnErrorResponse_WhenProductNotExists()
    {
        //Arrange
        var invalidProductId = ProductId.New();

        var request = GetRequest(invalidProductId.Value.ToString());

        //Act
        var response = await _restClient!.ExecuteGetAsync(request);

        //Assert
        response.StatusCode.Should().Be(BadRequest);

        var problemDetails = response.Deserialize<ValidationProblemDetails>();
        problemDetails!.ShouldConsistOf(Error.InvalidReference(invalidProductId.Value, nameof(Product)));
    }
}
