using OrderFlow.Domain.Common;

namespace OrderFlow.Application.Abstractions.Messaging
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken);
    }
}
