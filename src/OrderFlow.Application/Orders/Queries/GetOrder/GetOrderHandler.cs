using MediatR;
using OrderFlow.Application.Abstractions.Persistence;

namespace OrderFlow.Application.Orders.Queries.GetOrder
{
    public sealed class GetOrderHandler(IOrderRepository orderRepository) : IRequestHandler<GetOrderQuery, OrderDto?>
    {
        public async Task<OrderDto?> Handle(GetOrderQuery query, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetByIdAsync(query.OrderId, cancellationToken);

            if (order is null)
                return null;

            return new OrderDto(order.Id, order.CustomerId, order.CreatedAt, order.Status.ToString(), order.GetTotal(), 
                [.. order.Items.Select(item => new OrderItemDto(item.ProductId, item.Quantity, item.UnitPrice.Amount, item.GetTotal()))]);
        }
    }
}
