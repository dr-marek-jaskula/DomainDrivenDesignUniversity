using Shopway.Application.Features;

namespace Shopway.Application.Abstractions.CQRS;

public interface IMediatorProxyService
{
    IQuery<PageResponse<DataTransferObjectResponse>> Map(ProxyQuery proxyQuery);
}