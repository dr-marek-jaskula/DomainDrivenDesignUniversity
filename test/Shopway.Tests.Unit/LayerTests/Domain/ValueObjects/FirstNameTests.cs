using Shopway.Domain.Common.Errors;
using Shopway.Domain.Users.ValueObjects;
using Shopway.Tests.Unit.Abstractions;

namespace Shopway.Tests.Unit.LayerTests.Domain.ValueObjects;

[UnitTest.Domain]
public sealed class FirstNameTests : TestBase
{
    private sealed class InvalidFirstNameTestData : TheoryData<string, Error>
    {
        public InvalidFirstNameTestData()
        {
            var invalidCharacter = "_";
            var firstNameWithNotAllowedCharacter = $"{TestString(10)}{invalidCharacter}";
            Add(firstNameWithNotAllowedCharacter, FirstName.ContainsIllegalCharacter);

            var tooLongFirstName = TestString(1000);
            Add(tooLongFirstName, FirstName.TooLong);

            string emptyFirstName = string.Empty;
            Add(emptyFirstName, FirstName.Empty);

            string whitespaceFirstName = "    ";
            Add(whitespaceFirstName, FirstName.Empty);
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
