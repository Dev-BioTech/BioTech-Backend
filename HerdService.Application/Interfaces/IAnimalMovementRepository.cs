using HerdService.Domain.Entities;

namespace HerdService.Application.Interfaces;

public interface IAnimalMovementRepository
{
    Task<AnimalMovement> AddAsync(AnimalMovement movement, CancellationToken cancellationToken);
    Task<IEnumerable<AnimalMovement>> GetByAnimalIdAsync(long animalId, CancellationToken cancellationToken);
}
