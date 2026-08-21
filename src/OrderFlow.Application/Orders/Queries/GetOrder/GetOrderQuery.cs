using MediatR;

namespace OrderFlow.Application.Orders.Queries.GetOrder
{
    public sealed record GetOrderQuery(Guid OrderId) : IRequest<OrderDto?>;
}
