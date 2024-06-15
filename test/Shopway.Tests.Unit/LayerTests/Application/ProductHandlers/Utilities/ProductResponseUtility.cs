using Shopway.Application.Features.Products.Queries;
using Shopway.Domain.Products;

namespace Shopway.Tests.Unit.LayerTests.Application.ProductHandlers.Utilities;

public static class ProductResponseUtility
{
    /// <summary>
    /// Asserts product response.
    /// </summary>
    /// <param name="productResponse">Deserialized product response</param>
    /// <param name="product">Product to compare response with</param>
    public static void ShouldMatch(this ProductResponse productResponse, Product product)
    {
        productResponse.Id.Should().Be(product.Id.Value);
        productResponse.ProductName.Should().Be(product.ProductName.Value);
        productResponse.Revision.Should().Be(product.Revision.Value);
        productResponse.Price.Should().Be(product.Price.Value);
        productResponse.UomCode.Should().Be(product.UomCode.Value);
    }
}
