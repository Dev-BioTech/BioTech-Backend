using HealthService.Domain.Entities;

namespace HealthService.Application.Interfaces;

public interface IHealthEventRepository
{
    Task<HealthEvent?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<HealthEvent>> GetByFarmIdAsync(
        int farmId, 
        DateOnly? fromDate, 
        DateOnly? toDate,
        string? eventType,
        CancellationToken ct = default);
    Task<IEnumerable<HealthEvent>> GetByAnimalIdAsync(long animalId, string? eventType, CancellationToken ct = default);
    Task<IEnumerable<HealthEvent>> GetByBatchIdAsync(int batchId, string? eventType, CancellationToken ct = default);
    Task<IEnumerable<HealthEvent>> GetByEventTypeAsync(
        string eventType, 
        int farmId,
        DateOnly? fromDate, 
        DateOnly? toDate,
        CancellationToken ct = default);
    Task<HealthEvent> AddAsync(HealthEvent healthEvent, CancellationToken ct = default);
    Task UpdateAsync(HealthEvent healthEvent, CancellationToken ct = default);
    Task DeleteAsync(HealthEvent healthEvent, CancellationToken ct = default);
}
