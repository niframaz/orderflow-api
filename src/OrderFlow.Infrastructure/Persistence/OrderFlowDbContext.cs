using Microsoft.EntityFrameworkCore;
using OrderFlow.Application.Abstractions.Messaging;
using OrderFlow.Application.Abstractions.Persistence;
using OrderFlow.Domain.Common;
using OrderFlow.Domain.Entities;
using OrderFlow.Infrastructure.Persistence.Outbox;
using System.Text.Json;

namespace OrderFlow.Infrastructure.Persistence
{
    public class OrderFlowDbContext(DbContextOptions<OrderFlowDbContext> options, IDomainEventDispatcher domainEventDispatcher) : DbContext(options), IUnitOfWork
    {
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Product> Products => Set<Product>();

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entities = ChangeTracker.Entries<Entity>().Select(x => x.Entity).ToList();

            var domainEvents = entities.SelectMany(x => x.DomainEvents).ToList();

            var outboxMessages = domainEvents
                .Select(domainEvent => new OutboxMessage(
                    Guid.NewGuid(), 
                    domainEvent.GetType().Name, 
                    JsonSerializer.Serialize(domainEvent, domainEvent.GetType()),
                    domainEvent.CreatedOn))
                .ToList();

            await Set<OutboxMessage>().AddRangeAsync(outboxMessages, cancellationToken);

            var result = await base.SaveChangesAsync(cancellationToken);

            foreach (var entity in entities)
            {
                entity.ClearDomainEvents();
            }

            return result;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderFlowDbContext).Assembly);
        }
    }
}
