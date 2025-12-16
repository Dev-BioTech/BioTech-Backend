using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InventoryService.Domain.Entities;

namespace InventoryService.Domain.Interfaces;

public interface IInventoryRepository
{
    Task AddMovementAsync(InventoryMovement movement, CancellationToken cancellationToken);
    Task<IEnumerable<InventoryMovement>> GetMovementsByProductIdAsync(int productId, CancellationToken cancellationToken);
}
