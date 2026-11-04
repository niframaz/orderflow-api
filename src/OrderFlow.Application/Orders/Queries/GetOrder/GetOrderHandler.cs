using MediatR;
using OrderFlow.Application.Abstractions.Persistence;

namespace OrderFlow.Application.Orders.Queries.GetOrder
{
    public sealed class GetOrderHandler(IOrderReadRepository orderReadRepository) : IRequestHandler<GetOrderQuery, OrderDto?>
    {
        public async Task<OrderDto?> Handle(GetOrderQuery query, CancellationToken cancellationToken)
        {
            return await orderReadRepository.GetByIdAsync(query.OrderId, cancellationToken);
        }
    }
}
