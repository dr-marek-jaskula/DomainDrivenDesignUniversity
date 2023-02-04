using static Shopway.Domain.Utilities.RandomUtilities;

namespace Shopway.Tests.Unit.Abstractions;

public abstract class TestBase
{
    protected readonly string CreatedBy = $"{APP_PREFIX}_{GenerateString(Length)}";
    private const string APP_PREFIX = "auto_shopway";
    private const int Length = 22;

    /// <summary>
    /// Generates test string
    /// </summary>
    /// <param name="length">Test string length</param>
    /// <returns>Test string</returns>
    protected static string TestString(int length = Length)
    {
        return $"{APP_PREFIX}_test_{GenerateString(length)}";
    }

    /// <summary>
    /// Generates not trimmed test string
    /// </summary>
    /// <param name="length">The length of string (not including spaces)</param>
    /// <returns>Test string with enter, tabs and white spaces from both sides</returns>
    protected static string NotTrimmedTestString(int length = Length)
    {
        return $" \n  \t \n    \t {APP_PREFIX}_test_{GenerateString(length)}  \n \t   \n ";
    }
}