using HerdService.Application.Interfaces;
using HerdService.Domain.Entities;
using HerdService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HerdService.Infrastructure.Repositories;

public class BatchRepository : IBatchRepository
{
    private readonly HerdDbContext _context;

    public BatchRepository(HerdDbContext context)
    {
        _context = context;
    }

    public async Task<Batch> AddAsync(Batch batch, CancellationToken cancellationToken)
    {
        await _context.Batches.AddAsync(batch, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return batch;
    }

    public async Task<IEnumerable<Batch>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Batches.ToListAsync(cancellationToken);
    }

    public async Task<Batch?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Batches.FindAsync(new object[] { id }, cancellationToken);
    }
}
