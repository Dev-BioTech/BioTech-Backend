using HerdService.Application.Interfaces;
using HerdService.Domain.Entities;
using HerdService.Infrastructure.Persistence;

namespace HerdService.Infrastructure.Repositories;

public class AnimalRepository : IAnimalRepository
{
    private readonly HerdDbContext _context;

    public AnimalRepository(HerdDbContext context)
    {
        _context = context;
    }

    public async Task<Animal> AddAsync(Animal animal, CancellationToken cancellationToken)
    {
        await _context.Animals.AddAsync(animal, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return animal;
    }
}
