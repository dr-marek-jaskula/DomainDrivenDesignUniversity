using Shopway.Domain.Errors;
using Shopway.Domain.Results;
using Shopway.Tests.Unit.Constants;

namespace Shopway.Tests.Unit.LayerTests.Domain.Common;

[Trait(nameof(UnitTest), UnitTest.Domain)]
public sealed class ResultTests
{
    [Fact]
    public void GettingValueFromResult_ShouldThrowAnException_WhenResultIsFailureResult()
    {
        //Arrange
        var result = Result.Failure<string>(Error.ConditionNotSatisfied);
        string GetValueFromFailureResult() => result.Value;

        //Assert
        FluentActions
            .Invoking(GetValueFromFailureResult)
            .Should()
            .ThrowExactly<InvalidOperationException>()
            .Which
            .Message
            .Should()
            .Be("The value of a failure result can not be accessed. Type 'System.String'.");
    }
}