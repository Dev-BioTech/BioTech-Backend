using HealthService.Domain.Entities;

namespace HealthService.Application.Interfaces;

public interface IHealthEventRepository
{
    Task<HealthEvent> AddAsync(HealthEvent healthEvent, CancellationToken cancellationToken);
    Task<HealthEvent?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<IEnumerable<HealthEvent>> GetAllAsync(CancellationToken cancellationToken);
    
    Task<List<HealthEvent>> GetByFarmIdAsync(int farmId, int page, int pageSize, CancellationToken cancellationToken);
    Task<List<HealthEvent>> GetByAnimalIdAsync(long animalId, int page, int pageSize, CancellationToken cancellationToken);
    Task<List<HealthEvent>> GetByBatchIdAsync(int batchId, int page, int pageSize, CancellationToken cancellationToken);
    Task<List<HealthEvent>> GetByTypeAsync(string eventType, int farmId, int page, int pageSize, CancellationToken cancellationToken);
}
