using static Shopway.Domain.Utilities.RandomUtilities;

namespace Shopway.Tests.Unit.Abstractions;

/// <summary>
/// Contains methods to create entities and utility methods for test data
/// </summary>
public abstract partial class TestBase
{
    protected readonly string CreatedBy = $"{APP_PREFIX}{GenerateString(Length)}";
    protected static readonly Random _random = new();
    private const string APP_PREFIX = "auto";
    private const int Length = 22;

    /// <summary>
    /// Generates test string
    /// </summary>
    /// <param name="length">Test string length</param>
    /// <returns>Test string</returns>
    protected static string TestString(int length = Length)
    {
        return $"{APP_PREFIX}{GenerateString(length)}";
    }

    /// <summary>
    /// Generates not trimmed test string
    /// </summary>
    /// <param name="length">The length of string (not including spaces)</param>
    /// <returns>Test string with enter, tabs and white spaces from both sides</returns>
    protected static string NotTrimmedTestString(int length = Length)
    {
        return $" \n  \t \n    \t {APP_PREFIX}{GenerateString(length)}  \n \t   \n ";
    }

    /// <summary>
    /// Generates test int in given range
    /// </summary>
    /// <param name="min">Lower bound</param>
    /// <param name="max">Upper bound</param>
    /// <returns></returns>
    public static int TestInt(int min = 1, int max = 1000)
    {
        return _random.Next(min, max);
    }
}