using Shopway.Domain.Errors;
using Shopway.Domain.ValueObjects;
using Shopway.Tests.Unit.Abstractions;
using Shopway.Tests.Unit.Constants;
using static Shopway.Domain.Errors.Domain.DomainErrors;

namespace Shopway.Tests.Unit.LayerTests.Domain.ValueObjects;

public sealed class ProductNameTests : TestBase
{
    private sealed class InvalidProductNameTestData : TheoryData<string, Error>
    {
        public InvalidProductNameTestData()
        {
            var invalidCharacter = "_";
            var productNameWithNotAllowedCharacter = $"{TestString(10)}{invalidCharacter}";
            Add(productNameWithNotAllowedCharacter, ProductNameError.ContainsIllegalCharacter);

            var tooLongProductName = TestString(1000);
            var sss = tooLongProductName.Length;
            Add(tooLongProductName, ProductNameError.TooLong);

            string emptyProductName = string.Empty;
            Add(emptyProductName, ProductNameError.Empty);

            string whitespaceProductName = "    ";
            Add(whitespaceProductName, ProductNameError.Empty);
        }
    }

    [Fact]
    [Trait(TraitConstants.Category, TraitConstants.Domain)]
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
    [Trait(TraitConstants.Category, TraitConstants.Domain)]
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