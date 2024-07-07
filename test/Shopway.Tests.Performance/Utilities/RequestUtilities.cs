using System.Text;
using System.Text.Json;

namespace Shopway.Tests.Performance.Utilities;

public static class RequestUtilities
{
    public static StringContent ToStringContent<TBody>(this TBody body)
    {
        var bodyAsJson = JsonSerializer.Serialize(body);
        return new StringContent(bodyAsJson, Encoding.UTF8, Constants.Constants.Http.JsonMediaType);
    }
}
