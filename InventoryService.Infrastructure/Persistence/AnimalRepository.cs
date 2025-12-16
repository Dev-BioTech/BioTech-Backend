using InventoryService.Domain.Entities;
using InventoryService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryService.Infrastructure.Persistence;

public class AnimalRepository : IAnimalRepository
{
    private readonly InventoryDbContext _context;

    public AnimalRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<Animal?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await _context.Animals
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<Animal?> GetByVisualCodeAsync(string visualCode, int farmId, CancellationToken cancellationToken)
    {
        return await _context.Animals
            .FirstOrDefaultAsync(a => a.VisualCode == visualCode && a.FarmId == farmId, cancellationToken);
    }

    public async Task<IEnumerable<Animal>> GetAllAsync(int farmId, CancellationToken cancellationToken)
    {
        return await _context.Animals
            .Where(a => a.FarmId == farmId)
            .ToListAsync(cancellationToken);
    }

    public async Task<long> AddAsync(Animal animal, CancellationToken cancellationToken)
    {
        await _context.Animals.AddAsync(animal, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return animal.Id;
    }

    public async Task UpdateAsync(Animal animal, CancellationToken cancellationToken)
    {
        _context.Animals.Update(animal);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(string visualCode, int farmId, CancellationToken cancellationToken)
    {
        return await _context.Animals
            .AnyAsync(a => a.VisualCode == visualCode && a.FarmId == farmId, cancellationToken);
    }
}
