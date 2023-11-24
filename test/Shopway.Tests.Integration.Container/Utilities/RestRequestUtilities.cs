using RestSharp;
using Newtonsoft.Json;
using Shopway.Domain.Common.Utilities;
using static Shopway.Tests.Integration.Container.Constants.Constants.Header;

namespace Shopway.Tests.Integration.Utilities;

public static class RestRequestUtilities
{
    /// <summary>
    /// Appends the request path by query parameters (variable=value). 
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <example>request.AddQueryParameter("foo", "bar"); Results in '?foo=bar'</example>
    /// <param name="request">Request that path will be appended</param>
    /// <param name="name">Name of the variable that will be used in the query</param>
    /// <param name="values">Values of the variable used in the query</param>
    /// <param name="toString">The user way to map from value to the string</param>
    /// <returns></returns>
    public static RestRequest AddQueryParameters<TValue>(this RestRequest request, string name, IList<TValue> values, Func<TValue, string> toString)
    {
        if (values.IsNullOrEmpty()) 
        {
            return request;
        }

        foreach (var value in values)
        {
            request.AddQueryParameter(name, toString(value));
        }

        return request;
    }

    public static RestRequest AddJsonContentHeader(this RestRequest restRequest)
    {
        return restRequest
            .AddHeader(ContentTypeHeader, ContentType.Json);
    }

    public static RestRequest AddApiKeyAuthentication(this RestRequest restRequest, string apiKey)
    {
        return restRequest
            .AddHeader(ApiKeyHeader, apiKey);
    }

    /// <summary>
    /// Serialize object to the json format and add it to the request
    /// </summary>
    /// <param name="restRequest"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    public static RestRequest AddJson(this RestRequest restRequest, object body) 
    { 
        string serialized = JsonConvert.SerializeObject(body);

        return restRequest
            .AddJsonContentHeader()
            .AddJsonBody(serialized);
    }
}