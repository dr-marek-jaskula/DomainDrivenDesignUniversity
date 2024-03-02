using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Proxy;

public partial class MediatorProxyService
{
    [QueryStrategy("OrderHeader")]
    private static IQuery<OffsetPageResponse<DataTransferObjectResponse>> QueryOrderHeaders(ProxyQuery proxyQuery)
    {
        throw new NotImplementedException();
    }
}