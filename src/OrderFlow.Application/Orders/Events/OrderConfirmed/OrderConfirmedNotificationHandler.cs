using MediatR;

namespace OrderFlow.Application.Orders.Events.OrderConfirmed
{
    public sealed class OrderConfirmedNotificationHandler : INotificationHandler<OrderConfirmedNotification>
    {
        public async Task Handle(OrderConfirmedNotification notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Order {notification.OrderId} was confirmed.");

            await Task.CompletedTask;
        }
    }
}
