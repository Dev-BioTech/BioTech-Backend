using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;

namespace AuthService.Infrastructure.Persistence;

public class FarmRepository : IFarmRepository
{
    private readonly AuthDbContext _context;

    public FarmRepository(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<Farm?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Farms
            .Include(f => f.Tenant) // Eager load Tenant if needed
            .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Farm>> GetAllByTenantIdAsync(int tenantId, CancellationToken cancellationToken = default)
    {
        return await _context.Farms
            .Where(f => f.TenantId == tenantId)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Farm farm, CancellationToken cancellationToken = default)
    {
        await _context.Farms.AddAsync(farm, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Farm farm, CancellationToken cancellationToken = default)
    {
        _context.Farms.Update(farm);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Farm farm, CancellationToken cancellationToken = default)
    {
        _context.Farms.Remove(farm);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Farms.AnyAsync(f => f.Id == id, cancellationToken);
    }
}
