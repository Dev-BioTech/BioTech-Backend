using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InventoryService.Domain.Entities;

namespace InventoryService.Domain.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<IEnumerable<Product>> GetAllAsync(int farmId, CancellationToken cancellationToken);
    Task<IEnumerable<Product>> GetLowStockAsync(int farmId, CancellationToken cancellationToken);
    Task<long> AddAsync(Product product, CancellationToken cancellationToken);
    Task UpdateAsync(Product product, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(string name, int farmId, CancellationToken cancellationToken);
}
