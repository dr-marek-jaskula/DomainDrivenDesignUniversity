using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Persistence.Constants;
using Shopway.Domain.ValueObjects;
using Shopway.Persistence.Utilities;
using Shopway.Persistence.Converters.EntityIds;

namespace Shopway.Persistence.Configurations;

internal sealed class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(TableNames.User, SchemaNames.Master);

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasConversion<UserIdConverter, EntityIdComparer>()
            .HasColumnType(ColumnTypes.UniqueIdentifier);

        builder.ConfigureAuditableEntity();

        builder
            .OwnsOne(p => p.Username, navigationBuilder =>
            {
                navigationBuilder
                    .Property(n => n.Value)
                    .HasColumnName(nameof(Username))
                    .IsRequired(true)
                    .HasMaxLength(Username.MaxLength);

                navigationBuilder
                    .HasIndex(username => username.Value)
                    .HasDatabaseName($"UX_{nameof(Username)}_{nameof(Email)}")
                    //.IncludeProperties(p => p.Email)
                    .IsUnique(true);
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
                    .HasDatabaseName($"UX_{nameof(User)}_{nameof(Email)}")
                    .IsUnique(true);
            });

        builder
            .OwnsOne(p => p.PasswordHash, navigationBuilder =>
            {
                navigationBuilder
                    .Property(n => n.Value)
                    .HasColumnName(nameof(PasswordHash))
                    .HasColumnType(ColumnTypes.NChar(PasswordHash.BytesLong))
                    .IsRequired(true);
            });

        builder.HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity<RoleUser>();

        builder.HasMany(u => u.OrderHeaders)
            .WithOne(o => o.User)
            .HasForeignKey(u => u.UserId);

        builder.HasOne(u => u.Customer)
            .WithOne(c => c.User)
            .HasForeignKey<User>(u => u.CustomerId);
    }
}