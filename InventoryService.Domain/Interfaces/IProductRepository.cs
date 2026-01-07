using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InventoryService.Domain.Entities;

namespace InventoryService.Domain.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetAllAsync(int farmId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetLowStockAsync(int farmId, CancellationToken cancellationToken = default);
    Task<int> AddAsync(Product product, CancellationToken cancellationToken = default);
    Task UpdateAsync(Product product, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string name, int farmId, CancellationToken cancellationToken = default);
}
