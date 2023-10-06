using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Persistence.Constants;
using Shopway.Persistence.Outbox;
using Shopway.Persistence.Converters;
using static Shopway.Persistence.Constants.NumberConstants;

namespace Shopway.Persistence.Configurations;

internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable(TableNames.OutboxMessage, SchemaNames.Outbox);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion<UlidToStringConverter>()
            .HasColumnType(ColumnTypes.Char(UlidCharLenght));

        builder.Property(x => x.Type)
            .HasColumnType(ColumnTypes.VarChar(100));

        builder.Property(x => x.Content)
            .HasColumnType(ColumnTypes.VarChar(5000));

        builder.Property(x => x.Error)
            .HasColumnType(ColumnTypes.VarChar(8000));
    }
}
