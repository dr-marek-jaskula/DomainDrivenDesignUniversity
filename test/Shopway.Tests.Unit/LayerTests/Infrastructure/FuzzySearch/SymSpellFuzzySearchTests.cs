﻿using Shopway.Application.Abstractions;
using Shopway.Infrastructure.FuzzySearch;

namespace Shopway.Tests.Unit.LayerTests.Infrastructure.FuzzySearch;

[UnitTest.Infrastructure]
public sealed class SymSpellFuzzySearchTests
{
    private static readonly IFuzzySearch _fuzzySearch;

    static SymSpellFuzzySearchTests()
    {
        var symSpellFuzzySearchFactory = new SymSpellFuzzySearchFactory();
        _fuzzySearch = symSpellFuzzySearchFactory.Create();
    }

    private sealed class StringToSegmentTestData : TheoryData<string, string>
    {
        public StringToSegmentTestData()
        {
            Add("wolfintheforest", "wolf in the forest");
            Add("beautifulflowernear theriver", "beautiful flower near the river");
            Add("veryfastcar", "very fast car");
        }
    }

    private sealed class StringToFuzzySearchTestData : TheoryData<IList<string>, string, string>
    {
        public StringToFuzzySearchTestData()
        {
            Add(["wolf", "deer", "snake", "bear", "grizzly"], "grizzly", "grimzli");
            Add(["beautiful", "flower", "near", "the", "river"], "flower", "flomer");
            Add(["super", "very", "fast", "dignity", "majestic", "dusk"], "dignity", "diggnyty");
        }
    }

    [Theory]
    [ClassData(typeof(StringToSegmentTestData))]
    public void SymSpellForEnDictionary_SegmentString_ShouldSegmentString_WhenInputStringDoesNotContainAnyWhiteSpaces(string toSegment, string segmented)
    {
        //Act
        var result = _fuzzySearch.WordSegmentation(toSegment);

        //Assert
        result.Value.Should().Be(segmented);
    }

    [Theory]
    [ClassData(typeof(StringToFuzzySearchTestData))]
    public void SymSpellForCustomSet_FindBestSuggestion_ShouldFindSuggestion_WhenInputStringDoesNotStronglyDifferFromOnesInSearchSet(IList<string> searchSet, string expected, string actual)
    {
        //Assert
        var symSpellFuzzySearchFactory = new SymSpellFuzzySearchFactory();
        var fuzzySearch = symSpellFuzzySearchFactory.Create(searchSet);

        //Act
        var result = fuzzySearch.FindBestSuggestion(actual);

        //Assert
        result.Value.Should().Be(expected);
    }
}
