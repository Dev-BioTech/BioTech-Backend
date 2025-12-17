using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Repositories;

public class FarmRepository : IFarmRepository
{
    private readonly AuthDbContext _context;

    public FarmRepository(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<Farm?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Farms
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == id, ct);
    }

    public async Task<IEnumerable<Farm>> GetByTenantUserIdAsync(int tenantUserId, bool includeInactive, CancellationToken ct = default)
    {
        // New Logic: Farms are linked to users via UserFarmRole
        var query = _context.Farms.AsNoTracking()
            .Where(f => f.UserFarmRoles.Any(ufr => ufr.UserId == tenantUserId));

        if (!includeInactive)
        {
            query = query.Where(f => f.Active);
        }

        return await query
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<Farm> AddAsync(Farm farm, int? userId, CancellationToken ct = default)
    {
        await _context.Farms.AddAsync(farm, ct);
        await _context.SaveChangesAsync(ct);
        
        if (userId.HasValue)
        {
            // Ensure OWNER role exists, create if not present
            var role = await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == "Owner", ct);
            
            if (role == null)
            {
                role = new Role { Name = "Owner", Description = "Farm Owner with full access" };
                await _context.Roles.AddAsync(role, ct);
                await _context.SaveChangesAsync(ct);
            }
            
            var userFarmRole = new UserFarmRole
            {
                UserId = userId.Value,
                FarmId = farm.Id,
                RoleId = role.Id
            };
            
            await _context.UserFarmRoles.AddAsync(userFarmRole, ct);
            await _context.SaveChangesAsync(ct);
        }
        
        return farm;
    }

    public async Task UpdateAsync(Farm farm, CancellationToken ct = default)
    {
        _context.Farms.Update(farm);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        // For physical delete, though prompt implies soft delete via Deactivate.
        // If the interface implies deletion, we usually remove.
        // However, requirements say "Deactivate(), Activate()" methods exist.
        // Prompt creates DELETE /api/v1/farms/{id} ? No, it doesn't specify a DELETE endpoint.
        // I'll implement generic DeleteAsync as physical delete just in case, but application logic might use UpdateAsync.
        
        var farm = await _context.Farms.FindAsync(new object[] { id }, ct);
        if (farm != null)
        {
            _context.Farms.Remove(farm);
            await _context.SaveChangesAsync(ct);
        }
    }
}
