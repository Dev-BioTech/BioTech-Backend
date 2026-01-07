using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InventoryService.Domain.Entities;

namespace InventoryService.Domain.Interfaces;

public interface IInventoryRepository
{
    // InventoryItem methods
    Task<InventoryItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<InventoryItem>> GetByFarmIdAsync(int farmId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task AddAsync(InventoryItem item, CancellationToken cancellationToken = default);
    Task UpdateAsync(InventoryItem item, CancellationToken cancellationToken = default);
    Task DeleteAsync(InventoryItem item, CancellationToken cancellationToken = default);

    // InventoryMovement methods
    Task AddMovementAsync(InventoryMovement movement, CancellationToken cancellationToken = default);
    Task<IEnumerable<InventoryMovement>> GetMovementsByProductIdAsync(int productId, CancellationToken cancellationToken = default);
}
