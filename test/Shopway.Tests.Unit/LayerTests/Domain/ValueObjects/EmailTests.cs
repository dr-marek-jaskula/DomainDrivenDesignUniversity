using Shopway.Domain.Common.Errors;
using Shopway.Domain.Users.ValueObjects;
using Shopway.Tests.Unit.Abstractions;
using static Shopway.Tests.Unit.Constants.Constants;

namespace Shopway.Tests.Unit.LayerTests.Domain.ValueObjects;

[Trait(nameof(UnitTest), UnitTest.Domain)]
public sealed class EmailTests : TestBase
{
    private sealed class InvalidEmailTestData : TheoryData<string, Error[]>
    {
        public InvalidEmailTestData()
        {
            var tooLongEmail = TestString(1000);
            Add(tooLongEmail, [Email.TooLong, Email.Invalid]);

            string emptyEmail = string.Empty;
            Add(emptyEmail, [Email.Empty, Email.Invalid]);

            string whitespaceEmail = "    ";
            Add(whitespaceEmail, [Email.Empty, Email.Invalid]);
        }
    }

    [Theory]
    [ClassData(typeof(InvalidEmailTestData))]
    public void Email_ShouldNotCreate_WhenInvalidInput(string invalidEmail, Error[] exceptedError)
    {
        //Act
        var emailResult = Email.Create(invalidEmail);

        //Assert
        emailResult.IsFailure.Should().BeTrue();
        emailResult.Error.Should().Be(Error.ValidationError);
        emailResult.ValidationErrors.Should().HaveCount(2);
        emailResult.ValidationErrors.Should().Contain(exceptedError);
    }
}
