using Shopway.Domain.Enums;
using Shopway.Tests.Unit.Constants;
using static Shopway.Domain.Utilities.EnumUtilities;

namespace Shopway.Tests.Unit.UtilityTests;

[Trait(nameof(UnitTest), UnitTest.Utility)]
public sealed class EnumUtilityTests
{
    [Fact]
    public void LongestOf_ShouldReturnRankStandardLength()
    {
        //Act
        var result = LongestOf<Rank>();

        //Assert
        result.Should().Be(Rank.Standard.ToString().Length);
    }
}