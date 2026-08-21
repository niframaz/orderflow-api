using Microsoft.EntityFrameworkCore;
using OrderFlow.Application.Abstractions.Persistence;
using OrderFlow.Domain.Entities;
using OrderFlow.Infrastructure.Persistence;

namespace OrderFlow.Infrastructure.Repositories
{
    public class ProductRepository(OrderFlowDbContext context) : IProductRepository
    {
        public async Task<IReadOnlyDictionary<Guid, Product>> GetByIdsAsync(IReadOnlyCollection<Guid> productIds, CancellationToken cancellationToken)
        {
            return await context.Products.Where(x => productIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, cancellationToken);
        }
    }
}
