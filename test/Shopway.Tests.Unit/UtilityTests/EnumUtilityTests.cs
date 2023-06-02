using Shopway.Domain.Enums;
using Shopway.Tests.Unit.Constants;
using System.Collections.Generic;
using static Shopway.Domain.Utilities.EnumUtilities;

namespace Shopway.Tests.Unit.UtilityTests;

public sealed class EnumUtilityTests
{
    [Fact]
    [Trait(TraitConstants.Category, TraitConstants.Utility)]
    public void LongestOf_ShouldReturnRankStandardLength()
    {
        //Act
        var result = LongestOf<Rank>();

        //Assert
        result.Should().Be(Rank.Standard.ToString().Length);
    }
}