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
        ProductId? productId = null,
        string? productName = null,
        int? productPrice = null,
        string? uomCode = null,
        string? productRevision = null
    )
    {
        return Product.Create
        (
            productId ?? ProductId.New(),
            ProductName.Create(productName ?? TestString(10)).Value,
            Price.Create(productPrice ?? TestInt(1, 10)).Value,
            UomCode.Create(uomCode ?? UomCode.AllowedUomCodes.First()).Value,
            Revision.Create(productRevision ?? TestString(2)).Value
        );
    }

    /// <summary>
    /// Create a review
    /// </summary>
    /// <param name="productId">ReviewId</param>
    /// <param name="title">ProductName. If null, random 10 character long string will be generated</param>
    /// <param name="description">ProductPrice. If null, random integer in range 1 to 10 will be generated</param>
    /// <param name="username">Unit of Measure Code. If null, first uom code from AllowedUomCode will be selected</param>
    /// <param name="stars">ProductRevision. If null, random 2 character long string will be generated</param>
    /// <returns>Product instance</returns>
    protected static Review CreateReview
    (
        ReviewId? reviewId = null,
        string? title = null,
        string? description = null,
        string? username = null,
        decimal? stars = null
    )
    {
        return Review.Create
        (
            reviewId ?? ReviewId.New(),
            Title.Create(title ?? TestString(10)).Value,
            Description.Create(description ?? TestString(10)).Value,
            Username.Create(username ?? TestString(10)).Value,
            Stars.Create(stars ?? TestInt(1, 5)).Value
        );
    }
}