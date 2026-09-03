using OrderFlow.Domain.ValueObjects;

namespace OrderFlow.Domain.Entities
{
    public class OrderItem
    {
        public Guid Id { get; private set; }
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }
        public Money UnitPrice { get; private set; } = null!;
        
        private OrderItem() { }

        internal OrderItem(Guid productId, int quantity, Money unitPrice)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            ArgumentNullException.ThrowIfNull(unitPrice);

            if (unitPrice.Amount < 0)
                throw new ArgumentException("Unit price cannot be negative.");

            Id = Guid.NewGuid();
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public decimal GetTotal()
        {
            return Quantity * UnitPrice.Amount;
        }
    }
}
