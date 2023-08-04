using Shopway.Domain.Errors;
using Shopway.Domain.ValueObjects;
using Shopway.Tests.Unit.Abstractions;
using Shopway.Tests.Unit.Constants;
using static Shopway.Domain.Errors.Domain.DomainErrors;

namespace Shopway.Tests.Unit.LayerTests.Domain.ValueObjects;

[Trait(nameof(UnitTest), UnitTest.Domain)]
public sealed class FirstNameTests : TestBase
{
    private sealed class InvalidFirstNameTestData : TheoryData<string, Error>
    {
        public InvalidFirstNameTestData()
        {
            var invalidCharacter = "_";
            var productNameWithNotAllowedCharacter = $"{TestString(10)}{invalidCharacter}";
            Add(productNameWithNotAllowedCharacter, FirstNameError.ContainsIllegalCharacter);

            var tooLongProductName = TestString(1000);
            Add(tooLongProductName, FirstNameError.TooLong);

            string emptyProductName = string.Empty;
            Add(emptyProductName, FirstNameError.Empty);

            string whitespaceProductName = "    ";
            Add(whitespaceProductName, FirstNameError.Empty);
        }
    }

    [Theory]
    [ClassData(typeof(InvalidFirstNameTestData))]
    public void FirstName_ShouldNotCreate_WhenInvalidInput(string invalidFirstName, Error exceptedError)
    {
        //Act
        var firstName = FirstName.Create(invalidFirstName);

        //Assert
        firstName.IsFailure.Should().BeTrue();
        firstName.Error.Should().Be(Error.ValidationError);
        firstName.ValidationErrors.Should().HaveCount(1);
        firstName.ValidationErrors.Should().Contain(exceptedError);
    }
}