using MediatR;

namespace OrderFlow.Application.Orders.Events.OrderConfirmed
{
    public sealed record OrderConfirmedNotification(Guid OrderId, Guid CustomerId) : INotification;
}
