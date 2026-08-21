using OrderFlow.Domain.Common;

namespace OrderFlow.Domain.Events
{
    public sealed record OrderConfirmedEvent(Guid OrderId, Guid CustomerId, DateTime CreatedOn) : IDomainEvent;
}