using HerdService.Domain.Entities;

namespace HerdService.Application.Interfaces;

public interface IBreedRepository
{
    Task<Breed> AddAsync(Breed breed, CancellationToken cancellationToken);
    Task<IEnumerable<Breed>> GetAllAsync(CancellationToken cancellationToken);
    Task<Breed?> GetByIdAsync(int id, CancellationToken cancellationToken);
}
