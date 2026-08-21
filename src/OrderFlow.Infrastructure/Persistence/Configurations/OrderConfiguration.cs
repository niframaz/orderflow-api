using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderFlow.Domain.Entities;

namespace OrderFlow.Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CustomerId)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired();

            builder.HasMany(x => x.Items)
                .WithOne()
                .HasForeignKey("OrderId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(x => x.Items)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.OwnsOne(
                x => x.ShippingAddress,
                address =>
                {
                    address.Property(x => x.Street)
                        .HasMaxLength(200)
                        .IsRequired();

                    address.Property(x => x.City)
                        .HasMaxLength(100)
                        .IsRequired();

                    address.Property(x => x.PostalCode)
                        .HasMaxLength(20)
                        .IsRequired();

                    address.Property(x => x.Country)
                        .HasMaxLength(100)
                        .IsRequired();
                });
        }
    }
}
