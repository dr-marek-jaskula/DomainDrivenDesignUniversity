using Shopway.Domain.Common.Errors;
using Shopway.Domain.Products.ValueObjects;
using Shopway.Tests.Unit.Abstractions;

namespace Shopway.Tests.Unit.LayerTests.Domain.ValueObjects;

[UnitTest.Domain]
public sealed class ProductNameTests : TestBase
{
    private sealed class InvalidProductNameTestData : TheoryData<string, Error>
    {
        public InvalidProductNameTestData()
        {
            var invalidCharacter = "_";
            var productNameWithNotAllowedCharacter = $"{TestString(10)}{invalidCharacter}";
            Add(productNameWithNotAllowedCharacter, ProductName.ContainsIllegalCharacter);

            var tooLongProductName = TestString(1000);
            Add(tooLongProductName, ProductName.TooLong);

            string emptyProductName = string.Empty;
            Add(emptyProductName, ProductName.Empty);

            string whitespaceProductName = "    ";
            Add(whitespaceProductName, ProductName.Empty);
        }
    }

    [Fact]
    public void CreateProductName_ShouldCreateProductName_WhenValidInput()
    {
        //Arrange
        var validProductName = TestString(10);

        //Act
        var productNameResult = ProductName.Create(validProductName);

        //Assert
        productNameResult.IsSuccess.Should().BeTrue();
        productNameResult.Value.Value.Should().Be(validProductName);
    }

    [Theory]
    [ClassData(typeof(InvalidProductNameTestData))]
    public void CreateProductName_ShouldNotCreateProductName_WhenInvalidInput(string invalidProductName, Error expectedError)
    {
        //Act
        var productNameResult = ProductName.Create(invalidProductName);

        //Assert
        productNameResult.IsFailure.Should().BeTrue();
        productNameResult.Error.Should().Be(Error.ValidationError);
        productNameResult.ValidationErrors.Should().HaveCount(1);
        productNameResult.ValidationErrors.Should().Contain(expectedError);
    }
}
