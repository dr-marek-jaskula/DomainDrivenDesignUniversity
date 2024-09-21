using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using static Shopway.Domain.Common.Results.ResultUtilities;

namespace Shopway.Tests.Unit.UtilityTests;

[UnitTest.Utility]
public sealed class TypeUtilityTests
{
    private sealed class ResultTestData : TheoryData<Type>
    {
        public ResultTestData()
        {
            Add(Result.Success().GetType());
            Add(Result.Failure(Error.ConditionNotSatisfied).GetType());
            Add(Result.Failure(Error.ConditionNotSatisfied).GetType());
            Add(Result.Success(5).GetType());
            Add(Result.Success("value").GetType());
        }
    }

    private sealed class OnlyGenericResultTestData : TheoryData<Type>
    {
        public OnlyGenericResultTestData()
        {
            Add(Result.Success(5).GetType());
            Add(Result.Success("value").GetType());
            Add(Result.Success(true).GetType());
        }
    }

    private sealed class OnlyGenericResultWithUnderlyingTypeTestData : TheoryData<Type, Type>
    {
        public OnlyGenericResultWithUnderlyingTypeTestData()
        {
            Add(Result.Success(5).GetType(), typeof(int));
            Add(Result.Success("value").GetType(), typeof(string));
            Add(Result.Success(true).GetType(), typeof(bool));
        }
    }

    [Theory]
    [ClassData(typeof(ResultTestData))]
    public void IsResult_ShouldReturnTrue_WhenProvidedTypeIsResult(Type type)
    {
        //Act
        var isResult = type.IsResult();

        //Assert
        isResult.Should().BeTrue();
    }

    [Fact]
    public void IsResult_ShouldReturnFalse_WhenProvidedTypeIsBotResult()
    {
        //Act
        var isResult = typeof(int).IsResult();

        //Assert
        isResult.Should().BeFalse();
    }

    [Theory]
    [ClassData(typeof(OnlyGenericResultTestData))]
    public void IsGenericResult_ShouldReturnTrue_WhenProvidedTypeIsGenericResult(Type type)
    {
        //Act
        var isGenericResult = type.IsGenericResult();

        //Assert
        isGenericResult.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(OnlyGenericResultWithUnderlyingTypeTestData))]
    public void GetUnderlyingType_ShouldReturnUnderlyingType_WhenProvidedTypeIsGenericResult(Type type, Type underlyingType)
    {
        //Act
        var underlyingTypeResult = type.GetUnderlyingType();

        //Assert
        underlyingTypeResult.Should().Be(underlyingType);
    }
}
