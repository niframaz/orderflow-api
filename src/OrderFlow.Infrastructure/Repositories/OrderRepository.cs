using Microsoft.EntityFrameworkCore;
using OrderFlow.Application.Abstractions.Persistence;
using OrderFlow.Domain.Entities;
using OrderFlow.Infrastructure.Persistence;

namespace OrderFlow.Infrastructure.Repositories
{
    public class OrderRepository(OrderFlowDbContext context) : IOrderRepository
    {
        public async Task AddAsync(Order order, CancellationToken cancellationToken)
        {
            await context.Orders.AddAsync(order, cancellationToken);
        }

        public async Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken)
        {
            return await context.Orders
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == orderId, cancellationToken);
        }

        public async Task SaveAsync(Order order, CancellationToken cancellationToken)
        {
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
