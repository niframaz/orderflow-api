using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Enums;
using OrderFlow.Domain.Events;
using OrderFlow.Domain.ValueObjects;

namespace OrderFlow.Domain.Tests.Entities
{
    public class OrderTests
    {
        [Fact]
        public void NewOrder_ShouldStartAsPending()
        {
            // Arrange
            var customerId = Guid.NewGuid();

            var address = new ShippingAddress("123 Main Street", "Colombo", "00100", "Sri Lanka");

            // Act
            var order = new Order(customerId, address);

            // Assert
            Assert.Equal(OrderStatus.Pending, order.Status);
        }

        [Fact]
        public void AddItem_ShouldAddProduct()
        {
            // Arrange
            var order = CreatePendingOrder();

            var productId = Guid.NewGuid();

            var price = new Money(1000m, "LKR");

            // Act
            order.AddItem(productId, 2, price);

            // Assert
            Assert.Single(order.Items);

            var item = order.Items.Single();

            Assert.Equal(productId, item.ProductId);
            Assert.Equal(2, item.Quantity);
            Assert.Equal(price, item.UnitPrice);
        }

        [Fact]
        public void AddItem_WhenProductAlreadyExists_ShouldThrow()
        {
            // Arrange
            var order = CreatePendingOrder();

            var productId = Guid.NewGuid();

            order.AddItem(productId, 1, new Money(1000m, "LKR"));

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => order.AddItem(productId, 2, new Money(1000m, "LKR")));
        }

        [Fact]
        public void Confirm_WhenOrderHasNoItems_ShouldThrow()
        {
            // Arrange
            var order = CreatePendingOrder();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(order.Confirm);
        }

        [Fact]
        public void Confirm_ShouldRaiseOrderConfirmedEvent()
        {
            // Arrange
            var order = CreatePendingOrder();

            order.AddItem(Guid.NewGuid(), 1, new Money(1000m, "LKR"));

            // Act
            order.Confirm();

            // Assert
            var domainEvent = Assert.Single(order.DomainEvents);

            var confirmedEvent = Assert.IsType<OrderConfirmedEvent>(domainEvent);

            Assert.Equal( order.Id, confirmedEvent.OrderId);

            Assert.Equal(order.CustomerId, confirmedEvent.CustomerId);
        }

        [Fact]
        public void AddItem_WhenQuantityIsZero_ShouldThrow()
        {
            // Arrange
            var order = CreatePendingOrder();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => order.AddItem(Guid.NewGuid(), 0, new Money(1000m, "LKR")));
        }

        [Fact]
        public void AddItem_WhenQuantityIsNegative_ShouldThrow()
        {
            // Arrange
            var order = CreatePendingOrder();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => order.AddItem(Guid.NewGuid(), -1, new Money(1000m, "LKR")));
        }

        [Fact]
        public void AddItem_WhenOrderIsConfirmed_ShouldThrow()
        {
            // Arrange
            var order = CreatePendingOrder();

            order.AddItem(Guid.NewGuid(), 1, new Money(1000m, "LKR"));

            order.Confirm();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => order.AddItem(Guid.NewGuid(), 1, new Money(500m, "LKR")));
        }

        [Fact]
        public void Confirm_WhenAlreadyConfirmed_ShouldThrow()
        {
            // Arrange
            var order = CreatePendingOrder();

            order.AddItem(Guid.NewGuid(), 1, new Money(1000m, "LKR"));

            order.Confirm();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(order.Confirm);
        }

        private static Order CreatePendingOrder()
        {
            return new Order(Guid.NewGuid(), new ShippingAddress("123 Main Street", "Colombo", "00100", "Sri Lanka"));
        }
    }
}
