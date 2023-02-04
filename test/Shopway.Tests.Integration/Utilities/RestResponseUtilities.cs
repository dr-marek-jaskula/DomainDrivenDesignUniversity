using Newtonsoft.Json;
using RestSharp;
using Shopway.Tests.Integration.ControllersUnderTest;

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

    public static TValue DeserializeResponseResult<TValue>(this RestResponse response)
    {
        if (response.Content is null)
        {
            throw new ArgumentNullException(nameof(response.Content));
        }

        var responseResult = JsonConvert.DeserializeObject<ResponseResult<TValue>>(response.Content);

        if (responseResult is null || responseResult.IsFailure)
        {
            throw new ArgumentNullException("Response result is null or failure");
        }

        return responseResult.Value!;
    }
}