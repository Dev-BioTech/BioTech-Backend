using HerdService.Application.Interfaces;
using HerdService.Domain.Entities;
using HerdService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HerdService.Infrastructure.Repositories;

public class AnimalRepository : IAnimalRepository
{
    private readonly HerdDbContext _context;

    public AnimalRepository(HerdDbContext context)
    {
        _context = context;
    }

    public async Task<Animal?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        return await _context.Animals
            .Include(a => a.Breed)
            .Include(a => a.Category)
            .Include(a => a.Batch)
            .Include(a => a.Paddock)
            .FirstOrDefaultAsync(a => a.Id == id, ct);
    }

    public async Task<Animal?> GetByVisualCodeAsync(string visualCode, int farmId, CancellationToken ct = default)
    {
        return await _context.Animals
            .Include(a => a.Breed)
            .Include(a => a.Category)
            .FirstOrDefaultAsync(a => a.VisualCode == visualCode && a.FarmId == farmId, ct);
    }

    public async Task<IEnumerable<Animal>> GetByFarmIdAsync(int farmId, string? status, bool includeInactive, CancellationToken ct = default)
    {
        var query = _context.Animals.AsNoTracking().Where(a => a.FarmId == farmId);

        if (!includeInactive)
            query = query.Where(a => a.CurrentStatus == "ACTIVE");

        if (!string.IsNullOrEmpty(status))
        {
            var upperStatus = status.ToUpperInvariant();
            query = query.Where(a => a.CurrentStatus == upperStatus);
        }

        return await query
            .Include(a => a.Breed)
            .Include(a => a.Category)
            .Include(a => a.Batch)
            .Include(a => a.Paddock)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<Animal>> GetByBatchIdAsync(int batchId, CancellationToken ct = default)
    {
        return await _context.Animals.AsNoTracking()
            .Where(a => a.BatchId == batchId && a.CurrentStatus == "ACTIVE")
            .Include(a => a.Breed)
            .Include(a => a.Category)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<Animal>> GetByPaddockIdAsync(int paddockId, CancellationToken ct = default)
    {
        return await _context.Animals.AsNoTracking()
            .Where(a => a.PaddockId == paddockId && a.CurrentStatus == "ACTIVE")
            .Include(a => a.Breed)
            .Include(a => a.Category)
            .ToListAsync(ct);
    }

    public async Task<Animal> AddAsync(Animal animal, CancellationToken ct = default)
    {
        var entry = await _context.Animals.AddAsync(animal, ct);
        await _context.SaveChangesAsync(ct);
        return entry.Entity;
    }

    public async Task UpdateAsync(Animal animal, CancellationToken ct = default)
    {
        _context.Animals.Update(animal);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Animal animal, CancellationToken ct = default)
    {
        _context.Animals.Remove(animal);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<bool> VisualCodeExistsAsync(string visualCode, int farmId, CancellationToken ct = default)
    {
        return await _context.Animals.AnyAsync(a => a.VisualCode == visualCode && a.FarmId == farmId, ct);
    }
}
