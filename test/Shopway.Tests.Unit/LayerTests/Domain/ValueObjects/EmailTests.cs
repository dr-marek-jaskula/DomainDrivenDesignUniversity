using Shopway.Domain.Errors;
using Shopway.Domain.ValueObjects;
using Shopway.Tests.Unit.Abstractions;
using static Shopway.Tests.Unit.Constants.Constants;
using static Shopway.Domain.Errors.Domain.DomainErrors;

namespace Shopway.Tests.Unit.LayerTests.Domain.ValueObjects;

[Trait(nameof(UnitTest), UnitTest.Domain)]
public sealed class EmailTests : TestBase
{
    private sealed class InvalidEmailTestData : TheoryData<string, Error[]>
    {
        public InvalidEmailTestData()
        {
            var tooLongEmail = TestString(1000);
            Add(tooLongEmail, new[] { EmailError.TooLong, EmailError.Invalid });

            string emptyEmail = string.Empty;
            Add(emptyEmail, new[] { EmailError.Empty, EmailError.Invalid });

            string whitespaceEmail = "    ";
            Add(whitespaceEmail, new[] { EmailError.Empty, EmailError.Invalid });
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