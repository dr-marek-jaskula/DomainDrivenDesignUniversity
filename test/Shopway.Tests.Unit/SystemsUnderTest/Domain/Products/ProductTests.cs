using Shopway.Tests.Unit.Abstractions;
using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Shopway.Domain.ValueObjects;
using Shopway.Domain.DomainEvents;

namespace Shopway.Tests.Unit.SystemsUnderTest.Domain.Products;

public sealed class ProductTests : TestBase
{
    [Fact]
    public void ProductId_ShouldCreateProductIdFromGuid_WhenInputGuid()
    {
        //Arrange
        var guid = Guid.NewGuid();

        //Act
        var productId = ProductId.Create(guid);

        //Assert
        productId.Value.Should().Be(guid);
    }

    [Fact]
    public void CreateProduct_ShouldCreateProduct_WhenValidValueObjects()
    {
        //Arrange
        var productId = ProductId.New();
        var productName = ProductName.Create(TestString(10)).Value;
        var price = Price.Create(TestInt(1, 10)).Value;
        var uomCode = UomCode.Create(UomCode.AllowedUomCodes.First()).Value;
        var revision = Revision.Create(TestString(2)).Value;

        //Act
        var product = Product.Create
        (
            productId,
            productName,
            price,
            uomCode,
            revision
        );

        //Assert
        product.Id.Should().Be(productId);
        product.ProductName.Should().Be(productName);
        product.Revision.Should().Be(revision);
        product.Price.Should().Be(price);
        product.UomCode.Should().Be(uomCode);
    }

    [Fact]
    public void CreateProduct_ShouldRiseProductCreatedDomainEvent_WhenValidValueObjects()
    {
        //Arrange
        var productId = ProductId.New();
        var productName = ProductName.Create(TestString(10)).Value;
        var price = Price.Create(TestInt(1, 10)).Value;
        var uomCode = UomCode.Create(UomCode.AllowedUomCodes.First()).Value;
        var revision = Revision.Create(TestString(2)).Value;

        //Act
        var product = Product.Create
        (
            productId,
            productName,
            price,
            uomCode,
            revision
        );

        //Assert
        product.DomainEvents.Should().HaveCount(1);
        var domainEvent = product.DomainEvents.First();
        domainEvent.Should().BeOfType<ProductCreatedDomainEvent>();
        ((ProductCreatedDomainEvent)domainEvent).ProductId.Should().Be(productId);
    }

    [Fact]
    public void UpdatePrice_ShouldUpdateProductPrice_WhenValidPrice()
    {
        //Arrange
        var product = CreateProduct(ProductId.New());
        var newPrice = Price.Create(100).Value;

        //Act
        product.UpdatePrice(newPrice);

        //Assert
        product.Price.Should().Be(newPrice);
    }

    [Fact]
    public void UpdateUomCode_ShouldUpdateProductUomCode_WhenValidUomCode()
    {
        //Arrange
        var product = CreateProduct(ProductId.New());
        var newUomCode = UomCode.Create(UomCode.AllowedUomCodes.Last()).Value;

        //Act
        product.UpdateUomCode(newUomCode);

        //Assert
        product.UomCode.Should().Be(newUomCode);
    }

    [Fact]
    public void AddReview_ShouldAddReview_WhenWhenValidReview()
    {
        //Arrange
        var product = CreateProduct(ProductId.New());
        var review = CreateReview(ReviewId.New());

        //Act
        product.AddReview(review);

        //Assert
        product.Reviews.Should().Contain(review);
    }

    [Fact]
    public void AnyReview_ShouldReturnTrue_WhenReviewWithGivenTitleExists()
    {
        //Arrange
        var product = CreateProduct(ProductId.New());
        var review = CreateReview(ReviewId.New());

        //Act
        product.AddReview(review);

        //Assert
        product.AnyReview(review.Title).Should().BeTrue();
    }
}