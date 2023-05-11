using Shopway.Tests.Unit.Constants;
using static Shopway.Domain.Utilities.RangeUtility;
using static Shopway.Domain.Utilities.StringUtilities;

namespace Shopway.Tests.Unit.UtilityTests;

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

    [Theory]
    [ClassData(typeof(DigitTestData))]
    [Trait(TraitConstants.Category, TraitConstants.Utility)]
    public void ContainsDigit_ShouldReturnTrue_WhenDigitIsPresent(int digit)
    {
        //Arrange
        var stringWithDigit = $"stringWithDigit{digit}";

        //Act
        var result = stringWithDigit.ContainsDigit();

        //Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("string", "String")]
    [InlineData("STRING", "string")]
    [InlineData("string", "string")]
    [Trait(TraitConstants.Category, TraitConstants.Utility)]
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
    [Trait(TraitConstants.Category, TraitConstants.Utility)]
    public void CaseInsensitiveEquals_ShouldReturnFalse_WhenStringsDifferByContent(string first, string second)
    {
        //Act
        var result = first.CaseInsensitiveEquals(second);

        //Assert
        result.Should().BeFalse();
    }
}