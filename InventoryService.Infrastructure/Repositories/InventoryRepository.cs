using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InventoryService.Application.Interfaces;
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

    public async Task<InventoryItem> GetByIdAsync(int id)
    {
        return await _context.InventoryItems.FindAsync(id);
    }

    public async Task<IEnumerable<InventoryItem>> GetByFarmIdAsync(int farmId, int page, int pageSize)
    {
        return await _context.InventoryItems
            .AsNoTracking()
            .Where(i => i.FarmId == farmId)
            .OrderByDescending(i => i.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task AddAsync(InventoryItem item)
    {
        await _context.InventoryItems.AddAsync(item);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(InventoryItem item)
    {
        _context.InventoryItems.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(InventoryItem item)
    {
        _context.InventoryItems.Remove(item);
        await _context.SaveChangesAsync();
    }
}
