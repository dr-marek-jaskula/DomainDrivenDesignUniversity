using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Persistence.Constants;
using Shopway.Persistence.Converters;

namespace Shopway.Persistence.Configurations;

internal sealed class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(TableNames.User);

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .HasColumnType("UNIQUEIDENTIFIER");

        builder.Property(u => u.Username)
            .HasColumnType("VARCHAR(60)")
            .IsRequired(true);

        builder.Property(u => u.Email)
            .IsRequired(true)
            .HasColumnType("VARCHAR(40)");

        builder.Property(u => u.CreatedOn)
            .HasConversion<DateOnlyConverter, DateOnlyComparer>()
            .HasDefaultValueSql("getutcdate()") //need to use HasDefaultValueSql with "getutcdate" because it need to be the sql command
            .HasColumnType("DATE");

        builder.Property(u => u.PasswordHash)
            .HasColumnType("NCHAR(514)"); //512 + 2 for 'N' characters

        builder.Property(u => u.RoleId)
            .HasColumnType("TINYINT");

        builder.HasOne(u => u.Person)
            .WithOne(c => c.User)
            .HasForeignKey<User>(u => u.PersonId);

        //Indexes
        builder.HasIndex(u => u.Username, "IX_User_Username")
            .IsUnique()
            .IncludeProperties(o => o.Email);

        builder.HasIndex(u => u.Email, "IX_User_Email")
            .IsUnique();
    }
}