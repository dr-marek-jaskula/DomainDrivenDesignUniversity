using WireMock;
using WireMock.ResponseBuilders;
using WireMock.ResponseProviders;
using WireMock.Settings;

namespace Shopway.Tests.Integration.Container.WireMockUtilities.ResponseProviders;

public sealed class QueueResponseProvider(Queue<IResponseBuilder> responseQueue) : IResponseProvider
{
    private readonly Queue<IResponseBuilder> _responseQueue = responseQueue;

    public async Task<(IResponseMessage Message, IMapping? Mapping)> ProvideResponseAsync
    (
        IMapping mapping,
        IRequestMessage requestMessage,
        WireMockServerSettings settings
    )
    {
        return await _responseQueue.Dequeue().ProvideResponseAsync(mapping, requestMessage, settings);
    }
}
