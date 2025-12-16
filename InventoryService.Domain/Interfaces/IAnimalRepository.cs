using InventoryService.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryService.Domain.Interfaces;

public interface IAnimalRepository
{
    Task<Animal?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<Animal?> GetByVisualCodeAsync(string visualCode, int farmId, CancellationToken cancellationToken);
    Task<IEnumerable<Animal>> GetAllAsync(int farmId, CancellationToken cancellationToken);
    Task<long> AddAsync(Animal animal, CancellationToken cancellationToken);
    Task UpdateAsync(Animal animal, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(string visualCode, int farmId, CancellationToken cancellationToken);
}
