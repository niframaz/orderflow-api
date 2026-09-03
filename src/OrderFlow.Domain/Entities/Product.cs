using OrderFlow.Domain.ValueObjects;

namespace OrderFlow.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public Money Price { get; private set; } = null!;
        public int StockQuantity { get; private set; }

        private Product() { }

        public Product(string name, Money price, int stockQuantity)
        {
            if (string.IsNullOrWhiteSpace(name)) 
                throw new ArgumentException("Product name is required.");

            ArgumentNullException.ThrowIfNull(price);

            if (price.Amount < 0)
                throw new ArgumentException("Product price cannot be negative.");

            if (stockQuantity < 0)
                throw new ArgumentException("Stock quantity cannot be negative.");

            Id = Guid.NewGuid();
            Name = name;
            Price = price;
            StockQuantity = stockQuantity;
        }

        public void ChangePrice(Money newPrice)
        {
            ArgumentNullException.ThrowIfNull(newPrice);

            if (newPrice.Amount < 0)
                throw new ArgumentException("Product price cannot be negative.");

            Price = newPrice;
        }
    }
}
