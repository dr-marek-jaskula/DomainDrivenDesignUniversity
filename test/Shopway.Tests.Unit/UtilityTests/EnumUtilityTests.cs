using static Shopway.Tests.Unit.Constants.Constants;
using static Shopway.Domain.Common.Utilities.EnumUtilities;
using Shopway.Domain.Users.Enumerations;

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