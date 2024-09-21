using Shopway.Domain.Common.Errors;
using Shopway.Domain.Users.ValueObjects;
using Shopway.Tests.Unit.Abstractions;

namespace Shopway.Tests.Unit.LayerTests.Domain.ValueObjects;

[UnitTest.Domain]
public sealed class PasswordTests : TestBase
{
    public static TheoryData<string, Error> InvalidPasswordTestData => new()
    {
        { "invalidPassword", Password.Invalid },
        { "invalidPassword1", Password.Invalid },
        { "invalidPassword!", Password.Invalid },
        { "Sa1!", Password.TooShort },
        { "tooLongTooLongToLongTooLongtooLongTooLongToLongTooLongtooLongTooLongToLongTooLong", Password.TooLong },
        { " ", Password.Empty },
        { "\n", Password.Empty },
        { "\t", Password.Empty },
        { " ", Password.Empty },
        { string.Empty, Password.Empty }
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
