using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities.Parents;
using Shopway.Domain.Enums;

namespace Shopway.Persistence.Configurations.Parents;

public class PersonEntityTypeConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("Person");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnType("SMALLINT").UseIdentityColumn();

        builder.Property(c => c.FirstName)
            .IsRequired(true)
            .HasMaxLength(50);

        builder.Property(c => c.LastName)
            .IsRequired(true)
            .HasMaxLength(50);

        builder.Property(c => c.Email)
            .IsRequired(true)
            .HasMaxLength(50);

        builder.Property(c => c.ContactNumber)
            .IsRequired(true)
            .HasColumnType("VARCHAR(30)");

        builder.Property(c => c.DateOfBirth)
            .HasConversion<DateOnlyConverter, DateOnlyComparer>()
            .HasColumnType("DATE")
            .HasDefaultValue(null);

        builder.Property(c => c.Gender)
            .IsRequired(true)
            .HasColumnType("VARCHAR(7)")
            .HasConversion(g => g.ToString(),
            s => (Gender)Enum.Parse(typeof(Gender), s))
            .HasComment("Male, Female or Unknown");

        builder.HasOne(c => c.Address)
            .WithOne(a => a.Person)
            .HasForeignKey<Person>(c => c.AddressId);

        //Indexes
        builder.HasIndex(p => p.Email, "UX_Person_Email")
            .IsUnique()
            .IncludeProperties(p => new { p.FirstName, p.LastName });

        builder.HasIndex(p => p.Email, "IX_Person_Email")
            .IsUnique();
    }
}