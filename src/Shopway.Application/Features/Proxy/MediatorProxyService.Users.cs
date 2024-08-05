using Shopway.Application.Features.Proxy.GenericPageQuery;
using Shopway.Application.Features.Proxy.GenericQuery;
using Shopway.Application.Features.Proxy.GenericQuery.QueryById;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Users;

namespace Shopway.Application.Features.Proxy;

public partial class MediatorProxyService
{
    [GenericPageQueryStrategy(nameof(User), nameof(OffsetPage))]
    private static GenericOffsetPageQuery<User, UserId> GenericQueryUsersUsingOffsetPage(GenericProxyPageQuery proxyQuery)
        => GenericOffsetPageQuery<User, UserId>.From(proxyQuery);

    [GenericPageQueryStrategy(nameof(User), nameof(CursorPage))]
    private static GenericCursorPageQuery<User, UserId> GenericQueryUsersUsingCursorPage(GenericProxyPageQuery proxyQuery)
        => GenericCursorPageQuery<User, UserId>.From(proxyQuery);

    [GenericByIdQueryStrategy(nameof(User))]
    private static GenericByIdQuery<User, UserId> GenericQueryUserById(GenericProxyByIdQuery proxyQuery)
        => GenericByIdQuery<User, UserId>.From(proxyQuery);
}
