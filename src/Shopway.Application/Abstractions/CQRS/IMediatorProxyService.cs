using Shopway.Application.Features;
using Shopway.Application.Features.Proxy;
using Shopway.Application.Features.Proxy.GenericQuery.QueryByKey;
using Shopway.Application.Features.Proxy.PageQuery;
using Shopway.Application.Features.Proxy.Query;
using Shopway.Domain.Common.Results;

namespace Shopway.Application.Abstractions.CQRS;

public interface IMediatorProxyService
{
    Result<IQuery<DataTransferObjectResponse>> GenericMap(GenericProxyByIdQuery proxyQuery);
    Result<IQuery<DataTransferObjectResponse>> GenericMap(GenericProxyByKeyQuery proxyQuery);
    Result<IQuery<PageResponse<DataTransferObjectResponse>>> GenericMap(GenericProxyPageQuery proxyQuery);
    Result<IQuery<PageResponse<DataTransferObjectResponse>>> Map(ProxyPageQuery proxyQuery);
    Result<IQuery<DataTransferObjectResponse>> Map(ProxyQuery proxyQuery);
}
