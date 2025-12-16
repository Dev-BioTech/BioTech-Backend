using HerdService.Application.Interfaces;
using HerdService.Domain.Entities;
using HerdService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HerdService.Infrastructure.Repositories;

public class PaddockRepository : IPaddockRepository
{
    private readonly HerdDbContext _context;

    public PaddockRepository(HerdDbContext context)
    {
        _context = context;
    }

    public async Task<Paddock> AddAsync(Paddock paddock, CancellationToken cancellationToken)
    {
        await _context.Paddocks.AddAsync(paddock, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return paddock;
    }

    public async Task<IEnumerable<Paddock>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Paddocks.ToListAsync(cancellationToken);
    }

    public async Task<Paddock?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Paddocks.FindAsync(new object[] { id }, cancellationToken);
    }
}
