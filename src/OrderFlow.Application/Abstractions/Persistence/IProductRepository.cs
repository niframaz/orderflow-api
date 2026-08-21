using OrderFlow.Domain.Entities;

namespace OrderFlow.Application.Abstractions.Persistence
{
    public interface IProductRepository
    {
        Task<IReadOnlyDictionary<Guid, Product>> GetByIdsAsync(IReadOnlyCollection<Guid> productIds, CancellationToken cancellationToken);
    }
}
