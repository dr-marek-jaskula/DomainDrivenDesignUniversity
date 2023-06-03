using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Shopway.Domain.Enums;
using Shopway.Domain.ValueObjects;
using Shopway.Persistence.Constants;
using Shopway.Persistence.Converters;
using Shopway.Persistence.Converters.EntityIds;
using Shopway.Persistence.Converters.Enums;
using Shopway.Persistence.Converters.ValueObjects;
using static Shopway.Domain.Utilities.EnumUtilities;
using static Shopway.Persistence.Constants.NumberConstants;
using static Shopway.Persistence.Utilities.ConfigurationUtilities;

namespace Shopway.Persistence.Configurations;

internal sealed class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable(TableNames.Customer, SchemaNames.Master);

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion<CustomerIdConverter, CustomerIdComparer>()
            .HasColumnType(ColumnTypes.UniqueIdentifier);

        builder.Property(p => p.UserId)
            .HasConversion<UserIdConverter, UserIdComparer>()
            .HasColumnType(ColumnTypes.UniqueIdentifier);

        builder.Property(p => p.DateOfBirth)
            .HasConversion<DateOnlyConverter, DateOnlyComparer>()
            .HasColumnType(ColumnTypes.DateTimeOffset(2))
            .HasDefaultValue(null)
            .IsRequired(false);

        builder.Property(p => p.Gender)
            .HasConversion<GenderConverter>()
            .HasColumnName(nameof(Gender))
            .HasColumnType(ColumnTypes.VarChar(LongestOf<Gender>()))
            .IsRequired(true);

        builder.Property(c => c.Rank)
            .HasConversion<RankConverter>()
            .HasColumnName(nameof(Rank))
            .HasDefaultValue(Rank.Standard)
            .HasColumnType(ColumnTypes.VarChar(LongestOf<Rank>()));

        builder.ConfigureAuditableEntity();

        builder.Property(p => p.FirstName)
            .HasConversion<FirstNameConverter, FirstNameComparer>()
            .HasColumnName(nameof(FirstName))
            .HasMaxLength(FirstName.MaxLength)
            .IsRequired(true);

        builder.Property(p => p.LastName)
            .HasConversion<LastNameConverter, LastNameComparer>()
            .HasColumnName(nameof(LastName))
            .IsRequired(true)
            .HasMaxLength(LastName.MaxLength);

        builder.Property(p => p.PhoneNumber)
            .HasConversion<PhoneNumberConverter, PhoneNumberComparer>()
            .HasColumnName(nameof(PhoneNumber))
            .IsRequired(true)
            .HasMaxLength(PhoneNumberMaxLenght);
            
        builder.OwnsOne(
            p => p.Address,
            addressNavigationBuilder =>
            {
                //Configures a different table that the entity type maps to when targeting a relational database.
                addressNavigationBuilder.ToTable(TableNames.Address, SchemaNames.Master);

                //Configures the relationship to the owner, and indicates the Foreign Key.
                addressNavigationBuilder
                    .WithOwner()
                    .HasForeignKey(nameof(CustomerId)); //Shadow Foreign Key

                //Configure a property of the owned entity type, in this case the to be used as Primary Key
                addressNavigationBuilder
                    .Property<Guid>(ShadowColumnNames.Id); //Shadow property

                //Sets the properties that make up the primary key for this owned entity type.
                addressNavigationBuilder
                    .HasKey(ShadowColumnNames.Id); //Shadow Primary Key

                addressNavigationBuilder
                    .Property(p => p.Country)
                    .IsRequired(true)
                    .HasMaxLength(100);

                addressNavigationBuilder
                    .Property(p => p.City)
                    .IsRequired(true)
                    .HasMaxLength(100);

                addressNavigationBuilder
                    .Property(p => p.Street)
                    .IsRequired(true)
                    .HasMaxLength(100);

                addressNavigationBuilder
                    .Property(p => p.ZipCode)
                    .IsRequired(true)
                    .HasMaxLength(5);

                addressNavigationBuilder
                    .Property(p => p.Building)
                    .IsRequired(true)
                    .HasMaxLength(Address.MaxBuildingNumber);

                addressNavigationBuilder
                    .Property(p => p.Flat)
                    .IsRequired(false)
                    .HasMaxLength(Address.MaxFlatNumber);
            });

        builder.HasOne(c => c.User)
            .WithOne(u => u.Customer)
            .HasForeignKey<Customer>(c => c.UserId);
    }
}
