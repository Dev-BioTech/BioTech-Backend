using Microsoft.EntityFrameworkCore;
using ReproductionService.Application.Interfaces;
using ReproductionService.Domain.Entities;
using ReproductionService.Infrastructure.Persistence;

namespace ReproductionService.Infrastructure.Repositories;

public class ReproductionEventRepository : IReproductionEventRepository
{
    private readonly ReproductionDbContext _context;

    public ReproductionEventRepository(ReproductionDbContext context)
    {
        _context = context;
    }

    public async Task<ReproductionEvent> AddAsync(ReproductionEvent reproductionEvent, CancellationToken cancellationToken)
    {
        await _context.ReproductionEvents.AddAsync(reproductionEvent, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return reproductionEvent;
    }

    public async Task<ReproductionEvent?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await _context.ReproductionEvents
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
}
