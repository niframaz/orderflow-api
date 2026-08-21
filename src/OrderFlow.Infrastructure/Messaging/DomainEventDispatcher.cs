using MediatR;
using OrderFlow.Application.Abstractions.Messaging;
using OrderFlow.Application.Orders.Events.OrderConfirmed;
using OrderFlow.Domain.Common;
using OrderFlow.Domain.Events;

namespace OrderFlow.Infrastructure.Messaging
{
    public sealed class DomainEventDispatcher(IPublisher publisher) : IDomainEventDispatcher
    {
        public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken)
        {
            foreach (var domainEvent in domainEvents)
            {
                switch (domainEvent)
                {
                    case OrderConfirmedEvent e:
                        await publisher.Publish(new OrderConfirmedNotification(e.OrderId, e.CustomerId), cancellationToken);
                        break;
                }
                await publisher.Publish(domainEvent, cancellationToken);
            }
        }
    }
}
