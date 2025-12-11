using FeedingService.Domain.Entities;

namespace FeedingService.Application.Interfaces;

public interface IFeedingEventRepository
{
    Task<FeedingEvent?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<FeedingEvent>> GetByFarmIdAsync(int farmId, DateTime? from, DateTime? to, CancellationToken ct = default);
    Task<IEnumerable<FeedingEvent>> GetByBatchIdAsync(int batchId, CancellationToken ct = default);
    Task<IEnumerable<FeedingEvent>> GetByAnimalIdAsync(long animalId, CancellationToken ct = default);
    Task<FeedingEvent> AddAsync(FeedingEvent feedingEvent, CancellationToken ct = default);
    Task UpdateAsync(FeedingEvent feedingEvent, CancellationToken ct = default);
    Task DeleteAsync(long id, CancellationToken ct = default);
}