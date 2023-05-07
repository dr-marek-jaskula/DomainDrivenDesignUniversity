using Shopway.Application.CQRS.Products.Queries.GetProductById;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.EntityIds;
using Shopway.Tests.Unit.Abstractions;
using static System.Threading.CancellationToken;

namespace Shopway.Tests.Unit.LayerTests.Application.ProductHandlers;

public sealed class GetProductByIdQueryHandlerTests : TestBase
{
    /// <summary>
    /// System under tests
    /// </summary>
    private readonly GetProductByIdQueryHandler _sut;
    private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();

    public GetProductByIdQueryHandlerTests()
    {
        _sut = new(_productRepository);
    }

    [Fact]
	public async Task GetById_ShouldSucceed_WhenCreateValidProduct()
	{
        //Arrange
        var productId = ProductId.New();
        var expected = CreateProduct(productId);

        _productRepository
            .GetByIdAsync(productId, None)
            .Returns(expected);

        var query = new GetProductByIdQuery(productId);

        //Act
        var result = await _sut
            .Handle(query, None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();

        var actual = result.Value;

        AssertProductResponse(actual, expected);
    }
}