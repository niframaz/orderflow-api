namespace OrderFlow.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = default!;
        public decimal Price { get; private set; }
        public int StockQuantity { get; private set; }

        private Product() { }

        public Product(string name, decimal price, int stockQuantity)
        {
            if (string.IsNullOrWhiteSpace(name)) 
                throw new ArgumentException("Product name is required.");

            if (price < 0)
                throw new ArgumentException("Product price cannot be negative.");

            if (stockQuantity < 0)
                throw new ArgumentException("Stock quantity cannot be negative.");

            Id = Guid.NewGuid();
            Name = name;
            Price = price;
            StockQuantity = stockQuantity;
        }

        public void ChangePrice(decimal newPrice)
        {
            if (newPrice < 0)
                throw new ArgumentException("Product price cannot be negative.");

            Price = newPrice;
        }
    }
}
