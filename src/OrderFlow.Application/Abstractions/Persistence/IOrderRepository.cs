using OrderFlow.Domain.Entities;

namespace OrderFlow.Application.Abstractions.Persistence
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken);
        Task AddAsync(Order order, CancellationToken cancellationToken);
        Task SaveAsync(Order order, CancellationToken cancellationToken);
    }
}
