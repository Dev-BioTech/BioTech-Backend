using ReproductionService.Domain.Entities;
using ReproductionService.Domain.Enums;

namespace ReproductionService.Application.Interfaces;

public interface IReproductionEventRepository
{
    Task<ReproductionEvent> AddAsync(ReproductionEvent reproductionEvent, CancellationToken ct = default);
    Task<ReproductionEvent?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<ReproductionEvent>> GetByAnimalIdAsync(long animalId, CancellationToken ct = default);
    Task<IEnumerable<ReproductionEvent>> GetByFarmIdAsync(int farmId, DateTime? from, DateTime? to, CancellationToken ct = default);
    Task<IEnumerable<ReproductionEvent>> GetByTypeAsync(ReproductionEventType type, int farmId, CancellationToken ct = default);
    Task UpdateAsync(ReproductionEvent reproductionEvent, CancellationToken ct = default);
}
