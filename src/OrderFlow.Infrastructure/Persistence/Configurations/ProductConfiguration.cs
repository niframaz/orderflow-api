using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderFlow.Domain.Entities;

namespace OrderFlow.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();

            builder.OwnsOne(x => x.Price, price =>
            {
                price.Property(p => p.Amount).HasColumnName("PriceAmount").HasPrecision(18, 2).IsRequired();
                price.Property(p => p.Currency).HasColumnName("PriceCurrency").HasMaxLength(3).IsRequired();
            });

            builder.Property(x => x.StockQuantity).IsRequired();
        }
    }
}
