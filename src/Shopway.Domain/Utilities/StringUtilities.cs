namespace Shopway.Domain.Utilities;

public static class StringUtilities
{
    private static readonly char[] _illegalCharacter = 
        { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', 
          '\'', '"', '[', ']', ';', ':', '\\', '|', '/', '.', ',', '>', '<', '?', '-', '_', '+', '+', '~', '`' };
    private static readonly char[] _digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
    private static readonly char[] _toTrimCharacter = { ' ', '\n', '\t' };

    public static bool IsNullOrEmptyOrWhiteSpace(this string? input)
    {
        return string.IsNullOrWhiteSpace(input);
    }

    public static bool IsNotNullOrEmptyOrWhiteSpace(this string? input)
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
        return input.Any(character => _digits.Contains(character));
    }

    public static TEnum? ParseToNullableEnum<TEnum>(this string? input)
        where TEnum : struct, IConvertible
    {
        return Enum.TryParse<TEnum>(input, out var outPriority) ? outPriority : null;
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
}