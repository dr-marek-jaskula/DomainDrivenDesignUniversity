using Shopway.Domain.Errors;
using Shopway.Domain.ValueObjects;
using Shopway.Tests.Unit.Abstractions;
using Shopway.Tests.Unit.Constants;
using static Shopway.Domain.Errors.Domain.DomainErrors;

namespace Shopway.Tests.Unit.LayerTests.Domain.ValueObjects;

[Trait(nameof(UnitTest), UnitTest.Domain)]
public sealed class EmailTests : TestBase
{
    private sealed class InvalidEmailTestData : TheoryData<string, Error[]>
    {
        public InvalidEmailTestData()
        {
            var tooLongProductName = TestString(1000);
            Add(tooLongProductName, new[] { EmailError.TooLong, EmailError.Invalid });

            string emptyProductName = string.Empty;
            Add(emptyProductName, new[] { EmailError.Empty, EmailError.Invalid });

            string whitespaceProductName = "    ";
            Add(whitespaceProductName, new[] { EmailError.Empty, EmailError.Invalid });
        }
    }

    [Theory]
    [ClassData(typeof(InvalidEmailTestData))]
    public void Email_ShouldNotCreate_WhenInvalidInput(string invalidEmail, Error[] exceptedError)
    {
        //Act
        var firstName = Email.Create(invalidEmail);

        //Assert
        firstName.IsFailure.Should().BeTrue();
        firstName.Error.Should().Be(Error.ValidationError);
        firstName.ValidationErrors.Should().HaveCount(2);
        firstName.ValidationErrors.Should().Contain(exceptedError);
    }
}