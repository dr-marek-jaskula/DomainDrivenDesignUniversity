using Shopway.Domain.Users.Enumerations;
using static Shopway.Domain.Common.Utilities.EnumUtilities;

namespace Shopway.Tests.Unit.UtilityTests;

[UnitTest.Utility]
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
