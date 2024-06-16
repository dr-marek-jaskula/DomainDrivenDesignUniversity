using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Products;
using static Shopway.Domain.Users.Authorization.PermissionType;

namespace Shopway.Domain.Users.Authorization;

partial class Permission : Enumeration<Permission>
{
    private static Permission CreateReviewPermission(int id, string name, PermissionType permissionType)
    {
        return new Permission(id, name)
        {
            RelatedAggregateRoot = nameof(Product),
            RelatedEntity = nameof(Review),
            Type = permissionType
        };
    }

    private static Permission CreateProductPermission(int id, string name, PermissionType permissionType)
    {
        return new Permission(id, name)
        {
            RelatedAggregateRoot = nameof(Product),
            RelatedEntity = nameof(Product),
            Type = permissionType
        };
    }

    //These permissions should be stored in separate package (with their Properties), or be used as enumerations (so dummy entities with extended data, just with basic info)
    public static readonly Permission Review_Add = CreateReviewPermission(2, nameof(Review_Add), Add);
    public static readonly Permission Review_Update = CreateReviewPermission(3, nameof(Review_Update), Update);
    public static readonly Permission Review_Remove = CreateReviewPermission(4, nameof(Review_Remove), Remove);
    public static readonly Permission Review_Read = CreateReviewPermission(5, nameof(Review_Read), Read);

    public static readonly Permission Product_Read = CreateProductPermission(6, nameof(Product_Read), Read);
    public static readonly Permission Product_Read_Customer = CreateProductPermission(7, nameof(Product_Read_Customer), Read);
}
