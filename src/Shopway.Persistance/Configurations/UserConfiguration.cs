using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Persistence.Constants;
using Shopway.Domain.ValueObjects;

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
            .HasConversion(x => x.Value, v => Username.Create(v).Value)
            .HasColumnType("VARCHAR(60)")
            .IsRequired(true);

        builder.Property(u => u.Email)
            .HasConversion(x => x.Value, v => Email.Create(v).Value)
            .IsRequired(true)
            .HasColumnType("VARCHAR(40)");

        builder.Property(u => u.CreatedOn)
            .HasColumnType("datetimeoffset(2)");

        builder.Property(u => u.PasswordHash)
            .HasConversion(x => x.Value, v => PasswordHash.Create(v).Value)
            .HasColumnType("NCHAR(514)"); //512 + 2 for 'N' characters

        builder.Property(u => u.RoleId)
            .HasColumnType("UNIQUEIDENTIFIER");

        builder.HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId);

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