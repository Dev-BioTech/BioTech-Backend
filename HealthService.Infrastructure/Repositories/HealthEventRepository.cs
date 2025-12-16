using HealthService.Application.Interfaces;
using HealthService.Domain.Entities;
using HealthService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HealthService.Infrastructure.Repositories;

public class HealthEventRepository : IHealthEventRepository
{
    private readonly HealthServiceDbContext _context;

    public HealthEventRepository(HealthServiceDbContext context)
    {
        _context = context;
    }

    public async Task<HealthEvent> AddAsync(HealthEvent healthEvent, CancellationToken cancellationToken)
    {
        await _context.HealthEvents.AddAsync(healthEvent, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return healthEvent;
    }

    public async Task<HealthEvent?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await _context.HealthEvents
            .Include(he => he.Details)
            .FirstOrDefaultAsync(he => he.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<HealthEvent>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.HealthEvents
            .Include(he => he.Details)
            .ToListAsync(cancellationToken);
    }
}
