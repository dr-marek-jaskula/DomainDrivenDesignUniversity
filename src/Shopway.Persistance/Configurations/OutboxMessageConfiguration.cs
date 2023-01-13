using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Persistence.Constants;
using Shopway.Persistence.Outbox;

namespace Shopway.Persistence.Configurations;

internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable(TableNames.OutboxMessage, SchemaNames.Outbox);

        builder.HasKey(x => x.Id);
    }
}
