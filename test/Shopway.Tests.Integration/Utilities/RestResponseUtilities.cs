using RestSharp;
using Shopway.Tests.Integration.ControllersUnderTest;
using System.Text.Json;

namespace Shopway.Tests.Integration.Utilities;

public static class RestResponseUtilities
{
    /// <summary>
    /// Deserialize response content to given type 
    /// </summary>
    /// <typeparam name="TValue">Type used to deserialize content</typeparam>
    /// <param name="response">RestResponse</param>
    /// <returns>Deserialized response content</returns>
    /// <exception cref="ArgumentNullException">Thrown if response content is null</exception>
    public static TValue Deserialize<TValue>(this RestResponse response)
    {
        if (response.Content is null)
        {
            throw new ArgumentNullException(nameof(response.Content));
        }

        return JsonSerializer.Deserialize<TValue>(response.Content)!;
    }

    /// <summary>
    /// Deserialize response content to ResponseResult of given type. Response result is a test substitute of domain result
    /// </summary>
    /// <typeparam name="TValue">Type used to deserialize content</typeparam>
    /// <param name="response">RestResponse</param>
    /// <returns>Deserialized response content in the response result form</returns>
    /// <exception cref="ArgumentNullException">Thrown if response content is null or when response result is null or failure</exception>
    public static TValue DeserializeResponseResult<TValue>(this RestResponse response)
    {
        if (response.Content is null)
        {
            throw new ArgumentNullException(nameof(response.Content));
        }

        var responseResult = JsonSerializer.Deserialize<ResponseResult<TValue>>(response.Content);

        if (responseResult is null || responseResult.IsFailure)
        {
            throw new ArgumentNullException("Response result is null or failure");
        }

        return responseResult.Value!;
    }
}
