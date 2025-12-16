using HealthService.Domain.Entities;

namespace HealthService.Application.Interfaces;

public interface IHealthEventRepository
{
    Task<HealthEvent> AddAsync(HealthEvent healthEvent, CancellationToken cancellationToken);
    Task<HealthEvent?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<IEnumerable<HealthEvent>> GetAllAsync(CancellationToken cancellationToken);
    // Add other methods as needed (e.g., GetByAnimalId)
}
