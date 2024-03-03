using Shopway.Application.Features;
using Shopway.Application.Features.Proxy;
using Shopway.Domain.Common.Results;

namespace Shopway.Application.Abstractions.CQRS;

public interface IMediatorProxyService
{
    Result<IQuery<PageResponse<DataTransferObjectResponse>>> Map(ProxyQuery proxyQuery);
}