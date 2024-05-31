using Shopway.Application.Features;
using Shopway.Application.Features.Proxy;
using Shopway.Application.Features.Proxy.PageQuery;
using Shopway.Application.Features.Proxy.Query;
using Shopway.Domain.Common.Results;

namespace Shopway.Application.Abstractions.CQRS;

public interface IMediatorProxyService
{
    Result<IQuery<DataTransferObjectResponse>> GenericMap(GenericProxyQuery proxyQuery);
    Result<IQuery<PageResponse<DataTransferObjectResponse>>> GenericMap(GenericProxyPageQuery proxyQuery);
    Result<IQuery<PageResponse<DataTransferObjectResponse>>> Map(ProxyPageQuery proxyQuery);
    Result<IQuery<DataTransferObjectResponse>> Map(ProxyQuery proxyQuery);
}
