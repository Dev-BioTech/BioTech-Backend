using HerdService.Domain.Entities;

namespace HerdService.Application.Interfaces;

public interface IAnimalRepository
{
    Task<Animal> AddAsync(Animal animal, CancellationToken cancellationToken);
}
