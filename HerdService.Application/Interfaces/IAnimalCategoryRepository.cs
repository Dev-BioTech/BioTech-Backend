using HerdService.Domain.Entities;

namespace HerdService.Application.Interfaces;

public interface IAnimalCategoryRepository
{
    Task<AnimalCategory> AddAsync(AnimalCategory category, CancellationToken cancellationToken);
    Task<IEnumerable<AnimalCategory>> GetAllAsync(CancellationToken cancellationToken);
    Task<AnimalCategory?> GetByIdAsync(int id, CancellationToken cancellationToken);
}
