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

    // Dashboard & Stats
    Task<int> GetTotalEventsCountAsync(int farmId, CancellationToken ct = default);
    Task<decimal> GetTotalCostAsync(int farmId, CancellationToken ct = default); // Total cost all time or maybe this month, let's do all time for now as a base stat
    Task<int> GetSickAnimalsCountAsync(int farmId, CancellationToken ct = default); // Unique animals with recent diseases/treatments? Or just distinct animals in events? Let's say distinct animals in events this month.
    
    // Better granular queries for the dashboard handlers
    Task<IEnumerable<HealthEvent>> GetUpcomingEventsAsync(int farmId, int limit, CancellationToken ct = default);
    Task<IEnumerable<HealthEvent>> GetRecentTreatmentsAsync(int farmId, int limit, CancellationToken ct = default);
}
