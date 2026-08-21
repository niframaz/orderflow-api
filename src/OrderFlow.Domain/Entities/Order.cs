using OrderFlow.Domain.Common;
using OrderFlow.Domain.Enums;
using OrderFlow.Domain.Events;
using OrderFlow.Domain.ValueObjects;

namespace OrderFlow.Domain.Entities
{
    public class Order : Entity
    {
        private readonly List<OrderItem> _items = [];
        public Guid Id { get; private set; }
        public Guid CustomerId { get; private set; }
        public ShippingAddress ShippingAddress { get; private set; } = default!;
        public DateTime CreatedAt { get; private set; }
        public OrderStatus Status { get; set; }
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        private Order() { }

        public Order(Guid customerId, ShippingAddress shippingAddress)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
            ShippingAddress = shippingAddress;
            CreatedAt = DateTime.UtcNow;
            Status = OrderStatus.Pending;
        }

        public void AddItem(Guid productId, int quantity, decimal unitPrice)
        {
            if (Status != OrderStatus.Pending)
                throw new InvalidOperationException("Items can only be added to a pending order.");

            if (_items.Any(x => x.ProductId == productId))
                throw new InvalidOperationException("The product already exists in the order.");

            var item = new OrderItem(productId, quantity, unitPrice);

            _items.Add(item);
        }

        public decimal GetTotal()
        {
            return _items.Sum(x => x.GetTotal());
        }

        public void Confirm()
        {
            EnsureStatus(OrderStatus.Pending);

            if (_items.Count == 0)
                throw new InvalidOperationException("An order must contain at least one item.");
            
            Status = OrderStatus.Confirmed;

            RaiseDomainEvent(new OrderConfirmedEvent(Id, CustomerId, DateTime.UtcNow));
        }

        public void StartProcessing()
        {
            EnsureStatus(OrderStatus.Confirmed);

            Status = OrderStatus.Processing;
        }

        public void Ship()
        {
            EnsureStatus(OrderStatus.Processing);

            Status = OrderStatus.Shipped;
        }

        public void Deliver()
        {
            EnsureStatus(OrderStatus.Shipped);

            Status = OrderStatus.Delivered;
        }

        public void Cancel()
        {
            if (Status is not (OrderStatus.Pending or OrderStatus.Confirmed or OrderStatus.Processing))
                throw new InvalidOperationException($"Order cannot be cancelled when it is {Status}.");

            Status = OrderStatus.Cancelled;
        }

        private void EnsureStatus(OrderStatus expectedStatus)
        {
            if(Status  != expectedStatus)
                throw new InvalidOperationException($"Prder muste be {expectedStatus}.");
        }
    }
}
