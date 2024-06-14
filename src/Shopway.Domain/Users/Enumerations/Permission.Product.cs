using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Products;
using static Shopway.Domain.Users.Enumerations.PermissionType;

namespace Shopway.Domain.Users.Enumerations;

partial class Permission : Enumeration<Permission>
{
    private static Permission CreateReviewPermission(int id, string name, PermissionType? permissionType = null, string[]? properties = null)
    {
        return new Permission(id, name)
        {
            Properties = properties,
            RelatedAggregateRoot = typeof(Product),
            RelatedEntity = typeof(Review),
            Type = permissionType
        };
    }

    private static Permission CreateProductPermission(int id, string name, PermissionType? permissionType = null, string[]? properties = null)
    {
        return new Permission(id, name)
        {
            Properties = properties,
            RelatedAggregateRoot = typeof(Product),
            Type = permissionType
        };
    }

    public static readonly Permission Review_Add = CreateReviewPermission(2, nameof(Review_Add), Add);
    public static readonly Permission Review_Update = CreateReviewPermission(3, nameof(Review_Update), Update);
    public static readonly Permission Review_Remove = CreateReviewPermission(4, nameof(Review_Remove), Remove);
    public static readonly Permission Review_Read = CreateReviewPermission(5, nameof(Review_Read), Read); //Only for demo purposes
    
    public static readonly Permission Product_Read = CreateProductPermission(6, nameof(Product_Read), Read);
    public static readonly Permission Product_Read_Customer = CreateProductPermission(7, nameof(Product_Read_Customer), Read, [.. Product_Read_Customer_Properties, .. Review_Read_Customer_Properties]);

    //To demonstrate how to reuse accesses without hierarchy structure
    private static string[] Review_Read_Customer_Properties =>
    [
        $"{nameof(Product.Reviews)}.{nameof(Review.Id)}",
        $"{nameof(Product.Reviews)}.{nameof(Review.Description)}",
        $"{nameof(Product.Reviews)}.{nameof(Review.Title)}",
        $"{nameof(Product.Reviews)}.{nameof(Review.Username)}",
        $"{nameof(Product.Reviews)}.{nameof(Review.Stars)}",
        $"{nameof(Product.Reviews)}.{nameof(Review.CreatedOn)}",
        $"{nameof(Product.Reviews)}.{nameof(Review.CreatedBy)}",
        $"{nameof(Product.Reviews)}.{nameof(Review.UpdatedOn)}",
        $"{nameof(Product.Reviews)}.{nameof(Review.UpdatedBy)}"
    ];

    private static string[] Product_Read_Customer_Properties =>
    [
        nameof(Product.Id),
        nameof(Product.ProductName),
        nameof(Product.Price),
        nameof(Product.Revision),
    ];
}
