using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers;
using Shopway.Domain.Utilities;

namespace Shopway.Tests.Integration.Utilities;

public static class RestRequestUtilities
{
    private const string Content_Type = "content-type";

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
            .AddHeader(Content_Type, ContentType.Json);
    }

    public static RestRequest AddJson(this RestRequest restRequest, object body) 
    { 
        string serialized = JsonConvert.SerializeObject(body);

        return restRequest
            .AddJsonContentHeader()
            .AddJsonBody(serialized);
    }
}