using static Shopway.Domain.Common.Utilities.RandomUtilities;

namespace Shopway.Tests.Unit.Abstractions;

/// <summary>
/// Contains methods to create entities and utility methods for test data
/// </summary>
public abstract partial class TestBase
{
    protected readonly string CreatedBy = $"{AUTO_PREFIX}{GenerateString(Length)}";
    protected static readonly Random _random = new();
    private const string AUTO_PREFIX = "auto";
    private const int Length = 22;

    /// <summary>
    /// Generates test string
    /// </summary>
    /// <param name="length">Test string length</param>
    /// <returns>Test string</returns>
    protected static string TestString(int length = Length)
    {
        return $"{GenerateString(length)}";
    }

    /// <summary>
    /// Generates test string with 'auto' prefix
    /// </summary>
    /// <param name="length">Must be greater than 4</param>
    /// <returns>Test string with prefix</returns>
    /// <exception cref="ArgumentException"></exception>
    public static string TestStringWithPrefix(int length = Length)
    {
        if (length - AUTO_PREFIX.Length <= 0)
        {
            throw new ArgumentException($"{length} must be greater than {nameof(AUTO_PREFIX)} length: {AUTO_PREFIX.Length}");
        }

        return $"{AUTO_PREFIX}{GenerateString(length - AUTO_PREFIX.Length)}";
    }

    /// <summary>
    /// Generates not trimmed test string
    /// </summary>
    /// <param name="length">The length of string (not including spaces)</param>
    /// <returns>Test string with enter, tabs and white spaces from both sides</returns>
    protected static string NotTrimmedTestString(int length = Length)
    {
        return $" \n  \t \n    \t {GenerateString(length)}  \n \t   \n ";
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