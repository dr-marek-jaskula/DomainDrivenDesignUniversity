using Shopway.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;
using Shopway.Persistence.Converters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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

        builder
            .HasIndex(x => x.ProcessedOn)
            .HasDatabaseName($"IX_{nameof(OutboxMessage)}_{nameof(OutboxMessage.ProcessedOn)}")
            .HasFilter("[ProcessedOn] IS NULL");
    }
}
