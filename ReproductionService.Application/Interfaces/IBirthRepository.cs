using ReproductionService.Domain.Entities;

namespace ReproductionService.Application.Interfaces;

public interface IBirthRepository
{
    Task<IEnumerable<Birth>> GetByFarmIdAsync(int farmId, CancellationToken cancellationToken);
    Task<Birth> AddAsync(Birth birth, CancellationToken cancellationToken);
}
