using Shopway.Application.CQRS.Products.Queries;
using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Shopway.Domain.ValueObjects;

namespace Shopway.Tests.Unit.Abstractions;

public abstract partial class TestBase
{
    /// <summary>
    /// Create a product
    /// </summary>
    /// <param name="productId">ProductId</param>
    /// <param name="productName">ProductName. If null, random 10 character long string will be generated</param>
    /// <param name="productPrice">ProductPrice. If null, random integer in range 1 to 10 will be generated</param>
    /// <param name="uomCode">Unit of Measure Code. If null, first uom code from AllowedUomCode will be selected</param>
    /// <param name="productRevision">ProductRevision. If null, random 2 character long string will be generated</param>
    /// <returns>Product instance</returns>
    protected static Product CreateProduct
    (
        ProductId productId,
        string? productName = null,
        int? productPrice = null,
        string? uomCode = null,
        string? productRevision = null
    )
    {
        return Product.Create
        (
            productId,
            ProductName.Create(productName ?? TestString(10)).Value,
            Price.Create(productPrice ?? TestInt(1, 10)).Value,
            UomCode.Create(uomCode ?? UomCode.AllowedUomCodes.First()).Value,
            Revision.Create(productRevision ?? TestString(2)).Value
        );
    }

    /// <summary>
    /// Asserts product response.
    /// </summary>
    /// <param name="productResponse">Deserialized product response</param>
    /// <param name="product">Product to compare response with</param>
    protected static void AssertProductResponse(ProductResponse productResponse, Product product)
    {
        productResponse.Id.Should().Be(product.Id.Value);
        productResponse.ProductName.Should().Be(product.ProductName.Value);
        productResponse.Revision.Should().Be(product.Revision.Value);
        productResponse.Price.Should().Be(product.Price.Value);
        productResponse.UomCode.Should().Be(product.UomCode.Value);
    }
}