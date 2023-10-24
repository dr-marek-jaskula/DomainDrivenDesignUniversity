namespace Shopway.Domain.Utilities;

public static class StringUtilities
{
    private static readonly char[] _illegalCharacter = 
        { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', 
          '\'', '"', '[', ']', ';', ':', '\\', '|', '/', '.', ',', '>', '<', '?', '-', '_', '+', '+', '~', '`' };
    private static readonly char[] _toTrimCharacter = { ' ', '\n', '\t' };

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
        return input.Any(character => _illegalCharacter.Contains(character));
    }

    public static bool ContainsToTrimCharacter(this string input)
    {
        return input.Any(character => _toTrimCharacter.Contains(character));
    }

    public static bool ContainsDigit(this string input)
    {
        foreach (char character in input)
        {
            if (char.IsDigit(character))
            {
                return true;
            }
        }

        return false;
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

    public static bool NotContains(this string input, string value)
    {
        return input.Contains(value) is false;
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
}