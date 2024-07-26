using RestSharp;
using Shopway.Tests.Integration.ControllersUnderTest;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shopway.Tests.Integration.Utilities;

public static class RestResponseUtilities
{
    private static readonly JsonSerializerOptions _serializationOptions = CreateSerializationOptions();

    private static JsonSerializerOptions CreateSerializationOptions()
    {
        JsonSerializerOptions options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
        };

        options.Converters.Add(new JsonStringEnumConverter());
        return options;
    }

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

        return JsonSerializer.Deserialize<TValue>(response.Content, _serializationOptions)!;
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

        var responseResult = JsonSerializer.Deserialize<ResponseResult<TValue>>(response.Content, _serializationOptions);

        if (responseResult is null || responseResult.IsFailure)
        {
            throw new ArgumentNullException("Response result is null or failure");
        }

        return responseResult.Value!;
    }
}
