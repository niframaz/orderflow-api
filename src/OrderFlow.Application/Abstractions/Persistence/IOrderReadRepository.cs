using OrderFlow.Application.Orders.Queries.GetOrder;

namespace OrderFlow.Application.Abstractions.Persistence
{
    public interface IOrderReadRepository
    {
        Task<OrderDto?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken);
    }
}
