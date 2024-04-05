using Microsoft.IdentityModel.Tokens;
using Shopway.Application.Abstractions;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using static SymSpell;

namespace Shopway.Infrastructure.FuzzySearch;

//Verbosity.Top: suggestion with the highest term frequency of the suggestions of smallest edit distance found.
//Verbosity.Closest: All suggestions of smallest edit distance found, suggestions ordered by term frequency.
//All: All suggestions within maxEditDistance, suggestions ordered by edit distance, then by term frequency (slower, no early termination).
internal sealed class SymSpellFuzzySearch(SymSpell fuzzySearchEngine) : IFuzzySearch
{
    private readonly SymSpell _fuzzySearchEngine = fuzzySearchEngine;
    private const int DefaultMaxTermDistance = 2;

    /// <summary>
    /// Returns the closest term to the input, based on the predefined directory (biggest frequency, lowest distance)
    /// </summary>
    /// <param name="stringToApproximate">Input string to approximate</param>
    /// <param name="maxEditDistance">Distance from the stringToApproximate</param>
    /// <returns>Output approximated string</returns>
    public Result<string> FindBestSuggestion(string stringToApproximate, int maxEditDistance = DefaultMaxTermDistance)
    {
        if (maxEditDistance < 0)
        {
            var error = Error.New(nameof(SymSpellFuzzySearch), $"Distance must be a non negative value. Current value: '{maxEditDistance}'.");
            return Result.Failure<string>(error);
        }

        var suggestions = _fuzzySearchEngine.Lookup(stringToApproximate, Verbosity.Top, maxEditDistance);

        if (suggestions.IsNullOrEmpty())
        {
            var error = Error.New(nameof(SymSpellFuzzySearch), $"There is no suggestion for '{stringToApproximate}' in maximal distance: '{maxEditDistance}'.");
            return Result.Failure<string>(error);
        }

        return suggestions
            .First()
            .term;
    }

    /// <summary>
    /// Returns the closest terms to the input, based on the predefined directory (biggest frequency, lowest distance)
    /// </summary>
    /// <param name="stringToApproximate">Input string to approximate</param>
    /// <param name="count">The number of results</param>
    /// <param name="maxEditDistance">Distance from the stringToApproximate</param>
    /// <returns>Specified number of approximated strings</returns>
    public Result<IList<string>> FindMultipleBestSuggestions(string stringToApproximate, int count, int maxEditDistance = DefaultMaxTermDistance)
    {
        if (maxEditDistance < 0)
        {
            var error = Error.New(nameof(SymSpellFuzzySearch), $"Distance must be a non negative value. Current value: '{maxEditDistance}'.");
            return Result.Failure<IList<string>>(error);
        }

        if (count < 1)
        {
            var error = Error.New(nameof(SymSpellFuzzySearch), $"Invalid count: '{count}'. It has be at least 1.");
            return Result.Failure<IList<string>>(error);
        }

        var suggestions = _fuzzySearchEngine.Lookup(stringToApproximate, Verbosity.Closest, maxEditDistance);

        if (suggestions.IsNullOrEmpty())
        {
            var error = Error.New(nameof(SymSpellFuzzySearch), $"There is no suggestion for '{stringToApproximate}' in maximal distance: '{maxEditDistance}'.");
            return Result.Failure<IList<string>>(error);
        }

        return suggestions
            .Take(count)
            .Select(s => s.term)
            .ToList();
    }

    /// <summary>
    /// Provides the string segmentation - decomposes the characters to meaningful words
    /// </summary>
    /// <param name="inputTerm">Term to be segmented, preferred input should contain non segmented text without misspellings</param>
    /// <param name="maxEditDistance">>Distance from the strings in inputTerm</param>
    /// <returns>The segmented string</returns>
    public Result<string> WordSegmentation(string inputTerm, int maxEditDistance = 0)
    {
        if (maxEditDistance < 0)
        {
            var error = Error.New(nameof(SymSpellFuzzySearch), $"Distance must be a non negative value. Current value: '{maxEditDistance}'.");
            return Result.Failure<string>(error);
        }

        var suggestion = _fuzzySearchEngine.WordSegmentation(inputTerm, maxEditDistance);
        return suggestion.correctedString;
    }
}