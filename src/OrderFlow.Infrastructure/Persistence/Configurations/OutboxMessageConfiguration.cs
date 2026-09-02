using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderFlow.Infrastructure.Persistence.Outbox;

namespace OrderFlow.Infrastructure.Persistence.Configurations
{
    public sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.ToTable("OutboxMessages");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.EventType)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.Payload)
                .IsRequired();

            builder.Property(x => x.OccuredOn)
                .IsRequired();

            builder.Property(x => x.ProcessedOn);

            builder.Property(x => x.Error)
                .HasMaxLength(4000);
        }
    }
}
