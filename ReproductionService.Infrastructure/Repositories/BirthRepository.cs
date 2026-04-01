using Microsoft.EntityFrameworkCore;
using ReproductionService.Application.Interfaces;
using ReproductionService.Domain.Entities;
using ReproductionService.Infrastructure.Persistence;

namespace ReproductionService.Infrastructure.Repositories;

public class BirthRepository : IBirthRepository
{
    private readonly ReproductionDbContext _context;

    public BirthRepository(ReproductionDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Birth>> GetByFarmIdAsync(int farmId, CancellationToken cancellationToken)
    {
        return await _context.Births
            .Where(b => b.FarmId == farmId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Birth> AddAsync(Birth birth, CancellationToken cancellationToken)
    {
        await _context.Births.AddAsync(birth, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return birth;
    }
}
