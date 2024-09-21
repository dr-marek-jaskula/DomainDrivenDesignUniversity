using Shopway.Domain.Common.Utilities;
using static Shopway.Domain.Common.Utilities.RangeUtility;
using static Shopway.Domain.Common.Utilities.StringUtilities;

namespace Shopway.Tests.Unit.UtilityTests;

[UnitTest.Utility]
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

    [Theory]
    [InlineData("", "%", true)]
    [InlineData(" ", "%", true)]
    [InlineData("asdfa asdf asdf", "%", true)]
    [InlineData("%", "%", true)]
    [InlineData("", "_", false)]
    [InlineData(" ", "_", true)]
    [InlineData("4", "_", true)]
    [InlineData("C", "_", true)]
    [InlineData("c", "_", true)]
    [InlineData("CX", "_", false)]
    [InlineData("cx", "_", false)]
    [InlineData("a", "[A]", true)]
    [InlineData("ab", "[A]", false)]
    [InlineData("", "[ABCD]", false)]
    [InlineData("A", "[ABCD]", true)]
    [InlineData("b", "[ABCD]", true)]
    [InlineData("X", "[ABCD]", false)]
    [InlineData("AB", "[ABCD]", false)]
    [InlineData("C", "[B-D]", true)]
    [InlineData("D", "[B-D]", true)]
    [InlineData("A", "[B-D]", false)]
    [InlineData("C", "[^B-D]", false)]
    [InlineData("D", "[^B-D]", false)]
    [InlineData("A", "[^B-D)]", true)]
    [InlineData("lolTESTBXXX", "%TEST[ABCD]XXX", true)]
    [InlineData("lolTESTZXXX", "%TEST[ABCD]XXX", false)]
    [InlineData("lolTESTBXXX", "%TEST[^ABCD]XXX", false)]
    [InlineData("lolTESTZXXX", "%TEST[^ABCD]XXX", true)]
    [InlineData("lolTESTBXXX", "%TEST[B-D]XXX", true)]
    [InlineData("lolTESTZXXX", "%TEST[^B-D)]XXX", true)]
    [InlineData("Stuff.txt", "%Stuff.txt", true)]
    [InlineData("MagicStuff.txt", "%Stuff.txt", true)]
    [InlineData("MagicStuff.txt.img", "%Stuff.txt", false)]
    [InlineData("Stuff.txt.img", "%Stuff.txt", false)]
    [InlineData("MagicStuff001.txt.img", "%Stuff.txt", false)]
    [InlineData("Stuff.txt", "Stuff.txt%", true)]
    [InlineData("MagicStuff.txt", "Stuff.txt%", false)]
    [InlineData("MagicStuff.txt.img", "Stuff.txt%", false)]
    [InlineData("Stuff.txt.img", "Stuff.txt%", true)]
    [InlineData("MagicStuff001.txt.img", "Stuff.txt%", false)]
    [InlineData("Stuff.txt", "%Stuff.txt%", true)]
    [InlineData("MagicStuff.txt", "%Stuff.txt%", true)]
    [InlineData("MagicStuff.txt.img", "%Stuff.txt%", true)]
    [InlineData("Stuff.txt.img", "%Stuff.txt%", true)]
    [InlineData("MagicStuff001.txt.img", "%Stuff.txt%", false)]
    [InlineData("Stuff.txt", "%Stuff%.txt", true)]
    [InlineData("MagicStuff.txt", "%Stuff%.txt", true)]
    [InlineData("MagicStuff.txt.img", "%Stuff%.txt", false)]
    [InlineData("Stuff.txt.img", "%Stuff%.txt", false)]
    [InlineData("MagicStuff001.txt.img", "%Stuff%.txt", false)]
    [InlineData("MagicStuff001.txt", "%Stuff%.txt", true)]
    [InlineData("Stuff.txt", "Stuff%.txt%", true)]
    [InlineData("MagicStuff.txt", "Stuff%.txt%", false)]
    [InlineData("MagicStuff.txt.img", "Stuff%.txt%", false)]
    [InlineData("Stuff.txt.img", "Stuff%.txt%", true)]
    [InlineData("MagicStuff001.txt.img", "Stuff%.txt%", false)]
    [InlineData("MagicStuff001.txt", "Stuff%.txt%", false)]
    [InlineData("Stuff.txt", "%Stuff%.txt%", true)]
    [InlineData("MagicStuff.txt", "%Stuff%.txt%", true)]
    [InlineData("MagicStuff.txt.img", "%Stuff%.txt%", true)]
    [InlineData("Stuff.txt.img", "%Stuff%.txt%", true)]
    [InlineData("MagicStuff001.txt.img", "%Stuff%.txt%", true)]
    [InlineData("MagicStuff001.txt", "%Stuff%.txt%", true)]
    [InlineData("1Stuff3.txt4", "_Stuff_.txt_", true)]
    [InlineData("1Stuff.txt4", "_Stuff_.txt_", false)]
    [InlineData("1Stuff3.txt", "_Stuff_.txt_", false)]
    [InlineData("Stuff3.txt4", "_Stuff_.txt_", false)]
    public void SqlLike(string input, string pattern, bool expected)
    {
        var result = input.Like(pattern);

        result.Should().Be(expected);
    }
}
