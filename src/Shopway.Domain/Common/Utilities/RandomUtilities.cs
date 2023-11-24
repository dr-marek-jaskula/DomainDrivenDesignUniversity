using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using static System.Convert;
using static System.Text.RegularExpressions.RegexOptions;

namespace Shopway.Domain.Common.Utilities;

public static class RandomUtilities
{
    //To exclude more characters, adjust this regex
    private const string _base64NonAlpaNumbericCharactersPattern = "(?:[/+=0-9]+)";
    private static readonly Regex _base64NonAlphaNumbericCharacterRegex = new(_base64NonAlpaNumbericCharactersPattern, Compiled | CultureInvariant | Singleline);

    public static string GenerateString(int length)
    {
        using var generator = RandomNumberGenerator.Create();
        var builder = new StringBuilder();
        Append(builder, generator, length);

        while (builder.Length < length)
        {
            Append(builder, generator, length - builder.Length);
        }

        return builder
            .ToString()
            .Substring(0, length);

        static void Append(StringBuilder builder, RandomNumberGenerator generator, int length)
        {
            var bytes = new byte[length];
            generator.GetBytes(bytes);
            string base64 = ToBase64String(bytes);
            string stripped = _base64NonAlphaNumbericCharacterRegex.Replace(base64, string.Empty);
            builder.Append(stripped);
        }
    }

    public static IEnumerable<string> GenerateStrings(int length, int count)
    {
        for (int index = 0; index < count; index++)
        {
            yield return GenerateString(length);
        }
    }

    public static byte[] GetRandomByteArray(int length)
    {
        return RandomNumberGenerator.GetBytes(length);
    }
}