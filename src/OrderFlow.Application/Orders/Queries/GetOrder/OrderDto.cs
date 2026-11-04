namespace OrderFlow.Application.Orders.Queries.GetOrder
{
    public sealed record OrderItemDto(Guid ProductId, int Quantity, decimal UnitPrice, decimal Total);
    public sealed record OrderDto(Guid Id, Guid CustomerId, DateTime CreatedAt, string Status, decimal Total, IReadOnlyCollection<OrderItemDto> Items);
}
