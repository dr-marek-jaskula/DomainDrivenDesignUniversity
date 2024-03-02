using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.DataProcessing;

namespace Shopway.Application.Features.Proxy;

public partial class MediatorProxyService
{
    [QueryStrategy("OrderHeader", typeof(OffsetPage))]
    private static IQuery<OffsetPageResponse<DataTransferObjectResponse>> QueryOrderHeaders(ProxyQuery proxyQuery)
    {
        throw new NotImplementedException();
    }
}