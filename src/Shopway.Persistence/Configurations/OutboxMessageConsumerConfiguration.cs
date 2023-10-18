using Shopway.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;
using Shopway.Persistence.Constants;
using Shopway.Persistence.Converters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Shopway.Persistence.Constants.NumberConstants;

namespace Shopway.Persistence.Configurations;

internal sealed class OutboxMessageConsumerConfiguration : IEntityTypeConfiguration<OutboxMessageConsumer>
{
    public void Configure(EntityTypeBuilder<OutboxMessageConsumer> builder)
    {
        builder.ToTable(TableNames.OutboxMessageConsumer, SchemaNames.Outbox);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion<UlidToStringConverter>()
            .HasColumnType(ColumnTypes.Char(UlidCharLenght));

        builder.HasKey(outboxMessageConsumer => new
        {
            outboxMessageConsumer.Id,
            outboxMessageConsumer.Name
        });
    }
}
