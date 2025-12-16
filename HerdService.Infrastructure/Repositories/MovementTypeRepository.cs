using HerdService.Application.Interfaces;
using HerdService.Domain.Entities;
using HerdService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HerdService.Infrastructure.Repositories;

public class MovementTypeRepository : IMovementTypeRepository
{
    private readonly HerdDbContext _context;

    public MovementTypeRepository(HerdDbContext context)
    {
        _context = context;
    }

    public async Task<MovementType> AddAsync(MovementType movementType, CancellationToken cancellationToken)
    {
        await _context.MovementTypes.AddAsync(movementType, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return movementType;
    }

    public async Task<IEnumerable<MovementType>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.MovementTypes.ToListAsync(cancellationToken);
    }

    public async Task<MovementType?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.MovementTypes.FindAsync(new object[] { id }, cancellationToken);
    }
}
