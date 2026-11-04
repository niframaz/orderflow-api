using Microsoft.EntityFrameworkCore;
using OrderFlow.Application.Abstractions.Persistence;
using OrderFlow.Application.Orders.Queries.GetOrder;
using OrderFlow.Infrastructure.Persistence;

namespace OrderFlow.Infrastructure.Repositories
{
    public class OrderReadRepository(OrderFlowDbContext context) : IOrderReadRepository
    {
        public async Task<OrderDto?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken)
        {
            return await context.Orders
                .AsNoTracking()
                .Where(order => order.Id == orderId)
                .Select(order => new OrderDto(
                    order.Id,
                    order.CustomerId,
                    order.CreatedAt,
                    order.Status.ToString(),
                    order.Items.Sum(item => item.Quantity * item.UnitPrice.Amount),
                    order.Items
                        .Select(item => new OrderItemDto(
                            item.ProductId,
                            item.Quantity,
                            item.UnitPrice.Amount,
                            item.Quantity * item.UnitPrice.Amount))
                        .ToList()))
                .SingleOrDefaultAsync(cancellationToken);
        }
    }
}
