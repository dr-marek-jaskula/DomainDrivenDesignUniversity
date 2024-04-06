using Shopway.Domain.Common.Errors;
using Shopway.Domain.Users.ValueObjects;
using Shopway.Tests.Unit.Abstractions;
using static Shopway.Domain.Users.Errors.DomainErrors;
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
            Add(tooLongEmail, [EmailError.TooLong, EmailError.Invalid]);

            string emptyEmail = string.Empty;
            Add(emptyEmail, [EmailError.Empty, EmailError.Invalid]);

            string whitespaceEmail = "    ";
            Add(whitespaceEmail, [EmailError.Empty, EmailError.Invalid]);
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