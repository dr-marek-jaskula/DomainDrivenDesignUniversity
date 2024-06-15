using Newtonsoft.Json;
using RestSharp;
using Shopway.Tests.Integration.Container.WireMockUtilities.ResponseProviders;
using System.Net;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using static Shopway.Tests.Integration.Container.Constants.Constants.Header;

namespace Shopway.Tests.Integration.Container.WireMockUtilities;

public sealed class WebServiceMock(int? port = null) : IDisposable
{
    private readonly WireMockServer _mockServer = WireMockServer.Start(port);
    private bool _disposed;

    public string? Url => _mockServer.Url;

    public WebServiceMock MockServiceResponse<TBody>(string endpoint, HttpMethod httpMethod, HttpStatusCode httpStatusCode, TBody body)
    {
        var request = Request
            .Create()
            .WithPath($"/{endpoint}")
            .UsingMethod(httpMethod.Method);

        var response = Response
            .Create()
            .WithStatusCode(httpStatusCode)
            .WithHeader(ContentTypeHeader, ContentType.Json)
            .WithBody(ConvertToJson(body)!);

        _mockServer
            .Given(request)
            .RespondWith(response);

        return this;
    }

    public WebServiceMock MockServiceResponse<TBody>(string endpoint, HttpMethod httpMethod, HttpStatusCode httpStatusCode)
    {
        return MockServiceResponse<object>(endpoint, httpMethod, httpStatusCode, new());
    }

    public WebServiceMock MockServiceResponseQueue<TBody>(string endpoint, HttpMethod httpMethod, params ResponseEntry[] responseEntries)
    {
        var queue = CreateQueue(responseEntries);
        var responseProvider = new QueueResponseProvider(queue);

        var request = Request
            .Create()
            .WithPath($"/{endpoint}")
            .UsingMethod(httpMethod.Method);

        _mockServer
            .Given(request)
            .RespondWith(responseProvider);

        return this;
    }

    private void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _mockServer.Stop();
            _mockServer.Dispose();
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private static Queue<IResponseBuilder> CreateQueue(ResponseEntry[] responseEntries)
    {
        var builders = responseEntries
            .Select(responseEntry =>
            {
                var builder = Response
                    .Create()
                    .WithStatusCode(responseEntry.HttpStatusCode)
                    .WithHeader(ContentTypeHeader, ContentType.Json);

                if (responseEntry.HasBody)
                {
                    builder.WithBody(ConvertToJson(responseEntry.Body)!);
                }

                return builder;
            });

        return new Queue<IResponseBuilder>(builders);
    }

    private static string? ConvertToJson<TBody>(TBody? body)
    {
        if (body is null)
        {
            return null;
        }

        return JsonConvert.SerializeObject(body, Formatting.Indented);
    }
}
