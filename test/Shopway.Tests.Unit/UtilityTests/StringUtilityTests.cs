using Shopway.Domain.Common.Utilities;
using static Shopway.Tests.Unit.Constants.Constants;
using static Shopway.Domain.Common.Utilities.RangeUtility;
using static Shopway.Domain.Common.Utilities.StringUtilities;

namespace Shopway.Tests.Unit.UtilityTests;

[Trait(nameof(UnitTest), UnitTest.Utility)]
public sealed class StringUtilityTests
{
    private sealed class DigitTestData : TheoryData<int>
    {
        public DigitTestData()
        {
            foreach (var digit in 0..9)
            {
                Add(digit);
            }
        }
    }

    private sealed class IllegalCharacterTestData : TheoryData<char>
    {
        public IllegalCharacterTestData()
        {
            foreach (var illegalCharacter in IllegalCharacters)
            {
                Add(illegalCharacter);
            }
        }
    }

    private sealed class ToTrimCharacterTestData : TheoryData<char>
    {
        public ToTrimCharacterTestData()
        {
            foreach (var toTrimCharacter in ToTrimCharacters)
            {
                Add(toTrimCharacter);
            }
        }
    }

    [Theory]
    [ClassData(typeof(DigitTestData))]
    public void ContainsDigit_ShouldReturnTrue_WhenDigitIsPresent(int digit)
    {
        //Arrange
        var stringWithDigit = $"stringWithDigit{digit}";

        //Act
        var result = stringWithDigit.ContainsDigit();

        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ContainsDigit_ShouldReturnFalse_WhenDigitIsNotPresent()
    {
        //Arrange
        var stringWithDigit = $"stringWithoutDigit";

        //Act
        var result = stringWithDigit.ContainsDigit();

        //Assert
        result.Should().BeFalse();
    }

    [Theory]
    [ClassData(typeof(IllegalCharacterTestData))]
    public void ContainsIllegalCharacter_ShouldReturnTrue_WhenIllegalCharacterIsPresent(char illegalCharacter)
    {
        //Arrange
        var stringWithIllegalCharacter = $"stringWithIllegalCharacter{illegalCharacter}";

        //Act
        var result = stringWithIllegalCharacter.ContainsIllegalCharacter();

        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ContainsIllegalCharacter_ShouldReturnFalse_WhenIllegalCharacterIsNotPresent()
    {
        //Arrange
        var stringWithoutIllegalCharacter = $"stringWithoutIllegalCharacter";

        //Act
        var result = stringWithoutIllegalCharacter.ContainsIllegalCharacter();

        //Assert
        result.Should().BeFalse();
    }

    [Theory]
    [ClassData(typeof(ToTrimCharacterTestData))]
    public void ContainsToTrimCharacter_ShouldReturnTrue_WhenToTrimCharacterIsPresent(char toTrimCharacter)
    {
        //Arrange
        var stringWithToTrimCharacter = $"stringWithToTrimCharacter{toTrimCharacter}";

        //Act
        var result = stringWithToTrimCharacter.ContainsToTrimCharacter();

        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ContainsToTrimCharacter_ShouldReturnFalse_WhenToTrimCharacterIsNotPresent()
    {
        //Arrange
        var stringWithoutToTrimCharacter = $"stringWithoutToTrimCharacter";

        //Act
        var result = stringWithoutToTrimCharacter.ContainsToTrimCharacter();

        //Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ContainsMultipleStrings_ShouldReturnTrue_WhenStringsArePresent()
    {
        //Arrange
        var first = "first";
        var second = "second";
        var @string = $"{first} then another and {second} and then third string";

        //Act
        var result = @string.Contains(first, second);

        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ContainsMultipleStrings_ShouldReturnFalse_WhenStringsAreNotPresent()
    {
        //Arrange
        var first = "first";
        var second = "second";
        var @string = $"none then another and none and then third string";

        //Act
        var result = @string.Contains(first, second);

        //Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void NotContainsMultipleStrings_ShouldReturnTrue_WhenStringsAreNotPresent()
    {
        //Arrange
        var first = "first";
        var second = "second";
        var @string = $"none then another and none and then third string";

        //Act
        var result = @string.NotContains(first, second);

        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void NotContainsMultipleStrings_ShouldReturnFalse_WhenStringsArePresent()
    {
        //Arrange
        var first = "first";
        var second = "second";
        var @string = $"{first} then another and {second} and then third string";

        //Act
        var result = @string.NotContains(first, second);

        //Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("string", "String")]
    [InlineData("STRING", "string")]
    [InlineData("string", "string")]
    public void CaseInsensitiveEquals_ShouldReturnTrue_WhenStringsDifferByCaseSensitiveness(string first, string second)
    {
        //Act
        var result = first.CaseInsensitiveEquals(second);

        //Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("string2", "String")]
    [InlineData("STRING", "astring")]
    [InlineData("str", "string")]
    public void CaseInsensitiveEquals_ShouldReturnFalse_WhenStringsDifferByContent(string first, string second)
    {
        //Act
        var result = first.CaseInsensitiveEquals(second);

        //Assert
        result.Should().BeFalse();
    }
}