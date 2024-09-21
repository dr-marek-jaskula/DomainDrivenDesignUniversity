using Shopway.Application.Exceptions;
using Shopway.Domain.Common.Errors;

namespace Shopway.Tests.Unit.LayerTests.Domain.Common;

[UnitTest.Domain]
public sealed class ErrorTests
{
    private const string InvalidOperationExceptionMessage = "This was invalid operation";
    private const string ArgumentExceptionMessage = "Invalid argument";
    private const string NotFoundExceptionMessage = "Not found";

    [Fact]
    public void ThrowIfErrorNone_ShouldThrowAnException_WhenErrorIsNone()
    {
        //Arrange
        var error = Error.None;

        //Assert
        FluentActions
            .Invoking(error.ThrowIfErrorNone)
            .Should()
            .ThrowExactly<InvalidOperationException>()
            .Which
            .Message
            .Should()
            .Be("Provided error is Error.None");
    }

    [Fact]
    public void FromException_ErrorMessage_ShouldContainExceptionMessage()
    {
        //Arrange
        var invalidOperationException = new InvalidOperationException(InvalidOperationExceptionMessage);

        //Act
        var error = Error.FromException(invalidOperationException);

        //Assert
        error.Code.Should().Be(nameof(InvalidOperationException));
        error.Message.Should().Contain(InvalidOperationExceptionMessage);
    }

    [Fact]
    public void FromException_ErrorMessage_ShouldContainInnerExceptionMessage_WhenInnerExceptionExists()
    {
        //Arrange
        var invalidOperationException = new InvalidOperationException(InvalidOperationExceptionMessage, new ArgumentException(ArgumentExceptionMessage));

        //Act
        var error = Error.FromException(invalidOperationException);

        //Assert
        error.Code.Should().Be(nameof(InvalidOperationException));
        error.Message.Should().Contain(InvalidOperationExceptionMessage, ArgumentExceptionMessage);
    }

    [Fact]
    public void FromException_ErrorMessage_ShouldContainAllInnerExceptionsMessages_WhenAggregateExceptionIsProvided()
    {
        //Arrange
        var aggregateException = new AggregateException
        (
            new InvalidOperationException(InvalidOperationExceptionMessage),
            new ArgumentException(ArgumentExceptionMessage),
            new NotFoundException(NotFoundExceptionMessage)
        );

        //Act
        var error = Error.FromException(aggregateException);

        //Assert
        error.Code.Should().Be(nameof(AggregateException));
        error.Message.Should().ContainAll(InvalidOperationExceptionMessage, ArgumentExceptionMessage, NotFoundExceptionMessage);
    }
}
