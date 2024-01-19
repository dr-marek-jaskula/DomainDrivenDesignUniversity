using Shopway.Domain.Errors;
using Shopway.Domain.Users.ValueObjects;
using Shopway.Tests.Unit.Abstractions;
using static Shopway.Domain.Users.Errors.DomainErrors;
using static Shopway.Tests.Unit.Constants.Constants;

namespace Shopway.Tests.Unit.LayerTests.Domain.ValueObjects;

[Trait(nameof(UnitTest), UnitTest.Domain)]
public sealed class FirstNameTests : TestBase
{
    private sealed class InvalidFirstNameTestData : TheoryData<string, Error>
    {
        public InvalidFirstNameTestData()
        {
            var invalidCharacter = "_";
            var firstNameWithNotAllowedCharacter = $"{TestString(10)}{invalidCharacter}";
            Add(firstNameWithNotAllowedCharacter, FirstNameError.ContainsIllegalCharacter);

            var tooLongFirstName = TestString(1000);
            Add(tooLongFirstName, FirstNameError.TooLong);

            string emptyFirstName = string.Empty;
            Add(emptyFirstName, FirstNameError.Empty);

            string whitespaceFirstName = "    ";
            Add(whitespaceFirstName, FirstNameError.Empty);
        }
    }

    [Theory]
    [ClassData(typeof(InvalidFirstNameTestData))]
    public void FirstName_ShouldNotCreate_WhenInvalidInput(string invalidFirstName, Error exceptedError)
    {
        //Act
        var firstNameResult = FirstName.Create(invalidFirstName);

        //Assert
        firstNameResult.IsFailure.Should().BeTrue();
        firstNameResult.Error.Should().Be(Error.ValidationError);
        firstNameResult.ValidationErrors.Should().HaveCount(1);
        firstNameResult.ValidationErrors.Should().Contain(exceptedError);
    }
}