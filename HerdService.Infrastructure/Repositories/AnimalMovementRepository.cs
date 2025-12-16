using HerdService.Application.Interfaces;
using HerdService.Domain.Entities;
using HerdService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HerdService.Infrastructure.Repositories;

public class AnimalMovementRepository : IAnimalMovementRepository
{
    private readonly HerdDbContext _context;

    public AnimalMovementRepository(HerdDbContext context)
    {
        _context = context;
    }

    public async Task<AnimalMovement> AddAsync(AnimalMovement movement, CancellationToken cancellationToken)
    {
        await _context.AnimalMovements.AddAsync(movement, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return movement;
    }

    public async Task<IEnumerable<AnimalMovement>> GetByAnimalIdAsync(long animalId, CancellationToken cancellationToken)
    {
        return await _context.AnimalMovements
            .Where(m => m.AnimalId == animalId)
            .Include(m => m.MovementType)
            .OrderByDescending(m => m.MovementDate)
            .ToListAsync(cancellationToken);
    }
}
