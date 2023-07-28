using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Persistence.Constants;
using Shopway.Domain.ValueObjects;
using Shopway.Persistence.Utilities;
using Shopway.Persistence.Converters.EntityIds;
using Shopway.Persistence.Converters.ValueObjects;
using static Shopway.Persistence.Constants.NumberConstants;

namespace Shopway.Persistence.Configurations;

internal sealed class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(TableNames.User, SchemaNames.Master);

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasConversion<UserIdConverter, UserIdComparer>()
            .HasColumnType(ColumnTypes.Char(UlidCharLenght));

        builder.ConfigureAuditableEntity();

        builder.Property(u => u.Username)
            .HasConversion<UsernameConverter, UsernameComparer>()
            .HasColumnName(nameof(Username))
            .HasMaxLength(Username.MaxLength)
            .IsRequired(true);

        builder.Property(u => u.Email)
            .HasConversion<EmailConverter, EmailComparer>()
            .HasColumnName(nameof(Email))
            .HasMaxLength(Email.MaxLength)
            .IsRequired(true);

        builder.Property(u => u.PasswordHash)
            .HasConversion<PasswordHashConverter, PasswordHashComparer>()
            .HasColumnName(nameof(PasswordHash))
            .HasColumnType(ColumnTypes.NChar(PasswordHash.BytesLong))
            .IsRequired(true);

        builder.HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity<RoleUser>();

        builder.HasMany(u => u.OrderHeaders)
            .WithOne(o => o.User)
            .HasForeignKey(u => u.UserId);

        builder.HasOne(u => u.Customer)
            .WithOne(c => c.User)
            .HasForeignKey<User>(u => u.CustomerId);

        //Indexes
        builder
            .HasIndex(user => user.Username)
            .HasDatabaseName($"UX_{nameof(Username)}_{nameof(Email)}")
            .IncludeProperties(user => user.Email)
            .IsUnique(true);

        builder
            .HasIndex(user => user.Email)
            .HasDatabaseName($"UX_{nameof(User)}_{nameof(Email)}")
            .IsUnique(true);
    }
}