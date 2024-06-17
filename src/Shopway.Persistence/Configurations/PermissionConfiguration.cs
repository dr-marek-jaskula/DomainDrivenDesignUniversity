using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopway.Domain.Products;
using Shopway.Domain.Users.Authorization;
using Shopway.Persistence.Converters.Enums;
using static Shopway.Domain.Common.Utilities.EnumUtilities;
using static Shopway.Persistence.Constants.Constants;

namespace Shopway.Persistence.Configurations;

internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable(TableName.Permission, SchemaName.Master);

        builder.HasKey(p => p.Name);

        builder.Property(p => p.Name)
            .HasColumnType(ColumnType.VarChar(128));

        builder.Property(p => p.RelatedAggregateRoot)
            .HasColumnType(ColumnType.VarChar(128))
            .IsRequired(false);

        builder.Property(p => p.RelatedEntity)
            .HasColumnType(ColumnType.VarChar(128))
            .IsRequired(false);

        builder.Property(p => p.Type)
            .HasConversion<PermissionTypeConverter>()
            .HasColumnType(ColumnType.VarChar(LongestOf<PermissionType>()))
            .IsRequired(true);

        builder.Property(p => p.Properties);

        builder.HasData(PredefinedPermissions());
    }

    //For tutorial purposes
    private static List<Permission> PredefinedPermissions()
    {
        List<string> customerAllowedProperties = [
            "Id",
            "Price",
            "Revision",
            "ProductName",
            "Reviews.Id",
            "Reviews.Description",
            "Reviews.Title",
            "Reviews.Username",
            "Reviews.Stars",
            "Reviews.CreatedOn",
            "Reviews.CreatedBy",
            "Reviews.UpdatedOn",
            "Reviews.UpdatedBy"
        ];

        return
        [
            Permission.CreatePermission<Product, Review>(nameof(PermissionName.Review_Add), PermissionType.Add).Value,
            Permission.CreatePermission<Product, Review>(nameof(PermissionName.Review_Update), PermissionType.Update).Value,
            Permission.CreatePermission<Product, Review>(nameof(PermissionName.Review_Remove), PermissionType.Remove).Value,
            Permission.CreatePermission<Product, Review>(nameof(PermissionName.Review_Read), PermissionType.Read).Value,
            Permission.CreatePermission<Product, Product>(nameof(PermissionName.Product_Read), PermissionType.Read).Value,
            Permission.CreatePermission<Product, Product>(nameof(PermissionName.Product_Read_Customer), PermissionType.Read, customerAllowedProperties).Value
        ];
    }
}
