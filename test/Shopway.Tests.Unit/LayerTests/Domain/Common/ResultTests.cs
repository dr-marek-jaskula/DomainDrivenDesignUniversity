using Shopway.Domain.Errors;
using Shopway.Domain.Results;
using static Shopway.Tests.Unit.Constants.Constants;

namespace Shopway.Tests.Unit.LayerTests.Domain.Common;

[Trait(nameof(UnitTest), UnitTest.Domain)]
public sealed class ResultTests
{
    [Fact]
    public void TwoSuccessResults_ShouldReferTheSameCachedResultInstance_WhenTwoNonGenericSuccessResultsAreCreated()
    {
        //Arrange
        var firstResult = Result.Success();
        var secondResult = Result.Success();

        //Act
        var actual = ReferenceEquals(firstResult, secondResult);

        //Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void GettingValueFromGenericResult_ShouldThrowAnException_WhenResultIsFailureStringResult()
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

    [Fact]
    public void GettingValueFromGenericResult_ShouldThrowAnException_WhenResultIsFailureIntResult()
    {
        //Arrange
        var result = Result.Failure<int>(Error.ConditionNotSatisfied);
        var action = () => result.Value;

        //Assert
        action
            .Should()
            .ThrowExactly<InvalidOperationException>()
            .Which
            .Message
            .Should()
            .Be("The value of a failure result can not be accessed. Type 'System.Int32'.");
    }
}