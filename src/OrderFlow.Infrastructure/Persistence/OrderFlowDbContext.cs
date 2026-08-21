using Microsoft.EntityFrameworkCore;
using OrderFlow.Application.Abstractions.Messaging;
using OrderFlow.Application.Abstractions.Persistence;
using OrderFlow.Domain.Common;
using OrderFlow.Domain.Entities;
using OrderFlow.Infrastructure.Messaging;

namespace OrderFlow.Infrastructure.Persistence
{
    public class OrderFlowDbContext(DbContextOptions<OrderFlowDbContext> options, IDomainEventDispatcher domainEventDispatcher) : DbContext(options), IUnitOfWork
    {
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Product> Products => Set<Product>();

        //public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{
        //    var domainEvents = ChangeTracker
        //        .Entries<Entity>()
        //        .SelectMany(x => x.Entity.DomainEvents)
        //        .ToList();

        //    var result = await base.SaveChangesAsync(
        //        cancellationToken);

        //    foreach (var entity in ChangeTracker
        //        .Entries<Entity>()
        //        .Select(x => x.Entity))
        //    {
        //        entity.ClearDomainEvents();
        //    }

        //    await domainEventDispatcher.DispatchAsync(
        //        domainEvents,
        //        cancellationToken);

        //    return result;
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderFlowDbContext).Assembly);
        }
    }
}
