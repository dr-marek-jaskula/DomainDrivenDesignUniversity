using static Shopway.Domain.Utilities.RangeUtility;
using static Shopway.Domain.Utilities.StringUtilities;

namespace Shopway.Tests.Unit.SystemsUnderTest.Utilities;

public sealed class StringUtilitiesTests
{
    private sealed class DigitTestData : TheoryData<int>
    {
        public DigitTestData()
        {
            foreach (var digit in 1..9)
            {
                Add(digit);
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

    [Theory]
    [InlineData("string", "String")]
    [InlineData("STRING", "string")]
    [InlineData("string", "string")]
    public void CaseInsensitiveEquals_ShouldReturnTrue_WhenStringsDifferByCaseSensitiveness(string frist, string second)
    {
        //Act
        var result = frist.CaseInsensitiveEquals(second);

        //Assert
        result.Should().BeTrue();
    }
}