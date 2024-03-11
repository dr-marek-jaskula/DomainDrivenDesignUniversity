using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopway.Infrastructure.Outbox;
using Shopway.Persistence.Converters;
using Shopway.Persistence.Converters.Enums;
using Shopway.Persistence.Outbox;
using static Shopway.Domain.Common.Utilities.EnumUtilities;
using static Shopway.Persistence.Constants.Constants;
using static Shopway.Persistence.Constants.Constants.Number;

namespace Shopway.Persistence.Configurations;

internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable(TableName.OutboxMessage, SchemaName.Outbox);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion<UlidToStringConverter>()
            .HasColumnType(ColumnType.Char(UlidCharLenght));

        builder.Property(x => x.Type)
            .HasColumnType(ColumnType.VarChar(100));

        builder.Property(x => x.Content)
            .HasColumnType(ColumnType.VarChar(5000));

        builder.Property(x => x.Error)
            .HasColumnType(ColumnType.VarChar(8000));

        builder.Property(p => p.ExecutionStatus)
            .HasConversion<ExecutionStatusConverter>()
            .HasColumnType(ColumnType.VarChar(LongestOf<ExecutionStatus>()))
            .HasDefaultValue(ExecutionStatus.InProgress)
            .IsRequired(true);

        builder
            .HasIndex(x => x.ExecutionStatus)
            .HasDatabaseName($"IX_{nameof(OutboxMessage)}_{nameof(OutboxMessage.ExecutionStatus)}")
            .HasFilter("[ExecutionStatus] = 'InProgress'");
    }
}
