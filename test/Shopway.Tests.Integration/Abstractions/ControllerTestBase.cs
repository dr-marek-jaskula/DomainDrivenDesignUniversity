using RestSharp;
using Shopway.Tests.Integration.Utilities;
using static RestSharp.Method;
using static Shopway.Domain.Utilities.RandomUtilities;

namespace Shopway.Tests.Integration.Abstractions;

public abstract partial class ControllerTestsBase
{
    private const string ShopwayApiUrl = "https://localhost:7236/api/";
    private const string APP_PREFIX = "auto_shopway";
    private const int Length = 22;
    protected readonly string _controllerUri;

    public ControllerTestsBase()
	{
        _controllerUri = GetType().Name[..^"ControllerTests".Length];
    }

    protected static string TestString(int lenght = Length)
    {
        return $"{APP_PREFIX}_test_{GenerateString(lenght)}";
    }

    protected static RestClient RestClient(string controllerUri)
	{
		return new RestClient($"{ShopwayApiUrl}{controllerUri}");
	}

	protected static RestRequest GetRequest(string endpointUri)
	{
		return new RestRequest(endpointUri, Get);
    }

    protected static RestRequest PostRequest(string endpointUri, object body)
    {
        var request = new RestRequest(endpointUri, Post);

        return request
            .AddJson(body);
    }

    protected static RestRequest PatchRequest(string endpointUri, object body)
    {
        var request = new RestRequest(endpointUri, Patch);

        return request
            .AddJson(body);
    }

    protected static RestRequest DeleteRequest(string endpointUri)
    {
        return new RestRequest(endpointUri, Delete);
    }
}