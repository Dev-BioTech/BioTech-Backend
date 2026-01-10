using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InventoryService.Domain.Interfaces;
using InventoryService.Domain.Entities;
using InventoryService.Infrastructure.Persistence;

namespace InventoryService.Infrastructure.Repositories;

public class InventoryRepository : IInventoryRepository
{
    private readonly InventoryDbContext _context;

    public InventoryRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<InventoryItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.InventoryItems.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<InventoryItem>> GetByFarmIdAsync(int farmId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.InventoryItems
            .AsNoTracking()
            .Where(i => i.FarmId == farmId)
            .OrderByDescending(i => i.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(InventoryItem item, CancellationToken cancellationToken = default)
    {
        await _context.InventoryItems.AddAsync(item, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(InventoryItem item, CancellationToken cancellationToken = default)
    {
        _context.InventoryItems.Update(item);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(InventoryItem item, CancellationToken cancellationToken = default)
    {
        _context.InventoryItems.Remove(item);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddMovementAsync(InventoryMovement movement, CancellationToken cancellationToken = default)
    {
        await _context.InventoryMovements.AddAsync(movement, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<InventoryMovement>> GetMovementsByProductIdAsync(int productId, CancellationToken cancellationToken = default)
    {
        return await _context.InventoryMovements
            .AsNoTracking()
            .Where(m => m.ProductId == productId)
            .OrderByDescending(m => m.MovementDate)
            .ToListAsync(cancellationToken);
    }
}
