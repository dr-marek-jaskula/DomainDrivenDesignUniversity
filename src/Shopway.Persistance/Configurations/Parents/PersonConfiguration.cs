using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities.Parents;
using Shopway.Domain.Enums;
using Shopway.Persistence.Constants;
using Shopway.Domain.ValueObjects;
using Shopway.Persistence.Converters;
using Shopway.Domain.EntityIds;
using Shopway.Persistence.Utilities;

namespace Shopway.Persistence.Configurations.Parents;

internal class PersonEntityTypeConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable(TableNames.Person, SchemaNames.Master);

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(id => id.Value, guid => PersonId.Create(guid))
            .HasColumnType(ColumnTypes.UniqueIdentifier);

        builder.Property(p => p.DateOfBirth)
            .HasConversion<DateOnlyConverter, DateOnlyComparer>()
            .HasColumnType(ColumnTypes.DateTimeOffset(2))
            .HasDefaultValue(null)
            .IsRequired(false);

        builder.Property(p => p.Gender)
            .HasColumnType(ColumnTypes.VarChar(7))
            .HasConversion(g => g.ToString(), s => (Gender)Enum.Parse(typeof(Gender), s))
            .IsRequired(true);

        builder.ConfigureAuditableEntity();

        builder
            .OwnsOne(p => p.FirstName, navigationBuilder =>
            {
                navigationBuilder
                    .Property(n => n.Value)
                    .HasColumnName(nameof(FirstName))
                    .IsRequired(true)
                    .HasMaxLength(FirstName.MaxLength);
            });

        builder
            .OwnsOne(p => p.LastName, navigationBuilder =>
            {
                navigationBuilder
                    .Property(n => n.Value)
                    .HasColumnName(nameof(LastName))
                    .IsRequired(true)
                    .HasMaxLength(LastName.MaxLength);
            });

        builder
            .OwnsOne(p => p.Email, navigationBuilder =>
            {
                navigationBuilder
                    .Property(n => n.Value)
                    .HasColumnName(nameof(Email))
                    .IsRequired(true)
                    .HasMaxLength(Email.MaxLength);

                navigationBuilder
                    .HasIndex(email => email.Value)
                    .HasDatabaseName($"UX_{nameof(Person)}_{nameof(Email)}")
                    //.IncludeProperties(p => new { p.FirstName, p.LastName });
                    .IsUnique(true);
            });

        builder
            .OwnsOne(p => p.PhoneNumber, navigationBuilder =>
            {
                navigationBuilder
                    .Property(n => n.Value)
                    .HasColumnName(nameof(PhoneNumber))
                    .IsRequired(true)
                    .HasMaxLength(9);
            });

        builder.OwnsOne(
            p => p.Address,
            addressNavigationBuilder =>
            {
                //Configures a different table that the entity type maps to when targeting a relational database.
                addressNavigationBuilder.ToTable(TableNames.Address);

                //Configures the relationship to the owner, and indicates the Foreign Key.
                addressNavigationBuilder
                    .WithOwner()
                    .HasForeignKey(nameof(PersonId)); //Shadow Foreign Key

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
    }
}