using Shopway.Tests.Unit.Constants;
using Shopway.Tests.Unit.Abstractions;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Application.Features.Products.Queries.GetProductById;
using Shopway.Tests.Unit.LayerTests.Application.ProductHandlers.Utilities;
using static System.Threading.CancellationToken;

namespace Shopway.Tests.Unit.LayerTests.Application.ProductHandlers;

[Trait(nameof(UnitTest), UnitTest.Application)]
public sealed class GetProductByIdQueryHandlerTests : TestBase
{
    /// <summary>
    /// System under tests
    /// </summary>
    private readonly GetProductByIdQueryHandler _sut;
    private readonly IProductRepository _productRepositoryMock = Substitute.For<IProductRepository>();

    public GetProductByIdQueryHandlerTests()
    {
        _sut = new(_productRepositoryMock);
    }

    [Fact]
    public async Task GetById_ShouldReturnProduct_WhenProductExists()
	{
        //Arrange
        var expected = CreateProduct();

        _productRepositoryMock
            .GetByIdAsync(expected.Id, None)
            .Returns(expected);

        var query = new GetProductByIdQuery(expected.Id);

        //Act
        var result = await _sut.Handle(query, None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();

        var actual = result.Value;
        actual.ShouldMatch(expected);

        await _productRepositoryMock.Received(1).GetByIdAsync(expected.Id, None);
    }
}