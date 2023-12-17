using System.Buffers;
using Shopway.Domain.Common.Utilities;

namespace Shopway.Domain.Common.Utilities;

public static class StringUtilities
{
    public static readonly char[] IllegalCharacters =
        ['!', '@', '#', '$', '%', '^', '&', '*', '(', ')',
          '\'', '"', '[', ']', ';', ':', '\\', '|', '/', '.', ',', '>', '<', '?', '-', '_', '+', '+', '~', '`'];
    public static readonly char[] ToTrimCharacters = [' ', '\n', '\t'];
    public static readonly char[] Digits = [ '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' ];

    //SearchValues are highly efficient .NET 8 way to perform searches.
    private static readonly SearchValues<char> _illeagalCharacterSearchValues = SearchValues.Create(IllegalCharacters);
    private static readonly SearchValues<char> _toTrimCharacterSearchValues = SearchValues.Create(ToTrimCharacters);
    private static readonly SearchValues<char> _digitsSearchValues = SearchValues.Create(Digits);

    public static bool IsNullOrEmptyOrWhiteSpace(this string? input)
    {
        return string.IsNullOrWhiteSpace(input);
    }

    public static bool NotNullOrEmptyOrWhiteSpace(this string? input)
    {
        return string.IsNullOrWhiteSpace(input) is false;
    }

    public static bool ContainsIllegalCharacter(this string input)
    {
        return input.AsSpan().IndexOfAny(_illeagalCharacterSearchValues) is not -1;
    }

    public static bool ContainsToTrimCharacter(this string input)
    {
        return input.AsSpan().IndexOfAny(_toTrimCharacterSearchValues) is not -1;
    }

    public static bool ContainsDigit(this string input)
    {
        return input.AsSpan().IndexOfAny(_digitsSearchValues) is not -1;
    }

    public static bool Contains(this string input, params string[] strings)
    {
        foreach (string @string in strings)
        {
            if (input.Contains(@string) is false)
            {
                return false;
            }
        }

        return true;
    }

    public static bool NotContains(this string input, string value)
    {
        return input.Contains(value) is false;
    }

    public static bool NotContains(this string input, char character)
    {
        return input.Contains(character) is false;
    }

    public static bool NotContains(this string input, params string[] strings)
    {
        return input.Contains(strings) is false;
    }

    public static TEnum? ParseToNullableEnum<TEnum>(this string? input)
        where TEnum : struct, IConvertible
    {
        return Enum.TryParse<TEnum>(input, out var outPriority)
            ? outPriority
            : null;
    }

    public static TType Parse<TType>(this string input, IFormatProvider? formatProvider = null)
        where TType : IParsable<TType>
    {
        return TType.Parse(input, formatProvider);
    }

    public static bool IsLengthInRange(this string input, int lowerBound, int upperBound)
    {
        return input.Length >= lowerBound && input.Length <= upperBound;
    }

    public static bool IsLengthInRange(this string input, Range range)
    {
        return input.Length >= range.Start.Value && input.Length <= range.End.Value;
    }

    public static bool LengthNotInRange(this string input, int lowerBound, int upperBound)
    {
        return input.IsLengthInRange(lowerBound, upperBound) is false;
    }

    public static bool LengthNotInRange(this string input, Range range)
    {
        return input.IsLengthInRange(range) is false;
    }

    public static bool CaseInsensitiveEquals(this string input, string compareTo)
    {
        return string.Equals(input, compareTo, StringComparison.OrdinalIgnoreCase);
    }

    public static string RemoveAll(this string input, params string[] removeStrings)
    {
        removeStrings = removeStrings
            .Where(@string => input.Contains(@string))
            .ToArray();

        foreach (var removeString in removeStrings)
        {
            int index = input.IndexOf(removeString);
            input = input.Remove(index, removeString.Length);
        }

        return input;
    }
}