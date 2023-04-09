using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using static System.Convert;
using static System.Text.RegularExpressions.RegexOptions;

namespace Shopway.Domain.Utilities;

public static class RandomUtilities
{
    private const string _base64NonAlpaNumbericCharactersPattern = "(?:[/+=]+)";
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

    public static string GenerateTestString()
    {
        Guid opportunityId = Guid.NewGuid();
        Guid systemUserId = Guid.NewGuid();
        string currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        StringBuilder sb = new();
        sb.AppendFormat("opportunityid={0}", opportunityId.ToString());
        sb.AppendFormat("&systemuserid={0}", systemUserId.ToString());
        sb.AppendFormat("&currenttime={0}", currentTime);

        return sb.ToString();
    }

    public static byte[] GetRandomData(int length)
    {
        return RandomNumberGenerator.GetBytes(length);
    }
}