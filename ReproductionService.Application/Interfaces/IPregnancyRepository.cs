using ReproductionService.Domain.Entities;

namespace ReproductionService.Application.Interfaces;

public interface IPregnancyRepository
{
    Task<IEnumerable<Pregnancy>> GetByFarmIdAsync(int farmId, CancellationToken cancellationToken);
    Task<Pregnancy> AddAsync(Pregnancy pregnancy, CancellationToken cancellationToken);
}
