using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopway.Infrastructure.Outbox;
using Shopway.Persistence.Converters;
using static Shopway.Persistence.Constants.Constants;
using static Shopway.Persistence.Constants.Constants.Number;

namespace Shopway.Persistence.Configurations;

internal sealed class OutboxMessageConsumerConfiguration : IEntityTypeConfiguration<OutboxMessageConsumer>
{
    public void Configure(EntityTypeBuilder<OutboxMessageConsumer> builder)
    {
        builder.ToTable(TableName.OutboxMessageConsumer, SchemaName.Outbox);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion<UlidToStringConverter>()
            .HasColumnType(ColumnType.Char(UlidCharLenght));

        builder.HasKey(outboxMessageConsumer => new
        {
            outboxMessageConsumer.Id,
            outboxMessageConsumer.Name
        });
    }
}
