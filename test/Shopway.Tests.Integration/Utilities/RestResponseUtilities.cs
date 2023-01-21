using Newtonsoft.Json;
using RestSharp;

namespace Shopway.Tests.Integration.Utilities;

public static class RestResponseUtilities
{
    public static TValue? Deserialize<TValue>(this RestResponse response)
    {
        if (response.Content is null)
        {
            throw new ArgumentNullException(nameof(response.Content));
        }

        return JsonConvert.DeserializeObject<TValue>(response.Content);
    }
}