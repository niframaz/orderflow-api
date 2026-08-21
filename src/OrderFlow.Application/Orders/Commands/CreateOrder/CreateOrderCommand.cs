using MediatR;

namespace OrderFlow.Application.Orders.Commands.CreateOrder
{
    public sealed record CreateOrderItem(Guid ProductId, int Quantity);
    public sealed record CreateOrderCommand(Guid CustomerId, string Street, string City, string PostalCode, string Country, IReadOnlyCollection<CreateOrderItem> Items) : IRequest<Guid>;
}
