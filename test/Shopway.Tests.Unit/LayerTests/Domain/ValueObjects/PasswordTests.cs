using Shopway.Domain.Errors;
using Shopway.Domain.Users.ValueObjects;
using Shopway.Tests.Unit.Abstractions;
using static Shopway.Domain.Users.Errors.DomainErrors;
using static Shopway.Tests.Unit.Constants.Constants;

namespace Shopway.Tests.Unit.LayerTests.Domain.ValueObjects;

[Trait(nameof(UnitTest), UnitTest.Domain)]
public sealed class PasswordTests : TestBase
{
    public static TheoryData<string, Error> InvalidPasswordTestData => new()
    {
        { "invalidPassword", PasswordError.Invalid },
        { "invalidPassword1", PasswordError.Invalid },
        { "invalidPassword!", PasswordError.Invalid },
        { "Sa1!", PasswordError.TooShort },
        { "tooLongTooLongToLongTooLongtooLongTooLongToLongTooLongtooLongTooLongToLongTooLong", PasswordError.TooLong },
        { " ", PasswordError.Empty },
        { "\n", PasswordError.Empty },
        { "\t", PasswordError.Empty },
        { " ", PasswordError.Empty },
        { string.Empty, PasswordError.Empty }
    };

    public static TheoryData<string> ValidPasswordTestData => new()
    {
        { "validTest123!" },
        { "1validTest123!" },
        { "!validTest123!" },
    };

    [Theory]
    [MemberData(nameof(InvalidPasswordTestData))]
    public void Password_ShouldNotCreate_WhenInvalidInput(string invalidPassword, Error exceptedError)
    {
        //Act
        var passwordResult = Password.Create(invalidPassword);

        //Assert
        passwordResult.IsFailure.Should().BeTrue();
        passwordResult.Error.Should().Be(Error.ValidationError);
        passwordResult.ValidationErrors.Should().Contain(exceptedError);
    }

    [Theory]
    [MemberData(nameof(ValidPasswordTestData))]
    public void Password_ShouldCreate_WhenValidInput(string invalidPassword)
    {
        //Act
        var passwordResult = Password.Create(invalidPassword);

        //Assert
        passwordResult.IsSuccess.Should().BeTrue();
    }
}