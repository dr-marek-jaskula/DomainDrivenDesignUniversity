using Shopway.Domain.Common.Utilities;
using System.Diagnostics;

namespace Shopway.Tests.Unit.UtilityTests;

[UnitTest.Utility]
public sealed class TaskTests
{
    [Fact]
    public async Task TaskWhenAllOnTuple_ShouldRunInParallelAndReturnProperValues_WhenTasksDoNotThrowExceptions()
    {
        //Act
        var stopwatch = Stopwatch.StartNew();
        var result = await (GetInt(1), GetString("1"));
        var time = stopwatch.ElapsedMilliseconds;

        //Assert
        result.Should().Be((1, "1"));
        time.Should().BeLessThan(2000);
    }

    [Fact]
    public async Task TaskWhenAllOnTuple_ShouldThrowAggregatesException_WhenTasksThrowExceptions()
    {
        //Arrange
        var action = async () => await (ThrowExceptionWhenTryingToGetInt(), ThrowExceptionWhenTryingToGetString());

        //Assert
        await action
            .Should()
            .ThrowExactlyAsync<AggregateException>();
    }

    [Fact]
    public async Task TaskWhenAllOnEnumerable_ShouldRunInParallelAndReturnProperValues_WhenTasksDoNotThrowExceptions()
    {
        //Act
        var stopwatch = Stopwatch.StartNew();

        var result = await new List<Task<int>>()
        {
            GetInt(1),
            GetInt(2)
        };

        var time = stopwatch.ElapsedMilliseconds;

        //Assert
        result.Should().HaveCount(2);
        result.First().Should().Be(1);
        result.Last().Should().Be(2);
        time.Should().BeLessThan(2000);
    }

    [Fact]
    public async Task TaskWhenAllOnEnumerable_ShouldThrowAggregatesException_WhenTasksThrowExceptions()
    {
        //Arrange
        var action = async () => await new List<Task<int>>()
        {
            ThrowExceptionWhenTryingToGetInt(),
            ThrowExceptionWhenTryingToGetIntTwo()
        };

        //Assert
        await action
            .Should()
            .ThrowExactlyAsync<AggregateException>();
    }

    private static async Task<int> GetInt(int result)
    {
        await Task.Delay(1000);
        return result;
    }

    private static async Task<string> GetString(string result)
    {
        await Task.Delay(1000);
        return result;
    }

    private static async Task<int> ThrowExceptionWhenTryingToGetInt()
    {
        await Task.Delay(1000);
        throw new ArgumentException(nameof(ThrowExceptionWhenTryingToGetInt));
    }

    private static async Task<int> ThrowExceptionWhenTryingToGetIntTwo()
    {
        await Task.Delay(1000);
        throw new ArgumentException(nameof(ThrowExceptionWhenTryingToGetIntTwo));
    }

    private static async Task<string> ThrowExceptionWhenTryingToGetString()
    {
        await Task.Delay(1000);
        throw new InvalidOperationException(nameof(ThrowExceptionWhenTryingToGetString));
    }
}
