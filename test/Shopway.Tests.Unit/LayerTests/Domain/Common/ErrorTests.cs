using Shopway.Domain.Errors;
using static Shopway.Tests.Unit.Constants.Constants;

namespace Shopway.Tests.Unit.LayerTests.Domain.Common;

[Trait(nameof(UnitTest), UnitTest.Domain)]
public sealed class ErrorTests
{
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
}