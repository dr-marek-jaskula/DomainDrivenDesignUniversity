using Shopway.Application.Features.Users.Queries.GetPermissionDetails;
using Shopway.Domain.Users.Authorization;

namespace Shopway.Application.Mappings;

public static class PermissionMapping
{
    public static PermissionDetailsResponse ToDetailedResponse(this Permission permission)
    {
        return new PermissionDetailsResponse
        (
            permission.Name,
            permission.RelatedAggregateRoot,
            permission.RelatedEntity,
            permission.Type.ToString(),
            permission.Properties
        );
    }
}
