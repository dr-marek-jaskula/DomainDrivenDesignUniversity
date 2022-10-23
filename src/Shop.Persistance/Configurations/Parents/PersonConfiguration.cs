using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities.Parents;
using Shopway.Domain.Enums;
using Shopway.Persistence.Constants;
using Shopway.Domain.ValueObjects;
using Shopway.Persistence.Converters;

namespace Shopway.Persistence.Configurations.Parents;

internal class PersonEntityTypeConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable(TableNames.Person);

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasColumnType("UNIQUEIDENTIFIER");

        builder.Property(p => p.FirstName)
            .HasConversion(x => x.Value, v => FirstName.Create(v).Value)
            .HasMaxLength(FirstName.MaxLength);

        builder.Property(p => p.LastName)
            .HasConversion(x => x.Value, v => LastName.Create(v).Value)
            .HasMaxLength(LastName.MaxLength);

        builder.Property(p => p.Email)
            .HasConversion(x => x.Value, v => Email.Create(v).Value)
            .HasMaxLength(Email.MaxLength);

        builder.Property(p => p.ContactNumber)
            .HasConversion(x => x.Value, v => PhoneNumber.Create(v).Value)
            .HasMaxLength(9);

        builder.Property(p => p.DateOfBirth)
            .HasConversion<DateOnlyConverter, DateOnlyComparer>()
            .HasColumnType("DATE")
            .HasDefaultValue(null);

        builder.Property(p => p.Gender)
            .IsRequired(true)
            .HasColumnType("VARCHAR(7)")
            .HasConversion(g => g.ToString(),
            s => (Gender)Enum.Parse(typeof(Gender), s))
            .HasComment("Male, Female or Unknown");

        builder.OwnsOne(
            p => p.Address,
            addressNavigationBuilder =>
            {
                //Configures a different table that the entity type maps to when targeting a relational database.
                addressNavigationBuilder.ToTable("Address");

                //Configures the relationship to the owner, and indicates the Foreign Key.
                addressNavigationBuilder
                    .WithOwner()
                    .HasForeignKey("PersonId"); //Shadow Foreign Key

                //Configure a property of the owned entity type, in this case the to be used as Primary Key
                addressNavigationBuilder
                    .Property<Guid>("Id"); //Shadow property

                //Sets the properties that make up the primary key for this owned entity type.
                addressNavigationBuilder
                    .HasKey("Id"); //Shadow Primary Key

                addressNavigationBuilder
                    .Property(p => p.Street)
                    .HasMaxLength(100);

                addressNavigationBuilder
                    .Property(p => p.Building)
                    .HasColumnType("TINYINT");

                addressNavigationBuilder
                    .Property(p => p.Flat)
                    .HasColumnType("TINYINT");

                addressNavigationBuilder
                    .Property(p => p.Country)
                    .HasMaxLength(100);

                addressNavigationBuilder
                    .Property(p => p.City)
                    .HasMaxLength(100);

                addressNavigationBuilder
                    .Property(p => p.ZipCode)
                    .HasMaxLength(5);
            });

        //Indexes
        builder.HasIndex(p => p.Email, "UX_Person_Email")
            .IsUnique()
            .IncludeProperties(p => new { p.FirstName, p.LastName });

        builder.HasIndex(p => p.Email, "IX_Person_Email")
            .IsUnique();
    }
}