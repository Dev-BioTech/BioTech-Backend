using HealthService.Domain.Entities;

namespace HealthService.Application.Interfaces;

public interface IHealthEventRepository
{
    Task<HealthEvent> AddAsync(HealthEvent healthEvent, CancellationToken cancellationToken);
    Task<HealthEvent?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task UpdateAsync(HealthEvent healthEvent, CancellationToken cancellationToken);
    
    Task<List<HealthEvent>> GetByFarmIdAsync(int farmId, int page, int pageSize, CancellationToken cancellationToken);
    Task<List<HealthEvent>> GetByAnimalIdAsync(long animalId, int page, int pageSize, CancellationToken cancellationToken);
    Task<List<HealthEvent>> GetByBatchIdAsync(int batchId, int page, int pageSize, CancellationToken cancellationToken);
    Task<List<HealthEvent>> GetByTypeAsync(string eventType, int farmId, int page, int pageSize, CancellationToken cancellationToken);
    
    // Dashboard and statistics methods
    Task<int> GetTotalEventsCountAsync(int farmId, CancellationToken ct = default);
    Task<decimal> GetTotalCostAsync(int farmId, CancellationToken ct = default);
    Task<int> GetSickAnimalsCountAsync(int farmId, CancellationToken ct = default);
    Task<IEnumerable<HealthEvent>> GetUpcomingEventsAsync(int farmId, int limit, CancellationToken ct = default);
    Task<IEnumerable<HealthEvent>> GetUpcomingEventsAsync(int userId, DateOnly fromDate, int limit, CancellationToken cancellationToken);
    Task<IEnumerable<HealthEvent>> GetRecentTreatmentsAsync(int farmId, int limit, CancellationToken ct = default);
}
