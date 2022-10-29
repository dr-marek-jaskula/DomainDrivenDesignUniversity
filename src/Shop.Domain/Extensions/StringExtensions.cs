namespace Shopway.Domain.Extensions;

public static class StringExtensions
{
    private static readonly char[] _illegalCharacter = 
        { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', 
          '\'', '"', '[', ']', ';', ':', '\\', '|', '/', '.', ',', '>', '<', '?', '-', '_', '+', '+', '~', '`' };
    private static readonly char[] _digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

    public static bool IsNullOrEmptyOrWhiteSpace(this string input)
    {
        return string.IsNullOrWhiteSpace(input);
    }

    public static bool ContainsIllegalCharacter(this string input)
    {
        return input.Any(character => _illegalCharacter.Contains(character));
    }

    public static bool ContainsDigit(this string input)
    {
        return input.Any(character => _digits.Contains(character));
    }
}