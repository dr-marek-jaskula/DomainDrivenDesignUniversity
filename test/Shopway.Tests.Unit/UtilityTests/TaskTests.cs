using Shopway.Domain.Utilities;
using System.Diagnostics;
using static Shopway.Tests.Unit.Constants.Constants;

namespace Shopway.Tests.Unit.UtilityTests;

[Trait(nameof(UnitTest), UnitTest.Utility)]
public sealed class TaskTests
{
    [Fact]
    public async void TaskWhenAllOnTuple_ShouldRunInParallelAndReturnProperValues_WhenTasksDoNotThrowExceptions()
    {
        //Act
        var stopwatch = Stopwatch.StartNew();
        var result = await (GetInt(), GetString());
        var time = stopwatch.ElapsedMilliseconds;

        //Assert
        result.Should().Be((1, "1"));
        time.Should().BeLessThan(2000);
    }

    [Fact]
    public async Task TaskWhenAllOnTuple_ShouldRunInParallelAndReturnProperValues_WhenTasksThrowExceptions()
    {
        //Arrange
        var action = async () => await (ThrowExceptionWhenTryingToGetInt(), ThrowExceptionWhenTryingToGetString());

        //Assert
        await action
            .Should()
            .ThrowExactlyAsync<AggregateException>();
    }

    private static async Task<int> GetInt()
    {
        await Task.Delay(1000);
        return 1;
    }

    private static async Task<string> GetString()
    {
        await Task.Delay(1000);
        return "1";
    }

    private static async Task<int> ThrowExceptionWhenTryingToGetInt()
    {
        await Task.Delay(1000);
        throw new ArgumentException(nameof(ThrowExceptionWhenTryingToGetInt));
    }

    private static async Task<string> ThrowExceptionWhenTryingToGetString()
    {
        await Task.Delay(1000);
        throw new InvalidOperationException(nameof(ThrowExceptionWhenTryingToGetString));
    }
}